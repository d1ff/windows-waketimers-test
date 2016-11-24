using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WakeTimersTest
{
    public class TimersTester
    {
        public delegate void TimerCompleteDelegate();
        public static event TimerCompleteDelegate OnTimerCompleted;

        public delegate void TimerSetDelegate();
        public static event TimerSetDelegate OnTimerSet;

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset,
                          string lpTimerName);

        [DllImport("kernel32.dll")]
        public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref long pDueTime,
                                int lPeriod, TimerCompleteDelegate pfnCompletionRoutine,
                                IntPtr lpArgToCompletionRoutine, bool fResume);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(IntPtr handle, uint milliseconds);

        [DllImport("kernel32.dll")]
        static extern bool CancelWaitableTimer(IntPtr hTimer);

        public static uint INFINITE = 0xFFFFFFFF;

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        private static void TimerCompleted()
        {
            // Routine executed once the timer has expired. This is executed independently of the 
            // program calling this class implementation of the OnTimerCompleted Event
            //
            Console.WriteLine("Timer is complete in the class");
        }

        public static bool TimersSupported()
        {
            IntPtr timerHandle = CreateWaitableTimer(IntPtr.Zero, true, "Wait Timer 1");
            long interval = 0;
            int retVal = 0;
            if (timerHandle != IntPtr.Zero)
            {
                TimerCompleteDelegate TimerComplete = new TimerCompleteDelegate(TimerCompleted);
                SetWaitableTimer(timerHandle, ref interval, 0, TimerComplete, IntPtr.Zero, true);
                retVal = Marshal.GetLastWin32Error();
                CancelWaitableTimer(timerHandle);
                try
                {
                    CloseHandle(timerHandle);
                }
                catch (Exception exp)
                {

                }
                timerHandle = IntPtr.Zero;
            }

            //SUCCESS
            if (retVal == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void TimerIsSet()
        {
            Console.WriteLine("Timer is set in the class");
        }
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001;
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        private static void Wake()
        {
            mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
            Thread.Sleep(40);
            mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
            SendMessage(0xFFFF, 0x112, 0xF170, -1);
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        }

        private static void WaitTimer(IntPtr handle)
        {
            // Waiting for the timer to expire
            //
            if (WaitForSingleObject(handle, INFINITE) != 0)
            {
                Console.WriteLine("Last Error = " + Marshal.GetLastWin32Error().ToString());
            }
            else
            {
                Console.WriteLine("Timer expired @ " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                Wake();
            }

            // Closing the timer
            //
            CloseHandle(handle);

            // Raising event Timer Completed
            //
            if (OnTimerCompleted != null)
            {
                OnTimerCompleted();
            }
        }

        public static bool CreateTimer()
        {
            DateTime utc = DateTime.Now.AddMinutes(2);
            long Interval = utc.ToFileTime();
            TimerCompleteDelegate TimerComplete = new TimerCompleteDelegate(TimerCompleted);
            TimerSetDelegate TimerSet = new TimerSetDelegate(TimerIsSet);

            // Creating the timer
            //
            Console.WriteLine("Creating WaitableTimer");
            IntPtr handle = CreateWaitableTimer(IntPtr.Zero, true, "WaitableTimer");
            int retVal = Marshal.GetLastWin32Error();
            Console.WriteLine("Last Error = " + retVal.ToString());
            if (retVal != 0)
                return false;

            // Setting up the timer, the long Interval value needs to be negative if 
            // you want to set up a delay in milliseconds. ie 
            // if Interval = -60000000 the timer will expire in 1 minute. Once expired it runs the 
            // TimerComplete delegate
            //
            Console.WriteLine("Setting WaitableTimer");
            SetWaitableTimer(handle, ref Interval, 0, TimerComplete, IntPtr.Zero, true);
            retVal = Marshal.GetLastWin32Error();
            Console.WriteLine("Last Error = " + Marshal.GetLastWin32Error().ToString()); // The error may be 1004 (Invalid flags), this is not critical
            if (retVal != 0 && retVal != 1004)
                return false;
            Console.WriteLine("Timer set @ " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            // Starting a new thread which waits for the WaitableTimer to expire
            //
            Thread t_Wait = new Thread(new ThreadStart(() => WaitTimer(handle)));
            t_Wait.Start();

            // Raising Event Timer Set
            //
            if (OnTimerSet != null)
            {
                OnTimerSet();
            }
            return true;
        }
    }
}

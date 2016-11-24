using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WakeTimersTest
{
    public partial class WakeTimersTestForm : Form
    {
        public WakeTimersTestForm()
        {
            InitializeComponent();
            TimersTester.OnTimerSet += TimersTester_OnTimerSet;
            TimersTester.OnTimerCompleted += TimersTester_OnTimerCompleted;
        }

        private void TimersTester_OnTimerCompleted()
        {
            MessageBox.Show("OK", "Wake timer is completed", MessageBoxButtons.OK);
        }

        private void TimersTester_OnTimerSet()
        {
            MessageBox.Show("OK", "Wake timer is set", MessageBoxButtons.OK);
        }

        private void TestSupportClick(object sender, EventArgs e)
        {
            if (TimersTester.TimersSupported())
            {
                MessageBox.Show("Wake timers are supported by this system", "Wake timers support", MessageBoxButtons.OK);
            } else
            {
                MessageBox.Show("Wake timers are NOT supported by this system", "Wake timers support", MessageBoxButtons.OK);
            }
        }

        private void CreateTimerClick(object sender, EventArgs e)
        {
            if (TimersTester.CreateTimer())
            {
                MessageBox.Show("Created successfully", "New wake timer", MessageBoxButtons.OK);
            } else
            {
                MessageBox.Show("Could not create wake timer", "New wake timer", MessageBoxButtons.OK);
            }
        }
    }
}

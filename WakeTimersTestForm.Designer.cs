namespace WakeTimersTest
{
    partial class WakeTimersTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TestSupportButton = new System.Windows.Forms.Button();
            this.CreateTimerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TestSupportButton
            // 
            this.TestSupportButton.Location = new System.Drawing.Point(12, 12);
            this.TestSupportButton.Name = "TestSupportButton";
            this.TestSupportButton.Size = new System.Drawing.Size(260, 23);
            this.TestSupportButton.TabIndex = 0;
            this.TestSupportButton.Text = "Test timer support";
            this.TestSupportButton.UseVisualStyleBackColor = true;
            this.TestSupportButton.Click += new System.EventHandler(this.TestSupportClick);
            // 
            // CreateTimerButton
            // 
            this.CreateTimerButton.Location = new System.Drawing.Point(12, 41);
            this.CreateTimerButton.Name = "CreateTimerButton";
            this.CreateTimerButton.Size = new System.Drawing.Size(260, 23);
            this.CreateTimerButton.TabIndex = 1;
            this.CreateTimerButton.Text = "Create timer";
            this.CreateTimerButton.UseVisualStyleBackColor = true;
            this.CreateTimerButton.Click += new System.EventHandler(this.CreateTimerClick);
            // 
            // WakeTimersTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 70);
            this.Controls.Add(this.CreateTimerButton);
            this.Controls.Add(this.TestSupportButton);
            this.Name = "WakeTimersTestForm";
            this.Text = "WakeTimersTest";
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Button TestSupportButton;
        private System.Windows.Forms.Button CreateTimerButton;
    }
}


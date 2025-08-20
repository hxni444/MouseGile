using System.Runtime.InteropServices;

namespace MouseGile
{
    partial class MouseGile
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        /// 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int cButtons, uint dwExtraInfo);
        private System.ComponentModel.IContainer components = null;
        // Define mouse event constants
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_WHEEL = 0x0800;

        private System.Windows.Forms.Timer jiggleTimer = new System.Windows.Forms.Timer();
        private DateTime endTime;
        private bool isRunning = false;

        private Label lblStatus;
        private Button button1;
        private Button button2;


        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblStatus = new Label();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(119, 247);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Click += label1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.MediumSeaGreen;
            button1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.FromArgb(192, 255, 192);
            button1.Location = new Point(38, 156);
            button1.Name = "button1";
            button1.Size = new Size(117, 81);
            button1.TabIndex = 2;
            button1.Text = "START";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.IndianRed;
            button2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(255, 192, 192);
            button2.Location = new Point(245, 156);
            button2.Name = "button2";
            button2.Size = new Size(117, 81);
            button2.TabIndex = 3;
            button2.Text = "STOP";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 59);
            label1.Name = "label1";
            label1.Size = new Size(130, 15);
            label1.TabIndex = 4;
            label1.Text = "Set duration in minutes";
            label1.Click += label1_Click_1;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 19F);
            textBox1.Location = new Point(38, 77);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Enter Duration";
            textBox1.Size = new Size(324, 41);
            textBox1.TabIndex = 5;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 121);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 6;
            label2.Click += label2_Click;
            // 
            // MouseGile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 278);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MouseGile";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label2;
    }
}

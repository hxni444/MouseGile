namespace MouseGile
{
    partial class MouseGile
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Label lblStatus;
        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private Label label2;

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
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(250, 250, 250);
            lblStatus.Location = new Point(38, 242);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(324, 26);
            lblStatus.TabIndex = 0;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(22, 163, 74);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(21, 128, 61);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 83, 45);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(38, 150);
            button1.Name = "button1";
            button1.Size = new Size(150, 75);
            button1.TabIndex = 2;
            button1.Text = "START";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(220, 38, 38);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(127, 29, 29);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(212, 150);
            button2.Name = "button2";
            button2.Size = new Size(150, 75);
            button2.TabIndex = 3;
            button2.Text = "STOP";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(163, 163, 163);
            label1.Location = new Point(38, 54);
            label1.Name = "label1";
            label1.Size = new Size(155, 17);
            label1.TabIndex = 4;
            label1.Text = "Set duration in minutes";
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(23, 23, 23);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 16F);
            textBox1.ForeColor = Color.FromArgb(255, 255, 255);
            textBox1.Location = new Point(38, 77);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "e.g. 30";
            textBox1.Size = new Size(324, 36);
            textBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(38, 121);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 6;
            // 
            // MouseGile
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(10, 10, 10);
            ClientSize = new Size(400, 280);
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
            Text = "MouseGile";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}

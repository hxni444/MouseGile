namespace MouseGile
{
    public partial class MouseGile : Form
    {
        private RoundIndicator indicator;
        public MouseGile()
        {
            InitializeComponent();

            jiggleTimer.Tick += new EventHandler(JiggleTimer_Tick);

            button1.Enabled = true;
            button2.Enabled = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            indicator = new RoundIndicator();
            indicator.Location = new Point(this.ClientSize.Width - indicator.Width - 10, 10);
            indicator.Size = new Size(25, 25);
            indicator.IndicatorColor = Color.Red;
            this.Controls.Add(indicator);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                int durationMinutes;

                if (int.TryParse(textBox1.Text, out durationMinutes))
                {
                    if (durationMinutes > 0)
                    {
                        isRunning = true;
                        endTime = DateTime.Now.AddMinutes(durationMinutes);

                        jiggleTimer.Interval = 30000;
                        jiggleTimer.Start();

                        lblStatus.Text = $"Running for {durationMinutes} minutes...";
                        button1.Enabled = false;
                        button2.Enabled = true; 
                        indicator.IndicatorColor = Color.Green;
                        indicator.Invalidate();

                    }
                    else
                    {
                        MessageBox.Show("Please set a duration greater than zero.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.");
                }


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                jiggleTimer.Stop();
                lblStatus.Text = "";
                button1.Enabled = true;
                button2.Enabled = false;
                indicator.IndicatorColor = Color.Red;
                indicator.Invalidate();
            }
        }


        // This is the core logic, running every 30 seconds
        private void JiggleTimer_Tick(object sender, EventArgs e)
        {
            // Check time remaining
            if (DateTime.Now >= endTime)
            {
                jiggleTimer.Stop();
                isRunning = false;
                lblStatus.Text = "Finished.";
                button1.Enabled = true;
                button2.Enabled = false;
                return;
            }

            // --- Mouse Jiggle and Scroll Logic ---

            // Get current mouse position
            Point currentPosition = Cursor.Position;

            // 1. Jiggle the mouse
            // Move R,D 
            Cursor.Position = new Point(currentPosition.X + 2, currentPosition.Y + 2);
            Thread.Sleep(100); // Wait briefly

            // Move back to original position
            Cursor.Position = currentPosition;
            Thread.Sleep(100); // Wait briefly

            // 2. Scroll up and down
            // Scroll Up (Positive value)
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 5, 0);
            Thread.Sleep(100);

            // Scroll Down (Negative value)
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -5, 0);

            // Update status to show time remaining (optional, but helpful)
            TimeSpan remaining = endTime - DateTime.Now;
            lblStatus.Text = $"Running. Time Left: {remaining.Minutes}m {remaining.Seconds}s.";
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
       
    }
}

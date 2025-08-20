namespace MouseGile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            jiggleTimer.Tick += new EventHandler(JiggleTimer_Tick);

            // Set initial status text
            lblStatus.Text = "Ready. Set duration in minutes.";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                int durationMinutes = (int)numericUpDown1.Value;
                if (durationMinutes > 0)
                {
                    isRunning = true;
                    endTime = DateTime.Now.AddMinutes(durationMinutes);

                    // Set the interval to 30 seconds (30,000 milliseconds)
                    jiggleTimer.Interval = 30000;
                    jiggleTimer.Start();

                    lblStatus.Text = $"Running for {durationMinutes} minutes...";
                    button1.Enabled = false; // Disable Start button
                    button2.Enabled = true;  // Enable Stop button
                }
                else
                {
                    lblStatus.Text = "Please set a duration greater than zero.";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                jiggleTimer.Stop();
                lblStatus.Text = "Stopped.";
                button1.Enabled = true;
                button2.Enabled = false;
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
    }
}

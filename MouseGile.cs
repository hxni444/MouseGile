using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MouseGile
{
    public partial class MouseGile : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;

        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        private readonly System.Windows.Forms.Timer countdownTimer = new System.Windows.Forms.Timer();
        private DateTime endTime;
        private DateTime lastJiggleTime = DateTime.MinValue;
        private bool isRunning = false;

        private RoundIndicator indicator = null!;
        private VersionPill versionPill = null!;

        public MouseGile()
        {
            InitializeComponent();
            LoadAppIcon();

            // Real-time 1-second countdown timer
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void LoadAppIcon()
        {
            try
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mousegile.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else if (File.Exists("mousegile.ico"))
                {
                    this.Icon = new Icon("mousegile.ico");
                }
                else
                {
                    var processPath = Environment.ProcessPath;
                    if (!string.IsNullOrEmpty(processPath) && File.Exists(processPath))
                    {
                        var exeIcon = Icon.ExtractAssociatedIcon(processPath);
                        if (exeIcon != null)
                        {
                            this.Icon = exeIcon;
                        }
                    }
                }
            }
            catch
            {
                // Fallback silently if icon fails to load
            }
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            EnableDarkMode(this.Handle);

            // Add Round Status Indicator at top right with proper margin
            indicator = new RoundIndicator
            {
                Size = new Size(22, 22),
                Location = new Point(this.ClientSize.Width - 36, 14),
                IndicatorColor = Color.FromArgb(239, 68, 68)
            };
            this.Controls.Add(indicator);

            // Add Version Pill next to the indicator
            versionPill = new VersionPill
            {
                Size = new Size(88, 24),
                Location = new Point(this.ClientSize.Width - 36 - 88 - 8, 13)
            };
            this.Controls.Add(versionPill);

            // Check for updates in the background without popups unless an update is found
            _ = versionPill.CheckForUpdatesAsync(showNoUpdateMessage: false);
        }

        private static void EnableDarkMode(IntPtr handle)
        {
            try
            {
                int useDarkMode = 1;
                if (DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int)) != 0)
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDarkMode, sizeof(int));
                }

                // Set dark window border and caption to eliminate bright vertical lines on Windows 11
                int darkBorder = ColorTranslator.ToWin32(Color.FromArgb(10, 10, 10));
                DwmSetWindowAttribute(handle, DWMWA_BORDER_COLOR, ref darkBorder, sizeof(int));

                int darkCaption = ColorTranslator.ToWin32(Color.FromArgb(10, 10, 10));
                DwmSetWindowAttribute(handle, DWMWA_CAPTION_COLOR, ref darkCaption, sizeof(int));

                int whiteText = ColorTranslator.ToWin32(Color.FromArgb(240, 240, 240));
                DwmSetWindowAttribute(handle, DWMWA_TEXT_COLOR, ref whiteText, sizeof(int));
            }
            catch
            {
                // Ignored on platforms where DWM attribute is not supported
            }
        }

        private void button1_Click(object? sender, EventArgs e)
        {
            if (isRunning) return;

            if (int.TryParse(textBox1.Text.Trim(), out int durationMinutes) && durationMinutes > 0)
            {
                isRunning = true;
                endTime = DateTime.Now.AddMinutes(durationMinutes);

                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Enabled = false;

                indicator.IndicatorColor = Color.FromArgb(34, 197, 94); // Active Green

                // Immediate initial jiggle and real-time status update
                UpdateStatusDisplay(endTime - DateTime.Now);
                PerformJiggle();
                lastJiggleTime = DateTime.Now;

                // Start 1-second real-time countdown timer
                countdownTimer.Start();
            }
            else
            {
                MessageBox.Show("Please enter a valid positive duration in minutes.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }

        private void button2_Click(object? sender, EventArgs e)
        {
            if (isRunning)
            {
                StopSession(manual: true);
            }
        }

        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            TimeSpan remaining = endTime - DateTime.Now;

            // Check if duration expired
            if (remaining.TotalSeconds <= 0)
            {
                StopSession(manual: false);
                lblStatus.Text = "Finished.";
                return;
            }

            // Real-time second-by-second countdown update
            UpdateStatusDisplay(remaining);

            // Perform mouse jiggle every 30 seconds
            if ((DateTime.Now - lastJiggleTime).TotalSeconds >= 30)
            {
                PerformJiggle();
                lastJiggleTime = DateTime.Now;
            }
        }

        private void UpdateStatusDisplay(TimeSpan remaining)
        {
            string timeText = remaining.TotalHours >= 1
                ? $"{(int)remaining.TotalHours}h {remaining.Minutes:D2}m {remaining.Seconds:D2}s"
                : $"{remaining.Minutes:D2}m {remaining.Seconds:D2}s";

            lblStatus.Text = $"Time Left: {timeText}  •  Feel free to use the app";
        }

        private void StopSession(bool manual)
        {
            isRunning = false;
            countdownTimer.Stop();

            button1.Enabled = true;
            button2.Enabled = false;
            textBox1.Enabled = true;

            indicator.IndicatorColor = Color.FromArgb(239, 68, 68); // Inactive Red

            if (manual)
            {
                lblStatus.Text = "Stopped.";
            }
        }

        private void PerformJiggle()
        {
            try
            {
                Point currentPosition = Cursor.Position;

                // 1. Jiggle mouse 2px right and down, then return
                Cursor.Position = new Point(currentPosition.X + 2, currentPosition.Y + 2);
                Thread.Sleep(50);
                Cursor.Position = currentPosition;
                Thread.Sleep(50);

                // 2. Subtle mouse wheel scroll
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, 5, 0);
                Thread.Sleep(50);
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, unchecked((uint)-5), 0);
            }
            catch
            {
                // Ignore cursor movement exceptions
            }
        }
    }
}

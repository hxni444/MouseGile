using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;
using Velopack.Sources;

namespace MouseGile
{
    public class VersionPill : Control
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private string versionText = "v1.0.3";
        private bool isHovered = false;
        private bool isChecking = false;
        private bool hasUpdate = false;
        private float spinnerAngle = 0f;
        private readonly System.Windows.Forms.Timer animTimer = new System.Windows.Forms.Timer();
        private UpdateInfo? availableUpdate = null;
        private string? latestReleaseUrl = null;
        private string? latestVersionName = null;
        private ToolTip toolTip;

        public event Action<bool>? CheckingStateChanged;

        public Color PillBackColor { get; set; } = Color.FromArgb(20, 20, 20);
        public Color PillHoverColor { get; set; } = Color.FromArgb(38, 38, 38);
        public Color PillBorderColor { get; set; } = Color.FromArgb(55, 55, 55);
        public Color PillTextColor { get; set; } = Color.FromArgb(229, 229, 229);
        public Color UpdateBadgeColor { get; set; } = Color.FromArgb(16, 185, 129);
        public Color SpinnerColor { get; set; } = Color.FromArgb(56, 189, 248);

        static VersionPill()
        {
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MouseGile", "1.0"));
            httpClient.Timeout = TimeSpan.FromSeconds(6);
        }

        public VersionPill()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            this.Size = new Size(88, 24);

            toolTip = new ToolTip
            {
                InitialDelay = 200,
                ReshowDelay = 100,
                AutoPopDelay = 6000
            };

            animTimer.Interval = 40;
            animTimer.Tick += (s, e) =>
            {
                spinnerAngle = (spinnerAngle + 20f) % 360f;
                Invalidate();
            };

            LoadVersion();
            UpdateToolTip();
        }

        private void LoadVersion()
        {
            try
            {
                var mgr = new UpdateManager(new GithubSource("https://github.com/hxni444/MouseGile", null, false));
                if (mgr.IsInstalled && mgr.CurrentVersion != null)
                {
                    versionText = $"v{mgr.CurrentVersion.ToFullString()}";
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    if (ver != null)
                    {
                        versionText = $"v{ver.Major}.{ver.Minor}.{ver.Build}";
                    }
                }
            }
            catch
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                if (ver != null)
                {
                    versionText = $"v{ver.Major}.{ver.Minor}.{ver.Build}";
                }
            }
        }

        public void UpdateToolTip()
        {
            if (isChecking)
            {
                toolTip.SetToolTip(this, "Checking for updates in the background...\nFeel free to use the app while running.");
            }
            else if (hasUpdate)
            {
                string targetVer = latestVersionName ?? availableUpdate?.TargetFullRelease.Version.ToString() ?? "New";
                toolTip.SetToolTip(this, $"Update {targetVer} available on GitHub!\nClick to download & install now.");
            }
            else
            {
                toolTip.SetToolTip(this, $"MouseGile {versionText}\nClick to check GitHub for updates");
            }
        }

        public async Task CheckForUpdatesAsync(bool showNoUpdateMessage = false)
        {
            if (isChecking) return;

            isChecking = true;
            animTimer.Start();
            CheckingStateChanged?.Invoke(true);
            UpdateToolTip();
            Invalidate();

            try
            {
                var mgr = new UpdateManager(new GithubSource("https://github.com/hxni444/MouseGile", null, false));

                if (mgr.IsInstalled)
                {
                    var newVersion = await mgr.CheckForUpdatesAsync();
                    StopCheckingAnimation();

                    if (newVersion != null)
                    {
                        hasUpdate = true;
                        availableUpdate = newVersion;
                        latestVersionName = newVersion.TargetFullRelease.Version.ToString();
                        UpdateToolTip();
                        Invalidate();

                        var result = MessageBox.Show(
                            $"A new version of MouseGile ({newVersion.TargetFullRelease.Version}) is available on GitHub!\n\nWould you like to download and install it now?",
                            "Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (result == DialogResult.Yes)
                        {
                            await ApplyUpdateAsync(mgr, newVersion);
                        }
                        return;
                    }
                }
                else
                {
                    // Fallback to GitHub Releases API for development/portable mode
                    try
                    {
                        using var response = await httpClient.GetAsync("https://api.github.com/repos/hxni444/MouseGile/releases/latest");
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            using var doc = JsonDocument.Parse(json);
                            if (doc.RootElement.TryGetProperty("tag_name", out var tagProp))
                            {
                                string tagName = tagProp.GetString() ?? "";
                                latestReleaseUrl = doc.RootElement.TryGetProperty("html_url", out var urlProp)
                                    ? urlProp.GetString() ?? "https://github.com/hxni444/MouseGile/releases"
                                    : "https://github.com/hxni444/MouseGile/releases";

                                string cleanTag = tagName.TrimStart('v', 'V');
                                string cleanCurrent = versionText.TrimStart('v', 'V');

                                if (Version.TryParse(cleanTag, out var remoteVer) && Version.TryParse(cleanCurrent, out var localVer) && remoteVer > localVer)
                                {
                                    StopCheckingAnimation();
                                    hasUpdate = true;
                                    latestVersionName = $"v{remoteVer}";
                                    UpdateToolTip();
                                    Invalidate();

                                    var result = MessageBox.Show(
                                        $"A new version of MouseGile (v{remoteVer}) is available on GitHub!\n\nWould you like to open GitHub to download the latest release?",
                                        "Update Available",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information
                                    );

                                    if (result == DialogResult.Yes && !string.IsNullOrEmpty(latestReleaseUrl))
                                    {
                                        Process.Start(new ProcessStartInfo { FileName = latestReleaseUrl, UseShellExecute = true });
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore API request failures when offline
                    }
                }

                StopCheckingAnimation();
                hasUpdate = false;
                UpdateToolTip();
                Invalidate();

                if (showNoUpdateMessage)
                {
                    MessageBox.Show($"You're up to date! MouseGile {versionText} is the latest version.", "MouseGile Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                StopCheckingAnimation();
                UpdateToolTip();
                Invalidate();
                if (showNoUpdateMessage)
                {
                    MessageBox.Show($"Could not check for updates:\n{ex.Message}", "Update Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void StopCheckingAnimation()
        {
            isChecking = false;
            animTimer.Stop();
            CheckingStateChanged?.Invoke(false);
        }

        private async Task ApplyUpdateAsync(UpdateManager mgr, UpdateInfo newVersion)
        {
            try
            {
                isChecking = true;
                animTimer.Start();
                Invalidate();

                await mgr.DownloadUpdatesAsync(newVersion);
                mgr.ApplyUpdatesAndRestart(newVersion);
            }
            catch (Exception ex)
            {
                StopCheckingAnimation();
                Invalidate();
                MessageBox.Show($"Failed to apply update: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            Invalidate();
        }

        protected override async void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (hasUpdate && availableUpdate != null)
            {
                try
                {
                    var mgr = new UpdateManager(new GithubSource("https://github.com/hxni444/MouseGile", null, false));
                    await ApplyUpdateAsync(mgr, availableUpdate);
                    return;
                }
                catch
                {
                    // Fallthrough to regular check
                }
            }
            else if (hasUpdate && !string.IsNullOrEmpty(latestReleaseUrl))
            {
                Process.Start(new ProcessStartInfo { FileName = latestReleaseUrl, UseShellExecute = true });
                return;
            }

            await CheckForUpdatesAsync(showNoUpdateMessage: true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            int cornerRadius = this.Height / 2;

            using (GraphicsPath path = GetRoundedRectanglePath(rect, cornerRadius))
            {
                // Background
                Color currentBackColor = isHovered ? PillHoverColor : PillBackColor;
                if (hasUpdate)
                {
                    currentBackColor = isHovered ? Color.FromArgb(6, 78, 59) : Color.FromArgb(6, 95, 70);
                }

                using (SolidBrush brush = new SolidBrush(currentBackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Border
                Color currentBorderColor = hasUpdate ? UpdateBadgeColor : PillBorderColor;
                using (Pen pen = new Pen(currentBorderColor, 1f))
                {
                    e.Graphics.DrawPath(pen, path);
                }

                // Left Indicator: Animated Spinner or Static Dot
                int dotSize = 8;
                int dotX = 8;
                int dotY = (this.Height - dotSize) / 2;

                if (isChecking)
                {
                    // Smooth spinning loader arc
                    using (Pen spinnerPen = new Pen(SpinnerColor, 1.8f))
                    {
                        spinnerPen.StartCap = LineCap.Round;
                        spinnerPen.EndCap = LineCap.Round;
                        e.Graphics.DrawArc(spinnerPen, dotX - 1, dotY - 1, dotSize + 2, dotSize + 2, spinnerAngle, 220);
                    }
                }
                else
                {
                    // Static dot indicator
                    Color dotColor = hasUpdate ? UpdateBadgeColor : Color.FromArgb(56, 189, 248);
                    using (SolidBrush dotBrush = new SolidBrush(dotColor))
                    {
                        e.Graphics.FillEllipse(dotBrush, dotX + 1, dotY + 1, 6, 6);
                    }
                }

                // Version Text: Always keeps the version number displayed
                string displayText = hasUpdate ? $"{versionText} • ⬆" : versionText;
                Color currentTextColor = hasUpdate ? Color.FromArgb(167, 243, 208) : PillTextColor;

                using (SolidBrush textBrush = new SolidBrush(currentTextColor))
                {
                    var textRect = new Rectangle(dotX + dotSize + 4, 0, this.Width - (dotX + dotSize + 8), this.Height);
                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    e.Graphics.DrawString(displayText, this.Font, textBrush, textRect, stringFormat);
                }
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animTimer?.Dispose();
                toolTip?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

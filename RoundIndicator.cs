using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MouseGile
{
    public class RoundIndicator : Control
    {
        private Color _indicatorColor = Color.FromArgb(239, 68, 68); // Soft modern red

        public Color IndicatorColor
        {
            get => _indicatorColor;
            set
            {
                _indicatorColor = value;
                Invalidate();
            }
        }

        public RoundIndicator()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.Width = this.Height = 22;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Outer subtle translucent ring
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(40, _indicatorColor)))
            {
                e.Graphics.FillEllipse(bgBrush, 0, 0, this.Width - 1, this.Height - 1);
            }

            // Inner solid glowing dot
            int inset = 3;
            using (Brush brush = new SolidBrush(_indicatorColor))
            {
                e.Graphics.FillEllipse(brush, inset, inset, this.Width - 1 - (inset * 2), this.Height - 1 - (inset * 2));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGile
{
    public class RoundIndicator : Control
    {
        public Color IndicatorColor { get; set; } = Color.Red;

        public RoundIndicator()
        {
            this.DoubleBuffered = true;   // reduce flicker
            this.ResizeRedraw = true;     // redraw on resize
            this.Width = this.Height = 30; // default size
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Brush brush = new SolidBrush(IndicatorColor))
            {
                // Only fill, no border
                e.Graphics.FillEllipse(brush, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }
}

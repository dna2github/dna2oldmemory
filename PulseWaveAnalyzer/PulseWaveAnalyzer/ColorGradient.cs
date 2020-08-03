using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PulseWaveAnalyzer
{
    class ColorGradient
    {
        private SpacePoint[] gradient;

        public ColorGradient(SpacePoint[] ps)
        {
            gradient = ps;
        }

        public Color select(double value)
        {
            if (gradient == null) return Color.White;
            if (gradient.Length == 0) return Color.White;
            SpacePoint p1 = gradient[0], p2 = null;

            if (p1 == null) return Color.White;
            if (gradient.Length == 1 || p1.extra >= value)
            {
                // single color or min >= value
                return Color.FromArgb((int)(p1.x * 255), (int)(p1.y * 255), (int)(p1.z * 255));
            }

            SpaceLine line = null;
            double percent = 0.0;
            for (int i = 1; i < gradient.Length; i++)
            {
                p2 = gradient[i];
                if (p2 == null) return Color.White;
                if (value > p1.extra && value <= p2.extra)
                {
                    percent = p2.extra - p1.extra;
                    // point collision or decrease order
                    if (percent <= 0.0f) return Color.FromArgb((int)(p1.x * 255), (int)(p1.y * 255), (int)(p1.z * 255));
                    percent = (value - p1.extra) / percent;
                    line = new SpaceLine(p1, p2);
                    p1 = line.get(percent);
                    return Color.FromArgb((int)(p1.x * 255), (int)(p1.y * 255), (int)(p1.z * 255));
                }
                p1 = p2;
            }

            // max < value
            if (p2 == null) return Color.White;
            return Color.FromArgb((int)(p2.x * 255), (int)(p2.y * 255), (int)(p2.z * 255));
        }
    }
}

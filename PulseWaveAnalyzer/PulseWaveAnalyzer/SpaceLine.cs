using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer
{
    class SpaceLine
    {
        public SpaceLine(SpacePoint p1, SpacePoint p2)
        {
            x = p1.x; y = p1.y; z = p1.z;
            l = p2.x - p1.x;
            m = p2.y - p1.y;
            n = p2.z - p1.z;
        }

        public SpacePoint get(double percent)
        {
            return new SpacePoint(x + l * percent, y + m * percent, z + n * percent, percent);
        }

        private double l, m, n;
        private double x, y, z; 
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PulseWaveAnalyzer
{
    class SpacePoint
    {
        public SpacePoint(double x, double y, double z, double extra)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.extra = extra;
        }

        public double x, y, z;
        public double extra;
    }
}

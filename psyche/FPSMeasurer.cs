using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace psyche
{
    class SimpleFPSMeasurer
    {
        private readonly Stopwatch sw = new Stopwatch();
        private int count;

        public void Restart() {
            sw.Restart();
            count = 0;
        }

        public void Tick() {
            if (!sw.IsRunning) Restart();
            count++;
        }

        public double Average() {
            return count / sw.Elapsed.TotalSeconds;
        }

        public double Measure() {
            double a = Average();
            Restart();
            return a;
        }
    }
}

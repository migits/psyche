using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace psyche
{
    class FPSTimer
    {
        public delegate void FrameTickCallback(double time);
        
        private readonly Stopwatch sw = new Stopwatch();
        private readonly long ticksPerFrame;
        private long nextFrameTickTime;

        public double Fps { get; }

        public FPSTimer(double fps) {
            if (fps <= 0.0) {
                throw new ArgumentException("fps must be > 0.0");
            }
            
            this.ticksPerFrame = (long)(Stopwatch.Frequency / fps);
            this.Fps = fps;
            this.nextFrameTickTime = ticksPerFrame;
        }

        public void WaitSurplus() {
            while (sw.ElapsedTicks < nextFrameTickTime) {
                // CPUを一息入れるにはどれがいいんだろ...
                Thread.Yield();
                // Thread.Sleep(0);
                // Thread.Sleep(1);
            };

            nextFrameTickTime += ticksPerFrame;
        }

        public void Start() {
            if (sw.IsRunning) return;
            sw.Start();
        }

        public void Stop() {
            if (!sw.IsRunning) return;
            sw.Stop();
        }

        public bool IsRunning { get{
            return sw.IsRunning;
        } }

        public TimeSpan Elapsed { get {
            return sw.Elapsed;
        } }
    }
}

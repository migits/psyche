using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace psyche
{
    class FPSTimer
    {
        public delegate void FrameTickCallback(double time);

        private readonly Stopwatch sw = new Stopwatch();
        private readonly FrameTickCallback ftc;
        private readonly long ticksPerFrame;

        public readonly double fps;

        public FPSTimer(FrameTickCallback ftc, double fps) {
            this.ftc = ftc;
            this.fps = fps;
            this.ticksPerFrame = (long)(Stopwatch.Frequency / fps);
        }

        public void Start() {
            sw.Start();
            
            Task.Factory.StartNew(() => {
                long t0 = sw.ElapsedTicks;

                while (sw.IsRunning) {
                    this.ftc((double)t0 / Stopwatch.Frequency);

                    while (sw.ElapsedTicks - t0 < ticksPerFrame) {
                        // CPUを一息入れるにはどれがいいんだろ...
                        Thread.Yield();
                        // Thread.Sleep(0);
                        // Thread.Sleep(1);
                    };

                    t0 += ticksPerFrame;
                }
            });
        }

        public void Stop() {
            sw.Stop();
        }
    }
}

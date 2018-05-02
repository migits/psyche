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
        private readonly FrameTickCallback ftc;
        private readonly long ticksPerFrame;
        private readonly Control ctrl;
        private readonly object lockObj = new object();
        private readonly object ftcInvokingLock = new object();
        private Task task;

        public readonly double fps;

        public FPSTimer(FrameTickCallback ftc, double fps, Control ctrl) {
            this.ftc = ftc;
            this.fps = fps;
            this.ctrl = ctrl;
            this.ticksPerFrame = (long)(Stopwatch.Frequency / fps);
        }

        public void Start() {
            if (sw.IsRunning) return;

            sw.Start();
            
            task = Task.Factory.StartNew(() => {
                long t0, t1;
                lock(sw) t0 = t1 = sw.ElapsedTicks;
                
                IAsyncResult ar;
                while (true) {
                    ar = ctrl.BeginInvoke(ftc, (double)(t0 / Stopwatch.Frequency));

                    while (t1 - t0 < ticksPerFrame) {
                        // CPUを一息入れるにはどれがいいんだろ...
                        Thread.Yield();
                        // Thread.Sleep(0);
                        // Thread.Sleep(1);
                        
                        lock(lockObj) {
                            if (!sw.IsRunning) return;
                        }
                        t1 = sw.ElapsedTicks;
                    };

                    t0 += ticksPerFrame;
                }
            });
        }

        public void Stop() {
            if (!sw.IsRunning) return;
            
            lock(lockObj) sw.Stop();
            task.Wait();
        }

        public bool IsRunning { get{return sw.IsRunning;} }
    }
}

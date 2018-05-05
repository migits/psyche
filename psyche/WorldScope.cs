using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MathNet.Numerics;

namespace psyche {
    class WorldScope {
        private World w;
        private float zoomLevel;
        private float scale;
        private float rot;
        private Complex32 c;
        private Complex32 screenC, worldC;

        public WorldScope(World w) {
            this.w = w;
            this.c = Complex32.Zero;
            ZoomLevel = 0.0f;
            RotRad = 0.0f;
        }

        public void Paint(Graphics g, int height, int width) {
            g.Clear(Color.Black);

            var sc = new Complex32(width / 2.0f, height / 2.0f);
            
            (Complex32 z0, Complex32 z1)[] boundaryLines = {
                (new Complex32(-w.Size, 0.0f), new Complex32(2.0f*w.Size, 0.0f)),
                (new Complex32(-w.Size, w.Size), new Complex32(2.0f*w.Size, w.Size)),
                (new Complex32(0.0f, -w.Size), new Complex32(0.0f, 2.0f*w.Size)),
                (new Complex32(w.Size, -w.Size), new Complex32(w.Size, 2.0f*w.Size))
            };

            foreach (var line in boundaryLines) {
                Complex32 z0 = ScreenC * line.z0 + sc,
                          z1 = ScreenC * line.z1 + sc;
                g.DrawLine(Pens.Gray, z0.Real, z0.Imaginary, z1.Real, z1.Imaginary);
            }

            var points = new PointF[4];
            for (int i = 0; i < w.Size; i++) {
                for (int j = 0; j < w.Size; j++) {
                    if (w.cells[i, j]) {
                        Complex32 z0 = ScreenC * (new Complex32(j, i)) + sc,
                                  z1 = ScreenC * (new Complex32(j+1, i)) + sc,
                                  z2 = ScreenC * (new Complex32(j+1, i+1)) + sc,
                                  z3 = ScreenC * (new Complex32(j, i+1)) + sc;
                        (points[0].X, points[0].Y) = (z0.Real, z0.Imaginary);
                        (points[1].X, points[1].Y) = (z1.Real, z1.Imaginary);
                        (points[2].X, points[2].Y) = (z2.Real, z2.Imaginary);
                        (points[3].X, points[3].Y) = (z3.Real, z3.Imaginary);
                        g.FillPolygon(Brushes.Lime, points);
                    }
                }
            }
        }

        public Complex32 Center {
            get { return c; }
            set { c = value; }
        }

        private Complex32 ScreenC {
            get { return screenC; }
            set {
                screenC = value;
                worldC = Complex32.One / screenC;
            }
        }
        private Complex32 WorldC {
            get { return worldC; }
            set {
                worldC = value;
                screenC = Complex32.One / worldC;
            }
        }
        
        public float ZoomLevel {
            get { return zoomLevel; }
            set {
                zoomLevel = value;
                scale = Convert.ToSingle(Math.Pow(2.0, zoomLevel));
                ScreenC = Complex32.FromPolarCoordinates(scale, rot);
            }
        }
        public float Scale {
            get { return scale; }
            set {
                scale = value;
                zoomLevel = Convert.ToSingle(Math.Log(scale, 2.0));
                ScreenC = Complex32.FromPolarCoordinates(scale, rot);
            }
        }

        public float RotRad {
            get { return rot; }
            set {
                var t = 2.0 * Math.PI;
                rot = value - Convert.ToSingle(t*Math.Floor(value/t));
                Complex32.FromPolarCoordinates(scale, rot);
            }
        }
        public float RotDeg {
            get { return RotRad / Convert.ToSingle(Math.PI) * 180.0f; }
            set { RotRad = value / 180.0f * Convert.ToSingle(Math.PI); }
        }

        public float CenterX {
            get { return c.Real; }
            set { c = new Complex32(value, c.Imaginary); }
        }
        public float CenterY {
            get { return c.Imaginary; }
            set { c = new Complex32(c.Real, value); }
        }
    }
}
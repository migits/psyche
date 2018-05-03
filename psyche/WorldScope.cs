using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace psyche {
    class WorldScope {
        private World w;
        private double zoomLevel;
        private double scale;
        private double rot;
        private double cx, cy;

        public WorldScope(World w) {
            this.w = w;
            this.CenterX = w.Width/2.0;
            this.CenterY = w.Height/2.0;
            ZoomLevel = 0.0;
            this.rot = 0.0;
        }

        public void Paint(Graphics g) {
            g.Clear(Color.Black);

            double scx = 640 / 2,
                   scy = 480 / 2;

            (double x0, double y0, double x1, double y1)[] boardaryLines = {
                (-w.Width, 0.0, 2.0*w.Width, 0.0),
                (-w.Width, w.Height, 2.0*w.Width, w.Height),
                (0.0, -w.Height, 0.0, 2.0*w.Height),
                (w.Width, -w.Height, w.Width, 2.0*w.Height),
            };

            foreach (var line in boardaryLines) {
                (float sx0, float sy0) = TCWorldToScreen(line.x0, line.y0, scx, scy);
                (float sx1, float sy1) = TCWorldToScreen(line.x1, line.y1, scx, scy);
                g.DrawLine(Pens.Gray, sx0, sy0, sx1, sy1);
            }

            foreach (Atom a in w.Atoms) {
                (float sx, float sy) = TCWorldToScreen(a.X, a.Y, scx, scy);
                g.DrawEllipse(Pens.Lime, sx - AtomRadius, sy - AtomRadius, sx + AtomRadius, sy + AtomRadius);
            }
        }

        public double CenterX {
            get { return cx; }
            set { cx = value - w.Width*Math.Floor(value/w.Width); }
        }
        public double CenterY {
            get { return cy; }
            set { cy = value - w.Height * Math.Floor(value / w.Height); }
        }

        public double ZoomLevel {
            get { return zoomLevel; }
            set {
                zoomLevel = value;
                scale = Math.Pow(2.0, zoomLevel);
            }
        }
        public double Scale {
            get { return scale; }
            set {
                scale = value;
                zoomLevel = Math.Log(scale, 2.0);
            }
        }

        public double RotRad {
            get { return rot; }
            set {
                double t = 2.0 * Math.PI;
                rot = value - t*Math.Floor(value/t);
            }
        }
        public double RotDeg {
            get { return RotRad / Math.PI * 180.0; }
            set { RotRad = value / 180.0 * Math.PI; }
        }

        public float AtomRadius { get; set; }

        public (float sx, float sy) TCWorldToScreen(
            double wx, double wy, double scx, double scy)
        {
            double relX = wx - cx,
                   relY = wy - cy;
            return (
                Convert.ToSingle(scale*(relX*Math.Cos(rot) - relY*Math.Sin(rot)) + scx),
                Convert.ToSingle(-scale*(relX*Math.Sin(rot) + relY*Math.Cos(rot)) + scy)
            );
        }
    }
}
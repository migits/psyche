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
            this.CenterX = 0.0;
            this.CenterY = 0.0;
            ZoomLevel = 0.0;
            this.rot = 0.0;
        }

        public void Paint(Graphics g, int height, int width) {
            g.Clear(Color.Black);

            float scx = width / 2.0f,
                  scy = height / 2.0f;

            (double x0, double y0, double x1, double y1)[] boardaryLines = {
                (-w.Size, 0.0, 2.0*w.Size, 0.0),
                (-w.Size, w.Size, 2.0*w.Size, w.Size),
                (0.0, -w.Size, 0.0, 2.0*w.Size),
                (w.Size, -w.Size, w.Size, 2.0*w.Size),
            };

            foreach (var line in boardaryLines) {
                (float sx0, float sy0) = TCWorldToScreen(line.x0, line.y0, scx, scy);
                (float sx1, float sy1) = TCWorldToScreen(line.x1, line.y1, scx, scy);
                g.DrawLine(Pens.Gray, sx0, sy0, sx1, sy1);
            }

            var cell = new PointF[4];
            for (int i = 0; i < w.Size; i++) {
                for (int j = 0; j < w.Size; j++) {
                    if (w.cells[i, j]) {
                        (cell[0].X, cell[0].Y) = TCWorldToScreen(i, j, scx, scy);
                        (cell[1].X, cell[1].Y) = TCWorldToScreen(i, j+1, scx, scy);
                        (cell[2].X, cell[2].Y) = TCWorldToScreen(i-1, j+1, scx, scy);
                        (cell[3].X, cell[3].Y) = TCWorldToScreen(i-1, j, scx, scy);
                        g.FillPolygon(Brushes.Lime, cell);
                    }
                }
            }
        }

        public double CenterX {
            get { return cx; }
            set { cx = value - w.Size*Math.Floor(value/w.Size); }
        }
        public double CenterY {
            get { return cy; }
            set { cy = value - w.Size * Math.Floor(value / w.Size); }
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
            double wx, double wy, float scx, float scy)
        {
            double relX = wx - cx,
                   relY = wy - cy;
            return (
                Convert.ToSingle(scale*(relX*Math.Cos(rot) - relY*Math.Sin(rot)) + scx),
                Convert.ToSingle(-scale*(relX*Math.Sin(rot) + relY*Math.Cos(rot)) + scy)
            );
        }

        public (double wx, double wy) TCScreenToWorld(
            float sx, float sy, float scx, float scy)
        {
            float relX = sx - scx,
                  relY = sy - scy;
            return (
                (relX*Math.Cos(-rot) - (-relY)*Math.Sin(-rot))/scale + cx,
                (relX*Math.Sin(-rot) + (-relY)*Math.Cos(-rot))/scale + cy
            );
        }
    }
}
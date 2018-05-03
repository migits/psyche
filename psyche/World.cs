using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace psyche
{
    class World {
        private double height, width;
        private HashSet<Atom> atoms = new HashSet<Atom>();

        public World(double height, double width) {
            if (height <= 0.0 || width <= 0.0) {
                throw new ArgumentException("height or width must be > 0.0");
            }
            this.height = height;
            this.width = width;
        }

        public void Update() {
            foreach (var a in atoms) {
                a.Update();
                a.X %= width;
                a.Y %= height;
            }
        }

        public double Height { get { return height; } }
        public double Width { get { return width; } }
        public HashSet<Atom> Atoms { get { return atoms; } }
    }

    abstract class Atom {
        public double Z { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }

        public void Update() {
            Interact();
            X += VX;
            X += VY;
        }

        public void Accel(double ax, double ay) {
            VX += ax;
            VY += ay;
        }

        public void ApplyForce(double fx, double fy) {
            VX += fx/Z;
            VY += fy/Z;
        }

        protected abstract void Interact();
    }
}

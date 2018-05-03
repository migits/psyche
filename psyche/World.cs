using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace psyche
{
    class World {
        private Random random;

        public bool[,] cells;
        public int Size { get; }
        public SortedSet<int> ruleS, ruleB;
        public SortedSet<(int, int)> neighborhood;

        public World(int size) {
            if (size <= 0) {
                throw new ArgumentException("size must be > 0");
            }
            this.cells = new bool[size, size];
            this.Size = size;

            this.ruleS = new SortedSet<int> { 2, 3 };
            this.ruleB = new SortedSet<int> { 3 };
            this.neighborhood = new SortedSet<(int, int)> {
                (-1, -1), (-1, 0), (-1, 1),
                ( 0, -1),          ( 0, 1),
                ( 1, -1), ( 1, 0), ( 1, 1)
            };

            this.random = new Random();
        }

        public void Update() {
            var old = (bool[,])cells.Clone();

            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    int count = 0;
                    foreach ((int m, int n) in neighborhood) {
                        if (old[(int)MyMath.FloorMod(i+m, Size), (int)MyMath.FloorMod(j+n, Size)]) {
                            count++;
                        }
                    }
                    cells[i,j] &= ruleS.Contains(count);
                    cells[i,j] |= ruleB.Contains(count);
                }
            }
        }

        public void InitRandom(double rate) {
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    cells[i,j] = (random.NextDouble() < rate);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace psyche {
    static class MyMath {
        public static double FloorMod(double dividend, double divisor) {
            return dividend - divisor*Math.Floor(dividend/divisor);
        }
    }
}

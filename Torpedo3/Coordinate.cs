using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo3
{
    public class Coordinate
    {
        public int XCord { get; set; }
        public int YCord { get; set; }

        public Coordinate(int x, int y)
        {
            XCord = x;
            YCord = y;
        }
    }
}

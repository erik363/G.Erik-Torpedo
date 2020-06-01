using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo3
{
    public class Ship
    {
        public int Size { set; get; }
        public int X { set; get; }
        public int Y { set; get; }
        public int Direction { set; get; }
        public bool IsSank { set; get; }

        public Dictionary<int, List<int>> coords;       

        public Ship(int X, int Y, int Direction, int Size)
        {
            this.X = X;
            this.Y = Y;
            this.Direction = Direction;
            this.Size = Size;
            this.coords = new Dictionary<int, List<int>>();
            IsSank = false;
        }
    }
}
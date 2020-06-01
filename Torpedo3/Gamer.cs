using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo3
{
    public abstract class Gamer
    {
        public string Name { get; set; }
        public int[,] Board { get; set; }
        public int[,] ShootingBoard { get; set; }

        public List<Ship> ships;

        public Gamer(int[,] board, int[,] shootingBoard, string name)
        {
            Board = board;
            ShootingBoard = shootingBoard;
            ships = new List<Ship>();
            Name = name;
        }

        public abstract Coordinate Shoot();

    }
}
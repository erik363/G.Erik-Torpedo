﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo3
{
    public class Player : Gamer
    {
        List<Coordinate> shootedPlaces;
        public Player(int[,] board, int[,] shootingBoard, string name) : base(board, shootingBoard, name)
        {
            shootedPlaces = new List<Coordinate>();
        }
        public override Coordinate Shoot()
        {
            throw new NotImplementedException();
        }

        public Coordinate Shoot2(int xCord, int yCord)
        {
                Coordinate target = new Coordinate(xCord, yCord);
    
                    shootedPlaces.Add(target);
                    return target;
                          
        }
    }
}
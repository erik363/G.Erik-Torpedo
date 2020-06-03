using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = Microsoft.Build.Tasks.Message;

namespace Torpedo3
{
    public class Bot : Gamer
    {
        public List<Coordinate> catchedPoints;      
        List<Coordinate> shootedPlaces;
        public bool onHitStreak;
        public Bot(int[,] board, int[,] shootingBoard, string name) : base(board, shootingBoard, name)
        {
            shootedPlaces = new List<Coordinate>();
            catchedPoints = new List<Coordinate>();
            onHitStreak = false;
        }
        public override Coordinate Shoot()
        {
            List<int> nonShootedDirections = new List<int>() { 0, 1, 2, 3 };

            if (catchedPoints.Count == 0)       
            {
                return ShootFirstTime();
            }
            else if (catchedPoints.Count == 1)   
            {
                return ShootIfGotOneHit();
            }
            else   
            {
                return ShootIfGotMoreThan2Hits();
            }
        }


        public Coordinate ShootFirstTime()
        {
            Random random = new Random();
            int x = random.Next(0, Board.GetLength(0));
            int y = random.Next(0, Board.GetLength(1));


            bool unacceptedRandom = true;
            while (unacceptedRandom)
            {
                if (shootedPlaces.Count == 0)
                {
                    unacceptedRandom = false;
                }

                foreach (var coordinate in shootedPlaces)
                {
                    if (x != coordinate.XCord || y != coordinate.YCord)
                    {
                        unacceptedRandom = false;
                    }
                    else
                    {
                        x = random.Next(0, Board.GetLength(0));
                        y = random.Next(0, Board.GetLength(1));
                        break;
                    }
                }
            }

            Coordinate target = new Coordinate(x, y);
            shootedPlaces.Add(target);
            return target;
        }

        public Coordinate ShootIfGotOneHit()
        {
            Random random = new Random();
            Coordinate target;
            List<string> directionsToShoot = new List<string>() { "up", "down", "right", "left" };
            int directionIndex;

            while (true)
            {
                directionIndex = random.Next(0, directionsToShoot.Count);

                if (directionsToShoot[directionIndex].Equals("up"))
                {
                    if (catchedPoints[0].XCord > 0)
                    {
                        target = new Coordinate(catchedPoints[0].XCord - 1, catchedPoints[0].YCord);

                        if (ALreadyShootedThere(shootedPlaces, target))
                        {
                            shootedPlaces.Add(target);
                            return target;
                        }
                    }
                }
                else if (directionsToShoot[directionIndex].Equals("down"))
                {
                    if (catchedPoints[0].XCord < 9)
                    {
                        target = new Coordinate(catchedPoints[0].XCord + 1, catchedPoints[0].YCord);

                        if (ALreadyShootedThere(shootedPlaces, target))
                        {
                            shootedPlaces.Add(target);
                            return target;
                        }
                    }
                }
                else if (directionsToShoot[directionIndex].Equals("right"))
                {
                    if (catchedPoints[0].YCord < 9)
                    {
                        target = new Coordinate(catchedPoints[0].XCord, catchedPoints[0].YCord + 1);

                        if (ALreadyShootedThere(shootedPlaces, target))
                        {
                            shootedPlaces.Add(target);
                            return target;
                        }
                    }
                }
                else
                {
                    if (catchedPoints[0].YCord > 0)
                    {
                        target = new Coordinate(catchedPoints[0].XCord, catchedPoints[0].YCord - 1);

                        if (ALreadyShootedThere(shootedPlaces, target))
                        {
                            shootedPlaces.Add(target);
                            return target;
                        }
                    }
                }
                directionsToShoot.RemoveAt(directionIndex);
            }
        }


        public bool ALreadyShootedThere(List<Coordinate> shootedPlaces, Coordinate target)
        {
            foreach (var coordinate in shootedPlaces)   
            {
                if (coordinate.XCord == target.XCord && coordinate.YCord == target.YCord)
                    return false;
            }
            return true;
        }

        public Coordinate ShootIfGotMoreThan2Hits()
        {
            string direction = null;
            Coordinate target = null;
          
            if (catchedPoints[0].XCord == catchedPoints[1].XCord)
            {
                //vízszintes
                if (catchedPoints[0].YCord < catchedPoints[1].YCord)
                {
                    //jobbra
                    direction = "right";
                }
                else
                {
                    //balra
                    direction = "left";
                }
            }
            else if (catchedPoints[0].YCord == catchedPoints[1].YCord)
            {
                //függőleges
                if (catchedPoints[0].XCord < catchedPoints[1].XCord)
                {
                    //le
                    direction = "down";
                }
                else
                {
                    //fel
                    direction = "up";
                }
            }


            if (onHitStreak)
            {
                return ShootNextToHit(direction, target);
            }
            else
            {
                return ShootIfShipNotSankButMiss(direction, target);
            }
        }


        public Coordinate ShootNextToHit(string direction, Coordinate target)
        {
            while (true)
            {
                if (direction.Equals("right"))
                {

                    if (catchedPoints[catchedPoints.Count - 1].YCord < 9)
                    {
                        target =
                            new Coordinate(catchedPoints[catchedPoints.Count - 1].XCord,
                            catchedPoints[catchedPoints.Count - 1].YCord + 1);

                        shootedPlaces.Add(target);
                        return target;
                    }
                    else
                    {
                        Coordinate firstHit = catchedPoints[0];
                        catchedPoints = new List<Coordinate>();
                        catchedPoints.Add(firstHit);

                        direction = "left";     
                    }
                }
                else if (direction.Equals("left"))
                {
                    if (catchedPoints[catchedPoints.Count - 1].YCord > 0)  
                    {
                        target =
                            new Coordinate(catchedPoints[catchedPoints.Count - 1].XCord,
                            catchedPoints[catchedPoints.Count - 1].YCord - 1);

                        shootedPlaces.Add(target);
                        return target;
                    }
                    else
                    {
                        Coordinate firstHit = catchedPoints[0];
                        catchedPoints = new List<Coordinate>();
                        catchedPoints.Add(firstHit);

                        direction = "right"; 
                    }
                }
                else if (direction.Equals("up"))
                {
                    if (catchedPoints[catchedPoints.Count - 1].XCord > 0)
                    {
                        target =
                            new Coordinate(catchedPoints[catchedPoints.Count - 1].XCord - 1,
                            catchedPoints[catchedPoints.Count - 1].YCord);

                        shootedPlaces.Add(target);
                        return target;
                    }
                    else
                    {
                        Coordinate firstHit = catchedPoints[0];
                        catchedPoints = new List<Coordinate>();
                        catchedPoints.Add(firstHit);

                        direction = "down"; 
                    }
                }
                else
                {
                    if (catchedPoints[catchedPoints.Count - 1].XCord < 9)
                    {
                        target =
                            new Coordinate(catchedPoints[catchedPoints.Count - 1].XCord + 1,
                            catchedPoints[catchedPoints.Count - 1].YCord);

                        shootedPlaces.Add(target);
                        return target;
                    }
                    else
                    {
                        Coordinate firstHit = catchedPoints[0];
                        catchedPoints = new List<Coordinate>();
                        catchedPoints.Add(firstHit);

                        direction = "up"; 
                    }
                }
            }
        }


        public Coordinate ShootIfShipNotSankButMiss(string direction, Coordinate target)
        {
            if (direction.Equals("right"))
            {                  
                if (catchedPoints[0].YCord - 1 > 0)
                {
                    target =
                        new Coordinate(catchedPoints[catchedPoints.Count - 1].XCord,
                        catchedPoints[catchedPoints.Count - 1].YCord - 1);

                    shootedPlaces.Add(target);
                    return target;
                }


            }
            else if (direction.Equals("left"))
            {
                if (catchedPoints[0].YCord + 1 < 9)
                {
                    target =
                        new Coordinate(catchedPoints[0].XCord,
                        catchedPoints[0].YCord + 1);

                    shootedPlaces.Add(target);
                    return target;
                }


            }
            else if (direction.Equals("up"))
            {
                if (catchedPoints[0].XCord + 1 < 9)
                {
                    target =
                        new Coordinate(catchedPoints[0].XCord + 1,
                        catchedPoints[0].YCord);

                    shootedPlaces.Add(target);
                    return target;
                }

            }
            else
            {
                if (catchedPoints[0].XCord - 1 > 0)
                {
                    target =
                        new Coordinate(catchedPoints[0].XCord - 1,
                        catchedPoints[0].YCord);

                    shootedPlaces.Add(target);
                    return target;
                }

            }
            return null;
        }

        

    }
}
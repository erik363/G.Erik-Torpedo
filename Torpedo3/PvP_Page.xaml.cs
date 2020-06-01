using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Torpedo3
{
    /// <summary>
    /// Interaction logic for PvP_Page.xaml
    /// </summary>
    public partial class PvP_Page : Page
    {
        int[] sizes = { 4, 4, 3, 3, 2, 2, 1, 1, 1, 1 };
        Ship ship;
        List<Ship> ships;
        int[,] board;
        int presentTable;
        private Cell _cell;

        private const int GameWidth = 10;
        private const int GameHeight = 10;

        private Player player1;
        private Player player2;

        private int counter = 0;

        public PvP_Page()
        {
            InitializeComponent();

            InitGame();

            BeginGame();
        }

        private void InitGame()
        {
            player1Canvas.Children.Clear();
            player2Canvas.Children.Clear();

        }

        private void DrawPoint(Cell position, Brush brush, Canvas actCanvas)
        {
            var shape = new Rectangle();
            shape.Fill = brush;
            var unitX = player1Canvas.Width / GameWidth;
            var unitY = player1Canvas.Height / GameHeight;
            shape.Width = unitX;
            shape.Height = unitY;
            Canvas.SetTop(shape, position.Y * unitY);
            Canvas.SetLeft(shape, position.X * unitX);
            actCanvas.Children.Add(shape);
        }

        public void BeginGame()
        {
            int xLength = 10;
            int yLength = 10;

            board = new int[xLength, yLength];
            ships = new List<Ship>();

            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    board[i, j] = 0;
                    _cell = new Cell(i, j);
                    DrawPoint(_cell, Brushes.Blue, player1Canvas);
                    DrawPoint(_cell, Brushes.Blue, player2Canvas);
                }
            }

            int[,] boardPlayer = new int[xLength, yLength];
            Array.Copy(board, boardPlayer, board.Length);

            int[,] shotingBoardPlayer = new int[xLength, yLength];
            Array.Copy(board, shotingBoardPlayer, board.Length);

            player1 = new Player(boardPlayer, shotingBoardPlayer, "Játékos vagyok");

            int[,] boardBot = new int[xLength, yLength]; ;
            Array.Copy(board, boardBot, board.Length);

            int[,] shotingBoardBot = new int[xLength, yLength]; ;
            Array.Copy(board, shotingBoardBot, board.Length);

            player2 = new Player(boardBot, shotingBoardBot, "Játékos vagyok");

        }

        public void PrintingShootingBoard(Gamer player, int s)
        {
            //Console.WriteLine("////" + player.Name + "'s Shootingtable" + "\\\\");
            for (int i = 0; i < player.Board.GetLength(0); i++)
            {
                for (int j = 0; j < player.Board.GetLength(1); j++)
                {
                    if (s == 1)
                    {
                        int actual = player.ShootingBoard[i, j];
                        if (actual == 0)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, player1CanvasS);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, player1CanvasS);
                        }
                        else if (actual == 7)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Black, player1CanvasS);
                        }
                    }
                    else
                    {
                        int actual = player.ShootingBoard[i, j];
                        if (actual == 0)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, player2CanvasS);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, player2CanvasS);
                        }
                        else if (actual == 7)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Black, player2CanvasS);
                        }
                    }
                }
            }
        }

        public bool StartShooting(Player attacker, Player defender, int x, int y)
        {
            bool gameOver = false;
            Ship sankedShip = null;

            Coordinate target = attacker.Shoot2(x, y);
            Console.WriteLine(attacker.Name + " LŐTT A: " + target.XCord + " , " + target.YCord + " -ra");

            foreach (var ship in defender.ships)
            {
                int fixCoordinate;
                int targetingFixCoordinate = target.XCord;
                bool horizontal = true;
                bool toBreak = false;

                if (ship.Direction == 0)
                {
                    fixCoordinate = ship.coords.Keys.ElementAt(0);
                }
                else
                {
                    int firstXcoordinate = ship.coords.Keys.ElementAt(0);
                    fixCoordinate = ship.coords[firstXcoordinate][0];
                    targetingFixCoordinate = target.YCord;
                    horizontal = false;
                }


                int shipHitPoint = 0;
                foreach (var xCord in ship.coords.Keys)
                {
                    if (!horizontal)
                        shipHitPoint++;

                    foreach (var yCord in ship.coords[xCord])
                    {
                        if (horizontal)
                            shipHitPoint++;
                    }
                }
                foreach (var xCord in ship.coords.Keys)
                {


                    foreach (var yCord in ship.coords[xCord])
                    {
                        if (fixCoordinate == targetingFixCoordinate)
                        {
                            if (horizontal)
                            {
                                if (yCord == target.YCord)
                                {
                                    attacker.ShootingBoard[target.XCord, target.YCord] = 7;

                                    if (shipHitPoint > 1)
                                    {
                                        if (ship.coords[xCord].Count > 1)
                                        {
                                            ship.coords[xCord].Remove(yCord);
                                            toBreak = true;
                                            break;
                                        }
                                        else
                                        {
                                            ship.coords.Remove(xCord);

                                            toBreak = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        sankedShip = ship;
                                        toBreak = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    attacker.ShootingBoard[target.XCord, target.YCord] = 4;
                                }
                            }
                            else
                            {
                                if (xCord == target.XCord)
                                {
                                    attacker.ShootingBoard[target.XCord, target.YCord] = 7;

                                    if (ship.coords[xCord].Count > 1)
                                    {
                                        ship.coords[xCord].Remove(yCord);
                                        toBreak = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (ship.coords.Count > 0)
                                        {
                                            ship.coords.Remove(xCord);
                                            if (ship.coords.Count == 0)
                                            {
                                                sankedShip = ship;
                                            }
                                            toBreak = true;
                                            break;
                                        }
                                    }
                                    toBreak = true;
                                    break;
                                }
                                else
                                {
                                    attacker.ShootingBoard[target.XCord, target.YCord] = 4;
                                }
                            }
                        }
                        else
                        {
                            attacker.ShootingBoard[target.XCord, target.YCord] = 4;
                        }
                    }
                    if (toBreak)
                    {
                        break;
                    }
                }
                if (toBreak)
                    break;
            }

            if (sankedShip != null)
            {
                defender.ships.Remove(sankedShip);
                sankedShip.IsSank = true;
            }

            if (defender.ships.Count == 0)
                gameOver = true;

            return gameOver;
        }

        public void PlaceShips(Gamer player, int x = 0, int y = 0, int count = 0)
        {
            bool isBot = false;


            int direction = 0;

            if (player is Player)
            {
            }
            else
            {
                Random random = new Random();
                x = random.Next(0, board.GetLength(0));
                y = random.Next(0, board.GetLength(1));
                direction = random.Next(0, 2);

                isBot = true;
            }

            ship = CheckPositions(direction, x, y, count, isBot);

            if (player.ships.Count == 0)
            {
                AddFirstShip(ship, count, player);

            }
            else
            {
                List<int> changingCoords = GetMarkedCoords(count, ship);
                int fixCoord = y;

                if (direction == 0)
                {
                    fixCoord = x;
                }

                if (IsPlaceable(ship.Direction, changingCoords, fixCoord, player))
                {

                    player.ships.Add(ship);

                    AddShipCoordinates(ship, count, ship.Direction, ship.X, ship.Y);

                    DrawShips(player);
                    PrintingBoard(player);
                }
                else
                {

                    count--;
                }
            }

        }

        public bool IsPlaceable(int direction, List<int> changingCoords, int fixCoord, Gamer player)
        {
            bool canPlace = true;
            foreach (var currentShip in player.ships)
            {
                foreach (var xCord in currentShip.coords.Keys)
                {
                    if (direction == 0)
                    {
                        foreach (var yCord in currentShip.coords[xCord])
                        {
                            if (changingCoords.Contains(yCord) && xCord == fixCoord)
                            {
                                canPlace = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (var yCord in currentShip.coords[xCord])
                        {
                            if (changingCoords.Contains(xCord) && (yCord == fixCoord))
                            {
                                canPlace = false;
                                break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void DrawShips(Gamer player)
        {
            foreach (var ship in player.ships)
            {
                foreach (var xCord in ship.coords.Keys)
                {
                    foreach (var yCord in ship.coords[xCord])
                    {
                        player.Board[xCord, yCord] = 1;
                    }
                }
            }
        }

        public Ship AddShipCoordinates(Ship ship, int count, int direction, int x, int y)
        {
            List<int> tmp = new List<int>();
            for (int plus = 0; plus < sizes[count]; plus++)
            {
                if (direction == 1)
                {
                    tmp.Add(y);
                    ship.coords.Add(x + plus, tmp);
                    tmp = new List<int>();
                }
                else
                {
                    tmp.Add(y + plus);
                }
            }

            if (direction == 0)
                ship.coords.Add(x, tmp);

            return ship;
        }

        public void AddFirstShip(Ship ship, int count, Gamer player)
        {
            ship = AddShipCoordinates(ship, count, ship.Direction, ship.X, ship.Y);



            player.ships.Add(ship);

            DrawShips(player);
            PrintingBoard(player);
        }

        public void PrintingBoard(Gamer player)
        {
            for (int i = 0; i < player.Board.GetLength(0); i++)
            {
                for (int j = 0; j < player.Board.GetLength(1); j++)
                {
                    if (counter%2 == 0)
                    {
                        int actual = player.Board[i, j];
                        if (actual == 1)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, player1Canvas);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, player1Canvas);
                        }
                    }
                    else
                    {
                        int actual = player.Board[i, j];
                        if (actual == 1)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, player2Canvas);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, player2Canvas);
                        }
                    }


                }

            }

        }

        public List<int> GetMarkedCoords(int count, Ship ship)
        {
            List<int> changingCoords = new List<int>();

            for (int c = 0; c < sizes[count]; c++)
            {
                if (ship.Direction == 0)    //y változik
                {
                    changingCoords.Add(ship.Y + c);
                }
                else
                {
                    changingCoords.Add(ship.X + c);
                }
            }

            return changingCoords;
        }

        public Ship CheckPositions(int direction, int x, int y, int count, bool isBot)
        {
            while (true)
            {
                if (direction == 0)     //y változik
                {
                    if (y + sizes[count] < board.GetLength(1))
                    {
                        Ship result = new Ship(x, y, direction, sizes[count]);
                        return result;
                    }
                }
                else
                {
                    if (x + sizes[count] < board.GetLength(0))
                    {
                        Ship result = new Ship(x, y, direction, sizes[count]);
                        return result;
                    }
                }

                if (!isBot)
                {

                    direction = Convert.ToInt32(Console.ReadLine());
                }
                else
                {

                    Random random = new Random();
                    x = random.Next(0, board.GetLength(0));
                    y = random.Next(0, board.GetLength(1));

                    direction = random.Next(0, 2);
                }
            }
        }

        private void playerCanvasS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player2Canvas).X / 30;
            double y = e.GetPosition(player2Canvas).Y / 30;
            if(counter%2 == 1 && counter < 10)
            {
                tbScore.Text = x.ToString();

                PlaceShips(player2, (int)x, (int)y, counter);
                player2Canvas.Visibility = Visibility.Hidden;
                player1Canvas.Visibility = Visibility.Visible;
                counter++;
           

            }


        }

        private void playerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player1Canvas).X / 30;
            double y = e.GetPosition(player1Canvas).Y / 30;
            if (counter % 2 == 0 && counter < 10)
            {
                tbScore.Text = x.ToString();

                PlaceShips(player1, (int)x, (int)y, counter);

                player1Canvas.Visibility = Visibility.Hidden;
                player2Canvas.Visibility = Visibility.Visible;
                counter++;
                //tbScore.Text = "2";

            }

        }
    }
}

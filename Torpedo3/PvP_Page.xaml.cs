using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        int hits1 = 0;
        int hits2= 0;
        int[] sizes = { 4, 4, 3, 3, 2, 2, 1, 1, 1, 1 };
        Ship ship;
        List<Ship> ships;
        int[,] board;
        int presentTable;
        private Cell _cell;
        int rounds = 0;
        List<Cell> alreadyShooted;
        List<Cell> alreadyShooted2;
        private const int GameWidth = 10;
        private const int GameHeight = 10;

        private Player player1;
        private Player player2;

        private int counter = 0;
        String pName1;
        String pName2;

        public PvP_Page()
        {
            InitializeComponent();

            InitGame();

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

            player1 = new Player(boardPlayer, shotingBoardPlayer, pName1);

            int[,] boardBot = new int[xLength, yLength]; ;
            Array.Copy(board, boardBot, board.Length);

            int[,] shotingBoardBot = new int[xLength, yLength]; ;
            Array.Copy(board, shotingBoardBot, board.Length);

            player2 = new Player(boardBot, shotingBoardBot, pName2);

            alreadyShooted = new List<Cell>();
            alreadyShooted2 = new List<Cell>();
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
            count.Text = " : " + (int)++rounds/2;
            Coordinate target = attacker.Shoot2(x, y);
            Console.WriteLine(attacker.Name + " LŐTT A: " + target.XCord + " , " + target.YCord + " -ra");
            if (attacker.Name.Equals(pName1))
                alreadyShooted.Add(new Cell(x,y));
            else
                alreadyShooted2.Add(new Cell(x, y));

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
                                    if (attacker.Name.Equals(pName1))                                 
                                        play1hits.Text = "Találat: " + ++hits1;
                                    else
                                        play2hits.Text = "Találat: " + ++hits2;

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
                                    if (attacker.Name.Equals(pName1))
                                        play1hits.Text = "Találat: " + ++hits1;
                                    else
                                        play2hits.Text = "Találat: " + ++hits2;

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

        public void PlaceShips(Gamer player, int x = 0, int y = 0, int count = 0, int direction = 0)
        {
            bool isBot = false;
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
            if (counter % 2 == 0 && counter < sizes.Length)
            {
                tbScore.Text = x.ToString();
                PlaceShips(player1, (int)x, (int)y, counter);
                player1Canvas.Visibility = Visibility.Hidden;
                player2Canvas.Visibility = Visibility.Visible;
                counter++;
                //tbScore.Text = "2";
            }
            if (counter == sizes.Length)
            {
                player1CanvasS.Visibility = Visibility.Visible;
                player2CanvasS.Visibility = Visibility.Visible;
            }
        }

        private void player1Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player1Canvas).X / 30;
            double y = e.GetPosition(player1Canvas).Y / 30;
            if (counter % 2 == 0 && counter < sizes.Length)
            {
                tbScore.Text = x.ToString();
                PlaceShips(player1, (int)x, (int)y, counter, 1);
                player1Canvas.Visibility = Visibility.Hidden;
                player2Canvas.Visibility = Visibility.Visible;
                counter++;
                //tbScore.Text = "2";
            }

        }

        private void player2Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player2Canvas).X / 30;
            double y = e.GetPosition(player2Canvas).Y / 30;
            if (counter % 2 == 1 && counter < 10)
            {
                tbScore.Text = x.ToString();
                PlaceShips(player2, (int)x, (int)y, counter, 1);
                player2Canvas.Visibility = Visibility.Hidden;
                player1Canvas.Visibility = Visibility.Visible;
                counter++;
            }

        }

        private void player1CanvasS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player1CanvasS).X / 30;
            double y = e.GetPosition(player1CanvasS).Y / 30;
            bool contains = false;
            foreach (var i in alreadyShooted)
            {
                if (i.X == (int)x && i.Y == (int)y)
                    contains = true;

            }
            if (!contains)
            {
                if (counter % 2 == 0)
                {
                    player1Canvas.Visibility = Visibility.Hidden;
                    
                    StartShooting(player1, player2, (int)x, (int)y);
                    counter++;
                    PrintingShootingBoard(player1, 1);
                    PrintingShootingBoard(player2, 2);
                    String firstOwn = "0", secondOwn = "0", thirdOwn = "0", fourthOwn = "0";
                    String firstEnemy = "0", secondEnemy = "0", thirdEnemy = "0", fourthEnemy = "0";
                    try
                    {
                        firstOwn = player1.remainShips[0].Size.ToString();
                        secondOwn = player1.remainShips[1].Size.ToString();
                        thirdOwn = player1.remainShips[2].Size.ToString();
                        fourthOwn = player1.remainShips[3].Size.ToString();

                        firstEnemy = player2.remainShips[0].Size.ToString();
                        secondEnemy = player2.remainShips[1].Size.ToString();
                        thirdEnemy = player2.remainShips[2].Size.ToString();
                        fourthEnemy = player2.remainShips[3].Size.ToString();

                    }
                    catch
                    {

                    }
                    ownShips.Text = firstOwn + " " + secondOwn + " " + thirdOwn + " " + fourthOwn;
                    enemyShips.Text = firstEnemy + " " + secondEnemy + " " + thirdEnemy + " " + fourthEnemy;
                }
                if (hits1 == 11 || hits2 == 11)
                    GameWon();
            }
        }

        private void GameWon()
        {
            AllCanvas.Visibility = Visibility.Hidden;
            String JSONtxt = File.ReadAllText(@"d:\test.json");
            var accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Data>>(JSONtxt);
            List<Data> temp = accounts.ToList();
            temp.Add(new Data
            {
                Name1 = player1.Name,
                Name2 = player2.Name,
                Rounds = rounds
            });
            File.WriteAllText(@"d:\test.json", JsonConvert.SerializeObject(temp));
        }

        private void player2CanvasS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(player2CanvasS).X / 30;
            double y = e.GetPosition(player2CanvasS).Y / 30;
            bool contains = false;
            foreach (var i in alreadyShooted2)
            {
                if (i.X == (int)x && i.Y == (int)y)
                    contains = true;

            }
            if (!contains)
            {
                if (counter % 2 == 1)
                {
                    
                    StartShooting(player2, player1, (int)x, (int)y);
                    counter++;
                    PrintingShootingBoard(player1, 1);
                    PrintingShootingBoard(player2, 2);
                    String firstOwn = "0", secondOwn = "0", thirdOwn = "0", fourthOwn = "0";
                    String firstEnemy = "0", secondEnemy = "0", thirdEnemy = "0", fourthEnemy = "0";
                    try
                    {
                        firstOwn = player1.remainShips[0].Size.ToString();
                        secondOwn = player1.remainShips[1].Size.ToString();
                        thirdOwn = player1.remainShips[2].Size.ToString();
                        fourthOwn = player1.remainShips[3].Size.ToString();

                        firstEnemy = player2.remainShips[0].Size.ToString();
                        secondEnemy = player2.remainShips[1].Size.ToString();
                        thirdEnemy = player2.remainShips[2].Size.ToString();
                        fourthEnemy = player2.remainShips[3].Size.ToString();

                    }
                    catch
                    {

                    }
                    ownShips.Text = firstOwn + " " + secondOwn + " " + thirdOwn + " " + fourthOwn;
                    enemyShips.Text = firstEnemy + " " + secondEnemy + " " + thirdEnemy + " " + fourthEnemy;
                }
                if (hits1 == 11 || hits2 == 11)
                    GameWon();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pName1 = player1Name.Text;
            pName2 = player2Name.Text;
            names.Height = 0;
            names.Width = 0;
            names.Visibility = Visibility.Hidden;
            player1Canvas.Visibility = Visibility.Visible;
            player2Canvas.Visibility = Visibility.Visible;
            BeginGame();
            nameLabel1.Text = player1.Name + " hajói:";
            nameLabel2.Text = player2.Name + " hajói:";
           
        }
    }
}

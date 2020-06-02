using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PvAI_Page.xaml
    /// </summary>
    public partial class PvAI_Page : Page
    {
        int hits1 = 0;
        int hits2 = 0;
        //int[] sizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        int[] sizes = { 4, 3, 2, 1, 1 };
        Ship ship;
        List<Ship> ships;
        int[,] board;
        int presentTable;
        private Cell _cell;
        String pName1 = "";
        int rounds = 0;
        private const int GameWidth = 10;
        private const int GameHeight = 10;

        private Player player;
        private Bot bot;

        private int counter = 0;

        int who = -1;
        public PvAI_Page()
        {
            InitializeComponent();

            InitGame();
        }

        private void InitGame()
        {
            playerCanvas.Children.Clear();
            botCanvas.Children.Clear();

        }

        private void DrawPoint(Cell position, Brush brush, Canvas actCanvas)
        {
            var shape = new Rectangle();
            shape.Fill = brush;
            var unitX = playerCanvas.Width / GameWidth;
            var unitY = playerCanvas.Height / GameHeight;
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
                    DrawPoint(_cell, Brushes.Blue, playerCanvas);
                    DrawPoint(_cell, Brushes.Blue, botCanvas);
                }
            }

            int[,] boardPlayer = new int[xLength, yLength];
            Array.Copy(board, boardPlayer, board.Length);

            int[,] shotingBoardPlayer = new int[xLength, yLength];
            Array.Copy(board, shotingBoardPlayer, board.Length);

            player = new Player(boardPlayer, shotingBoardPlayer, pName1);

            int[,] boardBot = new int[xLength, yLength]; ;
            Array.Copy(board, boardBot, board.Length);

            int[,] shotingBoardBot = new int[xLength, yLength]; ;
            Array.Copy(board, shotingBoardBot, board.Length);

            bot = new Bot(boardBot, shotingBoardBot, "AI");

            botplace(bot);


        }

        public void botplace(Bot player)
        {
            for (int count = 0; count < sizes.Length; count++)
            {
                bool isBot = true;
                int x = 0;
                int y = 0;

                int direction = 0;

                Thread.Sleep(1000);
                Random random = new Random();
                x = random.Next(0, board.GetLength(0));
                Thread.Sleep(2);
                y = random.Next(0, board.GetLength(1));
                Thread.Sleep(2);
                direction = random.Next(0, 2);

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
                        player.remainShips.Add(ship);
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
        }


        public void PrintingShootingBoard(Gamer player)
        {
            //Console.WriteLine("////" + player.Name + "'s Shootingtable" + "\\\\");
            for (int i = 0; i < player.Board.GetLength(0); i++)
            {
                for (int j = 0; j < player.Board.GetLength(1); j++)
                {
                    if (player is Bot)
                    {
                        int actual = player.ShootingBoard[i, j];
                        if (actual == 0)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, botCanvasS);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, botCanvasS);
                        }
                        else if (actual == 7)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Black, botCanvasS);
                            
                        }
                    }
                    else
                    {
                        int actual = player.ShootingBoard[i, j];
                        if (actual == 0)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, playerCanvasS);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, playerCanvasS);
                        }
                        else if (actual == 7)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Black, playerCanvasS);

                        }
                    }
                }
            }
        }

        public bool StartShooting(Player attacker, Bot defender, int x, int y)
        {
            bool gameOver = false;
            Ship sankedShip = null;
            count.Text = " : " + ++rounds;
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
                                    
                                    play1hits.Text = "Találat: " + ++hits1;
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
                                    play1hits.Text = "Találat: " + ++hits1;
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

        public bool StartShooting(Bot attacker, Player defender)
        {
            bool gameOver = false;
            Ship sankedShip = null;

            Coordinate target = attacker.Shoot();
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
                                    play2hits.Text = "Találat: " + ++hits2;

                                    if (attacker is Bot)
                                    {
                                        Bot bot = (Bot)attacker;
                                        bot.onHitStreak = true;
                                        bot.catchedPoints.Add(target);
                                    }


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

                                        if (attacker is Bot)
                                        {
                                            Bot bot = (Bot)attacker;
                                            bot.catchedPoints = new List<Coordinate>();
                                            bot.onHitStreak = false;
                                        }

                                        toBreak = true;
                                        break;
                                    }
                                    toBreak = true;
                                    break;
                                }
                                else
                                {

                                    if (attacker is Bot)
                                    {
                                        Bot bot = (Bot)attacker;
                                        bot.onHitStreak = false;
                                    }

                                    attacker.ShootingBoard[target.XCord, target.YCord] = 4;
                                }
                            }
                            else
                            {
                                if (xCord == target.XCord)
                                {

                                    attacker.ShootingBoard[target.XCord, target.YCord] = 7;
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

                                    if (attacker is Bot)
                                    {
                                        Bot bot = (Bot)attacker;
                                        bot.onHitStreak = false;
                                    }
                                    attacker.ShootingBoard[target.XCord, target.YCord] = 4;
                                }
                            }
                        }
                        else
                        {

                            if (attacker is Bot)
                            {
                                Bot bot = (Bot)attacker;
                                bot.onHitStreak = false;
                            }
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
                    player.remainShips.Add(ship);
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
                    if (player is Bot)
                    {
                        int actual = player.Board[i, j];
                        if (actual == 1)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, botCanvas);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, botCanvas);
                        }
                    }
                    else
                    {
                        int actual = player.Board[i, j];
                        if (actual == 1)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Coral, playerCanvas);
                        }
                        else if (actual == 4)
                        {
                            _cell = new Cell(i, j);
                            DrawPoint(_cell, Brushes.Red, playerCanvas);
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


        private void playerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(playerCanvas).X / 30;
            double y = e.GetPosition(playerCanvas).Y / 30;

            if (counter < sizes.Length)
            {
                tbScore.Text = x.ToString();
                PlaceShips(player, (int)x, (int)y, counter);
                counter++;
                if (counter == sizes.Length)
                {
                    playerCanvasS.Visibility = Visibility.Visible;
                    botCanvasS.Visibility = Visibility.Visible;
                        
                }
            }
        }

        private void playerCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(playerCanvas).X / 30;
            double y = e.GetPosition(playerCanvas).Y / 30;

            if (counter < sizes.Length)
            {
                tbScore.Text = x.ToString();
                PlaceShips(player, (int)x, (int)y, counter, 1);
                counter++;
            }
        }



        private void playerCanvasS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(playerCanvasS).X / 30;
            double y = e.GetPosition(playerCanvasS).Y / 30;
            if (sizes.Length == counter)
            {
                Random begin = new Random();
                who = begin.Next(0, 2);
                if (who == 1)
                {
                    StartShooting(player, bot, (int)x, (int)y);
                    StartShooting(bot, player);
                }
                else
                {
                    StartShooting(bot, player);
                    StartShooting(player, bot, (int)x, (int)y);
                }
                PrintingShootingBoard(player);
                PrintingShootingBoard(bot);
                counter++;
            }
            else
            {
                if (who == 1)
                {
                    StartShooting(player, bot, (int)x, (int)y);
                    StartShooting(bot, player);
                }
                else
                {
                    StartShooting(bot, player);
                    StartShooting(player, bot, (int)x, (int)y);
                }
                counter++;
                PrintingShootingBoard(player);
                PrintingShootingBoard(bot);
                String firstOwn = "0", secondOwn = "0", thirdOwn = "0", fourthOwn = "0";
                String firstEnemy = "0", secondEnemy = "0", thirdEnemy = "0", fourthEnemy = "0";
                try
                {
                    firstOwn = player.remainShips[0].Size.ToString();
                    secondOwn = player.remainShips[1].Size.ToString();
                    thirdOwn = player.remainShips[2].Size.ToString();
                    fourthOwn = player.remainShips[3].Size.ToString();

                    firstEnemy = bot.remainShips[0].Size.ToString();
                    secondEnemy = bot.remainShips[1].Size.ToString();
                    thirdEnemy = bot.remainShips[2].Size.ToString();
                    fourthEnemy = bot.remainShips[3].Size.ToString();

                }
                catch
                {

                }
                ownShips.Text = firstOwn + " " + secondOwn + " " + thirdOwn + " " + fourthOwn;
                enemyShips.Text = firstEnemy + " " + secondEnemy + " " + thirdEnemy + " " + fourthEnemy;

                List<string> asd = new List<string>();
                

            }
        }



        private void Page_KeyDown_1(object sender, KeyEventArgs e)
        {
            tbScore.Text = e.Key.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pName1 = player1Name.Text;
            names.Height = 0;
            names.Width = 0;
            names.Visibility = Visibility.Hidden;
            playerCanvas.Visibility = Visibility.Visible;
            botCanvas.Visibility = Visibility.Visible;
            BeginGame();
            nameLabel.Text = player.Name + " hajói:";
            

        }
    }
}
    


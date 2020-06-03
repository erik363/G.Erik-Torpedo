using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo3;

namespace UnitTestTorpedo
{
    [TestClass]
    public class BotTest
    {
        [TestMethod]
        public void ALreadyShootedThereTest()
        {
            int xLength = 3;
            int yLength = 3;
            int[,] board = new int[xLength, yLength];
            int[,] sboard = new int[xLength, yLength];
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    board[i, j] = 0;
                    sboard[i, j] = 0;
                }
            }
            board[1, 1] = 4;
            var bot = new Bot(board, sboard, "AI");
            List<Coordinate> shootedPlaces = new List<Coordinate>(); 
            Coordinate target;
            target = new Coordinate(1, 1);
            shootedPlaces.Add(target);
           
            var result = bot.ALreadyShootedThere(shootedPlaces, target);
            var result1 = bot.ALreadyShootedThere(shootedPlaces, new Coordinate(0,1));
            var result2 = bot.ALreadyShootedThere(shootedPlaces, new Coordinate(2, 1));
            var result3 = bot.ALreadyShootedThere(shootedPlaces, new Coordinate(1, 0));
            var result4 = bot.ALreadyShootedThere(shootedPlaces, new Coordinate(1, 2));

            Assert.IsFalse(result);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
        }
    }
}

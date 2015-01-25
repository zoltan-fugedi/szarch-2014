using System;
using System.Linq;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MedievalWarfare.Common.Test
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void InitPlayer()
        {
            var player = new Player();

            Assert.AreEqual(ConstantValues.InitialGold, player.Gold);
            Assert.IsFalse(player.Neutral);

        }

        [TestMethod]
        public void InitUnit()
        {
            var unit = new Unit();

            Assert.AreEqual(ConstantValues.BaseMovement, unit.Movement);
            Assert.AreEqual(ConstantValues.BaseUnitStr, unit.Strength);
        }

        [TestMethod]
        public void Game_AddPlayer()
        {
            var game = new Game();
            var player = new Player { Name = "testPlayer" };

            game.AddPlayer(player);

            var addedplayer = game.Players.Single(p => p.PlayerId == player.PlayerId);

            Assert.AreEqual(player.Gold, addedplayer.Gold);
            Assert.AreEqual(player.IsWinner, addedplayer.IsWinner);
            Assert.AreEqual(player.Name, addedplayer.Name);
            Assert.AreEqual(player.Neutral, addedplayer.Neutral);
            Assert.AreEqual(player.IsWinner, addedplayer.IsWinner);
        }

        [TestMethod]
        public void Game_GetPlayer()
        {
            var game = new Game();
            var player = new Player { Name = "testPlayer" };

            game.AddPlayer(player);

            var addedplayer = game.GetPlayer(player.PlayerId);

            Assert.AreEqual(player.Gold, addedplayer.Gold);
            Assert.AreEqual(player.IsWinner, addedplayer.IsWinner);
            Assert.AreEqual(player.Name, addedplayer.Name);
            Assert.AreEqual(player.Neutral, addedplayer.Neutral);
            Assert.AreEqual(player.IsWinner, addedplayer.IsWinner);
        }

        [TestMethod]
        public void Game_IsEndGame()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            game.AddPlayer(player);

            var addedplayer = game.GetPlayer(player.PlayerId);

            Assert.IsTrue(game.IsEndGame());

            game.Map.AddBuilding(player, new Building(), 10, 10, true);

            Assert.IsFalse(game.IsEndGame());

        }

        [TestMethod]
        public void Game_IsWinner()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            game.AddPlayer(player);

            var addedplayer = game.GetPlayer(player.PlayerId);

            Assert.IsFalse(game.IsWinner(player));

            game.Map.AddBuilding(player, new Building(), 10, 10, true);

            Assert.IsTrue(game.IsWinner(player));
        }

        [TestMethod]
        public void Map_AddUnit()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            var unit = new Unit();
            game.Map.AddUnit(player, unit, 10, 10, true);

            var addedUnit = game.Map.ObjectList.Single(u => u.Owner.PlayerId == player.PlayerId);

            Assert.AreEqual(player.PlayerId, addedUnit.Owner.PlayerId);
            Assert.AreEqual(10, addedUnit.Tile.X);
            Assert.AreEqual(10, addedUnit.Tile.Y);
            Assert.AreEqual("Unit", addedUnit.Type);
        }

        [TestMethod]
        public void Map_AddBuilding()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            var building = new Building();
            game.Map.AddBuilding(player, building, 10, 10, true);

            var addedBuilding = game.Map.ObjectList.Single(b => b.Owner.PlayerId == player.PlayerId);

            Assert.AreEqual(player.PlayerId, addedBuilding.Owner.PlayerId);
            Assert.AreEqual(10, addedBuilding.Tile.X);
            Assert.AreEqual(10, addedBuilding.Tile.Y);
            Assert.AreEqual("Building", addedBuilding.Type);
        }

        [TestMethod]
        public void Map_AddTresure()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            game.Map.AddTreasure(10, 10, player, true);

            var addedTreasure = game.Map.ObjectList.Single(t => t.Owner.PlayerId == player.PlayerId);

            Assert.AreEqual(player.PlayerId, addedTreasure.Owner.PlayerId);
            Assert.AreEqual(10, addedTreasure.Tile.Y);
            Assert.AreEqual(10, addedTreasure.Tile.X);
            Assert.AreEqual("Treasure", addedTreasure.Type);
        }


        [TestMethod]
        public void Map_MoveUnti()
        {
            var game = new Game();
            game.Map.GenerateMap();
            var player = new Player { Name = "testPlayer" };

            var unit = new Unit();
            game.Map.AddUnit(player, unit, 10, 10, true);

            var addedUnit = game.Map.ObjectList.Single(u => u.Owner.PlayerId == player.PlayerId) as Unit;

            var succ = game.Map.MoveUnit(player, addedUnit, 11, 11);

            addedUnit = game.Map.ObjectList.Single(u => u.Owner.PlayerId == player.PlayerId) as Unit;

            Assert.AreEqual(player.PlayerId, addedUnit.Owner.PlayerId);
            Assert.AreEqual(11, addedUnit.Tile.X);
            Assert.AreEqual(11, addedUnit.Tile.Y);
            Assert.AreEqual("Unit", addedUnit.Type);
        }

    }
}

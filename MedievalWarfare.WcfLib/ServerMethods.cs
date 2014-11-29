using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using MedievalWarfare.WcfLib.Debug;
using MedievalWarfare.WcfLib.GameState;

namespace MedievalWarfare.WcfLib
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class ServerMethods : IServerMethods
    {
        // Change to dict to easy access to each user
        private ConcurrentDictionary<Guid, IClientCallback> callbackList;
        private Game currentGame;
        private GameStateController gameStateController;
        private OutputHelper _outputHelper;

        public ServerMethods()
        {
            callbackList = new ConcurrentDictionary<Guid, IClientCallback>();
            currentGame = new Game();
            currentGame.Map.GenerateMap();
            gameStateController = new GameStateController();
            _outputHelper = new OutputHelper(gameStateController);
        }

        public void Join(Player info)
        {
            var registeredUser = OperationContext.Current.GetCallbackChannel<IClientCallback>();

            if (gameStateController.CurreState == (GameState.GameState.State.WaitingForJoin) ||
                (gameStateController.CurreState == GameState.GameState.State.PlayerOneJoined) ||
                (gameStateController.CurreState == GameState.GameState.State.PlayerTwoJoined))
            {
                // Subscribe the user to the conversation

                if (callbackList.TryAdd(info.PlayerId, registeredUser))
                {
                    currentGame.AddPlayer(info);
                    if (gameStateController.CurreState == GameState.GameState.State.PlayerOneJoined)
                    {
                        currentGame.Map.AddNewPlayerObjects(6, 6, info);
                        Console.WriteLine(string.Format("Player Two joined!"));
                    }
                    else
                    {
                        currentGame.Map.AddNewPlayerObjects(2, 2, info);
                        Console.WriteLine(string.Format("Player One joined!"));
                    }
                    gameStateController.CurrentPlayer = info;
                    gameStateController.NextState();
                }

                if (gameStateController.CurreState == GameState.GameState.State.PlayerTwoJoined)
                {
                    callbackList[gameStateController.PlayerOne.PlayerId].StartGame(currentGame, true);
                    callbackList[gameStateController.PlayerTwo.PlayerId].StartGame(currentGame, false);
                    gameStateController.NextState();
                    callbackList[gameStateController.PlayerOne.PlayerId].StartTurn();
                    gameStateController.NextState();
                    Console.WriteLine(string.Format("Player One Turn!"));

                }

            }
        }

        public void Leave(Player info)
        {
            IClientCallback callBack;
            callbackList.TryRemove(info.PlayerId, out callBack);
            
            foreach (var clientCallback in callbackList)
            {
                clientCallback.Value.EndGame(true);
            }
        }

        public Game GetGameState()
        {
            return currentGame;
        }

        public void EndTurn(Player info)
        {
            if (gameStateController.CurrentPlayer.PlayerId == info.PlayerId &&
                ((gameStateController.CurreState == GameState.GameState.State.PlayerOneTurn) || (gameStateController.CurreState == GameState.GameState.State.PlayerTwoTurn)))
            {
                gameStateController.CurrentPlayerTurnEnded = true;
                Console.WriteLine(string.Format("{0} is ended the turn!", _outputHelper.GetPlayerName(info)));
                currentGame.EndPlayerTurn(info);
                gameStateController.NextState();
                callbackList[gameStateController.CurrentPlayer.PlayerId].StartTurn();
                Console.WriteLine(string.Format("{0} is started the turn!", _outputHelper.GetPlayerName(gameStateController.CurrentPlayer)));

            }

            if (currentGame.IsEndGame())
            {
                gameStateController.GameEndFlag = true;
                gameStateController.NextState();

                var players = currentGame.Players.Where(player => player.Neutral != true).ToList();

                if (players[0].IsWinner)
                {
                    callbackList[players[0].PlayerId].EndGame(true);
                    callbackList[players[1].PlayerId].EndGame(false);
                }
                else
                {
                    callbackList[players[0].PlayerId].EndGame(false);
                    callbackList[players[1].PlayerId].EndGame(true);
                }

                // set new state to server
                gameStateController.NextState();
                currentGame = new Game();
                currentGame.Map.GenerateMap();
                callbackList.Clear();

            }

        }

        public void UpdateMap(Command command)
        {
            bool success = false;
            var curretPlayerId = command.Player.PlayerId;

            if (command is MoveUnit)
            {
                var cmd = command as MoveUnit;
                var moveingUnit = currentGame.Map.ObjectList.Single(unit => unit.Id == cmd.Unit.Id) as Unit;
                Console.WriteLine(string.Format("{0} is moving unit to X:{1} Y:{2}. Unit movement: {3}", _outputHelper.GetPlayerName(command.Player), cmd.Position.X, cmd.Position.Y, moveingUnit.Movement));

                success = currentGame.Map.MoveUnit(cmd.Player, cmd.Unit, cmd.Position.X, cmd.Position.Y);
                if (!success)
                {
                    callbackList[curretPlayerId].ActionResult(command, false);
                    Console.WriteLine(string.Format("{0} is movement is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                    return;
                }
                Console.WriteLine(string.Format("{0} is movement is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                Console.WriteLine(string.Format("{0} is moving unit remaining movement: {1}", _outputHelper.GetPlayerName(command.Player), moveingUnit.Movement));
            }

            if (command is ConstructBuilding)
            {
                var cmd = command as ConstructBuilding;

                Console.WriteLine(string.Format("{0} is placing building to X:{1} Y:{2}. Gold before: {3}", _outputHelper.GetPlayerName(command.Player), cmd.Position.X, cmd.Position.Y, command.Player.Gold));

                success = currentGame.Map.AddBuilding(cmd.Player, cmd.Building, cmd.Position.X, cmd.Position.Y);
                if (!success)
                {
                    callbackList[curretPlayerId].ActionResult(command, false);
                    Console.WriteLine(string.Format("{0}'s placement is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                    return;
                }
                Console.WriteLine(string.Format("{0} is placement is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                Console.WriteLine(string.Format("{0} has placed a building remaining gold: {1}", _outputHelper.GetPlayerName(command.Player), command.Player.Gold));
            }

            if (command is CreateUnit)
            {
                var cmd = command as CreateUnit;

                Console.WriteLine(string.Format("{0} is creating unit at X:{1} Y:{2}. Gold before: {3}", _outputHelper.GetPlayerName(command.Player), cmd.Position.X, cmd.Position.Y, command.Player.Gold));

                success = currentGame.Map.AddUnit(cmd.Player, cmd.Unit, cmd.Position.X, cmd.Position.Y);
                if (!success)
                {
                    callbackList[curretPlayerId].ActionResult(command, false);
                    Console.WriteLine(string.Format("{0}'s creation is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                    return;
                }
                Console.WriteLine(string.Format("{0}'s creation is: {1}", _outputHelper.GetPlayerName(command.Player), success.ToString()));
                Console.WriteLine(string.Format("{0} has created a unit remaining gold: {1}", _outputHelper.GetPlayerName(command.Player), command.Player.Gold));
            }


            callbackList[curretPlayerId].ActionResult(command, true);

            // Notify other player
            switch (gameStateController.CurreState)
            {
                case GameState.GameState.State.PlayerOneTurn:
                    callbackList[gameStateController.PlayerTwo.PlayerId].Update(command);
                    Console.WriteLine(string.Format("{0} is Updated!", _outputHelper.GetPlayerName(gameStateController.PlayerTwo)));
                    break;
                case GameState.GameState.State.PlayerTwoTurn:
                    callbackList[gameStateController.PlayerOne.PlayerId].Update(command);
                    Console.WriteLine(string.Format("{0} is Updated!", _outputHelper.GetPlayerName(gameStateController.PlayerOne)));
                    break;
            }
        }

    }
}

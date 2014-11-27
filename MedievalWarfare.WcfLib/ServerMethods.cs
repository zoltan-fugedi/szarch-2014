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
        private DebugHelper debugHelper;

        public ServerMethods()
        {
            callbackList = new ConcurrentDictionary<Guid, IClientCallback>();
            currentGame = new Game();
            currentGame.Map.GenerateMap();
            gameStateController = new GameStateController();
            debugHelper = new DebugHelper(gameStateController);
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
            if (callbackList.TryRemove(info.PlayerId, out callBack))
            {
            }
            else
            {
            }

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
                Console.WriteLine(string.Format("{0} is ended the turn!", debugHelper.GetPlayerName(info)));
                currentGame.EndPlayerTurn(info);
                gameStateController.NextState();
                callbackList[gameStateController.CurrentPlayer.PlayerId].StartTurn();
                Console.WriteLine(string.Format("{0} is started the turn!", debugHelper.GetPlayerName(gameStateController.CurrentPlayer)));

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
                Console.WriteLine(string.Format("{0} is moving unit to X:{1} Y:{2}. Unit movement: {3}", debugHelper.GetPlayerName(command.Player), cmd.Position.X, cmd.Position.Y, moveingUnit.Movement));

                success = currentGame.Map.MoveUnit(cmd.Player, cmd.Unit, cmd.Position.X, cmd.Position.Y);
                if (!success)
                {
                    callbackList[curretPlayerId].ActionResult(command, false);
                    return;
                }
                Console.WriteLine(string.Format("{0} is moving unit remaining movement: {1}", debugHelper.GetPlayerName(command.Player), moveingUnit.Movement));
            }

            callbackList[curretPlayerId].ActionResult(command, true);


            // Notify other player
            switch (gameStateController.CurreState)
            {
                case GameState.GameState.State.PlayerOneTurn:
                    callbackList[gameStateController.PlayerTwo.PlayerId].Update(command);
                    Console.WriteLine(string.Format("{0} is Updated!", debugHelper.GetPlayerName(gameStateController.PlayerTwo)));
                    break;
                case GameState.GameState.State.PlayerTwoTurn:
                    callbackList[gameStateController.PlayerOne.PlayerId].Update(command);
                    Console.WriteLine(string.Format("{0} is Updated!", debugHelper.GetPlayerName(gameStateController.PlayerOne)));
                    break;
            }
        }

    }
}

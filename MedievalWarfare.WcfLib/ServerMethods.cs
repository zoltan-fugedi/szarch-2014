using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
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

        public ServerMethods()
        {
            callbackList = new ConcurrentDictionary<Guid, IClientCallback>();
            currentGame = new Game();
            currentGame.Map.GenerateMap();
            gameStateController = new GameStateController();

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
                        currentGame.Map.AddNewPlayerObjects(6, 6, info);
                    else
                        currentGame.Map.AddNewPlayerObjects(2, 2, info);
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
                gameStateController.CurrentPlayerTurnEnded = true;
                currentGame.EndPlayerTurn(currentGame.GetPlayer(info.PlayerId));
                gameStateController.NextState();
                callbackList[gameStateController.CurrentPlayer.PlayerId].StartTurn();
            }

        }

        public void UpdateMap(Command command)
        {
            bool success = false;
            var curretPlayerId = command.Player.PlayerId;

            if (command is MoveUnit)
            {
                var cmd = command as MoveUnit;
                success = currentGame.Map.MoveUnit(cmd.Player, cmd.Unit, cmd.Position.X, cmd.Position.Y);
                if (!success)
                {
                    callbackList[curretPlayerId].ActionResult(command, false);
                    return;
                }


            }

            callbackList[curretPlayerId].ActionResult(command, true);


            // Notify other player
            switch (gameStateController.CurreState)
            {
                case GameState.GameState.State.PlayerOneTurn:
                    callbackList[gameStateController.PlayerTwo.PlayerId].Update(command);
                    break;
                case GameState.GameState.State.PlayerTwoTurn:
                    callbackList[gameStateController.PlayerOne.PlayerId].Update(command);
                    break;
            }
        }

    }
}

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

            if (gameStateController.CurreState == (GameState.GameState.State.WaitingForJoin | GameState.GameState.State.PlayerOneJoined))
            {
                // Subscribe the user to the conversation

                if (callbackList.TryAdd(info.PlayerId, registeredUser))
                {
                    currentGame.AddPlayer(info);
                    currentGame.Map.AddNewPlayerObjects(2, 2, info);
                    registeredUser.ActionResult(true);
                    gameStateController.CurrentPlayer = info;
                    gameStateController.NextState();
                }

                if (gameStateController.CurreState == GameState.GameState.State.PlayerTwoJoined)
                {
                    gameStateController.NextState();
                    var currentPlayerId = gameStateController.CurrentPlayer.PlayerId;
                    callbackList[currentPlayerId].StartTurn();
                }

            }
            registeredUser.ActionResult(false);
        }

        public void Leave(Player info)
        {
            IClientCallback callBack;
            if (callbackList.TryRemove(info.PlayerId, out callBack))
            {
                callBack.ActionResult(true);
            }
            else
            {
                callBack.ActionResult(false);
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
                gameStateController.CurreState == (GameState.GameState.State.PlayerOneTurn | GameState.GameState.State.PlayerTwoTurn))
            {
                gameStateController.CurrentPlayerTurnEnded = true;
                // TODO do shits
                gameStateController.NextState();
            }

        }

        public void UpdateMap(Command command)
        {
            if (command is MoveUnit)
            {
               var cmd= command as MoveUnit;
               currentGame.Map.MoveUnit(cmd.Player, cmd.Unit, cmd.Position.X, cmd.Position.X);
            }

        }

    }
}

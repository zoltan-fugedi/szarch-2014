using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;

namespace MedievalWarfare.WcfLib.GameState
{
    class GameStateController
    {
        public GameState.State CurreState { get; private set; }

        public bool CurrentPlayerTurnEnded { get; set; }
        public bool GameEndFlag { get; set; }

        public Player CurrentPlayer
        {
            get
            {
                switch (CurreState)
                {
                    case GameState.State.WaitingForJoin:
                        return PlayerOne;
                    case GameState.State.PlayerOneJoined:
                        return PlayerTwo;
                    case GameState.State.PlayerOneTurn:
                        return PlayerOne;
                    case GameState.State.PlayerTwoTurn:
                        return PlayerTwo;
                    default:
                        return null;
                }
            }

            set
            {
                switch (CurreState)
                {
                    case GameState.State.WaitingForJoin:
                        PlayerOne = value;
                        break;
                    case GameState.State.PlayerOneJoined:
                        PlayerTwo = value;
                        break;
                }
            }

        }

        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }

        public GameStateController()
        {
            CurreState = GameState.State.WaitingForJoin;
        }

        public void NextState()
        {
            switch (CurreState)
            {
                case GameState.State.WaitingForJoin:
                    if (PlayerOne != null)
                    {
                        CurreState = GameState.State.PlayerOneJoined;
                    }
                    break;
                case GameState.State.PlayerOneJoined:
                    if (PlayerTwo != null)
                    {
                        CurreState = GameState.State.PlayerTwoJoined;
                    }
                    break;
                case GameState.State.PlayerTwoJoined:
                    CurreState = GameState.State.PlayerOneTurn;
                    CurrentPlayerTurnEnded = false;
                    break;
                case GameState.State.PlayerOneTurn:
                    if (CurrentPlayerTurnEnded && !GameEndFlag)
                    {
                        CurreState = GameState.State.PlayerTwoTurn;
                        CurrentPlayerTurnEnded = false;
                    }
                    if (GameEndFlag)
                    {
                        // TODO
                    }
                    break;
                case GameState.State.PlayerTwoTurn:
                    if (CurrentPlayerTurnEnded && !GameEndFlag)
                    {
                        CurreState = GameState.State.PlayerOneTurn;
                        CurrentPlayerTurnEnded = false;
                    }
                    if (GameEndFlag)
                    {
                        // TODO
                    }
                    break;
                case GameState.State.PlayerOneWin:
                    break;
                case GameState.State.PlayerTwoWin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

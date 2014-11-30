using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedievalWarfare.Common;

namespace MedievalWarfare.WcfLib.GameState
{
    public class GameState
    {
        public enum State
        {
            WaitingForJoin,
            PlayerOneJoined,
            PlayerTwoJoined,
            GameStart,
            PlayerOneTurn,
            PlayerTwoTurn,
            PlayerWins,
            PlayerLeft,

        }

        public Player CurretnPlayer { get; set; }
    }
}

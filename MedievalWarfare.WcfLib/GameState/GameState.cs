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
        [Flags]
        public enum State
        {
            WaitingForJoin,
            PlayerOneJoined,
            PlayerTwoJoined,
            PlayerOneTurn,
            PlayerTwoTurn,
            PlayerOneWin,
            PlayerTwoWin,

        }

        public Player CurretnPlayer { get; set; }
    }
}

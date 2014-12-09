using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.TestClient.Proxy;
using MedievalWarfare.TestClient.Utils;
using MedievalWarfare.TestClient.VM;
using Player = MedievalWarfare.Common.Player;

namespace MedievalWarfare.TestClient
{
    class MainWindowVm : VmBase
    {

        #region Accessors

        public App CurrentApp { get { return Application.Current as App; } }

        public IServerMethods CurrentPlayerMethods
        {
            get { return PlayerOneTurn ? CurrentApp.PlayerOne : CurrentApp.PlayerTwo; }
        }

        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }

        public Player CurrentPlayer
        {
            get { return PlayerOneTurn ? PlayerOne : PlayerTwo; }
        }

        #endregion

        #region Props

        public bool PlayerOneTurn { get; set; }

        #endregion

        #region Commands

        public AsyncCommand ConnectCommand { get; set; }
        public RelayCommand TurnEndCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }

        private void InitMenuCommands()
        {
            ConnectCommand = new AsyncCommand(
                async () =>
                {
                    IsConnected = true;

                    PlayerOne = new Player();
                    PlayerTwo = new Player();

                    CurrentApp.PlayerOne.Open();
                    await CurrentApp.PlayerOne.JoinAsync(PlayerOne);
                    CurrentApp.PlayerTwo.Open();
                    await CurrentApp.PlayerTwo.JoinAsync(PlayerTwo);

                    PlayerOneTurn = true;
                });

            TurnEndCommand = new RelayCommand((arg) => CurrentPlayerMethods.EndTurn(CurrentPlayer));

            ExitCommand = new RelayCommand((arg) =>
                CurrentApp.Shutdown());
        }

        #endregion


        public bool IsConnected { get; set; }

        public MainWindowVm()
        {
            IsConnected = false;
            InitMenuCommands();
        }


    }
}

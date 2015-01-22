using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using MedievalWarfare.TestClient.Proxy;
using MedievalWarfare.TestClient.Utils;
using MedievalWarfare.TestClient.View;
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

        public Player CurrentPlayer { get { return PlayerOneTurn ? PlayerOne : PlayerTwo; } }

        #endregion

        #region Props

        public bool PlayerOneTurn { get; set; }
        public ObjectListVM unitVM { get; set; }

        public GameObject SelectedGameObject { get; set; }


        #endregion

        #region Commands

        public AsyncCommand ConnectCommand { get; set; }
        public RelayCommand TurnEndCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public AsyncCommand DisconnectCommand { get; set; }

        private void InitMenuCommands()
        {
            ConnectCommand = new AsyncCommand(
                async () =>
                {
                    CurrentApp.PlayerOne.Open();
                    await CurrentApp.PlayerOne.JoinAsync(PlayerOne);
                    CurrentApp.PlayerTwo.Open();
                    await CurrentApp.PlayerTwo.JoinAsync(PlayerTwo);

                    PlayerOneTurn = true;

                    IsConnected = true;
                    OnPropertyChanged("IsConnected");

                    CurrentApp.Callbacks.Map.ObjectList.ForEach(unitVM.objectList.Add);
                });

            TurnEndCommand = new RelayCommand((arg) => CurrentPlayerMethods.EndTurn(CurrentPlayer));

            ExitCommand = new RelayCommand((arg) => CurrentApp.Shutdown());
        }

        #endregion


        public bool IsConnected { get; set; }

        public MainWindowVm()
        {
            PlayerOne = new Player();
            PlayerTwo = new Player();
            IsConnected = false;
            unitVM = new ObjectListVM(PlayerOne.PlayerId);
            InitMenuCommands();
        }

        public async Task Disconnec()
        {
            await CurrentPlayerMethods.LeaveAsync(CurrentPlayer);
        }
    }
}

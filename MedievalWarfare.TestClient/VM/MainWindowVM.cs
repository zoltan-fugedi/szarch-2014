using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using MedievalWarfare.TestClient.Db;
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

        public DbManager DbManager { get; set; }

        private bool _playerOneTurn;

        public bool PlayerOneTurn
        {
            get { return _playerOneTurn; }
            set
            {
                _playerOneTurn = value;
                OnPropertyChanged();
            }
        }

        public ObjectListVM UnitVm { get; set; }

        public GameObject SelectedGameObject { get; set; }


        #endregion

        #region Commands

        public AsyncCommand ConnectCommand { get; set; }
        public AsyncCommand TurnEndCommand { get; set; }
        public AsyncCommand InitDb { get; set; }
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

                    PlayerOneTurn = CurrentApp.Callbacks.PlayerOneTurn;

                    IsConnected = true;
                    OnPropertyChanged("IsConnected");

                    CurrentApp.Callbacks.Map.ObjectList.ForEach(UnitVm.objectList.Add);
                }, () => !IsConnected);

            InitDb = new AsyncCommand(async () =>
            {
                await DbManager.InitDbAsync(CurrentApp.Callbacks.Map);
                await DbManager.AddPlayerAsync(PlayerOne);
                await DbManager.AddPlayerAsync(PlayerTwo);
            }, () => IsConnected);

            TurnEndCommand = new AsyncCommand(async () =>
            {
                await CurrentPlayerMethods.EndTurnAsync(CurrentPlayer);

            });

            ExitCommand = new RelayCommand((arg) => CurrentApp.Shutdown());
        }

        #endregion


        public bool IsConnected { get; set; }

        public MainWindowVm()
        {
            PlayerOne = new Player { Name = "PlayerOne" };
            PlayerTwo = new Player { Name = "PlayerTwo" };
            DbManager = new DbManager();
            IsConnected = false;
            UnitVm = new ObjectListVM(PlayerOne.PlayerId);
            InitMenuCommands();
        }

        public async Task Disconnec()
        {
            await CurrentPlayerMethods.LeaveAsync(CurrentPlayer);
        }
    }
}

using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.Common;
using MedievalWarfare.TestClient.Db;
using MedievalWarfare.TestClient.Proxy;
using MedievalWarfare.TestClient.Utils;
using MedievalWarfare.TestClient.View;

namespace MedievalWarfare.TestClient.VM
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

        private GameObject _selectedGameObject;
        public GameObject SelectedGameObject
        {
            get { return _selectedGameObject; }
            set
            {
                _selectedGameObject = value;
                OnPropertyChanged();
            }
        }

        public bool IsObjectSelected { get; set; }

        #endregion

        #region Commands

        public AsyncCommand ConnectCommand { get; set; }
        public AsyncCommand TurnEndCommand { get; set; }
        public AsyncCommand InitDb { get; set; }
        public AsyncCommand PrintHistory { get; set; }
        public RelayCommand ExitCommand { get; set; }

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

                    CurrentApp.Callbacks.Map.ObjectList.ForEach(UnitVm.objectList.Add);
                }, () => !IsConnected);

            InitDb = new AsyncCommand(async () =>
            {
                await DbManager.InitDbAsync(CurrentApp.Callbacks.Map);
                await DbManager.AddPlayerAsync(PlayerOne);
                await DbManager.AddPlayerAsync(PlayerTwo);
            }, () => IsConnected);

            PrintHistory = new AsyncCommand(async () =>
            {
                var historyView = new HistoryView(DbManager);
                historyView.Show();
            });

            TurnEndCommand = new AsyncCommand(async () =>
            {
                await CurrentPlayerMethods.EndTurnAsync(CurrentPlayer);
                PlayerOneTurn = !PlayerOneTurn;
                UnitVm.Id = CurrentPlayer.PlayerId;

            }, () => IsConnected);

            ExitCommand = new RelayCommand((arg) => CurrentApp.Shutdown());
        }

        #endregion


        public bool IsConnected { get; set; }

        public MainWindowVm()
        {
            PlayerOne = new Player { Name = "PlayerOne" };
            PlayerTwo = new Player { Name = "PlayerTwo" };
            DbManager = new DbManager();
            UnitVm = new ObjectListVM(PlayerOne.PlayerId);
            IsConnected = false;
            InitMenuCommands();
        }

        public async Task Disconnec()
        {
            await CurrentPlayerMethods.LeaveAsync(CurrentPlayer);
        }
    }
}

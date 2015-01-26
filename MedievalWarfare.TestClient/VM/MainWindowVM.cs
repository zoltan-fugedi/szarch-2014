using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using MedievalWarfare.TestClient.Db;
using MedievalWarfare.TestClient.Proxy;
using MedievalWarfare.TestClient.Utils;
using MedievalWarfare.TestClient.View;

namespace MedievalWarfare.TestClient.VM
{
    class MainWindowVm : VmBase, Proxy.IServerMethodsCallback
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

        private bool _clientResponse;
        public bool ClientResponse
        {
            get { return _clientResponse; }
            set
            {
                _clientResponse = value;
                OnPropertyChanged();
            }
        }

        private bool _serverResponse;
        public bool ServerResponse
        {
            get { return _serverResponse; }
            set
            {
                _serverResponse = value;
                OnPropertyChanged();
            }
        }

        private int _untiX;
        public int UnitX
        {
            get { return _untiX; }
            set
            {
                _untiX = value;
                OnPropertyChanged();
            }
        }

        private int _untiY;
        public int UnitY
        {
            get { return _untiY; }
            set
            {
                _untiY = value;
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
        public AsyncCommand SendCommand { get; set; }
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

                    Map.ObjectList.ForEach(UnitVm.objectList.Add);

                    ServerResponse = false;
                    ClientResponse = false;

                }, () => !IsConnected);

            InitDb = new AsyncCommand(async () => await DbManager.InitDbAsync(Map), () => IsConnected);

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

            SendCommand = new AsyncCommand(async () =>
            {
                if (UnitX > 0 && UnitY > 0)
                {
                    var command = new MoveUnit
                    {
                        Position = new Tile(UnitX, UnitY, Map),
                        Unit = SelectedGameObject as Unit,
                        Player = CurrentPlayer
                    };
                    await CurrentPlayerMethods.UpdateMapAsync(command);
                    ClientResponse = Map.MoveUnit(CurrentPlayer, SelectedGameObject as Unit, UnitX, UnitY);
                   await  DbManager.AddCommandAsync(command);
                    var a = 11;
                }
            }, () => IsConnected);

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
            ServerResponse = true;
            ClientResponse = true;
            InitMenuCommands();
        }

        public async Task Disconnec()
        {
            await CurrentPlayerMethods.LeaveAsync(CurrentPlayer);
        }

        #region callbacks

        public Map Map { get; set; }


        public void ActionResult(Command command, bool result, string msg)
        {
            ServerResponse = result;
        }

        public void StartGame(Game game, bool isYourTurn)
        {
            Map = game.Map;
        }

        public void StartTurn()
        {

        }

        public void Update(Command command)
        {
        }

        public void EndGame(bool winner)
        {
        }

        #endregion
    }
}

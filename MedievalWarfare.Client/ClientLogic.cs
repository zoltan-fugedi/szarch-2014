using MedievalWarfare.Client.Proxy;
using MedievalWarfare.Common;
using MedievalWarfare.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MedievalWarfare.Client
{
    public enum GameStates 
    { 
        Init,
        Connected,
        TurnStarted,
        TurnEnded,
        Victory,
        Defeat
    }

    public enum Selection 
    { 
        None,
        FUnit,
        FBuilding,
        EUnit,
        EBuilding,
    }


    public class ClientLogic : Proxy.IServerMethodsCallback, INotifyPropertyChanged
    {
        public GameStates clientState;
        public GameStates ClientState
        {
            get { return clientState; }
            set
            {
                clientState = value;
                OnPropertyChanged("ClientState");

            }
        }

        Selection selection;
        public Selection Selection
        {
            get { return selection; }
            set
            {
                selection = value;
                OnPropertyChanged("Selection");

            }
        }
        public aObject SelectedObject { get; set; }

        Unit selectedUnit;

        public Unit SelectedUnit
        {
            get { return selectedUnit; }
            set
            {
                selectedUnit = value;
                OnPropertyChanged("SelectedUnit");
            }
        }

        Building selectedBuilding;

        public Building SelectedBuilding
        {
            get { return selectedBuilding; }
            set
            {
                selectedBuilding = value;
                OnPropertyChanged("SelectedBuilding");
            }
        }

        Player player;

        public Player Player
        {
            get { return player; }
            set
            {
                player = value;
                OnPropertyChanged("Player");
            }
        }
        public bool WaitingForReply { get; set; }
        public aHexMap MyMap { get; set; }
        public Game Game { get; set; }

        public MainWindow window;
        public event PropertyChangedEventHandler PropertyChanged;

        private String _message;
        private Proxy.ServerMethodsClient proxy;

        public String Message
        {
            get{ return _message;}
            set{
                if (value != _message)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }


        public ClientLogic(MainWindow window)
        {
            proxy = new ServerMethodsClient(new InstanceContext(this));
            Player = new Player();
            Game = new Game();
            this.window = window;
            Message = "Connect to the Server via the Main Menu";
            ClientState = GameStates.Init;
            this.Selection = Selection.None;
            WaitingForReply = false;
            
        }


        #region Communication: Client --> Server


        public void ConnectToServer() 
        {

                proxy.Open();
                proxy.JoinAsync(Player);

                ClientState = GameStates.Connected;
                Message = "Connected To server, waiting for other player";
            
        }

        public void CreateBuilding() 
        {
            if (Selection == Selection.FUnit) 
            {
                
                var com = new ConstructBuilding();
                com.Player = Player;
                com.Position = selectedUnit.Tile;
                proxy.UpdateMapAsync(com);
                SelectedObject = null;
                Selection = Selection.None;
                MyMap.removeRangeIndicator();
                WaitingForReply = true;
            }
        }
     
        public void CreateUnit()
        {

        }
        public void MoveUnit(Tile dest, Unit unit)
        {
            var com = new MoveUnit();
            com.Player = Player;
            com.Position = dest;
            com.Unit = unit;
            proxy.UpdateMapAsync(com);
            SelectedObject = null;
            Selection = Selection.None;
            MyMap.removeRangeIndicator();
            WaitingForReply = true;

        }
        public async void EndTurn() 
        {
            if (Selection != Selection.None) {
                MyMap.updateObject(SelectedObject);
                SelectedObject = null;
                Selection = Selection.None;
                MyMap.removeRangeIndicator();
            } 
            await proxy.EndTurnAsync(Player);
            Game.EndPlayerTurn(Game.GetPlayer(Player.PlayerId));
            Player.Gold = Game.GetPlayer(Player.PlayerId).Gold;
            ClientState = GameStates.TurnEnded;
            Message = "The Other player is moving";
        }
        public async void Exit()
        {
            ClientState = GameStates.Defeat;
            Message = "Closed the client";
            await proxy.LeaveAsync(Player);
        }

        

        #endregion

        #region Communication: Server --> Client

        public void ActionResult(Common.Utility.Command command, bool result, string msg)
        {
            if (WaitingForReply) 
            {
                if (result) 
                {
                    if (command is MoveUnit)
                    {
                        var clientresult = Game.Map.MoveUnit(command.Player, ((MoveUnit)command).Unit, ((MoveUnit)command).Position.X, ((MoveUnit)command).Position.Y);
                        if (!clientresult)
                        {
                            Game = proxy.GetGameState();
                            MyMap = new aHexMap(this, window);
                            window.mapCanvas.Children.Add(MyMap);
                        }
                        MyMap.drawMap(Player);
                        WaitingForReply = false;
                        Message = "Successful move";
                    }
                    if (command is ConstructBuilding) 
                    {
                        var clientresult = Game.Map.AddBuilding(((ConstructBuilding)command).Position.X, ((ConstructBuilding)command).Position.Y, Player, false);
                        if (!clientresult)
                        {
                            Game = proxy.GetGameState();
                            MyMap = new aHexMap(this, window);
                            window.mapCanvas.Children.Add(MyMap);
                        }
                        MyMap.drawMap(Player);
                        WaitingForReply = false;
                        Message = "Successful move";
                    }
                }
                else
                {
                    MyMap.drawMap(Player);
                    WaitingForReply = false;
                    Message = "Failed move";
                }

                Player.Gold = Game.GetPlayer(Player.PlayerId).Gold;
            }
            
        }

        public void StartGame(Game game, bool isYourTurn)
        {
            Game = game;
            Player = Game.GetPlayer(Player.PlayerId);
            MyMap = new aHexMap(this, window);
            window.mapCanvas.Children.Add(MyMap);
            MyMap.drawMap(Player);
            if (isYourTurn)
            {
                ClientState = GameStates.TurnStarted;
                Message = "It is your turn to move";
            }else
	        {
                ClientState = GameStates.TurnEnded;
                Message = "The Other player is moving";
	        }
        }
        
        public void StartTurn()
        {
            Game.EndPlayerTurn(Game.Players.Single(go => go.PlayerId != Player.PlayerId && !go.Neutral));
            MyMap.drawMap(Player);
            Player.Gold = Game.GetPlayer(Player.PlayerId).Gold;
            ClientState = GameStates.TurnStarted;
            Message = "It is your turn to move";
        }
        public void Update(Common.Utility.Command command)
        {
           
                if (command is MoveUnit)
                {
                    var result =Game.Map.MoveUnit(command.Player, ((MoveUnit)command).Unit, ((MoveUnit)command).Position.X, ((MoveUnit)command).Position.Y);
                    if (!result) 
                    {
                        Game = proxy.GetGameState();
                        MyMap = new aHexMap(this, window);
                        window.mapCanvas.Children.Add(MyMap);
                    }
                    MyMap.drawMap(Player);
                }
                if (command is ConstructBuilding)
                {
                    var clientresult = Game.Map.AddBuilding(((ConstructBuilding)command).Position.X, ((ConstructBuilding)command).Position.Y, command.Player, false);
                    if (!clientresult)
                    {
                        Game = proxy.GetGameState();
                        MyMap = new aHexMap(this, window);
                        window.mapCanvas.Children.Add(MyMap);
                    }
                    MyMap.drawMap(Player);
              
                }
                Player.Gold = Game.GetPlayer(Player.PlayerId).Gold;
                ClientState = GameStates.TurnEnded;
                Message = "The Other player is moving";

            
            

        }
        public void EndGame(bool winner)
        {
            switch (winner)
            {
                case true :
                    ClientState = GameStates.Victory;
                    Message = "You're Winner";
                    break;
                case false :
                    ClientState = GameStates.Defeat;
                    Message = "Defeat";
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Selection Mgmt

        public void ManageObjectSelection(aObject sel) 
        {
            SelectedUnit = null;
            SelectedBuilding = null;
            if (ClientState == GameStates.TurnStarted &&!WaitingForReply) 
            {
                if (Selection == Selection.None)
                {
                    if (sel.GameObject is Unit)
                    {
                        if (sel.GameObject.Owner.PlayerId == Player.PlayerId)
                        {
                            SelectedObject = sel;
                            MyMap.updateObject(SelectedObject);
                            MyMap.drawRangeIndicator(sel.GameObject);
                            SelectedUnit = (Unit)sel.GameObject;
                            Selection = Selection.FUnit;
                            Message = "Selected a firendly unit";
                        }
                        else
                        {
                            SelectedObject = sel;
                            MyMap.updateObject(SelectedObject);
                            SelectedUnit = (Unit)sel.GameObject;
                            Selection = Selection.EUnit;
                            Message = "Selected an enemy unit";
                        }
                    }
                    if (sel.GameObject is Building)
                    {
                        if (sel.GameObject.Owner.PlayerId == Player.PlayerId)
                        {
                            SelectedObject = sel;
                            MyMap.updateObject(SelectedObject);
                            Selection = Selection.FBuilding;
                            SelectedBuilding = (Building)sel.GameObject;
                            Message = "Selected a friendly building";
                        }
                        else
                        {
                            SelectedObject = sel;
                            MyMap.updateObject(SelectedObject);
                            Selection = Selection.EBuilding;
                            SelectedBuilding = (Building)sel.GameObject;
                            Message = "Selected an enemy building";
                        }
                    }
                }
                else
                {
                    //deselect
                    if (sel.Equals(SelectedObject))
                    {
                        MyMap.updateObject(SelectedObject);
                        SelectedObject = null;
                        Selection = Selection.None;
                        MyMap.removeRangeIndicator();
                        Message = "It is your turn to move";
                    }
                    //select different
                    else
                    {
                        if (sel.GameObject is Unit)
                        {
                            if (sel.GameObject.Owner.PlayerId == Player.PlayerId)
                            {
                                MyMap.updateObject(SelectedObject);
                                MyMap.removeRangeIndicator();
                                SelectedObject = sel;
                                MyMap.updateObject(SelectedObject);
                                MyMap.drawRangeIndicator(sel.GameObject);
                                SelectedUnit = (Unit)sel.GameObject;
                                Selection = Selection.FUnit;
                                Message = "Selected a firendly unit";
                            }
                            else
                            {
                                MyMap.updateObject(SelectedObject);
                                MyMap.removeRangeIndicator();
                                SelectedObject = sel;
                                MyMap.updateObject(SelectedObject);
                                SelectedUnit = (Unit)sel.GameObject;
                                Selection = Selection.EUnit;
                                Message = "Selected an enemy unit";
                            }
                        }
                        if (sel.GameObject is Building)
                        {
                            if (sel.GameObject.Owner.PlayerId == Player.PlayerId)
                            {
                                MyMap.updateObject(SelectedObject);
                                MyMap.removeRangeIndicator();
                                SelectedObject = sel;
                                MyMap.updateObject(SelectedObject);
                                SelectedBuilding = (Building)sel.GameObject;
                                Selection = Selection.FBuilding;
                                Message = "Selected a friendly building";
                            }
                            else
                            {
                                MyMap.updateObject(SelectedObject);
                                MyMap.removeRangeIndicator();
                                SelectedObject = sel;
                                MyMap.updateObject(SelectedObject);
                                SelectedBuilding = (Building)sel.GameObject;
                                Selection = Selection.EBuilding;
                                Message = "Selected an enemy building";
                            }
                        }
                    }
                }
            } 
        }
        public void ManageTileSelection(aHex sel)
        {
            if (ClientState == GameStates.TurnStarted && !WaitingForReply)
            {
                switch (Selection)
                {
                    case Selection.None:
                        break;
                    case Selection.FUnit:
                        var tile = sel.Tile;
                        Unit go = (Unit)SelectedObject.GameObject;
                        SelectedObject = null;
                        Selection = Selection.None;
                        MoveUnit(tile, go);
                        break;
                    case Selection.FBuilding:
                        break;
                    case Selection.EUnit:
                        
                        break;
                    case Selection.EBuilding:
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }




    }
}

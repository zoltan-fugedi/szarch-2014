﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedievalWarfare.TestClient.Proxy {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Proxy.IServerMethods", CallbackContract=typeof(MedievalWarfare.TestClient.Proxy.IServerMethodsCallback))]
    public interface IServerMethods {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/Join", ReplyAction="http://tempuri.org/IServerMethods/JoinResponse")]
        void Join(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/Join", ReplyAction="http://tempuri.org/IServerMethods/JoinResponse")]
        System.Threading.Tasks.Task JoinAsync(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/Leave", ReplyAction="http://tempuri.org/IServerMethods/LeaveResponse")]
        void Leave(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/Leave", ReplyAction="http://tempuri.org/IServerMethods/LeaveResponse")]
        System.Threading.Tasks.Task LeaveAsync(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/GetGameState", ReplyAction="http://tempuri.org/IServerMethods/GetGameStateResponse")]
        MedievalWarfare.Common.Game GetGameState();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/GetGameState", ReplyAction="http://tempuri.org/IServerMethods/GetGameStateResponse")]
        System.Threading.Tasks.Task<MedievalWarfare.Common.Game> GetGameStateAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/EndTurn", ReplyAction="http://tempuri.org/IServerMethods/EndTurnResponse")]
        void EndTurn(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/EndTurn", ReplyAction="http://tempuri.org/IServerMethods/EndTurnResponse")]
        System.Threading.Tasks.Task EndTurnAsync(MedievalWarfare.Common.Player info);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/UpdateMap", ReplyAction="http://tempuri.org/IServerMethods/UpdateMapResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.CreateUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.MoveUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.ConstructBuilding))]
        void UpdateMap(MedievalWarfare.Common.Utility.Command command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerMethods/UpdateMap", ReplyAction="http://tempuri.org/IServerMethods/UpdateMapResponse")]
        System.Threading.Tasks.Task UpdateMapAsync(MedievalWarfare.Common.Utility.Command command);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerMethodsCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServerMethods/ActionResult")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.CreateUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.MoveUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.ConstructBuilding))]
        void ActionResult(MedievalWarfare.Common.Utility.Command command, bool result, string msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServerMethods/StartGame")]
        void StartGame(MedievalWarfare.Common.Game game, bool isYourTurn);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServerMethods/StartTurn")]
        void StartTurn();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServerMethods/Update")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.CreateUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.MoveUnit))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MedievalWarfare.Common.Utility.ConstructBuilding))]
        void Update(MedievalWarfare.Common.Utility.Command command);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IServerMethods/EndGame")]
        void EndGame(bool winner);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerMethodsChannel : MedievalWarfare.TestClient.Proxy.IServerMethods, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerMethodsClient : System.ServiceModel.DuplexClientBase<MedievalWarfare.TestClient.Proxy.IServerMethods>, MedievalWarfare.TestClient.Proxy.IServerMethods {
        
        public ServerMethodsClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServerMethodsClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServerMethodsClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerMethodsClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerMethodsClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Join(MedievalWarfare.Common.Player info) {
            base.Channel.Join(info);
        }
        
        public System.Threading.Tasks.Task JoinAsync(MedievalWarfare.Common.Player info) {
            return base.Channel.JoinAsync(info);
        }
        
        public void Leave(MedievalWarfare.Common.Player info) {
            base.Channel.Leave(info);
        }
        
        public System.Threading.Tasks.Task LeaveAsync(MedievalWarfare.Common.Player info) {
            return base.Channel.LeaveAsync(info);
        }
        
        public MedievalWarfare.Common.Game GetGameState() {
            return base.Channel.GetGameState();
        }
        
        public System.Threading.Tasks.Task<MedievalWarfare.Common.Game> GetGameStateAsync() {
            return base.Channel.GetGameStateAsync();
        }
        
        public void EndTurn(MedievalWarfare.Common.Player info) {
            base.Channel.EndTurn(info);
        }
        
        public System.Threading.Tasks.Task EndTurnAsync(MedievalWarfare.Common.Player info) {
            return base.Channel.EndTurnAsync(info);
        }
        
        public void UpdateMap(MedievalWarfare.Common.Utility.Command command) {
            base.Channel.UpdateMap(command);
        }
        
        public System.Threading.Tasks.Task UpdateMapAsync(MedievalWarfare.Common.Utility.Command command) {
            return base.Channel.UpdateMapAsync(command);
        }
    }
}

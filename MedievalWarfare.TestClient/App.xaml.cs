using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using MedievalWarfare.TestClient.Proxy;
using MedievalWarfare.TestClient.Utils;
using MedievalWarfare.TestClient.View;
using MedievalWarfare.TestClient.VM;

namespace MedievalWarfare.TestClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServerMethodsClient PlayerOne { get; set; }
        public ServerMethodsClient PlayerTwo { get; set; }

        public ServerCallbacks Callbacks { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Callbacks = new ServerCallbacks();

            PlayerOne = new ServerMethodsClient(new InstanceContext(Callbacks));
            PlayerTwo = new ServerMethodsClient(new InstanceContext(Callbacks));

            var mainView = new MainWindow();
            var mvm = new MainWindowVm();

            mainView.DataContext = mvm;
            mainView.Show();

            base.OnStartup(e);
        }
    }
}

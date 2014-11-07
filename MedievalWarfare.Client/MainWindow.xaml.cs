using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MedievalWarfare.Client.Proxy;

namespace MedievalWarfare.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window, Proxy.IServerMethodsCallback
    {
        private Proxy.ServerMethodsClient proxy;
        private bool igaz = false;

        public MainWindow()
        {
            InitializeComponent();
            proxy = new ServerMethodsClient(new InstanceContext(this));
            proxy.Open();
        }

        public void ActionResult(bool result)
        {
            igaz = result;
            var a = 10;
        }

        public void StartTurn(Proxy.MapInfo mapInfo)
        {
            throw new NotImplementedException();
        }

        public void Update(Proxy.Command cmd)
        {
            throw new NotImplementedException();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            proxy.Join(new PlayerInfo());
        }
    }
}

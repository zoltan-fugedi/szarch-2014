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
using MedievalWarfare.Common;
using System.ComponentModel;
using MedievalWarfare.Common.Utility;
using System.Globalization;


namespace MedievalWarfare.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window
    {

        public ClientLogic Logic { get; set; }

        /// <summary>
        /// 
        /// CTOR
        /// </summary>
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = this;
            Logic = new ClientLogic(this);

        }

  

        #region Event Handlers

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            Logic.Exit();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            Logic.Exit();
            base.OnClosed(e);

        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_connect_Click(object sender, RoutedEventArgs e)
        {
            Logic.ConnectToServer();
        }

        private void AddBuilding_Click(object sender, RoutedEventArgs e)
        {
            Logic.CreateBuilding();
        }

        private void AddUnit_Click(object sender, RoutedEventArgs e)
        {
            Logic.CreateUnit();
        }

        private void EndTurn_Click(object sender, RoutedEventArgs e)
        {
            Logic.EndTurn();
        }

        #endregion

        
    }
    public class UnitDataVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Selection enumVal = (Selection)Enum.Parse(typeof(Selection), value.ToString());
            switch (enumVal)
            {
                case Selection.None:
                    return Visibility.Hidden;
                   
                case Selection.FUnit:
                    return Visibility.Visible;
                    
                case Selection.FBuilding:
                    return Visibility.Hidden;
                    
                case Selection.EUnit:
                    return Visibility.Visible;
                  
                case Selection.EBuilding:
                    return Visibility.Hidden;
                 
                default:
                    return Visibility.Hidden;
            }      

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AddBuildingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Selection enumVal = (Selection)Enum.Parse(typeof(Selection), value.ToString());
            switch (enumVal)
            {
                case Selection.None:
                    return Visibility.Hidden;

                case Selection.FUnit:
                    return Visibility.Visible;

                case Selection.FBuilding:
                    return Visibility.Hidden;

                case Selection.EUnit:
                    return Visibility.Hidden;

                case Selection.EBuilding:
                    return Visibility.Hidden;

                default:
                    return Visibility.Hidden;
            } 
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class BuildingDataVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Selection enumVal = (Selection)Enum.Parse(typeof(Selection), value.ToString());
            switch (enumVal)
            {
                case Selection.None:
                    return Visibility.Hidden;

                case Selection.FUnit:
                    return Visibility.Hidden;

                case Selection.FBuilding:
                    return Visibility.Visible;

                case Selection.EUnit:
                    return Visibility.Hidden;

                case Selection.EBuilding:
                    return Visibility.Visible;

                default:
                    return Visibility.Hidden;
            } 
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AddUnitDataVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Selection enumVal = (Selection)Enum.Parse(typeof(Selection), value.ToString());
            switch (enumVal)
            {
                case Selection.None:
                    return Visibility.Hidden;

                case Selection.FUnit:
                    return Visibility.Hidden;

                case Selection.FBuilding:
                    return Visibility.Visible;

                case Selection.EUnit:
                    return Visibility.Hidden;

                case Selection.EBuilding:
                    return Visibility.Hidden;

                default:
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SidePanelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            GameStates enumVal = (GameStates)Enum.Parse(typeof(GameStates), value.ToString());
            switch (enumVal)
            {
                case GameStates.Init:
                    return Visibility.Hidden;
                case GameStates.Connected:
                    return Visibility.Hidden;
                case GameStates.TurnStarted:
                    return Visibility.Visible;
                case GameStates.TurnEnded:
                    return Visibility.Hidden;
                case GameStates.Victory:
                    return Visibility.Hidden;
                case GameStates.Defeat:
                    return Visibility.Hidden;
                default:
                    return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ConnectVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            GameStates enumVal = (GameStates)Enum.Parse(typeof(GameStates), value.ToString());
            switch (enumVal)
            {
                case GameStates.Init:
                    return true;
                case GameStates.Connected:
                    return false;
                case GameStates.TurnStarted:
                    return false;
                case GameStates.TurnEnded:
                    return false;
                case GameStates.Victory:
                    return true;
                case GameStates.Defeat:
                    return true;
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

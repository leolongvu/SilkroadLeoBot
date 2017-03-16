using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeoBot
{
    /// <summary>
    /// Interaction logic for PortConfigs.xaml
    /// </summary>
    public partial class PortConfigs : Window
    {
        public PortConfigs()
        {
            InitializeComponent();

            bot1.IsChecked = true;
            config1.IsChecked = true;
        }

        #region Metro Style

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        public static TrainingWindow TrainWindow = new TrainingWindow();

        private void Selectport(object sender, RoutedEventArgs e)
        {
            if (bot1.IsChecked == true)
            {
                Proxy.port = 20001;
            }
            else if (bot2.IsChecked == true)
            {
                Proxy.port = 20002;
            }
            else if (bot3.IsChecked == true)
            {
                Proxy.port = 20003;
            }
            else if (bot4.IsChecked == true)
            {
                Proxy.port = 20004;
            }

            if (config1.IsChecked == true)
            {
                Configs.config = 1;
            }
            else if (config2.IsChecked == true)
            {
                Configs.config = 2;
            }
            if (config3.IsChecked == true)
            {
                Configs.config = 3;
            }
            else if (config4.IsChecked == true)
            {
                Configs.config = 4;
            }

            MainWindow BotWindow = new MainWindow();
            BotWindow.Show();
           
            TrainWindow.Left = BotWindow.Left;
            TrainWindow.Top = BotWindow.Top + 480;

            this.Close();
        }
    }
}

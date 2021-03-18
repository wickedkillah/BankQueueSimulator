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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankQueueSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Simulator sim;

        public MainWindow()
        {
            InitializeComponent();

        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.Visibility = Visibility.Collapsed;
            btnStop.Visibility = Visibility.Visible;
            sim = new Simulator(int.Parse(tellers.Text),
                                int.Parse(minTimeToWork.Text),
                                int.Parse(maxTimeToWork.Text),
                                int.Parse(minTimeToArrival.Text),
                                int.Parse(maxTimeToArrival.Text),
                                this);
            await sim.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.Visibility = Visibility.Collapsed;
            btnStart.Visibility = Visibility.Visible;
            sim.Stop();
        }

        private void btnExi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

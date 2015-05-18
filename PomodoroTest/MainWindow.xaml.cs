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

namespace PomodoroTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :   Window
    {
        PomodoroTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void findTimerResource()
        {
            if (_timer == null)
            {
                _timer = FindResource("myTimer") as PomodoroTimer;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            findTimerResource();
            _timer.Start(PomodoroTimer.PomodoroTimeSpanType.FullPomodoro);

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            findTimerResource();
            _timer.Stop();

        }

        private void btnShortBreak_Click(object sender, RoutedEventArgs e)
        {
            findTimerResource();
            _timer.Start(PomodoroTimer.PomodoroTimeSpanType.ShortBreak);

        }

    }
}

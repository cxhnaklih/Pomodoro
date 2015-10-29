using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        IProgressDetailStorage _storage;
        private readonly SynchronizationContext _syncContext;
        

        public MainWindow()
        {
            InitializeComponent();
            findTimerResource();
            findStorageResource();
            _timer.PomodoroCompleted += _timer_PomodoroCompleted;
            _syncContext = SynchronizationContext.Current;
            
        }

                    
                
        private void _timer_PomodoroCompleted(object sender, TimerCompletedEventArgs e)
        {
            _syncContext.Post(o=> { PomodoroCompleted(e.StartTime, e.EndTime); }, null);


        }


        private void PomodoroCompleted(DateTime startTime, DateTime endTime)
        {
            string category = "Unknown";
            string description = "Unknown";

            enableButtons();
            if (cboCategory.SelectedIndex != -1)
                category = cboCategory.Text;
            if (txtDetail.Text.Length > 0)
                description = txtDetail.Text;

            _storage.RecordSuccessfulPomodoro(startTime, endTime, category, description);
        }


        private void findTimerResource()
        {
            if (_timer == null)
            {
                _timer = FindResource("myTimer") as PomodoroTimer;
            }
        }

        private void findStorageResource()
        {
            if (_storage == null)
            {
                _storage = FindResource("myStorage") as PomodoroDetailedFileStorage;
            }
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            PomodoroDetailPopup.IsOpen = true;
            cboCategory.Focus();
            
        }

        private void pomodoro()
        {
            _timer.Start(PomodoroTimer.PomodoroTimeSpanType.FullPomodoro);
        }
       

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            
            _timer.Stop();
            enableButtons();
        }

        private void btnShortBreak_Click(object sender, RoutedEventArgs e)
        {
            
            _timer.Start(PomodoroTimer.PomodoroTimeSpanType.ShortBreak);
            disableButtons();

        }

        private void btnLongBreak_Click(object sender, RoutedEventArgs e)
        {

            _timer.Start(PomodoroTimer.PomodoroTimeSpanType.LongBreak);
            disableButtons();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            PomodoroDetails x = new PomodoroDetails();
            //x.WindowStartupLocation = WindowStartupLocation.Manual;
            Point relativePoint = ((Button)sender).PointToScreen(new Point(0,0));
            PresentationSource source = PresentationSource.FromVisual(this);
            Point windowPoint = source.CompositionTarget.TransformFromDevice.Transform(relativePoint);
            Button btn = (Button)sender;
            
            //Point relativePoint = btn.TransformToAncestor(this).Transform(new Point(this.Left, this.Top));
            
            x.Left = windowPoint.X ;
            x.Top = windowPoint.Y+btn.ActualHeight;
            //if(x.Top + x.ActualHeight > Scree)
            
            x.ShowDialog();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            PomodoroDetailPopup.IsOpen = false;
            pomodoro();
            disableButtons();
        }

        private void disableButtons()
        {
            btnStart.IsEnabled = false;
            btnLongBreak.IsEnabled = false;
            btnShortBreak.IsEnabled = false;
        }

        private void enableButtons()
        {
            btnStart.IsEnabled = true;
            btnLongBreak.IsEnabled = true;
            btnShortBreak.IsEnabled = true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.cboCategory.IsEditable = true;
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            this.cboCategory.IsEditable = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using Naklih.Com.Pomodoro.ClassLib;

namespace Naklih.Com.Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :   Window
    {
        PomodoroTimer _timer;
        IProgressDetailStorage _storage;
        private readonly SynchronizationContext _syncContext;
        ObservableCollection<String> _categoryItems;

        

        public MainWindow()
        {
            InitializeComponent();
            findTimerResource();
            findStorageResource();
            _timer.TimerCompleted += _timer_TimerCompleted;
            _syncContext = SynchronizationContext.Current;
            _categoryItems = getCategoryItems();
            //cboCategory.ItemsSource = getCategoryItems();
            //cboCategory.ItemBindingGroup.UpdateSources() 
            cboCategory.SelectedIndex = 1;
        }


        public ObservableCollection<String> CategoryItems
        {
            get
            {
                return _categoryItems;
            }
        }

        private ObservableCollection<String> getCategoryItems()
        {
            if(_categoryItems == null)
            {
                _categoryItems = new ObservableCollection< string> { "Office", "Home" };

            }
            return _categoryItems;
        }

       

       

       
        public string NewItem
        {
            set
            {
                if (this.cboCategory.SelectedItem != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    _categoryItems.Add(value);
                    this.cboCategory.SelectedItem = value;
                    this.cboCategory.IsEditable = false;
                }
            }
        }


        private void _timer_TimerCompleted(object sender, TimerCompletedEventArgs e)
        {
            _syncContext.Post(o=> { TimerCompleted(e.StartTime, e.EndTime); }, null);


        }


        private void TimerCompleted(DateTime startTime, DateTime endTime)
        {
            string category = "Unknown";
            string description = "Unknown";

            enableButtons();
            if (_timer.TimerMode == PomodoroTimer.PomodoroTimeSpanType.FullPomodoro)
            {
                if (cboCategory.Text.Length > 0)
                    category = cboCategory.Text;
                if (txtDetail.Text.Length > 0)
                    description = txtDetail.Text;

                _storage.RecordSuccessfulPomodoro(startTime, endTime, category, description);
            }
        }

        private void saveCategories()
        {
            MessageBox.Show(cboCategory.Items.Count.ToString());
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
             
            Process.Start(_storage.StorageLocation);
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
            this.cboCategory.ActivateEditMode();
            //this.cboCategory.IsEditable = true;
            
            
           
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            this.cboCategory.IsEditable = false;
            _categoryItems.Remove(this.cboCategory.Text);
        }

       
    }
}

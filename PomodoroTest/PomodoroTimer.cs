using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PomodoroTest
{
    public class PomodoroTimer : INotifyPropertyChanged
    {
        private TimeSpan _timeRemaining;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private TimeSpan _tickInterval = new TimeSpan(0, 0, 1);
        public event EventHandler TimerCompleted;

        public enum PomodoroTimeSpanType
        {
            FullPomodoro,
            ShortBreak,
            LongBreak
        }

         
        public PomodoroTimer(
        )
        {
            _timeRemaining = new TimeSpan(0, 25, 0);
             _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = _tickInterval.TotalMilliseconds;
            OnPropertyChanged("TimeRemaining");
            //_timeRemaining = span;
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (TimeRemaining.TotalSeconds > 0)
            {
                TimeRemaining = TimeRemaining.Subtract(_tickInterval);
                OnPropertyChanged("TimeRemaining");
            }
            else
            {
                _timer.Stop();
                onTimerCompleted();
            }
        }
        public void Start(PomodoroTimeSpanType spanType)
        {
            switch(spanType)
            {
                case PomodoroTimeSpanType.FullPomodoro:
                    default:
                    _timeRemaining = new TimeSpan(0,25,0);
                    break;
                case PomodoroTimeSpanType.LongBreak:
                    _timeRemaining = new TimeSpan(0,10,0);
                    break;
                case PomodoroTimeSpanType.ShortBreak:
                    _timeRemaining = new TimeSpan(0, 5,0);
                    break;
            }

            OnPropertyChanged("TimeRemaining");
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                return _timeRemaining;
            }
            set
            {
                
                _timeRemaining = value;
                OnPropertyChanged("TimeRemaining");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void onTimerCompleted()
        {
            System.Media.SystemSounds.Beep.Play();
            

            if(TimerCompleted != null)
            {
                TimerCompleted(this, new EventArgs());
            }
        }
    }
}

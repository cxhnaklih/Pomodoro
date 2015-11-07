using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public class PomodoroTimer : INotifyPropertyChanged
    {
        private TimeSpan _timeRemaining;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private TimeSpan _tickInterval = new TimeSpan(0, 0, 1);
        private PomodoroTimeSpanType _currentType = PomodoroTimeSpanType.FullPomodoro;
        private DateTime _lastPomodoro = PomodoroTimer.UnknownLastPomodoro;
        

        public static DateTime UnknownLastPomodoro
        {
            get
            {
                return new DateTime(1970, 01, 01);
            }
        }

        public event EventHandler<TimerCompletedEventArgs> TimerCompleted;
        


        public enum PomodoroTimeSpanType
        {
            FullPomodoro,
            ShortBreak,
            LongBreak
        }

         
        public PomodoroTimer(
        )
        {
            //_timeRemaining = getTimeSpan(PomodoroTimeSpanType.FullPomodoro);
            _timeRemaining = new TimeSpan(0,0,0);
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
            _currentType = spanType;
            _timeRemaining = getTimeSpan(spanType);
            _timer.Interval = _tickInterval.TotalMilliseconds;
            OnPropertyChanged("TimeRemaining");
            _timer.Start();
        }

        private TimeSpan getTimeSpan(PomodoroTimeSpanType spanType)
        {
            switch (spanType)
            {
                case PomodoroTimeSpanType.FullPomodoro:
                default:
                    return new TimeSpan(0,25,0);
                    
                case PomodoroTimeSpanType.LongBreak:
                    return  new TimeSpan(0, 10, 0);
                    
                case PomodoroTimeSpanType.ShortBreak:
                    return new TimeSpan(0, 5, 0);
                    
            }
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

        public DateTime LastPomodoro
        {
            get
            {
                return _lastPomodoro;
            }
        }


        public PomodoroTimeSpanType TimerMode
        {
            get
            {
                return _currentType;
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
            if (_currentType == PomodoroTimeSpanType.FullPomodoro)
            {
                _lastPomodoro = DateTime.Now;
                OnPropertyChanged("LastPomodoro");
            }
                if (TimerCompleted != null)
                {
                    TimerCompleted(this, new TimerCompletedEventArgs(getTimeSpan(_currentType)));
                }
            
        }
    }
}

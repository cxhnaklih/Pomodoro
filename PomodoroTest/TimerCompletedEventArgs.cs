using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;

namespace PomodoroTest
{
    public class TimerCompletedEventArgs: EventArgs
    {

        public DateTime StartTime { get;  }
        public DateTime EndTime { get; }

        public TimerCompletedEventArgs( DateTime timestamp, TimeSpan pomodoroLength)
        {
            StartTime = timestamp - pomodoroLength;
            EndTime = timestamp;
        }

        public TimerCompletedEventArgs(TimeSpan pomodoroLength) : this( DateTime.Now, pomodoroLength)
        {
         
        }
    }
}
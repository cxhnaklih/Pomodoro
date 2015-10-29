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
        public string WorkCategory { get; set; }
        public string WorkDetail { get; set; }
        public DateTime StartTime { get;  }
        public DateTime EndTime { get; }

        public TimerCompletedEventArgs(string workCategory, string workDetail, DateTime timestamp, TimeSpan pomodoroLength)
        {
            WorkCategory = workCategory;
            WorkDetail = workDetail;
            StartTime = timestamp - pomodoroLength;
            EndTime = timestamp;
        }

        public TimerCompletedEventArgs(string workCategory, string workDetail, TimeSpan pomodoroLength) : this(workCategory, workDetail, DateTime.Now(), pomodoroLength)
        {
         
        }
    }
}
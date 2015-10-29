using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PomodoroTest
{
    public class PomodoroTitleTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length ==2)
            {
                DateTime lastPomodoro= PomodoroTimer.UnknownLastPomodoro;
                TimeSpan timeRemaining= new TimeSpan(0,0,0);
                for(int i =0; i< values.Length; i++)
                {
                    if(values[i] is TimeSpan)
                    {
                        timeRemaining = (TimeSpan)values[i];
                    }
                    if(values[i] is DateTime)
                    {
                        lastPomodoro = (DateTime) values[i];
                    }

                }
                if (timeRemaining.TotalSeconds > 0)
                {
                    return string.Format("{0}", timeRemaining);
                }
                else
                {
                    if (lastPomodoro != PomodoroTimer.UnknownLastPomodoro)
                    {
                        return string.Format("Last Pomodoro {0:HH:mm}", lastPomodoro);
                    }
                }

            }
            
            
            
            return "Pomodoro Timer";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}

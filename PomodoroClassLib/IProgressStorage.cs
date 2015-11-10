using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public interface IProgressStorage: INotifyPropertyChanged
    {
        void RecordSuccessfulPomodoro();
        int PomodorosToday { get; }
        string StorageLocation { get; }
    }

    public interface IProgressDetailStorage:IProgressStorage
    {
        void RecordSuccessfulPomodoro(DateTime startTime, DateTime endTime, string category, string detail );
        
    }
}

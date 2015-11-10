using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;
using System.ComponentModel;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public class PomodoroFileStorage : IProgressStorage
    {
        BasicFileStorage _storage;
        protected int _pomodorosToday = 0;       

        public event PropertyChangedEventHandler PropertyChanged;


        public PomodoroFileStorage() : this(new FileSystem())
        {
        }

        public PomodoroFileStorage(IFileSystem fileSystem) : this(Constants.DEFAULT_FILE_STORAGE_FILENAME, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, fileSystem)
        {
        }

        public PomodoroFileStorage(string filename): this(filename,new FileSystem())
        {
        }

        public PomodoroFileStorage(string filename, IFileSystem fileSystem) : this(filename, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, fileSystem)
        {
        }

        public PomodoroFileStorage(string filename, string directoryName) : this(filename, directoryName , new FileSystem())
        {
        }
        
        public PomodoroFileStorage(string fileName, string directoryName, IFileSystem fileSystem)
        {
            _storage = new BasicFileStorage(fileName, directoryName, fileSystem);
            loadFile( true);
            
        }    

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       

        private string[] loadFile( bool setPomodoroCount)
        {
            string[] lines = _storage.GetAllLines();
            bool lastLineIsToday = false;

            string lastLine = "";
            if (lines.Length > 0)
                lastLine = lines.Last();

            if(lastLine.Length >0)
            {
                string[] components = lastLine.Split(',');
                if (components.Length >= 2)
                {
                    string date = components[0];
                    string pomodoros = components[1];
                    IFormatProvider provider = new System.Globalization.DateTimeFormatInfo();
                    
                    if(DateTime.ParseExact(date,"yyyyMMdd", provider).Date == DateTime.Now.Date)
                    {
                        lastLineIsToday = true;
                        if(setPomodoroCount)
                            _pomodorosToday = int.Parse(pomodoros);
                    }
                }
            }

            if(lastLineIsToday)// Special Treatment for today's line
            {
                string[] result = new string[lines.Count()-1];
                Array.Copy(lines, 0, result, 0, lines.Count() - 1);// remove the last line  from our array as we will update the count.
                return result;
            }
            else
            {
                return lines;
            }
            
        }

        public void SaveFile()
        {
            List<string> lines = new List<string>();
            lines.AddRange(loadFile(false));

            //format and add the latest count
            string newLine = string.Format("{0:yyyyMMdd},{1}", DateTime.Now.Date, this.PomodorosToday);
            lines.Add(newLine);

            _storage.SaveFile(lines);
            
        }

        public BasicFileStorage Storage
        {
            get
            {
                return _storage;
            }
        }
        public int PomodorosToday
        {
            get
            {
                return _pomodorosToday;
            }
        }

        public string StorageLocation
        {
            get
            {

                return _storage.StorageLocation;
            }
        }

        public virtual void RecordSuccessfulPomodoro()
        {
            _pomodorosToday += 1;
            SaveFile();
            OnPropertyChanged("PomodorosToday");
        }
    }
}

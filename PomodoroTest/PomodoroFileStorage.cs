using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;
using System.ComponentModel;

namespace PomodoroTest
{
    public class PomodoroFileStorage : IProgressStorage
    {
        protected string _filename= "PomodoroData.csv";
        protected string _filePath = ";";
        protected int _pomodorosToday = 0;
        protected readonly IFileSystem _fileSystem;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
        public PomodoroFileStorage(string filename): this(new FileSystem())
        {
            _filename = filename;
        }

        public PomodoroFileStorage(string filename, IFileSystem fileSystem) : this(fileSystem)
        {
            _filename = filename;
        }

        public PomodoroFileStorage():this(new FileSystem())
        {
        }


        public PomodoroFileStorage(IFileSystem filesystem)
        {
            _fileSystem = filesystem;

            _filePath = FindStorageFile(_filename);
            loadFile(_filePath, true);

        }

        protected virtual string FindStorageFile(string filename)
        {
            DirectoryInfoBase dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(_fileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.APPLICATION_NAME));
            if (!dirInfo.Exists)
                dirInfo.Create();

            FileInfoBase fileInfo = _fileSystem.FileInfo.FromFileName(_fileSystem.Path.Combine(dirInfo.FullName, filename));
            if (!fileInfo.Exists)
            {
                fileInfo.CreateText();
            }
            return _fileSystem.Path.Combine(dirInfo.FullName, filename);
        }

        private string[] loadFile(string filePath, bool setPomodoroCount)
        {
            string[] lines = _fileSystem.File.ReadAllLines(filePath);
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

            if(lastLineIsToday)
            {
                string[] result = new string[lines.Count()-1];
                Array.Copy(lines, 0, result, 0, lines.Count() - 1);
                return result;
            }
            else
            {
                return lines;
            }
            
        }

        public virtual void SaveFile()
        {
            List<string> lines = new List<string>();
            lines.AddRange(loadFile(_filePath, false));

            //format and add the latest count
            string newLine = string.Format("{0:yyyyMMdd},{1}", DateTime.Now.Date, this.PomodorosToday);
            lines.Add(newLine);

            //delete existing file
            _fileSystem.File.Delete(_filePath);

            //write new File
            _fileSystem.File.WriteAllLines(_filePath, lines);
            
        }

        public string FileName
        {
            get
            {
                return _filename;
            }
        }
        public int PomodorosToday
        {
            get
            {
                return _pomodorosToday;
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

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
        
        protected string _fileName= "";
        protected string _directoryName = "";
        protected string _filePath = "";
        protected int _pomodorosToday = 0;
        protected readonly IFileSystem _fileSystem;

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
            _fileSystem = fileSystem;
            _directoryName = directoryName;
            _fileName = fileName;
            _filePath = FindStorageFile(fileName);
            loadFile(_filePath, true);
            
        }    

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual string FindStorageFile(string filename)
        {
            DirectoryInfoBase dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(_directoryName);
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
                return _fileName;
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

                return _fileSystem.FileInfo.FromFileName(_filePath).Directory.FullName;
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

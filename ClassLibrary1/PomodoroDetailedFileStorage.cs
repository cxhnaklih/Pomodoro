using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public class PomodoroDetailedFileStorage : PomodoroFileStorage, IProgressDetailStorage
    {
        protected string _detailFileName= Constants.DEFAULT_FILE_STORAGE_DETAIL_FILENAME;

        public PomodoroDetailedFileStorage() : base()
        {
        }

        public PomodoroDetailedFileStorage(IFileSystem filesystem) : base(filesystem)
        {
        }

        public PomodoroDetailedFileStorage(string filename, string detailedFileName) : base(filename)
        {
            _detailFileName = detailedFileName;
        }

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, string directoryName) : base(filename, directoryName)
        {
            _detailFileName = detailedFileName;
        }

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, IFileSystem fileSystem) : base(filename, fileSystem)
        {
            _detailFileName = detailedFileName;
        }

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, string directoryName, IFileSystem fileSystem) : base(filename, directoryName, fileSystem)
        {
            _detailFileName = detailedFileName;
        }
        

        private void SaveFile(DateTime startTime, DateTime endTime, string category, string detail)
        {
            string filePath = FindStorageFile(_detailFileName);
            using (StreamWriter sr = _fileSystem.File.AppendText(filePath))
            {
                sr.WriteLine(string.Format("{0:O},{1:O},{2}, {3}", startTime, endTime, category, detail));
            }            
        }

        void IProgressDetailStorage.RecordSuccessfulPomodoro(DateTime startTime, DateTime endTime, string category, string detail)
        {
            base.RecordSuccessfulPomodoro();
            SaveFile(startTime, endTime, category, detail);
        }
    }
}

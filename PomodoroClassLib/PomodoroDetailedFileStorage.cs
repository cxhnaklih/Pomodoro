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
        
        public BasicFileStorage _detailedStorage;

        public PomodoroDetailedFileStorage() : this(Constants.DEFAULT_FILE_STORAGE_FILENAME, Constants.DEFAULT_FILE_STORAGE_DETAIL_FILENAME, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, new FileSystem())
        {}

        public PomodoroDetailedFileStorage(IFileSystem filesystem) : this(Constants.DEFAULT_FILE_STORAGE_FILENAME, Constants.DEFAULT_FILE_STORAGE_DETAIL_FILENAME, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, filesystem)
        {}

        public PomodoroDetailedFileStorage(string filename, string detailedFileName) : this(filename, detailedFileName, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, new FileSystem())
        {}

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, string directoryName) : this(filename,detailedFileName, directoryName, new FileSystem())
        {}

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, IFileSystem fileSystem) : this(filename, detailedFileName, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, fileSystem)
        {}

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, string directoryName, IFileSystem fileSystem) : base(filename, directoryName, fileSystem)
        {   
            _detailedStorage = new BasicFileStorage(detailedFileName, directoryName, fileSystem);
        }
        

        private void SaveFile(DateTime startTime, DateTime endTime, string category, string detail)
        {
            _detailedStorage.AppendLine(string.Format("{0:O},{1:O},{2}, {3}", startTime, endTime, category, detail));
            
        }

        void IProgressDetailStorage.RecordSuccessfulPomodoro(DateTime startTime, DateTime endTime, string category, string detail)
        {
            base.RecordSuccessfulPomodoro();
            SaveFile(startTime, endTime, category, detail);
        }
    }
}

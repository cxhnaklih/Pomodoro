using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Abstractions;

namespace PomodoroTest
{
    public class PomodoroDetailedFileStorage : PomodoroFileStorage, IProgressDetailStorage
    {
        protected string _detailFileName="PomodoroDetails.csv";


        public PomodoroDetailedFileStorage(string filename, string detailedFileName) : base(filename, new FileSystem())
        {
            _detailFileName = detailedFileName;
        }

        public PomodoroDetailedFileStorage(string filename, string detailedFileName, IFileSystem fileSystem) : base(filename, fileSystem)
        {
            _detailFileName = detailedFileName;
        }



        public PomodoroDetailedFileStorage(IFileSystem filesystem):base(filesystem)
        {
        }

        public PomodoroDetailedFileStorage() : base()
        {
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Naklih.Com.Pomodoro;
using Naklih.Com.Pomodoro.ClassLib;


namespace Naklih.Com.Pomodoro.UnitTests
{
    [TestFixture]
    public class PomodoroFileStorageTests
    {
        [Test]
        public void PomodoroFileStorage_NoFileNameSuppliedButFileExists_LoadsFileReports1Pomodoro()
        {           
            string fileName = System.IO.Path.Combine(Constants.DEFAULT_FILE_STORAGE_DIRECTORY, Constants.DEFAULT_FILE_STORAGE_FILENAME);
            var fileSystem =  new MockFileSystem(new Dictionary<string, MockFileData>
                    {
                        { fileName, new MockFileData(string.Format("{0:yyyyMMdd}, 1", DateTime.Now.Date)) }
                    }
            );
            PomodoroFileStorage fs = new PomodoroFileStorage(fileSystem);

            Assert.AreEqual(fs.PomodorosToday, 1);
        }

        [Test]
        public void PomodoroFileStorage_NoFileNameSuppliedAndFileDoesntExist_CreatesFile()
        {

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            
            );
            PomodoroFileStorage fs = new PomodoroFileStorage(fileSystem);

            Assert.AreEqual(fileSystem.AllFiles.Count(), 1);
        }

        [Test]
        public void PomodoroFileStorage_FileNameSuppliedAndFileDoesntExist_CreatesFile()
        {

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()

            );
            PomodoroFileStorage fs = new PomodoroFileStorage("test.csv", fileSystem );

            Assert.AreEqual(fileSystem.AllFiles.Count(), 1);
        }

       [Test]
        public void PomodoroFileStorage_PomodoroComplete_IncrementsPomodoroCountTo1()
        {

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()

            );
            PomodoroFileStorage fs = new PomodoroFileStorage( fileSystem);
            fs.RecordSuccessfulPomodoro();

            Assert.AreEqual( 1, fs.PomodorosToday);
        }
    }
}

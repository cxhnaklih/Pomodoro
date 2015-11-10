using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;
using System.IO;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public class BasicFileStorage
    {
        protected string _fileName = "";
        protected string _directoryName = "";
        protected string _filePath = "";
        protected readonly IFileSystem _fileSystem;

        
        public BasicFileStorage(string fileName, string directoryName, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _directoryName = directoryName;
            _fileName = fileName;
            _filePath = FindStorageFile(fileName);
            

        }

        protected virtual string FindStorageFile(string filename)
        {
            DirectoryInfoBase dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(_directoryName);
            if (!dirInfo.Exists)
                dirInfo.Create();

            FileInfoBase fileInfo = _fileSystem.FileInfo.FromFileName(_fileSystem.Path.Combine(dirInfo.FullName, filename));
            if (!fileInfo.Exists)
            {
                using (StreamWriter sw = fileInfo.CreateText())
                {
                    sw.Close();
                }
            }
            return _fileSystem.Path.Combine(dirInfo.FullName, filename);
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public string DirectoryName
        {
            get
            {
                return _directoryName;
            }
        }
        public string FullFilePath
        {
            get
            {
                return _filePath;
            }
        }

        public IFileSystem FileSystem
        {
            get
            {
                return _fileSystem;
            }
        }

        public string StorageLocation
        {
            get
            {

                return _fileSystem.FileInfo.FromFileName(_filePath).Directory.FullName;
            }
        }

        public string[] GetAllLines()
        {
            string[] lines = _fileSystem.File.ReadAllLines(this.FullFilePath);
            return lines;
        }

        public void SaveFile(List<string> lines)
        {
            //delete existing file
            this.FileSystem.File.Delete(this.FullFilePath);

            //write new File
            this.FileSystem.File.WriteAllLines(this.FullFilePath, lines);
        }

        public void AppendLine(string line)
        {
            using (StreamWriter sr = FileSystem.File.AppendText(FullFilePath))
            {
                sr.WriteLine(line);
            }
        }
    }
}

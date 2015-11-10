using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public class CategoryFileStorage : ICategoryStorage
    {
        BasicFileStorage _storage;

        public CategoryFileStorage(string fileName, string directoryName, IFileSystem fileSystem)
        {
            _storage = new BasicFileStorage(fileName, directoryName, fileSystem);

        }

        public CategoryFileStorage(string fileName, string directoryName) : this(fileName, directoryName, new FileSystem())
        { }

        public CategoryFileStorage(string fileName) : this(fileName, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, new FileSystem())
        { }

        public CategoryFileStorage(string fileName, IFileSystem fs) : this(fileName, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, fs)
        { }

        public CategoryFileStorage(IFileSystem fs) : this(Constants.DEFAULT_FILE_STORAGE_CATEGORY_FILENAME, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, fs)
        { }

        public CategoryFileStorage() : this(Constants.DEFAULT_FILE_STORAGE_CATEGORY_FILENAME, Constants.DEFAULT_FILE_STORAGE_DIRECTORY, new FileSystem())
        { }

        public List<string> RetrieveCategories()
        {
            List<string> result = new List<string>(_storage.GetAllLines());
            return result;
        }

        public void StoreCategories(List<string> categories)
        {
            _storage.SaveFile(categories);
        }
    }
}

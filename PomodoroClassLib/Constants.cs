using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naklih.Com.Pomodoro
{
    public static class Constants
    {
        public static string APPLICATION_NAME = "Pomodoro Timer";
        public static string DEFAULT_FILE_STORAGE_FILENAME = "PomodoroData.csv";
        public static string DEFAULT_FILE_STORAGE_DIRECTORY = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Constants.APPLICATION_NAME);
        public static string DEFAULT_FILE_STORAGE_DETAIL_FILENAME = "PomodoroDetails.csv";
        public static string DEFAULT_FILE_STORAGE_CATEGORY_FILENAME = "PomodoroCategories.csv";
    }
}

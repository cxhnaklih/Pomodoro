﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naklih.Com.Pomodoro.ClassLib
{
    public interface ICategoryStorage
    {
        void StoreCategories(List<string> categories);
        List<string> RetrieveCategories();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AwesomeAutoClicker.Commands;

namespace AwesomeAutoClicker.ViewModels
{
    public class HomeVM : ViewModelBase
    {
        public bool canNavigate = true;
        public HomeVM(bool _stop)
        {
            canNavigate = _stop;
        }
    }
}

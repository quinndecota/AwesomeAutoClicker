using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomeAutoClicker.Commands;
using System.Windows.Input;
using System.Windows;

namespace AwesomeAutoClicker.ViewModels
{
    public  class AdvancedVM : ViewModelBase
    {
        public MainWindowVM ViewModel { get; set; }

         
        public AdvancedVM(MainWindowVM viewModel)
        {
            ViewModel = viewModel;
        }
    }
}

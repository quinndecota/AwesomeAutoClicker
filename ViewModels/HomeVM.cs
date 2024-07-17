using System;
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
        public MainWindowVM ViewModel { get; set; }


        public HomeVM(MainWindowVM viewModel)
        {
            ViewModel = viewModel;
        }
    }
}

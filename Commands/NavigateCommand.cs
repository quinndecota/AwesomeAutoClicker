using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AwesomeAutoClicker.ViewModels;
using AwesomeAutoClicker.Views;

namespace AwesomeAutoClicker.Commands
{
    public class NavigateCommand : ICommand
    {
        private MainWindowVM ViewModel;

        public NavigateCommand(MainWindowVM viewModel)
        {
            ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
            if (parameter.ToString() == "Home")
            {
                ViewModel.SelectedViewModel = new HomeVM(ViewModel);
            }
            else if (parameter.ToString() == "Advanced")
            {
                ViewModel.SelectedViewModel = new AdvancedVM(ViewModel);
            }
            
            
        }
    }
}

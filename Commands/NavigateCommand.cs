using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AwesomeAutoClicker.ViewModels;

namespace AwesomeAutoClicker.Commands
{
    public class NavigateCommand : ICommand
    {
        private MainWindowVM viewModel;

        public NavigateCommand(MainWindowVM viewModel)
        {
            this.viewModel = viewModel;
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
                viewModel.SelectedViewModel = new HomeVM();
            }
            else if (parameter.ToString() == "Advanced")
            {
                viewModel.SelectedViewModel = new AdvancedVM();
            }
        }
    }
}

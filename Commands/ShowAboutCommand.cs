using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AwesomeAutoClicker.Views;

namespace AwesomeAutoClicker.Commands
{
    public class ShowAboutCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Window about = new AboutWindow();
            about.ShowDialog();
        }
    }
}

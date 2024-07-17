using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AwesomeAutoClicker.Commands;

namespace AwesomeAutoClicker.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        public bool canNavigate = true;

        private ViewModelBase _selectedViewModel;
        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand NavigateCommand { get; set; }
        public ICommand ShowAboutCommand { get; set; }

        public MainWindowVM()
        {
            SelectedViewModel = new HomeVM(this);
            NavigateCommand = new NavigateCommand(this);
            ShowAboutCommand = new ShowAboutCommand();
        }
    }
}

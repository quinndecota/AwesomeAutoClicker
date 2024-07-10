using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AwesomeAutoClicker.ViewModels;
using Gma.System.MouseKeyHook;

namespace AwesomeAutoClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IKeyboardMouseEvents m_GlobalHook;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM();
            Subscribe();
            
        }
        #region Mouse Coordinates
        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseMoveExt += GlobalHookMouseMoveExt;
        }

        private void GlobalHookMouseMoveExt(object? sender, MouseEventExtArgs e)
        {
            CoordDisplay.Text = "Your mouse is at (" + e.X + "," + e.Y + ")";
        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseMoveExt -= GlobalHookMouseMoveExt;

            m_GlobalHook.Dispose();
        }
        #endregion
    }
}
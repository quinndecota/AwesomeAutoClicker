using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace AwesomeAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : System.Windows.Controls.UserControl
    {
InputSimulator input = new();
        private IKeyboardMouseEvents m_GlobalHook;

        public HomeView()
        {
            InitializeComponent();
            Subscribe();


        }

        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyPress += GlobalHookKeyPress;


        }

        private void GlobalHookKeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'y' || e.KeyChar == 'Y')
            {
                Start();
            }
            else if (e.KeyChar == 'n' || e.KeyChar == 'N')
            {
                Stop();
            }
        }
        public void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            m_GlobalHook.Dispose();
        }


        public bool stop { get; set; }
        private void Start()
        {
            if (Infinite.IsChecked == true)
            {
                while (!stop)
                {
                    Thread.Sleep(1000);
                }
            }
            else
            {
                int count;
                try
                {
                    count = Int32.Parse(RepeatCount.Text);
                } catch
                {
                    return;
                }
                for (int i = 0; i < count; i++)
                {

                }
            }
        }

        private void Stop()
        {
            stop = true;
        }
    }
}

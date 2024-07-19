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
using System.Runtime.InteropServices.Marshalling;
using AwesomeAutoClicker.ViewModels;

namespace AwesomeAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : System.Windows.Controls.UserControl
    {
InputSimulator input = new();
        private IKeyboardMouseEvents? m_GlobalHook;
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;

        public HomeView() { 
InitializeComponent();        
            
            Subscribe();

            SelectedLocationRadioButton.IsChecked = false;

        }
        #region input stuff
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

        #endregion
        public bool stop { get; set; }
        private void Start()
        {

            RunningStats.Text = "Running :)";
            stop = false;

            int timeInMilliseconds = GetTimeInMilliseconds();
            if (timeInMilliseconds <1)
            {
                System.Windows.MessageBox.Show("Enter a valid amount of time between clicks");
                return;
            }
            if (Infinite.IsChecked == true)
            {
                while (!stop)
                {
                    
                    DoHomeViewMouseClick();
                    Wait(timeInMilliseconds);
                }
            }
            else
            {
                int count = 0;
                try
                {
                    count = Int32.Parse(RepeatCount.Text);
                    for (int i = 0; i < count; i++)
                    {
                        if (stop)
                        {
                                return; 
                        }
                        DoHomeViewMouseClick();
                        Wait(timeInMilliseconds);
                    }
                    RunningStats.Text = "Not Running";
                }
                catch
                {
                    RunningStats.Text = "Not Running";
                    return;
                }
            }
        }

        private void Stop()
        {
            stop = true;
            RunningStats.Text = "Not Running";
        }

        private int GetTimeInMilliseconds()
        {
            int timeInMilliseconds = 0;
            int current = 0;
            try
            {
                if (Hours.Text == null)
                {
                    throw new Exception();
                }
                current = Int32.Parse(Hours.Text);
            }
            catch
            {
                current = 0;
            }
            timeInMilliseconds += current * 3600000;
            try
            {
                if (Minutes.Text == null)
                {
                    throw new Exception();
                }
                current = Int32.Parse(Minutes.Text);
            }
            catch
            {
                current = 0;
            }
            timeInMilliseconds += current * 60000;
            try
            {
                if (Seconds.Text == null)
                {
                    throw new Exception();
                }
                current = Int32.Parse(Seconds.Text);
            }
            catch
            {
                current = 0;
            }
            timeInMilliseconds += current * 1000;
            try
            {
                if (Milliseconds.Text == null)
                {
                    throw new Exception();
                }
                current = Int32.Parse(Milliseconds.Text);
            }
            catch
            {
                current = 0;
            }
            return timeInMilliseconds + current;

        }

        private void DoHomeViewMouseClick()
        {
            if ((bool)SelectedLocationRadioButton.IsChecked)
            {
                try
                {
                    int xpos = Int32.Parse(TextBoxPickedXValue.Text);
                    int ypos = Int32.Parse(TextBoxPickedYValue.Text);
                    input.Mouse.MoveMouseTo((xpos * 65615 / (screenWidth)), (ypos * 65615 / (screenHeight)));
                }
                catch
                {
                    SelectedLocationRadioButton.IsChecked = false;
                    RadioButtonSelectedLocationMode_CurrentLocation.IsChecked = true;
                }
            }
            if (ClickType.Text == "Left")
            {
                input.Mouse.LeftButtonClick();
            }
            if (ClickType.Text == "Middle")
            {
                input.Mouse.MiddleButtonClick();
            }
            if (ClickType.Text == "Right")
            {
                input.Mouse.RightButtonClick();
            }
            
        }

        public static void Wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0)
                return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        #region Start/Stop Buttons
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        #endregion
    }
}
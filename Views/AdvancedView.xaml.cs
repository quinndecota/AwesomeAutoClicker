using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using AwesomeAutoClicker.Models;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Gma.System.MouseKeyHook;
using System.Drawing.Drawing2D;
using System.Threading;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using AwesomeAutoClicker.Commands;
using System.Reflection;
using AwesomeAutoClicker.ViewModels;

namespace AwesomeAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for AdvancedView.xaml
    /// </summary>
    public partial class AdvancedView : System.Windows.Controls.UserControl
    {
        List<Models.Action> script = new List<Models.Action>();
        InputSimulator input = new InputSimulator();
        ObservableCollection<string> gridViewActions = new ObservableCollection<string>();
        private IKeyboardMouseEvents? m_GlobalHook;
        private bool stop = false;
        double screenWidth = SystemParameters.PrimaryScreenWidth;
        double screenHeight = SystemParameters.PrimaryScreenHeight;


        public AdvancedView()
        {
            InitializeComponent();
            actions.ItemsSource = gridViewActions;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!(ValidateTextBoxes()))
            {
                return;
            }

            var index = actions.SelectedIndex;

            if(index == -1)
            {
                index = script.Count ;
            }

            if (TypeComboBox.Text == "Click")
            {
                if (XPos.Text == null || XPos.Text.Length == 0 || YPos.Text == null || YPos.Text.Length == 0 || ClickType.Text == null || ClickType.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, ClickType.Text, int.Parse(XPos.Text), int.Parse(YPos.Text), null, null, null);
                gridViewActions.Insert(index, currentAction.ClickType + " click at (" + currentAction.Xpos + "," + currentAction.Ypos + ")");
                script.Insert(index, currentAction);
            }
            else if (TypeComboBox.Text == "Command")
            {
                if (CmdChar.Text == null || CmdChar.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, null, null, CmdChar.Text);
                gridViewActions.Insert(index, "Ctrl + " + currentAction.CmdChar);
                script.Insert(index, currentAction);
            }
            else if (TypeComboBox.Text == "Interval")
            {
                if (Milliseconds.Text == null || Milliseconds.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, int.Parse(Milliseconds.Text), null, null);
                gridViewActions.Insert(index, "Wait: " + currentAction.Milliseconds + " milliseconds");
                script.Insert(index, currentAction);
            }
            else if (TypeComboBox.Text == "Send")
            {
                if (Message.Text == null || Message.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, null, Message.Text, null);
                gridViewActions.Insert(index, "Send Message: " + currentAction.Message);
                script.Insert(index, currentAction);
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var index = actions.SelectedIndex;
            if (gridViewActions.Count < 1)
            { return; }
            if (index == -1)
            {
                gridViewActions.RemoveAt(0);
                script.RemoveAt(0);
            }
            else
            {
                gridViewActions.RemoveAt(index);
                script.RemoveAt(index);
            }
        }

        private void Clr_Click(object sender, RoutedEventArgs e)
        {
            gridViewActions.Clear();
            script.Clear();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            stop = false;
            if (loopScriptCheckBox.IsChecked == true)
            {
                while (true)
                {
                    foreach (Models.Action currentAction in script)
                    {

                        if (stop)
                        {
                            return;
                        }
                        DoActionCommand(currentAction);
                    }
                }
            }
            foreach (Models.Action currentAction in script)
                {
                    if (stop)
                    {
                        return;
                    }
                    DoActionCommand(currentAction);
                }
    }
        
        #region DoActionCommand
        private void DoActionCommand(Models.Action action)
        {
            if (action.Type == "Click")
            {
                ClickActionCommand(action.ClickType, (int)action.Xpos, (int)action.Ypos);
            }
            else if (action.Type == "Interval")
            {
                IntervalActionCommand((int)action.Milliseconds);
            }
            else if (action.Type == "Send")
            {
                SendActionCommand(action.Message);
            }
            else if (action.Type == "Command")
            {
                CommandActionCommand(action.CmdChar);
            }
        }

        private void CommandActionCommand(string? cmdChar)
        {
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, GetVirtualKeyCode(cmdChar));
        }

        

        private void SendActionCommand(string? message)
        {
            input.Keyboard.TextEntry(message);
        }

        private void IntervalActionCommand(int m) => Wait(m);

        private void ClickActionCommand(string clickType, int xpos, int ypos)
        {
            input.Mouse.MoveMouseTo((xpos* 65615 / (screenWidth)) , (ypos * 65615 / (screenHeight)) );

            if (clickType == "Left")
            {
                input.Mouse.LeftButtonClick();
            }
            else if(clickType == "Right")
            {
                input.Mouse.RightButtonClick();
            }
            else if (clickType == "Hover")
            {
                return;
            }
        }
        #endregion

        #region Import/Export Code

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            string scriptString = "0";
            foreach (Models.Action currentAction in script)
            {
                if (currentAction.Type == "Click")
                {
                    scriptString += "|";

                    if (currentAction.ClickType == "Left")
                    {
                        scriptString += "1";
                    }
                    if (currentAction.ClickType == "Right")
                    {
                        scriptString += "2";
                    }
                    if (currentAction.ClickType == "Hover")
                    {
                        scriptString += "3";
                    }
                    scriptString += currentAction.Xpos.ToString() + "," + currentAction.Ypos.ToString();
                }
                else if (currentAction.Type == "Interval")
                {
                    scriptString += "\\" + currentAction.Milliseconds;
                }
                else if (currentAction.Type == "Command")
                {
                    scriptString += "`" + currentAction.CmdChar;
                }
                else if (currentAction.Type == "Send")
                {
                    scriptString += "~" + currentAction.Message;
                }
            }
            System.Windows.Clipboard.SetText(scriptString);
            System.Windows.MessageBox.Show("The Script for your macro has been copied to your clipboard: \n" + scriptString);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImportTextbox.Text.Length <= 1)
            {
                ImportTextbox.Visibility = Visibility.Visible;
                return;
            }

            gridViewActions.Clear();
            script.Clear();

             string scriptString = ImportTextbox.Text;
           
            try
            {
                string[] parts = scriptString.Split(new char[] { '|', '\\', '`', '~' });
                if (parts.Length <2)
                {
                    throw new Exception();
                }
                for (int i = 1; i < parts.Length; i++) // Start from 1 to skip the initial '0'
                {
                    char prefix = scriptString[scriptString.IndexOf(parts[i]) - 1];
                    Models.Action action = new(null, null, null, null, null, null, null);

                    switch (prefix)
                    {
                        case '|':
                            action.Type = "Click";

                            switch (parts[i][0])
                            {
                                case '1':
                                    action.ClickType = "Left";
                                    break;
                                case '2':
                                    action.ClickType = "Right";
                                    break;
                                case '3':
                                    action.ClickType = "Hover";
                                    break;
                            }
                            string[] coordinates = parts[i].Substring(1).Split(',');
                            action.Xpos = int.Parse(coordinates[0]);
                            action.Ypos = int.Parse(coordinates[1]);
                            gridViewActions.Add(action.ClickType + " click at (" + action.Xpos + "," + action.Ypos + ")");
                            break;
                        case '\\':
                            action.Type = "Interval";
                            action.Milliseconds = int.Parse(parts[i]);
                            gridViewActions.Add("Wait: " + action.Milliseconds + " milliseconds");
                            break;
                        case '`':
                            action.Type = "Command";
                            action.CmdChar = parts[i];
                            gridViewActions.Add("Ctrl + " + action.CmdChar);
                            break;
                        case '~':
                            action.Type = "Send";
                            action.Message = parts[i];
                            gridViewActions.Add("Send Message: " + action.Message);
                            break;
                        default:
                            Clr_Click(sender, e);
                            ImportErrorText.Visibility = Visibility.Visible;
                            return;
                    }
                    script.Add(action);
                }
            }
            catch
            {
                Clr_Click(sender, e);
                ImportErrorText.Visibility = Visibility.Visible;
                return;
            }
        ImportErrorText.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Input stuff
        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyPress += GlobalHookKeyPress;


        }

        private void GlobalHookKeyPress(object? sender, KeyPressEventArgs e)
        {
             if (e.KeyChar == 'n' || e.KeyChar == 'N')
            {
                stop = true;
            }
        }


        public void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            m_GlobalHook.Dispose();
        }

        #endregion

        #region Change Textbox Visibility
                        private void TypeComboBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
                        {
                            if (TypeComboBox.Text == "Click")
                            {
                                ClickTypeOption.Visibility = Visibility.Visible;
                                XPosOption.Visibility = Visibility.Visible;
                                YPosOption.Visibility = Visibility.Visible;
                                MillisecondsOption.Visibility = Visibility.Collapsed;
                                MessageOption.Visibility = Visibility.Collapsed;
                                CmdCharOption.Visibility = Visibility.Collapsed;
                            }
                            else if (TypeComboBox.Text == "Command")
                            {
                                ClickTypeOption.Visibility = Visibility.Collapsed;
                                XPosOption.Visibility = Visibility.Collapsed;
                                YPosOption.Visibility = Visibility.Collapsed;
                                MillisecondsOption.Visibility = Visibility.Collapsed;
                                MessageOption.Visibility = Visibility.Collapsed;
                                CmdCharOption.Visibility = Visibility.Visible;
                            }
                            else if (TypeComboBox.Text == "Interval")
                            {
                                ClickTypeOption.Visibility = Visibility.Collapsed;
                                XPosOption.Visibility = Visibility.Collapsed;
                                YPosOption.Visibility = Visibility.Collapsed;
                                MillisecondsOption.Visibility = Visibility.Visible;
                                MessageOption.Visibility = Visibility.Collapsed;
                                CmdCharOption.Visibility = Visibility.Collapsed;
                            }
                            else if (TypeComboBox.Text == "Send")
                            {
                                ClickTypeOption.Visibility = Visibility.Collapsed;
                                XPosOption.Visibility = Visibility.Collapsed;
                                YPosOption.Visibility = Visibility.Collapsed;
                                MillisecondsOption.Visibility = Visibility.Collapsed;
                                MessageOption.Visibility = Visibility.Visible;
                                CmdCharOption.Visibility = Visibility.Collapsed;
                            }
                        }
                        #endregion

        public void Wait(int milliseconds)
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
        private IEnumerable<VirtualKeyCode> GetVirtualKeyCode(string? cmdChar)
        {
            switch (cmdChar.ToLower())
            {
                case "a":
                    yield return VirtualKeyCode.VK_A;
                    break;
                case "b":
                    yield return VirtualKeyCode.VK_B;
                    break;
                case "c":
                    yield return VirtualKeyCode.VK_C;
                    break;
                case "d":
                    yield return VirtualKeyCode.VK_D;
                    break;
                case "e":
                    yield return VirtualKeyCode.VK_E;
                    break;
                case "f":
                    yield return VirtualKeyCode.VK_F;
                    break;
                case "g":
                    yield return VirtualKeyCode.VK_G;
                    break;
                case "h":
                    yield return VirtualKeyCode.VK_H;
                    break;
                case "i":
                    yield return VirtualKeyCode.VK_I;
                    break;
                case "j":
                    yield return VirtualKeyCode.VK_J;
                    break;
                case "k":
                    yield return VirtualKeyCode.VK_K;
                    break;
                case "l":
                    yield return VirtualKeyCode.VK_L;
                    break;
                case "m":
                    yield return VirtualKeyCode.VK_M;
                    break;
                case "n":
                    yield return VirtualKeyCode.VK_N;
                    break;
                case "o":
                    yield return VirtualKeyCode.VK_O;
                    break;
                case "p":
                    yield return VirtualKeyCode.VK_P;
                    break;
                case "q":
                    yield return VirtualKeyCode.VK_Q;
                    break;
                case "r":
                    yield return VirtualKeyCode.VK_R;
                    break;
                case "s":
                    yield return VirtualKeyCode.VK_S;
                    break;
                case "t":
                    yield return VirtualKeyCode.VK_T;
                    break;
                case "u":
                    yield return VirtualKeyCode.VK_U;
                    break;
                case "v":
                    yield return VirtualKeyCode.VK_V;
                    break;
                case "w":
                    yield return VirtualKeyCode.VK_W;
                    break;
                case "x":
                    yield return VirtualKeyCode.VK_X;
                    break;
                case "y":
                    yield return VirtualKeyCode.VK_Y;
                    break;
                case "z":
                    yield return VirtualKeyCode.VK_Z;
                    break;


            }
        }
        private bool ValidateTextBoxes()
        {
            if (TypeComboBox.Text == "Click")
            {
                try
                {
                    int x = int.Parse(XPos.Text);
                    int y = int.Parse(YPos.Text);

                    if (x > screenWidth || y > screenHeight)
                    {
                        throw new Exception();
                    }

                } catch
                {
                    System.Windows.MessageBox.Show("Invalid Coordinates");
                    return false;
                }
            }
            else if (TypeComboBox.Text == "Command")
            {
                if (CmdChar.Text.Length != 1)
                {
                    System.Windows.MessageBox.Show("Invalid Command Character (Only 1 letter)");
                    return false; 
                }
            }
            else if (TypeComboBox.Text == "Interval")
            {
                try
                {
                    int x = int.Parse(Milliseconds.Text);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Must type in an integer");
                    return false;
                }
            }
            return true;
        }
    }
}

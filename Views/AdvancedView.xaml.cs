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

namespace AwesomeAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for AdvancedView.xaml
    /// </summary>
    public partial class AdvancedView : System.Windows.Controls.UserControl
    {
        List<Models.Action> script = new List<Models.Action>();
        InputSimulator input = new InputSimulator();
        public ICommand showExportCommand { get; set; }
        ObservableCollection<string> gridViewActions = new ObservableCollection<string>();

        public AdvancedView()
        {
            InitializeComponent();
            actions.ItemsSource = gridViewActions;
        }


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

        private void Add_Click(object sender, RoutedEventArgs e)
        {

            if (TypeComboBox.Text == "Click")
            {
                if (XPos.Text == null || XPos.Text.Length == 0 || YPos.Text == null || YPos.Text.Length == 0 || ClickType.Text == null || ClickType.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, ClickType.Text, int.Parse(XPos.Text), int.Parse(YPos.Text), null, null, null);
                gridViewActions.Add(currentAction.ClickType + " click at (" + currentAction.Xpos + "," + currentAction.Ypos + ")");
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Command")
            {
                if (CmdChar.Text == null || CmdChar.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, null, null, CmdChar.Text);
                gridViewActions.Add("Ctrl + " + currentAction.CmdChar);
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Interval")
            {
                if (Milliseconds.Text == null || Milliseconds.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, int.Parse(Milliseconds.Text), null, null);
                gridViewActions.Add("Wait: " + currentAction.Milliseconds + " milliseconds");
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Send")
            {
                if (Message.Text == null || Message.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, null, Message.Text, null);
                gridViewActions.Add("Send Message: " + currentAction.Message);
                script.Add(currentAction);


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
            foreach (Models.Action currentAction in script)
            {
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
            input.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
        }

        private void SendActionCommand(string? message)
        {
            input.Keyboard.TextEntry(message);
        }

        private static void IntervalActionCommand(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        private void ClickActionCommand(string clickType, int xpos, int ypos)
        {
            input.Mouse.MoveMouseTo((xpos / 19.2) * 500, (ypos / 10.8) * 500);

            if (clickType == "Left")
            {
                input.Mouse.LeftButtonClick();
            }
            else
            {
                input.Mouse.RightButtonClick();
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
                    scriptString += "!";

                    if (currentAction.ClickType == "Left")
                    {
                        scriptString += "1";
                    }
                    if (currentAction.ClickType == "Right")
                    {
                        scriptString += "2";
                    }
                    scriptString += currentAction.Xpos.ToString() + "," + currentAction.Ypos.ToString();
                }
                else if (currentAction.Type == "Interval")
                {
                    scriptString += "@" + currentAction.Milliseconds;
                }
                else if (currentAction.Type == "Command")
                {
                    scriptString += "#" + currentAction.CmdChar;
                }
                else if (currentAction.Type == "Send")
                {
                    scriptString += "$" + currentAction.Message;
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

            string[] parts = scriptString.Split(new char[] { '!', '@', '#', '$' });
            for (int i = 1; i < parts.Length; i++) // Start from 1 to skip the initial '0'
            {
                char prefix = scriptString[scriptString.IndexOf(parts[i]) - 1];
                Models.Action action = new(null, null, null, null, null, null, null);

                switch (prefix)
                {
                    case '!':
                        action.Type = "Click";
                        action.ClickType = parts[i][0] == '1' ? "Left" : "Right";
                        string[] coordinates = parts[i].Substring(1).Split(',');
                        action.Xpos = int.Parse(coordinates[0]);
                        action.Ypos = int.Parse(coordinates[1]);
                        gridViewActions.Add(action.ClickType + " click at (" + action.Xpos + "," + action.Ypos + ")");
                        break;
                    case '@':
                        action.Type = "Interval";
                        action.Milliseconds = int.Parse(parts[i]);
                        gridViewActions.Add("Wait: " + action.Milliseconds + " milliseconds");
                        break;
                    case '#':
                        action.Type = "Command";
                        action.CmdChar = parts[i];
                        gridViewActions.Add("Ctrl + " + action.CmdChar);
                        break;
                    case '$':
                        action.Type = "Send";
                        action.Message = parts[i];
                        gridViewActions.Add("Send Message: " + action.Message);
                        break;
                }
                script.Add(action);
            }


            #endregion


        }
    }
}

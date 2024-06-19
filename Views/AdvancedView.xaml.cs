using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

namespace AwesomeAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for AdvancedView.xaml
    /// </summary>
    public partial class AdvancedView : UserControl
    {
        List<Models.Action> script = new List<Models.Action>();
        ObservableCollection<GridViewAction> gridViewActions = new ObservableCollection<GridViewAction>(); 
        public AdvancedView()
        {
            InitializeComponent();
            gridViewActions.Add(new GridViewAction("hi", "1"));
            actions.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = gridViewActions } );

        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {

            //Models.Action currentAction = new Models.Action(TypeComboBox.Text, "hi", 1, 2, 2, "hi,", "hi");


            if (TypeComboBox.Text == "Click")
            {
                if (XPos.Text == null ||  XPos.Text.Length == 0 || YPos.Text == null || YPos.Text.Length == 0 || ClickType.Text == null || ClickType.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, ClickType.Text, int.Parse(XPos.Text), int.Parse(YPos.Text), null, null, null);
                gridViewActions.Insert(0, new Models.GridViewAction(currentAction.Type, currentAction.ClickType+" at (" + currentAction.Xpos +","+currentAction.Ypos+")"));
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Command")
            {
                if (CmdChar.Text == null || CmdChar.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text,null,null, null, null, null, CmdChar.Text);
                gridViewActions.Insert(0, new Models.GridViewAction(currentAction.Type, "Ctrl + "+currentAction.CmdChar));
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Interval")
            {
                if (Milliseconds.Text == null || Milliseconds.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, int.Parse(Milliseconds.Text), null, null);
                gridViewActions.Insert(0, new Models.GridViewAction(currentAction.Type, "Wait "+currentAction.Milliseconds+" milliseconds"));
                script.Add(currentAction);
            }
            else if (TypeComboBox.Text == "Send")
            {
                if (Message.Text == null || Message.Text.Length == 0)
                { return; }
                Models.Action currentAction = new Models.Action(TypeComboBox.Text, null, null, null, null, Message.Text, null);
                gridViewActions.Insert(0, new Models.GridViewAction(currentAction.Type, "Send Message: "+currentAction.Message));
                script.Add(currentAction);
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            //if (EventEntries.SelectedIndex == -1)
            //{
            //    EventEntries.Items.RemoveAt(0);
            //    return;
            //}
            //EventEntries.Items.RemoveAt(EventEntries.SelectedIndex);
            gridViewActions.Add(new GridViewAction("hi", "2"));
        }

        private void Clr_Click(object sender, RoutedEventArgs e)
        {
            gridViewActions.Clear();
        }


        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            TypeComboBox.SelectedItem = TypeComboBox.SelectedItem;
        }

        
    }
}

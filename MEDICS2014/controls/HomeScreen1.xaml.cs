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

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for HomeScreen1.xaml
    /// </summary>
    public partial class HomeScreen1 : UserControl
    {
        Messages _messages = Messages.Instance;

        public HomeScreen1()
        {
            InitializeComponent();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleHomeData(message);
            }
        }

        private void handleHomeData(string message)
        {

        }

        private void AdminApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("ADMIN");
        }

        private void medicationsApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("MEDICATIONS");
        }

        private void treatmentsApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("TREATMENTS");
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("NEXT");
        }

        private void moiApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("MOI");
        }

        private void injuryApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("INJURIES");
        }

        private void sasApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("SAS");
        }

        private void notesApp_Click(object sender, RoutedEventArgs e)
        {
            //keep
            _messages.AddMessage("NOTES");
        }

        private void allergiesApp_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES");
        }

        private void pcdApp_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("PCD");
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("DELETE");
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("EXIT");
        }
    }
}

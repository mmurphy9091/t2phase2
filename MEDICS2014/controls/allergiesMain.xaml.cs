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
    /// Interaction logic for allergiesMain.xaml
    /// </summary>
    public partial class allergiesMain : UserControl
    {
        Messages _messages = Messages.Instance;
        public allergiesMain()
        {
            InitializeComponent();
            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            backButton.Visibility = Visibility.Hidden;
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleAppData(message);
            }
        }

        public void handleAppData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (message == "ADMIN ALLERGIES")
                {
                    backButton.Visibility = Visibility.Visible;
                }
                if (message == "ALLERGIES")
                {
                    backButton.Visibility = Visibility.Hidden;
                }
            }));
        }

        private void medicationsButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES MEDICATIONS");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ADMIN");
        }

        private void foodButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

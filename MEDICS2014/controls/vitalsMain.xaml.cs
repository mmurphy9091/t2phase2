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
    /// Interaction logic for vitalsMain.xaml
    /// </summary>
    public partial class vitalsMain : UserControl
    {
        vitals vitalsSummary = new vitals();
        vitalsAdd vitalsAddNew = new vitalsAdd();

        Messages _messages = Messages.Instance;

        public vitalsMain()
        {
            InitializeComponent();

            //show the first window
            vitalsMainStack.Children.Clear();
            vitalsMainStack.Children.Add(vitalsSummary);

            //set up messages instance
            _messages.HandleMessage += new EventHandler(OnHandleMessage);
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
                //Change to the add screen if add button pushed
                if (message == "ADD NEW VITALS")
                {
                    vitalsMainStack.Children.Clear();
                    vitalsMainStack.Children.Add(vitalsAddNew);
                }

                //Change to the summary page
                if (message == "VITALS SUMMARY")
                {
                    vitalsMainStack.Children.Clear();
                    vitalsMainStack.Children.Add(vitalsSummary);
                }
            }));
        }
    }
}

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
using MEDICS2014.controls.medsControls;

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for medsApp.xaml
    /// </summary>
    public partial class medsApp : UserControl
    {
        Messages _messages = Messages.Instance;

        //create all the meds controls
        medsPage1 meds1 = new medsPage1();
        medsAnalgesic1 analgesic = new medsAnalgesic1();
        medsAntibiotic1 antibiotic1 = new medsAntibiotic1();
        medsAntibiotic2 antibiotic2 = new medsAntibiotic2();
        medDetails medicationsDetails = new medDetails();
        medsUnits units = new medsUnits();
        medsOthers1 others1 = new medsOthers1();

        public medsApp()
        {
            InitializeComponent();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);

            //set the main page on startup
            medsStackPanel.Children.Clear();
            medsStackPanel.Children.Add(meds1);

        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleMessageData(message);
            }
        }

        public void handleMessageData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "MEDS":
                    case "MEDS PAGE 1":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(meds1);
                        break;
                    case "MEDS ANALGESIC":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(analgesic);
                        break;
                    case "MEDS ANTIBIOTIC 1":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(antibiotic1);
                        break;
                    case "MEDS ANTIBIOTIC 2":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(antibiotic2);
                        break;
                    case "MEDS DETAILS":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(medicationsDetails);
                        break;
                    case "MEDS UNITS":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(units);
                        break;
                    case "MEDS OTHERS":
                        medsStackPanel.Children.Clear();
                        medsStackPanel.Children.Add(others1);
                        break;
                }
                /*
                if (message == "MEDS" || message == "MEDS PAGE 1")
                {
                    medsStackPanel.Children.Clear();
                    medsStackPanel.Children.Add(meds1);
                }
                else if (message == "MEDS ANALGESIC")
                {
                    medsStackPanel.Children.Clear();
                    medsStackPanel.Children.Add(analgesic);
                }
                else if (message == "MEDS ANTIBIOTIC 1")
                {
                    medsStackPanel.Children.Clear();
                    medsStackPanel.Children.Add(antibiotic1);
                }
                else if (message == "MEDS ANTIBIOTIC 2")
                {
                    medsStackPanel.Children.Clear();
                    medsStackPanel.Children.Add(antibiotic2);
                }
                else if (message == "MEDS DETAILS")
                {
                    medsStackPanel.Children.Clear();
                    medsStackPanel.Children.Add(medicationsDetails);
                }
                 */

            }));

        }

    }
}

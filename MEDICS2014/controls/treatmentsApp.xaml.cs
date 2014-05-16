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

using MEDICS2014.controls.treamentsConrols;

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for treatmentsApp.xaml
    /// </summary>
    public partial class treatmentsApp : UserControl
    {
        //Message stuff
        Messages _messages = Messages.Instance;

        //create all the controls
        treatmentsMain main = new treatmentsMain();
        treatmentsCirculation circulation = new treatmentsCirculation();
        treatmentsAirway airway = new treatmentsAirway();
        treatmentsBreathing breathing = new treatmentsBreathing();
        treatmentsFluids fluids = new treatmentsFluids();
        normalSalineDetails saline = new normalSalineDetails();
        lactactedRingersDetails ringers = new lactactedRingersDetails();
        dextroseDetails dextrose = new dextroseDetails();
        otherFluidsDetails fluidsOther = new otherFluidsDetails();
        treatmentsBloodProducts bloodProducts = new treatmentsBloodProducts();
        bloodProductsDetails bloodDetails = new bloodProductsDetails();
        otherBloodProductsDetails bloodDetailsOther = new otherBloodProductsDetails();
        treatmentsOther other = new treatmentsOther();

        public treatmentsApp()
        {
            InitializeComponent();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);

            treatmentsStackPanel.Children.Add(main);
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
            //So MQTT can access the GUI
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "TREATMENTS MAIN":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(main);
                        break;
                    case "TREATMENTS CIRCULATION":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(circulation);
                        break;
                    case "TREATMENTS AIRWAY":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(airway);
                        break;
                    case "TREATMENTS BREATHING":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(breathing);
                        break;
                    case "TREATMENTS FLUIDS":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(fluids);
                        break;
                    case "TREATMENTS SALINE":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(saline);
                        break;
                    case "TREATMENTS RINGERS":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(ringers);
                        break;
                    case "TREATMENTS DEXTROSE":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(dextrose);
                        break;
                    case "TREATMENTS FLUIDS OTHER":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(fluidsOther);
                        break;
                    case "TREATMENTS BLOOD PRODUCTS":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(bloodProducts);
                        break;
                    case "TREATMENTS BLOOD DETAILS":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(bloodDetails);
                        break;
                    case "TREATMENTS BLOOD DETAILS OTHER":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(bloodDetailsOther);
                        break;
                    case "TREATMENTS OTHER":
                        treatmentsStackPanel.Children.Clear();
                        treatmentsStackPanel.Children.Add(other);
                        break;
                }
                /*
                //Switch to injuries main
                if (message == "TREATMENTS MAIN")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(main);
                }

                if (message == "TREATMENTS CIRCULATION")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(circulation);
                }

                if (message == "TREATMENTS AIRWAY")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(airway);
                }

                if (message == "TREATMENTS BREATHING")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(breathing);
                }

                if (message == "TREATMENTS FLUIDS")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(fluids);
                }
                if (message == "TREATMENTS SALINE")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(saline);
                }
                if (message == "TREATMENTS RINGERS")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(ringers);
                }
                if (message == "TREATMENTS DEXTROSE")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(dextrose);
                }
                if (message == "TREATMENTS FLUIDS OTHER")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(fluidsOther);
                }
                if (message == "TREATMENTS BLOOD PRODUCTS")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(bloodProducts);
                }
                if (message == "TREATMENTS BLOOD DETAILS")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(bloodDetails);
                }
                if (message == "TREATMENTS BLOOD DETAILS OTHER")
                {
                    treatmentsStackPanel.Children.Clear();
                    treatmentsStackPanel.Children.Add(bloodDetailsOther);
                }
                */
            }));

        }


    }
}

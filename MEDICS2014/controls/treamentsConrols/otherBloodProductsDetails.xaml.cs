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

namespace MEDICS2014.controls.treamentsConrols
{
    /// <summary>
    /// Interaction logic for otherBloodProductsDetails.xaml
    /// </summary>
    public partial class otherBloodProductsDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public otherBloodProductsDetails()
        {
            InitializeComponent();
            bindButtonsAndData();
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string Message = messageEvent.Message;
                handleMessageData(Message);
            }

        }

        public void handleMessageData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "CLEAR CONTROL":
                        foreach (Button route in routeButtonsList)
                        {
                            route.Background = Brushes.Firebrick;
                            route.Foreground = Brushes.FloralWhite;
                        }
                        typeTextBox.Text = "";
                        doseTextBox.Text = "";
                        timeTextBox.Text = "";
                        globalPatient.treatments.bloodProducts.other.Dose = null;
                        globalPatient.treatments.bloodProducts.other.Route = null;
                        globalPatient.treatments.bloodProducts.other.Time = null;
                        globalPatient.treatments.bloodProducts.other.Type = null;
                        break;
                }
            }));
        }


        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                handlePatientData(Message);
            }

        }

        public void handlePatientData(patient p)
        {
            if (p.DBOperation || p.fromDatabase)
            {
                if (!isInFocus)
                {
                    //check that other isn't null
                    if (p.treatments.bloodProducts.other.Dose == null && p.treatments.bloodProducts.other.Route == null && p.treatments.bloodProducts.other.Time == null && p.treatments.bloodProducts.other.Type == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        globalPatient.treatments.bloodProducts.other.Dose = p.treatments.bloodProducts.other.Dose;
                        globalPatient.treatments.bloodProducts.other.Route = p.treatments.bloodProducts.other.Route;
                        globalPatient.treatments.bloodProducts.other.Time = p.treatments.bloodProducts.other.Time;
                        globalPatient.treatments.bloodProducts.other.Type = p.treatments.bloodProducts.other.Type;
                    }

                    this.Dispatcher.Invoke((Action)(() =>
                    {

                        if (globalPatient.treatments.bloodProducts.other.Type != null)
                        {
                            typeTextBox.Text = globalPatient.treatments.bloodProducts.other.Type;
                        }
                        else
                        {
                            typeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.bloodProducts.other.Dose != null)
                        {
                            doseTextBox.Text = globalPatient.treatments.bloodProducts.other.Dose;
                        }
                        else
                        {
                            doseTextBox.Text = "";
                        }
                        if (globalPatient.treatments.bloodProducts.other.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.bloodProducts.other.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.bloodProducts.other.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.bloodProducts.other.Route) && globalPatient.treatments.bloodProducts.other.Route != "")
                                {
                                    //check only the button that matches the route
                                    route.Background = Brushes.Yellow;
                                    route.Foreground = Brushes.Black;

                                }
                            }
                        }
                        else
                        {
                            //uncheck all the buttons
                            foreach (Button route in routeButtonsList)
                            {
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;
                            }
                        }

                    }));
                }
            }
        }

        private void bindButtonsAndData()
        {
            otherButtonIM.Background = Brushes.Firebrick;
            otherButtonIM.Foreground = Brushes.FloralWhite;
            otherButtonIV.Background = Brushes.Firebrick;
            otherButtonIV.Foreground = Brushes.FloralWhite;
            otherButtonPO.Background = Brushes.Firebrick;
            otherButtonPO.Foreground = Brushes.FloralWhite;
            otherButtonPR.Background = Brushes.Firebrick;
            otherButtonPR.Foreground = Brushes.FloralWhite;
            otherButtonSL.Background = Brushes.Firebrick;
            otherButtonSL.Foreground = Brushes.FloralWhite;
            otherButtonSQ.Background = Brushes.Firebrick;
            otherButtonSQ.Foreground = Brushes.FloralWhite;

            //bind the route button list
            routeButtonsList.Add(otherButtonIM);
            routeButtonsList.Add(otherButtonIV);
            routeButtonsList.Add(otherButtonPO);
            routeButtonsList.Add(otherButtonPR);
            routeButtonsList.Add(otherButtonSL);
            routeButtonsList.Add(otherButtonSQ);


        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            grabFromTextBoxes();
            isInFocus = false;

            if (globalPatient.treatments.bloodProducts.other.Type == null & globalPatient.treatments.bloodProducts.other.Route == null && globalPatient.treatments.bloodProducts.other.Dose == null)
            {
                //do nothing
            }
            else
            {
                if (globalPatient.treatments.bloodProducts.other.Time == null)
                {
                    //if there is no time assigned create one and send it
                    globalPatient.treatments.bloodProducts.other.Time = DateTime.Now.ToString("HHmm");
                }
                else
                {
                    globalPatient.DBOperation = true;
                    _systemMessages.AddMessage(globalPatient);
                }
            }




            _messages.AddMessage("TREATMENTS BLOOD PRODUCTS");
        }

        private void grabFromTextBoxes()
        {
            globalPatient.treatments.bloodProducts.other.Type = typeTextBox.Text.ToString();
            globalPatient.treatments.bloodProducts.other.Dose = doseTextBox.Text.ToString();
            globalPatient.treatments.bloodProducts.other.Time = timeTextBox.Text.ToString();

        }

        private void otherRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;
            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.bloodProducts.other.Route = "";
            }

            else
            {
                //Go through the list and uncheck any other button
                foreach (Button route in routeButtonsList)
                {
                    route.Background = Brushes.Firebrick;
                    route.Foreground = Brushes.FloralWhite;
                }


                //then click the button mentioned
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;

                //and add it to the patient
                globalPatient.treatments.bloodProducts.other.Route = b.Content.ToString();
            }
        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;
            isInFocus = true;
            globalPatient.treatments.bloodProducts.other.Time = time;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

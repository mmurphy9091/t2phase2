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
    /// Interaction logic for dextroseDetails.xaml
    /// </summary>
    public partial class dextroseDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();

        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public dextroseDetails()
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
                        //might not need anything here
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
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (p.fromVDES)
                {
                    bool needsUppdate = false;
                    if (p.treatments.fluids.dextrose.Dose != null)
                    {
                        globalPatient.treatments.fluids.dextrose.Dose = p.treatments.fluids.dextrose.Dose;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.dextrose.Route != null)
                    {
                        globalPatient.treatments.fluids.dextrose.Route = p.treatments.fluids.dextrose.Route;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.dextrose.Time != null)
                    {
                        globalPatient.treatments.fluids.dextrose.Time = p.treatments.fluids.dextrose.Time;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.dextrose.Percentage != null)
                    {
                        globalPatient.treatments.fluids.dextrose.Percentage = p.treatments.fluids.dextrose.Percentage;
                        needsUppdate = true;
                    }


                    if (needsUppdate)
                    {
                        if (globalPatient.treatments.fluids.dextrose.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.dextrose.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.dextrose.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.dextrose.Route) && globalPatient.treatments.fluids.dextrose.Route != "")
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
                        //unlick all the buttons
                        dextroseButton1000ml.Background = Brushes.Firebrick;
                        dextroseButton1000ml.Foreground = Brushes.FloralWhite;
                        dextroseButton250ml.Background = Brushes.Firebrick;
                        dextroseButton250ml.Foreground = Brushes.FloralWhite;
                        dextroseButton500ml.Background = Brushes.Firebrick;
                        dextroseButton500ml.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.dextrose.Dose != null)
                        {
                            if (globalPatient.treatments.fluids.dextrose.Dose == "250ml")
                            {
                                dextroseButton250ml.Background = Brushes.Yellow;
                                dextroseButton250ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Dose == "500ml")
                            {
                                dextroseButton500ml.Background = Brushes.Yellow;
                                dextroseButton500ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Dose == "1000ml")
                            {
                                dextroseButton1000ml.Background = Brushes.Yellow;
                                dextroseButton1000ml.Foreground = Brushes.Black;
                            }
                        }
                        else
                        {
                            //do something else
                        }
                        //unclick both buttons 
                        dextroseButton10Percent.Background = Brushes.Firebrick;
                        dextroseButton10Percent.Foreground = Brushes.FloralWhite;
                        dextroseButton5Percent.Background = Brushes.Firebrick;
                        dextroseButton5Percent.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.dextrose.Percentage != null)
                        {
                            if (globalPatient.treatments.fluids.dextrose.Percentage == "10%")
                            {
                                dextroseButton10Percent.Background = Brushes.Yellow;
                                dextroseButton10Percent.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Percentage == "5%")
                            {
                                dextroseButton5Percent.Background = Brushes.Yellow;
                                dextroseButton5Percent.Foreground = Brushes.Black;
                            }
                        }
                    }
                }
                else if (p.DBOperation || p.fromDatabase)
                {
                    //check that noraml saline isn't null
                    if (p.treatments.fluids.normalSaline.Dose == null && p.treatments.fluids.normalSaline.Route == null && p.treatments.fluids.normalSaline.Time == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        globalPatient.treatments.fluids.normalSaline.Dose = p.treatments.fluids.normalSaline.Dose;
                        globalPatient.treatments.fluids.normalSaline.Route = p.treatments.fluids.normalSaline.Route;
                        globalPatient.treatments.fluids.normalSaline.Time = p.treatments.fluids.normalSaline.Time;
                    }
                    //check that lactacted ringers isn't null
                    if (p.treatments.fluids.lactactedRingers.Dose == null && p.treatments.fluids.lactactedRingers.Route == null && p.treatments.fluids.lactactedRingers.Time == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        globalPatient.treatments.fluids.lactactedRingers.Dose = p.treatments.fluids.lactactedRingers.Dose;
                        globalPatient.treatments.fluids.lactactedRingers.Route = p.treatments.fluids.lactactedRingers.Route;
                        globalPatient.treatments.fluids.lactactedRingers.Time = p.treatments.fluids.lactactedRingers.Time;
                    }
                    //check that dextrose isn't null
                    if (!isInFocus)
                    {
                        if (p.treatments.fluids.dextrose.Dose == null && p.treatments.fluids.dextrose.Percentage == null && p.treatments.fluids.dextrose.Route == null && p.treatments.fluids.dextrose.Time == null)
                        {
                            //do nothing
                        }
                        else
                        {
                            globalPatient.treatments.fluids.dextrose.Dose = p.treatments.fluids.dextrose.Dose;
                            globalPatient.treatments.fluids.dextrose.Route = p.treatments.fluids.dextrose.Route;
                            globalPatient.treatments.fluids.dextrose.Time = p.treatments.fluids.dextrose.Time;
                            globalPatient.treatments.fluids.dextrose.Percentage = p.treatments.fluids.dextrose.Percentage;
                        }
                    }
                    //check that other isn't null
                    if (p.treatments.fluids.other.Dose == null && p.treatments.fluids.other.Route == null && p.treatments.fluids.other.Time == null && p.treatments.fluids.other.Type == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        globalPatient.treatments.fluids.other.Dose = p.treatments.fluids.other.Dose;
                        globalPatient.treatments.fluids.other.Route = p.treatments.fluids.other.Route;
                        globalPatient.treatments.fluids.other.Time = p.treatments.fluids.other.Time;
                        globalPatient.treatments.fluids.other.Type = p.treatments.fluids.other.Type;
                    }


                    if (!isInFocus)
                    {
                        if (globalPatient.treatments.fluids.dextrose.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.dextrose.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.dextrose.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.dextrose.Route) && globalPatient.treatments.fluids.dextrose.Route != "")
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
                        //unlick all the buttons
                        dextroseButton1000ml.Background = Brushes.Firebrick;
                        dextroseButton1000ml.Foreground = Brushes.FloralWhite;
                        dextroseButton250ml.Background = Brushes.Firebrick;
                        dextroseButton250ml.Foreground = Brushes.FloralWhite;
                        dextroseButton500ml.Background = Brushes.Firebrick;
                        dextroseButton500ml.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.dextrose.Dose != null)
                        {
                            if (globalPatient.treatments.fluids.dextrose.Dose == "250ml")
                            {
                                dextroseButton250ml.Background = Brushes.Yellow;
                                dextroseButton250ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Dose == "500ml")
                            {
                                dextroseButton500ml.Background = Brushes.Yellow;
                                dextroseButton500ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Dose == "1000ml")
                            {
                                dextroseButton1000ml.Background = Brushes.Yellow;
                                dextroseButton1000ml.Foreground = Brushes.Black;
                            }
                        }
                        else
                        {
                            //do something else
                        }
                        //unclick both buttons 
                        dextroseButton10Percent.Background = Brushes.Firebrick;
                        dextroseButton10Percent.Foreground = Brushes.FloralWhite;
                        dextroseButton5Percent.Background = Brushes.Firebrick;
                        dextroseButton5Percent.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.dextrose.Percentage != null)
                        {
                            if (globalPatient.treatments.fluids.dextrose.Percentage == "10%")
                            {
                                dextroseButton10Percent.Background = Brushes.Yellow;
                                dextroseButton10Percent.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.dextrose.Percentage == "5%")
                            {
                                dextroseButton5Percent.Background = Brushes.Yellow;
                                dextroseButton5Percent.Foreground = Brushes.Black;
                            }
                        }
                    }


                }
            }));
        }

        private void bindButtonsAndData()
        {
            dextroseButton5Percent.Background = Brushes.Firebrick;
            dextroseButton5Percent.Foreground = Brushes.FloralWhite;
            dextroseButton10Percent.Background = Brushes.Firebrick;
            dextroseButton10Percent.Foreground = Brushes.FloralWhite;

            dextroseButton1000ml.Background = Brushes.Firebrick;
            dextroseButton1000ml.Foreground = Brushes.FloralWhite;
            dextroseButton250ml.Background = Brushes.Firebrick;
            dextroseButton250ml.Foreground = Brushes.FloralWhite;
            dextroseButton500ml.Background = Brushes.Firebrick;
            dextroseButton500ml.Foreground = Brushes.FloralWhite;

            dextroseButtonIM.Background = Brushes.Firebrick;
            dextroseButtonIM.Foreground = Brushes.FloralWhite;
            dextroseButtonIV.Background = Brushes.Firebrick;
            dextroseButtonIV.Foreground = Brushes.FloralWhite;
            dextroseButtonPO.Background = Brushes.Firebrick;
            dextroseButtonPO.Foreground = Brushes.FloralWhite;
            dextroseButtonPR.Background = Brushes.Firebrick;
            dextroseButtonPR.Foreground = Brushes.FloralWhite;
            dextroseButtonSL.Background = Brushes.Firebrick;
            dextroseButtonSL.Foreground = Brushes.FloralWhite;
            dextroseButtonSQ.Background = Brushes.Firebrick;
            dextroseButtonSQ.Foreground = Brushes.FloralWhite;

            //bind the route button list
            routeButtonsList.Add(dextroseButtonIM);
            routeButtonsList.Add(dextroseButtonIV);
            routeButtonsList.Add(dextroseButtonPO);
            routeButtonsList.Add(dextroseButtonPR);
            routeButtonsList.Add(dextroseButtonSL);
            routeButtonsList.Add(dextroseButtonSQ);


        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;
            isInFocus = true;
            globalPatient.treatments.fluids.dextrose.Time = time;
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            isInFocus = false;
            globalPatient.treatments.fluids.dextrose.Time = timeTextBox.Text.ToString();
                if (globalPatient.treatments.fluids.dextrose.Time == null)
                {
                    //if there is no time assigned create one and send it
                    globalPatient.treatments.fluids.dextrose.Time = DateTime.Now.ToString("HHmm");
                }

                globalPatient.DBOperation = true;
                _systemMessages.AddMessage(globalPatient);
            

            _messages.AddMessage("TREATMENTS FLUIDS");
        }

        private void dextroseVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.fluids.dextrose.Dose = "";
            }
            else
            {
                //unlick all the buttons
                dextroseButton1000ml.Background = Brushes.Firebrick;
                dextroseButton1000ml.Foreground = Brushes.FloralWhite;
                dextroseButton250ml.Background = Brushes.Firebrick;
                dextroseButton250ml.Foreground = Brushes.FloralWhite;
                dextroseButton500ml.Background = Brushes.Firebrick;
                dextroseButton500ml.Foreground = Brushes.FloralWhite;


                //then click the button mentioned
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;

                //and add it to the patient
                globalPatient.treatments.fluids.dextrose.Dose = b.Content.ToString();
            }


        }

        private void dextroseRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;
            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.fluids.dextrose.Route = "";
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
                globalPatient.treatments.fluids.dextrose.Route = b.Content.ToString();
            }


        }

        private void dextrosePercentageButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;
            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.fluids.dextrose.Percentage = "";
                //globalPatient.treatments.fluids.dextrose = false;
            }

            else
            {

                //unclick both buttons 
                dextroseButton10Percent.Background = Brushes.Firebrick;
                dextroseButton10Percent.Foreground = Brushes.FloralWhite;
                dextroseButton5Percent.Background = Brushes.Firebrick;
                dextroseButton5Percent.Foreground = Brushes.FloralWhite;

                //then click the button mentioned
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;

                //and add it to the patient
                globalPatient.treatments.fluids.dextrose.Percentage = b.Content.ToString();
            }

        }
        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

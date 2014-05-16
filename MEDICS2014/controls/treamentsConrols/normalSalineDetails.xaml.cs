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
    /// Interaction logic for normalSalineDetails.xaml
    /// </summary>
    public partial class normalSalineDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();

        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public normalSalineDetails()
        {
            InitializeComponent();

            bindButtonsAndData();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

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
                    if (p.treatments.fluids.normalSaline.Dose != null )
                    {
                        globalPatient.treatments.fluids.normalSaline.Dose = p.treatments.fluids.normalSaline.Dose;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.normalSaline.Route != null)
                    {
                        globalPatient.treatments.fluids.normalSaline.Route = p.treatments.fluids.normalSaline.Route;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.normalSaline.Time != null)
                    {
                        globalPatient.treatments.fluids.normalSaline.Time = p.treatments.fluids.normalSaline.Time;
                        needsUppdate = true;
                    }

                    if (needsUppdate)
                    {
                        if (globalPatient.treatments.fluids.normalSaline.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.normalSaline.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.normalSaline.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.normalSaline.Route) && globalPatient.treatments.fluids.normalSaline.Route != "")
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
                        normalSalineButton1000ml.Background = Brushes.Firebrick;
                        normalSalineButton1000ml.Foreground = Brushes.FloralWhite;
                        normalSalineButton250ml.Background = Brushes.Firebrick;
                        normalSalineButton250ml.Foreground = Brushes.FloralWhite;
                        normalSalineButton500ml.Background = Brushes.Firebrick;
                        normalSalineButton500ml.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.normalSaline.Dose != null)
                        {
                            if (globalPatient.treatments.fluids.normalSaline.Dose == "250ml")
                            {
                                normalSalineButton250ml.Background = Brushes.Yellow;
                                normalSalineButton250ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.normalSaline.Dose == "500ml")
                            {
                                normalSalineButton500ml.Background = Brushes.Yellow;
                                normalSalineButton500ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.normalSaline.Dose == "1000ml")
                            {
                                normalSalineButton1000ml.Background = Brushes.Yellow;
                                normalSalineButton1000ml.Foreground = Brushes.Black;
                            }
                        }
                        else
                        {
                            //do something else
                        }
                    }
                }
                else if (p.DBOperation || p.fromDatabase)
                {
                    if (!isInFocus)
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
                        if (globalPatient.treatments.fluids.normalSaline.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.normalSaline.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.normalSaline.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.normalSaline.Route) && globalPatient.treatments.fluids.normalSaline.Route != "")
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
                        normalSalineButton1000ml.Background = Brushes.Firebrick;
                        normalSalineButton1000ml.Foreground = Brushes.FloralWhite;
                        normalSalineButton250ml.Background = Brushes.Firebrick;
                        normalSalineButton250ml.Foreground = Brushes.FloralWhite;
                        normalSalineButton500ml.Background = Brushes.Firebrick;
                        normalSalineButton500ml.Foreground = Brushes.FloralWhite;
                        if (globalPatient.treatments.fluids.normalSaline.Dose != null)
                        {
                            if (globalPatient.treatments.fluids.normalSaline.Dose == "250ml")
                            {
                                normalSalineButton250ml.Background = Brushes.Yellow;
                                normalSalineButton250ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.normalSaline.Dose == "500ml")
                            {
                                normalSalineButton500ml.Background = Brushes.Yellow;
                                normalSalineButton500ml.Foreground = Brushes.Black;
                            }
                            else if (globalPatient.treatments.fluids.normalSaline.Dose == "1000ml")
                            {
                                normalSalineButton1000ml.Background = Brushes.Yellow;
                                normalSalineButton1000ml.Foreground = Brushes.Black;
                            }
                        }
                        else
                        {
                            //do something else
                        }
                    }


                }
            }));
        }


        private void bindButtonsAndData()
        {
            normalSalineButton1000ml.Background = Brushes.Firebrick;
            normalSalineButton1000ml.Foreground = Brushes.FloralWhite;
            normalSalineButton250ml.Background = Brushes.Firebrick;
            normalSalineButton250ml.Foreground = Brushes.FloralWhite;
            normalSalineButton500ml.Background = Brushes.Firebrick;
            normalSalineButton500ml.Foreground = Brushes.FloralWhite;

            normalSalineButtonIM.Background = Brushes.Firebrick;
            normalSalineButtonIM.Foreground = Brushes.FloralWhite;
            normalSalineButtonIV.Background = Brushes.Firebrick;
            normalSalineButtonIV.Foreground = Brushes.FloralWhite;
            normalSalineButtonPO.Background = Brushes.Firebrick;
            normalSalineButtonPO.Foreground = Brushes.FloralWhite;
            normalSalineButtonPR.Background = Brushes.Firebrick;
            normalSalineButtonPR.Foreground = Brushes.FloralWhite;
            normalSalineButtonSL.Background = Brushes.Firebrick;
            normalSalineButtonSL.Foreground = Brushes.FloralWhite;
            normalSalineButtonSQ.Background = Brushes.Firebrick;
            normalSalineButtonSQ.Foreground = Brushes.FloralWhite;

            //bind the route button list
            routeButtonsList.Add(normalSalineButtonIM);
            routeButtonsList.Add(normalSalineButtonIV);
            routeButtonsList.Add(normalSalineButtonPO);
            routeButtonsList.Add(normalSalineButtonPR);
            routeButtonsList.Add(normalSalineButtonSL);
            routeButtonsList.Add(normalSalineButtonSQ);


        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;

            globalPatient.treatments.fluids.normalSaline.Time = time;
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            globalPatient.treatments.fluids.normalSaline.Time = timeTextBox.Text.ToString();
            isInFocus = false;
                if (globalPatient.treatments.fluids.normalSaline.Time == null)
                {
                    //if there is no time assigned create one and send it
                    globalPatient.treatments.fluids.normalSaline.Time = DateTime.Now.ToString("HHmm");
                }
                globalPatient.DBOperation = true;
                _systemMessages.AddMessage(globalPatient);
            

            _messages.AddMessage("TREATMENTS FLUIDS");
        }

        private void normalSalineVolumeButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.fluids.normalSaline.Dose = "";
            }
            else
            {
                //unlick all the buttons
                normalSalineButton1000ml.Background = Brushes.Firebrick;
                normalSalineButton1000ml.Foreground = Brushes.FloralWhite;
                normalSalineButton250ml.Background = Brushes.Firebrick;
                normalSalineButton250ml.Foreground = Brushes.FloralWhite;
                normalSalineButton500ml.Background = Brushes.Firebrick;
                normalSalineButton500ml.Foreground = Brushes.FloralWhite;


                //then click the button mentioned
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;

                //and add it to the patient
                globalPatient.treatments.fluids.normalSaline.Dose = b.Content.ToString();
            }
        }

        private void normalSalineRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                globalPatient.treatments.fluids.normalSaline.Route = "";
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
                globalPatient.treatments.fluids.normalSaline.Route = b.Content.ToString();
            }

        }


    }
}

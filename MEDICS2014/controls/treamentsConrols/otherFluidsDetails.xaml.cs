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
    /// Interaction logic for otherFluidsDetails.xaml
    /// </summary>
    public partial class otherFluidsDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public otherFluidsDetails()
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
                    if (p.treatments.fluids.other.Dose != null)
                    {
                        globalPatient.treatments.fluids.other.Dose = p.treatments.fluids.other.Dose;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.other.Route != null)
                    {
                        globalPatient.treatments.fluids.other.Route = p.treatments.fluids.other.Route;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.other.Time != null)
                    {
                        globalPatient.treatments.fluids.other.Time = p.treatments.fluids.other.Time;
                        needsUppdate = true;
                    }
                    if (p.treatments.fluids.other.Type != null)
                    {
                        globalPatient.treatments.fluids.other.Type = p.treatments.fluids.other.Type;
                        needsUppdate = true;
                    }


                    if (needsUppdate)
                    {
                        if (globalPatient.treatments.fluids.other.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.other.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.other.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.other.Route) && globalPatient.treatments.fluids.other.Route != "")
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
                        //dose
                        if (globalPatient.treatments.fluids.other.Dose != null)
                        {
                            doseTextBox.Text = globalPatient.treatments.fluids.other.Dose;
                        }
                        else
                        {
                            doseTextBox.Text = "";
                        }
                        //type
                        if (globalPatient.treatments.fluids.other.Type != null)
                        {
                            typeTextBox.Text = globalPatient.treatments.fluids.other.Type;
                        }
                        else
                        {
                            typeTextBox.Text = "";
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
                    if (!isInFocus)
                    {
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
                    }


                    if (!isInFocus)
                    {
                        if (globalPatient.treatments.fluids.other.Time != null)
                        {
                            timeTextBox.Text = globalPatient.treatments.fluids.other.Time;
                        }
                        else
                        {
                            timeTextBox.Text = "";
                        }
                        if (globalPatient.treatments.fluids.other.Route != null)
                        {
                            foreach (Button route in routeButtonsList)
                            {
                                //uncheck every button
                                route.Background = Brushes.Firebrick;
                                route.Foreground = Brushes.FloralWhite;

                                if (route.Content.ToString().Contains(globalPatient.treatments.fluids.other.Route) && globalPatient.treatments.fluids.other.Route != "")
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
                        //dose
                        if (globalPatient.treatments.fluids.other.Dose != null)
                        {
                            doseTextBox.Text = globalPatient.treatments.fluids.other.Dose;
                        }
                        else
                        {
                            doseTextBox.Text = "";
                        }
                        //type
                        if (globalPatient.treatments.fluids.other.Type != null)
                        {
                            typeTextBox.Text = globalPatient.treatments.fluids.other.Type;
                        }
                        else
                        {
                            typeTextBox.Text = "";
                        }
                    }
                }
            }));
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
                    if (globalPatient.treatments.fluids.other.Type == null & globalPatient.treatments.fluids.other.Route == null && globalPatient.treatments.fluids.other.Dose == null)
                    {
                        //do nothing
                    }
                    else
                    {
                        if (globalPatient.treatments.fluids.other.Time == null)
                        {
                            //if there is no time assigned create one and send it
                            globalPatient.treatments.fluids.other.Time = DateTime.Now.ToString("HHmm");
                        }
                        else
                        {
                            globalPatient.DBOperation = true;
                            _systemMessages.AddMessage(globalPatient);
                        }
                    }
                
                
            

            _messages.AddMessage("TREATMENTS FLUIDS");
        }

        private void grabFromTextBoxes()
        {
                globalPatient.treatments.fluids.other.Type = typeTextBox.Text.ToString();
                globalPatient.treatments.fluids.other.Dose = doseTextBox.Text.ToString();
                globalPatient.treatments.fluids.other.Time = timeTextBox.Text.ToString();

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
                globalPatient.treatments.fluids.other.Route = "";
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
                globalPatient.treatments.fluids.other.Route = b.Content.ToString();
            }
        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;
            isInFocus = true;
            globalPatient.treatments.fluids.other.Time = time;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

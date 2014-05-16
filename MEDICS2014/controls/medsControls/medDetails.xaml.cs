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
using System.Reflection;

namespace MEDICS2014.controls.medsControls
{
    /// <summary>
    /// Interaction logic for medDetails.xaml
    /// </summary>
    public partial class medDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        patient.MedicationsDetails currentMed = new patient.MedicationsDetails();

        //string currentData = "";

        List<string> medTypes = new List<string>();

        patient.MedicationsDetails newMed = new patient.MedicationsDetails();

        public bool isInFocus = false;


        public medDetails()
        {
            InitializeComponent();

            bindButtonsAndData();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
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
                    case "CLEAR CONTROL":
                        //Hide all the things
                        if (!isInFocus)
                        {
                            globalPatient = new patient();
                            //clear everything but the current meds name
                            currentMed.Dose = null;
                            currentMed.Route = null;
                            currentMed.Time = null;
                            currentMed.Units = null;
                            isInFocus = false;
                        }
                        break;
                }
            }));

            if (!isInFocus)
            {
                bindTextBoxes();
            }

        }

        private void assignMed()
        {
            //check if the current med is already in the list
            foreach (patient.MedicationsDetails med in globalPatient.Medications)
            {
                if (med.Name == currentMed.Name)
                {
                    med.Dose = currentMed.Dose;
                    med.Route = currentMed.Route;
                    med.Time = currentMed.Time;
                    return;
                }
            }
            //if we can't find it in the list
            globalPatient.Medications.Add(currentMed);
            /*
            if (typeLabel.Content.ToString() == "Whole Blood")
            {
                globalPatient.treatments.bloodProducts.wholeBlood = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Packed Red Blood")
            {
                globalPatient.treatments.bloodProducts.packedRedBlood = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Whole Plasma")
            {
                globalPatient.treatments.bloodProducts.wholePlasma = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Cryoprecipitate Plasma")
            {
                globalPatient.treatments.bloodProducts.cryoprecipitate = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Platelete Rich Plasma")
            {
                globalPatient.treatments.bloodProducts.plateleteRichPlasma = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Plasmanate")
            {
                globalPatient.treatments.bloodProducts.plasmanate = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Hextend")
            {
                globalPatient.treatments.bloodProducts.hextend = currentMed;
            }
            else if (typeLabel.Content.ToString() == "Serum Albumin")
            {
                globalPatient.treatments.bloodProducts.serumAlbum = currentMed;
            }
             */
        }

        public void bindTextBoxes()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (currentMed.Other == "True")
                {
                    typeLabel.Visibility = Visibility.Hidden;
                    nameTextBox.Visibility = Visibility.Visible;
                    nameTextBox.Text = currentMed.Name;
                }
                else
                {
                    typeLabel.Visibility = Visibility.Visible;
                    nameTextBox.Visibility = Visibility.Hidden;
                }
                if (currentMed.Name != null)
                {
                    typeLabel.Content = currentMed.Name;
                }
                if (currentMed.Dose != null)
                {
                    doseTextBox.Text = currentMed.Dose;
                }
                else
                {
                    doseTextBox.Text = "";
                }
                if (currentMed.Time != null)
                {
                    timeTextBox.Text = currentMed.Time;
                }
                else
                {
                    timeTextBox.Text = "";
                }
                if (currentMed.Units != null)
                {
                    unitsButon.Content = currentMed.Units;
                }
                else
                {
                    //leave it the default
                    unitsButon.Content = "units";
                }
                if (currentMed.Route != null)
                {
                    if (currentMed.Route != "")
                    {
                        foreach (Button route in routeButtonsList)
                        {
                            //uncheck every button
                            route.Background = Brushes.Firebrick;
                            route.Foreground = Brushes.FloralWhite;

                            if (route.Content.ToString().Contains(currentMed.Route) && currentMed.Route != "")
                            {
                                //check only the button that matches the route
                                route.Background = Brushes.Yellow;
                                route.Foreground = Brushes.Black;

                            }
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
                //if this is data from database
                if (p.fromDatabase)
                {
                    //Copy the list
                    globalPatient.Medications = p.Medications;

                    foreach (patient.MedicationsDetails med in globalPatient.Medications)
                    {
                        if (med.Name == currentMed.Name)
                        {
                            if (!isInFocus)
                            {
                                //if we are not currently open
                                currentMed = med;
                                bindTextBoxes();
                            }
                            
                        }
                    }
                }
                //if this is a new med
                else if (p.tempMed.Name != null)
                {
                    if (globalPatient.Medications.Count > 0)
                    {
                        foreach (patient.MedicationsDetails med in globalPatient.Medications)
                        {
                            if (med.Name == p.tempMed.Name)
                            {
                                if (p.fromVDES)
                                {
                                    if (p.tempMed.Route != null)
                                    {
                                        currentMed.Route = p.tempMed.Route;
                                        med.Route = p.tempMed.Route;
                                    }
                                    if (p.tempMed.Time != null)
                                    {
                                        currentMed.Time = p.tempMed.Time;
                                        med.Time = p.tempMed.Time;
                                    }
                                    if (p.tempMed.Units != null)
                                    {
                                        currentMed.Units = p.tempMed.Units;
                                        med.Units = p.tempMed.Units;
                                    }
                                    if (p.tempMed.Dose != null)
                                    {
                                        currentMed.Dose = p.tempMed.Dose;
                                        med.Dose = p.tempMed.Dose;
                                    }
                                }
                                else
                                {
                                    currentMed = med;
                                }
                                bindTextBoxes();
                                return;
                            }
                        }
                        
                    }
                    
                    //if the med isn't in the list
                    currentMed = new patient.MedicationsDetails();

                    if (p.fromVDES)
                    {
                        if (p.tempMed.Name != null)
                        {
                            currentMed.Name = p.tempMed.Name;
                        }
                        if (p.tempMed.Route != null)
                        {
                            currentMed.Route = p.tempMed.Route;
                        }
                        if (p.tempMed.Time != null)
                        {
                            currentMed.Time = p.tempMed.Time;
                        }
                        if (p.tempMed.Units != null)
                        {
                            currentMed.Units = p.tempMed.Units;
                        }
                        if (p.tempMed.Dose != null)
                        {
                            currentMed.Dose = p.tempMed.Dose;
                        }
                        globalPatient.Medications.Add(currentMed);
                    }
                    else
                    {
                        currentMed.Name = p.tempMed.Name;
                        currentMed.Analgesic = p.tempMed.Analgesic;
                        currentMed.Antibiotic = p.tempMed.Antibiotic;
                        currentMed.Other = p.tempMed.Other;
                    }
                    bindTextBoxes();
                }
                //If this is an update from the units page
                else if (p.medUnits != null)
                {
                    unitsButon.Content = p.medUnits;

                    currentMed.Units = p.medUnits;
                }
                //If this is a VDES operation
                else if (p.fromVDES)
                {

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

            //create the blood types list
            medTypes.Add("Dilaudid");
            medTypes.Add("Fentanyl");
            medTypes.Add("Hydrocodone");
            medTypes.Add("Ibuprofen");
            medTypes.Add("Ketamine");
            medTypes.Add("Morphine");
            medTypes.Add("Oxycodone");
            medTypes.Add("Percocet");
            medTypes.Add("Toradol");
            medTypes.Add("Tylenol");
            medTypes.Add("Vicodin");
            medTypes.Add("Amoxicillin");
            medTypes.Add("Azithromycin");
            medTypes.Add("Cefalexin");
            medTypes.Add("Cipro");
            medTypes.Add("Clindamycin");
            medTypes.Add("Erythromycin");
            medTypes.Add("Levaquin");
            medTypes.Add("Metronidazole");
            medTypes.Add("Neomycin");
            medTypes.Add("Penicillin");
            medTypes.Add("Septra");
            medTypes.Add("Silvadene");
            medTypes.Add("Streptomycin");
            medTypes.Add("Tetracycline");
            medTypes.Add("Vancomycin");

            //Hide the name text box
            nameTextBox.Visibility = Visibility.Hidden;
        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;

            currentMed.Time = time;
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentMed.Other == "True")
            {
                currentMed.Name = nameTextBox.Text.ToString();
            }
            currentMed.Dose = doseTextBox.Text.ToString();
            currentMed.Time = timeTextBox.Text.ToString();
            if (unitsButon.Content.ToString() == "units")
            {
                currentMed.Units = "";
            }
            else
            {
                currentMed.Units = unitsButon.Content.ToString();
            }
            if (currentMed.Route == null)
            {
                currentMed.Route = "";
            }
            assignMed();

            //create a new patient with just that one med
            patient databasePatient = new patient();
            databasePatient.tempMed = currentMed;
            databasePatient.Medications = globalPatient.Medications;
            databasePatient.DBOperation = true;
            _systemMessages.AddMessage(databasePatient);
            _messages.AddMessage("MEDS");

            isInFocus = false;
        }
    


        private void unitsButon_Click(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
            //bind the entered data first
            if (currentMed.Other == "True")
            {
                currentMed.Name = nameTextBox.Text.ToString();
                currentMed.Dose = doseTextBox.Text.ToString();
            }
            else
            {
                currentMed.Dose = doseTextBox.Text.ToString();
            }
            _messages.AddMessage("MEDS UNITS");
        }

        private void routeButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                currentMed.Route = "";
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
                currentMed.Route = b.Content.ToString();
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

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

namespace MEDICS2014.controls.injuriesControls
{
    /// <summary>
    /// Interaction logic for injuriesLeftLeg.xaml
    /// </summary>
    public partial class injuriesLeftLeg : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        //Global lists
        List<Rectangle> allBoxesList = new List<Rectangle>();
        List<Rectangle> allLinesList = new List<Rectangle>();
        List<Label> allLabelsList = new List<Label>();

        patient globalPatient = new patient();

        public injuriesLeftLeg()
        {
            InitializeComponent();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

            bindAllLists();
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
                        //Hide all the things
                        foreach (Label l in allLabelsList)
                        {
                            l.Content = "";
                            l.Visibility = Visibility.Hidden;
                        }
                        foreach (Rectangle r in allLinesList)
                        {
                            r.Visibility = Visibility.Hidden;
                        }
                        foreach (Rectangle b in allBoxesList)
                        {
                            b.Opacity = 0;
                        }
                        typeTextBox.Text = "";
                        timeTextBox.Text = "";
                        break;
                }
            }));
        }

        private void bindAllLists()
        {
            //boxes
            allBoxesList.Add(antLeftProximalMedialThighBox);
            allBoxesList.Add(antLeftMiddleMedialThighBox);
            allBoxesList.Add(antLeftDistalMedialThighBox);
            allBoxesList.Add(antLeftProximalLateralThighBox);
            allBoxesList.Add(antLeftMiddleLateralThighBox);
            allBoxesList.Add(antLeftDistalLateralThighBox);
            allBoxesList.Add(antLeftKneeBox);
            allBoxesList.Add(antLeftProximalMedialLowerLegBox);
            allBoxesList.Add(antLeftMiddleMedialLowerLegBox);
            allBoxesList.Add(antLeftDistalMedialLowerLegBox);
            allBoxesList.Add(antLeftProximalLateralLowerLegBox);
            allBoxesList.Add(antLeftMiddleLateralLowerLegBox);
            allBoxesList.Add(antLeftDistalLateralLowerLegBox);
            allBoxesList.Add(antLeftAnkleBox);
            allBoxesList.Add(antLeftFootBox);
            allBoxesList.Add(antLeftBigToeBox);
            allBoxesList.Add(antLeftOtherToesBox);


            //labels
            allLabelsList.Add(antLeftProximalMedialThighLabel);
            allLabelsList.Add(antLeftMiddleMedialThighLabel);
            allLabelsList.Add(antLeftDistalMedialThighLabel);
            allLabelsList.Add(antLeftProximalLateralThighLabel);
            allLabelsList.Add(antLeftMiddleLateralThighLabel);
            allLabelsList.Add(antLeftDistalLateralThighLabel);
            allLabelsList.Add(antLeftKneeLabel);
            allLabelsList.Add(antLeftProximalMedialLowerLegLabel);
            allLabelsList.Add(antLeftMiddleMedialLowerLegLabel);
            allLabelsList.Add(antLeftDistalMedialLowerLegLabel);
            allLabelsList.Add(antLeftProximalLateralLowerLegLabel);
            allLabelsList.Add(antLeftMiddleLateralLowerLegLabel);
            allLabelsList.Add(antLeftDistalLateralLowerLegLabel);
            allLabelsList.Add(antLeftAnkleLabel);
            allLabelsList.Add(antLeftFootLabel);
            allLabelsList.Add(antLeftBigToeLabel);
            allLabelsList.Add(antLeftOtherToesLabel);


            //lines
            allLinesList.Add(antLeftProximalMedialThighLine);
            allLinesList.Add(antLeftMiddleMedialThighLine);
            allLinesList.Add(antLeftDistalMedialThighLine);
            allLinesList.Add(antLeftProximalLateralThighLine);
            allLinesList.Add(antLeftMiddleLateralThighLine);
            allLinesList.Add(antLeftDistalLateralThighLine);
            allLinesList.Add(antLeftKneeLine);
            allLinesList.Add(antLeftProximalMedialLowerLegLine);
            allLinesList.Add(antLeftMiddleMedialLowerLegLine);
            allLinesList.Add(antLeftDistalMedialLowerLegLine);
            allLinesList.Add(antLeftProximalLateralLowerLegLine);
            allLinesList.Add(antLeftMiddleLateralLowerLegLine);
            allLinesList.Add(antLeftDistalLateralLowerLegLine);
            allLinesList.Add(antLeftAnkleLine);
            allLinesList.Add(antLeftFootLine);
            allLinesList.Add(antLeftBigToeLine);
            allLinesList.Add(antLeftOtherToesLine);


            //Hide all the things
            foreach (Label l in allLabelsList)
            {
                l.Content = "";
                l.Visibility = Visibility.Hidden;
            }
            foreach (Rectangle r in allLinesList)
            {
                r.Visibility = Visibility.Hidden;
            }
            foreach (Rectangle b in allBoxesList)
            {
                b.Opacity = 0;
            }


        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("INJURIES MAIN");
        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;

            patient.TQ newTQ = new patient.TQ();
            bool isHere = false;
            //Check if this tq already exists
            foreach (patient.TQ tq in globalPatient.tqs)
            {
                if (tq.Location == "lleg")
                {
                    //tq = tq;
                    tq.Time = time;
                    isHere = true;
                    break;
                }
            }

            if (!isHere)
            {
                //create and add a new tq
                newTQ.Location = "lleg";
                newTQ.Time = time;
                globalPatient.tqs.Add(newTQ);
            }

            globalPatient.DBOperation = true;
            _systemMessages.AddMessage(globalPatient);

            //.Time = time; // no place to put it yet
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                handleControllerPatientData(Message);
            }

        }

        private void handleControllerPatientData(patient p)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    foreach (patient.TQ tq in p.tqs)
                    {
                        if (tq.Location == "lleg")
                        {
                            typeTextBox.Text = tq.Type.ToString();
                            timeTextBox.Text = tq.Time.ToString();
                        }
                    }
                    foreach (patient.Injuries i in p.injuries)
                    {
                        //If there is no content in the injury type, go to the next injury
                        if (i.Type == "" || i.Type == null)
                        {
                            break;
                        }
                        foreach (Rectangle b in allBoxesList)
                        {
                            string boxContent = b.Name.ToString();
                            if (boxContent.Contains(i.Location))
                            {
                                b.Opacity = 100;
                                foreach (Label l in allLabelsList)
                                {
                                    string labelContent = l.Name.ToString();
                                    if (labelContent.Contains(i.Location))
                                    {
                                        //check is injury needs to be deleted
                                        if (i.delete)
                                        {
                                            string old = l.Content.ToString();
                                            string notFirst = ", " + i.Type;
                                            //if this is the only entry in the label
                                            if (old == i.Type)
                                            {
                                                //clear the whole shebang
                                                l.Content = "";
                                                l.Visibility = Visibility.Hidden;
                                                b.Opacity = 0;
                                                foreach (Rectangle r in allLinesList)
                                                {
                                                    string lineContent = r.Name.ToString();
                                                    if (lineContent.Contains(i.Location))
                                                    {
                                                        r.Visibility = Visibility.Hidden;
                                                        break;
                                                    }
                                                }
                                                break;

                                            }
                                            //if this isn't the first entry in the label
                                            else if (old.Contains(notFirst))
                                            {
                                                string newData = old.Replace(notFirst, "");
                                                l.Content = newData;
                                            }
                                            //finally if this is just the first entry but not the only entry
                                            else if (old.Contains(i.Type))
                                            {
                                                string firstType = i.Type + ", ";
                                                string newData = old.Replace(firstType, "");
                                                l.Content = newData;
                                            }
                                        }
                                        else
                                        {
                                            l.Visibility = Visibility.Visible;
                                            //Check if injury type is already recorded
                                            string recordedInjuries = l.Content.ToString();
                                            if (!recordedInjuries.Contains(i.Type))
                                            {
                                                if (l.Content.ToString() == "")
                                                {
                                                    l.Content = i.Type;
                                                }
                                                else
                                                {
                                                    l.Content += ", " + i.Type;
                                                }

                                            }
                                            foreach (Rectangle r in allLinesList)
                                            {
                                                string lineContent = r.Name.ToString();
                                                if (lineContent.Contains(i.Location))
                                                {
                                                    r.Visibility = Visibility.Visible;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                catch { }
            }));
        }

        private void locationBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle location = (Rectangle)sender;

            //do the show and choose logirythm
            //get the injury types
            _messages.AddMessage("ADD INJURY");


            //send the patient data sans injury type
            string locationString = location.Name.ToString();
            locationString = locationString.Replace("Box", "");
            patient p = new patient();
            patient.Injuries i = new patient.Injuries();
            i.Location = locationString;
            if (location.Opacity == 100)
            {
                //add all the injuries needed
                //first make a list of all the types
                //find the type label
                foreach (Label typeLabel in allLabelsList)
                {
                    string labelName = typeLabel.Name.ToString();
                    if (labelName.Contains(locationString))
                    {
                        string content = typeLabel.Content.ToString();
                        IList<string> types = content.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string t in types)
                        {
                            patient.Injuries temp = new patient.Injuries();
                            temp.Location = locationString;
                            temp.Type = t;
                            p.injuries.Add(temp);
                        }
                    }
                }

            }
            else
            {
                //Just add the one injury
                p.injuries.Add(i);
            }
            _systemMessages.AddMessage(p);



        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ///string time = DateTime.Now.ToString("HHmm");
            //timeTextBox.Text = time;

            patient.TQ newTQ = new patient.TQ();
            bool isHere = false;
            //Check if this tq already exists
            foreach (patient.TQ tq in globalPatient.tqs)
            {
                if (tq.Location == "lleg")
                {
                    //tq = tq;
                    tq.Time = timeTextBox.Text.ToString();
                    tq.Type = typeTextBox.Text.ToString();
                    isHere = true;
                    break;
                }
            }

            if (!isHere)
            {
                //create and add a new tq
                newTQ.Location = "lleg";
                newTQ.Time = timeTextBox.Text.ToString();
                newTQ.Type = typeTextBox.Text.ToString();
                globalPatient.tqs.Add(newTQ);
            }

            globalPatient.DBOperation = true;
            _systemMessages.AddMessage(globalPatient);

        }

    }
}

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
    /// Interaction logic for mechanismOfInjury.xaml
    /// </summary>
    public partial class mechanismOfInjury : UserControl
    {
        SystemMessages _systemMessages = SystemMessages.Instance;

        Messages _messages = Messages.Instance;

        List<Button> allButtonsList = new List<Button>();

        List<string> allInjuryTypes = new List<string>();

        string location;

        List<string> Types = new List<string>();

        patient incomingPatient = new patient();

        string otherInjury = "";

        public mechanismOfInjury()
        {
            InitializeComponent();

            //Sets the colors for all the buttons to green, or whatever
            SelectButton(false, artilleryButton);
            SelectButton(false, bluntButton);
            SelectButton(false, burnButton);
            SelectButton(false, fallButton);
            SelectButton(false, grenadeButton);
            SelectButton(false, gswButton);
            SelectButton(false, iedButton);
            SelectButton(false, landmineButton);
            SelectButton(false, mvcButton);
            SelectButton(false, rpgButton);
            SelectButton(false, otherButton);

            //Disable otherTextBox, only allow entry if Other Button is pressed
            otherTextBox.IsEnabled = false;

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

            bindButtons();

        }

        public void bindButtons()
        {
            //bind all buttons and injuries
            allButtonsList.Add(artilleryButton);
            allInjuryTypes.Add("Artillery");
            allButtonsList.Add(bluntButton);
            allInjuryTypes.Add("Blunt");
            allButtonsList.Add(burnButton);
            allInjuryTypes.Add("Burn");
            allButtonsList.Add(fallButton);
            allInjuryTypes.Add("Fall");
            allButtonsList.Add(grenadeButton);
            allInjuryTypes.Add("Grenade");
            allButtonsList.Add(gswButton);
            allInjuryTypes.Add("GSW");
            allButtonsList.Add(iedButton);
            allInjuryTypes.Add("IED");
            allButtonsList.Add(landmineButton);
            allInjuryTypes.Add("Landmine");
            allButtonsList.Add(mvcButton);
            allInjuryTypes.Add("MVC");
            allButtonsList.Add(rpgButton);
            allInjuryTypes.Add("RPG");
            allButtonsList.Add(otherButton);
        }

        // set button on or off, true = on
        public void SelectButton(bool on, Button btn)
        {
            if (on)
            {
                btn.Foreground = Brushes.Black;     // to change colors, change these in each class plus the IsButtonSelected
                btn.Background = Brushes.Yellow;
            }
            else
            {
                btn.Foreground = Brushes.White;
                btn.Background = Brushes.DimGray;
            }
        }

        public bool IsButtonSelected(Button btn)
        {
            if (btn.Background == Brushes.Yellow)
                return true;
            else
                return false;
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                //set the global patient for deleting purposes
                incomingPatient = Message;
                handleControllerPatientData(Message);
            }

        }

        public void handleControllerPatientData(patient p)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (p.injuries != null && !p.fromDatabase)
                            {
                                foreach (patient.Injuries i in p.injuries)
                                {
                                    //handle blank injury data being sent from adding new injury funtion
                                    if (i.Type == "" || i.Type == null)
                                    {
                                        location = i.Location;
                                        break;
                                    }
                                    foreach (Button b in allButtonsList)
                                    {
                                        string buttonContent = b.Content.ToString();
                                        string injuryContent = i.Type.ToString();
                                        //do the other button if needed
                                        if (buttonContent == "Other")
                                        {
                                            //if the injury is not set to a button
                                            if (!ifGlobalInjury(injuryContent))
                                            {
                                                SelectButton(true, b);
                                                otherTextBox.IsEnabled = true;
                                                otherTextBox.Text = injuryContent;
                                                Types.Add(injuryContent);
                                                otherInjury = injuryContent;
                                                location = i.Location;
                                            }
                                            
                                        }
                                        else if (buttonContent == i.Type.ToString())
                                        {
                                            SelectButton(true, b);
                                            //Also save the location for edit or delete purposes
                                            location = i.Location;
                                            Types.Add(buttonContent);

                                        }
                                    }
                                }
                            }
                        }));
            }
            catch { }
        }

        private bool ifGlobalInjury(string injury)
        {
            foreach (string s in allInjuryTypes)
            {
                if (injury == s)
                {
                    return true;
                }
            }
            return false;
        }
        private void otherButton_Click(object sender, RoutedEventArgs e)
        {
            //If the button isn't clicked, then click the button
            if (!IsButtonSelected(otherButton))
            {
                SelectButton(true, otherButton);
                otherTextBox.IsEnabled = true;
                otherInjury = "Other";
                Types.Add(otherInjury);
            }

            //If the button is clicked, unclick the button
            else
            {
                SelectButton(false, otherButton);
                otherTextBox.IsEnabled = false;
                try
                {
                    Types.Remove(otherInjury);
                }
                catch { }
            }
        }

        private void typeButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (!IsButtonSelected(b))
            {
                SelectButton(true, b);
                Types.Add(b.Content.ToString());
            }

            //If the button is clicked, unclick the button
            else
            {
                SelectButton(false, b);
                try
                {
                    Types.Remove(b.Content.ToString());
                }
                catch { }

            }

        }

        private void otherTextBox_Click(object sender, TextChangedEventArgs e)
        {

        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add other injury if needed
                if (Types.Contains(otherInjury) && otherInjury == "Other")
                {
                    if (otherTextBox.Text != "" || otherTextBox.Text != otherInjury || otherTextBox.Text != "Other")
                    {
                        Types.Remove(otherInjury);
                        otherInjury = otherTextBox.Text.ToString();
                        //check for those damn commas
                        if (otherInjury.Contains(','))
                        {
                            otherInjury = otherInjury.Replace(",", " &");
                        }
                        Types.Add(otherInjury);
                    }
                }
                //Check if other text box has changed
                if (Types.Contains(otherInjury) && otherTextBox.Text != otherInjury)
                {
                    if (otherTextBox.Text == "")
                    {
                        //remove the injury
                        Types.Remove(otherInjury);
                    }
                    else
                    {
                        //Change the other injury to match the content of the text box
                        Types.Remove(otherInjury);
                        otherInjury = otherTextBox.Text.ToString();
                        //check for those damn commas
                        if (otherInjury.Contains(','))
                        {
                            otherInjury = otherInjury.Replace(",", " &");
                        }
                        Types.Add(otherInjury);

                    }
                }
                //package and send patient data
                patient temp = new patient();
                foreach (string iType in Types)
                {
                    patient.Injuries newinjury = new patient.Injuries();
                    newinjury.Location = location;
                    newinjury.Type = iType;
                    temp.injuries.Add(newinjury);
                }

                //do the deletes
                bool needsDelete = true;
                foreach (patient.Injuries old in incomingPatient.injuries)
                {
                    //if this isn't the same location
                    if (old.Location != location)
                    {
                        break;
                    }
                    //break if this was a new injury
                    if (old.Type == null || old.Type == "")
                    {
                        break;
                    }
                    foreach (patient.Injuries current in temp.injuries)
                    {
                        //if the injuries match, move on
                        if (current.Type == old.Type)
                        {
                            needsDelete = false;
                            //since this isn't a new injury remove it from the payload
                            temp.injuries.Remove(current);
                            break;
                        }
                    }
                    if (needsDelete)
                    {
                        //add in old injury that needs deleting
                        old.delete = true;
                        temp.injuries.Add(old);

                    }
                    //set this back to true for another iteration
                    needsDelete = true;
                }

                //Add that a Database Operation is needed
                temp.DBOperation = true;
                _systemMessages.AddMessage(temp);
                _messages.AddMessage("ADD INJURY DONE");
            }
            catch { }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            Types.Clear();

            artilleryButton.Background = Brushes.DimGray;
            bluntButton.Background = Brushes.DimGray;
            burnButton.Background = Brushes.DimGray;
            fallButton.Background = Brushes.DimGray;
            grenadeButton.Background = Brushes.DimGray;
            gswButton.Background = Brushes.DimGray;
            iedButton.Background = Brushes.DimGray;
            landmineButton.Background = Brushes.DimGray;
            mvcButton.Background = Brushes.DimGray;
            rpgButton.Background = Brushes.DimGray;
            otherButton.Background = Brushes.DimGray;
            otherTextBox.IsEnabled = false;
            otherTextBox.Text = "Other";
        }

    }
}

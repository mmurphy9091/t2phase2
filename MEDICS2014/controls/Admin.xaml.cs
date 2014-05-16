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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;
        patient gloablPatient = new patient();

        //bool isInFocus = false;   // use this to stop updates while editing a field, not finished yet

        public Admin()
        {
            InitializeComponent();

            //allergiesOtherTextBox.Text = "OTHER";
            //allergiesOtherTextBox.IsEnabled = false;

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
                handleControllerData(message);
            }
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

        public void handleControllerData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "CLEAR CONTROL":
                        maleRadioButton.IsChecked = false;
                        femaleRadioButton.IsChecked = false;
                        //weightTypeComboBox.SelectedIndex = -1;
                        if (!firstNameTextBox.IsFocused)
                        {
                            firstNameTextBox.Text = "New";
                        }
                        if (!middleNameTextBox.IsFocused)
                        {
                            middleNameTextBox.Text = "";
                        }
                        if (!lastNameTextbox.IsFocused)
                        {
                            lastNameTextbox.Text = "Patient";
                        }
                        if (!last4TextBox.IsFocused)
                        {
                            last4TextBox.Text = "";
                        }
                        if (!unitTextBox.IsFocused)
                        {
                            unitTextBox.Text = "";
                        }
                        if (!serviceTextBox.IsFocused)
                        {
                            serviceTextBox.Text = "";
                        }
                        if (!weightTexgtBox.IsFocused)
                        {
                            weightTexgtBox.Text = "";
                        }
                        if (!dateTextBox.IsFocused)
                        {
                            dateTextBox.Text = "";
                        }
                        if (!timeTextBox.IsFocused)
                        {
                            timeTextBox.Text = "";
                        }
                            
                        break;
                }
            }));
        }

        public void handleControllerPatientData(patient data)
        {
            //bool dataEntered = false;
                try
                {
                    //if the value isn't null, assing the value then set to null, that way he have no redundant data
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (data.firstName != null && !firstNameTextBox.IsFocused)
                            {
                                firstNameTextBox.Text = data.firstName.ToString();
                                //data.firstName = null;
                                //dataEntered = true;
                            }
                            if (data.middleName != null && !middleNameTextBox.IsFocused)
                            {
                                middleNameTextBox.Text = data.middleName.ToString();
                                //data.middleName = null;
                                //dataEntered = true;
                            }
                            if (data.lastName != null && !lastNameTextbox.IsFocused)
                            {
                                lastNameTextbox.Text = data.lastName.ToString();
                                //data.lastName = null;
                                //dataEntered = true;
                            }
                            if (data.SSN != null && !last4TextBox.IsFocused)
                            {
                                last4TextBox.Text = data.SSN.ToString();
                                //data.SSN = null;
                                //dataEntered = true;
                            }
                            if (data.unitName != null && !unitTextBox.IsFocused)
                            {
                                unitTextBox.Text = data.unitName.ToString();
                                //data.unitName = null;
                                //dataEntered = true;
                            }
                            if (data.service != null && !serviceTextBox.IsFocused)
                            {
                                serviceTextBox.Text = data.service.ToString();
                                //data.service = null;
                                //dataEntered = true;
                            }
                            if (data.weight != null && !weightTexgtBox.IsFocused)
                            {
                                weightTexgtBox.Text = data.weight.ToString();
                            }
                            if (data.weightType != null)
                            {
                                if (data.weightType == "LBS")
                                {
                                    weightTypeComboBox.SelectedIndex = 0;
                                }
                                else if (data.weightType == "KG")
                                {
                                    weightTypeComboBox.SelectedIndex = 1;
                                }
                                /*
                                int index = weightTypeComboBox.Items.IndexOf(data.weightType);
                                if (index > -1)
                                {
                                    weightTypeComboBox.SelectedIndex = index;
                                }
                                 */
                            }
                            if (data.recordDate != null && !dateTextBox.IsFocused)
                            {
                                dateTextBox.Text = data.recordDate.ToString();
                                //data.recordDate = null;
                                //dataEntered = true;
                            }
                            if (data.recordTime != null && !timeTextBox.IsFocused)
                            {
                                timeTextBox.Text = data.recordTime.ToString();
                                //data.recordTime = null;
                                //dataEntered = true;
                            }
                            if (data.gender != null)
                            {
                                if (data.gender == "Male")
                                {
                                    maleRadioButton.IsChecked = true;
                                    femaleRadioButton.IsChecked = false;
                                    //data.gender = null;
                                    //dataEntered = true;
                                }
                                else if (data.gender == "Female")
                                {
                                    femaleRadioButton.IsChecked = true;
                                    maleRadioButton.IsChecked = false;
                                    //data.gender = null;
                                    //dataEntered = true;
                                }
                            }
                            //send out remaining data, since admin hogs it for some reason
                            //_systemMessages.AddMessage(data);
                        }));
                }
                catch
                {

                }
    }
            
                
                
           
             
        

        private void currentDateButton_Click(object sender, RoutedEventArgs e)
        {
            //Gets the current date
            string currentDate = DateTime.Now.ToString("M/d/yyyy");

            //Place date in date textbox
            dateTextBox.Text = currentDate;
        }

        private void currentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            //Gets the current time, in HHMM format
            string currentTime = DateTime.Now.ToString("HHmm");

            //Place time in time textbox
            timeTextBox.Text = currentTime;
        }

        /*
        private void AllergiesCombo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem allergiesItem = (ComboBoxItem)allergiesComboBox.SelectedItem;
            //If a selection has been made, and that selection is "other" then enable the text box
            if (allergiesItem != null)
            {
                string allergiesItemText = allergiesItem.Content.ToString();

                if (allergiesItemText == "Other")
                {
                    allergiesOtherTextBox.IsEnabled = true;
                }
                else
                {
                    //If the allergies text box is enabled then disable that fool
                    if (allergiesOtherTextBox.IsEnabled)
                    {
                        allergiesOtherTextBox.IsEnabled = false;
                    }
                }
            }

        }
        */
        private void allergiesButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ADMIN ALLERGIES");
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            patient temp = new patient();
            bool sommethingToSend = false;

            //Add the data to the patient
            //First name
                //do any error handling here
                if (firstNameTextBox.Text != gloablPatient.firstName)
                {
                    temp.firstName = firstNameTextBox.Text;
                    sommethingToSend = true;
                    gloablPatient.firstName = temp.firstName;
                }
            //Middle name
                //do any error handling here
                if (middleNameTextBox.Text != gloablPatient.middleName)
                {
                    temp.middleName = middleNameTextBox.Text;
                    sommethingToSend = true;
                    gloablPatient.middleName = temp.middleName;
                }
            //Last name
                //do any error handling here
                if (lastNameTextbox.Text != gloablPatient.lastName)
                {
                    temp.lastName = lastNameTextbox.Text;
                    sommethingToSend = true;
                    gloablPatient.lastName = temp.lastName;
                }
            //Gender
            if (maleRadioButton.IsChecked == true)
            {
                if (gloablPatient.gender != "Male")
                {
                    temp.gender = "Male";
                    sommethingToSend = true;
                    gloablPatient.gender = temp.gender;
                }
            }
            else if (femaleRadioButton.IsChecked == true)
            {
                if (gloablPatient.gender != "Female")
                {
                    temp.gender = "Female";
                    sommethingToSend = true;
                    gloablPatient.gender = temp.gender;
                }
            }
            else
            {
                //nothing is checked somewhow, default to male
                temp.gender = "Male";
            }
            //Last 4
                //check that the text is a number

                //enter to the temp patient
                if (last4TextBox.Text != gloablPatient.SSN)
                {
                    temp.SSN = last4TextBox.Text;
                    sommethingToSend = true;
                    gloablPatient.SSN = temp.SSN;
                }
            //Service
                if (serviceTextBox.Text != gloablPatient.service)
                {
                    temp.service = serviceTextBox.Text;
                    sommethingToSend = true;
                    gloablPatient.service = temp.service;
                }
            //Unit
                if (unitTextBox.Text != gloablPatient.unitName)
                {
                    temp.unitName = unitTextBox.Text;
                    sommethingToSend = true;
                    gloablPatient.unitName = temp.unitName;
                }
            //Date
            if (dateTextBox.Text != gloablPatient.recordDate)
            {
                //save to our patient
                temp.recordDate = dateTextBox.Text;
                gloablPatient.recordDate = temp.recordDate;
                sommethingToSend = true;
            }
            //Weight
            if (weightTexgtBox.Text != gloablPatient.weight)
            {
                //save to our patient
                temp.weight = weightTexgtBox.Text;
                gloablPatient.weight = temp.weight;
                //Get the weight type
                ComboBoxItem weightTypeItem = (ComboBoxItem)weightTypeComboBox.SelectedItem;
                //If a selection has been made apply it to our patient
                if (weightTypeItem != null)
                {
                    temp.weightType = weightTypeItem.Content.ToString();
                }
                else
                {
                    //If there was no selection default to LBS
                    temp.weightType = "LBS";
                }
                sommethingToSend = true;
            }
            //Time
            if (timeTextBox.Text != gloablPatient.recordTime)
            {
                ///Save to our patient
                temp.recordTime = timeTextBox.Text;
                gloablPatient.recordTime = temp.recordTime;
                sommethingToSend = true;
            }
            //send data if needed
            if (sommethingToSend)
            {
                temp.DBOperation = true;
                _systemMessages.AddMessage(temp);
            }


        }

        private void genderRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;

            if (r.Name.ToString() == "maleradiobutton")
            {
                gloablPatient.gender = "Male";
            }
            else if (r.Name.ToString() == "femaleradiobutton")
            {
                gloablPatient.gender = "Female";
            }

        }

        private void weightTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //This is just a simple method to send the weight type, nothing else really
            patient weightPatient = new patient();
            ComboBoxItem weightTypeItem = (ComboBoxItem)weightTypeComboBox.SelectedItem;
            weightPatient.weightType = weightTypeItem.Content.ToString();
            weightPatient.weight = weightTexgtBox.Text;
            weightPatient.DBOperation = true;
            _systemMessages.AddMessage(weightPatient);
            
        }
    }
}

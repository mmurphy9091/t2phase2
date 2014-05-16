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
    /// Interaction logic for allergiesMedications.xaml
    /// </summary>
    public partial class allergiesMedications : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        //create a list of all the buttons
        List<Button> allButtonsList = new List<Button>();

        public allergiesMedications()
        {
            InitializeComponent();

            //Set the color of the buttons, unselected, sorry their names are weird
            SelectButton(false, AllopurinolButton);
            SelectButton(false, AminoglycosidesButton);
            SelectButton(false, AspirinButton);
            SelectButton(false, BarbituratesButton);
            SelectButton(false, BeeVenomButton);
            SelectButton(false, BenzodiazepinesButton);
            SelectButton(false, CephalosporinsButton);
            SelectButton(false, CodeineButton);
            SelectButton(false, DemerolButton);
            SelectButton(false, FlagylButton);
            SelectButton(false, InsulinsButton);

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

            bindButtonList();
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

        }

        // set button on or off, true = on
        public void SelectButton(bool on, Button btn )
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
                        foreach (Button b in allButtonsList)
                        {
                            SelectButton(false, b);
                        }
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
                handleControllerPatientData(Message);
            }

        }

        private void handleControllerPatientData(patient p)
        {
            if (!p.DBOperation)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    try
                    {
                        if (p.Allergies != null)
                        {
                            foreach (patient.Allergy allergy in p.Allergies)
                            {
                                foreach (Button b in allButtonsList)
                                {
                                    if (b.Content.ToString() == allergy.type)
                                    {
                                        SelectButton(true, b);

                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }));
            }
            else if (p.DBOperation)
            {
                if (p.vdesAllergy != null)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        foreach (Button allergyButton in allButtonsList)
                        {
                            if (p.vdesAllergy == allergyButton.Content.ToString())
                            {
                                //If the button isn't clicked, then click the button
                                if(!IsButtonSelected(allergyButton))
                                {
                                    SelectButton(true, allergyButton);
                                    //Add text to allergies list
                                    patient.Allergy newAllergy = new patient.Allergy();
                                    newAllergy.type = allergyButton.Content.ToString();
                                    //globalPatient.Allergies.Add(newAllergy);
                                    //Do the database operation
                                    patient temp = new patient();
                                    temp.DBOperation = true;
                                    temp.Allergies.Add(newAllergy);
                                    _systemMessages.AddMessage(temp);
                                }

                                //If the button is clicked, unclick the button
                                else
                                {
                                    SelectButton(false, allergyButton);
                                    //Remove text from allergies list
                                    patient.Allergy deleteAllergy = new patient.Allergy();
                                    deleteAllergy.type = allergyButton.Content.ToString();
                                    //var itemToRemove = globalPatient.Allergies.Single(allergy => allergy.type == deleteAllergy.type);
                                    //globalPatient.Allergies.Remove(itemToRemove);
                                    deleteAllergy.delete = true;
                                    //globalPatient.Allergies.Add(deleteAllergy);

                                    //Do the database operation
                                    patient temp = new patient();
                                    temp.DBOperation = true;
                                    temp.Allergies.Add(deleteAllergy);
                                    _systemMessages.AddMessage(temp);
                                }
                            }
                        }
                    }));
                }
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES MEDICATIONS 2");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("HOME");
        }

        private void allergyButton_Click(object sender, RoutedEventArgs e)
        {
            Button allergyButton = (Button)sender;

            //If the button isn't clicked, then click the button
            if ( !IsButtonSelected(allergyButton))
            {
                SelectButton(true, allergyButton);
                //Add text to allergies list
                patient.Allergy newAllergy = new patient.Allergy();
                newAllergy.type = allergyButton.Content.ToString();
                //globalPatient.Allergies.Add(newAllergy);
                //Do the database operation
                patient temp = new patient();
                temp.DBOperation = true;
                temp.Allergies.Add(newAllergy);
                _systemMessages.AddMessage(temp);
            }

            //If the button is clicked, unclick the button
            else
            {
                SelectButton(false, allergyButton);
                //Remove text from allergies list
                patient.Allergy deleteAllergy = new patient.Allergy();
                deleteAllergy.type = allergyButton.Content.ToString();
                //var itemToRemove = globalPatient.Allergies.Single(allergy => allergy.type == deleteAllergy.type);
                //globalPatient.Allergies.Remove(itemToRemove);
                deleteAllergy.delete = true;
                //globalPatient.Allergies.Add(deleteAllergy);

                //Do the database operation
                patient temp = new patient();
                temp.DBOperation = true;
                temp.Allergies.Add(deleteAllergy);
                _systemMessages.AddMessage(temp);
            }

            
        }

        public void bindButtonList()
        {
            allButtonsList.Add(AllopurinolButton);
            allButtonsList.Add(AminoglycosidesButton);
            allButtonsList.Add(AspirinButton);
            allButtonsList.Add(BarbituratesButton);
            allButtonsList.Add(BeeVenomButton);
            allButtonsList.Add(BenzodiazepinesButton);
            allButtonsList.Add(CephalosporinsButton);
            allButtonsList.Add(CodeineButton);
            allButtonsList.Add(DemerolButton);
            allButtonsList.Add(FlagylButton);
            allButtonsList.Add(InsulinsButton);

        }


    }
}

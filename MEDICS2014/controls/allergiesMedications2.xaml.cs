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
    /// Interaction logic for allergiesMedications2.xaml
    /// </summary>
    public partial class allergiesMedications2 : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        //bool to help with data generation
        public bool navigatedToYet = false;

        //create a list of all the buttons
        List<Button> allButtonsList = new List<Button>();

        patient globalPatient = new patient();

        public allergiesMedications2()
        {
            InitializeComponent();

            SelectButton(false, IodineButton);
            SelectButton(false, LatexButton);
            SelectButton(false, MacrolidesButton);
            SelectButton(false, MorphineButton);
            SelectButton(false, MotrinButton);
            SelectButton(false, NaprosynButton);
            SelectButton(false, PCNButton);
            SelectButton(false, SalicylatesButton);
            SelectButton(false, SulfaButton);
            SelectButton(false, TetracyclinesButton);
            SelectButton(false, TylenolButton);

            
            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

            bindButtonList();
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

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string Message = messageEvent.Message;
                //handleMessageData(Message);
            }
        }

        private void handleMessageData(string message)
        {
            if (message == "ALLERGIES MEDICATIONS 2")
            {
                navigatedToYet = true;
            }
            else
            {
                if (navigatedToYet)
                {
                    performAllergiesDatabaseOperation();
                    navigatedToYet = false;
                }
            }
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
                handleAppPatientData(Message);
            }

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES MEDICATIONS 3");
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES MEDICATIONS");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ALLERGIES MAIN");
        }

        private void allergyButton_Click(object sender, RoutedEventArgs e)
        {
            Button allergyButton = (Button)sender;

            //If the button isn't clicked, then click the button
            if (!IsButtonSelected(allergyButton))
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
                allergyButton.Background = Brushes.DimGray;
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

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public void handleAppPatientData(patient p)
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
                                        patient.Allergy newAllergy = new patient.Allergy();
                                        newAllergy.type = b.Content.ToString();
                                        globalPatient.Allergies.Add(newAllergy);

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
                                if (!IsButtonSelected(allergyButton))
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

        public void bindButtonList()
        {
            allButtonsList.Add(LatexButton);
            allButtonsList.Add(MacrolidesButton);
            allButtonsList.Add(MorphineButton);
            allButtonsList.Add(MotrinButton);
            allButtonsList.Add(NaprosynButton);
            allButtonsList.Add(PCNButton);
            allButtonsList.Add(SalicylatesButton);
            allButtonsList.Add(TetracyclinesButton);
            allButtonsList.Add(TylenolButton);
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            //send the allergy data off
            //_systemMessages.AddMessage(globalPatient);

            //get rid of all the alllergies that were deleted
            patient tempPatient = new patient();
            foreach (patient.Allergy i in globalPatient.Allergies)
            {
                patient.Allergy add = new patient.Allergy();
                add.type = i.type;
                add.delete = i.delete;
                tempPatient.Allergies.Add(add);
            }
            foreach (patient.Allergy a in tempPatient.Allergies)
            {
                if (a.delete)
                {
                    var itemToRemove = globalPatient.Allergies.Single(allergy => allergy.type == a.type);
                    globalPatient.Allergies.Remove(itemToRemove);
                }
            }
        }

        private void performAllergiesDatabaseOperation()
        {
            MessageBox.Show("Now");
        }
    }
}

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

namespace MEDICS2014.controls.medsControls
{
    /// <summary>
    /// Interaction logic for medsOthers1.xaml
    /// </summary>
    public partial class medsOthers1 : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        List<Button> otherButtons = new List<Button>();
        List<string> otherMeds = new List<string>();

        public medsOthers1()
        {
            InitializeComponent();
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

            bindButtonsAndData();
        }

        private void bindButtonsAndData()
        {
            otherButtons.Add(otherButton1);
            otherButtons.Add(otherButton2);
            otherButtons.Add(otherButton3);
            otherButtons.Add(otherButton4);
            otherButtons.Add(otherButton5);
            otherButtons.Add(otherButton6);
            otherButtons.Add(otherButton7);
            otherButtons.Add(otherButton8);
            otherButtons.Add(otherButton9);
            otherButtons.Add(otherButton10);

            foreach (Button b in otherButtons)
            {
                b.Visibility = Visibility.Hidden;
            }

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
                if (p.DBOperation)
                {
                    if (p.tempMed != null)
                    {
                        if (p.tempMed.Other == "True")
                        {
                            //if the name if blank, get rid of the button and shift the others up
                            if (p.tempMed.Name == "")
                            {
                                /* Put in some code to delet button later
                                bool moveUp = false;
                                Button lastButton = new Button();
                                foreach (Button b in otherButtons)
                                {
                                    if (moveUp)
                                    {
                                        if (b.Visibility == Visibility.Hidden)
                                        {
                                            lastButton.Content = "";
                                            lastButton.Visibility = Visibility.Hidden;
                                            break;
                                        }
                                        else
                                        {
                                            lastButton.Content = b.Content;
                                            lastButton = b;
                                        }
                                    }
                                    if (b.Content == p.tempMed.Name)
                                    {
                                        lastButton = b;
                                        moveUp = true;
                                    }
                                }
                                 */
                            }
                            else
                            {
                                //cycle through the global medications list, create buttons for each medication that's other
                                //first, hide all the buttons
                                foreach (Button b in otherButtons)
                                {
                                    b.Visibility = Visibility.Hidden;
                                }
                                foreach (patient.MedicationsDetails med in p.Medications)
                                {
                                    if (med.Other == "True")
                                    {
                                        foreach (Button b in otherButtons)
                                        {
                                            if (b.Visibility == Visibility.Hidden)
                                            {
                                                b.Visibility = Visibility.Visible;
                                                b.Content = med.Name;
                                                otherMeds.Add(med.Name);
                                                break;
                                            }
                                        }
                                    }
                                }
                                /*
                                foreach (Button b in otherButtons)
                                {
                                    //check if the button already has a med attached
                                    if (!otherMeds.Contains(p.tempMed.Name))
                                    {
                                        //find the first button to be hidden
                                        if (b.Visibility == Visibility.Hidden)
                                        {
                                            b.Visibility = Visibility.Visible;
                                            b.Content = p.tempMed.Name;
                                            otherMeds.Add(p.tempMed.Name);
                                            break;
                                        }
                                    }
                                }
                                */
                            }
                        }
                    }
                }
                else if (p.fromDatabase)
                {
                    foreach (Button b in otherButtons)
                    {
                        b.Visibility = Visibility.Hidden;
                    }
                    foreach (patient.MedicationsDetails med in p.Medications)
                    {
                        if (med.Other == "True")
                        {
                            foreach (Button b in otherButtons)
                            {
                                if (b.Visibility == Visibility.Hidden)
                                {
                                    b.Visibility = Visibility.Visible;
                                    b.Content = med.Name;
                                    otherMeds.Add(med.Name);
                                    break;
                                }
                            }
                        }
                    }
                    /*
                    foreach (patient.MedicationsDetails med in p.Medications)
                    {
                        if (med.Other == "True")
                        {
                            if (!otherMeds.Contains(med.Name))
                            {
                                foreach (Button b in otherButtons)
                                {
                                    //check if the button already has a med attached
                                    if (b.Content.ToString() != med.Name)
                                    {
                                        //find the first button to be hidden
                                        if (b.Visibility == Visibility.Hidden)
                                        {
                                            b.Visibility = Visibility.Visible;
                                            b.Content = med.Name;
                                            otherMeds.Add(med.Name);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                     */
                }
            }));

        }


        private void medsButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            patient temp = new patient();

            temp.tempMed.Name = b.Content.ToString();
            temp.tempMed.Other = "True";

            _systemMessages.AddMessage(temp);
            _messages.AddMessage("MEDS DETAILS");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            patient temp = new patient();

            temp.tempMed.Name = "OTHER";
            temp.tempMed.Other = "True";

            _systemMessages.AddMessage(temp);
            _messages.AddMessage("MEDS DETAILS");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS");
        }
    }
}

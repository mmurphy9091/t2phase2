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
using System.Collections.ObjectModel;

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for vitalsAdd.xaml
    /// </summary>
    public partial class vitalsAdd : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;


        //ComboBoxViewModel painScaleViewModel = new ComboBoxViewModel();

        public vitalsAdd()
        {
            InitializeComponent();

            bindAllObjects();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("VITALS SUMMARY");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            patient patientVitalsMessage = new patient();

            //bool to see if there is anything worth sending
            bool isEmpty = true;

            //bool to check that blood pressure is properly filled out
            bool hasSYS = false;

            //Put all the data into the patient class
            //time
            if (timeTextBox.Text != "")
            {
                patientVitalsMessage.vitalsTime = timeTextBox.Text.ToString();
                //isEmpty = false;
            }
            else
            {
                //if there is no time go ahead and make it
                patientVitalsMessage.vitalsTime = DateTime.Now.ToString("HHmm");
            }
            //heart rate
            if (hrTextBox.Text != "")
            {
                patientVitalsMessage.HR = hrTextBox.Text.ToString();
                isEmpty = false;
            }
            //blood pressure SYS
            if (bpSYSTextBox.Text != "")
            {
                patientVitalsMessage.BPSYS = bpSYSTextBox.Text.ToString();
                isEmpty = false;
                hasSYS = true;
            }
            //blood pressure DIA
            if (bpDIATextBox.Text != "")
            {
                if (!hasSYS)
                {
                    MessageBox.Show("ENTER A VALUE FOR THE SYSTOLIC BLOOD PRESSURE");
                    return;
                }
                else
                {
                    patientVitalsMessage.BPDIA = bpDIATextBox.Text.ToString();
                    isEmpty = false;
                }
            }
            else if (hasSYS)
            {
                MessageBox.Show("ENTER A VALUE FOR THE DIASTOLIC BLOOD PRESSURE");
                return;
            }
            //Respiratory Rate
            if (respTextBox.Text != "")
            {
                patientVitalsMessage.Resp = respTextBox.Text.ToString();
                isEmpty = false;
            }
            //ETC02
            if (sp02TextBox.Text != "")
            {
                patientVitalsMessage.SP02 = sp02TextBox.Text.ToString();
                isEmpty = false;
            }
            //Temperature
            if (tempTextBox.Text != "")
            {
                patientVitalsMessage.Temp = tempTextBox.Text.ToString();
                isEmpty = false;
            }
            //Temperature Type
            ComboBoxItem tempTypeItem = (ComboBoxItem)tempTypeComboBox.SelectedItem;
            //If a selection has been made apply it to our patient
            if (tempTypeItem != null)
            {
                patientVitalsMessage.TempType = tempTypeItem.Content.ToString();
            }
            else
            {
                //If there was no selection default to F
                patientVitalsMessage.TempType = "F";
            }
            //AVPU
            ComboBoxItem avpuItem = (ComboBoxItem)avpuComboBox.SelectedItem;
            if (avpuItem != null)
            {
                patientVitalsMessage.AVPU = avpuItem.Content.ToString();
                isEmpty = false;
            }
            else
            {
                patientVitalsMessage.AVPU = "";
            }
            //Pain Scale
            ComboBoxItem psItem = (ComboBoxItem)painScaleComboBox.SelectedItem;
            if (psItem != null)
            {
                patientVitalsMessage.painScale = psItem.Content.ToString();
                isEmpty = false;
            }
            else
            {
                patientVitalsMessage.painScale = "";
            }

            //Send the data if there is something worth sending
            if (!isEmpty)
            {
                //Add the ability for the database to create what it needs
                patientVitalsMessage.DBOperation = true;

                //Send the data
                _systemMessages.AddMessage(patientVitalsMessage);

                //Go back to the summary window
                _messages.AddMessage("VITALS SUMMARY");

                //And clear all the textboxs
                clearAllBoxes();

            }
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            clearAllBoxes();

        }

        private void clearAllBoxes()
        {
            timeTextBox.Text = "";
            hrTextBox.Text = "";
            bpSYSTextBox.Text = "";
            bpDIATextBox.Text = "";
            respTextBox.Text = "";
            sp02TextBox.Text = "";
            tempTextBox.Text = "";
        }

        private void bindAllObjects()
        {
            //Bind Pain Scale Combo Box **FIGURE THIS OUT LATER**
        }

        private void addTimeButton_Click(object sender, RoutedEventArgs e)
        {
            timeTextBox.Text = DateTime.Now.ToString("HHmm");
        }
        
    }

}

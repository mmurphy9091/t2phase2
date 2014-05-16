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
    /// Interaction logic for treatmentsCirculation.xaml
    /// </summary>
    public partial class treatmentsCirculation : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public treatmentsCirculation()
        {
            InitializeComponent();

            bindTQData();
            bindDressingData();
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient message = messageEvent.systemMessage;
                //System.Windows.MessageBox.Show(message);
                handleAppData(message);
            }
        }

        private void handleAppData(patient p)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {

                if (p.fromVDES)
                {
                    bool saveNeeded = false;
                    if (p.treatments.circulation.dressing.hemostatic)
                    {
                        globalPatient.treatments.circulation.dressing.hemostatic = !globalPatient.treatments.circulation.dressing.hemostatic;
                        saveNeeded = true;
                    }
                    if (p.treatments.circulation.dressing.pressure)
                    {
                        globalPatient.treatments.circulation.dressing.pressure = !globalPatient.treatments.circulation.dressing.pressure;
                        saveNeeded = true;
                    }
                    if (p.treatments.circulation.dressing.other)
                    {
                        if (globalPatient.treatments.circulation.dressing.other)
                        {
                            otherTextBox.IsEnabled = false;
                        }
                        else
                        {
                            otherTextBox.IsEnabled = true;
                            otherTextBox.Text = p.treatments.circulation.dressing.otherType;
                            globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;
                        }
                        globalPatient.treatments.circulation.dressing.other = !globalPatient.treatments.circulation.dressing.other;
                        saveNeeded = true;
                    }

                    if (p.treatments.circulation.TQ.extremity)
                    {
                        globalPatient.treatments.circulation.TQ.extremity = !globalPatient.treatments.circulation.TQ.extremity;
                        saveNeeded = true;
                    }
                    if (p.treatments.circulation.TQ.junctional)
                    {
                        globalPatient.treatments.circulation.TQ.junctional = !globalPatient.treatments.circulation.TQ.junctional;
                        saveNeeded = true;
                    }
                    if (p.treatments.circulation.TQ.truncal)
                    {
                        globalPatient.treatments.circulation.TQ.truncal = !globalPatient.treatments.circulation.TQ.truncal;
                        saveNeeded = true;
                    }

                    if (saveNeeded)
                    {
                        if (globalPatient.treatments.circulation.dressing.hemostatic)
                        {
                            hemostaticButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            hemostaticButton.Background = Brushes.DimGray;
                        }
                        //pressure
                        if (globalPatient.treatments.circulation.dressing.pressure)
                        {
                            pressureButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            pressureButton.Background = Brushes.DimGray;
                        }
                        //other
                        if (globalPatient.treatments.circulation.dressing.other)
                        {
                            otherButton.Background = Brushes.Yellow;
                            otherTextBox.IsEnabled = true;
                            otherTextBox.Text = globalPatient.treatments.circulation.dressing.otherType;
                        }
                        else
                        {
                            otherButton.Background = Brushes.DimGray;
                            otherTextBox.IsEnabled = false;
                        }

                        //TQ
                        //extremity
                        if (globalPatient.treatments.circulation.TQ.extremity)
                        {
                            extremityButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            extremityButton.Background = Brushes.DimGray;
                        }
                        //junctional
                        if (globalPatient.treatments.circulation.TQ.junctional)
                        {
                            junctionalButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            junctionalButton.Background = Brushes.DimGray;
                        }
                        //truncal
                        if (globalPatient.treatments.circulation.TQ.truncal)
                        {
                            truncalButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            truncalButton.Background = Brushes.DimGray;
                        }

                        globalPatient.DBOperation = true;
                        globalPatient.treatments.circulation.dbSave = true;
                        _systemMessages.AddMessage(globalPatient);
                    }
                }

            if (p.DBOperation)
            {
                //cycle through and make sure that this global patient is up to date on the other apps
                if (p.treatments.airway.dbSave)
                {
                    globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                    globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;
                    globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                    globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                    globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                }
                //breathing
                if (p.treatments.breathing.dbSave)
                {
                    globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                    globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                    globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                    globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;
                }

                //circulation
                    if (!isInFocus)
                    {
                if (p.treatments.circulation.dbSave)
                {
                    globalPatient.treatments.circulation.dressing.hemostatic = p.treatments.circulation.dressing.hemostatic;
                    globalPatient.treatments.circulation.dressing.pressure = p.treatments.circulation.dressing.pressure;
                    globalPatient.treatments.circulation.dressing.other = p.treatments.circulation.dressing.other;
                    globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;

                    globalPatient.treatments.circulation.TQ.truncal = p.treatments.circulation.TQ.truncal;
                    globalPatient.treatments.circulation.TQ.junctional = p.treatments.circulation.TQ.junctional;
                    globalPatient.treatments.circulation.TQ.extremity = p.treatments.circulation.TQ.extremity;
                }
            }
                }
            else if (p.fromDatabase)
            {
                //cycle through and make sure that this global patient is up to date on the other apps
                globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;
                globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                //breathing
                globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;

                //circulation
                    if (!isInFocus)
                    {
                globalPatient.treatments.circulation.dressing.hemostatic = p.treatments.circulation.dressing.hemostatic;
                globalPatient.treatments.circulation.dressing.pressure = p.treatments.circulation.dressing.pressure;
                globalPatient.treatments.circulation.dressing.other = p.treatments.circulation.dressing.other;
                globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;

                globalPatient.treatments.circulation.TQ.truncal = p.treatments.circulation.TQ.truncal;
                globalPatient.treatments.circulation.TQ.junctional = p.treatments.circulation.TQ.junctional;
                globalPatient.treatments.circulation.TQ.extremity = p.treatments.circulation.TQ.extremity;
                    }
                    //Click any button that needs to be clicked
                    //dressing
                    //hemostatic
                    if (!isInFocus)
                    {
                    if (p.treatments.circulation.dressing.hemostatic)
                    {
                        hemostaticButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        hemostaticButton.Background = Brushes.DimGray;
                    }
                    //pressure
                    if (p.treatments.circulation.dressing.pressure)
                    {
                        pressureButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        pressureButton.Background = Brushes.DimGray;
                    }
                    //other
                    if (p.treatments.circulation.dressing.other)
                    {
                        otherButton.Background = Brushes.Yellow;
                        otherTextBox.IsEnabled = true;
                        otherTextBox.Text = p.treatments.circulation.dressing.otherType;
                    }
                    else
                    {
                        otherButton.Background = Brushes.DimGray;
                        otherTextBox.IsEnabled = false;
                    }

                    //TQ
                    //extremity
                    if (p.treatments.circulation.TQ.extremity)
                    {
                        extremityButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        extremityButton.Background = Brushes.DimGray;
                    }
                    //junctional
                    if (p.treatments.circulation.TQ.junctional)
                    {
                        junctionalButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        junctionalButton.Background = Brushes.DimGray;
                    }
                    //truncal
                    if (p.treatments.circulation.TQ.truncal)
                    {
                        truncalButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        truncalButton.Background = Brushes.DimGray;
                    }
                    }
                    

                }
                }));
            }

        public void bindTQData()
        {
            extremityButton.Background = Brushes.DimGray;
            junctionalButton.Background = Brushes.DimGray;
            truncalButton.Background = Brushes.DimGray;

            //set the bools to false
            globalPatient.treatments.circulation.TQ.extremity = false;
            globalPatient.treatments.circulation.TQ.junctional = false;
            globalPatient.treatments.circulation.TQ.truncal = false;
        }

        public void bindDressingData()
        {
            hemostaticButton.Background = Brushes.DimGray;
            pressureButton.Background = Brushes.DimGray;
            otherButton.Background = Brushes.DimGray;
            otherTextBox.IsEnabled = false;

            //set the bools to false
            globalPatient.treatments.circulation.dressing.hemostatic = false;
            globalPatient.treatments.circulation.dressing.pressure = false;
            globalPatient.treatments.circulation.dressing.other = false;
            globalPatient.treatments.circulation.dressing.otherType = "";
        }

        private void tqButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            if (b.Background == Brushes.DimGray)
            {
                b.Background = Brushes.Yellow;

                //Add it to the patient
                if (b.Name.Contains("extremity"))
                {
                    globalPatient.treatments.circulation.TQ.extremity = true;
                }
                else if (b.Name.Contains("junctional"))
                {
                    globalPatient.treatments.circulation.TQ.junctional = true;
                }
                else if (b.Name.Contains("truncal"))
                {
                    globalPatient.treatments.circulation.TQ.truncal = true;
                }


            }

            //If the button is clicked, unclick the button
            else if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.DimGray;

                //Remove it from the patient
                if (b.Name.Contains("extremity"))
                {
                    globalPatient.treatments.circulation.TQ.extremity = false;
                }
                else if (b.Name.Contains("junctional"))
                {
                    globalPatient.treatments.circulation.TQ.junctional = false;
                }
                else if (b.Name.Contains("truncal"))
                {
                    globalPatient.treatments.circulation.TQ.truncal = false;
                }

            }
        }


        private void dressingButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            if (b.Background == Brushes.DimGray)
            {
                b.Background = Brushes.Yellow;

                if (b.Name.Contains("hemostatic"))
                {
                    globalPatient.treatments.circulation.dressing.hemostatic = true;
                }
                else if (b.Name.Contains("pressure"))
                {
                    globalPatient.treatments.circulation.dressing.pressure = true;
                }

                else if (b.Name == "otherButton")
                {
                    otherTextBox.IsEnabled = true;

                    globalPatient.treatments.circulation.dressing.other = true;
                }
            }

            //If the button is clicked, unclick the button
            else if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.DimGray;

                if (b.Name.Contains("hemostatic"))
                {
                    globalPatient.treatments.circulation.dressing.hemostatic = false;
                }
                else if (b.Name.Contains("pressure"))
                {
                    globalPatient.treatments.circulation.dressing.pressure = false;
                }

                else if (b.Name == "otherButton")
                {
                    otherTextBox.IsEnabled = false;

                    globalPatient.treatments.circulation.dressing.other = false;
                }

                try
                {

                }
                catch { }

            }
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
            isInFocus = false;

            //check for other
            if (globalPatient.treatments.circulation.dressing.other)
            {
                globalPatient.treatments.circulation.dressing.otherType = otherTextBox.Text.ToString();
            }

                globalPatient.DBOperation = true;
                globalPatient.treatments.circulation.dbSave = true;
                _systemMessages.AddMessage(globalPatient);
        }

        private bool checkForData()
        {
            if (globalPatient.treatments.circulation.TQ.extremity)
            {
                return true;
            }
            else if (globalPatient.treatments.circulation.TQ.junctional)
            {
                return true;
            }
            else if (globalPatient.treatments.circulation.TQ.truncal)
            {
                return true;
            }
            else if (globalPatient.treatments.circulation.dressing.hemostatic)
            {
                return true;
            }
            else if (globalPatient.treatments.circulation.dressing.other)
            {
                return true;
            }
            else if (globalPatient.treatments.circulation.dressing.pressure)
            {
                return true;
            }

            return false;

        }

    }
}

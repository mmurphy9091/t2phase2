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
    /// Interaction logic for treatmentsAirway.xaml
    /// </summary>
    public partial class treatmentsAirway : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        bool isInFocus = false;

        public treatmentsAirway()
        {
            InitializeComponent();

            bindButtonData();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

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
                        //might not need anything here
                        break;
                }
            }));
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
                    if (p.treatments.airway.intact)
                    {
                        globalPatient.treatments.airway.intact = !globalPatient.treatments.airway.intact;
                        saveNeeded = true;
                    }
                    if (p.treatments.airway.CRIC)
                    {
                        globalPatient.treatments.airway.CRIC = !globalPatient.treatments.airway.CRIC;
                        saveNeeded = true;
                    }
                    if (p.treatments.airway.SGA)
                    {
                        globalPatient.treatments.airway.SGA = !globalPatient.treatments.airway.SGA;
                        saveNeeded = true;
                    }
                    if (p.treatments.airway.NPA)
                    {
                        globalPatient.treatments.airway.NPA = !globalPatient.treatments.airway.NPA;
                        saveNeeded = true;
                    }
                    if (p.treatments.airway.etTube)
                    {
                        globalPatient.treatments.airway.etTube = !globalPatient.treatments.airway.etTube;
                        saveNeeded = true;
                    }
                    if (saveNeeded)
                    {
                        if (globalPatient.treatments.airway.CRIC)
                        {
                            cricButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            cricButton.Background = Brushes.DimGray;
                        }
                        //etTube
                        if (globalPatient.treatments.airway.etTube)
                        {
                            ettubeButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            ettubeButton.Background = Brushes.DimGray;
                        }
                        //intact
                        if (globalPatient.treatments.airway.intact)
                        {
                            intactButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            intactButton.Background = Brushes.DimGray;
                        }
                        //NPA
                        if (globalPatient.treatments.airway.NPA)
                        {
                            npaButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            npaButton.Background = Brushes.DimGray;
                        }
                        //SGA
                        if (globalPatient.treatments.airway.SGA)
                        {
                            sgaButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            sgaButton.Background = Brushes.DimGray;
                        }

                        globalPatient.DBOperation = true;
                        globalPatient.treatments.airway.dbSave = true;
                        _systemMessages.AddMessage(globalPatient);
                    }
                }
            if (p.DBOperation)
            {
                //cycle through and make sure that this global patient is up to date on the other apps
                    if (!isInFocus)
                    {
                if (p.treatments.airway.dbSave)
                {
                    globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                    globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;
                    globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                    globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                    globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                }
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
            else if (p.fromDatabase)
            {
                //cycle through and make sure that this global patient is up to date on the other apps
                    if (!isInFocus)
                    {
                    globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                    globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;
                    globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                    globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                    globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                    }
                //breathing
                    globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                    globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                    globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                    globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;

                //circulation
                    globalPatient.treatments.circulation.dressing.hemostatic = p.treatments.circulation.dressing.hemostatic;
                    globalPatient.treatments.circulation.dressing.pressure = p.treatments.circulation.dressing.pressure;
                    globalPatient.treatments.circulation.dressing.other = p.treatments.circulation.dressing.other;
                    globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;

                    globalPatient.treatments.circulation.TQ.truncal = p.treatments.circulation.TQ.truncal;
                    globalPatient.treatments.circulation.TQ.junctional = p.treatments.circulation.TQ.junctional;
                    globalPatient.treatments.circulation.TQ.extremity = p.treatments.circulation.TQ.extremity;

                        //Click any button that needs to be clicked
                        //CRIC
                    if (!isInFocus)
                    {
                        if (p.treatments.airway.CRIC)
                        {
                            cricButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            cricButton.Background = Brushes.DimGray;
                        }
                        //etTube
                        if (p.treatments.airway.etTube)
                        {
                            ettubeButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            ettubeButton.Background = Brushes.DimGray;
                        }
                        //intact
                        if (p.treatments.airway.intact)
                        {
                            intactButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            intactButton.Background = Brushes.DimGray;
                        }
                        //NPA
                        if (p.treatments.airway.NPA)
                        {
                            npaButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            npaButton.Background = Brushes.DimGray;
                        }
                        //SGA
                        if (p.treatments.airway.SGA)
                        {
                            sgaButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            sgaButton.Background = Brushes.DimGray;
                        }
                    }

            }

            }));
        }

        private void bindButtonData()
        {
            intactButton.Background = Brushes.DimGray;
            npaButton.Background = Brushes.DimGray;
            cricButton.Background = Brushes.DimGray;
            ettubeButton.Background = Brushes.DimGray;
            sgaButton.Background = Brushes.DimGray;

            //set the bool to false at startup
            globalPatient.treatments.airway.intact = false;
            globalPatient.treatments.airway.NPA = false;
            globalPatient.treatments.airway.CRIC = false;
            globalPatient.treatments.airway.etTube = false;
            globalPatient.treatments.airway.SGA = false;
        }

        private void airwayButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            isInFocus = true;

            if (b.Background == Brushes.DimGray)
            {
                b.Background = Brushes.Yellow;

                //add it to the patient
                if (b.Name.Contains("intact"))
                {
                    globalPatient.treatments.airway.intact = true;
                }
                else if (b.Name.Contains("npa"))
                {
                    globalPatient.treatments.airway.NPA = true;
                }
                else if (b.Name.Contains("cric"))
                {
                    globalPatient.treatments.airway.CRIC = true;
                }
                else if (b.Name.Contains("ettube"))
                {
                    globalPatient.treatments.airway.etTube = true;
                }
                else if (b.Name.Contains("sga"))
                {
                    globalPatient.treatments.airway.SGA = true;
                }

            }

            //If the button is clicked, unclick the button
            else if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.DimGray;

                //remove it frrom the patient
                if (b.Name.Contains("intact"))
                {
                    globalPatient.treatments.airway.intact = false;
                }
                else if (b.Name.Contains("npa"))
                {
                    globalPatient.treatments.airway.NPA = false;
                }
                else if (b.Name.Contains("cric"))
                {
                    globalPatient.treatments.airway.CRIC = false;
                }
                else if (b.Name.Contains("ettube"))
                {
                    globalPatient.treatments.airway.etTube = false;
                }
                else if (b.Name.Contains("sga"))
                {
                    globalPatient.treatments.airway.SGA = false;
                }

            }
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
            isInFocus = false;
                globalPatient.DBOperation = true;
                globalPatient.treatments.airway.dbSave = true;
                _systemMessages.AddMessage(globalPatient);
        }

        private bool checkForData()
        {
            if (globalPatient.treatments.airway.CRIC)
            {
                return true;
            }
            else if (globalPatient.treatments.airway.etTube)
            {
                return true;
            }
            else if (globalPatient.treatments.airway.intact)
            {
                return true;
            }
            else if (globalPatient.treatments.airway.NPA)
            {
                return true;
            }
            else if (globalPatient.treatments.airway.SGA)
            {
                return true;
            }

            return false;

        }
    }
}

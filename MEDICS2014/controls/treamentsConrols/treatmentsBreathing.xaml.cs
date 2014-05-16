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
    /// Interaction logic for treatmentsBreathing.xaml
    /// </summary>
    public partial class treatmentsBreathing : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();
 
        bool isInFocus = false;
 
        public treatmentsBreathing()
        {
            InitializeComponent();

            bindButtonData();

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
                    if (p.treatments.breathing.chestSeal)
                    {
                        globalPatient.treatments.breathing.chestSeal = !globalPatient.treatments.breathing.chestSeal;
                        saveNeeded = true;
                    }
                    if (p.treatments.breathing.chestTube)
                    {
                        globalPatient.treatments.breathing.chestTube = !globalPatient.treatments.breathing.chestTube;
                        saveNeeded = true;
                    }
                    if (p.treatments.breathing.needleD)
                    {
                        globalPatient.treatments.breathing.needleD = !globalPatient.treatments.breathing.needleD;
                        saveNeeded = true;
                    }
                    if (p.treatments.breathing.o2)
                    {
                        globalPatient.treatments.breathing.o2 = !globalPatient.treatments.breathing.o2;
                        saveNeeded = true;
                    }

                    if (saveNeeded)
                    {
                        if (globalPatient.treatments.breathing.chestSeal)
                        {
                            chestSealButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            chestSealButton.Background = Brushes.DimGray;
                        }
                        //chestTube
                        if (globalPatient.treatments.breathing.chestTube)
                        {
                            chestTubeButoon.Background = Brushes.Yellow;
                        }
                        else
                        {
                            chestTubeButoon.Background = Brushes.DimGray;
                        }
                        //needleD
                        if (globalPatient.treatments.breathing.needleD)
                        {
                            needleDButton.Background = Brushes.Yellow;
                        }
                        else
                        {
                            needleDButton.Background = Brushes.DimGray;
                        }
                        //o2
                        if (globalPatient.treatments.breathing.o2)
                        {
                            o2Button.Background = Brushes.Yellow;
                        }
                        else
                        {
                            o2Button.Background = Brushes.DimGray;
                        }

                        globalPatient.DBOperation = true;
                        globalPatient.treatments.breathing.dbSave = true;
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
                    if (!isInFocus)
                    {
                    if (p.treatments.breathing.dbSave)
                    {
                        globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                        globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                        globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                        globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;
                    }
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
                    globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                    globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;
                    globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                    globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                    globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                    if (!isInFocus)
                    {
                    //breathing
                    globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                    globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                    globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                    globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;
                    }
                    //circulation
                    globalPatient.treatments.circulation.dressing.hemostatic = p.treatments.circulation.dressing.hemostatic;
                    globalPatient.treatments.circulation.dressing.pressure = p.treatments.circulation.dressing.pressure;
                    globalPatient.treatments.circulation.dressing.other = p.treatments.circulation.dressing.other;
                    globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;

                    globalPatient.treatments.circulation.TQ.truncal = p.treatments.circulation.TQ.truncal;
                    globalPatient.treatments.circulation.TQ.junctional = p.treatments.circulation.TQ.junctional;
                    globalPatient.treatments.circulation.TQ.extremity = p.treatments.circulation.TQ.extremity;

                    //Click any button that needs to be clicked
                    //chestSeal
                    if (!isInFocus)
                    {
                    if (p.treatments.breathing.chestSeal)
                    {
                        chestSealButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        chestSealButton.Background = Brushes.DimGray;
                    }
                    //chestTube
                    if (p.treatments.breathing.chestTube)
                    {
                        chestTubeButoon.Background = Brushes.Yellow;
                    }
                    else
                    {
                        chestTubeButoon.Background = Brushes.DimGray;
                    }
                    //needleD
                    if (p.treatments.breathing.needleD)
                    {
                        needleDButton.Background = Brushes.Yellow;
                    }
                    else
                    {
                        needleDButton.Background = Brushes.DimGray;
                    }
                    //o2
                    if (p.treatments.breathing.o2)
                    {
                        o2Button.Background = Brushes.Yellow;
                    }
                    else
                    {
                        o2Button.Background = Brushes.DimGray;
                    }
                    }

                }
            }));
        }

        private void bindButtonData()
        {
            o2Button.Background = Brushes.DimGray;
            needleDButton.Background = Brushes.DimGray;
            chestSealButton.Background = Brushes.DimGray;
            chestTubeButoon.Background = Brushes.DimGray;

            //set the bools to false at startup
            globalPatient.treatments.breathing.o2 = false;
            globalPatient.treatments.breathing.needleD = false;
            globalPatient.treatments.breathing.chestSeal = false;
            globalPatient.treatments.breathing.chestTube = false;
        }

        private void breathingButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            isInFocus = true;

            if (b.Background == Brushes.DimGray)
            {
                b.Background = Brushes.Yellow;

                if (b.Name.Contains("o2"))
                {
                    globalPatient.treatments.breathing.o2 = true;
                }
                else if (b.Name.Contains("needleD"))
                {
                    globalPatient.treatments.breathing.needleD = true;
                }
                else if (b.Name.Contains("chestSeal"))
                {
                    globalPatient.treatments.breathing.chestSeal = true;
                }
                else if (b.Name.Contains("chestTube"))
                {
                    globalPatient.treatments.breathing.chestTube = true;
                }

            }

            //If the button is clicked, unclick the button
            else if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.DimGray;

                if (b.Name.Contains("o2"))
                {
                    globalPatient.treatments.breathing.o2 = false;
                }
                else if (b.Name.Contains("needleD"))
                {
                    globalPatient.treatments.breathing.needleD = false;
                }
                else if (b.Name.Contains("chestSeal"))
                {
                    globalPatient.treatments.breathing.chestSeal = false;
                }
                else if (b.Name.Contains("chestTube"))
                {
                    globalPatient.treatments.breathing.chestTube = false;
                }

            }
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
            isInFocus = false;
                globalPatient.DBOperation = true;
                globalPatient.treatments.breathing.dbSave = true;
                _systemMessages.AddMessage(globalPatient);
        }

        private bool checkForData()
        {
            if (globalPatient.treatments.breathing.chestTube)
            {
                return true;
            }
            else if (globalPatient.treatments.breathing.chestSeal)
            {
                return true;
            }
            else if (globalPatient.treatments.breathing.needleD)
            {
                return true;
            }
            else if (globalPatient.treatments.breathing.o2)
            {
                return true;
            }
            return false;

        }
    }
}

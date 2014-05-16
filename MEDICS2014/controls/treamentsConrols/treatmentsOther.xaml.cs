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
using System.Json;

namespace MEDICS2014.controls.treamentsConrols
{
    /// <summary>
    /// Interaction logic for treatmentsOther.xaml
    /// </summary>
    public partial class treatmentsOther : UserControl
    {
        Messages _messages = Messages.Instance;

        List<Button> allButtonsList = new List<Button>();

        patient globalPatient = new patient();

        SystemMessages _systemMessages = SystemMessages.Instance;

        bool isInFocus = false;

        public treatmentsOther()
        {
            InitializeComponent();

            allButtonsList.Add(combatPillPackButton);
            allButtonsList.Add(eyeShieldLButton);
            allButtonsList.Add(eyeShieldRButton);
            allButtonsList.Add(splintButton);
            allButtonsList.Add(hypothermiaPreventionButton);

            bindButtonsAndData();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);

           
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
                if (p.fromVDES)
                {
                    bool needSave = false;
                    if (p.OtherTreatments.CombatPillPack == "True")
                    {
                        needSave = true;
                        if (globalPatient.OtherTreatments.CombatPillPack == "True")
                        {
                            globalPatient.OtherTreatments.CombatPillPack = "False";
                        }
                        else
                        {
                            globalPatient.OtherTreatments.CombatPillPack = "True";
                        }
                    }
                    if (p.OtherTreatments.EyeShieldL == "True")
                    {
                        needSave = true;
                        if (globalPatient.OtherTreatments.EyeShieldL == "True")
                        {
                            globalPatient.OtherTreatments.EyeShieldL = "False";
                        }
                        else
                        {
                            globalPatient.OtherTreatments.EyeShieldL = "True";
                        }
                    }
                    if (p.OtherTreatments.EyeShieldR == "True")
                    {
                        needSave = true;
                        if (globalPatient.OtherTreatments.EyeShieldR == "True")
                        {
                            globalPatient.OtherTreatments.EyeShieldR = "False";
                        }
                        else
                        {
                            globalPatient.OtherTreatments.EyeShieldR = "True";
                        }
                    }
                    if (p.OtherTreatments.HypothermiaPrevention == "True")
                    {
                        needSave = true;
                        if (globalPatient.OtherTreatments.HypothermiaPrevention == "True")
                        {
                            globalPatient.OtherTreatments.HypothermiaPrevention = "False";
                        }
                        else
                        {
                            globalPatient.OtherTreatments.HypothermiaPrevention = "True";
                        }
                    }
                    if (p.OtherTreatments.Splint == "True")
                    {
                        needSave = true;
                        if (globalPatient.OtherTreatments.Splint == "True")
                        {
                            globalPatient.OtherTreatments.Splint = "False";
                        }
                        else
                        {
                            globalPatient.OtherTreatments.Splint = "True";
                        }
                    }

                    if (needSave)
                    {
                        bindButtonsWithData();
                        globalPatient.DBOperation = true;
                        globalPatient.OtherTreatments.dbSave = true;
                        _systemMessages.AddMessage(globalPatient);
                    }

                }
                else if (p.fromDatabase)
                {
                    
                    if (!isInFocus)
                    {
                        globalPatient.OtherTreatments = p.OtherTreatments;
                        bindButtonsWithData();
                    }
                }

            }));

        }

        private void bindButtonsWithData()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                bindButtonsAndData();

                if (globalPatient.OtherTreatments.CombatPillPack == "True")
                {
                    combatPillPackButton.Background = Brushes.Yellow;
                    combatPillPackButton.Foreground = Brushes.Black;
                }
                if (globalPatient.OtherTreatments.EyeShieldR == "True")
                {
                    eyeShieldRButton.Background = Brushes.Yellow;
                    eyeShieldRButton.Foreground = Brushes.Black;
                }
                if (globalPatient.OtherTreatments.EyeShieldL == "True")
                {
                    eyeShieldLButton.Background = Brushes.Yellow;
                    eyeShieldLButton.Foreground = Brushes.Black;
                }
                if (globalPatient.OtherTreatments.HypothermiaPrevention == "True")
                {
                    hypothermiaPreventionButton.Background = Brushes.Yellow;
                    hypothermiaPreventionButton.Foreground = Brushes.Black;
                }
                if (globalPatient.OtherTreatments.Splint == "True")
                {
                    splintButton.Background = Brushes.Yellow;
                    splintButton.Foreground = Brushes.Black;
                }

            }));
        }


        private void bindButtonsAndData()
        {

            foreach (Button b in allButtonsList)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;
            }

        }

        private void treatmentButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;

            //If not clicked
            if (b.Background == Brushes.Firebrick)
            {
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;
            }
            //if it is clicked
            else if  (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;
            }

            switch (b.Name)
            {
                case "combatPillPackButton":
                    if (globalPatient.OtherTreatments.CombatPillPack == null || globalPatient.OtherTreatments.CombatPillPack == "False")
                    {
                        globalPatient.OtherTreatments.CombatPillPack = "True";
                    }
                    else if (globalPatient.OtherTreatments.CombatPillPack == "True")
                    {
                        globalPatient.OtherTreatments.CombatPillPack = "False";
                    }
                    break;
                case "eyeShieldLButton":
                    if (globalPatient.OtherTreatments.EyeShieldL == null || globalPatient.OtherTreatments.EyeShieldL == "False")
                    {
                        globalPatient.OtherTreatments.EyeShieldL = "True";
                    }
                    else if (globalPatient.OtherTreatments.EyeShieldL == "True")
                    {
                        globalPatient.OtherTreatments.EyeShieldL = "False";
                    }
                    break;
                case "eyeShieldRButton":
                    if (globalPatient.OtherTreatments.EyeShieldR == null || globalPatient.OtherTreatments.EyeShieldR == "False")
                    {
                        globalPatient.OtherTreatments.EyeShieldR = "True";
                    }
                    else if (globalPatient.OtherTreatments.EyeShieldR == "True")
                    {
                        globalPatient.OtherTreatments.EyeShieldR = "False";
                    }
                    break;
                case "splintButton":
                    if (globalPatient.OtherTreatments.Splint == null || globalPatient.OtherTreatments.Splint == "False")
                    {
                        globalPatient.OtherTreatments.Splint = "True";
                    }
                    else if (globalPatient.OtherTreatments.Splint == "True")
                    {
                        globalPatient.OtherTreatments.Splint = "False";
                    }
                    break;
                case "hypothermiaPreventionButton":
                    if (globalPatient.OtherTreatments.HypothermiaPrevention == null || globalPatient.OtherTreatments.HypothermiaPrevention == "False")
                    {
                        globalPatient.OtherTreatments.HypothermiaPrevention = "True";
                    }
                    else if (globalPatient.OtherTreatments.HypothermiaPrevention == "True")
                    {
                        globalPatient.OtherTreatments.HypothermiaPrevention = "False";
                    }
                    break;

                   
            }

            globalPatient.DBOperation = true;
            globalPatient.OtherTreatments.dbSave = true;
            _systemMessages.AddMessage(globalPatient);
            
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
            isInFocus = false;
            globalPatient.DBOperation = true;
            //globalPatient.treatments.other.dbSave = true;
            globalPatient.OtherTreatments.dbSave = true;
            _systemMessages.AddMessage(globalPatient);
        }
    }
}

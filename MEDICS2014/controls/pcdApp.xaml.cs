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
    /// Interaction logic for pcdApp.xaml
    /// </summary>
    public partial class pcdApp : UserControl
    {
        List<Button> allButtonsList = new List<Button>();

        patient globalPatient = new patient();

        SystemMessages _systemMessages = SystemMessages.Instance;
        Messages _messages = Messages.Instance;

        public pcdApp()
        {
            InitializeComponent();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

            bindButtons();
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleMessageData(message);
            }
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

        public void handleMessageData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "CLEAR CONTROL":
                        //Hide all the things
                        foreach (Button b in allButtonsList)
                        {
                            b.Background = Brushes.DimGray;
                            b.Visibility = Visibility.Hidden;

                        }
                        break;
                }
            }));
        }

        private void handleAppPatientData(patient p)
        {
            //Populate the vitals bar if vitals exist in the patient
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (p.dbPCDs != globalPatient.dbPCDs && p.fromDatabase)
                {
                    //First, remove all the buttons and clear their contents
                    foreach (Button b in allButtonsList)
                    {
                        b.Visibility = Visibility.Hidden;
                        b.Background = Brushes.DimGray;
                        b.Content = "";
                    }
                    //Sedond, populate the control with the buttons
                    if (p.dbPCDs.Count > 0)
                    {
                        noPCDlabel.Visibility = Visibility.Hidden;

                        foreach (string pcdID in p.dbPCDs)
                        {
                            foreach (Button b in allButtonsList)
                            {
                                if (b.Visibility == Visibility.Hidden)
                                {
                                    b.Visibility = Visibility.Visible;
                                    b.Content = pcdID;
                                    break;
                                }
                            }
                        }

                        if (p.attachedPCDs.Count > 0)
                        {
                            globalPatient.attachedPCDs = p.attachedPCDs;
                            foreach (string patPCD in p.attachedPCDs)
                            {
                                foreach (Button b in allButtonsList)
                                {
                                    if (b.Content.ToString() == patPCD)
                                    {
                                        b.Background = Brushes.Yellow;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        noPCDlabel.Visibility = Visibility.Visible;
                    }
                }
            }));
        }


        private void bindButtons()
        {
            //noPCDlabel.Visibility = Visibility.Hidden;

            allButtonsList.Add(pcdButton1);
            allButtonsList.Add(pcdButton2);
            allButtonsList.Add(pcdButton3);
            allButtonsList.Add(pcdButton4);
            allButtonsList.Add(pcdButton5);
            allButtonsList.Add(pcdButton6);
            allButtonsList.Add(pcdButton7);
            allButtonsList.Add(pcdButton8);
            allButtonsList.Add(pcdButton9);
            allButtonsList.Add(pcdButton10);
            allButtonsList.Add(pcdButton11);
            allButtonsList.Add(pcdButton12);

            foreach (Button b in allButtonsList)
            {
                b.Background = Brushes.DimGray;
                b.Visibility = Visibility.Hidden;

            }

        }

        private void pcdButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            bool dboperationNeeded = false;
            //If clicked unlick and remove
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.DimGray;
                string ID = b.Content.ToString();
                if (globalPatient.attachedPCDs.Contains(ID))
                {
                    globalPatient.attachedPCDs.Remove(ID);
                    dboperationNeeded = true;
                }
            }

            //If not clicked then click and add
            else if (b.Background == Brushes.DimGray)
            {
                b.Background = Brushes.Yellow;
                string ID = b.Content.ToString();
                if (!globalPatient.attachedPCDs.Contains(ID))
                {
                    globalPatient.attachedPCDs.Add(ID);
                    dboperationNeeded = true;
                }
            }

            //Save the database
            if (dboperationNeeded)
            {
                globalPatient.PCDupdate = true;
                globalPatient.DBOperation = true;
                _systemMessages.AddMessage(globalPatient);
            }
        }
    }
}

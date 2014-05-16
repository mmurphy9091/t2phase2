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
using MEDICS2014.controls;
using System.Diagnostics;
using System.ComponentModel;
//using System.Threading;



using System.Collections;

namespace MEDICS2014
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HomeScreen1 home1 = new HomeScreen1();
        Admin adminApp = new Admin();
        vitalsMain vitalsApp = new vitalsMain();
        mechanismOfInjury MOIapp = new mechanismOfInjury();
        injuriesMain injuriesApp = new injuriesMain();
        allergiesApp allergyApp = new allergiesApp();
        patientSummary patSummary = new patientSummary();
        treatmentsApp treatments = new treatmentsApp();
        medsApp medications = new medsApp();
        notesApp notes = new notesApp();
        patientList patList = new patientList();
        pcdApp pcd = new pcdApp();
        //private System.Object lockThis = new System.Object();

        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        Mqtt.mqttMain MQTTService = new Mqtt.mqttMain();
        //MQTTstarter mqttClient = new MQTTstarter();
        //Thread mqttThread;

        //Python Processes
        Process p1 = new Process();
        Process p2 = new Process();

        //string saveMessage = "";

        UserControl lastWindow;

        bool patSum = false;

        public MainWindow()
        {
            InitializeComponent();

            //mqttThread = new Thread(mqttClient.startMQTT);
            //mqttThread.Start();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);



            stackPanelMain.Children.Add(home1);
            lastWindow = home1;

            patientListStackPanel.Children.Add(patList);
            patientListStackPanel.Visibility = Visibility.Hidden;

            loadPythonScripts();

            //MQTTService.MqttStart();

            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        public void loadPythonScripts()
        {
            //Process[] localByName2;
            string location = System.Reflection.Assembly.GetEntryAssembly().Location.ToString();
            location += "pythonScripts";
            location = location.Replace("MEDICS2014.exe", "");

            string database = location + "\\couchDBplugin.py";
            string controller = location + "\\controller.py";

      
            p1.StartInfo.FileName = database;
            p2.StartInfo.FileName = controller;

            p1.Start();
            p2.Start();

            /*
                        string str;
             * localByName2 = Process.GetProcessesByName("python");
                        for (int jj = 0; jj < localByName2.Length; jj++)
                        {
                            str = localByName2[jj].StartInfo.FileName;
                        }
                        */
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleAppData(message);
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

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            stackPanelMain.Children.Clear();
            stackPanelMain.Children.Add(home1);
            lastWindow = home1;
        }

        private void handleAppData(string message)
        {
 ///           lock (lockThis)
 ///           { 

            //Pat sum stuff, you wouldn't get it because you're not cool enough
            if (message != "PAT SUM")
            {
                //lastMessage = message;
            }

            try
            {
                //So MQTT can access the GUI
                this.Dispatcher.Invoke((Action)(() =>
                {
                    //handle the patlist commands
                    if (message != "PATLIST" && patientListStackPanel.Visibility == Visibility.Visible)
                    {
                        if (message != "GETPATLIST" && !message.Contains("HEREISPATIENTLIST"))
                        {
                            patientListStackPanel.Visibility = Visibility.Hidden;
                        }
                    }
                    switch (message)
                    {
                        case "HOME":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(home1);
                            lastWindow = home1;
                            break;
                        case "ADMIN":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(adminApp);
                            lastWindow = adminApp;
                            break;
                        case "SAS":
                        case "ADD NEW VITALS":
                        case "VITALS SUMMARY":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(vitalsApp);
                            lastWindow = vitalsApp;
                            break;
                        case "ALLERGIES":
                        case "ADMIN ALLERGIES":
                        case "ALLERGIES MEDICATIONS":
                        case "ALLERGIES MEDICATIONS 2":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(allergyApp);
                            lastWindow = allergyApp;
                            break;
                        case "MEDICATIONS":
                        case "MEDS":
                        case "MEDS PAGE 1":
                        case "MEDS ANALGESIC":
                        case "MEDS ANTIBIOTIC 1":
                        case "MEDS ANTIBIOTIC 2":
                        case "MEDS DETAILS":
                        case "MEDS UNITS":
                        case "MEDS OTHERS":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(medications);
                            lastWindow = medications;
                            break;
                        case "TREATMENTS":
                        case "TREATMENTS MAIN":
                        case "TREATMENTS CIRCULATION":
                        case "TREATMENTS AIRWAY":
                        case "TREATMENTS BREATHING":
                        case "TREATMENTS FLUIDS":
                        case "TREATMENTS SALINE":
                        case "TREATMENTS RINGERS":
                        case "TREATMENTS DEXTROSE":
                        case "TREATMENTS FLUIDS OTHER":
                        case "TREATMENTS BLOOD PRODUCTS":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(treatments);
                            lastWindow = treatments;
                            break;
                        case "INJURIES":
                        case "INJURIES MAIN":
                        case "INJURIES HEAD FRONT":
                        case "INJURIES HEAD BACK":
                        case "INJURIES CHEST":
                        case "INJURIES RIGHT ARM FRONT":
                        case "INJURIES RIGHT ARM BACK":
                        case "INJURIES LEFT ARM FRONT":
                        case "INJURIES LEFT ARM BACK":
                        case "INJURIES ABDOMEN":
                        case "INJURIES UPPER BACK":
                        case "INJURIES LOWER BACK":
                        case "INJUIRES RIGHT LEG FRONT":
                        case "INJURIES RIGHT LEG BACK":
                        case "INJURIES LEFT LEG FRONT":
                        case "INJURIES LEFT LEG BACK":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(injuriesApp);
                            lastWindow = injuriesApp;
                            break;
                        case "PAT SUM":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(patSummary);
                            break;
                        case "NOTES":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(notes);
                            lastWindow = notes;
                            break;
                        case "PATLIST":
                            patientListStackPanel.Visibility = Visibility.Visible;
                            break;
                        /*
                    case "CLEAR CONTROL":
                        HRLabel.Content = "--";
                        RESPLabel.Content = "--";
                        BPLabel.Content = "--/--";
                        TEMPLabel.Content = "--";
                        SP02Label.Content = "--";
                        break;
                         */
                        case "PCD":
                            stackPanelMain.Children.Clear();
                            stackPanelMain.Children.Add(pcd);
                            lastWindow = pcd;
                            break;
                        case "EXIT":
                            this.Close();
                            break;

                    }
                    if (message.Contains("PATID:") || message.Contains("NEWPATIENT:"))
                    {
                        HRLabel.Content = "--";
                        RESPLabel.Content = "--";
                        BPLabel.Content = "--/--";
                        TEMPLabel.Content = "--";
                        SP02Label.Content = "--";
                    }
                    /*
                    //Switch to homescreen 1
                    if (message == "HOME")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(home1);
                        lastWindow = home1;
                    }
    
                    //Switch to the admin app
                    if (message == "ADMIN")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(adminApp);
                        lastWindow = adminApp;
                    }

                    //Switch to the Procedure app
                    if (message == "PROCEDURE")
                    {
                        stackPanelMain.Children.Clear();
                        //stackPanelMain.Children.Add(adminApp);
                    }

                    //Switch to the Vitals app
                    if ((message == "SAS") || (message == "ADD NEW VITALS") || (message == "VITALS SUMMARY"))
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(vitalsApp);
                        lastWindow = vitalsApp;
                    }

                    //Switch to the TC3 app
                    if (message == "ALLERGIES" || message == "ADMIN ALLERGIES" || message == "ALLERGIES MEDICATIONS" || message == "ALLERGIES MEDICATIONS 2")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(allergyApp);
                        lastWindow = allergyApp;
                    }

                    //Switch to the Medications App
                    if (message == "MEDICATIONS")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(medications);
                    }

                    //Switch to the Seconday app
                    if (message == "SECONDARY")
                    {
                        stackPanelMain.Children.Clear();
                        //stackPanelMain.Children.Add(adminApp);
                    }

                    //Swtich to the Treatments app
                    if (message == "TREATMENTS" || message == "TREATMENTS MAIN" || message == "TREATMENTS CIRCULATION" || message == "TREATMENTS AIRWAY" || message == "TREATMENTS BREATHING"
                        || message == "TREATMENTS FLUIDS" || message == "TREATMENTS SALINE" || message == "TREATMENTS RINGERS" || message == "TREATMENTS DEXTROSE" || message == "TREATMENTS FLUIDS OTHER"
                        || message == "TREATMENTS BLOOD PRODUCTS")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(treatments);
                    }

                    //Switch to Injuries app
                    if (message == "INJURIES" || message == "INJURIES MAIN" || message == "INJURIES HEAD FRONT" ||
                        message == "INJURIES HEAD BACK" || message == "INJURIES CHEST" || message == "INJURIES RIGHT ARM FRONT" ||
                        message == "INJURIES RIGHT ARM BACK" || message == "INJURIES LEFT ARM FRONT" || message == "INJURIES LEFT ARM BACK" ||
                        message == "INJURIES ABDOMEN" || message == "INJURIES UPPER BACK" || message == "INJURIES LOWER BACK" ||
                        message == "INJUIRES RIGHT LEG FRONT" || message == "INJURIES RIGHT LEG BACK" || message == "INJURIES LEFT LEG FRONT" ||
                        message == "INJURIES LEFT LEG BACK")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(injuriesApp);
                        lastWindow = injuriesApp;
                    }

                    //Switch to the Patient Summary page
                    if (message == "PAT SUM")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(patSummary);
                    }

                    //Switch to the next Home Screen
                    if (message == "NEXT")
                    {
                        stackPanelMain.Children.Clear();
                        //stackPanelMain.Children.Add(adminApp);
                    }

                    //Switch to the Previous Home Screen
                    if (message == "PREVIOUS")
                    {

                    }

                    if (message == "NOTES")
                    {
                        stackPanelMain.Children.Clear();
                        stackPanelMain.Children.Add(notes);
                    }

                    */
                }));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
///          }
        }

        private void handleAppPatientData(patient p)
        {
            try
            {
                //Populate the vitals bar if vitals exist in the patient
                this.Dispatcher.Invoke((Action)(() =>
                    {
                        if (p.HR != null || p.Resp != null || p.BPSYS != null || p.BPDIA != null || p.Temp != null || p.SP02 != null)
                        {
                            if (p.HR != "")
                            {
                                HRLabel.Content = p.HR.ToString();
                            }
                            if (p.Resp != "")
                            {
                                RESPLabel.Content = p.Resp.ToString();
                            }
                            if (p.BPSYS != "" && p.BPDIA != "")
                            {
                                BPLabel.Content = p.BPSYS.ToString() + "/" + p.BPDIA.ToString();
                            }
                            if (p.Temp != "")
                            {
                                TEMPLabel.Content = p.Temp.ToString() + " " + p.TempType.ToString();
                            }
                            if (p.SP02 != "")
                            {
                                SP02Label.Content = p.SP02.ToString();
                            }
                        }
                    }));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
        }

        private void patSumBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (!patSum)
                {
                    stackPanelMain.Children.Clear();
                    stackPanelMain.Children.Add(patSummary);
                    patSum = true;
                    _messages.AddMessage("PATSUM");
                }
                else
                {
                    stackPanelMain.Children.Clear();
                    stackPanelMain.Children.Add(lastWindow);
                    //lastWindow = patSummary;
                    patSum = false;
                }

            }));

        }

        private void PatientListButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("GETPATLIST");

            _messages.AddMessage("PATLIST");
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                p1.Kill();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                p2.Kill();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                MQTTService.killMQTT();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.ToString());
            }
            //mqttThread.Abort();
        }

    }
}

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
    /// Interaction logic for vitals.xaml
    /// </summary>
    public partial class vitals : UserControl
    {
        List<vitalsInfo> vitalsGlobal = new List<vitalsInfo>();
        List<string> vitalsListGlobal = new List<string>();
        SystemMessages _systemMessages = SystemMessages.Instance;
        Messages _messages = Messages.Instance;


        public vitals()
        {
            InitializeComponent();

            _systemMessages.HandleSystemMessage += new EventHandler(vitalsOnHandleMessage);

            //bindVitals();
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
                        hrLabel.Content = "--";
                        respLabel.Content = "--";
                        bpLabel.Content = "--/--";
                        tempLabel.Content = "--";
                        sp02Label.Content = "--";
                        avpuLabel.Content = "--";
                        psLabel.Content = "--";
                        timeLabel.Content = "--";
                        break;
                }
            }));
        }

        public void bindVitals()
        {

            vitalsInfo temp = new vitalsInfo();
            temp.heartRate = "11";
            temp.respiratory = "22";
            temp.bloodPressureSYS = "33";
            temp.bloodPressureDIA = "44";
            temp.temperature = "55";
            temp.EtC02 = "66";
            temp.AVPU = "A";
            temp.painScale = "3";
            DateTime date = DateTime.Now;
            temp.time = date.ToString("HHmm");
            //temp.time = DateTime.Now;
            temp.setSummary();

            //vitalsListGlobal.Add(temp.summary);
            vitalsGlobal.Add(temp);

            vitalsInfo temp2 = new vitalsInfo();
            temp2.heartRate = "66";
            temp2.respiratory = "55";
            temp2.bloodPressureSYS = "44";
            temp2.bloodPressureDIA = "33";
            temp2.temperature = "22";
            temp2.EtC02 = "11";
            temp2.AVPU = "U";
            temp2.painScale = "10";
            DateTime date2 = DateTime.Now;
            temp2.time = date2.ToString("HHmm");
            //temp2.time = DateTime.Now;
            temp2.setSummary();

            //vitalsListGlobal.Add(temp2.summary);
            vitalsGlobal.Add(temp2);

            vitalsGlobal.OrderByDescending(x => x.time);

            foreach (vitalsInfo v in vitalsGlobal)
            {
                vitalsListGlobal.Add(v.summary);
            }

            vitalsListBox.ItemsSource = vitalsListGlobal.ToList();
        }

        private void vitalsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int heartRateIndex = vitalsListBox.SelectedIndex;
            int bloodPressureSYSIndex = vitalsListBox.SelectedIndex;
            int bloodPressureDIAIndex = vitalsListBox.SelectedIndex;
            int respiratoryIndex = vitalsListBox.SelectedIndex;
            int tempIndex = vitalsListBox.SelectedIndex;
            int etc02Index = vitalsListBox.SelectedIndex;
            int avpuIndex = vitalsListBox.SelectedIndex;
            int psIndex = vitalsListBox.SelectedIndex;

            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[heartRateIndex].heartRate != null)
                {
                    hrLabel.Content = vitalsGlobal[heartRateIndex].heartRate;
                    break;
                }
                heartRateIndex++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[bloodPressureSYSIndex].bloodPressureSYS != null)
                {
                    if (vitalsGlobal[bloodPressureDIAIndex].bloodPressureDIA != null)
                    {
                        bpLabel.Content = vitalsGlobal[bloodPressureSYSIndex].bloodPressureSYS + "/" + vitalsGlobal[bloodPressureSYSIndex].bloodPressureDIA;
                        break;
                    }
                }
                bloodPressureSYSIndex++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[respiratoryIndex].respiratory != null)
                {
                    respLabel.Content = vitalsGlobal[respiratoryIndex].respiratory;
                    break;
                }
                respiratoryIndex++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[tempIndex].temperature != null)
                {
                    tempLabel.Content = vitalsGlobal[tempIndex].temperature;
                    break;
                }
                tempIndex++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[etc02Index].EtC02 != null)
                {
                    sp02Label.Content = vitalsGlobal[etc02Index].EtC02;
                    break;
                }
                etc02Index++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[avpuIndex].AVPU != null)
                {
                    avpuLabel.Content = vitalsGlobal[avpuIndex].AVPU;
                    break;
                }
                avpuIndex++;
            }
            foreach (vitalsInfo vitals in vitalsGlobal)
            {
                if (vitalsGlobal[psIndex].painScale != null)
                {
                    psLabel.Content = vitalsGlobal[psIndex].painScale;
                    break;
                }
                psIndex++;
            }

            //hrLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].heartRate;
            //bpLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].bloodPressure;
            //respLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].respiratory;
            //tempLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].temperature;
            //etc02Label.Content = vitalsGlobal[vitalsListBox.SelectedIndex].EtC02;
            timeLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].time;
            //timeLabel.Content = vitalsGlobal[vitalsListBox.SelectedIndex].time.ToLongTimeString();
        }

        public void vitalsOnHandleMessage(object sender, EventArgs args)
        {
            var vitalsMessageEvent = args as SystemMessageEventArgs;
            if (vitalsMessageEvent != null)
            {
                patient vitalesMessage = vitalsMessageEvent.systemMessage;
                //System.Windows.MessageBox.Show(message);
                handleVitalsVoiceData(vitalesMessage);
            }
        }

        private void handleVitalsVoiceData(patient data)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    try
                    {
                        if (data.fromDatabase)
                        {
                            //here we will need to clear the list and rewrite it, based on what is now in the database
                            if (data.allVitals != null)
                            {
                                vitalsGlobal.Clear();
                                vitalsListGlobal.Clear();

                                foreach (patient.VitalsList v in data.allVitals)
                                {
                                    vitalsInfo vitals = new vitalsInfo();
                                    vitals.heartRate = v.HR;
                                    vitals.respiratory = v.Resp;
                                    vitals.bloodPressureSYS = v.BPSYS;
                                    vitals.bloodPressureDIA = v.BPDIA;
                                    vitals.temperature = v.Temp;
                                    vitals.EtC02 = v.SP02;
                                    DateTime date = DateTime.Now;
                                    if (v.vitalsTime == "")
                                    {
                                        vitals.time = date.ToString("HHmm");
                                    }
                                    if (v.vitalsTime == null)
                                    {
                                        vitals.time = date.ToString("HHmm");
                                    }
                                    else
                                    {
                                        vitals.time = v.vitalsTime;
                                    }
                                    //vitals.time = DateTime.Now;
                                    vitals.AVPU = v.AVPU;
                                    vitals.painScale = v.painScale;
                                    vitals.setSummary();

                                    vitalsGlobal.Add(vitals);

                                    vitalsGlobal = vitalsGlobal.OrderByDescending(x => x.time).ToList();
                                    vitalsListGlobal.Clear();

                                    foreach (vitalsInfo v2 in vitalsGlobal)
                                    {
                                        vitalsListGlobal.Add(v2.summary);
                                    }
                                }

                                addNewVitals();
                            }
                        }
                        else if (data.HR != null || data.Resp != null || data.BPSYS != null || data.BPDIA != null || data.Temp != null || data.SP02 != null)
                        {
                            if (data.HR == "" && data.Resp == "" && data.BPSYS == "" && data.BPDIA == "" && data.Temp == "" && data.SP02 == "")
                            {
                                //Do nothing
                            }
                            else
                            {
                                vitalsInfo vitals = new vitalsInfo();
                                vitals.heartRate = data.HR;
                                vitals.respiratory = data.Resp;
                                vitals.bloodPressureSYS = data.BPSYS;
                                vitals.bloodPressureDIA = data.BPDIA;
                                vitals.temperature = data.Temp;
                                vitals.EtC02 = data.SP02;
                                DateTime date = DateTime.Now;
                                if (data.vitalsTime == "")
                                {
                                    vitals.time = date.ToString("HHmm");
                                }
                                else if (data.vitalsTime == null)
                                {
                                    vitals.time = date.ToString("HHmm");
                                }
                                else
                                {
                                    vitals.time = data.vitalsTime;
                                }
                                //vitals.time = DateTime.Now;
                                vitals.AVPU = data.AVPU;
                                vitals.painScale = data.painScale;
                                vitals.setSummary();

                                vitalsGlobal.Add(vitals);

                                vitalsGlobal = vitalsGlobal.OrderByDescending(x => x.time).ToList();
                                vitalsListGlobal.Clear();

                                foreach (vitalsInfo v in vitalsGlobal)
                                {
                                    vitalsListGlobal.Add(v.summary);
                                }

                                addNewVitals();
                            }
                        }
                    }
                    catch { }
                }));
        }

        public void addNewVitals()
        {
            //vitalsListBox.Items.Clear();
            vitalsListBox.ItemsSource = vitalsListGlobal.ToList();
            //vitalsListBox.Items.Add(vitalsListGlobal[0]);
            vitalsListBox.SelectedIndex = 0;
        }

        public void selectVitalsAtTime(string commandTime)
        {
            vitalsInfo closestDate = null;
            long min = int.MaxValue;

            int temp;
            int command = int.Parse(commandTime);

            foreach (vitalsInfo v in vitalsGlobal)
            {
                temp = int.Parse(v.time);
                if (Math.Abs(temp - command) < min)
                {
                    min = Math.Abs(temp - command);
                    closestDate = v;
                }
                /*
                if (Math.Abs(v.time.Ticks - commandTime.Ticks) < min)
                {
                    min = Math.Abs(v.time.Ticks - commandTime.Ticks);
                    closestDate = v;
                }
                 */
            }
            if (closestDate != null)
            {
                vitalsListBox.SelectedIndex = vitalsGlobal.IndexOf(closestDate);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("ADD NEW VITALS");
        }

        /*
        public List<vitalsInfo> getVitalsGlobal()
        {
            return vitalsGlobal;
        }
         */
    }
}

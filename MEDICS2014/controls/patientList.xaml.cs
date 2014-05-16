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
using Newtonsoft.Json;

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for patientList.xaml
    /// </summary>
    public partial class patientList : UserControl
    {
        Messages _messages = Messages.Instance;

        List<patientData> globalPatDataList = new List<patientData>();
        List<Button> allButtonsList = new List<Button>();

        public patientList()
        {
            InitializeComponent();

            bindButtonData();

            _messages.HandleMessage += new EventHandler(OnHandleMessage);

        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleListData(message);
            }
        }

        private void handleListData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (message.Contains("HEREISPATIENTLIST"))
                {
                    message = message.Replace("HEREISPATIENTLIST", "");

                    //clear the global list
                    globalPatDataList.Clear();

                    //If the JSON holds the entire patient data, get and parse it here
                    try
                    {
                        // fullPatientResponseClass fullPatient =
                        //   (fullPatientResponseClass)json_serializer.DeserializeObject(dbMessage);
                        patientListFullClass list = JsonConvert.DeserializeObject<patientListFullClass>(message);
                        if (parsePatientIDList(list))
                        {
                            //hide all the buttons
                            foreach (Button b in allButtonsList)
                            {
                                b.Visibility = Visibility.Hidden;
                            }

                            foreach (patientData data in globalPatDataList)
                            {
                                foreach (Button b in allButtonsList)
                                {
                                    if (b.Visibility == Visibility.Hidden)
                                    {
                                        b.Visibility = Visibility.Visible;
                                        string idData = data.lastName + "," + data.firstName;
                                        b.Content = idData;
                                        break;
                                    }
                                }

                            }

                            //Finally, add a button to add a new patient, if needed
                            foreach (Button b in allButtonsList)
                            {
                                if (b.Visibility == Visibility.Hidden)
                                {
                                    b.Visibility = Visibility.Visible;
                                    b.Content = "ADD NEW PATIENT";
                                    break;
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }

                }

            }));


        }

        public bool parsePatientIDList(patientListFullClass list)
        {
            if (list.total_rows >= 0)
            {
                for (int i = 0; i < list.total_rows; i++)
                {
                    patientData newData = new patientData();

                    newData.ID = list.rows[i].value.ID.ToString();
                    newData.firstName = list.rows[i].value.FirstName.ToString();
                    newData.lastName = list.rows[i].value.LastName.ToString();

                    globalPatDataList.Add(newData);
                }
                return true;

            }
            return false;
        }

        private void bindButtonData()
        {
            allButtonsList.Add(patButton1);
            allButtonsList.Add(patButton2);
            allButtonsList.Add(patButton3);
            allButtonsList.Add(patButton4);
            allButtonsList.Add(patButton5);
            allButtonsList.Add(patButton6);

            foreach (Button b in allButtonsList)
            {
                b.Visibility = Visibility.Hidden;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sends a messaage to clear every control
            _messages.AddMessage("CLEAR CONTROL");

            //sends a messaage to the DBhandler requesting all database information
            Button b = (Button)sender;

            if (b.Content.ToString() == "ADD NEW PATIENT")
            {
                //Guid newId = Guid.NewGuid;
                _messages.AddMessage("NEWPATIENT:" + Guid.NewGuid().ToString());
            }
            else
            {
                List<string> names = b.Content.ToString().Split(',').ToList();

                foreach (patientData data in globalPatDataList)
                {
                    if (data.lastName == names[0] && data.firstName == names[1])
                    {
                        _messages.AddMessage("PATID:" + data.ID);
                    }
                }
            }
        }

        public class patientData
        {
            public string ID;
            public string firstName;
            public string lastName;
        }
    }
}

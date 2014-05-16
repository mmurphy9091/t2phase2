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
    /// Interaction logic for patientSummary.xaml
    /// </summary>
    public partial class patientSummary : UserControl
    {
        injuriesPatSum injuries = new injuriesPatSum();
        SystemMessages _systemMessages = SystemMessages.Instance;
        Messages _messages = Messages.Instance;

        List<string> logList = new List<string>();
        List<patientLog> allLists = new List<patientLog>();
        string currentId;

        patient globalPatient = new patient();

        public patientSummary()
        {
            InitializeComponent();

            injuriesStackPanel.Children.Add(injuries);

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleLogMessage);
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
                        logListBox.ItemsSource = null;
                        break;
                }
                if (message.Contains("PATID:"))
                {
                    message = message.Replace("PATID:", "");
                    currentId = message;
                    //MessageBox.Show(currentPatID);
                }
                else if (message.Contains("NEWPATIENT:"))
                {
                    message = message.Replace("NEWPATIENT:", "");
                    currentId = message;
                    patientLog newLog = new patientLog();
                    newLog.patID = currentId;
                    allLists.Add(newLog);
                    //MessageBox.Show(currentPatID);
                }
                else if (message.Contains("HEREISPATIENTLIST"))
                {
                    message = message.Replace("HEREISPATIENTLIST", "");

                    //clear the global list
                    //globalPatDataList.Clear();

                    //If the JSON holds the entire patient data, get and parse it here
                    try
                    {
                        // fullPatientResponseClass fullPatient =
                        //   (fullPatientResponseClass)json_serializer.DeserializeObject(dbMessage);
                        patientListFullClass list = JsonConvert.DeserializeObject<patientListFullClass>(message);
                        if (parsePatientIDList(list))
                        {
        
                            //might not need to do anything here
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }

                }
            }));
        }

        public void OnHandleLogMessage(object sender, EventArgs args)
        {
            var vitalsMessageEvent = args as SystemMessageEventArgs;
            if (vitalsMessageEvent != null)
            {
                patient logMessage = vitalsMessageEvent.systemMessage;
                //System.Windows.MessageBox.Show(message);
                handleLogData(logMessage);
            }
        }

        private void handleLogData(patient p)
        {
            if (p.DBOperation)
            {
                //Handle injury adding
                //
                //
                //
                if (p.injuries.Count > 0)
                {
                    foreach (patient.Injuries i in p.injuries)
                    {
                        //if it's a sneaky patient data do nothing
                        if (i.Type == null || i.Type == "")
                        {
                            //do nothing
                        }
                        else
                        {
                            //if this is a database operation then this is easy
                            if (p.DBOperation)
                            {
                                if (i.delete)
                                {
                                    string logentry = "Injury deleted: " + i.Type + " at " + i.Location;
                                    addToLogListBox(logentry);
                                    
                                }
                                else
                                {
                                    string logentry = "Injury added: " + i.Type + " at " + i.Location;
                                    addToLogListBox(logentry);
                                    
                                }
                            }
                        }
                    }
                }

                //Handle Admin data adding
                //
                //
                //
                //first name
                if (p.firstName != null)
                {
                    //if first name is just blank
                    if (p.firstName == "" && globalPatient.firstName == null)
                    {
                        //do nothing
                    }
                    else if (p.firstName == "" && globalPatient.firstName != "")
                    {
                        //this means the first name was removed
                        string logentry = "First Name removed: " + globalPatient.firstName;
                        addToLogListBox(logentry);
                        
                        globalPatient.firstName = null;
                    }
                    else if (p.firstName != "" && globalPatient.firstName == null)
                    {
                        string logentry = "First Name added: " + p.firstName;
                        addToLogListBox(logentry);
                        
                        globalPatient.firstName = p.firstName;
                    }
                    else if (p.firstName != "" && p.firstName != globalPatient.firstName && globalPatient.firstName != "" && globalPatient.firstName != null)
                    {
                        string logentry = "First Name changed from " + globalPatient.firstName + " to: " + p.firstName;
                        addToLogListBox(logentry);
                        
                        globalPatient.firstName = p.firstName;
                    }
                }
                //middle name
                if (p.middleName != null)
                {
                    if (p.middleName == "" && globalPatient.middleName == null)
                    {
                        //do nothing
                    }
                    else if (p.middleName == "" && globalPatient.middleName != "")
                    {
                        //this means the first name was removed
                        string logentry = "Middle Name removed: " + globalPatient.middleName;
                        addToLogListBox(logentry);
                        
                        globalPatient.middleName = null;
                    }
                    else if (p.middleName != "" && globalPatient.middleName == null)
                    {
                        string logentry = "Middle Name added: " + p.middleName;
                        addToLogListBox(logentry);
                        
                        globalPatient.middleName = p.middleName;
                    }
                    else if (p.middleName != "" && p.middleName != globalPatient.middleName && globalPatient.middleName != "" && globalPatient.middleName != null)
                    {
                        string logentry = "Middle Name changed from " + globalPatient.middleName + " to: " + p.middleName;
                        addToLogListBox(logentry);
                        
                        globalPatient.middleName = p.middleName;
                    }
                }
                //last name
                if (p.lastName != null)
                {
                    if (p.lastName == "" && globalPatient.lastName == null)
                    {
                        //do nothing
                    }
                    else if (p.lastName == "" && globalPatient.lastName != "")
                    {
                        //this means the first name was removed
                        string logentry = "Last Name removed: " + globalPatient.lastName;
                        addToLogListBox(logentry);
                        
                        globalPatient.lastName = null;
                    }
                    else if (p.lastName != "" && globalPatient.lastName == null)
                    {
                        string logentry = "Last Name added: " + p.lastName;
                        addToLogListBox(logentry);
                        
                        globalPatient.lastName = p.lastName;
                    }
                    else if (p.lastName != "" && p.lastName != globalPatient.lastName && globalPatient.lastName != "" && globalPatient.lastName != null)
                    {
                        string logentry = "Last Name changed from " + globalPatient.lastName + " to: " + p.lastName;
                        addToLogListBox(logentry);
                        
                        globalPatient.lastName = p.lastName;
                    }
                }
                //gender
                if (p.gender != null)
                {
                    if (p.gender == "" && globalPatient.gender == null)
                    {
                        //do nothing
                    }
                    else if (p.gender == "" && globalPatient.gender != "")
                    {
                        //this means the first name was removed
                        string logentry = "Gender removed: " + globalPatient.gender;
                        addToLogListBox(logentry);
                        
                        globalPatient.gender = null;
                    }
                    else if (p.gender != "" && globalPatient.gender == null)
                    {
                        string logentry = "Gender added: " + p.gender;
                        addToLogListBox(logentry);
                        
                        globalPatient.gender = p.gender;
                    }
                    else if (p.gender != "" && p.gender != globalPatient.gender && globalPatient.gender != "" && globalPatient.gender != null)
                    {
                        string logentry = "Gender changed from " + globalPatient.gender + " to: " + p.gender;
                        addToLogListBox(logentry);
                        
                        globalPatient.gender = p.gender;
                    }
                }
                //last 4
                if (p.SSN != null)
                {
                    if (p.SSN == "" && globalPatient.SSN == null)
                    {
                        //do nothing
                    }
                    else if (p.SSN == "" && globalPatient.SSN != "")
                    {
                        //this means the first name was removed
                        string logentry = "SSN removed: " + globalPatient.SSN;
                        addToLogListBox(logentry);
                        
                        globalPatient.SSN = null;
                    }
                    else if (p.SSN != "" && globalPatient.SSN == null)
                    {
                        string logentry = "SSN added: " + p.SSN;
                        addToLogListBox(logentry);
                        
                        globalPatient.SSN = p.SSN;
                    }
                    else if (p.SSN != "" && p.SSN != globalPatient.SSN && globalPatient.SSN != "" && globalPatient.SSN != null)
                    {
                        string logentry = "SSN changed from " + globalPatient.SSN + " to: " + p.SSN;
                        addToLogListBox(logentry);
                        
                        globalPatient.SSN = p.SSN;
                    }
                }
                //service
                if (p.service != null)
                {
                    if (p.service == "" && globalPatient.service == null)
                    {
                        //do nothing
                    }
                    else if (p.service == "" && globalPatient.service != "")
                    {
                        //this means the first name was removed
                        string logentry = "Service Name removed: " + globalPatient.service;
                        addToLogListBox(logentry);
                        
                        globalPatient.service = null;
                    }
                    else if (p.service != "" && globalPatient.service == null)
                    {
                        string logentry = "Service Name added: " + p.service;
                        addToLogListBox(logentry);
                        
                        globalPatient.service = p.service;
                    }
                    else if (p.service != "" && p.service != globalPatient.service && globalPatient.service != "" && globalPatient.service != null)
                    {
                        string logentry = "Service Name changed from " + globalPatient.service + " to: " + p.service;
                        addToLogListBox(logentry);
                        
                        globalPatient.service = p.service;
                    }
                }
                //unit
                if (p.unitName != null)
                {
                    if (p.unitName == "" && globalPatient.unitName == null)
                    {
                        //do nothing
                    }
                    else if (p.unitName == "" && globalPatient.unitName != "")
                    {
                        //this means the first name was removed
                        string logentry = "Unit Name removed: " + globalPatient.unitName;
                        addToLogListBox(logentry);
                        
                        globalPatient.unitName = null;
                    }
                    else if (p.unitName != "" && globalPatient.unitName == null)
                    {
                        string logentry = "Unit Name added: " + p.unitName;
                        addToLogListBox(logentry);
                        
                        globalPatient.unitName = p.unitName;
                    }
                    else if (p.unitName != "" && p.unitName != globalPatient.unitName && globalPatient.unitName != "" && globalPatient.unitName != null)
                    {
                        string logentry = "Unit Name changed from " + globalPatient.unitName + " to: " + p.unitName;
                        addToLogListBox(logentry);
                        
                        globalPatient.unitName = p.unitName;
                    }
                }
                //weight
                if (p.weight != null)
                {
                    if (p.weight == "" && globalPatient.weight == null)
                    {
                        //do nothing
                    }
                    else if (p.weight == globalPatient.weight)
                    {
                        //do nothing again
                    }
                    else if (p.weight == "" && globalPatient.weight != "")
                    {
                        //this means the first name was removed
                        string logentry = "Weight removed: " + globalPatient.weight + p.weightType;
                        addToLogListBox(logentry);

                        globalPatient.weight = null;
                    }
                    else if (p.weight != "" && globalPatient.weight == null)
                    {
                        string logentry = "Weight added: " + p.weight + p.weightType;
                        addToLogListBox(logentry);

                        globalPatient.weight = p.weight;
                    }
                    else if (p.weight != "" && p.weight != globalPatient.weight && globalPatient.weight != "" && globalPatient.weight != null)
                    {
                        string logentry = "Weight changed from " + globalPatient.weight + " to: " + p.weight + p.weightType;
                        addToLogListBox(logentry);

                        globalPatient.weight = p.weight;
                    }
                }
                //date
                if (p.recordDate != null)
                {
                    if (p.recordDate == "" && globalPatient.recordDate == null)
                    {
                        //do nothing
                    }
                    else if (p.recordDate == "" && globalPatient.recordDate != "")
                    {
                        //this means the first name was removed
                        string logentry = "Record Date removed: " + globalPatient.recordDate;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordDate = null;
                    }
                    else if (p.recordDate != "" && globalPatient.recordDate == null)
                    {
                        string logentry = "Record Date added: " + p.recordDate;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordDate = p.recordDate;
                    }
                    else if (p.recordDate != "" && p.recordDate != globalPatient.recordDate && globalPatient.recordDate != "" && globalPatient.recordDate != null)
                    {
                        string logentry = "Record Date changed from " + globalPatient.recordDate + " to: " + p.recordDate;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordDate = p.recordDate;
                    }
                }
                //time
                if (p.recordTime != null)
                {
                    if (p.recordTime == "" && globalPatient.recordTime == null)
                    {
                        //do nothing
                    }
                    else if (p.recordTime == "" && globalPatient.recordTime != "")
                    {
                        //this means the first name was removed
                        string logentry = "Record Time removed: " + globalPatient.recordTime;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordTime = null;
                    }
                    else if (p.recordTime != "" && globalPatient.recordTime == null)
                    {
                        string logentry = "Record Time added: " + p.recordTime;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordTime = p.recordTime;
                    }
                    else if (p.recordTime != "" && p.recordTime != globalPatient.recordTime && globalPatient.recordTime != "" && globalPatient.recordTime != null)
                    {
                        string logentry = "Record Time changed from " + globalPatient.recordTime + " to: " + p.recordTime;
                        addToLogListBox(logentry);
                        
                        globalPatient.recordTime = p.recordTime;
                    }
                }

                //Handle Allergies Log
                //
                //
                //
                if (p.Allergies.Count > 0)
                {
                    //there should only be one but lets worry about that later
                    foreach (patient.Allergy allergy in p.Allergies)
                    {
                        if (allergy.delete)
                        {
                            string logentry = "Allergy removed: " + allergy.type;
                            addToLogListBox(logentry);
                            
                        }
                        else
                        {
                            string logentry = "Allergy added: " + allergy.type;
                            addToLogListBox(logentry);
                            
                        }
                    }
                }


                //Handle the vitals data
                
                //HR
                if (p.HR != null)
                {
                    if (p.HR != "")
                    {
                        string logentry = "HR added: " + p.HR;
                        addToLogListBox(logentry);
                    }
                }

                //Resp
                if (p.Resp != null)
                {
                    if (p.Resp != "")
                    {
                        string logentry = "Respiratory added: " + p.Resp;
                        addToLogListBox(logentry);
                    }
                }

                //SP02
                if (p.SP02 != null)
                {
                    if (p.SP02!= "")
                    {
                        string logentry = "SP02 added: " + p.SP02;
                        addToLogListBox(logentry);
                    }
                }

                //Temp
                if (p.Temp != null)
                {
                    if (p.Temp != "")
                    {
                        string logentry = "Temperature added: " + p.Temp;
                        if (p.TempType != null)
                        {
                            logentry += p.TempType;
                        }
                        addToLogListBox(logentry);
                    }
                }

                //AVPU
                if (p.AVPU != null)
                {
                    if (p.AVPU != "")
                    {
                        string logentry = "AVPU added: " + p.AVPU;
                        addToLogListBox(logentry);
                    }
                }

                //Pain scale
                if (p.painScale != null)
                {
                    if (p.painScale != "")
                    {
                        string logentry = "Pain Scale added: " + p.painScale;
                        addToLogListBox(logentry);
                    }
                }

                //Blood pressure
                if (p.BPSYS != null)
                {
                    if (p.BPDIA != null)
                    {
                        if (p.BPSYS != "")
                        {
                            if (p.BPDIA != "")
                            {
                                string logentry = "Blood Pressure: " + p.BPSYS + "/" + p.BPDIA;
                                addToLogListBox(logentry);
                            }
                        }
                    }
                }
            }

            //do log info for info from database
            else if (p.fromDatabase)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    try
                    {
                        //Handle injury adding
                        //
                        //
                        //
                        if (p.injuries.Count > 0)
                        {
                            foreach (patient.Injuries i in p.injuries)
                            {
                                //if it's a sneaky patient data do nothing
                                if (i.Type == null || i.Type == "")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    //if this is a database operation then this is easy
                                    if (p.DBOperation)
                                    {
                                        if (i.delete)
                                        {
                                            string logentry = "From DB: Injury deleted: " + i.Type + " at " + i.Location;
                                            addToLogListBox(logentry);
                                            
                                        }
                                        else
                                        {
                                            string logentry = "From DB: Injury added: " + i.Type + " at " + i.Location;
                                            addToLogListBox(logentry);
                                            
                                        }
                                    }
                                }
                            }
                        }

                        //Handle Admin data adding
                        //
                        //
                        //
                        //first name
                        if (p.firstName != null)
                        {
                            //if first name is just blank
                            if (p.firstName == "" && globalPatient.firstName == null)
                            {
                                //do nothing
                            }
                            else if (p.firstName == globalPatient.firstName)
                            {
                                //do nothing again
                            }
                            else if (p.firstName == "" && globalPatient.firstName != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: First Name removed: " + globalPatient.firstName;
                                addToLogListBox(logentry);
                                
                                globalPatient.firstName = null;
                            }
                            else if (p.firstName != "" && globalPatient.firstName == null)
                            {
                                string logentry = "From DB: First Name added: " + p.firstName;
                                addToLogListBox(logentry);
                                
                                globalPatient.firstName = p.firstName;
                            }
                            else if (p.firstName != "" && p.firstName != globalPatient.firstName && globalPatient.firstName != "" && globalPatient.firstName != null)
                            {
                                string logentry = "From DB: First Name changed from " + globalPatient.firstName + " to: " + p.firstName;
                                addToLogListBox(logentry);
                                
                                globalPatient.firstName = p.firstName;
                            }
                        }
                        //middle name
                        if (p.middleName != null)
                        {
                            if (p.middleName == "" && globalPatient.middleName == null)
                            {
                                //do nothing
                            }
                            else if (p.middleName == globalPatient.middleName)
                            {
                                //do nothing again
                            }
                            else if (p.middleName == "" && globalPatient.middleName != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Middle Name removed: " + globalPatient.middleName;
                                addToLogListBox(logentry);
                                
                                globalPatient.middleName = null;
                            }
                            else if (p.middleName != "" && globalPatient.middleName == null)
                            {
                                string logentry = "From DB: Middle Name added: " + p.middleName;
                                addToLogListBox(logentry);
                                
                                globalPatient.middleName = p.middleName;
                            }
                            else if (p.middleName != "" && p.middleName != globalPatient.middleName && globalPatient.middleName != "" && globalPatient.middleName != null)
                            {
                                string logentry = "From DB: Middle Name changed from " + globalPatient.middleName + " to: " + p.middleName;
                                addToLogListBox(logentry);
                                
                                globalPatient.middleName = p.middleName;
                            }
                        }
                        //last name
                        if (p.lastName != null)
                        {
                            if (p.lastName == "" && globalPatient.lastName == null)
                            {
                                //do nothing
                            }
                            else if (p.lastName == globalPatient.lastName)
                            {
                                //do nothing again
                            }
                            else if (p.lastName == "" && globalPatient.lastName != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Last Name removed: " + globalPatient.lastName;
                                addToLogListBox(logentry);
                                
                                globalPatient.lastName = null;
                            }
                            else if (p.lastName != "" && globalPatient.lastName == null)
                            {
                                string logentry = "From DB: Last Name added: " + p.lastName;
                                addToLogListBox(logentry);
                                
                                globalPatient.lastName = p.lastName;
                            }
                            else if (p.lastName != "" && p.lastName != globalPatient.lastName && globalPatient.lastName != "" && globalPatient.lastName != null)
                            {
                                string logentry = "From DB: Last Name changed from " + globalPatient.lastName + " to: " + p.lastName;
                                addToLogListBox(logentry);
                                
                                globalPatient.lastName = p.lastName;
                            }
                        }
                        //gender
                        if (p.gender != null)
                        {
                            if (p.gender == "" && globalPatient.gender == null)
                            {
                                //do nothing
                            }
                            else if (p.gender == globalPatient.gender)
                            {
                                //do nothing again
                            }
                            else if (p.gender == "" && globalPatient.gender != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Gender removed: " + globalPatient.gender;
                                addToLogListBox(logentry);
                                
                                globalPatient.gender = null;
                            }
                            else if (p.gender != "" && globalPatient.gender == null)
                            {
                                string logentry = "From DB: Gender added: " + p.gender;
                                addToLogListBox(logentry);
                                
                                globalPatient.gender = p.gender;
                            }
                            else if (p.gender != "" && p.gender != globalPatient.gender && globalPatient.gender != "" && globalPatient.gender != null)
                            {
                                string logentry = "From DB: Gender changed from " + globalPatient.gender + " to: " + p.gender;
                                addToLogListBox(logentry);
                                
                                globalPatient.gender = p.gender;
                            }
                        }
                        //last 4
                        if (p.SSN != null)
                        {
                            if (p.SSN == "" && globalPatient.SSN == null)
                            {
                                //do nothing
                            }
                            else if (p.SSN == globalPatient.SSN)
                            {
                                //do nothing again
                            }
                            else if (p.SSN == "" && globalPatient.SSN != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: SSN removed: " + globalPatient.SSN;
                                addToLogListBox(logentry);
                                
                                globalPatient.SSN = null;
                            }
                            else if (p.SSN != "" && globalPatient.SSN == null)
                            {
                                string logentry = "From DB: SSN added: " + p.SSN;
                                addToLogListBox(logentry);
                                
                                globalPatient.SSN = p.SSN;
                            }
                            else if (p.SSN != "" && p.SSN != globalPatient.SSN && globalPatient.SSN != "" && globalPatient.SSN != null)
                            {
                                string logentry = "From DB: SSN changed from " + globalPatient.SSN + " to: " + p.SSN;
                                addToLogListBox(logentry);
                                
                                globalPatient.SSN = p.SSN;
                            }
                        }
                        //service
                        if (p.service != null)
                        {
                            if (p.service == "" && globalPatient.service == null)
                            {
                                //do nothing
                            }
                            else if (p.service == globalPatient.service)
                            {
                                //do nothing again
                            }
                            else if (p.service == "" && globalPatient.service != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Service Name removed: " + globalPatient.service;
                                addToLogListBox(logentry);
                                
                                globalPatient.service = null;
                            }
                            else if (p.service != "" && globalPatient.service == null)
                            {
                                string logentry = "From DB: Service Name added: " + p.service;
                                addToLogListBox(logentry);
                                
                                globalPatient.service = p.service;
                            }
                            else if (p.service != "" && p.service != globalPatient.service && globalPatient.service != "" && globalPatient.service != null)
                            {
                                string logentry = "From DB: Service Name changed from " + globalPatient.service + " to: " + p.service;
                                addToLogListBox(logentry);
                                
                                globalPatient.service = p.service;
                            }
                        }
                        //unit
                        if (p.unitName != null)
                        {
                            if (p.unitName == "" && globalPatient.unitName == null)
                            {
                                //do nothing
                            }
                            else if (p.unitName == globalPatient.unitName)
                            {
                                //do nothing again
                            }
                            else if (p.unitName == "" && globalPatient.unitName != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Unit Name removed: " + globalPatient.unitName;
                                addToLogListBox(logentry);
                                
                                globalPatient.unitName = null;
                            }
                            else if (p.unitName != "" && globalPatient.unitName == null)
                            {
                                string logentry = "From DB: Unit Name added: " + p.unitName;
                                addToLogListBox(logentry);
                                
                                globalPatient.unitName = p.unitName;
                            }
                            else if (p.unitName != "" && p.unitName != globalPatient.unitName && globalPatient.unitName != "" && globalPatient.unitName != null)
                            {
                                string logentry = "From DB: Unit Name changed from " + globalPatient.unitName + " to: " + p.unitName;
                                addToLogListBox(logentry);
                                
                                globalPatient.unitName = p.unitName;
                            }
                        }
                        //weight
                        if (p.weight != null)
                        {
                            if (p.weight == "" && globalPatient.weight == null)
                            {
                                //do nothing
                            }
                            else if (p.weight == globalPatient.weight)
                            {
                                //do nothing again
                            }
                            else if (p.weight == "" && globalPatient.weight != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Weight removed: " + globalPatient.weight + p.weightType;
                                addToLogListBox(logentry);

                                globalPatient.weight = null;
                            }
                            else if (p.weight != "" && globalPatient.weight == null)
                            {
                                string logentry = "From DB: Weight added: " + p.weight + p.weightType;
                                addToLogListBox(logentry);

                                globalPatient.weight = p.weight;
                            }
                            else if (p.weight != "" && p.weight != globalPatient.weight && globalPatient.weight != "" && globalPatient.weight != null)
                            {
                                string logentry = "From DB: Weight changed from " + globalPatient.weight + " to: " + p.weight + p.weightType;
                                addToLogListBox(logentry);

                                globalPatient.weight = p.weight;
                            }
                        }
                        //date
                        if (p.recordDate != null)
                        {
                            if (p.recordDate == "" && globalPatient.recordDate == null)
                            {
                                //do nothing
                            }
                            else if (p.recordDate == globalPatient.recordDate)
                            {
                                //do nothing again
                            }
                            else if (p.recordDate == "" && globalPatient.recordDate != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Record Date removed: " + globalPatient.recordDate;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordDate = null;
                            }
                            else if (p.recordDate != "" && globalPatient.recordDate == null)
                            {
                                string logentry = "From DB: Record Date added: " + p.recordDate;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordDate = p.recordDate;
                            }
                            else if (p.recordDate != "" && p.recordDate != globalPatient.recordDate && globalPatient.recordDate != "" && globalPatient.recordDate != null)
                            {
                                string logentry = "From DB: Record Date changed from " + globalPatient.recordDate + " to: " + p.recordDate;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordDate = p.recordDate;
                            }
                        }
                        //time
                        if (p.recordTime != null)
                        {
                            if (p.recordTime == "" && globalPatient.recordTime == null)
                            {
                                //do nothing
                            }
                            else if (p.recordTime == globalPatient.recordTime)
                            {
                                //do nothing again
                            }
                            else if (p.recordTime == "" && globalPatient.recordTime != "")
                            {
                                //this means the first name was removed
                                string logentry = "From DB: Record Time removed: " + globalPatient.recordTime;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordTime = null;
                            }
                            else if (p.recordTime != "" && globalPatient.recordTime == null)
                            {
                                string logentry = "From DB: Record Time added: " + p.recordTime;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordTime = p.recordTime;
                            }
                            else if (p.recordTime != "" && p.recordTime != globalPatient.recordTime && globalPatient.recordTime != "" && globalPatient.recordTime != null)
                            {
                                string logentry = "From DB: Record Time changed from " + globalPatient.recordTime + " to: " + p.recordTime;
                                addToLogListBox(logentry);
                                
                                globalPatient.recordTime = p.recordTime;
                            }
                        }

                        //Handle Allergies Log
                        //
                        //
                        //
                        if (p.Allergies.Count > 0)
                        {
                            //there should only be one but lets worry about that later
                            foreach (patient.Allergy allergy in p.Allergies)
                            {
                                //find out if this allergy has already been recorded
                                try
                                {
                                    var itemToRemove = globalPatient.Allergies.Single(a => a.type == allergy.type);
                                }
                                catch
                                { 
                                    if (allergy.delete)
                                    {
                                        string logentry = "From DB: Allergy removed: " + allergy.type;
                                        addToLogListBox(logentry);
                                        
                                    }
                                    else
                                    {
                                        string logentry = "From DB: Allergy added: " + allergy.type;
                                        addToLogListBox(logentry);
                                        
                                        globalPatient.Allergies.Add(allergy);
                                    }
                                }
                            }
                        }

                        //Handle new vitals data
                        //HR
                if (p.HR != null)
                {
                    if (p.HR != "")
                    {
                        string logentry = "From DB: HR added: " + p.HR;
                        addToLogListBox(logentry);
                    }
                }

                //Resp
                if (p.Resp != null)
                {
                    if (p.Resp != "")
                    {
                        string logentry = "From DB: Respiratory added: " + p.Resp;
                        addToLogListBox(logentry);
                    }
                }

                //SP02
                if (p.SP02 != null)
                {
                    if (p.SP02!= "")
                    {
                        string logentry = "From DB: SP02 added: " + p.SP02;
                        addToLogListBox(logentry);
                    }
                }

                //Temp
                if (p.Temp != null)
                {
                    if (p.Temp != "")
                    {
                        string logentry = "From DB: Temperature added: " + p.Temp;
                        if (p.TempType != null)
                        {
                            logentry += p.TempType;
                        }
                        addToLogListBox(logentry);
                    }
                }

                //AVPU
                if (p.AVPU != null)
                {
                    if (p.AVPU != "")
                    {
                        string logentry = "From DB: AVPU added: " + p.AVPU;
                        addToLogListBox(logentry);
                    }
                }

                //Pain scale
                if (p.painScale != null)
                {
                    if (p.painScale != "")
                    {
                        string logentry = "From DB: Pain Scale added: " + p.painScale;
                        addToLogListBox(logentry);
                    }
                }

                //Blood pressure
                if (p.BPSYS != null)
                {
                    if (p.BPDIA != null)
                    {
                        if (p.BPSYS != "")
                        {
                            if (p.BPDIA != "")
                            {
                                string logentry = "From DB: Blood Pressure: " + p.BPSYS + "/" + p.BPDIA;
                                addToLogListBox(logentry);
                            }
                        }
                    }
                }
                    }
                    catch { }
                }));
            }
        }
    
        private void addToLogListBox(string logentry)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (patientLog log in allLists)
                {
                    if (log.patID == currentId)
                    {
                        log.log.Insert(0, logentry);
                        logListBox.ItemsSource = log.log.ToList();
                    }
                }
            }));
            //logList.Insert(0, logentry);
            //logListBox.ItemsSource = logList.ToList();
        }

        public class patientLog
        {
            public string patID;
            public List<string> log = new List<string>();
        }

        public bool parsePatientIDList(patientListFullClass list)
        {
            if (list.total_rows >= 0)
            {
                for (int i = 0; i < list.total_rows; i++)
                {
                    bool alreadyHere = false;
                    foreach (patientLog log in allLists)
                    {
                        if (log.patID == list.rows[i].value.ID.ToString())
                        {
                            alreadyHere = true;
                            break;
                        }
                    }
                    
                    if (!alreadyHere)
                    {
                        patientLog newLog = new patientLog();
                        newLog.patID = list.rows[i].value.ID.ToString();
                        allLists.Add(newLog);
                    }
                }
                return true;

            }
            return false;
        }


    }
}

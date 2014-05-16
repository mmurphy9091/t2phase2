using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace MEDICS2014.MQTT
{
    class DBHandler
    {
        SystemMessages _systemMessages = SystemMessages.Instance;
        Messages _messages = Messages.Instance;

        string currentPatID;

        //create a global patient to handle any duplication of data
        patient globalPatient = new patient();
        
        public void startDBHandler()
        {
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
            if (message.Contains("PATID:"))
            {
                message = message.Replace("PATID:", "");
                currentPatID = message;
                //MessageBox.Show(currentPatID);
            }
            else if (message.Contains("NEWPATIENT:"))
            {
                message = message.Replace("NEWPATIENT:", "");
                currentPatID = message;
                //MessageBox.Show(currentPatID);
            }
                switch (message)
                {
                    case "CLEAR CONTROL":
                        //might not need anything here
                        break;
                    case "DELETE":
                        string deleteMessage = "DELETE:" + currentPatID;
                        _messages.AddMessage(deleteMessage);
                        break;
                }
        }

        private void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                handleControllerPatientData(Message);
            }

        }

        private void savePatientData(patient p)
        {
            //other treatments
            globalPatient.OtherTreatments = p.OtherTreatments;
        }

        private void handleControllerPatientData(patient p)
        {
            if (p.fromDatabase)
            {
                savePatientData(p);
            }
            if (p.DBOperation)
            {
                //Assign new patID if not assigned yet
                if (currentPatID == null)
                {
                    currentPatID = Guid.NewGuid().ToString();
                    _messages.AddMessage("NEWPATIENT:" + currentPatID);
                }

                //Admin add
                try
                {
                    bool stuffHere = false;

                    //Set up the string
                    StringBuilder dataAdmin = new StringBuilder();
                    dataAdmin.Append("DBPOST:{\"ID\":\"");
                    dataAdmin.Append(currentPatID);
                    dataAdmin.Append("\",\"Admin\":{");

                    //first name
                    if (p.firstName != null)
                    {
                        //this is the first, so no comma
                        dataAdmin.Append("\"FirstName\":\"");
                        dataAdmin.Append(p.firstName.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //middle name
                    if (p.middleName != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"MiddleInitial\":\"");
                        dataAdmin.Append(p.middleName.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //last name
                    if (p.lastName != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"LastName\":\"");
                        dataAdmin.Append(p.lastName.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //gender
                    if (p.gender != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Gender\":\"");
                        dataAdmin.Append(p.gender.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //last 4
                    if (p.SSN != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Last4\":\"");
                        dataAdmin.Append(p.SSN.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //service
                    if (p.service != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Service\":\"");
                        dataAdmin.Append(p.service.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //unit
                    if (p.unitName != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Unit\":\"");
                        dataAdmin.Append(p.unitName.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //weight
                    if (p.weight != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Weight\":\"");
                        dataAdmin.Append(p.weight.ToString());
                        dataAdmin.Append("\",\"WeightType\":\"");
                        dataAdmin.Append(p.weightType.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //date
                    if (p.recordDate != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"Date\":\"");
                        dataAdmin.Append(p.recordDate.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }
                    //time
                    if (p.recordTime != null)
                    {
                        //put in test to see if this is the first bit of data
                        if (stuffHere)
                        {
                            //add a comma before the new data
                            dataAdmin.Append(",");
                        }
                        dataAdmin.Append("\"RecordTime\":\"");
                        dataAdmin.Append(p.recordTime.ToString());
                        dataAdmin.Append("\"");
                        stuffHere = true;
                    }

                    if (stuffHere)
                    {
                        //finish off the string and send the data
                        dataAdmin.Append("}}");
                        _messages.AddMessage(dataAdmin.ToString());
                    }
                }
                catch (Exception e) { if (e.Source != null)
                            Console.WriteLine("Exception source: {0}", e.Source); }

                //Alllergy Add
                //Here we are just going to send the allergy and they will update the table if they need to be deleted
                //structure:  {"ID":"1234567890","Allergy":{"Type":"Vaccines","Delete":"false"}}
                if (p.Allergies.Count > 0)
                {
                    try
                    {
                        //there should be only one but we will worry about that later
                        foreach (patient.Allergy allergy in p.Allergies)
                        {
                            StringBuilder allergyData = new StringBuilder();
                            allergyData.Append("DBPOST:{\"ID\":\"");
                            allergyData.Append(currentPatID);
                            allergyData.Append("\",\"Allergy\":{\"Type\":\"");
                            allergyData.Append(allergy.type.ToString());
                            allergyData.Append("\",\"delete\":\"");
                            allergyData.Append(allergy.delete.ToString());
                            allergyData.Append("\"}}");
                            _messages.AddMessage(allergyData.ToString());
                        }
                    }
                        catch (Exception e) { if (e.Source != null)
                            Console.WriteLine("Exception source: {0}", e.Source); }

                }

                //Injury add
                if (p.injuries.Count > 0)
                {
                    //Make sure this isn't some trickery
                    if (p.injuries[0].Type != null && p.injuries[0].Type != "" &&
                        p.injuries[0].Location != null && p.injuries[0].Location != "")
                    {
                        try
                        {
                            foreach (patient.Injuries i in p.injuries)
                            {
                                i.deleteString = i.delete.ToString();
                                string jsonInj = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Injury\":" + JsonConvert.SerializeObject(i) + "}";
                                _messages.AddMessage(jsonInj);

                                /*
                                    //create and send in this format{"ID":"1234567890","Injury":{"Type":"GSW","Location":"postNeck","delete":"false"}}
                                    StringBuilder data = new StringBuilder();
                                    data.Append("DBPOST:{\"ID\":\"");
                                    data.Append(p.patID);
                                    data.Append("\",\"Injury\":{\"Type\":\"");
                                    data.Append(i.Type);
                                    data.Append("\",\"Location\":\"");
                                    data.Append(i.Location);
                                    data.Append("\",\"delete\":\"");
                                    data.Append(i.delete.ToString());
                                    data.Append("\"}}");
                                    _messages.AddMessage(data.ToString());
                                */
                                    //add injury to global patient if delete is false
                                    if (!i.delete)
                                    {
                                        globalPatient.injuries.Add(i);
                                    }
                                    else
                                    {
                                        globalPatient.injuries.Remove(i);
                                    }
                                
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.Source != null)
                                Console.WriteLine("Exception source: {0}", e.Source);
                        }
                    }
                }

                //Vitals add
                if (p.HR == "" && p.Resp == "" && p.BPSYS == "" && p.BPDIA == "" && p.Temp == "" && p.SP02 == "")
                {
                    //Do nothing
                }
                else
                {
                    p.bindToVitalsList();
                    string jsonVitals = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Vitals\":" + JsonConvert.SerializeObject(p.Vitals) + "}";
                    _messages.AddMessage(jsonVitals);
                    //MessageBox.Show(json);
                }

                //Treatments add

                //Airway
                if (p.treatments.airway.dbSave)
                {
                    if (p.treatments.airway.CRIC == globalPatient.treatments.airway.CRIC && p.treatments.airway.etTube == globalPatient.treatments.airway.etTube && p.treatments.airway.intact == globalPatient.treatments.airway.intact &&
                         p.treatments.airway.NPA == globalPatient.treatments.airway.NPA && p.treatments.airway.SGA == globalPatient.treatments.airway.SGA)
                    {
                        //do nothing
                    }
                    else
                    {
                        p.treatments.airway.bindStrings();
                        string jsonAirway = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"Airway\":" + JsonConvert.SerializeObject(p.treatments.airway.databaseStrings) + "}}";
                        //MessageBox.Show(jsonAirway);
                        _messages.AddMessage(jsonAirway);

                        //bind all the new data
                        globalPatient.treatments.airway.CRIC = p.treatments.airway.CRIC;
                        globalPatient.treatments.airway.etTube = p.treatments.airway.etTube;
                        globalPatient.treatments.airway.intact = p.treatments.airway.intact;
                        globalPatient.treatments.airway.NPA = p.treatments.airway.NPA;
                        globalPatient.treatments.airway.SGA = p.treatments.airway.SGA;

                    }
                }

                //Breathing
                if (p.treatments.breathing.dbSave)
                {
                    if (globalPatient.treatments.breathing.chestSeal == p.treatments.breathing.chestSeal && globalPatient.treatments.breathing.chestTube == p.treatments.breathing.chestTube && globalPatient.treatments.breathing.needleD == p.treatments.breathing.needleD
                        && globalPatient.treatments.breathing.o2 == p.treatments.breathing.o2)
                    {
                        //do nothing
                    }
                    else
                    {
                        p.treatments.breathing.bindStrings();
                        string jsonBreathing = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"Breathing\":" + JsonConvert.SerializeObject(p.treatments.breathing.databaseStrings) + "}}";
                        //MessageBox.Show(jsonAirway);
                        _messages.AddMessage(jsonBreathing);

                        //bind all the new data
                        globalPatient.treatments.breathing.chestSeal = p.treatments.breathing.chestSeal;
                        globalPatient.treatments.breathing.chestTube = p.treatments.breathing.chestTube;
                        globalPatient.treatments.breathing.needleD = p.treatments.breathing.needleD;
                        globalPatient.treatments.breathing.o2 = p.treatments.breathing.o2;

                    }
                }

                //Circulation
                if (p.treatments.circulation.dbSave)
                {
                    //dressing
                    if (globalPatient.treatments.circulation.dressing.hemostatic == p.treatments.circulation.dressing.hemostatic && globalPatient.treatments.circulation.dressing.other == p.treatments.circulation.dressing.other &&
                        globalPatient.treatments.circulation.dressing.otherType == p.treatments.circulation.dressing.otherType && globalPatient.treatments.circulation.dressing.pressure == p.treatments.circulation.dressing.pressure)
                    {
                        //do nothing
                    }
                    else
                    {
                        p.treatments.circulation.dressing.bindStrings();
                        string jsonDressing = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"Circulation\":{\"dressing\":" + JsonConvert.SerializeObject(p.treatments.circulation.dressing.databaseStrings) + "}}}";
                        //MessageBox.Show(jsonAirway);
                        _messages.AddMessage(jsonDressing);

                        //bind all the new data
                        globalPatient.treatments.circulation.dressing.hemostatic = p.treatments.circulation.dressing.hemostatic;
                        globalPatient.treatments.circulation.dressing.other = p.treatments.circulation.dressing.other;
                        globalPatient.treatments.circulation.dressing.otherType = p.treatments.circulation.dressing.otherType;
                        globalPatient.treatments.circulation.dressing.pressure = p.treatments.circulation.dressing.pressure;

                    }
                    //TQ
                    if (globalPatient.treatments.circulation.TQ.extremity == p.treatments.circulation.TQ.extremity && globalPatient.treatments.circulation.TQ.junctional == p.treatments.circulation.TQ.junctional
                        && globalPatient.treatments.circulation.TQ.truncal == p.treatments.circulation.TQ.truncal)
                    {
                        //do nothing
                    }
                    else
                    {
                        p.treatments.circulation.TQ.bindStrings();
                        string jsonTQ = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"Circulation\":{\"TQ\":" + JsonConvert.SerializeObject(p.treatments.circulation.TQ.databaseStrings) + "}}}";
                        //MessageBox.Show(jsonAirway);
                        _messages.AddMessage(jsonTQ);

                        //bind all the new data
                        globalPatient.treatments.circulation.TQ.extremity = p.treatments.circulation.TQ.extremity;
                        globalPatient.treatments.circulation.TQ.junctional = p.treatments.circulation.TQ.junctional;
                        globalPatient.treatments.circulation.TQ.truncal = p.treatments.circulation.TQ.truncal;

                    }
                }

                //Fluids
                if (fluidsHere(p))
                {
                    string jsonFluids = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"Fluids\":" + JsonConvert.SerializeObject(p.treatments.fluids) + "}}";
                    //MessageBox.Show(jsonFluids);
                    _messages.AddMessage(jsonFluids);
                }

                //Blood Products
                if (bloodProductsHere(p))
                {
                    string jsonBlood = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Treatments\":{\"BloodProducts\":" + JsonConvert.SerializeObject(p.treatments.bloodProducts) + "}}";
                    _messages.AddMessage(jsonBlood);
                }

                //Other treatments
                if (!p.fromVDES && p.OtherTreatments.dbSave)
                {
                    string otherTreat = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"OtherTreatments\":" + JsonConvert.SerializeObject(p.OtherTreatments) + "}";
                    _messages.AddMessage(otherTreat);
                }

                //Medications
                if (p.tempMed.Name != null)
                {
                    string jsonMeds = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Medications\":" + JsonConvert.SerializeObject(p.tempMed) + "}";
                    _messages.AddMessage(jsonMeds);
                    //if the med is an other, this is here to make sure that data doesn't get replicated
                    if (p.tempMed.Other == "True")
                    {
                        string jsonMedsList = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"MedicationsList\":" + JsonConvert.SerializeObject(p.Medications) + "}";
                        _messages.AddMessage(jsonMedsList);
                    }
                }

                //Notes
                if (p.notes != null)
                {
                    if (p.notes != "")
                    {
                        string jsonNotes = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"Notes\":" + JsonConvert.SerializeObject(p.notes) + "}";
                        _messages.AddMessage(jsonNotes);
                    }
                }

                //TQList
                if (p.tqs.Count > 0)
                {
                    string jsonNotes = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"TQList\":" + JsonConvert.SerializeObject(p.tqs) + "}";
                    _messages.AddMessage(jsonNotes);
                }

                //Attached PCD's list
                if (p.PCDupdate)
                {
                    string jsonPCD = "DBPOST:{\"ID\":\"" + currentPatID + "\",\"PatPCDs\":" + JsonConvert.SerializeObject(p.attachedPCDs) + "}";
                    _messages.AddMessage(jsonPCD);
                }
            }
        }

        private bool otherTreatmentsChange(patient p)
        {
            if (globalPatient.OtherTreatments.CombatPillPack != p.OtherTreatments.CombatPillPack)
            {
                globalPatient.OtherTreatments.CombatPillPack = p.OtherTreatments.CombatPillPack;
                return true;
            }
            if (globalPatient.OtherTreatments.EyeShieldL != p.OtherTreatments.EyeShieldL)
            {
                globalPatient.OtherTreatments.EyeShieldL = p.OtherTreatments.EyeShieldL;
                return true;
            }
            if (globalPatient.OtherTreatments.EyeShieldR != p.OtherTreatments.EyeShieldR)
            {
                globalPatient.OtherTreatments.EyeShieldR = p.OtherTreatments.EyeShieldR;
                return true;
            }
            if (globalPatient.OtherTreatments.HypothermiaPrevention != p.OtherTreatments.HypothermiaPrevention)
            {
                globalPatient.OtherTreatments.HypothermiaPrevention = p.OtherTreatments.HypothermiaPrevention;
                return true;
            }
            if (globalPatient.OtherTreatments.Splint != p.OtherTreatments.Splint)
            {
                globalPatient.OtherTreatments.Splint = p.OtherTreatments.Splint;
                return true;
            }

            return false;
        }

        private bool bloodProductsHere(patient p)
        {
            if (p.treatments.bloodProducts.hextend.Dose == null && p.treatments.bloodProducts.hextend.Route == null && p.treatments.bloodProducts.hextend.Time == null &&
                p.treatments.bloodProducts.cryoprecipitate.Dose == null && p.treatments.bloodProducts.cryoprecipitate.Route == null && p.treatments.bloodProducts.cryoprecipitate.Time == null &&
                p.treatments.bloodProducts.packedRedBlood.Dose == null && p.treatments.bloodProducts.packedRedBlood.Route == null && p.treatments.bloodProducts.packedRedBlood.Time == null &&
                p.treatments.bloodProducts.other.Dose == null && p.treatments.bloodProducts.other.Route == null && p.treatments.bloodProducts.other.Time == null && p.treatments.bloodProducts.other.Type == null &&
                p.treatments.bloodProducts.plasmanate.Dose == null && p.treatments.bloodProducts.plasmanate.Route == null && p.treatments.bloodProducts.plasmanate.Time == null &&
                p.treatments.bloodProducts.plateleteRichPlasma.Dose == null && p.treatments.bloodProducts.plateleteRichPlasma.Route == null && p.treatments.bloodProducts.plateleteRichPlasma.Time == null &&
                p.treatments.bloodProducts.serumAlbum.Dose == null && p.treatments.bloodProducts.serumAlbum.Route == null && p.treatments.bloodProducts.serumAlbum.Time == null &&
                p.treatments.bloodProducts.wholeBlood.Dose == null && p.treatments.bloodProducts.wholeBlood.Route == null && p.treatments.bloodProducts.wholeBlood.Time == null &&
                p.treatments.bloodProducts.wholePlasma.Dose == null && p.treatments.bloodProducts.wholePlasma.Route == null && p.treatments.bloodProducts.wholePlasma.Time == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool fluidsHere(patient p)
        {
            if (p.treatments.fluids.normalSaline.Dose == null && p.treatments.fluids.normalSaline.Route == null && p.treatments.fluids.normalSaline.Time == null &&
                p.treatments.fluids.lactactedRingers.Dose == null && p.treatments.fluids.lactactedRingers.Route == null && p.treatments.fluids.lactactedRingers.Time == null &&
                p.treatments.fluids.dextrose.Dose == null && p.treatments.fluids.dextrose.Percentage == null && p.treatments.fluids.dextrose.Route == null && p.treatments.fluids.dextrose.Time == null &&
                p.treatments.fluids.other.Dose == null && p.treatments.fluids.other.Route == null && p.treatments.fluids.other.Time == null && p.treatments.fluids.other.Type == null)
            {
                return false; 
            }
            else
            {
                return true;
            }
        }
    }
}

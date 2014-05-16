using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace MEDICS2014.dbJsonInterface
{
    class dbJsonParser
    {
        public patient parseJSON(string dbMessage)
        {
            patient p = new patient();


            
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();

            //If the JSON holds the entire patient data, get and parse it here
            try
            {
               // fullPatientResponseClass fullPatient =
               //   (fullPatientResponseClass)json_serializer.DeserializeObject(dbMessage);
                fullPatientResponseClass fullPatient = JsonConvert.DeserializeObject<fullPatientResponseClass>(dbMessage);
                return parseFullpatientData(fullPatient);
                
            }
           catch(Exception e)
            {
                Console.Write(e);
            }

            /*
            DataContractJsonSerializer jsonFullPatient = new DataContractJsonSerializer(typeof(fullPatientResponse));
            fullPatientResponse fullPatient = (fullPatientResponse)jsonFullPatient.ReadObject(data);
            object objResponse = jsonFullPatient.ReadObject(dbMessage);
            TrendResponse TRENDRESPONSE = objResponse as TrendResponse;
            */

            return p;
        }

        public patient parseFullpatientData(fullPatientResponseClass data)
        {
            patient temp = new patient();

            //Get the id
            temp.patID = data.rows[0].value._id.ToString();

            //Grab the admin information
            temp.firstName = data.rows[0].value.Admin.FirstName.ToString();
            temp.middleName = data.rows[0].value.Admin.MiddleInitial.ToString();
            temp.lastName = data.rows[0].value.Admin.LastName.ToString();
            temp.unitName = data.rows[0].value.Admin.Unit.ToString();
            temp.service = data.rows[0].value.Admin.Service.ToString();
            temp.recordDate = data.rows[0].value.Admin.Date.ToString();
            temp.recordTime = data.rows[0].value.Admin.RecordTime.ToString();
            temp.SSN = data.rows[0].value.Admin.Last4.ToString();
            temp.gender = data.rows[0].value.Admin.Gender.ToString();
            temp.weight = data.rows[0].value.Admin.Weight.ToString();
            temp.weightType = data.rows[0].value.Admin.WeightType.ToString();

            //Grab the allergies
            foreach (Allergy a in data.rows[0].value.Allergies)
            {
                patient.Allergy allergy = new patient.Allergy();
                allergy.type = a.Type.ToString();
                temp.Allergies.Add(allergy);
            }

            //Grab the injury information
            foreach (Injury i in data.rows[0].value.InjuryList.Injuries)
            {
                patient.Injuries tempI = new patient.Injuries();
                tempI.Location = i.Location.ToString();
                tempI.Type = i.Type.ToString();
                temp.injuries.Add(tempI);
            }

            //Grab the lastest vitals information
            if (data.rows[0].value.Vitals.Count > 0)
            {
                int latest = data.rows[0].value.Vitals.Count - 1;
                temp.HR = data.rows[0].value.Vitals[latest].HR;
                temp.Resp = data.rows[0].value.Vitals[latest].Resp;
                temp.SP02 = data.rows[0].value.Vitals[latest].SP02;
                temp.AVPU = data.rows[0].value.Vitals[latest].AVPU;
                temp.painScale = data.rows[0].value.Vitals[latest].painScale;
                temp.Temp = data.rows[0].value.Vitals[latest].Temp;
                temp.TempType = data.rows[0].value.Vitals[latest].TempType;
                temp.vitalsTime = data.rows[0].value.Vitals[latest].vitalsTime;
                temp.BPSYS = data.rows[0].value.Vitals[latest].BPSYS;
                temp.BPDIA = data.rows[0].value.Vitals[latest].BPDIA;

                //Bind the entire list now
                for (int i = 0; i < data.rows[0].value.Vitals.Count; i++)
                {
                    patient.VitalsList newVital = new patient.VitalsList();
                    newVital.HR = data.rows[0].value.Vitals[i].HR;
                    newVital.Resp = data.rows[0].value.Vitals[i].Resp;
                    newVital.SP02 = data.rows[0].value.Vitals[i].SP02;
                    newVital.AVPU = data.rows[0].value.Vitals[i].AVPU;
                    newVital.painScale = data.rows[0].value.Vitals[i].painScale;
                    newVital.Temp = data.rows[0].value.Vitals[i].Temp;
                    newVital.TempType = data.rows[0].value.Vitals[i].TempType;
                    newVital.vitalsTime = data.rows[0].value.Vitals[i].vitalsTime;
                    newVital.BPSYS = data.rows[0].value.Vitals[i].BPSYS;
                    newVital.BPDIA = data.rows[0].value.Vitals[i].BPDIA;


                    temp.allVitals.Add(newVital);
                }
            }

            

            //Grab the Treatments Information

            //Airway
            if (data.rows[0].value.Treatments.Airway.CRIC == "True")
            {
                temp.treatments.airway.CRIC = true;
            }
            if (data.rows[0].value.Treatments.Airway.etTube == "True")
            {
                temp.treatments.airway.etTube = true;
            }
            if (data.rows[0].value.Treatments.Airway.intact == "True")
            {
                temp.treatments.airway.intact = true;
            }
            if (data.rows[0].value.Treatments.Airway.NPA == "True")
            {
                temp.treatments.airway.NPA = true;
            }
            if (data.rows[0].value.Treatments.Airway.SGA == "True")
            {
                temp.treatments.airway.SGA = true;
            }

            //Breathing
            if (data.rows[0].value.Treatments.Breathing.chestSeal == "True")
            {
                temp.treatments.breathing.chestSeal = true;
            }
            if (data.rows[0].value.Treatments.Breathing.chestTube == "True")
            {
                temp.treatments.breathing.chestTube = true;
            }
            if (data.rows[0].value.Treatments.Breathing.needleD == "True")
            {
                temp.treatments.breathing.needleD = true;
            }
            if (data.rows[0].value.Treatments.Breathing.o2 == "True")
            {
                temp.treatments.breathing.o2 = true;
            }

            //Circulation
            //TQ
            if (data.rows[0].value.Treatments.Circulation.TQ.extremity == "True")
            {
                temp.treatments.circulation.TQ.extremity = true;
            }
            if (data.rows[0].value.Treatments.Circulation.TQ.junctional == "True")
            {
                temp.treatments.circulation.TQ.junctional = true;
            }
            if (data.rows[0].value.Treatments.Circulation.TQ.truncal == "True")
            {
                temp.treatments.circulation.TQ.truncal = true;
            }
            //Dressing
            if (data.rows[0].value.Treatments.Circulation.dressing.hemostatic == "True")
            {
                temp.treatments.circulation.dressing.hemostatic = true;
            }
            if (data.rows[0].value.Treatments.Circulation.dressing.pressure == "True")
            {
                temp.treatments.circulation.dressing.pressure = true;
            }
            if (data.rows[0].value.Treatments.Circulation.dressing.other == "True")
            {
                temp.treatments.circulation.dressing.other = true;
                temp.treatments.circulation.dressing.otherType = data.rows[0].value.Treatments.Circulation.dressing.otherType;
            }

            //Fluids
            //normalSaline
            temp.treatments.fluids.normalSaline.Dose = data.rows[0].value.Treatments.Fluids.normalSaline.Dose;
            temp.treatments.fluids.normalSaline.Route = data.rows[0].value.Treatments.Fluids.normalSaline.Route;
            temp.treatments.fluids.normalSaline.Time = data.rows[0].value.Treatments.Fluids.normalSaline.Time;
            //lactactedRingers
            temp.treatments.fluids.lactactedRingers.Dose = data.rows[0].value.Treatments.Fluids.lactactedRingers.Dose;
            temp.treatments.fluids.lactactedRingers.Route = data.rows[0].value.Treatments.Fluids.lactactedRingers.Route;
            temp.treatments.fluids.lactactedRingers.Time = data.rows[0].value.Treatments.Fluids.lactactedRingers.Time;
            //dextrose
            temp.treatments.fluids.dextrose.Dose = data.rows[0].value.Treatments.Fluids.dextrose.Dose;
            temp.treatments.fluids.dextrose.Route = data.rows[0].value.Treatments.Fluids.dextrose.Route;
            temp.treatments.fluids.dextrose.Time = data.rows[0].value.Treatments.Fluids.dextrose.Time;
            temp.treatments.fluids.dextrose.Percentage = data.rows[0].value.Treatments.Fluids.dextrose.Percentage;
            //other
            temp.treatments.fluids.other.Dose = data.rows[0].value.Treatments.Fluids.other.Dose;
            temp.treatments.fluids.other.Route = data.rows[0].value.Treatments.Fluids.other.Route;
            temp.treatments.fluids.other.Time = data.rows[0].value.Treatments.Fluids.other.Time;
            temp.treatments.fluids.other.Type = data.rows[0].value.Treatments.Fluids.other.Type;

            //Blood Products
            //Whole Blood
            temp.treatments.bloodProducts.wholeBlood.Dose = data.rows[0].value.Treatments.BloodProducts.wholeBlood.Dose;
            temp.treatments.bloodProducts.wholeBlood.Route = data.rows[0].value.Treatments.BloodProducts.wholeBlood.Route;
            temp.treatments.bloodProducts.wholeBlood.Time = data.rows[0].value.Treatments.BloodProducts.wholeBlood.Time;
            //Packed Red Blood
            temp.treatments.bloodProducts.packedRedBlood.Dose = data.rows[0].value.Treatments.BloodProducts.packedRedBlood.Dose;
            temp.treatments.bloodProducts.packedRedBlood.Route = data.rows[0].value.Treatments.BloodProducts.packedRedBlood.Route;
            temp.treatments.bloodProducts.packedRedBlood.Time = data.rows[0].value.Treatments.BloodProducts.packedRedBlood.Time;
            //Whole Plasma
            temp.treatments.bloodProducts.wholePlasma.Dose = data.rows[0].value.Treatments.BloodProducts.wholePlasma.Dose;
            temp.treatments.bloodProducts.wholePlasma.Route = data.rows[0].value.Treatments.BloodProducts.wholePlasma.Route;
            temp.treatments.bloodProducts.wholePlasma.Time = data.rows[0].value.Treatments.BloodProducts.wholePlasma.Time;
            //Cryoprecipitate Plasma
            temp.treatments.bloodProducts.cryoprecipitate.Dose = data.rows[0].value.Treatments.BloodProducts.cryoprecipitate.Dose;
            temp.treatments.bloodProducts.cryoprecipitate.Route = data.rows[0].value.Treatments.BloodProducts.cryoprecipitate.Route;
            temp.treatments.bloodProducts.cryoprecipitate.Time = data.rows[0].value.Treatments.BloodProducts.cryoprecipitate.Time;
            //Platelete Rich Plasma
            temp.treatments.bloodProducts.plateleteRichPlasma.Dose = data.rows[0].value.Treatments.BloodProducts.plateleteRichPlasma.Dose;
            temp.treatments.bloodProducts.plateleteRichPlasma.Route = data.rows[0].value.Treatments.BloodProducts.plateleteRichPlasma.Route;
            temp.treatments.bloodProducts.plateleteRichPlasma.Time = data.rows[0].value.Treatments.BloodProducts.plateleteRichPlasma.Time;
            //Plasmanate
            temp.treatments.bloodProducts.plasmanate.Dose = data.rows[0].value.Treatments.BloodProducts.plasmanate.Dose;
            temp.treatments.bloodProducts.plasmanate.Route = data.rows[0].value.Treatments.BloodProducts.plasmanate.Route;
            temp.treatments.bloodProducts.plasmanate.Time = data.rows[0].value.Treatments.BloodProducts.plasmanate.Time;
            //Hextend
            temp.treatments.bloodProducts.hextend.Dose = data.rows[0].value.Treatments.BloodProducts.hextend.Dose;
            temp.treatments.bloodProducts.hextend.Route = data.rows[0].value.Treatments.BloodProducts.hextend.Route;
            temp.treatments.bloodProducts.hextend.Time = data.rows[0].value.Treatments.BloodProducts.hextend.Time;
            //Serum Albumin
            temp.treatments.bloodProducts.serumAlbum.Dose = data.rows[0].value.Treatments.BloodProducts.serumAlbum.Dose;
            temp.treatments.bloodProducts.serumAlbum.Route = data.rows[0].value.Treatments.BloodProducts.serumAlbum.Route;
            temp.treatments.bloodProducts.serumAlbum.Time = data.rows[0].value.Treatments.BloodProducts.serumAlbum.Time;
            //other
            temp.treatments.bloodProducts.other.Dose = data.rows[0].value.Treatments.BloodProducts.other.Dose;
            temp.treatments.bloodProducts.other.Route = data.rows[0].value.Treatments.BloodProducts.other.Route;
            temp.treatments.bloodProducts.other.Time = data.rows[0].value.Treatments.BloodProducts.other.Time;
            temp.treatments.bloodProducts.other.Type = data.rows[0].value.Treatments.BloodProducts.other.Type;

            //Medications
            if (data.rows[0].value.Medications.Count > 0)
            {
                foreach (MEDICS2014.dbJsonInterface.Medication med in data.rows[0].value.Medications)
                {
                    patient tMed = new patient();
                    tMed.tempMed.Name = med.Name;
                    tMed.tempMed.Dose = med.Dose;
                    tMed.tempMed.Units = med.Units;
                    tMed.tempMed.Route = med.Route;
                    tMed.tempMed.Time = med.Time;
                    tMed.tempMed.Analgesic = med.Analgesic;
                    tMed.tempMed.Antibiotic = med.Antibiotic;
                    tMed.tempMed.Other = med.Other;

                    temp.Medications.Add(tMed.tempMed);
                }
            }

            //Notes
            if (data.rows[0].value.Notes != null)
            {
                temp.notes = data.rows[0].value.Notes;
            }

            //Other treatments
            temp.OtherTreatments.CombatPillPack = data.rows[0].value.OtherTreatments.CombatPillPack;
            temp.OtherTreatments.EyeShieldL = data.rows[0].value.OtherTreatments.EyeShieldL;
            temp.OtherTreatments.EyeShieldR = data.rows[0].value.OtherTreatments.EyeShieldR;
            temp.OtherTreatments.Splint = data.rows[0].value.OtherTreatments.Splint;
            temp.OtherTreatments.HypothermiaPrevention = data.rows[0].value.OtherTreatments.HypothermiaPrevention;


            //TQList
            if (data.rows[0].value.TQList.Count > 0)
            {
                foreach (MEDICS2014.dbJsonInterface.TQList tq in data.rows[0].value.TQList)
                {
                    patient.TQ newTQ = new patient.TQ();
                    newTQ.Location = tq.Location;
                    newTQ.Time = tq.Time;
                    newTQ.Type = tq.Type;
                    temp.tqs.Add(newTQ);

                }
            }


            //DB PCD List
            if (data.rows[0].value.AllPCDs.Count > 0)
            {
                foreach (MEDICS2014.dbJsonInterface.AllPCD pcd in data.rows[0].value.AllPCDs)
                {
                    temp.dbPCDs.Add(pcd.ID.ToString());
                }
            }

            //Attached PCD List
            if (data.rows[0].value.PatPCDs.Count > 0)
            {
                foreach (MEDICS2014.dbJsonInterface.PatPCD pcd in data.rows[0].value.PatPCDs)
                {
                    temp.attachedPCDs.Add(pcd.ID.ToString());
                }
            }

            temp.fromDatabase = true;

            return temp;
        }

        public class Medication
        {
            public string Antibiotic { get; set; }
            public string Name { get; set; }
            public string Route { get; set; }
            public string Dose { get; set; }
            public string Analgesic { get; set; }
            public string Other { get; set; }
            public string Time { get; set; }
            public string Units { get; set; }
        }
    }

}

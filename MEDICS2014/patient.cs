using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDICS2014
{
    public class patient
    {
        public string patID = "1234567890";

        public string firstName;
        public string middleName;
        public string lastName;
        public string SSN;
        public string DOB;
        public string age;
        public string Rank;
        public string unitName;
        public string unitPhone;
        public string service;
        public string height;
        public string weight;
        public string weightType;
        public string gender;

        public string recordDate;
        public string recordTime;

        public List<Allergy> Allergies = new List<Allergy>();

        public string HR = "";
        public string Resp = "";
        public string BPSYS = "";
        public string BPDIA = "";
        public string Temp = "";
        public string TempType = "F";
        public string SP02 = "";
        public string AVPU = "";
        public string painScale = "";
        public string vitalsTime = "";

        public List<Injuries> injuries = new List<Injuries>();

        public string airway;
        public string breathing;
        public string circulation;
        public string skin;
        public string head;
        public string neck;
        public string chest;
        public string abdomen;
        public string pelvis;
        public string back;

        public string ParklandPercent;

        public bool CRPgiven = false;

        public Treatments treatments = new Treatments();

        public VitalsList Vitals = new VitalsList();

        public List<VitalsList> allVitals = new List<VitalsList>();

        public List<MedicationsDetails> Medications = new List<MedicationsDetails>();

        public MedicationsDetails tempMed = new MedicationsDetails();
        public string medUnits;

        public string notes;

        public TreatmentsOther OtherTreatments = new TreatmentsOther();

        public string vdesAllergy;

        public List<TQ> tqs = new List<TQ>();

        public List<string> dbPCDs = new List<string>();

        public List<string> attachedPCDs = new List<string>();

        public bool PCDupdate = false;
        //public MedicationsAnalgesic Analgesic = new MedicationsAnalgesic();
        //public MedicationsAntibiotic Antibiotic = new MedicationsAntibiotic();
        //Other list
        //public List<MedicationsDetailsOther> other = new List<MedicationsDetailsOther>();


        public class VitalsList
        {
            public string HR = "";
            public string Resp = "";
            public string BPSYS = "";
            public string BPDIA = "";
            public string Temp = "";
            public string TempType = "F";
            public string SP02 = "";
            public string AVPU = "";
            public string painScale = "";
            public string vitalsTime = "";
        }

        public void bindToVitalsList()
        {
            Vitals.HR = HR;
            Vitals.Resp = Resp;
            Vitals.BPSYS = BPSYS;
            Vitals.BPDIA = BPDIA;
            Vitals.Temp = Temp;
            Vitals.TempType = TempType;
            Vitals.SP02 = SP02;
            Vitals.AVPU = AVPU;
            Vitals.painScale = painScale;
            Vitals.vitalsTime = vitalsTime;
        }

        public class Injuries
        {
           public string Type;
           public string Location;
           public bool delete = false;
           public string deleteString;
        }

        public class Allergy
        {
            public string type;
            public bool delete = false;
        }

        public class Treatments
        {
            public Airway airway = new Airway();
            public Breathing breathing = new Breathing();
            public Circulation circulation = new Circulation();
            public Fluids fluids = new Fluids();
            public BloodProducts bloodProducts = new BloodProducts();
        }

        public class Airway
        {
            public bool intact;
            public bool NPA;
            public bool CRIC;
            public bool etTube;
            public bool SGA;
            public bool dbSave = false;
            public dbStrings databaseStrings = new dbStrings();

            public class dbStrings
            {
                public string intact;
                public string NPA;
                public string CRIC;
                public string etTube;
                public string SGA;
            }

            public void bindStrings()
            {
                databaseStrings.intact = intact.ToString();
                databaseStrings.NPA = NPA.ToString();
                databaseStrings.CRIC = CRIC.ToString();
                databaseStrings.etTube = etTube.ToString();
                databaseStrings.SGA = SGA.ToString();
            }
        }

        public class Breathing
        {
            public bool o2;
            public bool needleD;
            public bool chestTube;
            public bool chestSeal;
            public bool dbSave = false;
            public dbStrings databaseStrings = new dbStrings();

            public class dbStrings
            {
                public string o2;
                public string needleD;
                public string chestTube;
                public string chestSeal;
            }

            public void bindStrings()
            {
                databaseStrings.o2 = o2.ToString();
                databaseStrings.needleD = needleD.ToString();
                databaseStrings.chestTube = chestTube.ToString();
                databaseStrings.chestSeal = chestSeal.ToString();
            }
        }

        public class Circulation
        {
            public circulationTQ TQ = new circulationTQ();
            public circulationDressing dressing = new circulationDressing();
            public bool dbSave = false;
        }

        public class circulationTQ
        {
            public bool extremity;
            public bool junctional;
            public bool truncal;
            public dbStrings databaseStrings = new dbStrings();

            public class dbStrings
            {
                public string extremity;
                public string junctional;
                public string truncal;
            }

            public void bindStrings()
            {
                databaseStrings.extremity = extremity.ToString();
                databaseStrings.junctional = junctional.ToString();
                databaseStrings.truncal = truncal.ToString();
            }

        }

        public class circulationDressing
        {
            public bool hemostatic;
            public bool pressure;
            public bool other;
            public string otherType;
            public dbStrings databaseStrings = new dbStrings();

            public class dbStrings
            {
                public string hemostatic;
                public string pressure;
                public string other;
                public string otherType;
            }

            public void bindStrings()
            {
                databaseStrings.hemostatic = hemostatic.ToString();
                databaseStrings.pressure = pressure.ToString();
                databaseStrings.other = other.ToString();
                if (other )
                databaseStrings.otherType = otherType.ToString();
            }

        }

        public class Fluids
        {
            public FluidsDetails normalSaline = new FluidsDetails();
            public FluidsDetails lactactedRingers = new FluidsDetails();
            public FluidsDextroseDetails dextrose = new FluidsDextroseDetails();
            public FluidsOtherDetails other = new FluidsOtherDetails();

        }

        public class FluidsDetails
        {
            public string Dose;
            public string Route;
            public string Time;
        }

        public class FluidsDextroseDetails
        {
            public string Dose;
            public string Route;
            public string Time;
            public string Percentage;
        }

        public class FluidsOtherDetails
        {
            public string Dose;
            public string Route;
            public string Time;
            public string Type;
        }


        public class BloodProducts
        {
            public BloodProductsDetails wholeBlood = new BloodProductsDetails();
            public BloodProductsDetails packedRedBlood = new BloodProductsDetails();
            public BloodProductsDetails plateleteRichPlasma = new BloodProductsDetails();
            public BloodProductsDetails wholePlasma = new BloodProductsDetails();
            public BloodProductsDetails cryoprecipitate = new BloodProductsDetails();
            public BloodProductsDetails serumAlbum = new BloodProductsDetails();
            public BloodProductsDetails plasmanate = new BloodProductsDetails();
            public BloodProductsDetails hextend = new BloodProductsDetails();
            public BloodProductsDetailsOther other = new BloodProductsDetailsOther();
        }

        public class BloodProductsDetails
        {
            public string Dose;
            public string Route;
            public string Time;
        }

        public class BloodProductsDetailsOther
        {
            public string Type;
            public string Dose;
            public string Route;
            public string Time;
        }

        public class MedicationsAnalgesic
        {
            //Analgesic
            public MedicationsDetails Dilaudid = new MedicationsDetails();
            public MedicationsDetails Fentanyl = new MedicationsDetails();
            public MedicationsDetails Hydrocodone = new MedicationsDetails();
            public MedicationsDetails Ibuprofen = new MedicationsDetails();
            public MedicationsDetails Ketamine = new MedicationsDetails();
            public MedicationsDetails Morphine = new MedicationsDetails();
            public MedicationsDetails Oxycodone = new MedicationsDetails();
            public MedicationsDetails Percocet = new MedicationsDetails();
            public MedicationsDetails Toradol = new MedicationsDetails();
            public MedicationsDetails Tylenol = new MedicationsDetails();
            public MedicationsDetails Vicodin = new MedicationsDetails();
            public MedicationsDetailsOther analgesicOther = new MedicationsDetailsOther();

        }

        public class MedicationsAntibiotic
        {
            //Antibiotic
            public MedicationsDetails Amoxicillin = new MedicationsDetails();
            public MedicationsDetails Azithromycin = new MedicationsDetails();
            public MedicationsDetails Cefalexin = new MedicationsDetails();
            public MedicationsDetails Cipro = new MedicationsDetails();
            public MedicationsDetails Clindamycin = new MedicationsDetails();
            public MedicationsDetails Erythromycin = new MedicationsDetails();
            public MedicationsDetails Levaquin = new MedicationsDetails();
            public MedicationsDetails Metronidazole = new MedicationsDetails();
            public MedicationsDetails Neomycin = new MedicationsDetails();
            public MedicationsDetails Penicillin = new MedicationsDetails();
            public MedicationsDetails Septra = new MedicationsDetails();
            public MedicationsDetails Silvadene = new MedicationsDetails();
            public MedicationsDetails Streptomycin = new MedicationsDetails();
            public MedicationsDetails Tetracycline = new MedicationsDetails();
            public MedicationsDetails Vancomycin = new MedicationsDetails();
            public MedicationsDetailsOther antibioticOther = new MedicationsDetailsOther();
        }


        public class MedicationsDetails
        {
            public string Name;
            public string Dose;
            public string Units;
            public string Route;
            public string Time;
            public string Analgesic = "False";
            public string Antibiotic = "False";
            public string Other = "False";
        }

        public class MedicationsDetailsOther
        {
            public string Name;
            public string Dose;
            public string Units;
            public string Route;
            public string Time;
        }

        public class TreatmentsOther
        {
            public string CombatPillPack = "False";
            public string EyeShieldR = "False";
            public string EyeShieldL = "False";
            public string Splint = "False";
            public string HypothermiaPrevention = "False";
            public bool dbSave = false;
        }

        public class TQ
        {
            public string Type;
            public string Time;
            public string Location;
        }

        public bool DBOperation = false;
        public bool fromDatabase = false;
        public bool fromVDES = false;

    }

   
}

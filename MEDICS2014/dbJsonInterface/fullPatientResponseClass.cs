using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDICS2014.dbJsonInterface
{
    public class Medication
    {
        public string Antibiotic { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public string Dose { get; set; }
        public string Units { get; set; }
        public string Other { get; set; }
        public string Time { get; set; }
        public string Analgesic { get; set; }
    }

    public class PatPCD
    {
        public string ID { get; set; }
    }

    public class AllPCD
    {
        public string ID { get; set; }
    }

    public class Admin
    {
        public string RecordTime { get; set; }
        public string FirstName { get; set; }
        public string Service { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Weight { get; set; }
        public string Rank { get; set; }
        public string Last4 { get; set; }
        public string WeightType { get; set; }
        public string Gender { get; set; }
        public string Date { get; set; }
        public string Unit { get; set; }
    }

    public class Allergy
    {
        public string Type { get; set; }
    }

    public class PackedRedBlood
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class SerumAlbum
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class Plasmanate
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class Hextend
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class WholePlasma
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class Other
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Dose { get; set; }
    }

    public class Cryoprecipitate
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class PlateleteRichPlasma
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class WholeBlood
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class BloodProducts
    {
        public PackedRedBlood packedRedBlood { get; set; }
        public SerumAlbum serumAlbum { get; set; }
        public Plasmanate plasmanate { get; set; }
        public Hextend hextend { get; set; }
        public WholePlasma wholePlasma { get; set; }
        public Other other { get; set; }
        public Cryoprecipitate cryoprecipitate { get; set; }
        public PlateleteRichPlasma plateleteRichPlasma { get; set; }
        public WholeBlood wholeBlood { get; set; }
    }

    public class Breathing
    {
        public string chestTube { get; set; }
        public string needleD { get; set; }
        public string o2 { get; set; }
        public string chestSeal { get; set; }
    }

    public class LactactedRingers
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class Dextrose
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Percentage { get; set; }
        public string Dose { get; set; }
    }

    public class Other2
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Dose { get; set; }
    }

    public class NormalSaline
    {
        public string Route { get; set; }
        public string Time { get; set; }
        public string Dose { get; set; }
    }

    public class Fluids
    {
        public LactactedRingers lactactedRingers { get; set; }
        public Dextrose dextrose { get; set; }
        public Other2 other { get; set; }
        public NormalSaline normalSaline { get; set; }
    }

    public class Dressing
    {
        public string otherType { get; set; }
        public string hemostatic { get; set; }
        public string other { get; set; }
        public string pressure { get; set; }
    }

    public class TQ
    {
        public string extremity { get; set; }
        public string junctional { get; set; }
        public string truncal { get; set; }
    }

    public class Circulation
    {
        public Dressing dressing { get; set; }
        public TQ TQ { get; set; }
    }

    public class Airway
    {
        public string SGA { get; set; }
        public string etTube { get; set; }
        public string intact { get; set; }
        public string NPA { get; set; }
        public string CRIC { get; set; }
    }

    public class Treatments
    {
        public BloodProducts BloodProducts { get; set; }
        public Breathing Breathing { get; set; }
        public Fluids Fluids { get; set; }
        public Circulation Circulation { get; set; }
        public Airway Airway { get; set; }
    }

    public class Injury
    {
        public string Type { get; set; }
        public string Location { get; set; }
    }

    public class InjuryList
    {
        public List<Injury> Injuries { get; set; }
    }

    public class OtherTreatments
    {
        public string EyeShieldR { get; set; }
        public string Splint { get; set; }
        public string HypothermiaPrevention { get; set; }
        public string CombatPillPack { get; set; }
        public string EyeShieldL { get; set; }
    }

    public class Vital
    {
        public string vitalsTime { get; set; }
        public string Temp { get; set; }
        public string HR { get; set; }
        public string Resp { get; set; }
        public string painScale { get; set; }
        public string BPSYS { get; set; }
        public string BPDIA { get; set; }
        public string SP02 { get; set; }
        public string TempType { get; set; }
        public string AVPU { get; set; }
    }

    public class TQList
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
    }
    public class Value
    {
        public string _id { get; set; }
        public string _rev { get; set; }
        public List<Medication> Medications { get; set; }
        public List<PatPCD> PatPCDs { get; set; }
        public Admin Admin { get; set; }
        public List<AllPCD> AllPCDs { get; set; }
        public List<Allergy> Allergies { get; set; }
        public Treatments Treatments { get; set; }
        public InjuryList InjuryList { get; set; }
        public OtherTreatments OtherTreatments { get; set; }
        public List<Vital> Vitals { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public List<TQList> TQList { get; set; }
    }

    public class Row
    {
        public string id { get; set; }
        public string key { get; set; }
        public Value value { get; set; }
    }

    public class fullPatientResponseClass
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public List<Row> rows { get; set; }
    }
    
}

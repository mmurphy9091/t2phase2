using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace MEDICS2014.dbJsonInterface
{
    [DataContract]
    public class fullPatientResponse
    {
        [DataMember(Name = "total_rows")]
        public int totalRows { get; set; }
        [DataMember(Name = "offset")]
        public int offset { get; set; }
        [DataMember(Name = "rows")]
        public rows[] rows { get; set; }
    }

    [DataContract]
    public class rows
    {
        [DataMember(Name = "id")]
        public int patID { get; set; }
        [DataMember(Name = "key")]
        public int patKey { get; set; }
        [DataMember(Name = "value")]
        public patientData patientData { get; set; }
    }

    [DataContract]
    public class patientData
    {
        [DataMember(Name = "_id")]
        public int patID { get; set; }
        [DataMember(Name = "_rev")]
        public string revision { get; set; }
        [DataMember(Name = "Type")]
        public string dataType { get; set; }
        [DataMember(Name = "FirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "MiddleInitial")]
        public string MiddleInitial { get; set; }
        [DataMember(Name = "LastName")]
        public string LastName { get; set; }
        public injuryList injuryList { get; set; }
    }

    [DataContract]
    public class injuryList
    {
        [DataMember(Name = "InjuryNum")]
        public int numberOfInjuries { get; set; }
        [DataMember(Name = "Injuries")]
        public Injuries[] Injuries { get; set; }
    }

    [DataContract]
    public class Injuries
    {
        [DataMember(Name = "Type")]
        public string Type { get; set; }
        [DataMember(Name = "Location")]
        public string Location { get; set; }
    }
}

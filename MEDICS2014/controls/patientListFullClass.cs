using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDICS2014.controls
{
    public class Value
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Row
    {
        public string id { get; set; }
        public object key { get; set; }
        public Value value { get; set; }
    }

    public class patientListFullClass
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public List<Row> rows { get; set; }
    }
}

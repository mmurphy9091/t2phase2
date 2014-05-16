using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDICS2014
{
    public class vitalsInfo
    {
        public string heartRate;
        public string respiratory;
        public string bloodPressureSYS;
        public string bloodPressureDIA;
        public string temperature;
        public string EtC02;
        public string time;
        public string AVPU;
        public string painScale;

        public string summary;

        public void setSummary()
        {
            summary = "Time: " + time + "  HR: " + heartRate + "  BP: " + bloodPressureSYS + "/" + bloodPressureDIA + "  Resp: " + respiratory + "  EtC02: " + EtC02 + "  AVPU: " + AVPU +
                "  Pain Scale: " + painScale + "  Temp: " + temperature;
        }
    }
}

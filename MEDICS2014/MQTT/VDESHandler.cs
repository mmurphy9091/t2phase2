using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Json;

namespace MEDICS2014.MQTT
{
    class VDESHandler
    {
        public patient parseJSON(string vdesMessage)
        {
            patient p = new patient();

            //do a contains if statement, then use this algorythm, returns only the value

            //Vitals
            if (vdesMessage.Contains("Vitals"))
            {
                //HR
                if (vdesMessage.Contains("HR"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.HR;
                    p.HR = node2.Value;
                    return p;
                }
                //BP
                if (vdesMessage.Contains("BPSYS"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.BPSYS;
                    p.BPSYS = node2.Value;
                    if (vdesMessage.Contains("BPDIA"))
                    {
                        node2 = value.BPDIA;
                        p.BPDIA = node2.Value;
                    }
                    return p;
                }
                //SP02
                if (vdesMessage.Contains("SP02"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.SP02;
                    p.SP02 = node2.Value;
                    return p;
                }
                //Resp
                if (vdesMessage.Contains("Resp"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.Resp;
                    p.Resp = node2.Value;
                    return p;
                }
                //Temp
                if (vdesMessage.Contains("Temp"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.Temp;
                    p.Temp = node2.Value;
                    if (vdesMessage.Contains("TempType"))
                    {
                        node2 = node.TempType;
                        p.TempType = node2.Value;
                    }
                    return p;
                }
                //Pain Scale
                if (vdesMessage.Contains("PainScale"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.PainScale;
                    p.painScale = node2.Value;
                    return p;
                }
                //AVPU
                if (vdesMessage.Contains("AVPU"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Vitals;
                    var node2 = node.AVPU;
                    p.AVPU = node2.Value;
                    return p;
                }
            }
            //Admin
            if (vdesMessage.Contains("Admin"))
            {
                //firstname
                if (vdesMessage.Contains("firstName"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.firstName;
                    p.firstName = node.Value;
                    return p;
                }
                //middle name
                if (vdesMessage.Contains("middleName"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.middleName;
                    p.middleName = node.Value;
                    return p;
                }
                //last name
                if (vdesMessage.Contains("lastName"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.lastName;
                    p.lastName = node.Value;
                    return p;
                }
                //unit
                if (vdesMessage.Contains("unit"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.unit;
                    p.unitName = node.Value;
                    return p;
                }
                //service
                if (vdesMessage.Contains("service"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.service;
                    p.service = node.Value;
                    return p;
                }
                //rank
                if (vdesMessage.Contains("rank"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.rank;
                    p.Rank = node.Value;
                    return p;
                }
                //weight
                if (vdesMessage.Contains("weight"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.weight;
                    p.weight = node.Value;
                    if (vdesMessage.Contains("weightType"))
                    {
                        node = value.Admin.weightType;
                        p.weightType = node.Value;
                    }
                    return p;
                }
                //gender
                if (vdesMessage.Contains("gender"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.gender;
                    p.gender = node.Value;
                    return p;
                }
                //SSN (last4)
                if (vdesMessage.Contains("SSN"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.SSN;
                    p.SSN = node.Value;
                    return p;
                }
                //Date
                if (vdesMessage.Contains("Date"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.Date;
                    p.recordDate = node.Value;
                    return p;
                }
                //Time
                if (vdesMessage.Contains("Time"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Admin.Time;
                    p.recordTime = node.Value;
                    return p;
                }
            }
            //Injury
            if (vdesMessage.Contains("Injury"))
            {
                //Type & Location
                if (vdesMessage.Contains("Type") && vdesMessage.Contains("Location"))
                {
                    patient.Injuries injury = new patient.Injuries();
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Injury.Type;
                    injury.Type = node.Value;
                    node = value.Injury.Location;
                    injury.Location = node.Value;
                    p.injuries.Add(injury);
                    return p;
                }
            }

            //Treatments
            if (vdesMessage.Contains("Treatment"))
            {
                //Airway
                if (vdesMessage.Contains("Airway"))
                {
                    //Intact
                    if (vdesMessage.Contains("Intact"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Airway.Intact;
                        if (node.Value == "True")
                        {
                            p.treatments.airway.intact = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.airway.intact = false;
                            return p;
                        }
                    }
                    //NPA
                    if (vdesMessage.Contains("NPA"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Airway.NPA;
                        if (node.Value == "True")
                        {
                            p.treatments.airway.NPA = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.airway.NPA = false;
                            return p;
                        }
                    }
                    //CRIC
                    if (vdesMessage.Contains("CRIC"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Airway.CRIC;
                        if (node.Value == "True")
                        {
                            p.treatments.airway.CRIC = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.airway.CRIC = false;
                            return p;
                        }
                    }
                    //ET-Tube
                    if (vdesMessage.Contains("etTube"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Airway.ETtube;
                        if (node.Value == "True")
                        {
                            p.treatments.airway.etTube = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.airway.etTube = false;
                            return p;
                        }
                    }
                    //SGA
                    if (vdesMessage.Contains("SGA"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Airway.SGA;
                        if (node.Value == "True")
                        {
                            p.treatments.airway.SGA = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.airway.SGA = false;
                            return p;
                        }
                    }
                }
                //Breathing
                if (vdesMessage.Contains("Breathing"))
                {
                    //O2
                    if (vdesMessage.Contains("O2"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Breathing.O2;
                        if (node.Value == "True")
                        {
                            p.treatments.breathing.o2 = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.breathing.o2 = false;
                            return p;
                        }
                    }
                    //Needle-D
                    if (vdesMessage.Contains("needleD"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Breathing.needleD;
                        if (node.Value == "True")
                        {
                            p.treatments.breathing.needleD = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.breathing.needleD = false;
                            return p;
                        }
                    }
                    //Chest-Tube
                    if (vdesMessage.Contains("chestTube"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Breathing.chestTube;
                        if (node.Value == "True")
                        {
                            p.treatments.breathing.chestTube = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.breathing.chestTube = false;
                            return p;
                        }
                    }
                    //Chest-Seal
                    if (vdesMessage.Contains("chestSeal"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Breathing.chestSeal;
                        if (node.Value == "True")
                        {
                            p.treatments.breathing.chestSeal = true;
                            return p;
                        }
                        else if (node.Value == "False")
                        {
                            p.treatments.breathing.chestSeal = false;
                            return p;
                        }
                    }
                }
                //Circulation
                if (vdesMessage.Contains("Circulation"))
                {
                    //TQ
                    if (vdesMessage.Contains("TQ"))
                    {
                        //Extremity
                        if (vdesMessage.Contains("Extremity"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.TQ.Extremity;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.TQ.extremity = true;
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.TQ.extremity = false;
                                return p;
                            }
                        }
                        //Junctional
                        if (vdesMessage.Contains("Junctional"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.TQ.Junctional;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.TQ.junctional = true;
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.TQ.junctional = false;
                                return p;
                            }
                        }
                        //Truncal
                        if (vdesMessage.Contains("Truncal"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.TQ.Truncal;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.TQ.truncal = true;
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.TQ.truncal = false;
                                return p;
                            }
                        }
                    }
                    //Dressing
                    if (vdesMessage.Contains("Dressing"))
                    {
                        //Hemostatic
                        if (vdesMessage.Contains("Hemostatic"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.Dressing.Hemostatic;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.dressing.hemostatic = true;
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.dressing.hemostatic = false;
                                return p;
                            }
                        }
                        //Pressure
                        if (vdesMessage.Contains("Pressure"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.Dressing.Pressure;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.dressing.pressure = true;
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.dressing.pressure = false;
                                return p;
                            }
                        }
                        //Other
                        if (vdesMessage.Contains("Other"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Circulation.Dressing.Other;
                            if (node.Value == "True")
                            {
                                p.treatments.circulation.dressing.other = true;
                                if (vdesMessage.Contains("OtherType"))
                                {
                                    node = value.Treatment.Circulation.Dressing.OtherType;
                                    p.treatments.circulation.dressing.otherType = node.Value;
                                }
                                return p;
                            }
                            else if (node.Value == "False")
                            {
                                p.treatments.circulation.dressing.other = false;
                                return p;
                            }
                        }
                    }
                }
                //FLuids
                if (vdesMessage.Contains("Fluid"))
                {
                    //Normal Saline
                    if (vdesMessage.Contains("normalSaline"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.normalSaline.Volume;
                            p.treatments.fluids.normalSaline.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.normalSaline.Route;
                            p.treatments.fluids.normalSaline.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.normalSaline.Time;
                            p.treatments.fluids.normalSaline.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Lactacted Ringers
                    if (vdesMessage.Contains("lactactedRingers"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.lactactedRingers.Volume;
                            p.treatments.fluids.lactactedRingers.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.lactactedRingers.Route;
                            p.treatments.fluids.lactactedRingers.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.lactactedRingers.Time;
                            p.treatments.fluids.lactactedRingers.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }                    
                    //Dextrose
                    if (vdesMessage.Contains("dextrose"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.dextrose.Volume;
                            p.treatments.fluids.dextrose.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Percentage"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.dextrose.Percentage;
                            p.treatments.fluids.dextrose.Percentage = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.dextrose.Route;
                            p.treatments.fluids.dextrose.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.dextrose.Time;
                            p.treatments.fluids.dextrose.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Other
                    if (vdesMessage.Contains("other"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Type"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.other.Type;
                            p.treatments.fluids.other.Type = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.other.Volume;
                            p.treatments.fluids.other.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.other.Route;
                            p.treatments.fluids.other.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.Fluid.other.Time;
                            p.treatments.fluids.other.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                }

                //BloodProduct
                if (vdesMessage.Contains("BloodProduct"))
                {
                    //WholeBlood
                    if (vdesMessage.Contains("wholeBlood"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholeBlood.Volume;
                            p.treatments.bloodProducts.wholeBlood.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholeBlood.Route;
                            p.treatments.bloodProducts.wholeBlood.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholeBlood.Time;
                            p.treatments.bloodProducts.wholeBlood.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //PackedRedBlood
                    if (vdesMessage.Contains("packedRedBlood"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.packedRedBlood.Volume;
                            p.treatments.bloodProducts.packedRedBlood.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.packedRedBlood.Route;
                            p.treatments.bloodProducts.packedRedBlood.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.packedRedBlood.Time;
                            p.treatments.bloodProducts.packedRedBlood.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //PlateleteRichPlasma
                    if (vdesMessage.Contains("plateleteRichPlasma"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plateleteRichPlasma.Volume;
                            p.treatments.bloodProducts.plateleteRichPlasma.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plateleteRichPlasma.Route;
                            p.treatments.bloodProducts.plateleteRichPlasma.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plateleteRichPlasma.Time;
                            p.treatments.bloodProducts.plateleteRichPlasma.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //WholePlasma
                    if (vdesMessage.Contains("wholePlasma"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholePlasma.Volume;
                            p.treatments.bloodProducts.wholePlasma.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholePlasma.Route;
                            p.treatments.bloodProducts.wholePlasma.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.wholePlasma.Time;
                            p.treatments.bloodProducts.wholePlasma.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Cryoprecipitate
                    if (vdesMessage.Contains("cryoprecipitate"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.cryoprecipitate.Volume;
                            p.treatments.bloodProducts.cryoprecipitate.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.cryoprecipitate.Route;
                            p.treatments.bloodProducts.cryoprecipitate.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.cryoprecipitate.Time;
                            p.treatments.bloodProducts.cryoprecipitate.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //SerumAlbumin
                    if (vdesMessage.Contains("serumAlbumin"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.serumAlbumin.Volume;
                            p.treatments.bloodProducts.serumAlbum.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.serumAlbumin.Route;
                            p.treatments.bloodProducts.serumAlbum.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.serumAlbumin.Time;
                            p.treatments.bloodProducts.serumAlbum.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Plasmanate
                    if (vdesMessage.Contains("plasmanate"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plasmanate.Volume;
                            p.treatments.bloodProducts.plasmanate.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plasmanate.Route;
                            p.treatments.bloodProducts.plasmanate.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.plasmanate.Time;
                            p.treatments.bloodProducts.plasmanate.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Hextend
                    if (vdesMessage.Contains("hextend"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.hextend.Volume;
                            p.treatments.bloodProducts.hextend.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.hextend.Route;
                            p.treatments.bloodProducts.hextend.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.hextend.Time;
                            p.treatments.bloodProducts.hextend.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                    //Other
                    if (vdesMessage.Contains("other"))
                    {
                        bool hasData = false;
                        //Type, Volume, Route, Time, Percentage
                        if (vdesMessage.Contains("Type"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.other.Type;
                            p.treatments.bloodProducts.other.Type = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Volume"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.other.Volume;
                            p.treatments.bloodProducts.other.Dose = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Route"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.other.Route;
                            p.treatments.bloodProducts.other.Route = node.Value;
                            hasData = true;
                        }
                        if (vdesMessage.Contains("Time"))
                        {
                            dynamic value = JsonValue.Parse(vdesMessage);
                            var node = value.Treatment.BloodProduct.other.Time;
                            p.treatments.bloodProducts.other.Time = node.Value;
                            hasData = true;
                        }
                        if (hasData)
                        {
                            return p;
                        }
                    }
                }
                //Other
                if (vdesMessage.Contains("Other"))
                {
                    //CombatPillPack
                    if (vdesMessage.Contains("CombatPillPack"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Other.CombatPillPack;
                        p.OtherTreatments.CombatPillPack = node.Value;
                        return p;
                    }
                    //Splint
                    if (vdesMessage.Contains("Splint"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Other.Splint;
                        p.OtherTreatments.Splint = node.Value;
                        return p;
                    }
                    //EyeShieldL
                    if (vdesMessage.Contains("EyeShieldL"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Other.EyeShieldL;
                        p.OtherTreatments.EyeShieldL = node.Value;
                        return p;
                    }
                    //EyeShieldR
                    if (vdesMessage.Contains("EyeShieldR"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Other.EyeShieldR;
                        p.OtherTreatments.EyeShieldR = node.Value;
                        return p;
                    }
                    //HypothermiaPrevention
                    if (vdesMessage.Contains("HypothermiaPrevention"))
                    {
                        dynamic value = JsonValue.Parse(vdesMessage);
                        var node = value.Treatment.Other.HypothermiaPrevention;
                        p.OtherTreatments.HypothermiaPrevention = node.Value;
                        return p;
                    }
                }
            }
            //Medications
            if (vdesMessage.Contains("Medication"))
            {
                bool hasData = false;

                //Type, Volume, Route, Time, Percentage
                if (vdesMessage.Contains("Type"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Type;
                    p.tempMed.Name = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Dose"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Dose;
                    p.tempMed.Dose = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Route"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Route;
                    p.tempMed.Route = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Time"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Time;
                    p.tempMed.Time = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Units"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Units;
                    p.tempMed.Units = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Analgesic"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Analgesic;
                    p.tempMed.Analgesic = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Antibiotic"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Antibiotic;
                    p.tempMed.Antibiotic = node.Value;
                    hasData = true;
                }
                if (vdesMessage.Contains("Other"))
                {
                    dynamic value = JsonValue.Parse(vdesMessage);
                    var node = value.Medication.Other;
                    p.tempMed.Other = node.Value;
                    hasData = true;
                }
                if (hasData)
                {
                    return p;
                }
            }


            //Notes
            if (vdesMessage.Contains("Notes"))
            {
                dynamic value = JsonValue.Parse(vdesMessage);
                var node = value.Notes;
                p.notes = node.Value;
                return p;
            }

            //Allergies
            if (vdesMessage.Contains("Allergy"))
            {
                dynamic value = JsonValue.Parse(vdesMessage);
                var node = value.Allergy;
                p.vdesAllergy = node.Value;
                return p;
            }
                
            

            return p;
        }

    }
}

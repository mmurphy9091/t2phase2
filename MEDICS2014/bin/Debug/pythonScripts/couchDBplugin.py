import mosquitto
import requests
import couchdb
import json

from couchdb.mapping import Document, TextField, DictField, Mapping, ListField
from collections import namedtuple

#DB Patient class
class Patient(Document):
    Admin = DictField(Mapping.build(
        FirstName = TextField(),
        MiddleInitial = TextField(),
        LastName = TextField(),
        Rank = TextField(),
        Gender = TextField(),
        Date = TextField(),
        RecordTime = TextField(),
        Last4 = TextField(),
        Unit = TextField(),
        Service= TextField(),
        Weight = TextField(),
        WeightType = TextField()
        ))
    InjuryList = DictField(Mapping.build(
        Injuries = ListField(DictField(Mapping.build(
            Type = TextField(),
            Location = TextField()
            )))
        ))
    Allergies = ListField(DictField(Mapping.build(
        Type = TextField()
        )))
    Vitals = ListField(DictField(Mapping.build(
        HR = TextField(),
        Resp = TextField(),
        SP02 = TextField(),
        AVPU = TextField(),
        BPSYS = TextField(),
        BPDIA = TextField(),
        painScale = TextField(),
        Temp = TextField(),
        TempType = TextField(),
        vitalsTime = TextField()
        )))
    Treatments = DictField(Mapping.build(
        Airway = DictField(Mapping.build(
            CRIC = TextField(),
            etTube = TextField(),
            intact = TextField(),
            NPA = TextField(),
            SGA = TextField()
        )),
        Breathing = DictField(Mapping.build(
            o2 = TextField(),
            needleD = TextField(),
            chestTube = TextField(),
            chestSeal = TextField()
        )),
        Circulation = DictField(Mapping.build(
            TQ = DictField(Mapping.build(
                extremity = TextField(),
                junctional = TextField(),
                truncal = TextField()
                )),
            dressing = DictField(Mapping.build(
                hemostatic = TextField(),
                pressure = TextField(),
                other = TextField(),
                otherType = TextField()
                )),
        )),
        BloodProducts = DictField(Mapping.build(
            wholeBlood = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            packedRedBlood = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            plateleteRichPlasma = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            wholePlasma = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            cryoprecipitate = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            serumAlbum = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            plasmanate = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            hextend = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            other = DictField(Mapping.build(
                Type = TextField(),
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            ))
        )),
        Fluids = DictField(Mapping.build(
            normalSaline = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            lactactedRingers = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            )),
            dextrose = DictField(Mapping.build(
                Dose = TextField(),
                Route = TextField(),
                Time = TextField(),
                Percentage = TextField()
            )),
            other = DictField(Mapping.build(
                Type = TextField(),
                Dose = TextField(),
                Route = TextField(),
                Time = TextField()
            ))
        )))
                           )
    Medications = ListField(DictField(Mapping.build(
        Name = TextField(),
        Dose = TextField(),
        Units = TextField(),
        Route = TextField(),
        Time = TextField(),
        Analgesic = TextField(),
        Antibiotic = TextField(),
        Other = TextField()
        )))
    Notes = TextField()
    OtherTreatments = (DictField(Mapping.build(
        CombatPillPack = TextField(),
        EyeShieldR = TextField(),
        EyeShieldL = TextField(),
        Splint = TextField(),
        HypothermiaPrevention = TextField()
        )))
    TQList = ListField(DictField(Mapping.build(
        Type = TextField(),
        Location = TextField(),
        Time = TextField()
        )))
    AllPCDs = ListField(DictField(Mapping.build(
        ID = TextField(),
        )))
    PatPCDs = ListField(DictField(Mapping.build(
        ID = TextField(),
        )))
        
#MQTT Globals
mqttc = mosquitto.Mosquitto()
listenTopic = "medics/data/#"
postTopic = "medics/data/4"
dbExists = 'false'

#URL Globals
startURL = "http://127.0.0.1:5984/medics/_design/patient/_view/"
midURL = "?key=%22"
endURL = "%22"
patListURL = "http://127.0.0.1:5984/medics/_design/patient/_view/allPatients"

#Database Globals
DBall = "allByID"
DBinjuries = "Injuries"

def on_connect(mosq, obj, rc):
    mosq.subscribe(listenTopic, 1)
    print("rc: "+str(rc))

def on_message(mosq, obj, msg):
    #Check if it's on the delete Channel
    if str(msg.topic) == "medics/data/3":
        message = msg.payload.replace("DELETE:","")
        print message
        print 'harhar'
        deletePatient(message)
    #Check if it's on the Get Channel
    if str(msg.topic) == "medics/data/5":
        parseGetMessage(msg.payload)
    #Check if it's on the Post Channel
    elif str(msg.topic) == "medics/data/6":
        parsePostMessage(msg.payload)
    #print(msg.topic+" "+str(msg.qos)+" "+str(msg.payload))+" "+str(msg.topic)
    #Check if it's a grab for medics data
    elif str(msg.topic) == "medics/data/9":
        if msg.payload == patListURL:
            getPatientList()
        #check if it's a call for a new patient
        if "NEWPATIENT:" in msg.payload:
            message = msg.payload.replace('NEWPATIENT:', '')
            createNewPatient(message)
    #Check if it's a replication command
    elif str(msg.topic) == "medics/data/10":
        if 'Replicate:' in msg.payload:
            replicateIP = msg.payload.replace('Replicate:','')
            replicateURL = 'http://'+replicateIP+':5984/medics'
            #print replicateURL
            #grab the database
            couch = couchdb.Server()
            couch.replicate('medics',replicateURL, continuous=True)
            couch.replicate(replicateURL, 'medics', continuous=True)
            print 'Replication set up to URL ' + replicateURL
            mqttc.publish("medics/data/10", 'Done', 1)
        if 'Kill:' in msg.payload:
            killIP = msg.payload.replace('Kill:','')
            killURL = 'http://'+killIP+':5984/medics'
            #print replicateURL
            #grab the database
            couch = couchdb.Server()
            couch.replicate('medics',killURL, continuous=True, cancel=True)
            #couch.replicate(replicateURL, 'medics', continuous=True)
            print 'Replication canceled to URL ' + killURL
            #mqttc.publish("medics/data/10", 'Done', 1)

def on_publish(mosq, obj, mid):
    print("mid: "+str(mid))

def on_subscribe(mosq, obj, mid, granted_qos):
    print("Subscribed: "+str(mid)+" "+str(granted_qos))

def on_log(mosq, obj, level, string):
    print(string)

#Deletes patient from database. not to be used calmly
#
#
#
def deletePatient(patientID):
    #set up couch
    couch = couchdb.Server()
    db = couch['medics']
    print patientID
    del db[patientID]
    print "Deleted patient with ID: " + patientID

#Creates a new patient document with the ID created by the GUI
#
#
#
def createNewPatient(ID):
    #global dbExists
    #if dbExists == 'false':
        #db = couch.create('medics')
        #check for database
    checkForMedicsDB()
    
    doc = {
   "_id": "" + ID + "",
   "Medications": [
       
   ],
   "Admin": {
       "RecordTime": "",
       "FirstName": "New",
       "Service": "",
       "Last4": "",
       "LastName": "Patient",
       "MiddleInitial": "",
       "Rank": "",
       "Weight": "",
       "WeightType": "",
       "Gender": "",
       "Date": "",
       "Unit": ""
   },
   "Allergies": [
       
   ],
   "TQList": [
       
   ],
   "AllPCDs": [
       
   ],
   "PatPCDs": [
       
   ],
   "Treatments": {
       "BloodProducts": {
           "packedRedBlood": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "serumAlbum": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "plasmanate": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "hextend": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "wholePlasma": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "other": {
               "Route": "",
               "Dose": "",
               "Type": "",
               "Time": ""
           },
           "cryoprecipitate": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "plateleteRichPlasma": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "wholeBlood": {
               "Route": "",
               "Dose": "",
               "Time": ""
           }
       },
       "Breathing": {
           "needleD": "False",
           "chestTube": "False",
           "o2": "False",
           "chestSeal": "False"
       },
       "Fluids": {
           "lactactedRingers": {
               "Route": "",
               "Dose": "",
               "Time": ""
           },
           "dextrose": {
               "Route": "",
               "Dose": "",
               "Percentage": "",
               "Time": ""
           },
           "other": {
               "Route": "",
               "Dose": "",
               "Type": "",
               "Time": ""
           },
           "normalSaline": {
               "Route": "",
               "Dose": "",
               "Time": ""
           }
       },
       "Circulation": {
           "TQ": {
               "extremity": "False",
               "junctional": "False",
               "truncal": "False"
           },
           "dressing": {
               "hemostatic": "False",
               "pressure": "False",
               "other": "False",
               "otherType": ""
           }
       },
       "Airway": {
           "SGA": "False",
           "intact": "False",
           "NPA": "False",
           "CRIC": "False",
           "etTube": "False"
       }
   },
   "InjuryList": {
       "Injuries": [
           
       ]
   },
   "OtherTreatments": {
       "EyeShieldR": "False",
       "CombatPillPack": "False",
       "HypothermiaPrevention": "False",
       "Splint": "False",
       "EyeShieldL": "False"
   },
   "Vitals": [
       
   ],
   "Type": "Patient",
   "Notes": ""
}
    #set up couch
    couch = couchdb.Server()
    db = couch['medics']
    db.save(doc)
    print "Added new patient with ID: " + ID

#Gets the patient list and returns the data
#
#
#
def getPatientList():
    #getURL = msg;
    print "Retrieving patient list"
    print "URL: " + patListURL
    response = requests.get(patListURL)
    message = response.content
    print "Response body: " + message
    global mqttc
    mqttc.publish("medics/data/8", message, 1)

#Gets patient info by ID
#
#
#
def get_all_info_by_id(id):
    getURL = "http://127.0.0.1:5984/medics/_design/patient/_view/allByID?key=%22" + id + "%22"
    print "URL: " + getURL
    response = requests.get(getURL)
    message = response.content
    print "Sending entire patient info"
    global mqttc
    mqttc.publish("hello/world", message, 1)

#Gets patient injuries info by ID
#
#
#
def get_injuries_by_id(id):
    getURL = "http://127.0.0.1:5984/medics/_design/patient/_view/Injuries?key=%22" + id + "%22"
    print "URL: " + getURL
    response = requests.get(getURL)
    message = response.content
    print "Response body: " + message
    global mqttc
    mqttc.publish("hello/world", message, 1)

#Gets the requested patient data
#
#
#
def request_data_from_DB(id, type):
    getURL = startURL + type + midURL + id + endURL
    print "URL: " + getURL
    response = requests.get(getURL)
    message = response.content
    print "Sending data from request " + type
    global mqttc
    mqttc.publish(postTopic, message, 1)

#Parses message and calls the correct method
#
#
#
def parseGetMessage(message):

    #check for method and variable
    parsedMessage = message.split(":")

    #gets and sends the data
    print "ID: " + parsedMessage[1]
    print "Type: " + parsedMessage[0]
    request_data_from_DB(parsedMessage[1], parsedMessage[0])

    
    #Does the get all method
    #if parsedMessage[0] == "getPatientInfoAll":
    #    request_data_from_DB(parsedMessage[1], DBall)

    #Does the get injuries method
    #elif parsedMessage[0] == "getPatientInjuries":
    #    request_data_from_DB(parsedMessage[1], DBinjuries)
        

#check for and create new database
#
#
#
def checkForMedicsDB():
    checkURL = 'http://127.0.0.1:5984/medics'
    r = requests.put(checkURL)
    if '{"ok":true}' in r.text:
        doc = {
   "_id": "_design/patient",
   "language": "javascript",
   "views": {
       "all": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(null,doc)}"
       },
       "allByID": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(doc._id,doc)}"
       },
       "Injuries": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(doc._id,{Injuries:doc.InjuryList.Injuries})}"
       },
       "Admin": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(doc._id,{Admin:doc.Admin})}"
       },
       "allPatients": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(null,{ID: doc._id, FirstName: doc.Admin.FirstName, LastName: doc.Admin.LastName})}"
       },
       "IDsAndPCDs": {
           "map": "function(doc) {if(doc.Type=='Patient') emit(null,{ID: doc._id, PCDs:doc.PatPCDs})}"
       }
   }
}
        couch = couchdb.Server()
        db = couch['medics']
        db.save(doc)
        print "Created MEDICS database"
        #global dbExists
        #dbExists = 'true'
        

#Parses POST message and calls correct method based on data
#
#
#
def parsePostMessage(message):
    #set up couch
    couch = couchdb.Server()
    
    #global dbExists
    #if dbExists == 'false':
        #db = couch.create('medics')
        #check for database
    checkForMedicsDB()
    
    db = couch['medics']
    #check for ID and data
    #patient = Patient.load(db, '1234567890')

    #set the message as an object
    data = json.loads(message, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))

    #Load the document
    patient = Patient.load(db, str(data.ID))
    
    #see if it's an injury needing to be altered
    if hasattr(data, 'Injury'):
        #See if it needs to be deleted
        if data.Injury.deleteString == 'True':
            #delete the injury
            obj = {'Type' : data.Injury.Type, 'Location' : data.Injury.Location}
            patient.InjuryList.Injuries.remove(obj)
            patient.store(db)
            print "Removed injury " + data.Injury.Type + " " + data.Injury.Location
        else:
            #append the injury
            injuryNeedsAdded = 'true'
            for inj in patient.InjuryList.Injuries:
                if inj.Type == data.Injury.Type and inj.Location == data.Injury.Location:
                    print "Injury already in database"
                    injuryNeedsAdded = 'false'
            if injuryNeedsAdded == 'true':
                patient.InjuryList.Injuries.append(Type = data.Injury.Type,
                                                   Location = data.Injury.Location)
                patient.store(db)
                print "Added injury " + data.Injury.Type + " " + data.Injury.Location
    #Do allergy work
    if hasattr(data, 'Allergy'):
        #See if it needs to be deleted
        if data.Allergy.delete == 'True':
            #delete the allergy
            obj = {'Type' : data.Allergy.Type}
            patient.Allergies.remove(obj)
            patient.store(db)
            print "Removed allergy " + data.Allergy.Type
        else:
            #append the injury to the list
            patient.Allergies.append(Type = data.Allergy.Type)
            patient.store(db)
            print "Added allergy " + data.Allergy.Type
    #Do admin work
    if hasattr(data, "Admin"):
        needsSave = "false"
        logList = list()
        #First Name
        if hasattr(data.Admin, "FirstName"):
            patient.Admin.FirstName = data.Admin.FirstName
            logItem = "Added first name: " + patient.Admin.FirstName
            logList.append(logItem)
            needsSave = "true"
        #Middle Initial
        if hasattr(data.Admin, "MiddleInitial"):
            patient.Admin.MiddleInitial = data.Admin.MiddleInitial
            logItem = "Added middle initial: " + patient.Admin.MiddleInitial
            logList.append(logItem)
            needsSave = "true"
        #Last Name
        if hasattr(data.Admin, "LastName"):
            patient.Admin.LastName = data.Admin.LastName
            logItem = "Added last name: " + patient.Admin.LastName
            logList.append(logItem)
            needsSave = "true"
        #Weight
        if hasattr(data.Admin, "Weight"):
            patient.Admin.Weight = data.Admin.Weight
            logItem = "Added weight: " + patient.Admin.Weight
            if data.Admin.Weight != "":
                if hasattr(data.Admin, "WeightType"):
                    patient.Admin.WeightType = data.Admin.WeightType
                    logItem += " " + data.Admin.WeightType
            logList.append(logItem)
            needsSave = "true"
        #Last 4
        if hasattr(data.Admin, "Last4"):
            patient.Admin.Last4 = data.Admin.Last4
            logItem = "Added SSN: " + patient.Admin.Last4
            logList.append(logItem)
            needsSave = "true"
        #Gender
        if hasattr(data.Admin, "Gender"):
            patient.Admin.Gender = data.Admin.Gender
            logItem = "Added gender: " + patient.Admin.Gender
            logList.append(logItem)
            needsSave = "true"
        #Service
        if hasattr(data.Admin, "Service"):
            patient.Admin.Service = data.Admin.Service
            logItem = "Added service: " + patient.Admin.Service
            logList.append(logItem)
            needsSave = "true"
        #Unit
        if hasattr(data.Admin, "Unit"):
            patient.Admin.Unit = data.Admin.Unit
            logItem = "Added unit: " + patient.Admin.Unit
            logList.append(logItem)
            needsSave = "true"
        #Record Time
        if hasattr(data.Admin, "RecordTime"):
            patient.Admin.RecordTime = data.Admin.RecordTime
            logItem = "Added record time: " + patient.Admin.RecordTime
            logList.append(logItem)
            needsSave = "true"
        #Date
        if hasattr(data.Admin, "Date"):
            patient.Admin.Date = data.Admin.Date
            logItem = "Added record date: " + patient.Admin.Date
            logList.append(logItem)
            needsSave = "true"
        #Save data if needed
        if needsSave == "true":
            patient.store(db)
            for item in logList:
                print item
    #Vitals work
    if hasattr(data, 'Vitals'):
        #handle ap exceptions
        fromAP = 'false'
        if hasattr(data.Vitals, 'AVPU') and hasattr(data.Vitals, 'painScale'):
            print "HAS"
            obj = {'HR' : data.Vitals.HR,
                   'Resp' : data.Vitals.Resp,
                   'SP02' : data.Vitals.SP02,
                   'Temp' : data.Vitals.Temp,
                   'TempType' : data.Vitals.TempType,
                   'AVPU' : data.Vitals.AVPU,
                   'BPSYS' : data.Vitals.BPSYS,
                   'BPDIA' : data.Vitals.BPDIA,
                   'painScale' : data.Vitals.painScale,
                   'vitalsTime' : data.Vitals.vitalsTime
                   }
        else:
            print "HASNT"
            fromAP = 'true'
            obj = {'HR' : data.Vitals.HR,
                   'Resp' : data.Vitals.Resp,
                   'SP02' : data.Vitals.SP02,
                   'Temp' : data.Vitals.Temp,
                   'TempType' : data.Vitals.TempType,
                   'AVPU' : '',
                   'BPSYS' : data.Vitals.BPSYS,
                   'BPDIA' : data.Vitals.BPDIA,
                   'painScale' : '',
                   'vitalsTime' : data.Vitals.vitalsTime,
                   'PCDID' : data.Vitals.PCDID
                   }
        if fromAP == 'false':
            if data.Vitals.HR == "" and data.Vitals.Resp == "" and data.Vitals.SP02 == "" and data.Vitals.Temp == "" and data.Vitals.AVPU == "" and data.Vitals.BPSYS == "" and data.Vitals.BPDIA == "" and data.Vitals.painScale == "":
                #do nothing
                print "No vitals to add"
            else:
                patient.Vitals.append(obj)
                patient.store(db)
            if data.Vitals.HR != "":
                print "Added HR:" + data.Vitals.HR
            if data.Vitals.Resp != "":
                print "Added Resp:" + data.Vitals.Resp
            if data.Vitals.SP02 != "":
                print "Added Sp02:" + data.Vitals.SP02
            if data.Vitals.Temp != "":
                print "Added Temp:" + data.Vitals.Temp + data.Vitals.TempType
            if data.Vitals.AVPU != "":
                print "Added AVPU:" + data.Vitals.AVPU
            if data.Vitals.BPSYS != "":
                print "Added BP:" + data.Vitals.BPSYS + "/" + data.Vitals.BPDIA
            if data.Vitals.painScale != "":
                print "Added Pain Scale:" + data.Vitals.painScale
            if data.Vitals.vitalsTime != "":
                print "Added Time for vitals:" + data.Vitals.vitalsTime
        elif fromAP == 'true':
            if data.Vitals.HR == "" and data.Vitals.Resp == "" and data.Vitals.SP02 == "" and data.Vitals.Temp == "" and data.Vitals.BPSYS == "" and data.Vitals.BPDIA == "":
                #do nothing
                print "No vitals to add"
            else:
                patient.Vitals.append(obj)
                patient.store(db)
            print "Added vitals from PCD " + data.Vitals.PCDID
    #Treatments work
    if hasattr(data, 'Treatments'):
        #Airway
        if hasattr(data.Treatments, 'Airway'):
            #CRIC
            if (data.Treatments.Airway.CRIC == 'True' and patient.Treatments.Airway.CRIC == 'False'):
                patient.Treatments.Airway.CRIC = data.Treatments.Airway.CRIC
                patient.store(db)
                print "Treatment added: Airway CRIC"
            elif (data.Treatments.Airway.CRIC == 'False' and patient.Treatments.Airway.CRIC == 'True'):
                patient.Treatments.Airway.CRIC = data.Treatments.Airway.CRIC
                patient.store(db)
                print "Treatment removed: Airway CRIC"
            #SGA
            if (data.Treatments.Airway.SGA == 'True' and patient.Treatments.Airway.SGA == 'False'):
                patient.Treatments.Airway.SGA = data.Treatments.Airway.SGA
                patient.store(db)
                print "Treatment added: Airway SGA"
            elif (data.Treatments.Airway.SGA == 'False' and patient.Treatments.Airway.SGA == 'True'):
                patient.Treatments.Airway.SGA = data.Treatments.Airway.SGA
                patient.store(db)
                print "Treatment removed: Airway SGA"
            #NPA
            if (data.Treatments.Airway.NPA == 'True' and patient.Treatments.Airway.NPA == 'False'):
                patient.Treatments.Airway.NPA = data.Treatments.Airway.NPA
                patient.store(db)
                print "Treatment added: Airway NPA"
            elif (data.Treatments.Airway.NPA == 'False' and patient.Treatments.Airway.NPA == 'True'):
                patient.Treatments.Airway.NPA = data.Treatments.Airway.NPA
                patient.store(db)
                print "Treatment removed: Airway NPA"
            #etTube
            if (data.Treatments.Airway.etTube == 'True' and patient.Treatments.Airway.etTube == 'False'):
                patient.Treatments.Airway.etTube = data.Treatments.Airway.etTube
                patient.store(db)
                print "Treatment added: Airway ET-Tube"
            elif (data.Treatments.Airway.etTube == 'False' and patient.Treatments.Airway.etTube == 'True'):
                patient.Treatments.Airway.etTube = data.Treatments.Airway.etTube
                patient.store(db)
                print "Treatment removed: Airway ET-Tube"
            #intact
            if (data.Treatments.Airway.intact == 'True' and patient.Treatments.Airway.intact == 'False'):
                patient.Treatments.Airway.intact = data.Treatments.Airway.intact
                patient.store(db)
                print "Treatment added: Airway intact"
            elif (data.Treatments.Airway.intact == 'False' and patient.Treatments.Airway.intact == 'True'):
                patient.Treatments.Airway.intact = data.Treatments.Airway.intact
                patient.store(db)
                print "Treatment removed: Airway intact"

        #Breathing
        if hasattr(data.Treatments, 'Breathing'):
            #o2
            if (data.Treatments.Breathing.o2 == 'True' and patient.Treatments.Breathing.o2 == 'False'):
                patient.Treatments.Breathing.o2 = data.Treatments.Breathing.o2
                patient.store(db)
                print "Treatment added: Breathing O2"
            elif (data.Treatments.Breathing.o2 == 'False' and patient.Treatments.Breathing.o2 == 'True'):
                patient.Treatments.Breathing.o2 = data.Treatments.Breathing.o2
                patient.store(db)
                print "Treatment removed: Breathing O2"
            #needleD
            if (data.Treatments.Breathing.needleD == 'True' and patient.Treatments.Breathing.needleD == 'False'):
                patient.Treatments.Breathing.needleD = data.Treatments.Breathing.needleD
                patient.store(db)
                print "Treatment added: Breathing Needle-D"
            elif (data.Treatments.Breathing.needleD == 'False' and patient.Treatments.Breathing.needleD == 'True'):
                patient.Treatments.Breathing.needleD = data.Treatments.Breathing.needleD
                patient.store(db)
                print "Treatment removed: Breathing Needle-D"
            #chestTube
            if (data.Treatments.Breathing.chestTube == 'True' and patient.Treatments.Breathing.chestTube == 'False'):
                patient.Treatments.Breathing.chestTube = data.Treatments.Breathing.chestTube
                patient.store(db)
                print "Treatment added: Breathing Chest Tube"
            elif (data.Treatments.Breathing.chestTube == 'False' and patient.Treatments.Breathing.chestTube == 'True'):
                patient.Treatments.Breathing.chestTube = data.Treatments.Breathing.chestTube
                patient.store(db)
                print "Treatment removed: Breathing Chest Tube"
            #chestSeal
            if (data.Treatments.Breathing.chestSeal == 'True' and patient.Treatments.Breathing.chestSeal == 'False'):
                patient.Treatments.Breathing.chestSeal = data.Treatments.Breathing.chestSeal
                patient.store(db)
                print "Treatment added: Breathing Chest Seal"
            elif (data.Treatments.Breathing.chestSeal == 'False' and patient.Treatments.Breathing.chestSeal == 'True'):
                patient.Treatments.Breathing.chestSeal = data.Treatments.Breathing.chestSeal
                patient.store(db)
                print "Treatment removed: Breathing Chest Seal"

        #Circulation
        if hasattr(data.Treatments, 'Circulation'):
            #dressing
            if hasattr(data.Treatments.Circulation, 'dressing'):
                #hemostatic
                if (data.Treatments.Circulation.dressing.hemostatic == 'True' and patient.Treatments.Circulation.dressing.hemostatic == 'False'):
                    patient.Treatments.Circulation.dressing.hemostatic = data.Treatments.Circulation.dressing.hemostatic
                    patient.store(db)
                    print "Treatment added: Circulation dressing Hemostatic"
                elif (data.Treatments.Circulation.dressing.hemostatic == 'False' and patient.Treatments.Circulation.dressing.hemostatic == 'True'):
                    patient.Treatments.Circulation.dressing.hemostatic = data.Treatments.Circulation.dressing.hemostatic
                    patient.store(db)
                    print "Treatment removed: Circulation dressing Hemostatic"
                #pressure
                if (data.Treatments.Circulation.dressing.pressure == 'True' and patient.Treatments.Circulation.dressing.pressure == 'False'):
                    patient.Treatments.Circulation.dressing.pressure = data.Treatments.Circulation.dressing.pressure
                    patient.store(db)
                    print "Treatment added: Circulation dressing Pressure"
                elif (data.Treatments.Circulation.dressing.pressure == 'False' and patient.Treatments.Circulation.dressing.pressure == 'True'):
                    patient.Treatments.Circulation.dressing.pressure = data.Treatments.Circulation.dressing.pressure
                    patient.store(db)
                    print "Treatment removed: Circulation dressing Pressure"
                #other
                if (data.Treatments.Circulation.dressing.other == 'True' and patient.Treatments.Circulation.dressing.other == 'False'):
                    patient.Treatments.Circulation.dressing.other = data.Treatments.Circulation.dressing.other
                    patient.Treatments.Circulation.dressing.otherType = data.Treatments.Circulation.dressing.otherType
                    patient.store(db)
                    print "Treatment added: Circulation dressing Other: " + patient.Treatments.Circulation.dressing.otherType
                elif (data.Treatments.Circulation.dressing.other == 'False' and patient.Treatments.Circulation.dressing.other == 'True'):
                    patient.Treatments.Circulation.dressing.other = data.Treatments.Circulation.dressing.other
                    patient.store(db)
                    print "Treatment removed: Circulation dressing Other: " + patient.Treatments.Circulation.dressing.otherType
                elif (data.Treatments.Circulation.dressing.other == patient.Treatments.Circulation.dressing.other == 'True'):
                    if data.Treatments.Circulation.dressing.otherType != patient.Treatments.Circulation.dressing.otherType:
                        patient.Treatments.Circulation.dressing.otherType = data.Treatments.Circulation.dressing.otherType
                        print "Treatment changed: Circulation dressing Other: " + patient.Treatments.Circulation.dressing.otherType
            if hasattr(data.Treatments.Circulation, 'TQ'):
                #extremity
                if (data.Treatments.Circulation.TQ.extremity == 'True' and patient.Treatments.Circulation.TQ.extremity == 'False'):
                    patient.Treatments.Circulation.TQ.extremity = data.Treatments.Circulation.TQ.extremity
                    patient.store(db)
                    print "Treatment added: Circulation TQ Extremity"
                elif (data.Treatments.Circulation.TQ.extremity == 'False' and patient.Treatments.Circulation.TQ.extremity == 'True'):
                    patient.Treatments.Circulation.TQ.extremity = data.Treatments.Circulation.TQ.extremity
                    patient.store(db)
                    print "Treatment removed: Circulation TQ Extremity"
                #junctional
                if (data.Treatments.Circulation.TQ.junctional == 'True' and patient.Treatments.Circulation.TQ.junctional == 'False'):
                    patient.Treatments.Circulation.TQ.junctional = data.Treatments.Circulation.TQ.junctional
                    patient.store(db)
                    print "Treatment added: Circulation TQ Junctional"
                elif (data.Treatments.Circulation.TQ.junctional == 'False' and patient.Treatments.Circulation.TQ.junctional == 'True'):
                    patient.Treatments.Circulation.TQ.junctional = data.Treatments.Circulation.TQ.junctional
                    patient.store(db)
                    print "Treatment removed: Circulation TQ Junctional"
                #truncal
                if (data.Treatments.Circulation.TQ.truncal == 'True' and patient.Treatments.Circulation.TQ.truncal == 'False'):
                    patient.Treatments.Circulation.TQ.truncal = data.Treatments.Circulation.TQ.truncal
                    patient.store(db)
                    print "Treatment added: Circulation TQ Truncal"
                elif (data.Treatments.Circulation.TQ.truncal == 'False' and patient.Treatments.Circulation.TQ.truncal == 'True'):
                    patient.Treatments.Circulation.TQ.truncal = data.Treatments.Circulation.TQ.truncal
                    patient.store(db)
                    print "Treatment removed: Circulation TQ Truncal"
        #Blood Products
        if hasattr(data.Treatments, 'BloodProducts'):
            logList = list()
            needsSave = 'false'
            #whole blood
            if patient.Treatments.BloodProducts.wholeBlood.Dose != data.Treatments.BloodProducts.wholeBlood.Dose and data.Treatments.BloodProducts.wholeBlood.Dose != None:
                patient.Treatments.BloodProducts.wholeBlood.Dose = data.Treatments.BloodProducts.wholeBlood.Dose
                logitem = "Blood Product Whole Blood Dose: " + patient.Treatments.BloodProducts.wholeBlood.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.wholeBlood.Route != data.Treatments.BloodProducts.wholeBlood.Route and data.Treatments.BloodProducts.wholeBlood.Route != None:
                patient.Treatments.BloodProducts.wholeBlood.Route = data.Treatments.BloodProducts.wholeBlood.Route
                logitem = "Blood Product Whole Blood Route: " + patient.Treatments.BloodProducts.wholeBlood.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.wholeBlood.Time != data.Treatments.BloodProducts.wholeBlood.Time and data.Treatments.BloodProducts.wholeBlood.Time != None:
                patient.Treatments.BloodProducts.wholeBlood.Time = data.Treatments.BloodProducts.wholeBlood.Time
                logitem = "Blood Product Whole Blood Time: " + patient.Treatments.BloodProducts.wholeBlood.Time
                logList.append(logitem)
                needsSave = 'true'
                
            #packedRedBlood
            if patient.Treatments.BloodProducts.packedRedBlood.Dose != data.Treatments.BloodProducts.packedRedBlood.Dose and data.Treatments.BloodProducts.packedRedBlood.Dose != None:
                patient.Treatments.BloodProducts.packedRedBlood.Dose = data.Treatments.BloodProducts.packedRedBlood.Dose
                logitem = "Blood Product Packed Red Blood Dose: " + patient.Treatments.BloodProducts.packedRedBlood.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.packedRedBlood.Route != data.Treatments.BloodProducts.packedRedBlood.Route and data.Treatments.BloodProducts.packedRedBlood.Route != None:
                patient.Treatments.BloodProducts.packedRedBlood.Route = data.Treatments.BloodProducts.packedRedBlood.Route
                logitem = "Blood Product Packed Red Blood Route: " + patient.Treatments.BloodProducts.packedRedBlood.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.packedRedBlood.Time != data.Treatments.BloodProducts.packedRedBlood.Time and data.Treatments.BloodProducts.packedRedBlood.Time != None:
                patient.Treatments.BloodProducts.packedRedBlood.Time = data.Treatments.BloodProducts.packedRedBlood.Time
                logitem = "Blood Product Packed Red Blood Time: " + patient.Treatments.BloodProducts.packedRedBlood.Time
                logList.append(logitem)
                needsSave = 'true'

            #plateleteRichPlasma
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Dose != data.Treatments.BloodProducts.plateleteRichPlasma.Dose and data.Treatments.BloodProducts.plateleteRichPlasma.Dose != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Dose = data.Treatments.BloodProducts.plateleteRichPlasma.Dose
                logitem = "Blood Product Platelete Rich Plasma Dose: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Route != data.Treatments.BloodProducts.plateleteRichPlasma.Route and data.Treatments.BloodProducts.plateleteRichPlasma.Route != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Route = data.Treatments.BloodProducts.plateleteRichPlasma.Route
                logitem = "Blood Product Platelete Rich Plasma Route: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Time != data.Treatments.BloodProducts.plateleteRichPlasma.Time and data.Treatments.BloodProducts.plateleteRichPlasma.Time != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Time = data.Treatments.BloodProducts.plateleteRichPlasma.Time
                logitem = "Blood Product Platelete Rich Plasma Time: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Time
                logList.append(logitem)
                needsSave = 'true'

            #plateleteRichPlasma
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Dose != data.Treatments.BloodProducts.plateleteRichPlasma.Dose and data.Treatments.BloodProducts.plateleteRichPlasma.Dose != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Dose = data.Treatments.BloodProducts.plateleteRichPlasma.Dose
                logitem = "Blood Product Platelete Rich Plasma Dose: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Route != data.Treatments.BloodProducts.plateleteRichPlasma.Route and data.Treatments.BloodProducts.plateleteRichPlasma.Route != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Route = data.Treatments.BloodProducts.plateleteRichPlasma.Route
                logitem = "Blood Product Platelete Rich Plasma Route: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plateleteRichPlasma.Time != data.Treatments.BloodProducts.plateleteRichPlasma.Time and data.Treatments.BloodProducts.plateleteRichPlasma.Time != None:
                patient.Treatments.BloodProducts.plateleteRichPlasma.Time = data.Treatments.BloodProducts.plateleteRichPlasma.Time
                logitem = "Blood Product Platelete Rich Plasma Time: " + patient.Treatments.BloodProducts.plateleteRichPlasma.Time
                logList.append(logitem)
                needsSave = 'true'

            #wholePlasma
            if patient.Treatments.BloodProducts.wholePlasma.Dose != data.Treatments.BloodProducts.wholePlasma.Dose and data.Treatments.BloodProducts.wholePlasma.Dose != None:
                patient.Treatments.BloodProducts.wholePlasma.Dose = data.Treatments.BloodProducts.wholePlasma.Dose
                logitem = "Blood Product Whole Plasma Dose: " + patient.Treatments.BloodProducts.wholePlasma.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.wholePlasma.Route != data.Treatments.BloodProducts.wholePlasma.Route and data.Treatments.BloodProducts.wholePlasma.Route != None:
                patient.Treatments.BloodProducts.wholePlasma.Route = data.Treatments.BloodProducts.wholePlasma.Route
                logitem = "Blood Product Whole Plasma Route: " + patient.Treatments.BloodProducts.wholePlasma.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.wholePlasma.Time != data.Treatments.BloodProducts.wholePlasma.Time and data.Treatments.BloodProducts.wholePlasma.Time != None:
                patient.Treatments.BloodProducts.wholePlasma.Time = data.Treatments.BloodProducts.wholePlasma.Time
                logitem = "Blood Product Whole Plasma Time: " + patient.Treatments.BloodProducts.wholePlasma.Time
                logList.append(logitem)
                needsSave = 'true'

            #cryoprecipitate
            if patient.Treatments.BloodProducts.cryoprecipitate.Dose != data.Treatments.BloodProducts.cryoprecipitate.Dose and data.Treatments.BloodProducts.cryoprecipitate.Dose != None:
                patient.Treatments.BloodProducts.cryoprecipitate.Dose = data.Treatments.BloodProducts.cryoprecipitate.Dose
                logitem = "Blood Product Cryoprecipitate Plasma Dose: " + patient.Treatments.BloodProducts.cryoprecipitate.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.cryoprecipitate.Route != data.Treatments.BloodProducts.cryoprecipitate.Route and data.Treatments.BloodProducts.cryoprecipitate.Route != None:
                patient.Treatments.BloodProducts.cryoprecipitate.Route = data.Treatments.BloodProducts.cryoprecipitate.Route
                logitem = "Blood Product Cryoprecipitate Plasma Route: " + patient.Treatments.BloodProducts.cryoprecipitate.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.cryoprecipitate.Time != data.Treatments.BloodProducts.cryoprecipitate.Time and data.Treatments.BloodProducts.cryoprecipitate.Time != None:
                patient.Treatments.BloodProducts.cryoprecipitate.Time = data.Treatments.BloodProducts.cryoprecipitate.Time
                logitem = "Blood Product Cryoprecipitate Plasma Time: " + patient.Treatments.BloodProducts.cryoprecipitate.Time
                logList.append(logitem)
                needsSave = 'true'

            #serumAlbum
            if patient.Treatments.BloodProducts.serumAlbum.Dose != data.Treatments.BloodProducts.serumAlbum.Dose and data.Treatments.BloodProducts.serumAlbum.Dose != None:
                patient.Treatments.BloodProducts.serumAlbum.Dose = data.Treatments.BloodProducts.serumAlbum.Dose
                logitem = "Blood Product Serum Albumin Dose: " + patient.Treatments.BloodProducts.serumAlbum.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.serumAlbum.Route != data.Treatments.BloodProducts.serumAlbum.Route and data.Treatments.BloodProducts.serumAlbum.Route != None:
                patient.Treatments.BloodProducts.serumAlbum.Route = data.Treatments.BloodProducts.serumAlbum.Route
                logitem = "Blood Product Serum Albumin Route: " + patient.Treatments.BloodProducts.serumAlbum.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.serumAlbum.Time != data.Treatments.BloodProducts.serumAlbum.Time and data.Treatments.BloodProducts.serumAlbum.Time != None:
                patient.Treatments.BloodProducts.serumAlbum.Time = data.Treatments.BloodProducts.serumAlbum.Time
                logitem = "Blood Product Serum Albumin Time: " + patient.Treatments.BloodProducts.serumAlbum.Time
                logList.append(logitem)
                needsSave = 'true'

            #plasmanate
            if patient.Treatments.BloodProducts.plasmanate.Dose != data.Treatments.BloodProducts.plasmanate.Dose and data.Treatments.BloodProducts.plasmanate.Dose != None:
                patient.Treatments.BloodProducts.plasmanate.Dose = data.Treatments.BloodProducts.plasmanate.Dose
                logitem = "Blood Product Plasmanate Dose: " + patient.Treatments.BloodProducts.plasmanate.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plasmanate.Route != data.Treatments.BloodProducts.plasmanate.Route and data.Treatments.BloodProducts.plasmanate.Route != None:
                patient.Treatments.BloodProducts.plasmanate.Route = data.Treatments.BloodProducts.plasmanate.Route
                logitem = "Blood Product Plasmanate Route: " + patient.Treatments.BloodProducts.plasmanate.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.plasmanate.Time != data.Treatments.BloodProducts.plasmanate.Time and data.Treatments.BloodProducts.plasmanate.Time != None:
                patient.Treatments.BloodProducts.plasmanate.Time = data.Treatments.BloodProducts.plasmanate.Time
                logitem = "Blood Product Plasmanate Time: " + patient.Treatments.BloodProducts.plasmanate.Time
                logList.append(logitem)
                needsSave = 'true'

            #hextend
            if patient.Treatments.BloodProducts.hextend.Dose != data.Treatments.BloodProducts.hextend.Dose and data.Treatments.BloodProducts.hextend.Dose != None:
                patient.Treatments.BloodProducts.hextend.Dose = data.Treatments.BloodProducts.hextend.Dose
                logitem = "Blood Product Hextend Dose: " + patient.Treatments.BloodProducts.hextend.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.hextend.Route != data.Treatments.BloodProducts.hextend.Route and data.Treatments.BloodProducts.hextend.Route != None:
                patient.Treatments.BloodProducts.hextend.Route = data.Treatments.BloodProducts.hextend.Route
                logitem = "Blood Product Hextend Route: " + patient.Treatments.BloodProducts.hextend.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.hextend.Time != data.Treatments.BloodProducts.hextend.Time and data.Treatments.BloodProducts.hextend.Time != None:
                patient.Treatments.BloodProducts.hextend.Time = data.Treatments.BloodProducts.hextend.Time
                logitem = "Blood Product Hextend Time: " + patient.Treatments.BloodProducts.hextend.Time
                logList.append(logitem)
                needsSave = 'true'

            #other
            if patient.Treatments.BloodProducts.other.Type != data.Treatments.BloodProducts.other.Type and data.Treatments.BloodProducts.other.Type != None:
                patient.Treatments.BloodProducts.other.Type = data.Treatments.BloodProducts.other.Type
                logitem = "Other Blood Product named: " + patient.Treatments.BloodProducts.other.Type
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.other.Dose != data.Treatments.BloodProducts.other.Dose and data.Treatments.BloodProducts.other.Dose != None:
                patient.Treatments.BloodProducts.other.Dose = data.Treatments.BloodProducts.other.Dose
                logitem = "Blood Product " + patient.Treatments.BloodProducts.other.Type + " Dose: " + patient.Treatments.BloodProducts.other.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.other.Route != data.Treatments.BloodProducts.other.Route and data.Treatments.BloodProducts.other.Route != None:
                patient.Treatments.BloodProducts.other.Route = data.Treatments.BloodProducts.other.Route
                logitem = "Blood Product " + patient.Treatments.BloodProducts.other.Type + " Route: " + patient.Treatments.BloodProducts.other.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.BloodProducts.other.Time != data.Treatments.BloodProducts.other.Time and data.Treatments.BloodProducts.other.Time != None:
                patient.Treatments.BloodProducts.other.Time = data.Treatments.BloodProducts.other.Time
                logitem = "Blood Product " + patient.Treatments.BloodProducts.other.Type + " Time: " + patient.Treatments.BloodProducts.other.Time
                logList.append(logitem)
                needsSave = 'true'
                
            #Save data if needed
            if needsSave == "true":
                patient.store(db)
                for item in logList:
                    print item
                    
        #Fluids
        if hasattr(data.Treatments, 'Fluids'):
            logList = list()
            needsSave = 'false'
            #normalSaline
            if patient.Treatments.Fluids.normalSaline.Dose != data.Treatments.Fluids.normalSaline.Dose and data.Treatments.Fluids.normalSaline.Dose != None:
                patient.Treatments.Fluids.normalSaline.Dose = data.Treatments.Fluids.normalSaline.Dose
                logitem = "Fluids Normal Saline Dose: " + patient.Treatments.Fluids.normalSaline.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.normalSaline.Route != data.Treatments.Fluids.normalSaline.Route and data.Treatments.Fluids.normalSaline.Route != None:
                patient.Treatments.Fluids.normalSaline.Route = data.Treatments.Fluids.normalSaline.Route
                logitem = "Fluids Normal Saline Route: " + patient.Treatments.Fluids.normalSaline.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.normalSaline.Time != data.Treatments.Fluids.normalSaline.Time and data.Treatments.Fluids.normalSaline.Time != None:
                patient.Treatments.Fluids.normalSaline.Time = data.Treatments.Fluids.normalSaline.Time
                logitem = "Fluids Normal Saline Time: " + patient.Treatments.Fluids.normalSaline.Time
                logList.append(logitem)
                needsSave = 'true'
                
            #lactactedRingers
            if patient.Treatments.Fluids.lactactedRingers.Dose != data.Treatments.Fluids.lactactedRingers.Dose and data.Treatments.Fluids.lactactedRingers.Dose != None:
                patient.Treatments.Fluids.lactactedRingers.Dose = data.Treatments.Fluids.lactactedRingers.Dose
                logitem = "Fluids Lactacted Ringers Dose: " + patient.Treatments.Fluids.lactactedRingers.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.lactactedRingers.Route != data.Treatments.Fluids.lactactedRingers.Route and data.Treatments.Fluids.lactactedRingers.Route != None:
                patient.Treatments.Fluids.lactactedRingers.Route = data.Treatments.Fluids.lactactedRingers.Route
                logitem = "Fluids Lactacted Ringers Route: " + patient.Treatments.Fluids.lactactedRingers.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.lactactedRingers.Time != data.Treatments.Fluids.lactactedRingers.Time and data.Treatments.Fluids.lactactedRingers.Time != None:
                patient.Treatments.Fluids.lactactedRingers.Time = data.Treatments.Fluids.lactactedRingers.Time
                logitem = "Fluids Lactacted Ringers Time: " + patient.Treatments.Fluids.lactactedRingers.Time
                logList.append(logitem)
                needsSave = 'true'

            #dextrose
            if patient.Treatments.Fluids.dextrose.Dose != data.Treatments.Fluids.dextrose.Dose and data.Treatments.Fluids.dextrose.Dose != None:
                patient.Treatments.Fluids.dextrose.Dose = data.Treatments.Fluids.dextrose.Dose
                logitem = "Fluids Dextrose Dose: " + patient.Treatments.Fluids.dextrose.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.dextrose.Route != data.Treatments.Fluids.dextrose.Route and data.Treatments.Fluids.dextrose.Route != None:
                patient.Treatments.Fluids.dextrose.Route = data.Treatments.Fluids.dextrose.Route
                logitem = "Fluids Dextrose Route: " + patient.Treatments.Fluids.dextrose.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.dextrose.Time != data.Treatments.Fluids.dextrose.Time and data.Treatments.Fluids.dextrose.Time != None:
                patient.Treatments.Fluids.dextrose.Time = data.Treatments.Fluids.dextrose.Time
                logitem = "Fluids Dextrose Time: " + patient.Treatments.Fluids.dextrose.Time
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.dextrose.Percentage != data.Treatments.Fluids.dextrose.Percentage and data.Treatments.Fluids.dextrose.Percentage != None:
                patient.Treatments.Fluids.dextrose.Percentage = data.Treatments.Fluids.dextrose.Percentage
                logitem = "Fluids Dextrose Percentage: " + patient.Treatments.Fluids.dextrose.Percentage
                logList.append(logitem)
                needsSave = 'true'

            #other
            if patient.Treatments.Fluids.other.Type != data.Treatments.Fluids.other.Type and data.Treatments.Fluids.other.Type != None:
                patient.Treatments.Fluids.other.Type = data.Treatments.Fluids.other.Type
                logitem = "Other Fluids named: " + patient.Treatments.Fluids.other.Type
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.other.Dose != data.Treatments.Fluids.other.Dose and data.Treatments.Fluids.other.Dose != None:
                patient.Treatments.Fluids.other.Dose = data.Treatments.Fluids.other.Dose
                logitem = "Fluids " + patient.Treatments.Fluids.other.Type + " Dose: " + patient.Treatments.Fluids.other.Dose
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.other.Route != data.Treatments.Fluids.other.Route and data.Treatments.Fluids.other.Route != None:
                patient.Treatments.Fluids.other.Route = data.Treatments.Fluids.other.Route
                logitem = "Fluids " + patient.Treatments.Fluids.other.Type + " Route: " + patient.Treatments.Fluids.other.Route
                logList.append(logitem)
                needsSave = 'true'
            if patient.Treatments.Fluids.other.Time != data.Treatments.Fluids.other.Time and data.Treatments.Fluids.other.Time != None:
                patient.Treatments.Fluids.other.Time = data.Treatments.Fluids.other.Time
                logitem = "Fluids " + patient.Treatments.Fluids.other.Type + " Time: " + patient.Treatments.Fluids.other.Time
                logList.append(logitem)
                needsSave = 'true'

            #Save data if needed
            if needsSave == "true":
                patient.store(db)
                for item in logList:
                    print item

    #Medications
    if hasattr(data, 'Medications'):
        stop = 'False'
        for med in patient.Medications:
            if med.Name == data.Medications.Name:
                if data.Medications.Dose != None:
                   med.Dose = data.Medications.Dose
                if data.Medications.Units != None:
                    med.Units = data.Medications.Units
                if data.Medications.Route != None:
                    med.Route = data.Medications.Route
                if data.Medications.Time != None:
                    med.Time = data.Medications.Time
                if data.Medications.Analgesic != None:
                    med.Analgesic = data.Medications.Analgesic
                if data.Medications.Antibiotic != None:
                    med.Antibiotic = data.Medications.Antibiotic
                if data.Medications.Other != None:
                    med.Other = data.Medications.Other
                   
               #obj = {'Name': data.Medications.Name,
               #    'Dose': data.Medications.Dose,
               #    'Units': data.Medications.Units,
               #    'Route': data.Medications.Route,
               #    'Time': data.Medications.Time,
               #    'Analgesic': data.Medications.Analgesic,
               #    'Antibiotic': data.Medications.Antibiotic,
               #    'Other':data.Medications.Other
               #    }
               #patient.Medications.remove(med)
               #patient.Medications.append(obj)
                #med = data.Medications
                patient.store(db)
                stop = 'True'
                print "Edited Medication "  + med.Name
        if stop == 'False':
            obj = {'Name': data.Medications.Name,
                   'Dose': data.Medications.Dose,
                   'Units': data.Medications.Units,
                   'Route': data.Medications.Route,
                   'Time': data.Medications.Time,
                   'Analgesic': data.Medications.Analgesic,
                   'Antibiotic': data.Medications.Antibiotic,
                   'Other':data.Medications.Other
                   }
            patient.Medications.append(obj)
            patient.store(db)
            print "Added Medication " + data.Medications.Name
    #Medications list (if needed)
    if hasattr(data,'MedicationsList'):
        #remove all the other meds
        patient.Medications = [item for item in patient.Medications if item.Other != 'True']
        for med in data.MedicationsList:
            if med.Other == 'True':
                obj = {'Name': med.Name,
                   'Dose': med.Dose,
                   'Units': med.Units,
                   'Route': med.Route,
                   'Time': med.Time,
                   'Analgesic': med.Analgesic,
                   'Antibiotic': med.Antibiotic,
                   'Other':med.Other
                   }
                patient.Medications.append(obj)
                patient.store(db)
                print "Modified Other Med " + med.Name + " with Dose " + med.Dose + med.Units + " with Route " + med.Route

    #Notes
    if hasattr(data, 'Notes'):
        patient.Notes = data.Notes
        patient.store(db)
        print "Note has been set at " + patient.Notes

    #Other tretments
    if hasattr(data, 'OtherTreatments'):
        patient.OtherTreatments.CombatPillPack = data.OtherTreatments.CombatPillPack
        patient.OtherTreatments.EyeShieldR = data.OtherTreatments.EyeShieldR
        patient.OtherTreatments.EyeShieldL = data.OtherTreatments.EyeShieldL
        patient.OtherTreatments.Splint = data.OtherTreatments.Splint
        patient.OtherTreatments.HypothermiaPrevention = data.OtherTreatments.HypothermiaPrevention
        patient.store(db)
        print "Other treatments altered"

    #TQS
    if hasattr(data, 'TQList'):
        for tq in data.TQList:
            isHere = 'False'
            for patientTQ in patient.TQList:
                if patientTQ.Location == tq.Location:
                    patientTQ.Time = tq.Time
                    patientTQ.Type = tq.Type
                    isHere = 'True'
                    patient.store(db)
                    print "Edited TQ " + tq.Type + " at " + tq.Location + " with time " + tq.Time
            if isHere == 'False':
                obj = {'Type': tq.Type,
                       'Time': tq.Time,
                       'Location': tq.Location
                       }
                patient.TQList.append(obj)
                patient.store(db)
                print "Added TQ " + tq.Type + " at " + tq.Location + " with time " + tq.Time
                
    #Global PCD list from ap
    if hasattr(data, 'PCDIDS'):
        print data
        print patient
        del patient.AllPCDs [:]
        for pcd in data.PCDIDS:
            obj = {'ID': pcd.ID}
            patient.AllPCDs.append(obj)
        #print patient.AllPCDs
        patient.store(db)
        print "Updated global PCD list for " + data.ID
        print "Current global PCDs:"
        for pcdID in patient.AllPCDs:
            print pcdID.ID

    #Pat PCD list from GUI
    if hasattr(data, 'PatPCDs'):
        del patient.PatPCDs [:]
        for pcd in data.PatPCDs:
            obj = {'ID': pcd}
            patient.PatPCDs.append(obj)
        #print patient.PatPCDs
        patient.store(db)
        print "Updated patient PCD list"
        print "Attached PCD's to patient:"
        for pcdID in patient.PatPCDs:
            print pcdID.ID
        
        
  
        
    #Test code
    #print patient.Admin.FirstName
    #print patient.Allergies
    #print patient.InjuryList.Injuries
    #patient.InjuryList.Injuries.append(Type='MVC', Location='postNeck')
    #patient.store(db)

    
# If you want to use a specific client id, use
# mqttc = mosquitto.Mosquitto("client-id")
# but note that the client id must be unique on the broker. Leaving the client
# id parameter empty will generate a random id for you.
#global mqttc
#mqttc = mosquitto.Mosquitto()
#start out by immediately checking for the medics database
checkForMedicsDB()

#now set up all the mqtt shit
mqttc.on_message = on_message
mqttc.on_connect = on_connect
mqttc.on_publish = on_publish
mqttc.on_subscribe = on_subscribe
# Uncomment to enable debug messages
#mqttc.on_log = on_log
mqttc.connect("127.0.0.1", 1883, 60)

#mqttc.subscribe("string", 0)
#mqttc.subscribe(("tuple", 1))
#mqttc.subscribe([("list0", 0), ("list1", 1)])

mqttc.loop_forever()

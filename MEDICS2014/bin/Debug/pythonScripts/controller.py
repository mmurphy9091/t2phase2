import mosquitto
import time
import threading
import json
import time
import socket
import requests

from threading import Thread, Event
from random import randint
from collections import namedtuple

class Patient:
        def __init__(self, ID):
                self.ID = ID
                self.pcds = list()

patientList = list()    
mqttcAP = mosquitto.Mosquitto()
mqttcUD = mosquitto.Mosquitto()
apIP = "10.8.3.165"
patID = ""
myDBmessage = ""
myIP = ""
theirIPs = list()
idsAndPCDsCommand = 'http://127.0.0.1:5984/medics/_design/patient/_view/IDsAndPCDs'

class MyThread(Thread):
	def __init__(self, event):
		Thread.__init__(self)
		self.stopped = event
	
	def run(self):
		mqttcUD.on_message = on_messageUD
		mqttcUD.on_connect = on_connectUD
		mqttcUD.on_publish = on_publishUD
		mqttcUD.on_subscribe = on_subscribeUD
		mqttcUD.connect('127.0.0.1', 1883, 60)
		mqttcUD.loop_forever()

#sends vitals info to database
#
#
def sendVitalsMessage(vitals):
    global patID
    global patientList
    data = json.loads(vitals, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
    #make sure this vitals message is connected to a patient
    if hasattr(data, 'Vitals'):
        if hasattr(data.Vitals, 'PCDID'):
            for p in patientList:
                for pcd in p.pcds:
                    if data.Vitals.PCDID == pcd:
                        sendMessage = vitals[:1] + '"ID": "' + str(p.ID) + '",' + vitals[1:]
                        print "Adding vitals to patient " + p.ID
                        print sendMessage
                        mqttcUD.publish('medics/data/6',sendMessage,1)
                        #print sendMessage
                        if p.ID == patID:
                            dataMessage = "allByID:" + str(p.ID)
                            print 'Updating patient ' + p.ID
                            mqttcUD.publish('medics/data/5',dataMessage,1)
                            #print dataMessage

#AP connection functions
def on_connectAP(mosq, obj, rc):
    global myIP
    mosq.subscribe("ap/data/#", 0)
    print("rc: "+str(rc))
    #Grab IP address and send to other UD's
    
    message = 'Alive:' + myIP
    mqttcAP.publish('ap/data/3',message,1)

def on_messageAP(mosq, obj, msg):
    global myDBmessage
    global patID
    global theirIPs
    global myIP
    #if it is a vitals message
    if str(msg.topic) == "ap/data/1":
        if 'Vitals' in msg.payload:
            sendVitalsMessage(msg.payload)
    #if it is a database update
    if str(msg.topic) == "ap/data/2":
        if msg.payload != myDBmessage:
            #dataMessage = "allByID:" + patID
            #print 'Updating patient ' + patID
            #mqttcUD.publish('medics/data/5',dataMessage,1)
            #check for the id
            #data = json.loads(msg.payload, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
            if patID in str(msg.payload):
                message = msg.payload.replace(patID, '')
                message = message.replace('"ID":"",','')
                #message = message[1:]
                message = message[:-1]
                print "Recieved message from other node: " + message
                print "Updating current patient"
                mqttcUD.publish('medics/data/2',message,1)
    #if it is a newly added or crashed UD
    if str(msg.topic) == "ap/data/3":
        if 'Alive:' in msg.payload:
            theirIP = msg.payload.replace('Alive:', '')
            if theirIP != myIP and theirIP not in theirIPs:
                theirIPs.append(theirIP)
                replicateMessage = 'Replicate:'+theirIP
                mqttcUD.publish('medics/data/10',replicateMessage,1)
                print 'Replicating to IP ' + theirIP + '\n'
                message = 'Alive:' + myIP
                mqttcAP.publish('ap/data/3',message,1)
                        
        if 'Dead:' in msg.payload:
            #print theirIPs
            theirIP = msg.payload.replace('Dead:', '')
            #print theirIP
            if theirIP in theirIPs:
                theirIPs.remove(theirIP)
                print 'Removed dead IP ' + theirIP
                #possible algorythm to stop replication here
                killMessage = 'Kill:'+theirIP
                mqttcUD.publish('medics/data/10',killMessage,1)
                print 'Killing replication to IP ' + theirIP + '\n'

        if msg.payload == 'Done':
            #send out another message stating to the new UD to replicate here
            replicateMessage = 'Alive:'+myIP
            mqttcAP.publish('ap/data/3',replicateMessage,1)
    #If this is a PCDID return (can be either by call or automatically
    if str(msg.topic) == "ap/data/5":
        if 'PCDIDS' in msg.payload:
            parseAPpcdIDS(msg.payload)
        

def on_publishAP(mosq, obj, mid):
    print("mid: "+str(mid))

def on_subscribeAP(mosq, obj, mid, granted_qos):
    print("Subscribed: "+str(mid)+" "+str(granted_qos))

def on_logAP(mosq, obj, level, string):
    print(string)

#UD connection functions
def on_connectUD(mosq, obj, rc):
    mosq.subscribe("medics/data/#", 1)
    print("rc: "+str(rc))
    #grab a list of patients and their pcds
    print 'Retrieving patient data from DB'
    getPCDsAndIDs()
    print 'Retrieving PCDIDs from AP'
    mqttcAP.publish('ap/data/4', "PCD CALL", 1)

def on_messageUD(mosq, obj, msg):
    global patID
    global patientList
    global myDBmessage
    #Check if it's a delete command
    if str(msg.topic) == "medics/data/3":
            #Make sure it is a delete command
            message = str(msg.payload)
            if "DELETE:" in message:
                    #Broadcast over same channel that refreshes all nodes
                    mqttcAP.publish('ap/data/2',msg.payload)
    #Check if it's an update of the database
    if str(msg.topic) == "medics/data/6":
            #Wait a few seconds for couch to replicate
            #time.sleep(3)
            #Carry the message to the AP
            myDBmessage = msg.payload
            if "PCDIDS" not in myDBmessage:
                    mqttcAP.publish('ap/data/2',msg.payload)
                    print 'Message sent to AP'
                    #Parse post message for data if needed
                    parsePostMessage(msg.payload)
    #Check if it's a grab for medics data
    if str(msg.topic) == "medics/data/9":
        #if msg.payload == patListURL:
        #    getPatientList()
        #check if it's a call for a new patient
        if "NEWPATIENT:" in msg.payload:
            message = msg.payload.replace('NEWPATIENT:', '')
            patID = message
            new = Patient(patID)
            patientList.append(new)
            print 'Current patient: ' + message
            #Retrieve and update PCDs
            mqttcAP.publish('ap/data/4', "PCD CALL", 1)
        elif "PATID:" in msg.payload:
            message = msg.payload.replace('PATID:', '')
            patID = message
            print 'Current patient: ' + message
    #Check if it's a return of patid's and pcd data
    #if str(msg.topic) == 'medics/data/12':
    #    #Send it to the id and pcd parser
    #    parsePCDMessage(msg.payload)

def on_publishUD(mosq, obj, mid):
    print("mid: "+str(mid))

def on_subscribeUD(mosq, obj, mid, granted_qos):
    print("Subscribed: "+str(mid)+" "+str(granted_qos))

def on_logUD(mosq, obj, level, string):
    print(string)


#Parses POST message and calls correct method based on data
#
#
#
def parseAPpcdIDS(message):
    global patientList
    #set the message as an object
    data = json.loads(message, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
    if hasattr(data, 'PCDIDS'):
        #Just carry the message to the database for each patient
        for patient in patientList:
            postMessage = message[:1] + '"ID": "' + str(patient.ID) + '",' + message[1:]
            #print postMessage
            mqttcUD.publish("medics/data/6", postMessage, 1)
                


        
#Parses POST message and calls correct method based on data
#
#
#
def parsePostMessage(message):
    #Set up the Patient
    global patientList
    
    #set the message as an object
    data = json.loads(message, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
    #select the right patient
    if hasattr(data, 'PatPCDs'):
            for p in patientList:
                if p.ID == data.ID:
                    #Pat PCD list from GUI
                    if hasattr(data, 'PatPCDs'):
                        del p.pcds [:]
                        for pcd in data.PatPCDs:
                            p.pcds.append(pcd)
                        print "Updated patient PCD list"
                        print "Attached PCD's to patient " + p.ID + ":"
                        for pcdID in p.pcds:
                            print pcdID

#Parses PCD and ID message and sets them to the correct globals
#
#
#
def getPCDsAndIDs():
    global patientList
    response = requests.get(idsAndPCDsCommand)
    message = response.content
    #set the message as an object
    data = json.loads(message, object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
    #do a check to make sure we have the right command
    if hasattr(data, 'rows'):
        for row in data.rows:
            new = Patient(row.id)
            for pcd in row.value.PCDs:
                new.pcds.append(pcd.ID)
            patientList.append(new)
            print 'Added patient: ' + new.ID
            print 'With PCDs: '
            for pcd in new.pcds:
                    print pcd

# If you want to use a specific client id, use
# mqttc = mosquitto.Mosquitto("client-id")
# but note that the client id must be unique on the broker. Leaving the client
# id parameter empty will generate a random id for you.

mqttcAP.on_message = on_messageAP
mqttcAP.on_connect = on_connectAP
mqttcAP.on_publish = on_publishAP
mqttcAP.on_subscribe = on_subscribeAP



# Uncomment to enable debug messages
#mqttc.on_log = on_log
myIP = socket.gethostbyname(socket.gethostname())
mqttcAP.will_set('ap/data/3', payload='Dead:'+myIP, qos=1)
mqttcAP.connect(apIP, 1883, 60)


#mqttc.subscribe("string", 0)
#mqttc.subscribe(("tuple", 1))
#mqttc.subscribe([("list0", 0), ("list1", 1)])


stopFlag = Event()
thread = MyThread(stopFlag)
thread.daemon = True
thread.start()

mqttcAP.loop_forever()


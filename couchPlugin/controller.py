import mosquitto
import time
import threading
import json

from threading import Thread, Event
from random import randint

mqttcAP = mosquitto.Mosquitto()
mqttcUD = mosquitto.Mosquitto()
apIP = "10.8.3.103"
patID = ""

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
    sendMessage = vitals[:1] + '"ID": "' + patID + '",' + vitals[1:]
    print "Adding vitals to patient " + patID
    mqttcUD.publish('medics/data/6',sendMessage,1)
    #print sendMessage
    dataMessage = "allByID:" + patID
    print 'Updating patient ' + patID
    mqttcUD.publish('medics/data/5',dataMessage,1)
    #print dataMessage

#AP connection functions
def on_connectAP(mosq, obj, rc):
    mosq.subscribe("ap/data/1", 0)
    print("rc: "+str(rc))

def on_messageAP(mosq, obj, msg):
    #if it is a vitals message
    if 'Vitals' in msg.payload:
        sendVitalsMessage(msg.payload)

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

def on_messageUD(mosq, obj, msg):
    global patID
    #Check if it's a grab for medics data
    if str(msg.topic) == "medics/data/9":
        #if msg.payload == patListURL:
        #    getPatientList()
        #check if it's a call for a new patient
        if "NEWPATIENT:" in msg.payload:
            message = msg.payload.replace('NEWPATIENT:', '')
            patID = message
            print 'Current patient: ' + message
        elif "PATID:" in msg.payload:
            message = msg.payload.replace('PATID:', '')
            patID = message
            print 'Current patient: ' + message

def on_publishUD(mosq, obj, mid):
    print("mid: "+str(mid))

def on_subscribeUD(mosq, obj, mid, granted_qos):
    print("Subscribed: "+str(mid)+" "+str(granted_qos))

def on_logUD(mosq, obj, level, string):
    print(string)
    

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
mqttcAP.connect(apIP, 1883, 60)


#mqttc.subscribe("string", 0)
#mqttc.subscribe(("tuple", 1))
#mqttc.subscribe([("list0", 0), ("list1", 1)])


stopFlag = Event()
thread = MyThread(stopFlag)
thread.daemon = True
thread.start()

mqttcAP.loop_forever()


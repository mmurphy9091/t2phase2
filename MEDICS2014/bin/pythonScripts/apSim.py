#!/usr/bin/python

# Copyright (c) 2010,2011 Roger Light <roger@atchoo.org>
# All rights reserved.
# 
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are met:
# 
# 1. Redistributions of source code must retain the above copyright notice,
#   this list of conditions and the following disclaimer.
# 2. Redistributions in binary form must reproduce the above copyright
#   notice, this list of conditions and the following disclaimer in the
#   documentation and/or other materials provided with the distribution.
# 3. Neither the name of mosquitto nor the names of its
#   contributors may be used to endorse or promote products derived from
#   this software without specific prior written permission.
# 
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
# AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
# IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
# ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
# LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
# CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
# SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
# INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
# CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
# ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
# POSSIBILITY OF SUCH DAMAGE.

import mosquitto
import time
import threading
import json
import datetime

from threading import Thread, Event
from random import randint

mqttc = mosquitto.Mosquitto()

class MyThread(Thread):
	def __init__(self, event):
		Thread.__init__(self)
		self.stopped = event
	
	def run(self):
		while not self.stopped.wait(10):
			hr = randint(65,90)
			resp =randint(15,30)
			sp02 = randint(0,50)
			temp = randint(85, 102)
			bpsys = randint(150,190)
			bpdia = randint(50,100) 
			obj1 = {'Vitals':{
					'HR' : str(randint(65,90)),
					'Resp' : str(randint(15,30)),
					'SP02' : str(randint(0,50)),
					'Temp' : str(randint(85, 102)),
					'TempType' : 'F',
					'BPSYS' : str(randint(150,190)),
					'BPDIA' : str(randint(50,100)),
					'vitalsTime' : str(datetime.datetime.now().time()),
					'PCDID':'1'
					}
				}
			obj2 = {'Vitals':{
					'HR' : str(randint(65,90)),
					'Resp' : str(randint(15,30)),
					'SP02' : str(randint(0,50)),
					'Temp' : str(randint(85, 102)),
					'TempType' : 'F',
					'BPSYS' : str(randint(150,190)),
					'BPDIA' : str(randint(50,100)),
					'vitalsTime' : str(datetime.datetime.now().strftime("%H:%M:%S")),
					'PCDID':'2'
					}
				}
			#print json.dumps(obj)
			mqttc.publish("ap/data/1", json.dumps(obj1))
			mqttc.publish("ap/data/1", json.dumps(obj2))

def on_connect(mosq, obj, rc):
    mosq.subscribe("ap/data/#", 0)
    print("rc: "+str(rc))

def on_message(mosq, obj, msg):
    print(msg.topic+" "+str(msg.qos)+" "+str(msg.payload))
    #If this is a UD calling for the PCD list
    if str(msg.topic) == 'ap/data/4':
    	if str(msg.payload) == "PCD CALL":
    		#put some code here for retrieving the pcdids
    		ids = {'PCDIDS':[
    			{'ID':'1'},
    			{'ID':'2'}
    		]
    		}
    		mqttc.publish("ap/data/5", json.dumps(ids))
    

def on_publish(mosq, obj, mid):
    print("mid: "+str(mid))

def on_subscribe(mosq, obj, mid, granted_qos):
    print("Subscribed: "+str(mid)+" "+str(granted_qos))

def on_log(mosq, obj, level, string):
    print(string)
    

# If you want to use a specific client id, use
# mqttc = mosquitto.Mosquitto("client-id")
# but note that the client id must be unique on the broker. Leaving the client
# id parameter empty will generate a random id for you.

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


stopFlag = Event()
thread = MyThread(stopFlag)
thread.daemon = True
thread.start()

mqttc.loop_forever()

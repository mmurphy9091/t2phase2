using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Collections;

using MEDICS2014.MQTT;
using MqttLib;

namespace MEDICS2014.Mqtt
{
    class mqttMain
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        string postTopic = "medics/data/6";
        string dbpatListCommand = "http://127.0.0.1:5984/medics/_design/patient/_view/allPatients";
        string listTopic = "medics/data/9";
        string getAllTopic = "medics/data/5";
        string deleteTopic = "medics/data/3";

        dynamic kill = null;

        IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");

        public mqttMain()
        {
            sender.Connect(true);

            //initialize messaging service
            _messages.HandleMessage += new EventHandler(OnHandleMessage);

            //start the database handler
            DBHandler database = new DBHandler();
            database.startDBHandler();

            mqttMain prog = new mqttMain("TCP://127.0.0.1:1883", "THIS");
            prog.Start();
            kill = prog;
        }

        IMqtt _client;

        public void killMQTT()
        {
            kill.Stop();
            sender.Disconnect();
        }

        mqttMain(string connectionString, string clientId)
        {
            // Instantiate client using MqttClientFactory
            _client = MqttClientFactory.CreateClient(connectionString, clientId);

            // Setup some useful client delegate callbacks
            _client.Connected += new ConnectionDelegate(client_Connected);
            _client.ConnectionLost += new ConnectionDelegate(_client_ConnectionLost);
            _client.PublishArrived += new PublishArrivedDelegate(client_PublishArrived);
        }

        void Start()
        {
            // Connect to broker in 'CleanStart' mode
            //Console.WriteLine("Client connecting\n");
            _client.Connect(true);
        }

        void Stop()
        {
            if (_client.IsConnected)
            {
                //Console.WriteLine("Client disconnecting\n");
                _client.Disconnect();
                //Console.WriteLine("Client disconnected\n");
            }
        }

        void client_Connected(object sender, EventArgs e)
        {
            //Console.WriteLine("Client connected\n");
            RegisterOurSubscriptions();
            //PublishSomething(postTopic, "HELLO");
        }

        void _client_ConnectionLost(object sender, EventArgs e)
        {
            //Console.WriteLine("Client connection lost\n");
        }

        void RegisterOurSubscriptions()
        {
            //Console.WriteLine("Subscribing to medics/data/#\n");
            _client.Subscribe("medics/data/#", QoS.BestEfforts);
        }

        void PublishSomething(string topic, string message)
        {
            //Console.WriteLine("Publishing on mqttdotnet/pubtest\n");
            _client.Publish(topic, message, QoS.BestEfforts, false);
        }

        bool client_PublishArrived(object sender, PublishArrivedArgs e)
        {
            //Console.WriteLine("Received Message");
            //Console.WriteLine("Topic: " + e.Topic);
            //Console.WriteLine("Payload: " + e.Payload);
            //Console.WriteLine();

            //Fix the message for proper consumption
            string message = e.Payload.ToString();
            //Get rid of the beginning of the string, has that stupid '
            if (message.StartsWith("'"))
            {
                int location = message.IndexOf("'");
                if (location >= 0)
                {
                    message = message.Substring(location + 1);
                }
            }
            //Get rid of the very end of the string, same reason
            if (message.EndsWith("'"))
            {
                int trim = message.LastIndexOf("'");

                if (trim >= 0)
                {
                    message = message.Substring(0, trim);
                }
            }

            //Send message to the GUI controller if correct topic number
            if (e.Topic == "medics/data/1")
            {
                
                _messages.AddMessage(message);
            }

            //Parse the VDES message data and send to GUI
            if (e.Topic == "medics/data/2")
            {
                VDESHandler vdes = new VDESHandler();
                patient voiceData = vdes.parseJSON(message);
                voiceData.fromVDES = true;
                voiceData.DBOperation = true;
                _systemMessages.AddMessage(voiceData);
            }

            //Send the message to the JSON parser if a return from the database
            if (e.Topic == "medics/data/4")
            {
                dbJsonInterface.dbJsonParser data = new dbJsonInterface.dbJsonParser();
                patient temp = data.parseJSON(message);
                _messages.AddMessage("CLEAR CONTROL");
                _systemMessages.AddMessage(temp);
            }

            //Send the message to the Patient List parser if a return from the database of thte patlist
            if (e.Topic == "medics/data/8")
            {
                if (message != dbpatListCommand)
                {
                    if (!message.Contains("NEWPATIENT:"))
                    {
                        message += "HEREISPATIENTLIST";
                        _messages.AddMessage(message);
                    }
                }
            }
            return true;
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleDBData(message);
            }
        }

        private void handleDBData(string message)
        {
            //Enter data to be sent to the controller

            //Handle post data
            if (message.Contains("DBPOST:"))
            {
                message = message.Replace("DBPOST:", "");
                //send the data
                //IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(postTopic, message, QoS.BestEfforts, false);
                //sender.Disconnect();
            }
            else if (message == "GETPATLIST")
            {
                //IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(listTopic, dbpatListCommand, QoS.BestEfforts, false);
                //sender.Disconnect();
            }
            else if (message.Contains("PATID:"))
            {
                //update nodes of current patient
                //IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(listTopic, message, QoS.BestEfforts, false);
                //sender.Disconnect();

                //Get the data for the switched patient
                message = message.Replace("PATID", "allByID");
                //sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(getAllTopic, message, QoS.BestEfforts, false);
                //sender.Disconnect();

            }
            else if (message.Contains("NEWPATIENT:"))
            {
                //IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(listTopic, message, QoS.BestEfforts, false);
               // sender.Disconnect();
            }
            else if (message.Contains("DELETE:"))
            {
                //IMqtt sender = MqttClientFactory.CreateClient("TCP://127.0.0.1:1883", "SENDER");
                //sender.Connect(true);
                sender.Publish(deleteTopic, message, QoS.BestEfforts, false);
                //sender.Disconnect();
            }
        }

    }

}

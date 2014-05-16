using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MEDICS2014.controls.injuriesControls;

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for injuries.xaml
    /// </summary>
    public partial class injuriesMain : UserControl
    {
        //Message stuff
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        injuriesFull fullView = new injuriesFull();
        injuriesHead headView = new injuriesHead();
        injuriesLeftArm leftArmFrontView = new injuriesLeftArm();
        injuriesRightArm rightArmFrontView = new injuriesRightArm();
        injuriesRightLeg rightLegFrontView = new injuriesRightLeg();
        injuriesLeftLeg leftLegFrontView = new injuriesLeftLeg();
        injuriesChest chestView = new injuriesChest();
        injuriesAbdomen abdomenView = new injuriesAbdomen();
        injuriesHeadBack headBackView = new injuriesHeadBack();
        injuriesUpperBack upperBackView = new injuriesUpperBack();
        injuriesLowerBack lowerBackView = new injuriesLowerBack();
        rightLegBack rightLegBackView = new rightLegBack();
        leftLegBack leftLegBackView = new leftLegBack();
        rightArmBack rightArmBackView = new rightArmBack();
        leftArmBack leftArmBackView = new leftArmBack();

        

        UserControl currentWindow;

        public injuriesMain()
        {
            InitializeComponent();

            injuriesStackPanel.Children.Add(fullView);

            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleMessageData(message);
            }
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                handleSystemPatientData(Message);
            }

        }

        public void handleMessageData(string message)
        {
            //So MQTT can access the GUI
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "INJURIES MAIN":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(fullView);
                        break;
                    case "INJURIES HEAD FRONT":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(headView);
                        currentWindow = headView;
                        break;
                    case "INJURIES RIGHT ARM FRONT":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(rightArmFrontView);
                        currentWindow = rightArmFrontView;
                        break;
                    case "INJURIES LEFT ARM FRONT":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(leftArmFrontView);
                        currentWindow = leftArmFrontView;
                        break;
                    case "INJURIES RIGHT LEG FRONT":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(rightLegFrontView);
                        currentWindow = rightLegFrontView;
                        break;
                    case "INJURIES LEFT LEG FRONT":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(leftLegFrontView);
                        currentWindow = leftLegFrontView;
                        break;
                    case "INJURIES CHEST":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(chestView);
                        currentWindow = chestView;
                        break;
                    case "INJURIES ABDOMEN":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(abdomenView);
                        currentWindow = abdomenView;
                        break;
                    case "INJURIES HEAD BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(headBackView);
                        currentWindow = headBackView;
                        break;
                    case "INJURIES RIGHT ARM BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(rightArmBackView);
                        currentWindow = rightArmBackView;
                        break;
                    case "INJURIES LEFT ARM BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(leftArmBackView);
                        currentWindow = leftArmBackView;
                        break;
                    case "INJURIES RIGHT LEG BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(rightLegBackView);
                        currentWindow = rightLegBackView;
                        break;
                    case "INJURIES LEFT LEG BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(leftLegBackView);
                        currentWindow = leftLegBackView;
                        break;
                    case "INJURIES UPPER BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(upperBackView);
                        currentWindow = upperBackView;
                        break;
                    case "INJURIES LOWER BACK":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(lowerBackView);
                        currentWindow = lowerBackView;
                        break;
                    case "ADD INJURY":
                        //create a new mech page
                        mechanismOfInjury addInjuryMech = new mechanismOfInjury();
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(addInjuryMech);
                        break;
                    case "ADD INJURY DONE":
                        injuriesStackPanel.Children.Clear();
                        injuriesStackPanel.Children.Add(currentWindow);
                        break;
                }

                /*
                //Switch to injuries main
                if (message == "INJURIES MAIN")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(fullView);
                }

                //Switch to injuries head front
                else if (message == "INJURIES HEAD FRONT")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(headView);
                    currentWindow = headView;
                }

                //Switch to injuries right arm front
                else if (message == "INJURIES RIGHT ARM FRONT")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(rightArmFrontView);
                    currentWindow = rightArmFrontView;
                }

                //Switch to injuries left arm front
                else if (message == "INJURIES LEFT ARM FRONT")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(leftArmFrontView);
                    currentWindow = leftArmFrontView;

                }

                //Switch to injuries right leg front
                else if (message == "INJURIES RIGHT LEG FRONT")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(rightLegFrontView);
                    currentWindow = rightLegFrontView;

                }

                //Switch to injuries left leg front
                else if (message == "INJURIES LEFT LEG FRONT")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(leftLegFrontView);
                    currentWindow = leftLegFrontView;

                }

                //Switch to injuries chest
                else if (message == "INJURIES CHEST")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(chestView);
                    currentWindow = chestView;

                }

                //Switch to injuries abdomen
                else if (message == "INJURIES ABDOMEN")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(abdomenView);
                    currentWindow = abdomenView;
                }

                //Switch to injuries head back
                else if (message == "INJURIES HEAD BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(headBackView);
                    currentWindow = headBackView;

                }

                //Switch to injuries right arm back
                else if (message == "INJURIES RIGHT ARM BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(rightArmBackView);
                    currentWindow = rightArmBackView;

                }

                //Switch to injuries left arm back
                else if (message == "INJURIES LEFT ARM BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(leftArmBackView);
                    currentWindow = leftArmBackView;

                }

                //Switch to injuries right leg back
                else if (message == "INJURIES RIGHT LEG BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(rightLegBackView);
                    currentWindow = rightLegBackView;

                }

                //Switch to injuries left leg back
                else if (message == "INJURIES LEFT LEG BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(leftLegBackView);
                    currentWindow = leftLegBackView;

                }

                //Switch to injuries upper back
                else if (message == "INJURIES UPPER BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(upperBackView);
                    currentWindow = upperBackView;

                }

                //Switch to injuries abdomen
                else if (message == "INJURIES LOWER BACK")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(lowerBackView);
                    currentWindow = lowerBackView;

                }

                //Switch to add injuries page
                else if (message == "ADD INJURY")
                {
                    //create a new mech page
                    mechanismOfInjury addInjuryMech = new mechanismOfInjury();
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(addInjuryMech);
                }

                //Once injury added, switch to last page
                else if (message == "ADD INJURY DONE")
                {
                    injuriesStackPanel.Children.Clear();
                    injuriesStackPanel.Children.Add(currentWindow);
                }
                */


            }));

        }

        public void handleSystemPatientData(patient p)
        {

        }
    }
}

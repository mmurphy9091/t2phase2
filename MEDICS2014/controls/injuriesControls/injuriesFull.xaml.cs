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

namespace MEDICS2014.controls.injuriesControls
{
    /// <summary>
    /// Interaction logic for injuriesFull.xaml
    /// </summary>
    public partial class injuriesFull : UserControl
    {
        //Message stuff
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        //lines and labels lists
        List<Label> allLabels = new List<Label>();
        List<Rectangle> allLines = new List<Rectangle>();

        //Master list of all lists of locations
        List<List<string>> masterLocationsList = new List<List<string>>();

        //list for locations associated with areas
        List<string> headFrontLocations = new List<string>();
        List<string> chestLocations = new List<string>();
        List<string> abdomenLocations = new List<string>();
        List<string> rightArmFrontLocations = new List<string>();
        List<string> leftArmFrontLocations = new List<string>();
        List<string> rightLegFrontLocations = new List<string>();
        List<string> leftLegFrontLocations = new List<string>();

        List<string> headBackLocations = new List<string>();
        List<string> upperBackLocations = new List<string>();
        List<string> lowerBackLocations = new List<string>();
        List<string> rightArmBackLocations = new List<string>();
        List<string> leftArmBackLocations = new List<string>();
        List<string> rightLegBackLocations = new List<string>();
        List<string> leftLegBackLocations = new List<string>();

        public injuriesFull()
        {
            InitializeComponent();

            //hide all the lines and labels
            bindStartMode();

            //bind all the data for the locations
            bindLocationsData();

            _systemMessages.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
            _messages.HandleMessage += new EventHandler(OnHandleMessage);
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string Message = messageEvent.Message;
                handleMessageData(Message);
            }

        }

        public void handleMessageData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "CLEAR CONTROL":
                        //make all the lines and labels not visible
                        foreach (Label l in allLabels)
                        {
                            l.Content = "";
                            l.Visibility = Visibility.Hidden;
                        }
                        foreach (Rectangle r in allLines)
                        {
                            r.Visibility = Visibility.Hidden;
                        }
                        break;
                }
            }));
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                updateGUIInjuriesLocations(Message);
            }

        }

        private void bindStartMode()
        {
            //add the labels
            allLabels.Add(headFrontLabel);
            allLabels.Add(chestLabel);
            allLabels.Add(abdomenLabel);
            allLabels.Add(rightArmFrontLabel);
            allLabels.Add(leftArmFrontLabel);
            allLabels.Add(rightLegFrontLabel);
            allLabels.Add(leftLegFrontLabel);

            allLabels.Add(headBackLabel);
            allLabels.Add(upperBackLabel);
            allLabels.Add(lowerbackLabel);
            allLabels.Add(rightArmBackLabel);
            allLabels.Add(leftArmBackLabel);
            allLabels.Add(rightLegBackLabel);
            allLabels.Add(leftLegBackLabel);

            //add the line
            allLines.Add(headFrontLine);
            allLines.Add(chestLine);
            allLines.Add(abdomenLine);
            allLines.Add(rightArmFrontLine);
            allLines.Add(leftArmFrontLine);
            allLines.Add(rightLegFrontLine);
            allLines.Add(leftLegFrontLine);

            allLines.Add(headBackLine);
            allLines.Add(upperBackLine);
            allLines.Add(lowerBackLine);
            allLines.Add(rightArmBackLine);
            allLines.Add(leftArmBackLine);
            allLines.Add(rightLegBackLine);
            allLines.Add(leftLegBackLine);

            //make all the lines and labels not visible
            foreach (Label l in allLabels)
            {
                l.Content = "";
                l.Visibility = Visibility.Hidden;
            }
            foreach (Rectangle r in allLines)
            {
                r.Visibility = Visibility.Hidden;
            }

        }

        private void bindLocationsData()
        { 
            //bind all the locations for the front head
            headFrontLocations.Add("forehead");
            headFrontLocations.Add("rightEye");
            headFrontLocations.Add("leftEye");
            headFrontLocations.Add("rightEar");
            headFrontLocations.Add("leftEar");
            headFrontLocations.Add("nose");
            headFrontLocations.Add("rightCheek");
            headFrontLocations.Add("leftCheek");
            headFrontLocations.Add("mouth");
            headFrontLocations.Add("chin");

            //bind all locations for the chest
            chestLocations.Add("anteriorNeck");
            chestLocations.Add("rightUpperChest");
            chestLocations.Add("leftUpperChest");
            chestLocations.Add("rightAxillary");
            chestLocations.Add("leftAxillary");
            chestLocations.Add("rightLowerChest");
            chestLocations.Add("leftLowerChest");
            chestLocations.Add("sternum");

            //bind all locations for the abdomen
            abdomenLocations.Add("ruq");
            abdomenLocations.Add("luq");
            abdomenLocations.Add("rightFlank");
            abdomenLocations.Add("leftFlank");
            abdomenLocations.Add("rlq");
            abdomenLocations.Add("llq");
            abdomenLocations.Add("rightHip");
            abdomenLocations.Add("leftHip");
            abdomenLocations.Add("rightGroin");
            abdomenLocations.Add("leftGroin");
            abdomenLocations.Add("pelvis");
            abdomenLocations.Add("genitals");

            //bind all locations for the right arm front
            rightArmFrontLocations.Add("antRightShoulder");
            rightArmFrontLocations.Add("antRightLateralBicep");
            rightArmFrontLocations.Add("antRightMedialBicep");
            rightArmFrontLocations.Add("antRightElbow");
            rightArmFrontLocations.Add("antRightProximalLateralForearm");
            rightArmFrontLocations.Add("antRightProximalMedialForearm");
            rightArmFrontLocations.Add("antRightMiddleLateralForearm");
            rightArmFrontLocations.Add("antRightMiddleMedialForearm");
            rightArmFrontLocations.Add("antRightDistalLateralForearm");
            rightArmFrontLocations.Add("antRightDistalMedialForearm");
            rightArmFrontLocations.Add("antRightWrist");
            rightArmFrontLocations.Add("rightPalm");
            rightArmFrontLocations.Add("antRightThumb");
            rightArmFrontLocations.Add("antRightFingers");

            //bind all locations for the left arm front
            leftArmFrontLocations.Add("antLeftShoulder");
            leftArmFrontLocations.Add("antLeftLateralBicep");
            leftArmFrontLocations.Add("antLeftMedialBicep");
            leftArmFrontLocations.Add("antLeftElbow");
            leftArmFrontLocations.Add("antLeftProximalLateralForearm");
            leftArmFrontLocations.Add("antLeftProximalMedialForearm");
            leftArmFrontLocations.Add("antLeftMiddleLateralForearm");
            leftArmFrontLocations.Add("antLeftMiddleMedialForearm");
            leftArmFrontLocations.Add("antLeftDistalLateralForearm");
            leftArmFrontLocations.Add("antLeftDistalMedialForearm");
            leftArmFrontLocations.Add("antLeftWrist");
            leftArmFrontLocations.Add("leftPalm");
            leftArmFrontLocations.Add("antLeftThumb");
            leftArmFrontLocations.Add("antLeftFingers");

            //bind all locations for the right leg front
            rightLegFrontLocations.Add("antRightProximalLateralThigh");
            rightLegFrontLocations.Add("antRightProximalMedialThigh");
            rightLegFrontLocations.Add("antRightMiddleLateralThigh");
            rightLegFrontLocations.Add("antRightMiddleMedialThigh");
            rightLegFrontLocations.Add("antRightDistalLateralThigh");
            rightLegFrontLocations.Add("antRightDistalMedialThigh");
            rightLegFrontLocations.Add("antRightKnee");
            rightLegFrontLocations.Add("antRightProximalLateralLowerLeg");
            rightLegFrontLocations.Add("antRightProximalMedialLowerLeg");
            rightLegFrontLocations.Add("antRightMiddleLateralLowerLeg");
            rightLegFrontLocations.Add("antRightMiddleMedialLowerLeg");
            rightLegFrontLocations.Add("antRightDistalLateralLowerLeg");
            rightLegFrontLocations.Add("antRightDistalMedialLowerLeg");
            rightLegFrontLocations.Add("antRightAnkle");
            rightLegFrontLocations.Add("antRightFoot");
            rightLegFrontLocations.Add("antRightOtherToes");
            rightLegFrontLocations.Add("antRightBigToe");

            //bind all locations for the left leg front
            leftLegFrontLocations.Add("antLeftProximalLateralThigh");
            leftLegFrontLocations.Add("antLeftProximalMedialThigh");
            leftLegFrontLocations.Add("antLeftMiddleLateralThigh");
            leftLegFrontLocations.Add("antLeftMiddleMedialThigh");
            leftLegFrontLocations.Add("antLeftDistalLateralThigh");
            leftLegFrontLocations.Add("antLeftDistalMedialThigh");
            leftLegFrontLocations.Add("antLeftKnee");
            leftLegFrontLocations.Add("antLeftProximalLateralLowerLeg");
            leftLegFrontLocations.Add("antLeftProximalMedialLowerLeg");
            leftLegFrontLocations.Add("antLeftMiddleLateralLowerLeg");
            leftLegFrontLocations.Add("antLeftMiddleMedialLowerLeg");
            leftLegFrontLocations.Add("antLeftDistalLateralLowerLeg");
            leftLegFrontLocations.Add("antLeftDistalMedialLowerLeg");
            leftLegFrontLocations.Add("antLeftAnkle");
            leftLegFrontLocations.Add("antLeftFoot");
            leftLegFrontLocations.Add("antLeftOtherToes");
            leftLegFrontLocations.Add("antLeftBigToe");

            //bind all locations for back of head
            headBackLocations.Add("postHead");
            headBackLocations.Add("postNeck");

            //bind all locations for upper back
            upperBackLocations.Add("leftUpperBack");
            upperBackLocations.Add("rightUpperBack");
            upperBackLocations.Add("thoracicSpine");

            //bind all locations for lower back
            lowerBackLocations.Add("leftMiddleBack");
            lowerBackLocations.Add("rightMiddleBack");
            lowerBackLocations.Add("lumbarSpine");
            lowerBackLocations.Add("leftLowerBack");
            lowerBackLocations.Add("rightLowerBack");
            lowerBackLocations.Add("leftButtock");
            lowerBackLocations.Add("rightButtock");
            lowerBackLocations.Add("tailBone");

            //bind all locations for right arm back
            rightArmBackLocations.Add("postRightShoulder");
            rightArmBackLocations.Add("postRightLateralBicep");
            rightArmBackLocations.Add("postRightMedialBicep");
            rightArmBackLocations.Add("postRightElbow");
            rightArmBackLocations.Add("postRightProximalLateralForearm");
            rightArmBackLocations.Add("postRightProximalMedialForearm");
            rightArmBackLocations.Add("postRightMiddleLateralForearm");
            rightArmBackLocations.Add("postRightMiddleMedialForearm");
            rightArmBackLocations.Add("postRightDistalLateralForearm");
            rightArmBackLocations.Add("postRightDistalMedialForearm");
            rightArmBackLocations.Add("postRightWrist");
            rightArmBackLocations.Add("rightHand");
            rightArmBackLocations.Add("postRightThumb");
            rightArmBackLocations.Add("postRightFingers");

            //bind all locations for left arm back
            leftArmBackLocations.Add("postLeftShoulder");
            leftArmBackLocations.Add("postLeftLateralBicep");
            leftArmBackLocations.Add("postLeftMedialBicep");
            leftArmBackLocations.Add("postLeftElbow");
            leftArmBackLocations.Add("postLeftProximalLateralForearm");
            leftArmBackLocations.Add("postLeftProximalMedialForearm");
            leftArmBackLocations.Add("postLeftMiddleLateralForearm");
            leftArmBackLocations.Add("postLeftMiddleMedialForearm");
            leftArmBackLocations.Add("postLeftDistalLateralForearm");
            leftArmBackLocations.Add("postLeftDistalMedialForearm");
            leftArmBackLocations.Add("postLeftWrist");
            leftArmBackLocations.Add("leftHand");
            leftArmBackLocations.Add("postLeftThumb");
            leftArmBackLocations.Add("postLeftFingers");

            //bind all locations for the right leg back
            rightLegBackLocations.Add("postRightProximalLateralThigh");
            rightLegBackLocations.Add("postRightProximalMedialThigh");
            rightLegBackLocations.Add("postRightMiddleLateralThigh");
            rightLegBackLocations.Add("postRightMiddleMedialThigh");
            rightLegBackLocations.Add("postRightDistalLateralThigh");
            rightLegBackLocations.Add("postRightDistalMedialThigh");
            rightLegBackLocations.Add("postRightKnee");
            rightLegBackLocations.Add("postRightProximalLateralLowerLeg");
            rightLegBackLocations.Add("postRightProximalMedialLowerLeg");
            rightLegBackLocations.Add("postRightMiddleLateralLowerLeg");
            rightLegBackLocations.Add("postRightMiddleMedialLowerLeg");
            rightLegBackLocations.Add("postRightDistalLateralLowerLeg");
            rightLegBackLocations.Add("postRightDistalMedialLowerLeg");
            rightLegBackLocations.Add("postRightAnkle");
            rightLegBackLocations.Add("rightHeel");


            //bind all locations for the left leg back
            leftLegBackLocations.Add("postLeftProximalLateralThigh");
            leftLegBackLocations.Add("postLeftProximalMedialThigh");
            leftLegBackLocations.Add("postLeftMiddleLateralThigh");
            leftLegBackLocations.Add("postLeftMiddleMedialThigh");
            leftLegBackLocations.Add("postLeftDistalLateralThigh");
            leftLegBackLocations.Add("postLeftDistalMedialThigh");
            leftLegBackLocations.Add("postLeftKnee");
            leftLegBackLocations.Add("postLeftProximalLateralLowerLeg");
            leftLegBackLocations.Add("postLeftProximalMedialLowerLeg");
            leftLegBackLocations.Add("postLeftMiddleLateralLowerLeg");
            leftLegBackLocations.Add("postLeftMiddleMedialLowerLeg");
            leftLegBackLocations.Add("postLeftDistalLateralLowerLeg");
            leftLegBackLocations.Add("postLeftDistalMedialLowerLeg");
            leftLegBackLocations.Add("postLeftAnkle");
            leftLegBackLocations.Add("leftHeel");

            //bind master list
            masterLocationsList.Add(headFrontLocations);
            masterLocationsList.Add(chestLocations);
            masterLocationsList.Add(abdomenLocations);
            masterLocationsList.Add(rightArmFrontLocations);
            masterLocationsList.Add(leftArmFrontLocations);
            masterLocationsList.Add(rightLegFrontLocations);
            masterLocationsList.Add(leftLegFrontLocations);

            masterLocationsList.Add(headBackLocations);
            masterLocationsList.Add(upperBackLocations);
            masterLocationsList.Add(lowerBackLocations);
            masterLocationsList.Add(rightArmBackLocations);
            masterLocationsList.Add(leftArmBackLocations);
            masterLocationsList.Add(rightLegBackLocations);
            masterLocationsList.Add(leftLegBackLocations);

        }

        public void updateGUIInjuriesLocations(patient p)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (patient.Injuries i in p.injuries)
                {
                    //if there is no content in the type, go to next injury
                    if (i.Type == "" || i.Type == null)
                    {
                        break;
                    }
                    foreach (List<string> locList in masterLocationsList)
                    {
                        //front head
                        if (locList == headFrontLocations)
                        {
                            if (findAndShowInjury(i, locList, headFrontLabel, headFrontLine))
                            {
                                break;
                            }
                        }
                        //chest
                        else if (locList == chestLocations)
                        {
                            if (findAndShowInjury(i, locList, chestLabel, chestLine))
                            {
                                break;
                            }
                        }
                        //abdomen
                        else if (locList == abdomenLocations)
                        {
                            if (findAndShowInjury(i, locList, abdomenLabel, abdomenLine))
                            {
                                break;
                            }
                        }
                        //right front arm
                        else if (locList == rightArmFrontLocations)
                        {
                            if (findAndShowInjury(i, locList, rightArmFrontLabel, rightArmFrontLine))
                            {
                                break;
                            }
                        }
                        //left front arm
                        else if (locList == leftArmFrontLocations)
                        {
                            if (findAndShowInjury(i, locList, leftArmFrontLabel, leftArmFrontLine))
                            {
                                break;
                            }
                        }
                        //right front leg
                        else if (locList == rightLegFrontLocations)
                        {
                            if (findAndShowInjury(i, locList, rightLegFrontLabel, rightLegFrontLine))
                            {
                                break;
                            }
                        }
                        //left front leg
                        else if (locList == leftLegFrontLocations)
                        {
                            if (findAndShowInjury(i, locList, leftLegFrontLabel, leftLegFrontLine))
                            {
                                break;
                            }
                        }
                            //Back stuffs
                            //Back head
                        else if (locList == headBackLocations)
                        {
                            if (findAndShowInjury(i, locList, headBackLabel, headBackLine))
                            {
                                break;
                            }
                        }
                        //upper back
                        else if (locList == upperBackLocations)
                        {
                            if (findAndShowInjury(i, locList, upperBackLabel, upperBackLine))
                            {
                                break;
                            }
                        }
                        //lowerBack
                        else if (locList == lowerBackLocations)
                        {
                            if (findAndShowInjury(i, locList, lowerbackLabel, lowerBackLine))
                            {
                                break;
                            }
                        }
                        //right Back arm
                        else if (locList == rightArmBackLocations)
                        {
                            if (findAndShowInjury(i, locList, rightArmBackLabel, rightArmBackLine))
                            {
                                break;
                            }
                        }
                        //left Back arm
                        else if (locList == leftArmBackLocations)
                        {
                            if (findAndShowInjury(i, locList, leftArmBackLabel, leftArmBackLine))
                            {
                                break;
                            }
                        }
                        //right Back leg
                        else if (locList == rightLegBackLocations)
                        {
                            if (findAndShowInjury(i, locList, rightLegBackLabel, rightLegBackLine))
                            {
                                break;
                            }
                        }
                        //left Back leg
                        else if (locList == leftLegBackLocations)
                        {
                            if (findAndShowInjury(i, locList, leftLegBackLabel, leftLegBackLine))
                            {
                                break;
                            }
                        }
                    }
                }
            }));
        }

        public bool findAndShowInjury(patient.Injuries i, List<string> locationList, Label locLabel, Rectangle locRect)
        {
            foreach (string location in locationList)
            {
                if (location == i.Location)
                {
                    string labelContent = locLabel.Content.ToString();
                    string newContent = i.Type + ", " + i.Location;
                    //If the injury needs to be deleted clear it from the label
                    if (i.delete)
                    {
                        newContent += System.Environment.NewLine;
                        //if this is the only entry in the label
                        if (labelContent == newContent)
                        {
                            //clear the whole shebang
                            locLabel.Content = "";
                            locLabel.Visibility = Visibility.Hidden;
                            //b.Opacity = 0;
                            locRect.Visibility = Visibility.Hidden;
                            break;
                        }
                        //if this isn't the only entry in the label
                        else if (labelContent.Contains(newContent))
                        {
                            
                            string newData = labelContent.Replace(newContent, "");
                            locLabel.Content = newData;
                            break;
                        }
                    }
                    else
                    {
                        //if the injury and location have not been recorded yet, enter them into the label
                        if (!labelContent.Contains(newContent))
                        {
                            locLabel.Visibility = Visibility.Visible;
                            locRect.Visibility = Visibility.Visible;
                            locLabel.Content += i.Type + ", " + i.Location + System.Environment.NewLine;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private void locationBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = (Rectangle)sender;

            if (r.Name.ToString() == "headFrontBox")
            {
                _messages.AddMessage("INJURIES HEAD FRONT");
            }
            else if (r.Name.ToString() == "rightArmFrontBox")
            {
                _messages.AddMessage("INJURIES RIGHT ARM FRONT");
            }
            else if (r.Name.ToString() == "leftArmFrontBox")
            {
                _messages.AddMessage("INJURIES LEFT ARM FRONT");
            }
            else if (r.Name.ToString() == "chestBox")
            {
                _messages.AddMessage("INJURIES CHEST");
            }
            else if (r.Name.ToString() == "abdomenBox")
            {
                _messages.AddMessage("INJURIES ABDOMEN");
            }
            else if (r.Name.ToString() == "rightLegFrontBox")
            {
                _messages.AddMessage("INJURIES RIGHT LEG FRONT");
            }
            else if (r.Name.ToString() == "leftLegFrontBox")
            {
                _messages.AddMessage("INJURIES LEFT LEG FRONT");
            }
            else if (r.Name.ToString() == "headBackBox")
            {
                _messages.AddMessage("INJURIES HEAD BACK");
            }
            else if (r.Name.ToString() == "rightArmBackBox")
            {
                _messages.AddMessage("INJURIES RIGHT ARM BACK");
            }
            else if (r.Name.ToString() == "leftArmBackBox")
            {
                _messages.AddMessage("INJURIES LEFT ARM BACK");
            }
            else if (r.Name.ToString() == "upperBackBox")
            {
                _messages.AddMessage("INJURIES UPPER BACK");
            }
            else if (r.Name.ToString() == "lowerBackBox")
            {
                _messages.AddMessage("INJURIES LOWER BACK");
            }
            else if (r.Name.ToString() == "rightLegBackBox")
            {
                _messages.AddMessage("INJURIES RIGHT LEG BACK");
            }
            else if (r.Name.ToString() == "leftLegBackBox")
            {
                _messages.AddMessage("INJURIES LEFT LEG BACK");
            }
        }
    }
}

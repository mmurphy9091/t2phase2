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

namespace MEDICS2014.controls.treamentsConrols
{
    /// <summary>
    /// Interaction logic for bloodProductsDetails.xaml
    /// </summary>
    public partial class bloodProductsDetails : UserControl
    {
        List<Button> routeButtonsList = new List<Button>();
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        patient globalPatient = new patient();

        patient.BloodProductsDetails currentBloodProduct = new patient.BloodProductsDetails();

        string currentData = "";

        List<string> bloodTypes = new List<string>();

        bool isInFocus = false;

        public bloodProductsDetails()
        {
            InitializeComponent();

            bindButtonsAndData();

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

        public void handleMessageData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "CLEAR CONTROL":
                        //Hide all the things
                        globalPatient = new patient();
                        //clear everything but the current meds name
                        currentBloodProduct.Dose = null;
                        currentBloodProduct.Route = null;
                        currentBloodProduct.Time = null;
                        break;
                    case "Whole Blood":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.wholeBlood;
                        break;
                    case "Packed Red Blood":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.packedRedBlood;
                        break;
                    case "Whole Plasma":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.wholePlasma;
                        break;
                    case "Cryoprecipitate Plasma":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.cryoprecipitate;
                        break;
                    case "Platelete Rich Plasma":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.plateleteRichPlasma;
                        break;
                    case "Plasmanate":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.plasmanate;
                        break;
                    case "Hextend":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.hextend;
                        break;
                    case "Serum Albumin":
                        currentData = message;
                        typeLabel.Content = message;
                        currentBloodProduct = globalPatient.treatments.bloodProducts.serumAlbum;
                        break;
                }
            }));

            /*
            if (message == "Whole Blood")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.wholeBlood;
            }
            else if (message == "Packed Red Blood")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.packedRedBlood;
            }
            else if (message == "Whole Plasma")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.wholePlasma;
            }
            else if (message == "Cryoprecipitate Plasma")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.cryoprecipitate;
            }
            else if (message == "Platelete Rich Plasma")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.plateleteRichPlasma;
            }
            else if (message == "Plasmanate")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.plasmanate;
            }
            else if (message == "Hextend")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.hextend;
            }
            else if (message == "Serum Albumin")
            {
                currentData = message;
                typeLabel.Content = message;
                currentBloodProduct = globalPatient.treatments.bloodProducts.serumAlbum;
            }
            */
            if (!isInFocus)
            {
                bindTextBoxes();
            }

        }

        private void assignBloodProduct()
        {
            if (typeLabel.Content.ToString() == "Whole Blood")
            {
                globalPatient.treatments.bloodProducts.wholeBlood = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Packed Red Blood")
            {
                globalPatient.treatments.bloodProducts.packedRedBlood = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Whole Plasma")
            {
                globalPatient.treatments.bloodProducts.wholePlasma = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Cryoprecipitate Plasma")
            {
                globalPatient.treatments.bloodProducts.cryoprecipitate = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Platelete Rich Plasma")
            {
                globalPatient.treatments.bloodProducts.plateleteRichPlasma = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Plasmanate")
            {
                globalPatient.treatments.bloodProducts.plasmanate = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Hextend")
            {
                globalPatient.treatments.bloodProducts.hextend = currentBloodProduct;
            }
            else if (typeLabel.Content.ToString() == "Serum Albumin")
            {
                globalPatient.treatments.bloodProducts.serumAlbum = currentBloodProduct;
            }
        }

        public void bindTextBoxes()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (currentBloodProduct.Dose != null)
                {
                    doseTextBox.Text = currentBloodProduct.Dose;
                }
                else
                {
                    doseTextBox.Text = "";
                }
                if (currentBloodProduct.Time != null)
                {
                    timeTextBox.Text = currentBloodProduct.Time;
                }
                else
                {
                    timeTextBox.Text = "";
                }
                if (currentBloodProduct.Route != null)
                {
                    foreach (Button route in routeButtonsList)
                    {
                        //uncheck every button
                        route.Background = Brushes.Firebrick;
                        route.Foreground = Brushes.FloralWhite;

                        if (route.Content.ToString().Contains(currentBloodProduct.Route) && currentBloodProduct.Route != "")
                        {
                            //check only the button that matches the route
                            route.Background = Brushes.Yellow;
                            route.Foreground = Brushes.Black;

                        }
                    }
                }
                else
                {
                    //uncheck all the buttons
                    foreach (Button route in routeButtonsList)
                    {
                        route.Background = Brushes.Firebrick;
                        route.Foreground = Brushes.FloralWhite;
                    }
                }

            }));
        }

        public void OnHandleSystemMessage(object sender, EventArgs args)
        {
            var messageEvent = args as SystemMessageEventArgs;
            if (messageEvent != null)
            {
                patient Message = messageEvent.systemMessage;
                handlePatientData(Message);
            }

        }

        private void assignVDESdata(patient p)
        {
            //check every fucking thing to see if it's null, if it's not then assign the data
            //whole blood
            if (p.treatments.bloodProducts.wholeBlood.Dose != null)
            {
                globalPatient.treatments.bloodProducts.wholeBlood.Dose = p.treatments.bloodProducts.wholeBlood.Dose;
            }
            if (p.treatments.bloodProducts.wholeBlood.Route != null)
            {
                globalPatient.treatments.bloodProducts.wholeBlood.Route = p.treatments.bloodProducts.wholeBlood.Route;
            }
            if (p.treatments.bloodProducts.wholeBlood.Time != null)
            {
                globalPatient.treatments.bloodProducts.wholeBlood.Time = p.treatments.bloodProducts.wholeBlood.Time;
            }
            //Packed Red Blood
            if (p.treatments.bloodProducts.packedRedBlood.Dose != null)
            {
                globalPatient.treatments.bloodProducts.packedRedBlood.Dose = p.treatments.bloodProducts.packedRedBlood.Dose;
            }
            if (p.treatments.bloodProducts.packedRedBlood.Route != null)
            {
                globalPatient.treatments.bloodProducts.packedRedBlood.Route = p.treatments.bloodProducts.packedRedBlood.Route;
            }
            if (p.treatments.bloodProducts.packedRedBlood.Time != null)
            {
                globalPatient.treatments.bloodProducts.packedRedBlood.Time = p.treatments.bloodProducts.packedRedBlood.Time;
            }
            //Whole Plasma
            if (p.treatments.bloodProducts.wholePlasma.Dose != null)
            {
                globalPatient.treatments.bloodProducts.wholePlasma.Dose = p.treatments.bloodProducts.wholePlasma.Dose;
            }
            if (p.treatments.bloodProducts.wholePlasma.Route != null)
            {
                globalPatient.treatments.bloodProducts.wholePlasma.Route = p.treatments.bloodProducts.wholePlasma.Route;
            }
            if (p.treatments.bloodProducts.wholePlasma.Time != null)
            {
                globalPatient.treatments.bloodProducts.wholePlasma.Time = p.treatments.bloodProducts.wholePlasma.Time;
            }
            //Cryoprecipitate Plasma
            if (p.treatments.bloodProducts.cryoprecipitate.Dose != null)
            {
                globalPatient.treatments.bloodProducts.cryoprecipitate.Dose = p.treatments.bloodProducts.cryoprecipitate.Dose;
            }
            if (p.treatments.bloodProducts.cryoprecipitate.Route != null)
            {
                globalPatient.treatments.bloodProducts.cryoprecipitate.Route = p.treatments.bloodProducts.cryoprecipitate.Route;
            }
            if (p.treatments.bloodProducts.cryoprecipitate.Time != null)
            {
                globalPatient.treatments.bloodProducts.cryoprecipitate.Time = p.treatments.bloodProducts.cryoprecipitate.Time;
            }
            //Platelete Rich Plasma
            if (p.treatments.bloodProducts.plateleteRichPlasma.Dose != null)
            {
                globalPatient.treatments.bloodProducts.plateleteRichPlasma.Dose = p.treatments.bloodProducts.plateleteRichPlasma.Dose;
            }
            if (p.treatments.bloodProducts.plateleteRichPlasma.Route != null)
            {
                globalPatient.treatments.bloodProducts.plateleteRichPlasma.Route = p.treatments.bloodProducts.plateleteRichPlasma.Route;
            }
            if (p.treatments.bloodProducts.plateleteRichPlasma.Time != null)
            {
                globalPatient.treatments.bloodProducts.plateleteRichPlasma.Time = p.treatments.bloodProducts.plateleteRichPlasma.Time;
            }
            //Plasmanate
            if (p.treatments.bloodProducts.plasmanate.Dose != null)
            {
                globalPatient.treatments.bloodProducts.plasmanate.Dose = p.treatments.bloodProducts.plasmanate.Dose;
            }
            if (p.treatments.bloodProducts.plasmanate.Route != null)
            {
                globalPatient.treatments.bloodProducts.plasmanate.Route = p.treatments.bloodProducts.plasmanate.Route;
            }
            if (p.treatments.bloodProducts.plasmanate.Time != null)
            {
                globalPatient.treatments.bloodProducts.plasmanate.Time = p.treatments.bloodProducts.plasmanate.Time;
            }
            //Hextend
            if (p.treatments.bloodProducts.hextend.Dose != null)
            {
                globalPatient.treatments.bloodProducts.hextend.Dose = p.treatments.bloodProducts.hextend.Dose;
            }
            if (p.treatments.bloodProducts.hextend.Route != null)
            {
                globalPatient.treatments.bloodProducts.hextend.Route = p.treatments.bloodProducts.hextend.Route;
            }
            if (p.treatments.bloodProducts.hextend.Time != null)
            {
                globalPatient.treatments.bloodProducts.hextend.Time = p.treatments.bloodProducts.hextend.Time;
            }
            //Serum Albumin
            if (p.treatments.bloodProducts.serumAlbum.Dose != null)
            {
                globalPatient.treatments.bloodProducts.serumAlbum.Dose = p.treatments.bloodProducts.serumAlbum.Dose;
            }
            if (p.treatments.bloodProducts.serumAlbum.Route != null)
            {
                globalPatient.treatments.bloodProducts.serumAlbum.Route = p.treatments.bloodProducts.serumAlbum.Route;
            }
            if (p.treatments.bloodProducts.serumAlbum.Time != null)
            {
                globalPatient.treatments.bloodProducts.serumAlbum.Time = p.treatments.bloodProducts.serumAlbum.Time;
            }

            if (bloodTypes.Contains(currentData))
            {
                _messages.AddMessage(currentData);
            }

        }

        public void handlePatientData(patient p)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {

                if (p.fromVDES)
                {
                    assignVDESdata(p);
                    return;
                }

                if (p.fromDatabase)
                {
                    //Whole Blood
                    if (!isInFocus && currentData == "Whole Blood")
                    {
                        globalPatient.treatments.bloodProducts.wholeBlood = p.treatments.bloodProducts.wholeBlood;
                    }
                    //Packed Red Blood
                    if (!isInFocus && currentData == "Packed Red Blood")
                    {
                        globalPatient.treatments.bloodProducts.packedRedBlood = p.treatments.bloodProducts.packedRedBlood;
                    }
                    //Whole Plasma
                    if (!isInFocus && currentData == "Whole Plasma")
                    {
                        globalPatient.treatments.bloodProducts.wholePlasma = p.treatments.bloodProducts.wholePlasma;
                    }
                    //Cryoprecipitate Plasma
                    if (!isInFocus && currentData == "Cryoprecipitate Plasma")
                    {
                        globalPatient.treatments.bloodProducts.cryoprecipitate = p.treatments.bloodProducts.cryoprecipitate;
                    }
                    //Platelete Rich Plasma
                    if (!isInFocus && currentData == "Platelete Rich Plasma")
                    {
                        globalPatient.treatments.bloodProducts.plateleteRichPlasma = p.treatments.bloodProducts.plateleteRichPlasma;
                    }
                    //Plasmanate
                    if (!isInFocus && currentData == "Plasmanate")
                    {
                        globalPatient.treatments.bloodProducts.plasmanate = p.treatments.bloodProducts.plasmanate;
                    }
                    //Hextend
                    if (!isInFocus && currentData == "Hextend")
                    {
                        globalPatient.treatments.bloodProducts.hextend = p.treatments.bloodProducts.hextend;
                    }
                    //Serum Albumin
                    if (!isInFocus && currentData == "Serum Albumin")
                    {
                        globalPatient.treatments.bloodProducts.serumAlbum = p.treatments.bloodProducts.serumAlbum;
                    }
                    /*
                    //other
                    globalPatient.treatments.bloodProducts.other.Dose = p.treatments.bloodProducts.other.Dose;
                    globalPatient.treatments.bloodProducts.other.Route = p.treatments.bloodProducts.other.Route;
                    globalPatient.treatments.bloodProducts.other.Time = p.treatments.bloodProducts.other.Time;
                    globalPatient.treatments.bloodProducts.other.Type = p.treatments.bloodProducts.other.Type;
                     */

                    if (bloodTypes.Contains(currentData))
                    {
                        _messages.AddMessage(currentData);
                    }

                }
            }));
        }

        private void bindButtonsAndData()
        {
            otherButtonIM.Background = Brushes.Firebrick;
            otherButtonIM.Foreground = Brushes.FloralWhite;
            otherButtonIV.Background = Brushes.Firebrick;
            otherButtonIV.Foreground = Brushes.FloralWhite;
            otherButtonPO.Background = Brushes.Firebrick;
            otherButtonPO.Foreground = Brushes.FloralWhite;
            otherButtonPR.Background = Brushes.Firebrick;
            otherButtonPR.Foreground = Brushes.FloralWhite;
            otherButtonSL.Background = Brushes.Firebrick;
            otherButtonSL.Foreground = Brushes.FloralWhite;
            otherButtonSQ.Background = Brushes.Firebrick;
            otherButtonSQ.Foreground = Brushes.FloralWhite;

            //bind the route button list
            routeButtonsList.Add(otherButtonIM);
            routeButtonsList.Add(otherButtonIV);
            routeButtonsList.Add(otherButtonPO);
            routeButtonsList.Add(otherButtonPR);
            routeButtonsList.Add(otherButtonSL);
            routeButtonsList.Add(otherButtonSQ);

            //create the blood types list
            bloodTypes.Add("Whole Blood");
            bloodTypes.Add("Packed Red Blood");
            bloodTypes.Add("Whole Plasma");
            bloodTypes.Add("Cryoprecipitate Plasma");
            bloodTypes.Add("Platelete Rich Plasma");
            bloodTypes.Add("Plasmanate");
            bloodTypes.Add("Hextend");
            bloodTypes.Add("Serum Albumin");
        }

        private void bloodRouteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            isInFocus = true;
            //check if the button is already clicked
            if (b.Background == Brushes.Yellow)
            {
                b.Background = Brushes.Firebrick;
                b.Foreground = Brushes.FloralWhite;

                //remove it from the global patient
                currentBloodProduct.Route = "";
            }

            else
            {
                //Go through the list and uncheck any other button
                foreach (Button route in routeButtonsList)
                {
                    route.Background = Brushes.Firebrick;
                    route.Foreground = Brushes.FloralWhite;
                }


                //then click the button mentioned
                b.Background = Brushes.Yellow;
                b.Foreground = Brushes.Black;

                //and add it to the patient
                currentBloodProduct.Route = b.Content.ToString();
            }
        }

        private void getCurrentTimeButton_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("HHmm");
            timeTextBox.Text = time;
            isInFocus = true;
            currentBloodProduct.Time = time;
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            isInFocus = false;
            currentBloodProduct.Dose = doseTextBox.Text.ToString();
            currentBloodProduct.Time = timeTextBox.Text.ToString();
            assignBloodProduct();
            globalPatient.DBOperation = true;
            _systemMessages.AddMessage(globalPatient);
            _messages.AddMessage("TREATMENTS BLOOD PRODUCTS");
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

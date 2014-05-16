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

namespace MEDICS2014.controls
{
    /// <summary>
    /// Interaction logic for allergiesApp.xaml
    /// </summary>
    public partial class allergiesApp : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessage = SystemMessages.Instance;

        allergiesMain allMain = new allergiesMain();
        allergiesMedications allMed = new allergiesMedications();
        allergiesMedications2 allMed2 = new allergiesMedications2();
        allergiesMedications3 allMed3 = new allergiesMedications3();

        public allergiesApp()
        {
            InitializeComponent();

            allergiesMainStackPanel.Children.Clear();
            allergiesMainStackPanel.Children.Add(allMed);

            _messages.HandleMessage += new EventHandler(OnHandleMessage);
            //_systemMessage.HandleSystemMessage += new EventHandler(OnHandleSystemMessage);
        }

        public void OnHandleMessage(object sender, EventArgs args)
        {
            var messageEvent = args as MessageEventArgs;
            if (messageEvent != null)
            {
                string message = messageEvent.Message;
                //System.Windows.MessageBox.Show(message);
                handleAppData(message);
            }
        }

        /*
        public void OnHandleSystemMessage(object sender, EventArgs args)
        {

        }
         */ 

        public void handleAppData(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                switch (message)
                {
                    case "ALLERGIES MAIN":
                        allergiesMainStackPanel.Children.Clear();
                        allergiesMainStackPanel.Children.Add(allMed);
                        break;
                    case "ALLERGIES MEDICATIONS":
                        allergiesMainStackPanel.Children.Clear();
                        allergiesMainStackPanel.Children.Add(allMed);
                        break;
                    case "ALLERGIES MEDICATIONS 2":
                        allergiesMainStackPanel.Children.Clear();
                        allergiesMainStackPanel.Children.Add(allMed2);
                        break;
                    case "ALLERGIES MEDICATIONS 3":
                        allergiesMainStackPanel.Children.Clear();
                        allergiesMainStackPanel.Children.Add(allMed3);
                        break;
                }
                /*
                if (message == "ALLERGIES MAIN")
                {
                    allergiesMainStackPanel.Children.Clear();
                    allergiesMainStackPanel.Children.Add(allMed);
                }
                if (message == "ALLERGIES MEDICATIONS")
                {
                    allergiesMainStackPanel.Children.Clear();
                    allergiesMainStackPanel.Children.Add(allMed);
                }
                if (message == "ALLERGIES MEDICATIONS 2")
                {
                    allergiesMainStackPanel.Children.Clear();
                    allergiesMainStackPanel.Children.Add(allMed2);
                }
                 */
            }));
        }
    }
}

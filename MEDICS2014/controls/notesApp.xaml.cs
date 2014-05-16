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
    /// Interaction logic for notesApp.xaml
    /// </summary>
    public partial class notesApp : UserControl
    {
        SystemMessages _systemMessages = SystemMessages.Instance;
        Messages _messages = Messages.Instance;

        bool isInFocus = false;

        public notesApp()
        {
            InitializeComponent();

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
                        if (!isInFocus)
                        {
                            notesTextBox.Text = "";
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
                handlePatientData(Message);
            }

        }

        public void handlePatientData(patient p)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (!isInFocus)
                {
                    if (p.DBOperation || p.fromDatabase)
                    {
                        if (p.notes != null)
                        {
                            if (p.notes != "")
                            {
                                string currentText = notesTextBox.Text.ToString();
                                if (p.notes != currentText)
                                {
                                    currentText += p.notes;
                                    notesTextBox.Text = currentText;
                                }
                            }
                        }
                    }
                }
            }));
        }


        private void notesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = false;
            if (notesTextBox.Text != "")
            {
                patient notes = new patient();
                notes.DBOperation = true;
                notes.notes = notesTextBox.Text.ToString();
                _systemMessages.AddMessage(notes);
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            notesTextBox.Text = "";
        }

        private void notesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isInFocus = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDICS2014
{
    class SystemMessages
    {
        private List<patient> _systemMessages = new List<patient>();
        private static readonly SystemMessages _instance = new SystemMessages();
        public event EventHandler HandleSystemMessage;

        /// <summary>
        /// Prevents a default instance of the <see cref="Messages"/> class from being created.
        /// </summary>
        private SystemMessages()
        {
        }

        /// <summary>
        /// Gets the instance of the class.
        /// </summary>
        public static SystemMessages Instance
        {
            get
            {
                return _instance;
            }   
        }

        /// <summary>
        /// Gets the current messages list.
        /// </summary>
        public List<patient> CurrentMessages
        {
            get
            {
                return _systemMessages;
            }
        }

        /// <summary>
        /// Notifies any of the subscribers that a new message has been received.
        /// </summary>
        /// <param name="message">The message.</param>
        public void NotifyNewMessage(patient systemMessage)
        {
            EventHandler handler = HandleSystemMessage;
            if (handler != null)
            {
                // This will call the any form that is currently "wired" to the event, notifying them
                // of the new message.
                handler(this, new SystemMessageEventArgs(systemMessage));
            }
        }

        /// <summary>
        /// Adds a new messages to the "central" list
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage(patient systemMessage)
        {
            _systemMessages.Add(systemMessage);
            NotifyNewMessage(systemMessage);
        }
    }

    /// <summary>
    /// Special Event Args used to pass the message data to the subscribers.
    /// </summary>
    class SystemMessageEventArgs : EventArgs
    {
        private patient _systemMessage = null;
        public SystemMessageEventArgs(patient systemMessage)
        {
            _systemMessage = systemMessage;
        }

        public patient systemMessage
        {
            get
            {
                return _systemMessage;
            }
        }
    }
}

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

namespace MEDICS2014.controls.medsControls
{
    /// <summary>
    /// Interaction logic for medsUnits.xaml
    /// </summary>
    public partial class medsUnits : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;

        public medsUnits()
        {
            InitializeComponent();
        }

        private void unitButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            patient unit = new patient();
            unit.medUnits = b.Content.ToString();
            _systemMessages.AddMessage(unit);
            _messages.AddMessage("MEDS DETAILS");
        }
    }
}

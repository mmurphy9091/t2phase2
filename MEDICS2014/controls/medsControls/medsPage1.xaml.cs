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
    /// Interaction logic for medsPage1.xaml
    /// </summary>
    public partial class medsPage1 : UserControl
    {
        Messages _messages = Messages.Instance;

        public medsPage1()
        {
            InitializeComponent();
        }

        private void analgesicButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS ANALGESIC");
        }

        private void antibioticButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS ANTIBIOTIC 1");
        }

        private void otherButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS OTHERS");
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("HOME");
        }
    }
}

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
    /// Interaction logic for treatmentsFluids.xaml
    /// </summary>
    public partial class treatmentsFluids : UserControl
    {
        Messages _messages = Messages.Instance;

        public treatmentsFluids()
        {
            InitializeComponent();

            bindFluidsButtons();
        }

        private void bindFluidsButtons()
        {
            normalSalineButton.Background = Brushes.DimGray;
            normalSalineButton.Content = "Normal" + System.Environment.NewLine + "Saline";

            lactactedRingersButton.Background = Brushes.DimGray;
            lactactedRingersButton.Content = "Lactacted" + System.Environment.NewLine + "Ringers";


            dextroseButton.Background = Brushes.DimGray;

        }

        private void normalSalineButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS SALINE");
        }

        private void lactactedRingersButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS RINGERS");
        }

        private void dextroseButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS DEXTROSE");
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
        }


        private void lactactedRingersDetailsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dextroseDetailsButton_Click(object sender, RoutedEventArgs e)
        {


        }

        private void otherButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS FLUIDS OTHER");
        }
    }
}

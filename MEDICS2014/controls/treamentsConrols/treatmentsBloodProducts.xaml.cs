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
    /// Interaction logic for treatmentsBloodProducts.xaml
    /// </summary>
    public partial class treatmentsBloodProducts : UserControl
    {

        Messages _messages = Messages.Instance;

        public treatmentsBloodProducts()
        {
            InitializeComponent();

            bindButtonData();
        }

        private void bindButtonData()
        {
            wholeButton.Content = "Whole" + System.Environment.NewLine + "Blood";
            packedButton.Content = "Packed" + System.Environment.NewLine + "Red Blood";
            plateleteRichPlasmaButton.Content = "Platelete" + System.Environment.NewLine + "Rich Plasma";
            wholePlasmaButton.Content = "Whole" + System.Environment.NewLine + "Plasma";
            serumAlbuminButton.Content = "Serum" + System.Environment.NewLine + "Album";

        }

        private void bloodButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            _messages.AddMessage(b.Content.ToString());
        }

        private void plateleteRichPlasmaButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Platelete Rich Plasma");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void plasmanateButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Plasmanate");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void hextendButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Hextend");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void serumAlbuminButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Serum Albumin");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void wholeButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Whole Blood");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void packedButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Packed Red Blood");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void wholePlasmaButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Whole Plasma");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void cryoprecipitateplasmaButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("Cryoprecipitate Plasma");
            _messages.AddMessage("TREATMENTS BLOOD DETAILS");
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS MAIN");
        }

        private void otherButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("TREATMENTS BLOOD DETAILS OTHER");
        }
    }
}

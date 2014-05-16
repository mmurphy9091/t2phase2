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
    /// Interaction logic for treatmentsMain.xaml
    /// </summary>
    public partial class treatmentsMain : UserControl
    {
        Messages _messages = Messages.Instance;

        public treatmentsMain()
        {
            InitializeComponent();

            bloodProductsButton.Content = " Blood" + System.Environment.NewLine + "Products";
        }

        private void sectionButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            switch (b.Name)
            {
                case "airwayButton":
                    _messages.AddMessage("TREATMENTS AIRWAY");
                    break;
                case "breathingButton":
                    _messages.AddMessage("TREATMENTS BREATHING");
                    break;
                case "circulationButton":
                    _messages.AddMessage("TREATMENTS CIRCULATION");
                    break;
                case "fluidsButton":
                    _messages.AddMessage("TREATMENTS FLUIDS");
                    break;
                case "bloodProductsButton":
                    _messages.AddMessage("TREATMENTS BLOOD PRODUCTS");
                    break;
                case "otherButton":
                    _messages.AddMessage("TREATMENTS OTHER");
                    break;
            }
            /*
            if (b.Name == "airwayButton")
            {
                _messages.AddMessage("TREATMENTS AIRWAY");
            }
            else if (b.Name == "breathingButton")
            {
                _messages.AddMessage("TREATMENTS BREATHING");
            }
            else if (b.Name == "circulationButton")
            {
                _messages.AddMessage("TREATMENTS CIRCULATION");
            }
            else if (b.Name == "fluidsButton")
            {
                _messages.AddMessage("TREATMENTS FLUIDS");
            }
            else if (b.Name == "bloodProductsButton")
            {
                _messages.AddMessage("TREATMENTS BLOOD PRODUCTS");
            }
             */
        }
    }
}

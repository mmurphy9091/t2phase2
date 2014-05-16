﻿using System;
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
    /// Interaction logic for medsAntibiotic1.xaml
    /// </summary>
    public partial class medsAntibiotic1 : UserControl
    {
        Messages _messages = Messages.Instance;
        SystemMessages _systemMessages = SystemMessages.Instance;
        public medsAntibiotic1()
        {
            InitializeComponent();
        }

        private void medsButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            patient temp = new patient();

            temp.tempMed.Name = b.Content.ToString();
            temp.tempMed.Antibiotic = "True";

            _systemMessages.AddMessage(temp);
            _messages.AddMessage("MEDS DETAILS");

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS ANTIBIOTIC 2");
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            _messages.AddMessage("MEDS");
        }
    }
}

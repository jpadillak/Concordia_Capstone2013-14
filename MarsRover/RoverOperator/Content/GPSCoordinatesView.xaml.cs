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
using Microsoft.Maps.MapControl.WPF;

namespace RoverOperator.Content
{
    /// <summary>
    /// Interaction logic for GPSMapView.xaml
    /// </summary>
    public partial class GPSCoordinatesView : UserControl
    {
        GPSViewViewModel gpsVM;

        public GPSCoordinatesView()
        {
            InitializeComponent();
            gpsVM = new GPSViewViewModel(this.Dispatcher);
            this.DataContext = gpsVM;
        }
    }
}

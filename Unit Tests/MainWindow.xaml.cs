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
using CoordinateSharp;
using System.Diagnostics;
using System.ComponentModel;
namespace CoordinateShaprWPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Coordinate coord;
        public MainWindow()
        {
            
            coord = new Coordinate(40.0362, -74.6179, new DateTime(2017, 8, 21));
            coord.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
            coord.FormatOptions.Display_Leading_Zeros = true;
            this.DataContext = coord; 
           
            InitializeComponent();
                           
            latPosBox.Items.Add(CoordinatesPosition.N);
            latPosBox.Items.Add(CoordinatesPosition.S);
            longPosBox.Items.Add(CoordinatesPosition.E);
            longPosBox.Items.Add(CoordinatesPosition.W);

          
     
        }
      
    }
  
}

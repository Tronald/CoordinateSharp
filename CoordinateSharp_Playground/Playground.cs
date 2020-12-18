using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSharp;
using CoordinateSharp.Formatters;
using CoordinateSharp.Magnetic;
using CoordinateSharp.Debuggers;
using System.IO;
using System.Timers;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace CoordinateSharp_Playground
{
    class Playground
    {    
        [STAThread]
        static void Main()
        {
            //89,-121 (2020-1-1) should return GV 9.08
            // Get others to complete tests.


            //NE TOO HANDLE LOCAL TIMES
            //Play Here
            Coordinate c = new Coordinate(89, -121, new DateTime(2020, 1, 1), new EagerLoad(false));
            var m = new Magnetic(c,new Distance(0), DataModel.WMM2020);
            Output.Output_Class_Values(m.MagneticFieldElements, OutputOption.Console);
            Console.WriteLine();
            Output.Output_Class_Values(m.SecularVariations, OutputOption.Console);
            Console.WriteLine();
            Output.Output_Class_Values(m.Uncertainty, OutputOption.Console);
            Console.WriteLine();
            //End
            Console.ReadKey();
           
        }

        private static void C_CoordinateChanged(object sender, EventArgs e)
        {            
            var c = (Coordinate)sender;
            Magnetic m = new Magnetic(c, DataModel.WMM2020);
            Console.WriteLine("DEC: " + m.MagneticFieldElements.Declination);
        }
    } 
}

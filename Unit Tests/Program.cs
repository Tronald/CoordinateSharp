using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSharp;
using System.Diagnostics;
namespace CSTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //NO RISE KNOWN CASE:
            Coordinate cord = new Coordinate(59, 82.2209974507276, new DateTime(2018, 2, 18, 0, 0, 0));
            Console.WriteLine(cord.CelestialInfo.SunCondition + " " + cord.CelestialInfo.SunRise.Value.ToString() + " " + cord.CelestialInfo.SunSet.Value.ToString());
            //NO SET KNOWN CASE
            cord = new Coordinate(59.57, - 98.2209974507276, new DateTime(2018, 3, 1, 0, 0, 0));     
            Console.WriteLine(cord.CelestialInfo.SunCondition + " " + cord.CelestialInfo.SunRise.Value.ToString() + " " + cord.CelestialInfo.SunSet.Value.ToString());
            //NO DAWN KNOWN CASE
            cord = new Coordinate(55.57, -75.2209974507276, new DateTime(2018, 7, 1, 0, 0, 0));
            Console.WriteLine(cord.CelestialInfo.SunCondition + " " + cord.CelestialInfo.SunRise + " " + cord.CelestialInfo.SunSet + " " + cord.CelestialInfo.AdditionalSolarTimes.CivilDawn);
 
            Console.ReadKey();
            DateTime date = new DateTime(2018, 1, 1);


            for (int i = -56; i < 90; i++)
            {            
                    for (int m = 0; m < 60; m++)
                    {
                        double lat = i + (m / 100.0);
                        for (int d = 0; d < 365; d++)
                        {
                            Coordinate c = new Coordinate(lat, 82.2209974507276, date.AddDays(d));
                            if (i < 60 && i > -60)
                            {
                                if (c.CelestialInfo.SunCondition != CelestialStatus.RiseAndSet)
                                {
                                    Debug.Print(c.Latitude.ToDouble() + " " + c.Longitude.ToDouble() + " " + c.GeoDate.ToString() + " " + c.CelestialInfo.SunCondition);
                                }
                            }
                        }

                }
               
                Console.WriteLine("NEXT:    ..." + i);
            }
        }
    }
}

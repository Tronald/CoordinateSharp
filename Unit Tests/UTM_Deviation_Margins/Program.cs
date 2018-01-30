using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSharp;
using System.Diagnostics;
namespace UTM_Margin_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] maxLatMargin = new double[]{0,0,0}; //{Max Deviation Recorded, @Lat, @Long}
            double[] maxLongMargin = new double[] { 0, 0, 0 }; //{Max Deviation Recorded, @Lat, @Long}

            //Deviations Neglagible between 85th parallels. Beyond thats, Lat deviations can occur up to 3 degrees. 
            //This forumala should not be used for calculations above/below -85/85 parallels.
            //Adjust parallels lat degrees in the below loop to guage deviations.

            for(double lat = -90.0; lat<91; lat++)
            {
                for (double lng = -180.0; lng < 181; lng++)
                {
                    double latMargin = 0;
                    double longMargin = 0;
                    Coordinate c = new Coordinate(lat, lng);
                    UniversalTransverseMercator utm = c.UTM;
                    Coordinate nc;
                    
                    nc = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

                    double clat = c.Latitude.ToDouble();
                    double nlat = nc.Latitude.ToDouble();
                    double clong=c.Longitude.ToDouble();
                    double nlong = nc.Longitude.ToDouble();
                    if (clat < 0) { clat *= -1; }
                    if(clong <0){clong*=-1;}
                    if (nlat < 0) { nlat *= -1; }
                    if (nlong < 0) { nlong *= -1; }
                    latMargin =Math.Round(clat,6) - Math.Round(nlat,6);
                    
                    longMargin = Math.Round(clong,6) - Math.Round(nlong,6);
                    if (latMargin < 0) { latMargin *= -1; }
                  
                    if (longMargin < 0) { longMargin *= -1; }
                    Console.WriteLine(c.Latitude.ToDouble() + " " + c.Longitude.ToDouble() + " : " + 
                        nc.Latitude.ToDouble() + " " + nc.Longitude.ToDouble() + " : " 
                        + latMargin.ToString() + " " + longMargin.ToString());
                    if (latMargin > maxLatMargin[0]) { maxLatMargin[0] = latMargin; maxLatMargin[1] = c.Latitude.ToDouble(); maxLatMargin[2] = c.Longitude.ToDouble(); }
                    if (longMargin > maxLongMargin[0]) { maxLongMargin[0] = longMargin; maxLongMargin[1] = c.Latitude.ToDouble(); maxLatMargin[2] = c.Longitude.ToDouble(); }
                }
           
            }
            Console.WriteLine("MAX LAT DEVIATIONS - " + maxLatMargin[0].ToString() + " @ " + maxLatMargin[1].ToString() + " " + maxLatMargin[2].ToString());
            Console.WriteLine("MAX LONG DEVIATIONS - " + maxLongMargin[0].ToString() + " @ " + maxLongMargin[1].ToString() + " " + maxLongMargin[2].ToString());
            Console.ReadKey();
        }
    }
}

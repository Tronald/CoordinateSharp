using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // CHANGE LATS e LONS
            double lat1 = 0, lon1 = 0, lat2 = 0, lon2 = 0, lat3 = 0, lon3 = 0; 

            CoordinateSharp.Coordinate P1 = new CoordinateSharp.Coordinate(lat1, lon1);
            CoordinateSharp.Coordinate P2 = new CoordinateSharp.Coordinate(lat2, lon2);
            CoordinateSharp.Coordinate P3 = new CoordinateSharp.Coordinate(lat3, lon3);

            var d = P1.Get_Distance_From_Coordinate(P2).Meters;
            var s = CoordinateSharp.Celestial.CalculateSunData(lat1, lon1, DateTime.Now);
            Console.WriteLine("Distance? {0}", d);
            Console.WriteLine("Sunrise? {0}", s.SunRise);

            //geofence test
            Dictionary<string, FeatureCollection> _geoFeatures = new Dictionary<string, FeatureCollection>();
            string fileJsonToLoad = "cmp.json"; //find  test geojason file

            try
            {
                string json = File.ReadAllText(fileJsonToLoad);
                var feature = JsonConvert.DeserializeObject<FeatureCollection>(json);
                Path.GetFileNameWithoutExtension(fileJsonToLoad);
                _geoFeatures.Add(Path.GetFileNameWithoutExtension(fileJsonToLoad), feature);
            }
            catch (Exception e)
            {

            }


            List<CoordinateSharp.GeoFence.Point> points = new List<CoordinateSharp.GeoFence.Point>();
            var featureCollection = _geoFeatures["cmp"];
            if (featureCollection.Features.Count > 0)
            {
                if (featureCollection.Features[0].Geometry is GeoJSON.Net.Geometry.LineString)
                {
                    foreach (var pp in (featureCollection.Features[0].Geometry as GeoJSON.Net.Geometry.LineString).Coordinates)
                    {
                        points.Add(new CoordinateSharp.GeoFence.Point
                        {
                            Latitude = pp.Latitude,
                            Longitude = pp.Longitude
                        });
                    }
                }
                else if (featureCollection.Features[0].Geometry is GeoJSON.Net.Geometry.Polygon)
                {
                    foreach (var pp in (featureCollection.Features[0].Geometry as GeoJSON.Net.Geometry.Polygon).Coordinates)
                    {
                        foreach (var ppp in pp.Coordinates)
                        {
                            points.Add(new CoordinateSharp.GeoFence.Point
                            {
                                Latitude = ppp.Latitude,
                                Longitude = ppp.Longitude
                            });
                        }
                    }
                }
            }
            CoordinateSharp.GeoFence gf = new CoordinateSharp.GeoFence(points);

            //Console.WriteLine("P1 in polygon? {0}", gf.IsPointInPolygon(P1));
            Console.WriteLine("P2 in range of line? {0}", gf.IsPointInRangeOfLine(P3, 12));

            Console.ReadKey();
        }
    }
}

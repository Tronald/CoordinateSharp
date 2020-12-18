using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;

namespace CoordinateSharp_UnitTests
{
    /// <summary>
    /// Test event handler subscriptions.
    /// </summary>
    [TestClass]
    public class EventHandler_Tests
    {
        bool latB = false;
        bool lngB = false;
        bool geodateB = false;
        bool offsetB = false;
        bool datumB = false;
        bool moveB = false;
      
        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChanged_Lat()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_Lat;
           
            c.Latitude.DecimalDegree++;
            Assert.AreEqual(true, latB, "Latitude change did not trigger change.");
        }
      
        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChange_Long()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_Lng;
            c.Longitude.DecimalDegree++;
            Assert.AreEqual(true, lngB, "Longitude change did not trigger change.");
           ;
        }

        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChange_GeoDate()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_GeoDate;
            c.GeoDate = new DateTime(2020, 1, 1);
            Assert.AreEqual(true, geodateB, "GeoDate change did not trigger change.");
        }
        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChange_Offset()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_Offset;
            c.Offset = -1;
            Assert.AreEqual(true, offsetB, "Offset change did not trigger change.");         
        }
        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChange_Datum()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_Datum;
            c.Set_Datum(6378135, 298.24);
            Assert.AreEqual(true, datumB, "Datum 1 change did not trigger change.");
            datumB = false;

            c.Set_Datum(6378136, 298.25, Coordinate_Datum.LAT_LONG);
            Assert.AreEqual(true, datumB, "Datum 2 change did not trigger change.");
        }
        /// <summary>
        /// Ensures subscription and triggers for CoordinateChanged event.
        /// </summary>
        [TestMethod]
        public void CoordinateChange_Move()
        {
            Coordinate c = new Coordinate(25, 25);
            c.CoordinateChanged += C_CoordinateChanged_Move;
            c.Move(new Coordinate(26, 26), new Distance(10), Shape.Ellipsoid);
            Assert.AreEqual(true, moveB, "Move 1 change did not trigger change.");
            moveB = false;

            c.Move(new Coordinate(26, 26), 100, Shape.Ellipsoid);
            Assert.AreEqual(true, moveB, "Move 2 change did not trigger change.");
            moveB = false;

            c.Move(10, 10, Shape.Sphere);
            Assert.AreEqual(true, moveB, "Move 3 change did not trigger change.");
            moveB = false;

            c.Move(new Distance(10), 10, Shape.Sphere);
            Assert.AreEqual(true, moveB, "Move 4 change did not trigger change.");
        }    

        private void C_CoordinateChanged_Lat(object sender, EventArgs e)
        {
            latB= true;
        }
        private void C_CoordinateChanged_Lng(object sender, EventArgs e)
        {
            lngB = true;
        }
        private void C_CoordinateChanged_GeoDate(object sender, EventArgs e)
        {
            geodateB = true;
        }
        private void C_CoordinateChanged_Offset(object sender, EventArgs e)
        {
            offsetB = true;
        }
        private void C_CoordinateChanged_Datum(object sender, EventArgs e)
        {
            datumB = true;
        }
        private void C_CoordinateChanged_Move(object sender, EventArgs e)
        {
            moveB = true;
        }
    }
}

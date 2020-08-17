using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoordinateSharp;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class MGRS_GridBoxTests
    {
        /// <summary>
        /// Ensures true 100km identifier plots correctly.
        /// </summary>
        [TestMethod]
        public void Normal_Box_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("13S", "EA", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13S", "EA", 0, 0),gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"13S EA 00000 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13S", "EA", 99999, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"13S EA 99999 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13S", "EA", 00000, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"13S EA 00000 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13S", "EA", 99999, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"13S EA 99999 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Right_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("4Q", "HH", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("4Q", "HH", 0, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"4Q HH 00000 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("4Q", "HH", 14181, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"4Q HH 14181 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("4Q", "HH", 00000, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"4Q HH 00000 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("4Q", "HH", 12361, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"4Q HH 12361 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Left_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("5Q", "JC", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("5Q", "JC", 85819, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"5Q JC 85819 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("5Q", "JC", 99999, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"5Q JC 99999 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("5Q", "JC", 87639, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"5Q JC 87639 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("5Q", "JC", 99999, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"5Q JC 99999 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Right_Descending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("18L", "ZN", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("18L", "ZN", 0, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"18L ZN 00000 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("18L", "ZN", 27020, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"18L ZN 27020 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("18L", "ZN", 00000, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"18L ZN 00000 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("18L", "ZN", 28046, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"18L ZN 28046 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Left_Descending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("19L", "AH", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("19L", "AH", 72980, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"19L AH 72980 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("19L", "AH", 99999, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"19L AH 99999 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("19L", "AH", 71954, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"19L AH 71954 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("19L", "AH", 99999, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"19L AH 99999 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Right_Pointed_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("13J", "HN", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13J", "HN", 0, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"13J HN 00000 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13J", "HN", 02112, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"13J HN 02112 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13J", "HN", 00000, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"13J HN 00000 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("13J", "HN", 04313, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"13J HN 04313 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Left_Pointed_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("14J", "JT", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("14J", "JT", 97888, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"14J JT 97888 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("14J", "JT", 99999, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"14J JT 99999 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("14J", "JT", 95687, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"14J JT 95687 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("14J", "JT", 99999, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"14J JT 99999 99999\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Right_Pointed_Low_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("9U", "YV", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("9U", "YV", 0, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"9U YV 00000 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("9U", "YV", 00320, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"9U YV 00320 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("9U", "YV", 00000, 07598), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"9U YV 00000 07598\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("9U", "YV", 00000, 07598), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"9U YV 00000 07598\"     {gb.Top_Right_MGRS_Point}");
        }
        [TestMethod]
        public void GZJ_Box_Left_Pointed_Low_Ascending_Accuracy_Test()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("10U", "BD", 0, 0);
            var gb = mgrs.Get_Box_Boundaries();
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("10U", "BD", 95508, 0), gb.Bottom_Left_MGRS_Point), $"Bottom Left Corner Exceeds Delta: \"10U BD 95508 00000\"     {gb.Bottom_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("10U", "BD", 99999, 00000), gb.Bottom_Right_MGRS_Point), $"Bottom Right Corner Exceeds Delta: \"10U BD 99999 00000\"     {gb.Bottom_Right_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("10U", "BD", 99680, 99999), gb.Top_Left_MGRS_Point), $"Top Left Corner Exceeds Delta: \"10U BD 99680 99999\"     {gb.Top_Left_MGRS_Point}");
            Assert.IsTrue(MGRS_Comparer(new MilitaryGridReferenceSystem("10U", "BD", 99999, 99999), gb.Top_Right_MGRS_Point), $"Top Right Corner Exceeds Delta: \"10U BD 99999 99999\"     {gb.Top_Right_MGRS_Point}");
        }


        private bool MGRS_Comparer(MilitaryGridReferenceSystem expected, MilitaryGridReferenceSystem actual)
        {
            if(expected.LatZone != actual.LatZone) { return false; }
            if(expected.LongZone != actual.LongZone) { return false; }
            if(expected.Digraph != actual.Digraph) { return false; }
            if(Math.Abs(expected.Easting - actual.Easting)>2) { return false; }
            if(Math.Abs(expected.Northing - actual.Northing) > 2) { return false; }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace CoordinateSharp
{
    /// <summary>
    /// Military Grid Reference System (MGRS)
    /// Relies upon values from the UniversalTransverseMercator class
    /// </summary>
    public class MilitaryGridReferenceSystem : INotifyPropertyChanged
    {
        private string latZone;
        private int longZone;
        private double easting;
        private double northing;
        private string digraph;

        public string LatZone
        {
            get { return this.latZone; }
            
        }
        public int LongZone
        {
            get { return this.longZone; }
           
        }
        public double Easting
        {
            get { return this.easting; }
          
        }
        public double Northing
        {
            get { return this.northing; }
           
        }
        public string Digraph
        {
            get { return this.digraph; }
        }

        internal MilitaryGridReferenceSystem(UniversalTransverseMercator utm)
        {
            ToMGRS(utm);
        }
        internal void ToMGRS(UniversalTransverseMercator utm)
        {
            Digraphs digraphs = new Digraphs();

            string digraph1 = digraphs.getDigraph1(utm.LongZone, utm.Easting);
            string digraph2 = digraphs.getDigraph2(utm.LongZone, utm.Northing);

            this.digraph = digraph1 + digraph2;
            this.latZone = utm.LatZone;
            this.longZone = utm.LongZone;

            //String easting = String.valueOf((int)_easting);
            string e =  ((int)utm.Easting).ToString();
            if (e.Length < 5)
            {
                e = "00000" + ((int)utm.Easting).ToString();
            }
            e = e.Substring(e.Length - 5);
           
            this.easting = Convert.ToInt32(e);
            
            string n =  ((int)utm.Northing).ToString();
            if (n.Length < 5)
            {
                n = "0000" + ((int)utm.Northing).ToString();
            }
            n = n.Substring(n.Length - 5);
           
            this.northing = Convert.ToInt32(n);
        }
      
        /// <summary>
        /// MGRS Default String Format
        /// </summary>
        /// <returns>MGRS Formatted Coordinate String</returns>
        public override string ToString()
        {
            return this.longZone.ToString() + this.LatZone + " " + this.digraph + " " + ((int)this.easting).ToString("00000") + " " + ((int)this.northing).ToString("00000");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        
    }
}

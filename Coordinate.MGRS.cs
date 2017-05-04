﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace CoordinateSharp
{
    /// <summary>
    /// Military Grid Reference System (MGRS)
    /// Relies upon the UniversalTransverseMercator class
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

        public MilitaryGridReferenceSystem(UniversalTransverseMercator utm)
        {
            ToMGRS(utm);
        }
        public void ToMGRS(UniversalTransverseMercator utm)
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
            Debug.Print(e);
            this.easting = Convert.ToInt32(e);
            
            string n =  ((int)utm.Northing).ToString();
            if (n.Length < 5)
            {
                n = "0000" + ((int)utm.Northing).ToString();
            }
            n = n.Substring(n.Length - 5);
            Debug.Print(n);
            this.northing = Convert.ToInt32(n);
        }
        /// <summary>
        /// Property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies Coordinate property of changing.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                if (propName != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            }
        }

        public override string ToString()
        {
            return this.longZone.ToString() + this.LatZone + " " + this.digraph + " " + ((int)this.easting).ToString("00000") + " " + ((int)this.northing).ToString("00000");
        }            
        
    }
}
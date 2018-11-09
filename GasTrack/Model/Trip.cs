using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace GasTrack.Model
{
    public class Trip
    {
        [PrimaryKey][AutoIncrement]
        public int TripId { get; set; }
        public int CarId { get; set; }
        public string TripName { get; set; }
        public DateTime TripDate { get; set; }
        public double CounterStart { get; set; }
        //public 
        public double CounterEnd { get; set; }
    }
}

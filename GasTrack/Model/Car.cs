using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace GasTrack.Model
{
    public class Car
    {
        [PrimaryKey]
        [AutoIncrement]
        public int CarId { get; set; }
        public string CarName { get; set; }
        public string CarOwner { get; set; }
        public string LicensePlate { get; set; }
        public double CostPerDistance { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Data;
using System.Diagnostics;

namespace GasTrack.Model
{
    public class TripManager
    {
        public List<Trip> Trips { get; set; }


        public TripManager(int carId)
        {
            Trips = DatabaseHelper.GetTrips(carId);  // Get trips from dbase
        }



        // Add new trip
        public void Add(Trip trip)
        {
            Debug.WriteLine("TripManager: Add");
            trip.TripDate = DateTime.Now;
            DatabaseHelper.Write(trip);
            Trips.Add(trip);
        }


        // Delete trip
        public void Delete(Trip trip)
        {
            if (Trips.Contains(trip))
            {
                Debug.WriteLine("TripManager: Delete");
                DatabaseHelper.Delete(trip);
                Trips.Remove(trip);
            }
        }


        // Update a trip
        public void Update(Trip trip)
        {
            Debug.WriteLine("TripManager: Update");
            DatabaseHelper.Write(trip);
        }
    }
}

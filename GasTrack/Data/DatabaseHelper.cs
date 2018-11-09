using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Model;
using SQLite.Net;
using System.Diagnostics;

namespace GasTrack.Data
{
    public class DatabaseHelper
    {
        // Load data from database
        // Trips
        public static List<Trip> GetTrips(int carId)
        {
            List<Trip> tripList = new List<Trip>();

            // Load the individual trips from the dbase.
            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                var query = db.Table<Trip>().OrderBy(c => c.TripId);
                foreach (var _trip in query)
                {
                    if (_trip.CarId == carId)
                    {
                        tripList.Add(_trip);
                    }
                }
            }

            return tripList;
        }

        // Cars
        public static List<Car> GetCars()
        {
            List<Car> carList = new List<Car>();

            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                var query = db.Table<Car>().OrderBy(c => c.CarId);
                foreach (var _car in query)
                {
                    carList.Add(_car);
                }
            }

            return carList;
        }



        // Save data to database
        // Trips
        public static void Write(Trip trip)
        {
            Debug.WriteLine("DIRECT: Saving trip...");

            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                try
                {
                    var existingTrip = (db.Table<Trip>().Where(c => c.TripId == trip.TripId)).SingleOrDefault();
                    if (existingTrip != null)
                    {
                        // Update existing item in case this exists
                        existingTrip.TripName = trip.TripName;
                        existingTrip.CounterStart = trip.CounterStart;
                        existingTrip.CounterEnd = trip.CounterEnd;

                        int success = db.Update(existingTrip);
                    }
                    else
                    {
                        // Otherwise, just create a new one! :)
                        int success = db.Insert(new Trip()
                        {
                            CarId = trip.CarId,
                            TripName = trip.TripName,
                            TripDate = trip.TripDate,
                            CounterStart = trip.CounterStart,
                            CounterEnd = trip.CounterEnd
                        });
                    }

                    Debug.WriteLine("DATABASE: CarId of new trip = " + trip.CarId);
                    Debug.WriteLine("DIRECT: Trip successfully saved!");
                }
                catch
                {
                    Debug.WriteLine("DIRECT: Trip was not saved :(");
                }
            }
        }


        // Cars
        public static void Write(Car car)
        {
            Debug.WriteLine("DIRECT: Saving car...");

            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                try
                {
                    // First, check if the id is the same to an already existing item
                    var existingCar = (db.Table<Car>().Where(c => c.CarId == car.CarId)).SingleOrDefault();
                    if (existingCar != null)
                    {
                        // Update existing item in case this exists
                        existingCar.CarName = car.CarName;
                        existingCar.CarOwner = car.CarOwner;
                        existingCar.LicensePlate = car.LicensePlate;
                        existingCar.CostPerDistance = car.CostPerDistance;

                        int success = db.Update(existingCar);
                    }
                    else
                    {
                        // Otherwise, just create a new one! :)
                        int success = db.Insert(new Car()
                        {
                            CarName = car.CarName,
                            CarOwner = car.CarOwner,
                            LicensePlate = car.LicensePlate,
                            CostPerDistance = car.CostPerDistance
                        });
                    }

                    Debug.WriteLine("DIRECT: Car successfully saved!");
                }
                catch
                {
                    Debug.WriteLine("DIRECT: Car was not saved :(");
                }
            }
        }





        // Delete data from database
        // Trips
        public static void Delete(Trip trip)
        {
            Debug.WriteLine("DIRECT: Deleting trip... ");
            Debug.WriteLine("TripId: " + trip.TripId);

            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                var existingTrip = (db.Table<Trip>().Where(c => c.TripId == trip.TripId)).SingleOrDefault();
                if (existingTrip != null)
                {
                    db.RunInTransaction(() =>
                    {
                        db.Delete(existingTrip);
                        if ((db.Table<Trip>().Where(c => c.TripId == existingTrip.TripId)).SingleOrDefault() == null)
                        {
                            Debug.WriteLine("DIRECT: Trip successfully deleted");
                        }
                        else
                        {
                            Debug.WriteLine("DIRECT: Trip was not removed :(");
                        }
                    });
                }
            }
        }


        // Cars
        public static void Delete(Car car)
        {
            Debug.WriteLine("DIRECT: Deleting car... ");
            Debug.WriteLine("CarId: " + car.CarId);

            using (var db = new SQLiteConnection(App.SQLITE_PLATFORM, App.DB_PATH))
            {
                int carIdTemp = car.CarId;


                var existingCar = (db.Table<Car>().Where(c => c.CarId == car.CarId)).SingleOrDefault();
                if (existingCar != null)
                {
                    db.RunInTransaction(() =>
                    {
                        db.Delete(existingCar);
                        if ((db.Table<Car>().Where(c => c.CarId == car.CarId)).SingleOrDefault() == null)
                        {
                            Debug.WriteLine("DIRECT: Car successfully deleted");

                            // Delete all trips related to the car
                            List<Trip> tripsTemp = new List<Trip>();
                            tripsTemp = GetTrips(carIdTemp);

                            foreach (Trip trip in tripsTemp)
                            {
                                if (trip.CarId == carIdTemp)
                                {
                                    Delete(trip);
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("DIRECT: Car was not removed :(");
                        }
                    });
                }
            }
        }

    }
}

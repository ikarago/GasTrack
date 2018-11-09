using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Data;
using System.Diagnostics;

namespace GasTrack.Model
{
    public class CarManager
    {
        public List<Car> Cars { get; set; }

        public CarManager()
        {
            Cars = DatabaseHelper.GetCars();    // Get cars from database
        }


        // Add new car
        public void Add(Car car)
        {
            Debug.WriteLine("CarManager: Add");
            DatabaseHelper.Write(car);
            Cars.Add(car);
        }


        // Delete trip
        public void Delete(Car car)
        {
            if (Cars.Contains(car))
            {
                Debug.WriteLine("CarManager: Delete");
                DatabaseHelper.Delete(car);
                Cars.Remove(car);
            }
        }


        // Update a trip
        public void Update(Car car)
        {
            Debug.WriteLine("CarManager: Update");
            DatabaseHelper.Write(car);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GasTrack.ViewModel
{
    public class CarManagerViewModel : NotificationBase
    {
        private CarManager carManager;
        public CarViewModel currentCar;

        // Constructor for the ViewModel
        public CarManagerViewModel()
        {
            this.GetCars();
        }



        // Get the cars from the Model
        public void GetCars()
        {
            carManager = new CarManager();
            _SelectedIndex = -1;

            _Cars.Clear();
            foreach (var car in carManager.Cars)
            {
                var newCar = new CarViewModel(car);
                newCar.PropertyChanged += Car_OnNotifyPropertyChanged;  // Subscribe object to the OnNotifyPropertyChanged Event
                _Cars.Add(newCar);
            }
        }


        #region MVVM and List-stuff
        // Notify Manager when an object has changed
        public void Car_OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            carManager.Update((CarViewModel)sender);
        }



        // List with cars
        ObservableCollection<CarViewModel> _Cars = new ObservableCollection<CarViewModel>();
        public ObservableCollection<CarViewModel> Cars
        {
            get { return _Cars; }
            set { SetProperty(ref _Cars, value); }
        }



        // Selected Index
        int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (SetProperty(ref _SelectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedCar)); }
            }
        }

        // Get Selected Car from the Index
        public CarViewModel SelectedCar
        {
            get { return (_SelectedIndex >= 0) ? _Cars[_SelectedIndex] : null; }
        }
        #endregion



        // Add
        public void Add(string carOwner, string carName = null, string licensePlate = null, double distanceCost = 0)
        {
            Debug.WriteLine("CMVM: Add Car - Starting attempt");
            CarViewModel carViewModel = new CarViewModel();

            // First, the Car Owner MUST be set!
            carViewModel.CarOwner = carOwner;

            // Second, add everything else :P
            // Do something with a non filled in name...
            carViewModel.CarName = carName;
            carViewModel.LicensePlate = licensePlate;
            carViewModel.CostPerDistance = distanceCost;

            this.carManager.Add(carViewModel);

            // Before we continue, don't we have to fix somthing with the ID and such?
            // Yeah, we really need to do that.. :/
            this._Cars.Add(carViewModel);
            this._SelectedIndex = this._Cars.IndexOf(carViewModel);

            carViewModel.PropertyChanged += Car_OnNotifyPropertyChanged;

            // Update Total distance
            Debug.WriteLine("ID: " + carViewModel.CarId);
            Debug.WriteLine("CarName: " + carViewModel.CarName);

            Debug.WriteLine("CMVM: Add Car - Successfull");
        }



        // Update
        public void Update(CarViewModel oldCar, string carOwner = null, string carName = null, string licensePlate = null, double distanceCost = 0)
        {
            // Get all the data from the old note and the newly entered stuff
            Debug.WriteLine("CMVM: Update Car - Starting attempt");

            Car newCar = new Car();
            newCar.CarId = oldCar.CarId;
            if (carOwner != "" && carOwner != null) { newCar.CarOwner = carOwner; }
            if (carName != "" && carName != null) { newCar.CarName = carName; }
            if (licensePlate != "" && licensePlate != null) { newCar.LicensePlate = licensePlate; }
            if (distanceCost > 0) { newCar.CostPerDistance = distanceCost; }    // Make this more reliable with different regions and stuff

            this.carManager.Update(newCar);
            Debug.WriteLine("ID: " + newCar.CarId);
            Debug.WriteLine("CarName: " + newCar.CarName);
            Debug.WriteLine("CMVM: Update Car - Successfull");
            // Message to the user that changes have been saved
        }



        // Delete Car from list
        public bool Delete(CarViewModel carToDelete)
        {
            bool success = false;
            Debug.WriteLine("CMVM: Delete Car - Starting attempt");

            try
            {
                this.carManager.Delete(carToDelete);
                this._Cars.Remove(carToDelete);
                this._SelectedIndex = -1;
                Debug.WriteLine("CMVM: Delete Car - Successful");
                success = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CMVM: Delete Car - Failed");
                Debug.WriteLine("Error: " + ex);
            }


            return success;
        }



        public CarViewModel GetCarById(int carId)
        {
            CarViewModel car = this._Cars.Where((x) => x.CarId == carId).FirstOrDefault();
            return car;
        }

    }
}

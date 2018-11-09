using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace GasTrack.ViewModel
{
    public class TripManagerViewModel : NotificationBase
    {
        TripManager tripManager;
        ResourceHelper fuck = new ResourceHelper();

        public TripViewModel currentTrip;

        public TripManagerViewModel(int carId)
        {
            GetTrips(carId);
        }


        public void GetTrips(int carId)
        {
            tripManager = new TripManager(carId);
            _SelectedIndex = -1;

            _Trips.Clear();
            foreach (var trip in tripManager.Trips)
            {
                var newTrip = new TripViewModel(trip);
                newTrip.PropertyChanged += Trip_OnNotifyPropertyChanged;

                // Check whether the End-Distance has been filled in. If not, don't add it to the list, but add is as current car
                if (newTrip.CounterEnd != 0)
                {
                    _Trips.Add(newTrip);
                }
                else
                {
                    currentTrip = newTrip;
                }
            }
        }


        public void Trip_OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            tripManager.Update((TripViewModel)sender);
        }


        // List with trips
        ObservableCollection<TripViewModel> _Trips = new ObservableCollection<TripViewModel>();
        public ObservableCollection<TripViewModel> Trips
        {
            get { return _Trips; }
            set { SetProperty(ref _Trips, value); }
        }

        // Selected Index
        int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (SetProperty(ref _SelectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedTrip)); }
            }
        }

        // Selected Trip
        public TripViewModel SelectedTrip
        {
            get { return (_SelectedIndex >= 0) ? _Trips[_SelectedIndex] : null; }
        }


        // Total distance
        public double TotalDistance
        {
            get
            {
                double total = 0;
                foreach (var trip in _Trips)
                {
                    total += trip.TripDistance;
                }
                return total;
            }
        }

        // Total cost
        public double TotalCost
        {
            get
            {
                double total = 0;
                foreach (var trip in _Trips)
                {
                    total += trip.TripCost;
                }
                return total;
            }
        }





        // Add
        public void AddNewTrip (int carId, double counterStart, double counterStartDecimal)
        {
            Debug.WriteLine("TMVM: Add new trip...");
            TripViewModel tripViewModel = new TripViewModel();

            // Get Car ID and add that to the trip
            tripViewModel.CarId = carId;

            if (counterStartDecimal <= 0)
            {
                counterStartDecimal = 0;
            }

            string counterString = counterStart + "." + counterStartDecimal;
            double counter = Convert.ToDouble(counterString, CultureInfo.InvariantCulture);
            
            counter = Math.Round(counter, 1);
            tripViewModel.CounterStart = counter;

            tripManager.Add(tripViewModel);
            this._Trips.Add(tripViewModel);
            this._SelectedIndex = this._Trips.IndexOf(tripViewModel);

            tripViewModel.PropertyChanged += Trip_OnNotifyPropertyChanged;

            // Update Total distance
            Debug.WriteLine("Trip ID: " + tripViewModel.TripId);
            Debug.WriteLine("GrandTotalDistance: " + this.TotalDistance.ToString());
        }





        // Update
        public bool FinishTrip (double counterEnd, double counterEndDecimal)
        {
            bool success = false;
            Debug.WriteLine("TMVM: Finishing trip...");

            if (counterEndDecimal <= 0)
            {
                counterEndDecimal = 0;
            }


            string counterString = counterEnd + "." + counterEndDecimal;
            double counter = Convert.ToDouble(counterString, CultureInfo.InvariantCulture);

            counter = Math.Round(counter, 1);

            if (counter > currentTrip.CounterStart)
            {
                counterEnd = Math.Round(counter, 1);
                currentTrip.CounterEnd = counter;
                tripManager.Update(currentTrip);
                currentTrip = null;
                success = true;
            }
            else
            {
                // Give error
                fuck.ShowErrorDialog("Message-WrongAmount");
            }

            return success;
        }



        // Delete single trip from list
        public bool Delete(TripViewModel tripToDelete)
        {
            bool success = false;
            Debug.WriteLine("TMVM: Delete single Trip - Starting attempt");

            try
            {
                this.tripManager.Delete(tripToDelete);
                this._Trips.Remove(tripToDelete);
                this._SelectedIndex = -1;
                Debug.WriteLine("TMVM: Delete single Trip - Successful");
                success = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TMVM: Delete single Trip - Failed");
                Debug.WriteLine("Error: " + ex);
            }


            return success;
        }




        // Delete all trips
        public void DeleteAll()
        {
            try
            {
                foreach (TripViewModel trip in _Trips)
                {
                    tripManager.Delete(trip);
                }
                _Trips.Clear();
                currentTrip = null;
                Debug.WriteLine("TMVM: Deleted all trips!");
            }
            catch { Debug.WriteLine("TMVM: Coudn't delete all trips :("); }
        }

    }
}

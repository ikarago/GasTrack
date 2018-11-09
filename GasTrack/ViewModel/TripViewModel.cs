using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Model;
using Windows.Globalization.DateTimeFormatting;
using Windows.Storage;
using GasTrack.Model.Helpers;
using System.Diagnostics;

namespace GasTrack.ViewModel
{
    public class TripViewModel : NotificationBase<Trip>
    {
        SettingsHelper settingsHelper = new SettingsHelper();
        CarManagerViewModel carManager = new CarManagerViewModel();
        public int selectedCarId;
        

        public TripViewModel(Trip trip = null) : base(trip) { }
        public int TripId
        {
            get { return This.TripId; }
            set { SetProperty(This.TripId, value, () => This.TripId = value); }
        }
        public int CarId
        {
            get { return This.CarId; }
            set { SetProperty(This.CarId, value, () => This.CarId = value); }
        }
        public string TripName
        {
            get { return This.TripName; }
            set { SetProperty(This.TripName, value, () => This.TripName = value); }
        }
        public double CounterStart
        {
            get { return This.CounterStart; }
            set { SetProperty(This.CounterStart, value, () => This.CounterStart = value); }
        }
        public double CounterEnd
        {
            get { return This.CounterEnd; }
            set { SetProperty(This.CounterEnd, value, () => This.CounterEnd = value); }
        }       
        public DateTime TripDate
        {
            get
            {
                if (This.TripDate != DateTime.MinValue)
                {
                    return This.TripDate;
                }
                else { return DateTime.Now; }
            }
            set { }
        }
        public string TripDateString
        {
            get
            {
                try
                {
                    var formatter = new DateTimeFormatter("shortdate");
                    return formatter.Format(TripDate);
                }
                catch { return TripDate.ToString(); }
            }
        }

        // Totals
        public double TripDistance
        {
            get
            {
                double totalDistance = (this.CounterEnd - this.CounterStart);
                totalDistance = Math.Round(totalDistance, 1);

                return totalDistance;
            }
        }

        // Money shiz
        public double TripCost
        {
            get
            {
                double tripCost = 0;
                this.selectedCarId = settingsHelper.GetSelectedCarId();
                CarViewModel selectedCar = carManager.GetCarById(selectedCarId);

                try
                {
                    tripCost = Math.Round((this.TripDistance * selectedCar.CostPerDistance), 2);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("TripViewModel - Can't calculate tripcosts");
                    Debug.WriteLine(ex);
                }

                return tripCost;
            }
        }





    }
}

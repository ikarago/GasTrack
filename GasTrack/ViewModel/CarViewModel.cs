using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasTrack.Model;

namespace GasTrack.ViewModel
{
    public class CarViewModel : NotificationBase<Car>
    {

        public CarViewModel(Car car = null) : base(car) { }

        public int CarId
        {
            get { return This.CarId; }
            set { SetProperty(This.CarId, value, () => This.CarId = value); }
        }
        public string CarName
        {
            get { return This.CarName; }
            set { SetProperty(This.CarName, value, () => This.CarName= value); }
        }
        public string CarOwner
        {
            get { return This.CarOwner; }
            set { SetProperty(This.CarOwner, value, () => This.CarOwner = value); }
        }
        public string LicensePlate
        {
            get { return This.LicensePlate; }
            set { SetProperty(This.LicensePlate, value, () => This.LicensePlate = value); }
        }
        public double CostPerDistance
        {
            get { return This.CostPerDistance; }
            set { SetProperty(This.CostPerDistance, value, () => This.CostPerDistance = value); }
        }
    }
}

using FoodTrucks.Common;
using System;

namespace FoodTrucks.Services
{
    public static class FoodTruckDataModelExtensions
    {
        public static FoodTruckModel MapToFoodTruckModel(this FoodTruckDataModel data)
        {
            if (data==null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return new FoodTruckModel(data.locationId, data.Applicant, data.FacilityType, data.cnn, data.LocationDescription, data.Address, data.blocklot, data.block, data.lot, data.permit
                , data.Status, data.FoodItems, data.X, data.Y, data.Latitude, data.Longitude, data.Schedule, data.daysHours, data.NOISent, data.Approved, data.received, data.PriorPermit, data.ExpirationDate, data.Location);
        }
    }
}

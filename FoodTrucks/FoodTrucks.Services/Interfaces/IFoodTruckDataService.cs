using FoodTrucks.Common;
using System.Collections.Generic;

namespace FoodTrucks.Services
{
    public interface IFoodTruckDataService
    {
        FoodTruckModel GetByLocationId(int locationId);
        IReadOnlyCollection<FoodTruckModel> GetByBlock(string blockId);
        void Add(FoodTruckModel foodTruck);
    }
}

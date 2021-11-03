using System.Collections.Generic;

namespace FoodTrucks.Services
{
    public interface IFoodTruckDataSource
    {
       IEnumerable<FoodTruckDataModel> GetAll();
    }

}

namespace FoodTrucks.Services
{
    public interface IFoodTruckDataCSVParser
    {
        FoodTruckDataModel Parse(string line);
    }
}
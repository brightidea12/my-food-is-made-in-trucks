using FoodTrucks.Common;
using MediatR;

namespace FoodTrucks.Handlers
{
    public class AddNewFoodTruckRequest : IRequest
    {
        public FoodTruckModel Model { get; init; }
        public AddNewFoodTruckRequest(FoodTruckModel model)
        {
            Model = model;
        }
    }
}

using FoodTrucks.Common;
using MediatR;

namespace FoodTrucks.Handlers
{
    public class GetFoodTruckByLocationIdRequest : IRequest<FoodTruckModel> {
        public int LocationId {get; init;}
        public GetFoodTruckByLocationIdRequest(int locationId)
        {
            LocationId = locationId;
        }
    }
}

using FoodTrucks.Common;
using MediatR;
using System.Collections.Generic;

namespace FoodTrucks.Handlers
{
    public class GetFoodTrucksByBlockRequest : IRequest<IReadOnlyCollection<FoodTruckModel>>
    {
        public string Block { get; init; }
        public GetFoodTrucksByBlockRequest(string blockId)
        {
            Block = blockId;
        }
    }

}

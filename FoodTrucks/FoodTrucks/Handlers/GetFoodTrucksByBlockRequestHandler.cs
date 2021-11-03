using FoodTrucks.Common;
using FoodTrucks.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodTrucks.Handlers
{
    public class GetFoodTrucksByBlockRequestHandler : IRequestHandler<GetFoodTrucksByBlockRequest, IReadOnlyCollection<FoodTruckModel>>
    {
        private readonly IFoodTruckDataService _foodTruckDataService;

        public GetFoodTrucksByBlockRequestHandler(IFoodTruckDataService foodTruckDataService)
        {
            _foodTruckDataService = foodTruckDataService;
        }

        public Task<IReadOnlyCollection<FoodTruckModel>> Handle(GetFoodTrucksByBlockRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request?.Block))
            {
                throw new ArgumentNullException($"Block is required.");
            }

            var result = _foodTruckDataService.GetByBlock(request.Block);
            if (result == null)
            {
                throw new KeyNotFoundException($"There are no food trucks registered at block {request.Block}.");
            }
          
            return Task.FromResult(result);
        }
    }

}

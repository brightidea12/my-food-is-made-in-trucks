using FoodTrucks.Common;
using FoodTrucks.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodTrucks.Handlers
{
    public class GetFoodTruckByLocationIdRequestHandler : IRequestHandler<GetFoodTruckByLocationIdRequest, FoodTruckModel>
    {
        private readonly IFoodTruckDataService _foodTruckDataService;

        public GetFoodTruckByLocationIdRequestHandler(IFoodTruckDataService foodTruckDataService)
        {
            _foodTruckDataService = foodTruckDataService;
        }

        public Task<FoodTruckModel> Handle(GetFoodTruckByLocationIdRequest request, CancellationToken cancellationToken)
        {

            if (!(request?.LocationId > 0))
            {
                throw new ArgumentNullException("LocationId is a required.");
            }

            var result = _foodTruckDataService.GetByLocationId(request.LocationId);
            if (result == null)
            {
                throw new KeyNotFoundException($"{request.LocationId} does not correspond to a registered food truck location.");
            }

            return Task.FromResult(result);
        }
    }

}

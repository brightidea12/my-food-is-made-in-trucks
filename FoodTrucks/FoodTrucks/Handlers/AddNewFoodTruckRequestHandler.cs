using FoodTrucks.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodTrucks.Handlers
{
    public class AddNewFoodTruckRequestHandler : IRequestHandler<AddNewFoodTruckRequest>
    {
        private readonly IFoodTruckDataService _foodTruckDataService;

        public AddNewFoodTruckRequestHandler(IFoodTruckDataService foodTruckDataService)
        {
            _foodTruckDataService = foodTruckDataService;
        }

        public Task<Unit> Handle(AddNewFoodTruckRequest request, CancellationToken cancellationToken)
        {

            if ((request==null) || (request.Model == null) || (request.Model.LocationId <= 0))
            {
                throw new ArgumentNullException("The provided model was incorrectly formatted.");
            }

            var checkForDuplicate = _foodTruckDataService.GetByLocationId(request.Model.LocationId);
            if (checkForDuplicate != null)
            {
                throw new InvalidOperationException ($"LocationId: {request.Model.LocationId} already exists, duplicates are not allowed.");
            }

            _foodTruckDataService.Add(request.Model);

            return Unit.Task;
        }
    }


}

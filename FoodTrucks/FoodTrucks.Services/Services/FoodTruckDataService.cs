using FoodTrucks.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FoodTrucks.Services
{
    public class FoodTruckDataService : IFoodTruckDataService
    {
        private readonly ILogger _logger;
        private readonly IFoodTruckDataSource _dataSource;
        private readonly Dictionary<int, FoodTruckModel> _foodTrucksByLocationId;
        private readonly Dictionary<string, List<FoodTruckModel>> _foodTrucksByBlock;

        public FoodTruckDataService(IFoodTruckDataSource dataSource, ILogger<IFoodTruckDataService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _foodTrucksByLocationId = new();
            _foodTrucksByBlock = new();
            Initialize();
        }

        private void Initialize()
        {
            foreach (var foodTruck in _dataSource.GetAll())
            {
                if (foodTruck!=null)
                {
                    var model = foodTruck.MapToFoodTruckModel();
                    Index(model);
                }
            }
        }
        
        private void Index(FoodTruckModel model)
        {
            string block = model.Block;
            _foodTrucksByLocationId.Add(model.LocationId, model);

            if (!_foodTrucksByBlock.ContainsKey(block))
            {
                _foodTrucksByBlock.Add(block, new());
            }

            _foodTrucksByBlock[block].Add(model);
        }

        public IReadOnlyCollection<FoodTruckModel> GetByBlock(string blockId)
        {
            List<FoodTruckModel> result = null;
            if (_foodTrucksByBlock.ContainsKey(blockId))
            {
                result = _foodTrucksByBlock[blockId];
            }

            return result;
        }

        public FoodTruckModel GetByLocationId(int locationId)
        {
            FoodTruckModel result=null;
            if (_foodTrucksByLocationId.ContainsKey(locationId))
            {
                result = _foodTrucksByLocationId[locationId];
            }

            return result;
        }

        public void Add(FoodTruckModel foodTruck)
        {
            Index(foodTruck);
        }
    }
}

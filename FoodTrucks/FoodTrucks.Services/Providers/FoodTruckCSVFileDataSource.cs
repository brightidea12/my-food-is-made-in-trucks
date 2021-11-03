using FoodTrucks.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoodTrucks.Services
{
    public class FoodTruckCSVFileDataSource : IFoodTruckDataSource
    {
       
        private readonly ILogger _logger;
        private readonly IFoodTruckDataCSVParser _csvParser;
        private readonly IFileReader _fileReader;

        public FoodTruckCSVFileDataSource(IFileReader fileReader, IFoodTruckDataCSVParser csvParser, ILogger<IFoodTruckDataSource> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _csvParser = csvParser ?? throw new ArgumentNullException(nameof(csvParser));
            _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        }

        public IEnumerable<FoodTruckDataModel> GetAll()
        {
            bool isCSVHeader = true;
            int foodTruckCount = 0;

            foreach (var line in _fileReader.ReadLines())
            {
                if (isCSVHeader)
                {
                    _logger.LogTrace($"CSV file header row: {line}");
                    isCSVHeader = false;
                }
                else
                {
                    var foodTruck = _csvParser.Parse(line);
                    if (foodTruck != null)
                    {
                        yield return foodTruck;
                        foodTruckCount++;
                    }
                }
            }
        }
    }

}

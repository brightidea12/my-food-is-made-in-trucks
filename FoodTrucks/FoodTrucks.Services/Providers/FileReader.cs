using FoodTrucks.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoodTrucks.Services
{
    public class FileReader : IFileReader
    {
        private readonly string _filePath;
        private readonly ILogger _logger;
        public FileReader(IOptions<FoodTruckCSVFileOptions> fileReaderConfiguration, ILogger<IFileReader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (fileReaderConfiguration.Value == null || string.IsNullOrEmpty(fileReaderConfiguration.Value?.Path))
            {
                throw new ArgumentNullException(nameof(fileReaderConfiguration));
            }
            _filePath = fileReaderConfiguration.Value.Path;

            if (!Exists())
            {
                var errMsg = $"{_filePath} does not exist.";
                _logger.LogError(errMsg);
                throw new FileNotFoundException(errMsg);
            }
        }

        public bool Exists()
        {
            return File.Exists(_filePath);
        }

        public IEnumerable<string> ReadLines()
        {
            return File.ReadLines(_filePath);
        }
    }
}

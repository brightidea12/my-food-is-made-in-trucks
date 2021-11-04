using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodTrucks.Services
{
    public class FoodTruckDataCSVParser : IFoodTruckDataCSVParser
    {
        private readonly ILogger _logger;

        public FoodTruckDataCSVParser(ILogger<IFoodTruckDataCSVParser> logger)
        {
            _logger = logger;
        }

        public FoodTruckDataModel Parse(string line)
        {
            try
            {
                if (!String.IsNullOrEmpty(line))
                {
                    var columns = Split(line);
                    return ToFoodTruckDataModel(columns);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{FoodTruckCSVParserErrorMessages.Format_Error}: {line}", ex);
            }

            return null;
        }

        private IList<string> Split(string line) //time trade off. Ideally, initial split would detect enclosing quotes.
        {
            string[] commaSplit = line.Split(',');
            List<string> quoteAdjusted = new();
            StringBuilder field = new StringBuilder(); ;
            for (int i=0; i< commaSplit.Length-1; i++)
            {
                field.Append(commaSplit[i]);
                if (field.Length > 0 && field[0] == '"')
                {
                    field = field.Remove(0, 1); //remove opening quote
                    field.Append($",{commaSplit[++i]}"); //pulls next index to concat, while also skipping index in next loop iteration.
                    field.Remove(field.Length - 1, 1); //remove closing quote.                    
                    _logger.LogTrace("Enclosed split detected, merged and restored comma: ${field}");
                }
                quoteAdjusted.Add(field.ToString());
                field.Clear();
            }

            return quoteAdjusted;         
        }

        private FoodTruckDataModel ToFoodTruckDataModel(IList<string> columns)
        {
            //there are several possible failures, including chances that int parse will fail due to bad data format. I will allow error to be thrown and caught by caller.
            return new FoodTruckDataModel(int.Parse(columns[0]), columns[1], columns[2], int.Parse(columns[3]), columns[4], columns[5], columns[6], columns[7], columns[8], columns[9], columns[10], columns[11], columns[12],
                columns[13], double.Parse(columns[14]), double.Parse(columns[15]), columns[16], columns[17], columns[18], columns[19], columns[20], int.Parse(columns[21]), columns[22], columns[23]);
        }
    }
}

using FoodTrucks.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace FoodTrucks.Tests
{
    [TestClass]
    public class FoodTruckCSVParserTests
    {

        private const string _csvRow = "519938,Bob Johnson,Truck,0,,Assessors Block 0733 / Lot010,0733010,0733,010,14MFF - 0009,EXPIRED,Watermelon,6003818.547,2113011.835,37.782143532929,-122.430449785949,http://bsm.sfdpw.org/PermitsTracker/reports/report.aspx?title=schedule&report=rptSchedule&params=permit=14MFF-0009&ExportPDF=1&Filename=14MFF-0009_schedule.pdf,Mo-Su:10AM-7PM,,02/25/2014 12:00:00 AM,2014-02-25,1,03/15/2015 12:00:00 AM,\"(37.782143532929, -122.430449785949)\"";


        [TestMethod]
        public void FoodTruckCSVParser_Should_Parse_CommaDelimited_String_And_Return_Data_Model()
        {
            var logger = new LoggerFactory().CreateLogger<IFoodTruckDataCSVParser>();
            var dataModel = new FoodTruckDataModel(519938, "Bob Johnson", "Truck", 0, "", "Assessors Block 0733 / Lot010", "0733010", "0733", "010", "14MFF - 0009", "EXPIRED", "Watermelon", "6003818.547","2113011.835", 37.782143532929, -122.430449785949,
                "http://bsm.sfdpw.org/PermitsTracker/reports/report.aspx?title=schedule&report=rptSchedule&params=permit=14MFF-0009&ExportPDF=1&Filename=14MFF-0009_schedule.pdf", "Mo-Su:10AM-7PM", "", "02/25/2014 12:00:00 AM", "2014-02-25", 1, "03/15/2015 12:00:00 AM", "(37.782143532929, -122.430449785949)");
            var parsedDataModel = new FoodTruckDataCSVParser(logger).Parse(_csvRow);
            Assert.AreEqual(dataModel, parsedDataModel);            
        }

        [TestMethod]
        public void FoodTruckCSVParser_Should_Log_FormatWarning_If_Provided_Malformed_Input_And_Return_Null()
        {
           
            var logger = new Mock<ILogger<FoodTruckDataCSVParser>>();                
            var firstComma = _csvRow.IndexOf(',');
            string malformedRow = _csvRow.Substring(firstComma + 1);
            Assert.IsNull(new FoodTruckDataCSVParser(logger.Object).Parse(malformedRow));

            logger.Verify(log => log.Log(LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(FoodTruckCSVParserErrorMessages.Format_Error)),
                 It.IsAny<Exception>(),
                 It.IsAny<Func<It.IsAnyType, Exception, string>>()));

        }

        [TestMethod]
        public void FoodTruckCSVParser_Should_Return_Null_If_Provided_An_Empty_Input()
        {
            var logger = new LoggerFactory().CreateLogger<IFoodTruckDataCSVParser>();
            Assert.IsNull(new FoodTruckDataCSVParser(logger).Parse(string.Empty));
        }

        [TestMethod]
        public void FoodTruckCSVParser_Should_Not_Split_Or_Remove_Commas_Enclosed_By_Quotes()
        {
            var logger = Mock.Of<ILogger<IFoodTruckDataCSVParser>>();
            string input = _csvRow.Replace("Bob Johnson", "\"Tate,Sean\"");            
            string expectedOutput = "Tate,Sean";
            var parsedOutput = new FoodTruckDataCSVParser(logger).Parse(input);
            Assert.AreEqual(expectedOutput, parsedOutput.Applicant);
        }
    }
}

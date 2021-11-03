using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using FoodTrucks.Common;
using System.Collections.Generic;
using System.Text;

namespace FoodTrucks.Tests.Integration
{
    [TestClass]
    public class FoodTrucksAPIIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly int _knownLocationId = 1024464;
        private readonly FoodTruckModel _newFoodTruck = new FoodTruckModel(19999999, "Bob Johnson", "Truck", 0, "", "Assessors Block 0733 / Lot010", "0733010", "0733", "010", "14MFF - 0009", "EXPIRED", "Watermelon", "6003818.547", "2113011.835", 37.782143532929, -122.430449785949,
    "website-schedule-pdf", "Mo-Su:10AM-7PM", "", "02/25/2014 12:00:00 AM", "2014-02-25", 1, "03/15/2015 12:00:00 AM", "(37.782143532929, -122.430449785949)");

        public FoodTrucksAPIIntegrationTests()
        {

            _server = new TestServer(new WebHostBuilder()
                             .ConfigureAppConfiguration((x) => { x.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false); })
            .UseStartup<Startup>()); ;
            _client = _server.CreateClient();
        }

        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTruckByLocationId_Should_Return_200_And_Response_Should_Include_FoodTruckModel_Matching_SpecifiedID()
        {

            var response = await _client.GetAsync($"/api/v1/foodtrucks/{_knownLocationId}");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var foodTruck = JsonConvert.DeserializeObject<FoodTruckModel>(stringResponse);
            Assert.IsNotNull(foodTruck);
            Assert.AreEqual(_knownLocationId, foodTruck.LocationId);
        }

        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTruckByLocationId_Should_Return_404_When_Provided_With_A_LocationId_NotMatching_Any_Registered_FoodTruck()
        {
            var response = await _client.GetAsync($"/api/v1/foodtrucks/59999999");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTruckByLocationId_Should_Return_400_When_Provided_With_An_Invalid_LocationId()
        {
            var response = await _client.GetAsync($"/api/v1/foodtrucks/0");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTrucksByBlock_Should_Return_200_When_Provided_With_A_Block_Containing_FoodTrucks_And_IncludeAList_Of_FoodTrucks_On_ThatBlock()
        {
            string block = "3707";
            var response = await _client.GetAsync($"/api/v1/foodtrucks?block={block}");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var foodTrucks = JsonConvert.DeserializeObject<List<FoodTruckModel>>(stringResponse);
            Assert.IsNotNull(foodTrucks);            
            Assert.IsTrue(foodTrucks.Count == 4);
            Assert.IsTrue(foodTrucks.TrueForAll(p => p.Block == block));            
        }

        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTrucksByBlock_Should_Return_404_When_Provided_With_An_Id_NotMatching_Any_Blocks_With_Registered_FoodTrucks()
        {
            string block = "NEVERVALID";
            var response = await _client.GetAsync($"/api/v1/foodtrucks?block={block}");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound); 
        }


        [TestMethod]
        public async Task FoodTrucksAPI_GetFoodTrucksByBlock_Should_Return_400_When_No_Block_Is_Specified_In_The_Request()
        {
            string block = "";
            var response = await _client.GetAsync($"/api/v1/foodtrucks?block={block}");
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task FoodTrucksAPI_AddFoodTruck_Should_Return_201_When_Provided_A_Valid_FoodTruckModel_And_The_FoodTruck_Should_Be_Retrievable_Via_Location_And_Block_Endpoints_After_Creation()
        {
            var response = await _client.PostAsync($"/api/v1/foodtrucks",  new StringContent(JsonConvert.SerializeObject(_newFoodTruck), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Created);
            var locationResponse = await _client.GetAsync($"/api/v1/foodtrucks/{_newFoodTruck.LocationId}");
            var blockResponse = await _client.GetAsync($"/api/v1/foodtrucks?block={_newFoodTruck.Block}");
            Assert.AreEqual(locationResponse.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(blockResponse.StatusCode, System.Net.HttpStatusCode.OK);


            var stringLocationResponse = await locationResponse.Content.ReadAsStringAsync();
            var foodTruck = JsonConvert.DeserializeObject<FoodTruckModel>(stringLocationResponse);
            Assert.IsNotNull(foodTruck);
            Assert.AreEqual(_newFoodTruck.LocationId, foodTruck.LocationId);

            var stringBlockResponse = await blockResponse.Content.ReadAsStringAsync();
            var foodTrucks = JsonConvert.DeserializeObject<List<FoodTruckModel>>(stringBlockResponse);
            Assert.IsNotNull(foodTrucks);
            Assert.IsTrue(foodTrucks.Count == 3);
            Assert.IsTrue(foodTrucks.TrueForAll(p => p.Block == _newFoodTruck.Block));

        }

        [TestMethod]
        public async Task FoodTrucksAPI_AddFoodTruck_Should_Return_409_When_Provided_A_Valid_FoodTruckModel_That_Contains_A_LocationId_Of_A_Previously_Registered_FoodTruck()
        {
            var newFoodTruck = _newFoodTruck with { LocationId = _knownLocationId };

            var response = await _client.PostAsync($"/api/v1/foodtrucks", new StringContent(JsonConvert.SerializeObject(newFoodTruck), Encoding.UTF8, "application/json"));
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Conflict);
        }

        [TestMethod]
        public async Task FoodTrucksAPI_AddFoodTruck_Should_Return_400_When_Provided_A_Invalid_FoodTruckModel()
        {
            var response = await _client.PostAsync($"/api/v1/foodtrucks", new StringContent("{'Truck':'BadTruck'}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

    }
}

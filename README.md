# My Food is Made in trucks

## Overview

MFT is an HTTP-accessible API designed to provide consumers with information about food trucks. The API exposes endpoints that allow consumers to retrieve food trucks by locationId (unique identifier) and a collection of trucks located on a specific city block. The API also enables the registration of new food trucks.

## MFT is built using

- Frameworks: .Net 5.0, ASP.NET WebAPI
- Languages: C# 
- Databases: In-Memory (Volatile)
- Tools: Visual Studio 2019, Swagger
- Testing: Unit and Integration


## High-level design approach

MFT's data source is a CSV data file containing a list of registered food trucks. At startup, MFT reads the CSV file, parses it, and stores it in memory as a pair of dictionaries.

Since the API only allows retrieval by `locationId` and `block`, using these as dictionary keys enabled O(1) time complexity for data retrieval.

In recognization of the future potential size of the data source, the solution is comprised of several layers. The file retrieval and parsing components are designed to be incremental. Each line is read, parsed, and returned to the consumer as an enumerable with a yield return. Therefore, as the dataset grows, the data service could be modified to store the data in a method better suited to large collections of data (i.e., persistence to non-volatile storage, etc.) without impacting the other layers of this solution.


## REST API Specifications

> Retrieves a food truck based on the `locationId`

### Request

GET: /api/v1/FoodTrucks/{locationId}

|   Name	|   Data Type	|  Location | Example  |
|---	|---	|--- |--- |
|   locationId	|   int32	 | Path  |1024464|

### Responses

**Success**
> HTTP Code: 200

```
FoodTruckModel{
locationId	integer($int32)
applicant	string
nullable: true
facilityType	string
nullable: true
cnn	integer($int32)
locationDescription	string
nullable: true
address	string
nullable: true
blockLot	string
nullable: true
block	string
nullable: true
lot	string
nullable: true
permit	string
nullable: true
status	string
nullable: true
foodItems	string
nullable: true
x	string
nullable: true
y	string
nullable: true
latitude	number($double)
longitude	number($double)
schedule	string
nullable: true
daysHours	string
nullable: true
noiSent	string
nullable: true
approved	string
nullable: true
received	string
nullable: true
priorPermit	integer($int32)
expirationDate	string
nullable: true
location	string
nullable: true
}
```

**Errors**

|   Code	|  Description  |
|---	|---	
| 404	| No Food Truck with locationId found |
| 400	| Invalid locationId (i.e. positive int32 value > 0)

---

> Get all food trucks for a given `block`

### Request

GET: /api/v1/FoodTrucks?block=

|   Name	|   Data Type	|  Location | Example  |
|---	|---	|--- |--- |
|   block	|   string	 | query  |"3707"|

### Responses

**Success**
> HTTP Code: 200

```
[FoodTruckModel{...}]
```
*See FoodTruckModel definition in /api/v1/FoodTrucks/{locationId} response specification*


**Errors**

|   Code	|  Description  |
|---	|---	
| 404	| No food trucks with registered on specified block |
| 400	| Invalid block (i.e. empty or non-string value)

---

> Add a new food trucks

### Request

POST: /api/v1/FoodTrucks

|   Name	|   Data Type	|  Location | Example  |
|---	|---	|--- |--- |
|   Body	|   FoodTruckModel	 | Body  | See FoodTruckModel definition|

### Responses

**Success**
> HTTP Code: 201

**Errors**

|   Code	|  Description  |
|---	|---
| 409   | Conflict - locationId must be unique	
| 404	| No food trucks with registered on specified block |
| 400	| Invalid block (i.e. empty or non-string value)

## Required Configuration

Before running this solution, please modify FoodTruckCSVFile:Path inside of appsettings.json to point to the location of a CSV file containing the dataset. [A sample file has been included in the root of this repository](https://github.com/brightidea12/my-food-is-made-in-trucks/blob/main/Mobile_Food_Facility_Permit.csv). 

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "FoodTruckCSVFile": {
    "Path": "<INSERT_PATH_HERE>\\Mobile_Food_Facility_Permit.csv"
  },
    "AllowedHosts": "*"
  }

```

## Testing

 > my-food-is-made-in-trucks/FoodTrucks/FoodTrucks.Tests/

The solution currently includes a small set of unit tests that focuses on verifying the functionaily of the CSV parsing process.

> my-food-is-made-in-trucks/FoodTrucks/FoodTrucks.Tests.Integration/

The solution also includes a battery of integration tests to verify the API against its published specification. It also serves to test the solution in its entirety.


## Running

This solution was build using .NET 5.0, it can be built, tested, and ran using dotnet CLI commands or from Visual Studio 2019 or later.

### Example of running solution from dotnet CLI

```
my-food-is-made-in-trucks\FoodTrucks\FoodTrucks> dotnet run
Building...
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\dev\interview\my-food-is-made-in-trucks\FoodTrucks\FoodTrucks
```

### Example of test execution

`my-food-is-made-in-trucks\FoodTrucks> dotnet test`

`Passed!  - Failed:     0, Passed:     4, Skipped:     0, Total:     4, Duration: 212 ms - FoodTrucks.Tests.Unit.dll (net5.0)`

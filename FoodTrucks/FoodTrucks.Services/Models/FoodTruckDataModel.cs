namespace FoodTrucks.Services
{
    public record FoodTruckDataModel(int locationId, string Applicant, string FacilityType, int cnn, string LocationDescription, string Address, string blocklot, string block, string lot, string permit,
        string Status, string FoodItems, string X, string Y, double Latitude, double Longitude, string Schedule, string daysHours, string NOISent, string Approved, string received, int PriorPermit, string ExpirationDate, string Location);
}

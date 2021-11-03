namespace FoodTrucks.Common
{
    public record FoodTruckModel(int LocationId, string Applicant, string FacilityType, int Cnn, string LocationDescription, string Address, string BlockLot, string Block, string Lot, string Permit,
        string Status, string FoodItems, string X, string Y, double Latitude, double Longitude, string Schedule, string DaysHours, string NOISent, string Approved, string Received, int PriorPermit, string ExpirationDate, string Location);
}

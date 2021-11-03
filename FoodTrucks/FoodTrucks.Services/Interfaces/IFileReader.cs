using System.Collections.Generic;

namespace FoodTrucks.Services
{
    public interface IFileReader
    {
        bool Exists();
        IEnumerable<string> ReadLines();
    }
}

using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // Creates a private readonly property called Current
        // every time this is called, and instance of CitiesData is called
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        // Public property that is available in any instance of the application
        public List<CityDto> Cities { get; set; }

        // Checks if an ID exists
        // Returns null if an ID does not exists
        public static CityDto? GetCity(int id)
        {
            
            return Current.Cities.Find(x => x.Id == id);
        }
       

        private CitiesDataStore() 
        {
            
            Cities = new List<CityDto>()
            {
                new CityDto {Name = "Iligan City", Id = 1},
                new CityDto {Id = 2, Name = "Cagayan City "}
            };

        }
    }
}

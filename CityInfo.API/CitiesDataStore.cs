using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // Creates a private readonly property called Current
        // every time this is called, and instance of CitiesData is called
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        // Public property that is available in any instance of the application
        public List<CityDto> Cities { get; set; }

        // Checks if an ID exists
        // Returns null if an ID does not exists
        //public static CityDto? GetCity(int id)
        //{
            
        //    return Current.Cities.Find(x => x.Id == id);
        //}
       

        public CitiesDataStore() 
        {

            Cities = new List<CityDto>()
            {
                new CityDto
                {
                    Id = 1,
                    Name = "Iligan City",
                    Description = "Known as the 'City of Majestic Waterfalls'.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto {
                            Id = 1,
                            Name = "Maria Cristina Falls",
                            Description = "A majestic waterfall and a vital source of hydroelectric power."
                        },
                        new PointsOfInterestDto {
                            Id = 2,
                            Name = "Tinago Falls",
                            Description = "A hidden gem with a stunning curtain-like cascade."
                        }
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Cagayan de Oro City",
                    Description = "The 'City of Golden Friendship' and a major gateway to Northern Mindanao.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto {
                            Id = 3,
                            Name = "MacArthur Memorial Park",
                            Description = "A historical park commemorating General Douglas MacArthur's landing."
                        },
                        new PointsOfInterestDto {
                            Id = 4,
                            Name = "Whitewater Rafting at Cagayan River",
                            Description = "Experience thrilling rapids in one of the Philippines' best rafting destinations."
                        },
                        new PointsOfInterestDto {
                            Id = 5,
                            Name = "Gardens of Malasag Eco-Tourism Village",
                            Description = "A scenic park showcasing the culture and nature of Northern Mindanao."
                        }
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Davao City",
                    Description = "The largest city in the Philippines by land area and known for its durian fruit.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto {
                            Id = 6,
                            Name = "Mount Apo",
                            Description = "The highest mountain in the Philippines, popular for hiking and trekking."
                        },
                        new PointsOfInterestDto {
                            Id = 7,
                            Name = "Philippine Eagle Center",
                            Description = "A conservation center dedicated to the endangered Philippine Eagle."
                        },
                        new PointsOfInterestDto {
                            Id = 8,
                            Name = "Eden Nature Park",
                            Description = "A mountain resort offering various attractions and stunning views."
                        }
                    }
                },
                new CityDto
                {
                    Id = 4,
                    Name = "Zamboanga City",
                    Description = "Known as the 'City of Flowers' and famous for its vintas and rich cultural heritage.",
                    PointsOfInterest = new List<PointsOfInterestDto>()
                    {
                        new PointsOfInterestDto {
                            Id = 9,
                            Name = "Fort Pilar",
                            Description = "A 17th-century Spanish fort and a significant historical landmark."
                        },
                        new PointsOfInterestDto {
                            Id = 10,
                            Name = "Great Santa Cruz Island",
                            Description = "Famous for its pink sand beach."
                        }
                    }
                }
            };

        }
    }
}

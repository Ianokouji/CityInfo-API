using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;
        public CitiesController(CitiesDataStore citiesDataStore) 
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            //return new JsonResult(new List<object> {
            //    new { id = 1, name = "Iligan City" },
            //    new { id = 2, age = "Cagayan City"}
            //});

            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            //return new JsonResult(CitiesDataStore.GetCity(id));
            CityDto? city = _citiesDataStore.Cities.Find(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

      

    }
}

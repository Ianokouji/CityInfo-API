using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {

        [HttpGet]
        public JsonResult GetCities()
        {
            //return new JsonResult(new List<object> {
            //    new { id = 1, name = "Iligan City" },
            //    new { id = 2, age = "Cagayan City"}
            //});

            return new JsonResult(CitiesDataStore.Current.Cities);
        }
        
    }
}

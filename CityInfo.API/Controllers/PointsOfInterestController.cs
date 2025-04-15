using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/points-of-interest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore _citiesDataStore;

        // Constructor injection for dependencies
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterestDto>> GetCityPointOfInterests(int cityId)
        {
                
            try
            {

                // Check if cityId exits
                CityDto? city = _citiesDataStore.Cities.Find(city => city.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"No matches for cityId {cityId} in records");
                    return NotFound();
                }

                // Get the points of interests of a specific City
                IEnumerable<PointsOfInterestDto> pointsOfInterests = city.PointsOfInterest;

                if (pointsOfInterests == null)
                {
                    return NotFound();
                }

                // If it exists then return
                return Ok(pointsOfInterests);
            }

            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city Id {cityId}", ex);
                return StatusCode(500, "A problem happend while handling your request");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointsOfInterestDto> GetCityPointOfInterest([FromRoute] int cityId, [FromRoute] int pointOfInterestId)
        {
            // Check if cityId exits
            CityDto? city = _citiesDataStore.Cities.Find(x => x.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"No matches for cityId {cityId} in records");
                return NotFound();
            }

            // Get the specified points of interest of a specific City
            PointsOfInterestDto? pointsOfInterest = city.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestId);

            if (pointsOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointsOfInterest);
        }

        [HttpPost]
        public ActionResult<PointsOfInterestDto> CreatePointOfInterest([FromRoute] int cityId, [FromBody] PointsOfInterestCreationDto pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Validate cityId and retrieve valid city
            CityDto? validCity = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId); 
            if (validCity == null)
            {
                _logger.LogInformation($"No matches for cityId {cityId} in records");
                return NotFound();
            }

            // Get the max PointOfInterestId for that city
            int maxPointsOfInterestId = validCity.PointsOfInterest.Max(x => x.Id);

            // Create new PointOfInterest
            PointsOfInterestDto newPointOfInterest = new PointsOfInterestDto()
            {
                Id = maxPointsOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            // Add as new point of interest for the valid city
            validCity.PointsOfInterest.Add(newPointOfInterest);

            // OPTION 1: Return newly created point of interest object
            //return Ok(newPointOfInterest);

            // OPTION 2: Return  with the route where the newly created point of intereset could be found
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = newPointOfInterest.Id,

                },
                    newPointOfInterest
                );
        }


        [HttpPut("{pointOfInterestId}")]
        public IActionResult UpdatePointOfInterest([FromRoute] int cityId, [FromRoute] int pointOfInterestId, [FromBody] PointsOfInterestUpdateDto updatedPointOfInterest)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            // Validate cityId and retrieve valid city
            CityDto? validCity = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId); 
            if (validCity == null) 
            {
                _logger.LogInformation($"No matches for cityId {cityId} in records");
                return NotFound(); 
            }

            // Get valid point of interest
            PointsOfInterestDto? validPointOfInterest = validCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestId); if (validPointOfInterest == null) return NotFound();

            // Update the actual content
            validPointOfInterest.Name = updatedPointOfInterest.Name;
            validPointOfInterest.Description = updatedPointOfInterest.Description;

            return NoContent();


        }

        [HttpPatch("{pointOfInterestId}")]
        public IActionResult PatchPointOfInterest([FromRoute] int cityId, [FromRoute] int pointOfInterestId, [FromBody] JsonPatchDocument<PointsOfInterestUpdateDto> patchDocument)
        {
            // Validate cityId and retrieve valid city
            CityDto? validCity = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (validCity == null)
            {
                _logger.LogInformation($"No matches for cityId {cityId} in records");
                return NotFound();
            }

            // Get valid point of interest
            PointsOfInterestDto? validPointOfInterest = validCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestId); if (validPointOfInterest == null) return NotFound();


            // Convert DTO type for update
            PointsOfInterestUpdateDto convertedPointOfInterestForPatch = new PointsOfInterestUpdateDto()
            {
                Name = validPointOfInterest.Name,
                Description = validPointOfInterest.Description,
            };

            // Apply the patch transformations on valid point of interest 
            patchDocument.ApplyTo(convertedPointOfInterestForPatch, ModelState);

            // Validates as long as the fields are valid (Before update validation)
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            // Validates thoroughly and checks if it is valid even (After update validation)
            if (!TryValidateModel(convertedPointOfInterestForPatch)) { return BadRequest(ModelState); }

            // Actual update from the data store
            validPointOfInterest.Name = convertedPointOfInterestForPatch.Name;
            validPointOfInterest.Description = convertedPointOfInterestForPatch.Description;

            return NoContent();

        }



        [HttpDelete("{pointOfInterestId}")]
        public IActionResult DeletePointOfInterest([FromRoute] int cityId, [FromRoute] int pointOfInterestId)
        {

            try
            {

                // Validate cityId and retrieve valid city
                CityDto? validCity = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
                if (validCity == null)
                {
                    _logger.LogInformation($"No matches for cityId {cityId} in records");
                    return NotFound(new { message = $"No matches for cityId {cityId} in records" });
                }

                // Get valid point of interest
                PointsOfInterestDto? validPointOfInterest = validCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestId);
                if (validPointOfInterest == null)
                {

                    return NotFound(new { message = $"No matches for pointOfInterestId {pointOfInterestId} in records" });
                }

                // Actual Deletion
                validCity.PointsOfInterest.Remove(validPointOfInterest);

                _mailService.Send(subject: "Point of Interest Deletion", message: $"Point of Interest with name {validPointOfInterest.Name} and id {validPointOfInterest.Id} has been deleted");
                return NoContent();

            }
            catch (Exception ex) 
            {
                _logger.LogError($"Exception while detecting point of interest: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An unexpected error occurred while processing your request.",
                    error = ex.Message,
                });
            }


        }
    }
}

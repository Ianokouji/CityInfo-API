using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestUpdateDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name should not be more than 50 characters"),
         MinLength(2, ErrorMessage = "Name should at least be 2 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Your description should not be more than 200 characters")]
        public string? Description { get; set; }
    }
}

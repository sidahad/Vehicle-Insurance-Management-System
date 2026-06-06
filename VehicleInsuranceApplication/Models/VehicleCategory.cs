using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceApplication.Models
{
    public class VehicleCategory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}

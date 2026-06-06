using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceApplication.Models
{
    public class User
    {
        [Key]

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }
    }
}

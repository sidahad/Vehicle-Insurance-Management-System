using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleInsuranceApplication.Models
{
    public class Vehicle
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public decimal Rate { get; set; }

    public string BodyNumber { get; set; } = string.Empty;
    public string EngineNumber { get; set; } = string.Empty;
    public string VehicleNumber { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public VehicleCategory? Category { get; set; }

    public ICollection<Policy> Policies { get; set; } = new List<Policy>();

    // Foreign key for WebUser
    public int Uid { get; set; }

    [ForeignKey("Uid")]
    public WebUser? WebUser { get; set; }
 }
}

//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace VehicleInsuranceApplication.Models
//{
//    public class Vehicle
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string Name { get; set; } = string.Empty;

//        public string OwnerName { get; set; } = string.Empty;
//        public string Model { get; set; } = string.Empty;
//        public string Version { get; set; } = string.Empty;

//        public decimal Rate { get; set; }

//        public string BodyNumber { get; set; } = string.Empty;
//        public string EngineNumber { get; set; } = string.Empty;
//        public string VehicleNumber { get; set; } = string.Empty;

//        public int CategoryId { get; set; }
//        public VehicleCategory? Category { get; set; }

//        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
//    }
//}

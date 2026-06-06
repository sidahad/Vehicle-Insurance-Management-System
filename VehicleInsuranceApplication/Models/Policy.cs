using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleInsuranceApplication.Models
{
    public class Policy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VehicleName { get; set; } = string.Empty; // User inputs this directly

        public string PolicyNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}




//{

//        public int Id { get; set; }
//public int CustomerId { get; set; }
//public Customer? Customer { get; set; } // ✅ Nullable

//public int VehicleId { get; set; }
//public Vehicle? Vehicle { get; set; } // ✅ Nullable

//public string PolicyNumber { get; set; } = string.Empty;
//public DateTime StartDate { get; set; }
//public DateTime EndDate { get; set; }
//public decimal Amount { get; set; }

//public ICollection<Claim> Claims { get; set; } = new List<Claim>();
//public ICollection<Payment> Payments { get; set; } = new List<Payment>();
//	}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleInsuranceApplication.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public int Uid { get; set; }

        [Required]
        [ForeignKey("Uid")]
        public WebUser WebUser { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Required]
        [ForeignKey("PolicyId")]
        public Policy Policy { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal AmountPaid { get; set; } // Auto-filled based on selected policy

        [Required]
        public int ClaimableAmount { get; set; }
        [Required]
        public string Reason { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}


//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace VehicleInsuranceApplication.Models
//{
//    public class Claim
//    {
//        public int Id { get; set; }

//        [Required]
//        public int PolicyId { get; set; }
//        public Policy Policy { get; set; }
//        [Required]
//        public int InsuredAmount { get; set; }
//        [Required]
//        public int ClaimableAmount { get; set; }
//        [Required]
//        public string Reason { get; set; }
//        public string ClaimStatus { get; set; } = "Pending";
//        public DateTime CreatedAt { get; set; } = DateTime.Now;
//    }
//}



//using System.ComponentModel.DataAnnotations;

//namespace VehicleInsuranceApplication.Models
//{
//    public class Claim
//    {
//        [Key]
//        public int Id { get; set; }

//        public int PolicyId { get; set; }
//        public required Policy Policy { get; set; }

//        public string ClaimNumber { get; set; } = string.Empty;
//        public DateTime DateOfAccident { get; set; }
//        public string PlaceOfAccident { get; set; } = string.Empty;
//        public decimal InsuredAmount { get; set; }
//        public decimal ClaimableAmount { get; set; }
//    }
//}
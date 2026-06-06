using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceApplication.Models.ViewModels
{
    public class CreateClaimViewModel
    {
        public int Uid { get; set; }
        [Required]
        public int PolicyId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int ClaimableAmount { get; set; }
        [Required]
        public string Reason { get; set; }
        public string ClaimStatus { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<WebUser> WebUsers { get; set; } = new();
        public List<Policy> Policies { get; set; } = new();
    }
}

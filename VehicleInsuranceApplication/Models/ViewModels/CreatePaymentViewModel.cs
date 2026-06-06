using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceApplication.Models.ViewModels
{
    public class CreatePaymentViewModel
    {
        public int Uid { get; set; }
        public int PolicyId { get; set; }
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "Credit Card"; // Default

        public DateTime PaymentDate { get; set; } = DateTime.Now; // Auto-set date

        public List<WebUser> WebUsers { get; set; } = new();
        public List<Policy> Policies { get; set; } = new();
    }
}

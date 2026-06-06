using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleInsuranceApplication.Models
{
    public class Payment
    {
        [Key]
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
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}

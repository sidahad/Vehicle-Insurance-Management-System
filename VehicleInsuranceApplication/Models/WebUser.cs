using System.ComponentModel.DataAnnotations;

namespace VehicleInsuranceApplication.Models
{
    public class WebUser
    {
        [Key]
        public int Uid { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public List<Claim>? Claims { get; set; }

    }
}
//{
//        public int Id { get; set; }

//public string? Name { get; set; }

//public string? Password { get; set; }
//    }


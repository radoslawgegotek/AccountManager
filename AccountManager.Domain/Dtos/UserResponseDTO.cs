using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Domain.Dto
{
    public class UserResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PESEL { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }
        public double? AvgPowerConsumption { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AccountManager.Domain.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(11)]
        [Column(TypeName = "varchar(11)")]
        public string PESEL { get; set; }
        [Required]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        [ValidateNever]
        public string? NormalizedEmail { get; set; }
        public int? Age { get; set; }
        public double? AvgPowerConsumption { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTest.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        [MaxLength(10)]
        public string MobileNumber { get; set; }

        [MaxLength(256)]
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
       
    }
}

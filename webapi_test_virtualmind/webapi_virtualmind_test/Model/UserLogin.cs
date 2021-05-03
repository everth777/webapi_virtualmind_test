using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Model
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}

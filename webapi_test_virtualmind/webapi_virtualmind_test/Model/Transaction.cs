using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Model
{
    public class Transaction
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public double Amount { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Currency { get; set; }
        
        public DateTime Date { get; set; }
    }
}

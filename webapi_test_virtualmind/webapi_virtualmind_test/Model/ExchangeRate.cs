using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Model
{
    public class ExchangeRate
    {
        
        public long Id { get; set; }
        
        public double Buying {get; set;}
        
        public double Selling { get; set; }
        
        public String Date { get; set; }
        
    }
}

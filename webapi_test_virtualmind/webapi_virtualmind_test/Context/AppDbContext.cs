using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_virtualmind_test.Model;

namespace webapi_virtualmind_test.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {            
        }
                
        public DbSet<Transaction> Transaction { get; set; }
    }
}

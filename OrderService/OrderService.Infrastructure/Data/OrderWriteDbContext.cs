using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Data
{
    public class OrderWriteDbContext : DbContext
    {
        public OrderWriteDbContext(DbContextOptions<OrderWriteDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}

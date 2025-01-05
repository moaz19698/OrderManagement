using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
   

    public class OrderRepository : IOrderRepository
    {
        private readonly OrderWriteDbContext _writeContext;
        private readonly OrderReadDbContext _readContext;

        public OrderRepository(OrderWriteDbContext writeContext, OrderReadDbContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
        }

        // Write operations
        public async Task AddAsync(Order order)
        {
            await _writeContext.Orders.AddAsync(order);
            await _writeContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _writeContext.Orders.Update(order);
            await _writeContext.SaveChangesAsync();
        }

        // Read operations
        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _readContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _readContext.Orders.ToListAsync();
        }
    }

}

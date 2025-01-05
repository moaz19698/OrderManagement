using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository
    {
        // Write operations
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);

        // Read operations
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();

    }

}

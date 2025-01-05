using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByIdHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.Id} not found.");
            }
            return order;
        }
    }
}

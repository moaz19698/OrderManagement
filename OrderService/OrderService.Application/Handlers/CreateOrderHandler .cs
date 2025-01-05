using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = request.CustomerName,
                CreatedAt = request.CreatedAt,
                Status = "New"
            };

            await _repository.AddAsync(order);
            return order.Id;
        }
    }

}

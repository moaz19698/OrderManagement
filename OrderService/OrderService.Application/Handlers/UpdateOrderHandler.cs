using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand,Unit>
    {
        private readonly IOrderRepository _repository;

        public UpdateOrderHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.CustomerName = request.CustomerName;
            await _repository.UpdateAsync(order);

            return Unit.Value;
        }
    }

}

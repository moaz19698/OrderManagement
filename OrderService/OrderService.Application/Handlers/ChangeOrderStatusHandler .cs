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
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OrderService.Application.Interfaces;
    using OrderService.Domain.Entities;

    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusCommand, Unit>
    {
        private readonly IOrderRepository _repository;

        public ChangeOrderStatusHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.Status = request.Status;
            await _repository.UpdateAsync(order);

            return Unit.Value;
        }
    }


}

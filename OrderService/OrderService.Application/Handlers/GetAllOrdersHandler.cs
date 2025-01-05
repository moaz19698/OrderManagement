using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Handlers
{

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _repository;

        public GetAllOrdersHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }

}

using MediatR;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Queries
{
    public class GetOrderByIdQuery : IRequest<Order>
    {
        public Guid Id { get; set; }

        public GetOrderByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

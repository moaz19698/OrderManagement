using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Commands
{
    public class ChangeOrderStatusCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }

}

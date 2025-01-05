using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.Queries;

namespace OrderService.Api.Controllers
{
    namespace OrderService.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class OrdersController : ControllerBase
        {
            private readonly IMediator _mediator;

            public OrdersController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPost]
            public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetOrderById), new { id = result }, result);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderCommand command)
            {
                command.Id = id;
                await _mediator.Send(command);
                return NoContent();
            }

            [HttpPatch("{id}/status")]
            public async Task<IActionResult> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusCommand command)
            {
                command.Id = id;
                await _mediator.Send(command);
                return NoContent();
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetOrderById(Guid id)
            {
                var query = new GetOrderByIdQuery(id);
                var order = await _mediator.Send(query);
                return Ok(order);
            }

            [HttpGet]
            public async Task<IActionResult> GetAllOrders()
            {
                var query = new GetAllOrdersQuery();
                var orders = await _mediator.Send(query);
                return Ok(orders);
            }
        }
    }
}

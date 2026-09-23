using Microsoft.AspNetCore.Mvc;
using OrderService.Models;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController: ControllerBase
    {
        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(new[]
            {
                new Order
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 2
                },
                new Order
                {
                    Id = 2,
                    ProductId = 2,
                    Quantity = 1
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = new Order {

                Id = id,
                ProductId =  1,
                Quantity = 2
            };

            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            return Ok(order);
        }
    }
}

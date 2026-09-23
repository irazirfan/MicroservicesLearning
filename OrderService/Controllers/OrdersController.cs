using Microsoft.AspNetCore.Mvc;
using OrderService.Models;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController: ControllerBase
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;

        public OrdersController(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(new[]
            {
        new Order { Id = 1, ProductId = 1, Quantity = 2 },
        new Order { Id = 2, ProductId = 2, Quantity = 1 }
    });
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = new Order
            {
                Id = id,
                ProductId = 1,
                Quantity = 2
            };

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var client = _httpClientFactory.CreateClient();

            var productServiceUrl = _configuration["Services:ProductService"];

            var productResponse = await client.GetAsync($"{productServiceUrl}/api/products/{order.ProductId}");

            if (!productResponse.IsSuccessStatusCode)
            {
                return BadRequest("Product not found");
            }

            var product = await productResponse.Content.ReadFromJsonAsync<Product>();

            return Ok(new
            {
                Message = "Order created",
                Product = product,
                Quantity = order.Quantity
            });
        }

    }
}

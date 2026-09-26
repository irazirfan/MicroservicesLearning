using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController: ControllerBase
    {
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        private OrderDbContext _db;

        public OrdersController(IHttpClientFactory httpClientFactory, IConfiguration configuration, OrderDbContext db) 
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _db.Orders.ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var client = _httpClientFactory.CreateClient("ProductServiceClient");

            var productServiceUrl = _configuration["Services:ProductService"];

            HttpResponseMessage productResponse;

            try
            {
                productResponse = await client.GetAsync($"{productServiceUrl}/api/products/{order.ProductId}");
            }
            catch (TaskCanceledException)
            {
                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "ProductService took too long to respond");
            }
            catch (HttpRequestException)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    "ProductService is currently unavailable");
            }

            if (!productResponse.IsSuccessStatusCode)
            {
                return BadRequest("Product not found");
            }

            var product = await productResponse.Content.ReadFromJsonAsync<Product>();

            _db.Orders.Add(order);

            await _db.SaveChangesAsync();

            return Ok(new
            {
                Message = "Order created",
                Order = order,
                Product = product,
                Quantity = order.Quantity
            });
        }

    }
}

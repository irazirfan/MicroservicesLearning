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

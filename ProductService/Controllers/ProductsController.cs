using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products =
    [
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1000
        },
        new Product
        {
            Id = 2,
            Name = "Keyboard",
            Price = 50
        },
        new Product
        {
            Id = 3,
            Name = "Mouse",
            Price = 25
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return Ok(Products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
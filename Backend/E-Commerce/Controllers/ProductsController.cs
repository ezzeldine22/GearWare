using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductsController : ControllerBase
    {
        private readonly EcommerceDbContext _dbContext;

        public ProductsController(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]

        public ActionResult<IEnumerable<Product>> GetProdcts()
        {
            var _products = _dbContext.Set<Product>().ToList();
            return Ok(_products);
        }

        [HttpPost]
        [Route("")]

        public ActionResult<int> AddProduct(Product product)
        {
            product.ProductId = 0;
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return Ok(product.ProductId);
        }

        //[HttpPut]
        //[Route("{id}")]

        //public  ActionResult EditProduct(int id ,)
        //{


        //    return Ok();
        //}

    }
}

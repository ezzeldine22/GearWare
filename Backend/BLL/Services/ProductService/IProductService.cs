using BLL.DTOs.ProductDtos;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ProductService
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductDto _addProductDto);
        Task DeleteProductAsync();
        Task EditProductAsync();

        Task GetProductByIdAsync();
        Task<IEnumerable<Product>> GetAllProductsAsync(GetAllProductsDto productDto);

    }
}

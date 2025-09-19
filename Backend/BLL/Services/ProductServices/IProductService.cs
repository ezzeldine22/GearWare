using BLL.DTOs.ProductDtos;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ProductServices
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductDto _addProductDto);
        Task DeleteProductAsync(int id);
        Task EditProductAsync(EditProductDto editProductDto);
        Task<GetProductByIdDto> GetProductByIdAsync(int Id);
        IEnumerable<GetAllProductsDto> GetAllProducts();
        Task<IEnumerable<GetAllProductsDto>> SearchProductsPagedAsync(string query, int PageNumber);

    }
}

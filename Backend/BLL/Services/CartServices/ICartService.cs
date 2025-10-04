using BLL.DTOs.CartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.CartServices
{
    public interface ICartService
    {
        Task AddToCart(AddToCartDto addToCartDto);
        Task UpdateQuantity(UpdateQuantityDto updateQuantityDto);
        IEnumerable<GetAllCartItemsDto> GetAllCartItems();
        Task DeleteCartItem(int id);
    }
}

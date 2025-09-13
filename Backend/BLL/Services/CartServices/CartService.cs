using BLL.DTOs.CartDtos;
using BLL.DTOs.CategoryDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<CartItem> _cartItemRepo;


        public CartService(IRepository<Cart> CartRepo , IRepository<CartItem> CartItemRepo)
        {
            _cartRepo = CartRepo;
            _cartItemRepo = CartItemRepo;
        }
        public async Task AddToCart(AddToCartDto addToCartDto)
        {
            var CartItem = new CartItem
            {
                CartId = addToCartDto.CartId,
                ProductId = addToCartDto.ProductId,
                Quantity = addToCartDto.Quantity,
            };

            await _cartItemRepo.AddAsync(CartItem);
            _cartItemRepo.SaveChanges();

        }
        public async Task UpdateQuantity(UpdateQuantityDto updateQuantityDto)
        {
            var CartItem = await _cartItemRepo.FirstOrDefaultAsync(c => c.CartItemId == updateQuantityDto.CartItemId);

            CartItem.Quantity = updateQuantityDto.Quantity;

            _cartItemRepo.SaveChanges();

        }

        public  IEnumerable<GetAllCartItemsDto> GetAllCartItems()
        {
            var AllCartItems = _cartItemRepo.ReadAll();

            if (AllCartItems == null)
            {
                throw new CustomException(new List<string> { "No CartItems To Show !!!" });
            }

            var AllCartItemsDto = AllCartItems.Select(c => new GetAllCartItemsDto
            {
                CartItemDescription = c.Product.Description,
                CartItemPrice = c.Product.Price,
                
            }).ToList();

            return  AllCartItemsDto;
        }

        public async Task DeleteCartItem(int id)
        {
            await _cartItemRepo.DeleteAsync(id);
            _cartItemRepo.SaveChanges();
        }
        

        
    }
}

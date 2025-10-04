using BLL.DTOs.OrderDtos;
using BLL.DTOs.WishListDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.WishListService
{
    public class WishListService : IWishListService
    {
        private readonly IRepository<Wishlist> _wishListRepo;

        public WishListService(IRepository<Wishlist> wishListRepo)
        {
            _wishListRepo = wishListRepo;
        }

        public void AddItemToWishList(int productId, string ClientId)
        {
            var newWishList = new Wishlist
            {
                ProductId = productId,
                UserId = ClientId
            };

            _wishListRepo.AddAsync(newWishList);
            _wishListRepo.SaveChanges();
        }

        public IEnumerable<GetAllWishListItemDtos> GetAllWishListItems(string clientId)
        {
            var AllWishListItems = _wishListRepo.ReadAll().Include(wl => wl.User).Where(wl => wl.UserId == clientId);

            if (AllWishListItems == null)
            {
                throw new CustomException(new List<string> { "No Items To Show !!!" });
            }

            var AllWishListItemsDto = AllWishListItems.Select(o => new GetAllWishListItemDtos
            {
                description = o.Product.Description,
                price = o.Product.Price
            }).ToList();

            return AllWishListItemsDto;
        }
    }
}

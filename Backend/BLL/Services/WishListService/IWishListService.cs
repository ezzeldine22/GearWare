using BLL.DTOs.WishListDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.WishListService
{
    public interface IWishListService
    {
        void AddItemToWishList(int productId, string ClientId);
        IEnumerable<GetAllWishListItemDtos> GetAllWishListItems(string clientId);
    }
}

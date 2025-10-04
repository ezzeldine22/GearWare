using BLL.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ReviewService
{
    public interface IReviewService
    {
        Task AddReviewAsync(AddReviewDto addReviewDto, string ClientId, int ProdutId);
        Task<GetAllReviewsDto> GetReviewAsync(int ProductId);
    }
}

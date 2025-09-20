using BLL.DTOs.ReviewDTOs;
using BLL.DTOs.WishListDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepo;

        public ReviewService(IRepository<Review> reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task AddReviewAsync(AddReviewDto addReviewDto, string clientId, int productId)
        {
            var AddedReview = new Review
            {
                UserId = clientId,
                ProductId = productId,
                Rating = addReviewDto.Rating,
                Comment = addReviewDto.Comment,
            };

            await _reviewRepo.AddAsync(AddedReview);
            _reviewRepo.SaveChanges();
        }

        public async Task<GetAllReviewsDto> GetReviewAsync(int ProductId)
        {
            var productReviews = _reviewRepo.ReadAll().Where(pr => pr.ProductId == ProductId);

            if (!await productReviews.AnyAsync())
            {
                throw new CustomException(new List<string> { "there is no reviews her" });
            }

            var rate = (float)await productReviews.AverageAsync(pr => pr.Rating); 
                          
                          

            var AllReviewsDto = await productReviews.Select(o => new GetReviewDto
            {
               comment = o.Comment
            }).ToListAsync();

            return new GetAllReviewsDto
            {
                avrRate = rate,
                comments =  AllReviewsDto
            };
        }
    }
}

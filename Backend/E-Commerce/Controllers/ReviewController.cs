using BLL.DTOs.ProductDtos;
using BLL.DTOs.ReviewDTOs;
using BLL.Services.ProductServices;
using BLL.Services.ReviewService;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<GetAllReviewsDto>> GetAll(int productId)
        {
            var reviews = await _reviewService.GetReviewAsync(productId);
            return Ok(reviews);
        }

        [HttpPost("{clientId}/{productId}")]
        public async Task<ActionResult> AddReview(AddReviewDto addReviewDto, string clientId, int productId)
        {
            await _reviewService.AddReviewAsync(addReviewDto, clientId, productId);
            return Ok();
        }
    }
}

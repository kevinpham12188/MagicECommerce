namespace MagicECommerce_API.DTOS.Response
{
    public class ProductRatingStatsDto
    {
        public Guid ProductId { get; set; }
                public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new()
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 }
        };
    }
}

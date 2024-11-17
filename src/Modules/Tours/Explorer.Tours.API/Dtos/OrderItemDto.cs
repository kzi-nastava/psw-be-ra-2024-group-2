namespace Explorer.Tours.API.Public.Tourist.DTOs
{
    public class OrderItemDto
    {
        public string TourName { get; set; } 
        public double Price { get; set; } 
        public long TourId { get; set; } 
        public long UserId { get; set; } 
    }
}

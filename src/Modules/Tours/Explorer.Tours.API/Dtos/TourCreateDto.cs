namespace Explorer.Tours.API.Dtos
{
    public class TourCreateDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public long guideId { get; set; }
        public string Status { get; set; }
        public List<KeyPointDto>? KeyPoints { get; set; }
        public double? AverageRating { get; set; }
    }
}

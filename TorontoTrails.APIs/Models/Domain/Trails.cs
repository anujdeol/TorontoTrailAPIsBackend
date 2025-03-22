namespace TorontoTrails.APIs.Models.Domain
{
    public class Trails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TrailImageURL { get; set; }
        public double LengthInMiles { get; set; }

        // Foreign keys
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        // Navigation properties
        public Difficulty Difficulty { get; set; }
        public Region Region { get; set; }
    }

}

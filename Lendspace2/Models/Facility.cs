namespace LendSpace.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public decimal HourlyRate { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
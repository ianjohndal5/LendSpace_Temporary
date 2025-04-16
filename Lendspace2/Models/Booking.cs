namespace LendSpace.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AttendeeCount { get; set; }
        public string Purpose { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookingStatus Status { get; set; }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
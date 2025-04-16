using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages.Facilities
{
    public class DetailsModel : PageModel
    {
        private readonly FacilityService _facilityService;
        private readonly BookingService _bookingService;

        public DetailsModel(FacilityService facilityService, BookingService bookingService)
        {
            _facilityService = facilityService;
            _bookingService = bookingService;
        }

        public Facility Facility { get; set; }

        [BindProperty]
        public Booking Booking { get; set; }

        public DateTime SearchDate { get; set; }
        public TimeSpan SearchStartTime { get; set; }
        public TimeSpan SearchEndTime { get; set; }
        public Dictionary<TimeSpan, bool> AvailabilitySlots { get; set; }
        public double Duration => (SearchEndTime - SearchStartTime).TotalHours;
        public decimal TotalCost => Facility?.HourlyRate * (decimal)Duration ?? 0;

        public async Task<IActionResult> OnGetAsync(int id, DateTime? date, TimeSpan? startTime, TimeSpan? endTime)
        {
            Facility = await _facilityService.GetFacilityByIdAsync(id);
            if (Facility == null)
            {
                return NotFound();
            }

            SearchDate = date ?? DateTime.Today;
            SearchStartTime = startTime ?? new TimeSpan(9, 0, 0); // Default 9 AM
            SearchEndTime = endTime ?? new TimeSpan(17, 0, 0);    // Default 5 PM

            // Initialize booking with default values
            Booking = new Booking
            {
                FacilityId = id,
                BookingDate = SearchDate,
                StartTime = SearchStartTime,
                EndTime = SearchEndTime
            };

            // Load availability slots for the day
            AvailabilitySlots = new Dictionary<TimeSpan, bool>();
            for (int hour = 8; hour < 22; hour++)
            {
                var timeSlot = new TimeSpan(hour, 0, 0);
                var slotEndTime = new TimeSpan(hour + 1, 0, 0);
                bool isAvailable = await _facilityService.CheckFacilityAvailabilityAsync(
                    id, SearchDate, timeSlot, slotEndTime);
                AvailabilitySlots[timeSlot] = isAvailable;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Facility = await _facilityService.GetFacilityByIdAsync(Booking.FacilityId);
                return Page();
            }

            // Check availability again to make sure it's still available
            bool isAvailable = await _facilityService.CheckFacilityAvailabilityAsync(
                Booking.FacilityId, Booking.BookingDate, Booking.StartTime, Booking.EndTime);

            if (!isAvailable)
            {
                ModelState.AddModelError(string.Empty, "This facility is no longer available for the selected time.");
                Facility = await _facilityService.GetFacilityByIdAsync(Booking.FacilityId);
                return Page();
            }

            // Calculate total cost
            Facility = await _facilityService.GetFacilityByIdAsync(Booking.FacilityId);
            double duration = (Booking.EndTime - Booking.StartTime).TotalHours;
            Booking.TotalCost = Facility.HourlyRate * (decimal)duration;
            Booking.CreatedAt = DateTime.Now;
            Booking.Status = BookingStatus.Pending;

            // Save the booking
            int bookingId = await _bookingService.CreateBookingAsync(Booking);

            // Redirect to confirmation page
            return RedirectToPage("/Bookings/Confirmation", new { id = bookingId });
        }
    }
}

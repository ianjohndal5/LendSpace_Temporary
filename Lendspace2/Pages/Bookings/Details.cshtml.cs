using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages.Bookings
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

        public Booking Booking { get; set; }
        public Facility Facility { get; set; }
        public double Duration => (Booking.EndTime - Booking.StartTime).TotalHours;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Booking = await _bookingService.GetBookingByIdAsync(id);
            if (Booking == null)
            {
                return NotFound();
            }

            Facility = await _facilityService.GetFacilityByIdAsync(Booking.FacilityId);
            if (Facility == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Only allow cancellation if booking is pending
            if (booking.Status == BookingStatus.Pending)
            {
                await _bookingService.UpdateBookingStatusAsync(id, BookingStatus.Cancelled);
            }

            return RedirectToPage("./Details", new { id });
        }
    }
}

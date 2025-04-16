using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages.Bookings
{
    public class MyBookingsModel : PageModel
    {
        private readonly FacilityService _facilityService;
        private readonly BookingService _bookingService;

        public MyBookingsModel(FacilityService facilityService, BookingService bookingService)
        {
            _facilityService = facilityService;
            _bookingService = bookingService;
        }

        public string SearchEmail { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Facility> Facilities { get; set; }

        public async Task OnGetAsync(string email)
        {
            SearchEmail = email;
            Bookings = new List<Booking>();
            Facilities = new List<Facility>();

            if (!string.IsNullOrEmpty(email))
            {
                Bookings = await _bookingService.GetUserBookingsAsync(email);

                // Get facility details for each booking
                var facilityIds = Bookings.Select(b => b.FacilityId).Distinct().ToList();
                foreach (var id in facilityIds)
                {
                    var facility = await _facilityService.GetFacilityByIdAsync(id);
                    if (facility != null)
                    {
                        Facilities.Add(facility);
                    }
                }
            }
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

            return RedirectToPage("./MyBookings", new { email = booking.UserEmail });
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages.Bookings
{
    public class ConfirmationModel : PageModel
    {
        private readonly FacilityService _facilityService;
        private readonly BookingService _bookingService;

        public ConfirmationModel(FacilityService facilityService, BookingService bookingService)
        {
            _facilityService = facilityService;
            _bookingService = bookingService;
        }

        public Booking Booking { get; set; }
        public Facility Facility { get; set; }

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
    }
}
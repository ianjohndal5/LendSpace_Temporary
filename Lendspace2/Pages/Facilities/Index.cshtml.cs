using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages.Facilities
{
    public class IndexModel : PageModel
    {
        private readonly FacilityService _facilityService;

        public IndexModel(FacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        public List<Facility> Facilities { get; set; }
        public DateTime SearchDate { get; set; }
        public TimeSpan SearchStartTime { get; set; }
        public TimeSpan SearchEndTime { get; set; }

        public async Task OnGetAsync(DateTime? date, TimeSpan? startTime, TimeSpan? endTime)
        {
            SearchDate = date ?? DateTime.Today;
            SearchStartTime = startTime ?? new TimeSpan(9, 0, 0); // Default 9 AM
            SearchEndTime = endTime ?? new TimeSpan(17, 0, 0);    // Default 5 PM

            var allFacilities = await _facilityService.GetAllFacilitiesAsync();
            Facilities = new List<Facility>();

            foreach (var facility in allFacilities)
            {
                if (facility.IsAvailable)
                {
                    bool isAvailable = await _facilityService.CheckFacilityAvailabilityAsync(
                        facility.Id, SearchDate, SearchStartTime, SearchEndTime);

                    if (isAvailable)
                    {
                        Facilities.Add(facility);
                    }
                }
            }
        }
    }
}

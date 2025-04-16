using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LendSpace.Models;
using LendSpace.Services;

namespace LendSpace.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FacilityService _facilityService;

        public IndexModel(FacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        public List<Facility> FeaturedFacilities { get; set; }

        public async Task OnGetAsync()
        {
            var allFacilities = await _facilityService.GetAllFacilitiesAsync();
            // Get first 4 facilities as featured
            FeaturedFacilities = allFacilities.Count > 4
                ? allFacilities.GetRange(0, 4)
                : allFacilities;
        }
    }
}
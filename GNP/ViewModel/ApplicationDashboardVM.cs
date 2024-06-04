using GNP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GNP.ViewModel
{
    public class ApplicationDashboardVM
    {
        public Applicant Applicant { get; set; }

        public List<SelectListItem> Locations { get; set; }


    }
}

using GNP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GNP.ViewModel
{
    public class Dashboard
    {
        public Applicant Applicant { get; set; }

        public List<SelectListItem> Locations { get; set; }

        public DateTime Date { get; set; }

        public ApplicationForm Form { get; set; }
    }
}

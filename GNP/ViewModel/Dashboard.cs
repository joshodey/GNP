using GNP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GNP.ViewModel
{
    public class Dashboard
    {
        public Applicant Applicant { get; set; }

        public List<SelectListItem> Locations { get; set; }

        public List<SelectListItem> Admins { get; set; }
        public List<SelectListItem> AdminEmails { get; set; }

        public string Admin { get; set; }
        public string AdminMail { get; set; }

        public DateTime Date { get; set; }

        public ApplicationForm Form { get; set; }

        public ReviewTools tools {get; set;}

        public string SuccessReport => $"Your message has gone to {Admin} for approval!!!";
    }
    public class ReviewTools
    {
        public bool HasSafetyGlasses { get; set; }
        public bool HasGloves { get; set; }
        public bool HasSafetyFootwares { get; set;}
        public bool HasHardHat { get; set; }
        public bool HasHiViVest { get; set; }
        
    }
}

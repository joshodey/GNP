using GNP.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GNP.Controllers
{
    public class FormController : Controller
    {
        public IActionResult ApplicantForm(ApplicationDashboardVM applicant)
        {
            return View(applicant);
        }
    }
}

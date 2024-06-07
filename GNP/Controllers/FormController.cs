using GNP.IRepository;
using GNP.Models;
using GNP.Service;
using GNP.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GNP.Controllers
{
    public class FormController : Controller
    {
        private readonly IRepository<ApplicationForm, int> _form;
        private readonly IRepository<Applicant, long> _applicant;
        private readonly IEmailService _emailService;
        private readonly IRepository<Admin, long> _admin;

        public FormController(IRepository<ApplicationForm, int> form, IRepository<Applicant, long> applicant, IEmailService emailService, IRepository<Admin, long> admin)
        {
            _form = form;
            _applicant = applicant;
            _emailService = emailService;
            _admin = admin;
        }
        public IActionResult ApplicantForm(Dashboard applicant)
        {
            return View(applicant);
        }


        public async Task<IActionResult> Submit(Dashboard reviewForm)
        {
            if (reviewForm.Form is null)
            {
                return BadRequest();
            }
            string userId = HttpContext.Session.GetString("userId");

            var applicant = _applicant.GetAllAsync().Include(c => c.User).FirstOrDefault(x => x.UserId == long.Parse(userId));

            if (reviewForm.Applicant is null)
            {
                return BadRequest("user did not sign in ");
            }

            reviewForm.Form.ApplicantId = applicant.Id;
            

            var response = await _form.CreateAsync(reviewForm.Form);

            if (response is null)
            {
                return BadRequest();
            }
            //send email to user
            await _emailService.SendMailAsync(applicant.User.Email, "Awaiting Approval", "your GWP awaits approval");


            var admins= await _admin.GetAllAsync().Include(x => x.User).ToListAsync();
            //send mail to admin
            foreach (var admin in admins)
            {
                await _emailService.SendMailAsync(admin.User.Email, "Applican Await Approval", "ToApprove applicant, click here");
            }
            

            return View("./Views/Form/Index.cshtml", reviewForm);

        }
    }
}

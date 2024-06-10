using GeneralWorkPermit.EmailService;
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
            try
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

                reviewForm.Form.ReviewAssessment.ProtectiveEquipments = new List<ProtectiveEquipments>();

                reviewForm.Form.ApplicantId = applicant.Id;
                if (reviewForm.tools.HasSafetyFootwares)
                    reviewForm.Form.ReviewAssessment.ProtectiveEquipments.Add(ProtectiveEquipments.SafetyFootwares);
                if (reviewForm.tools.HasSafetyGlasses)
                    reviewForm.Form.ReviewAssessment.ProtectiveEquipments.Add(ProtectiveEquipments.SafetyGlasses);
                if (reviewForm.tools.HasGloves)
                    reviewForm.Form.ReviewAssessment.ProtectiveEquipments.Add(ProtectiveEquipments.Glooves);
                if (reviewForm.tools.HasHardHat)
                    reviewForm.Form.ReviewAssessment.ProtectiveEquipments.Add(ProtectiveEquipments.HardHat);
                if (reviewForm.tools.HasHiViVest)
                    reviewForm.Form.ReviewAssessment.ProtectiveEquipments.Add(ProtectiveEquipments.HiVisVest);




                var response = await _form.CreateAsync(reviewForm.Form);

                if (response is null)
                {
                    return BadRequest();
                }
                var emailMessage = new EmailMessage(new List<string> {applicant.User.Email}, "New General Work Permit Registration", "Congraturations, your permit will be reviewed");
                //send email to user
                await _emailService.SendEmailAsync(emailMessage);


                var admins = await _admin.GetAllAsync().Include(x => x.User).ToListAsync();
                //send mail to admin
                foreach (var admin in admins)
                {
                    if (admin.User.Email == reviewForm.AdminMail || reviewForm.Admin == admin.User.FirstName)
                        await _emailService.SendEmailAsync(new EmailMessage(new List<string> { admin.User.Email}, "Applicant Await Approval", "ToApprove applicant, click here"));
                }


                return View("./Views/Form/ApplicantForm.cshtml", reviewForm);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }

        }
    }
}

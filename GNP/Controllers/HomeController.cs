using System.Diagnostics;
using System.Net;
using GeneralWorkPermit.EmailService;
using GNP.IRepository;
using GNP.Models;
using GNP.Service;
using GNP.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GNP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _user;
        private readonly IRepository<Applicant, long> _applicant;
        private readonly IRepository<Admin, long> _admin;
        private ISession session;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<User> user, IRepository<Applicant, long> applicant, IEmailService emailService, IRepository<Admin, long> admin)
        {
            _logger = logger;
            _user = user;
            _applicant = applicant;
            _emailService = emailService;
            _admin = admin;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> LoginAsync(LoginSignUpVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return LoginError(errorMessage: ModelState.ToString(), errorCode: (int)HttpStatusCode.BadRequest);
            }


            var user = await _user.FindByEmailAsync(credentials.Email);

            if (user is null)
            {
                return LoginError(errorMessage: "UserName or Password Invalid", errorCode: (int)HttpStatusCode.NotFound);
            }

            if (!(await _user.CheckPasswordAsync(user, credentials.Password)))
            {
                return LoginError(errorMessage: "UserName or Password Invalid", errorCode: (int)HttpStatusCode.NotFound);
            }

            var applicant = await _applicant.GetAsync(user.Id);
            applicant.User = user;



            HttpContext.Session.SetString("userId", user.Id.ToString());

            var locations = new List<SelectListItem>()
                {
                    new SelectListItem("Abuja", "Abuja"),
                    new SelectListItem("Lagos", "Lagos"),
                    new SelectListItem("Calabar", "Calabar")
            };

            var AdminMails = await _admin.GetAllAsync().Include(x => x.User).ToListAsync();
            var names = AdminMails.Select(mail => new SelectListItem(mail.User.FirstName + " " + mail.User.LastName, mail.User.Email
                )).ToList();
            var mails = AdminMails.Select(admin => new SelectListItem(admin.User.Email, admin.User.Email)).ToList();

            return View("./Views/Form/ApplicantForm.cshtml", new Dashboard()
            {
                Applicant = applicant,
                Locations = locations,
                AdminEmails = mails,
                Admins = names,
            });

        }

        public async Task<IActionResult> RegisterAsync(LoginSignUpVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return LoginError(errorMessage: ModelState.ToString(), errorCode: (int)HttpStatusCode.BadRequest);
            }
            var user = new User()
            {
                Email = credentials.Email,
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                UserName = credentials.Email,
                UserType = UserType.Applicant
            };

            var res = await _user.CreateAsync(user, credentials.Password);

            if (!res.Succeeded)
            {
                var dataUser = await _user.FindByEmailAsync(credentials.Email);
                
                if (dataUser is null)
                {
                    return LoginError(errorMessage: "Something Went Wrong, Please try Again", errorCode: (int)HttpStatusCode.InternalServerError);
                }

                var dataApplicant = await _applicant.GetAsync(dataUser.Id);

                if (dataApplicant is null)
                {
                    var applicantToSave = new Applicant()
                    {
                        CreatedAt = DateTime.Now,
                        UserId = dataUser.Id,
                    };

                    var response = await _applicant.CreateAsync(applicantToSave);

                    if (response is null)
                    {
                        return LoginError(errorMessage: "Something Went Wrong, Please try Again", errorCode: (int)HttpStatusCode.InternalServerError);
                    }
                }
                
            }
            
            await _user.AddToRoleAsync(user, "applicant");

            var savedUser = await _user.FindByEmailAsync(credentials.Email);
            if (savedUser is null)
            {
                return LoginError(errorMessage: "Something Went Wrong, Please try Again", errorCode: (int)HttpStatusCode.InternalServerError);
            }

            var applicant = new Applicant()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = savedUser.Id,
            };

            var savedApplicant = await _applicant.CreateAsync(applicant);

            if (savedApplicant is null)
            {
                return LoginError(errorMessage: "Something Went Wrong, Please try Again", errorCode: (int)HttpStatusCode.InternalServerError);
            }

            return View("./Views/Home/Index.cshtml");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult LoginError(string errorMessage, int errorCode)
        {
            var error = new ErrorViewModel()
            {
                ErrorMessage = errorMessage,
                RequestId = errorCode.ToString(),
            };
            return View("./Views/Shared/Error.cshtml", error);
        }
    }
}
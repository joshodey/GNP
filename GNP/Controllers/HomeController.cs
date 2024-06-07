using System.Diagnostics;
using GNP.IRepository;
using GNP.Models;
using GNP.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GNP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _user;
        private readonly IRepository<Applicant, long> _applicant;
        private ISession session;

        public HomeController(ILogger<HomeController> logger, UserManager<User> user, IRepository<Applicant, long> applicant)
        {
            _logger = logger;
            _user = user;
            _applicant = applicant;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> LoginAsync(LoginSignUpVM credentials)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }


            var user = await _user.FindByEmailAsync(credentials.Email);

            if (user is null)
            {
                return Error();
            }

            if (!(await _user.CheckPasswordAsync(user, credentials.Password)))
            {
                return Error();
            }

            var applicant = await _applicant.GetAsync(user.Id);
            applicant.User = user;



            HttpContext.Session.SetString("userId", user.Id.ToString());

            return View("./Views/Form/ApplicantForm.cshtml", new Dashboard()
            {
                Applicant = applicant,
                Locations = new List<SelectListItem>()
                {
                    new SelectListItem("Abuja", "Abuja"),
                    new SelectListItem("Lagos", "Lagos"),
                    new SelectListItem("Calabar", "Calabar")
                }
            });

        }

        public async Task<IActionResult> RegisterAsync(LoginSignUpVM credentials)
        {
            if (!ModelState.IsValid)
            {
                Error();
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
                    return Error();
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
                        return Error();
                    }
                }
                
            }
            
            await _user.AddToRoleAsync(user, "applicant");

            var savedUser = await _user.FindByEmailAsync(credentials.Email);
            if (savedUser is null)
            {
                throw new Exception("user Not Saved");
            }

            var applicant = new Applicant()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = savedUser.Id,
            };

            var savedApplicant = await _applicant.CreateAsync(applicant);

            if (savedApplicant is null)
            {
                return Error();
            }

            return Index();

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

        private IActionResult LoginError(string callbackCode, int errorCode)
        {
            return View();
        }
    }
}
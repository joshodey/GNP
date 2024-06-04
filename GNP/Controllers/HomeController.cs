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
        private UserManager<User> _user;
        private readonly IRepository<Applicant, long> _applicant;

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
                return Error();
            }
            
            await _user.AddToRoleAsync(user, "user");

            var savedUser = await _user.FindByEmailAsync(credentials.Email);
            if (savedUser is null)
            {
                throw new Exception("user Not Saved");
            }

            var applicant = new Applicant()
            {
                Id = savedUser.Id,
                CreatedAt = DateTime.UtcNow,
                UserId = savedUser.Id,
            };

            var savedApplicant = await _applicant.CreateAsync(applicant);

            if (savedApplicant is null)
            {
                return Error();
            }

            savedApplicant.User = savedUser;



            ViewData["user"] = user;

            return View("", new ApplicationDashboardVM()
            {
                Applicant = savedApplicant,
                Locations = new List<SelectListItem>()
                {
                    new SelectListItem("Abuja", "Abuja"),
                    new SelectListItem("Lagos", "Lagos"),
                    new SelectListItem("Calabar", "Calabar")
                }
            });

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
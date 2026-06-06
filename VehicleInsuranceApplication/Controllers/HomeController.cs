using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VehicleInsuranceApplication.Data;
using VehicleInsuranceApplication.Models;
using VehicleInsuranceApplication.Models.ViewModels;
using ClaimModel = VehicleInsuranceApplication.Models.Claim;

namespace VehicleInsuranceApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Contact abc)
        {
            _context.Contacts.Add(abc);
            _context.SaveChanges();
            return RedirectToAction("Contact");
        }

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Car()
        {
            return View();
        }

        public IActionResult Cardetails()
        {
            return View();
        }

        public IActionResult Carbooking()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult Testimonial()
        {
            return View();
        }
        // ==================== CLAIMS ====================

        public IActionResult CreateClaim()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");  // Fetch user ID from session
            if (userId == null)
            {
                return RedirectToAction("Login"); // Redirect if session expired
            }

            var model = new CreateClaimViewModel
            {
                Uid = userId.Value,  // Auto-set Uid in the form
                Policies = _context.Policies.ToList()
                //ViewBag.Policies = new SelectList(_context.Policies.ToList(), "Id", "PolicyNumber");
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateClaim(CreateClaimViewModel model)
        {


            int? userId = HttpContext.Session.GetInt32("UserId"); // Fetch user ID from session
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                model.Policies = _context.Policies.ToList();
                return View(model);
            }


            var claim = new Claim
            {
                Uid = userId.Value, // Assign the logged-in user's Uid
                PolicyId = model.PolicyId,
                AmountPaid = model.Amount,
                ClaimableAmount = model.ClaimableAmount,
                Reason = model.Reason,
                CreatedAt = model.CreatedAt
            };

            _context.Claims.Add(claim);
            _context.SaveChanges();
            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction("CreateClaim");
        }


        [HttpGet]
        public IActionResult GetPolicyAmount(int policyId)
        {
            var policy = _context.Policies.FirstOrDefault(p => p.Id == policyId);
            if (policy == null)
            {
                return Json(new { amount = 0 });
            }

            return Json(new { amount = policy.Amount });
        }




        // ==================== PAYMENTS ====================

        // Payment Create Page (GET)
        public IActionResult CreatePayment()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");  // Fetch user ID from session
            if (userId == null)
            {
                return RedirectToAction("Login"); // Redirect if session expired
            }

            var model = new CreatePaymentViewModel
            {
                Uid = userId.Value,  // Auto-set Uid in the form
                Policies = _context.Policies.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePayment(CreatePaymentViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("UserId"); // Fetch user ID from session
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                model.Policies = _context.Policies.ToList();
                ViewBag.Message = "Payment Added successfully!";
                return View(model);
            }

            var payment = new Payment
            {
                Uid = userId.Value, // Assign the logged-in user's Uid
                PolicyId = model.PolicyId,
                AmountPaid = model.Amount,
                PaymentMethod = model.PaymentMethod,
                PaymentDate = model.PaymentDate
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ==================== POLICIES ====================
        public async Task<IActionResult> Policies()
        {
            var policies = await _context.Policies.ToListAsync();
            return View(policies);
        }

        // ==================== VEHICLES ====================
        public async Task<IActionResult> Vehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.WebUser)  // Ensure WebUser is included
                .Include(v => v.Category)  // Include Category if needed
                .ToListAsync();

            return View(vehicles);
        }

        public IActionResult CreateVehicle()
        {
            ViewBag.Categories = _context.VehicleCategories.Select(c => new { c.Id, c.Name }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value; // Fetch Uid from session
            vehicle.Uid = userId;

            if (ModelState.IsValid)
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Policies));
            }

            ViewBag.Categories = _context.VehicleCategories.Select(c => new { c.Id, c.Name }).ToList();
            return View(vehicle);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all session data
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSessions") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] WebUser abcd)
        {
            if (abcd == null || string.IsNullOrEmpty(abcd.UserName) || string.IsNullOrEmpty(abcd.Password))
            {
                ViewBag.Message = "Please enter both username and password.";
                return View();
            }

            var mywebuser = _context.WebUsers.FirstOrDefault(y => y.UserName == abcd.UserName);

            if (mywebuser != null && BCrypt.Net.BCrypt.Verify(abcd.Password, mywebuser.Password))
            {
                // Set session variables
                HttpContext.Session.SetString("UserSessions", mywebuser.UserName);
                HttpContext.Session.SetInt32("UserId", mywebuser.Uid); // Ensure this is correctly set

                return RedirectToAction("Index");
            }

            ViewBag.Message = "Invalid username or password. Please try again.";
            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserSessions") != null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(WebUser newWebUser)
        {
            if (string.IsNullOrWhiteSpace(newWebUser.UserName) || string.IsNullOrWhiteSpace(newWebUser.Password))
            {
                ViewBag.Message = "Please fill in all fields!";
                return View();
            }

            var existingWebUser = _context.WebUsers.FirstOrDefault(f => f.UserName == newWebUser.UserName);
            if (existingWebUser != null)
            {
                ViewBag.Message = "Username already exists!";
                return View(); // Stay on the register page
            }

            // Hash the password before saving (Replace this with actual hashing logic)
            newWebUser.Password = BCrypt.Net.BCrypt.HashPassword(newWebUser.Password);

            _context.WebUsers.Add(newWebUser);
            _context.SaveChanges();

            ViewBag.Message = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }

        public IActionResult EditRegister()
        {
            if (HttpContext.Session.GetString("UserSessions") == null)
            {
                return RedirectToAction("login");
            }

            string username = HttpContext.Session.GetString("UserSessions");
            var webuser = _context.WebUsers.FirstOrDefault(f => f.UserName == username);

            if (webuser == null)
            {
                return RedirectToAction("Login");
            }

            return View(webuser);
        }

        [HttpPost]
        public IActionResult EditRegister(WebUser updatedWebUser)
        {
            if (HttpContext.Session.GetString("UserSessions") == null)
            {
                return RedirectToAction("Login");
            }

            var webuser = _context.WebUsers.FirstOrDefault(f => f.Uid == updatedWebUser.Uid);
            if (webuser != null)
            {
                webuser.UserName = updatedWebUser.UserName;
                webuser.Password = updatedWebUser.Password;
                _context.SaveChanges();

                // Update session name if the username is changed
                HttpContext.Session.SetString("UserSessions", webuser.UserName);

                ViewBag.Message = "Profile updated successfully!";
            }
            else
            {
                ViewBag.Message = "User not found!";
            }

            return View(webuser);
        }
    }
}

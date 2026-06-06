using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleInsuranceApplication.Models;
using VehicleInsuranceApplication.Models.ViewModels;
using AppClaim = VehicleInsuranceApplication.Models.Claim;

//using ClaimModel = VehicleInsuranceApplication.Models.Claim;

namespace VehicleInsuranceApplication.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.mysession = HttpContext.Session.GetString("UserSession");
            }
            else
            {
                return RedirectToAction("login");
            }
            return View();
        }

        // ==================== VEHICLE CATEGORIES ====================
        public IActionResult VehicleCategory()
        {
            var categories = _db.VehicleCategories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult CreateVehicleCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicleCategory(VehicleCategory category)
        {

            if (ModelState.IsValid)
            {
                _db.VehicleCategories.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(VehicleCategory));
            }
            return View(category);
        }

        public async Task<IActionResult> EditVehicleCategory(int id)
        {
            var category = await _db.VehicleCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditVehicleCategory(int id, VehicleCategory category)
        {
            if (id != category.Id) return BadRequest();

            if (ModelState.IsValid)
            {
               _db.VehicleCategories.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(VehicleCategory));
            }
            return View(category);
        }

        public async Task<IActionResult> DeleteVehicleCategory(int id)
        {
            var category = await _db.VehicleCategories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("DeleteVehicleCategory")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.VehicleCategories.FindAsync(id);
            if (category == null) return NotFound();

            _db.VehicleCategories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(VehicleCategory));
        }

        // ==================== POLICIES ====================
        public async Task<IActionResult> Policies()
        {
            var policies = await _db.Policies.ToListAsync();
            return View(policies);
        }

        public IActionResult CreatePolicy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePolicy(Policy policy, IFormFile ImageUpload)
        {
            if (!ModelState.IsValid)
            {
                return View(policy);
            }

            if (ImageUpload != null && ImageUpload.Length > 0)
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");


                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                var uniqueFileName = $"{Guid.NewGuid()}_{ImageUpload.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }

                policy.ImagePath = $"/images/{uniqueFileName}";
            }
            else
            {
                policy.ImagePath = null;
            }

            _db.Policies.Add(policy);
            await _db.SaveChangesAsync();

            return RedirectToAction("Policies", "Admin");
        }


        public async Task<IActionResult> EditPolicy(int id)
        {
            var policy = await _db.Policies.FindAsync(id);
            if (policy == null) return NotFound();
            return View(policy);
        }

        [HttpPost]
        public async Task<IActionResult> EditPolicy(int id, Policy policy, IFormFile? ImageUpload)
        {
            if (id != policy.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var existingPolicy = await _db.Policies.FindAsync(id);
                if (existingPolicy == null) return NotFound();


                existingPolicy.VehicleName = policy.VehicleName;
                existingPolicy.PolicyNumber = policy.PolicyNumber;
                existingPolicy.StartDate = policy.StartDate;
                existingPolicy.EndDate = policy.EndDate;
                existingPolicy.Amount = policy.Amount;


                if (ImageUpload != null && ImageUpload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{ImageUpload.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUpload.CopyToAsync(fileStream);
                    }


                    if (!string.IsNullOrEmpty(existingPolicy.ImagePath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPolicy.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    existingPolicy.ImagePath = $"/images/{uniqueFileName}";
                }

                _db.Policies.Update(existingPolicy);
                await _db.SaveChangesAsync();
                return RedirectToAction("Policies", "Admin");
            }
            return View(policy);
        }


        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _db.Policies.FindAsync(id);
            if (policy == null) return NotFound();
            return View(policy);
        }

        [HttpPost, ActionName("DeletePolicyConfirmed")]
        public async Task<IActionResult> DeletePolicyConfirmed(int id)
        {
            var policy = await _db.Policies.FindAsync(id);
            if (policy == null) return NotFound();


            var relatedPayments = _db.Payments.Where(p => p.PolicyId == id);
            _db.Payments.RemoveRange(relatedPayments);


            if (!string.IsNullOrEmpty(policy.ImagePath))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", policy.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _db.Policies.Remove(policy);
            await _db.SaveChangesAsync();

            return RedirectToAction("Policies" , "Admin");
        }

        // ==================== VEHICLES ====================
        public async Task<IActionResult> Vehicles()
        {
            var vehicles = await _db.Vehicles
                .Include(v => v.WebUser)  
                .Include(v => v.Category)
                .ToListAsync();

            return View(vehicles);
        }

        public async Task<IActionResult> EditVehicle(int id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _db.VehicleCategories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> EditVehicle(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                // Check if the Uid exists in the WebUsers table
                bool userExists = await _db.WebUsers.AnyAsync(u => u.Uid == vehicle.Uid);
                if (!userExists)
                {
                    ModelState.AddModelError("Uid", "The selected user does not exist.");
                    ViewBag.Categories = _db.VehicleCategories
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                        .ToList();
                    return View(vehicle);
                }

                try
                {
                    _db.Vehicles.Update(vehicle);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Vehicles", "Admin");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Error updating vehicle. Please check the user ID.");
                    return View(vehicle);
                }
            }

            ViewBag.Categories = _db.VehicleCategories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            return View(vehicle);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();
            return RedirectToAction("Vehicles", "Admin");
        }

        // ==================== PAYMENTS ====================

        // View all Payments
        public async Task<IActionResult> Payments()
        {
            var payments = await _db.Payments
                .Include(p => p.WebUser)
                .Include(p => p.Policy)
                .ToListAsync();
            return View(payments);
        }

        public async Task<IActionResult> EditPayment(int? id)
        {
            if (id == null)
                return NotFound();

            var payment = await _db.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            ViewBag.Users = _db.WebUsers.ToList();
            ViewBag.Policies = _db.Policies.ToList();
            return View(payment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Attach payment but do not track navigation properties
                    _db.Attach(payment);
                    _db.Entry(payment).State = EntityState.Modified;

                    // Explicitly set foreign key references
                    _db.Entry(payment).Property(x => x.Uid).IsModified = true;
                    _db.Entry(payment).Property(x => x.PolicyId).IsModified = true;

                    await _db.SaveChangesAsync();
                    return RedirectToAction("Payments", "Admin");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Payments.Any(e => e.Id == payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Users = _db.WebUsers.ToList();
            ViewBag.Policies = _db.Policies.ToList();
            return View(payment);
        }

        // Payment Delete Page (GET)
        public async Task<IActionResult> DeletePayment(int? id)
        {
            if (id == null)
                return NotFound();

            var payment = await _db.Payments
                .Include(p => p.WebUser)
                .Include(p => p.Policy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        // Payment Delete (POST)
        [HttpPost, ActionName("DeletePayment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePaymentConfirmed(int id)
        {
            var payment = await _db.Payments.FindAsync(id);
            if (payment != null)
            {
                _db.Payments.Remove(payment);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Payments", "Admin");
        }

        // ==================== CLAIMS ====================
        public async Task<IActionResult> Claims()
        {
            var claims = await _db.Claims
                .Include(c => c.Policy)
                .Include(c => c.WebUser)
                .ToListAsync();


            if (claims == null || !claims.Any())
            {
                ViewBag.Message = "No claims available.";
            }

            return View(claims);
        }

        // GET: Admin/EditClaim/5
        public async Task<IActionResult> EditClaim(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            var policies = await _db.Policies.ToListAsync();
            ViewBag.Policies = new SelectList(policies, "Id", "PolicyNumber", claim.PolicyId);
            return View(claim);
        }

        // POST: Admin/EditClaim/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClaim(AppClaim model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Debug.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
                ViewBag.Policies = new SelectList(_db.Policies, "Id", "PolicyNumber", model.PolicyId);
                return View(model);
            }


            _db.Claims.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Claim updated successfully.";
            return RedirectToAction("Claims");
        }

        // GET: Admin/DeleteClaim/5
        public async Task<IActionResult> DeleteClaim(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            _db.Claims.Remove(claim);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Claim deleted successfully.";
            return RedirectToAction("Claims");
        }

        public IActionResult Errorpage()
        {
            return View();
        }
        public IActionResult Blank()
        {
            return View();
        }

        public IActionResult logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("login");
            }
            return View();
        }
        public IActionResult login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult login(User abc)
        {
            var myuser = _db.Users.Where(x => x.Name == abc.Name && x.Password == abc.Password).FirstOrDefault();
            if (myuser != null)
            {
                HttpContext.Session.SetString("UserSession", myuser.Name);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Please Register Yourself!";
                return RedirectToAction("register");
            }
            return View();
        }
        public IActionResult register()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult register(User newUser)
        {
            if (!string.IsNullOrEmpty(newUser.Name) && !string.IsNullOrEmpty(newUser.Password))
            {
                var existingUser = _db.Users.FirstOrDefault(u => u.Name == newUser.Name);
                if (existingUser == null)
                {
                    _db.Users.Add(newUser);
                    _db.SaveChanges();
                    ViewBag.Message = "Registration successful! You can now log in.";
                    return RedirectToAction("login");
                }
                else
                {
                    ViewBag.Message = "Username already exists!";
                }
            }
            else
            {
                ViewBag.Message = "Please fill in all fields!";
            }
            return View();
        }
        public IActionResult editprofile()
        {
            if (HttpContext.Session.GetString("UserSession") == null)
            {
                return RedirectToAction("login");
            }

            string username = HttpContext.Session.GetString("UserSession");
            var user = _db.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                return RedirectToAction("login");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult editprofile(User updatedUser)
        {
            if (HttpContext.Session.GetString("UserSession") == null)
            {
                return RedirectToAction("login");
            }

            var user = _db.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.Name = updatedUser.Name;
                user.Password = updatedUser.Password;
                _db.SaveChanges();

                // Update session name if the username is changed
                HttpContext.Session.SetString("UserSession", user.Name);

                ViewBag.Message = "Profile updated successfully!";
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message = "User not found!";
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult EditContact(int? id)
        {
            return View(_db.Contacts.Find(id));
        }
        [HttpPost]
        public IActionResult EditContact(Contact abc)
        {
            _db.Contacts.Update(abc);
            _db.SaveChanges();
            return RedirectToAction("ContactTable");
        }
        public IActionResult DeleteContact(int? id)
        {
            var a = _db.Contacts.Find(id);
            _db.Contacts.Remove(a);
            _db.SaveChanges();
            return RedirectToAction("ContactTable");
        }

        [HttpGet]
        public IActionResult ContactTable()
        {
            return View(_db.Contacts.ToList());
        }

    }
}

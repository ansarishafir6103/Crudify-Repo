using System.Diagnostics;
using Crudify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace Crudify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SecureSignContext _context;

        public HomeController(ILogger<HomeController> logger, SecureSignContext context)
        {
            _logger = logger;
            _context = context;
        }
        public JsonResult LoadHobbies(int id)
        {
            var hobbies = (from fh in _context.TblFavouriteHobbies
                           join h in _context.TblHobbies on fh.HobbyId equals h.HobbyId
                           where fh.EmployeeId == id
                           select h.HobbyText).ToList();

            return Json(hobbies);
        }

        public JsonResult GetCountries()
        {
            var countries = _context.TblCountries.OrderBy(x => x.CountryName).ToList();
            return new JsonResult(countries);
        }
        public JsonResult GetStates(int id)
        {
            var states = _context.TblStates.Where(x => x.CountryId == id).OrderBy(x => x.StateName).ToList();
            return new JsonResult(states);
        }
        public JsonResult GetCities(int id)
        {
            var cities = _context.TblCities.Where(x => x.StateId == id).OrderBy(x => x.CityName).ToList();
            return new JsonResult(cities);
        }
        public IActionResult Index(int? page, string? search)
        {
            try
            {
                int pageNumber = page ?? 1;
                int pageSize = 3;

                ViewBag.Search = search;

                var rawData = (from e in _context.TblEmployees
                               join fh in _context.TblFavouriteHobbies on e.EmployeeId equals fh.EmployeeId into favHobbiesGroup
                               from fh in favHobbiesGroup.DefaultIfEmpty()
                               join h in _context.TblHobbies on fh.HobbyId equals h.HobbyId into hobbiesGroup
                               from h in hobbiesGroup.DefaultIfEmpty()
                               select new { e, h }).ToList();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    rawData = rawData
                        .Where(x => string.Equals(x.e.EmployeeFirstName, search, StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(x.e.EmployeeLastName, search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }


                // Group and shape data
                var groupedData = rawData
                    .GroupBy(x => x.e.EmployeeId)
                    .Select(grouped => new TblEmployeeViewModel
                    {
                        Employee = grouped.FirstOrDefault(x => x.e != null)?.e,
                        Hobbies = grouped.Where(x => x.h != null).Select(x => x.h).Distinct().ToList()
                    })
                    .ToList();

                var pagedList = groupedData.ToPagedList(pageNumber, pageSize);
                return View(pagedList);
            }
            catch (Exception)
            {
                var emptyList = new List<TblEmployeeViewModel>().ToPagedList(1, 10);
                return View(emptyList);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var vm = new TblEmployeeViewModel
                {
                    Hobbies = _context.TblHobbies.ToList()
                };

                vm.DepartmentsList = await _context.TblDepartments.ToListAsync();

                return View(vm);
            }
            catch (Exception)
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(TblEmployeeViewModel vm, IFormFile Photo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid)
                {
                    // Handle photo upload
                    if (Photo != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Photo.CopyToAsync(stream);
                        }
                        vm.Employee!.EmployeePhoto = fileName;
                    }

                    // Insert employee and hobbies
                    _context.TblEmployees.Add(vm.Employee!);
                    await _context.SaveChangesAsync();

                    foreach (var hobbyId in vm.SelectedHobbyIds)
                    {
                        _context.TblFavouriteHobbies.Add(new TblFavouriteHobby
                        {
                            EmployeeId = vm.Employee!.EmployeeId,
                            HobbyId = hobbyId
                        });
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Employee created successfully!";
                    return RedirectToAction("Index");
                }

                vm.Hobbies = _context.TblHobbies.ToList();
                TempData["ErrorMessage"] = "Please correct the form errors.";
                return View(vm);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while saving employee. Please try again.";
                return View(vm);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                TblEmployeeViewModel vm = new TblEmployeeViewModel();

                vm.CountriesList = await _context.TblCountries.ToListAsync();
                vm.StatesList = await _context.TblStates.ToListAsync();
                vm.CitiesList = await _context.TblCities.ToListAsync();

                vm.DepartmentsList = await _context.TblDepartments.ToListAsync();

                vm.Employee = await _context.TblEmployees.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

                vm.Hobbies = await _context.TblHobbies.ToListAsync();

                vm.SelectedHobbyIds = await _context.TblFavouriteHobbies
                    .Where(x => x.EmployeeId == id)
                    .Select(x => x.HobbyId!.Value)
                    .ToListAsync();

                return View(vm);
            }
            catch (Exception)
            {
                // Return an error view or redirect to an error page
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TblEmployeeViewModel vm, IFormFile Photo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingEmployee = await _context.TblEmployees
                    .FirstOrDefaultAsync(x => x.EmployeeId == id);

                if (existingEmployee == null)
                {
                    TempData["ErrorMessage"] = "Employee not found.";
                    return RedirectToAction("Index");
                }

                // === 1. Handle photo upload ===
                if (Photo != null && Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Photo.CopyToAsync(fileStream);
                    }

                    // Delete old photo
                    if (!string.IsNullOrEmpty(existingEmployee.EmployeePhoto))
                    {
                        var oldPath = Path.Combine(uploadsFolder, existingEmployee.EmployeePhoto);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    existingEmployee.EmployeePhoto = uniqueFileName;
                }

                // === 2. Update employee details ===
                existingEmployee.EmployeeFirstName = vm.Employee!.EmployeeFirstName;
                existingEmployee.EmployeeLastName = vm.Employee.EmployeeLastName;
                existingEmployee.EmployeeEmail = vm.Employee.EmployeeEmail;
                existingEmployee.EmployeePhone = vm.Employee.EmployeePhone;
                existingEmployee.CountryId = vm.Employee.CountryId;
                existingEmployee.StateId = vm.Employee.StateId;
                existingEmployee.CityId = vm.Employee.CityId;
                existingEmployee.DepartmentId = vm.Employee.DepartmentId;

                // === 3. Update hobbies ===
                var oldHobbies = _context.TblFavouriteHobbies.Where(h => h.EmployeeId == id);
                _context.TblFavouriteHobbies.RemoveRange(oldHobbies);

                if (vm.SelectedHobbyIds != null && vm.SelectedHobbyIds.Any())
                {
                    foreach (var hobbyId in vm.SelectedHobbyIds)
                    {
                        _context.TblFavouriteHobbies.Add(new TblFavouriteHobby
                        {
                            EmployeeId = id,
                            HobbyId = hobbyId
                        });
                    }
                }

                // === 4. Save changes ===
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Employee updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Rollback transaction
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while updating the employee.";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var employee = await _context.TblEmployees
                    .Include(e => e.Department)
                    .Include(e => e.Country)
                    .Include(e => e.State)
                    .Include(e => e.City)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                var hobbies = await _context.TblFavouriteHobbies
                .Where(fh => fh.EmployeeId == id)
                .Include(fh => fh.Hobby)
                .Select(fh => fh.Hobby)
                .Where(h => h != null) // Filter out null hobbies
                .ToListAsync();

                var viewModel = new TblEmployeeViewModel
                {
                    Employee = employee,
                    Hobbies = hobbies! // The '!' operator asserts that hobbies is non-null
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.TblEmployees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var employee = await _context.TblEmployees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                // Remove associated hobbies
                var favouriteHobbies = _context.TblFavouriteHobbies.Where(fh => fh.EmployeeId == id);
                _context.TblFavouriteHobbies.RemoveRange(favouriteHobbies);

                // Remove employee
                _context.TblEmployees.Remove(employee);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Employee deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the employee.";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}

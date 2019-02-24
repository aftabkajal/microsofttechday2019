using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.DbContexts;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public EmployeeController(EmployeeDbContext context, IHostingEnvironment appEnvironment)
        {
            this._context = context;
            this._appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["ShortCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var employees = from emp in _context.Employees.Include(e => e.Department) select emp;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.ShortCode);
                    break;
                case "Date":
                    employees = employees.OrderBy(e => e.Department);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(e => e.Designation);
                    break;
                default:
                    employees = employees.OrderBy(s => s.FirstName);
                    break;
            }
            
            return View(await employees.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Designation,DepartmentId,Salary,DateOfBirth,Email,PhoneNumber,ShortCode,Id,ImageUrl, Gender")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _appEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                employee.ImageUrl = ImageUpload(webRootPath, files);

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Designation,DepartmentId,Salary,DateOfBirth,Email,PhoneNumber,ShortCode,Id, Gender")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    //var webRootPath = _appEnvironment.WebRootPath;
                    //var files = HttpContext.Request.Form.Files;
                    //employee.ImageUrl = ImageUpload(webRootPath, files);

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        private string ImageUpload(string webRootPath, IFormFileCollection files)
        {
            string imageId = Guid.NewGuid().ToString();
            if (files.Count != 0)
            {
                var uploads = Path.Combine(webRootPath, @"uploads\\img");
                var extention = Path.GetExtension(files[0].FileName);
                using (var fileStram = new FileStream(Path.Combine(uploads, imageId + extention), FileMode.Create))
                {
                    files[0].CopyTo(fileStram);
                }
                string imageUrl = @"~/" + @"uploads/img" + @"/" + imageId + extention;
                return imageUrl;
            }
            return null;
        }
    }
}

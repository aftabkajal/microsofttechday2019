using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Core.Entities;
using ToDo.Core.Interfaces;

namespace ToDo.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;
       public ToDoController(IRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var todos = _repository.GetItems<ToDoItem>();
            return View(todos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repository.Add<ToDoItem>(item);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError($"{ex.Message}", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
            }

            return View();
        }
    }
}
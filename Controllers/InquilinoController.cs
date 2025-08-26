using System.Security.Cryptography.X509Certificates;
using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repo;

        public InquilinoController(IConfiguration config)
        {
            repo = new RepositorioInquilino(config.GetConnectionString("DefaultConnection"));

        }
        public IActionResult Index() => View(repo.ObtenerTodos());

        public IActionResult Details(int id)
        {
            var x = repo.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino x)
        {
            if (!ModelState.IsValid) return View(x);
            repo.Alta(x);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int id)
        {
            var x = repo.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inquilino x)
        {
            if (id != x.Id) return BadRequest();
            if (!ModelState.IsValid) return View(x);
            repo.Modificar(x);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var x = repo.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
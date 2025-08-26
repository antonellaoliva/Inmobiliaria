using System.Security.Cryptography.X509Certificates;
using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repo;

        public PropietarioController(IConfiguration config)
        {
            repo = new RepositorioPropietario(config.GetConnectionString("DefaultConnection"));

        }
        public IActionResult Index() => View(repo.ObtenerTodos());

        public IActionResult Details(int id)
        {
            var p = repo.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Propietario p)
        {
            if (!ModelState.IsValid) return View(p);
            repo.Alta(p);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int id)
        {
            var p = repo.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Propietario p)
        {
            if (id != p.Id) return BadRequest();
            if (!ModelState.IsValid) return View(p);
            repo.Modificar(p);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var p = repo.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
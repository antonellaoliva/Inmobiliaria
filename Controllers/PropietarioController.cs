using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Mvc;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repoPropietario;

        public PropietarioController(IConfiguration configuration)
        {
            repoPropietario = new RepositorioPropietario(configuration);

        }
        public IActionResult Index() => View(repoPropietario.ObtenerTodos());

        public IActionResult Details(int id)
        {
            var p = repoPropietario.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Propietario p)
        {
            if (!ModelState.IsValid) return View(p);
            repoPropietario.Alta(p);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int id)
        {
            var p = repoPropietario.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Propietario p)
        {
            if (id != p.Id) return BadRequest();
            if (!ModelState.IsValid) return View(p);
            repoPropietario.Modificar(p);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var p = repoPropietario.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoPropietario.Baja(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
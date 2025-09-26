using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino repoInquilino;

        public InquilinoController(IConfiguration configuration)
        {
            repoInquilino = new RepositorioInquilino(configuration);

        }
        public IActionResult Index() => View(repoInquilino.ObtenerTodos());

        public IActionResult Details(int id)
        {
            var x = repoInquilino.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino x)
        {
            if (repoInquilino.ExisteDNI(x.DNI))
                ModelState.AddModelError("Dni", "Este DNI ya está registrado.");

            if (!ModelState.IsValid) return View(x);
            repoInquilino.Alta(x);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int id)
        {
            var x = repoInquilino.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inquilino x)
        {
            if (repoInquilino.ExisteDNI(x.DNI, x.Id))
                ModelState.AddModelError("Dni", "Este DNI ya está registrado.");

            if (id != x.Id) return BadRequest();
            if (!ModelState.IsValid) return View(x);

            repoInquilino.Modificar(x);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var x = repoInquilino.ObtenerPorId(id);
            if (x == null) return NotFound();
            return View(x);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoInquilino.Baja(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
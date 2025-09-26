using Microsoft.AspNetCore.Mvc;
using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class TipoInmuebleController : Controller
    {
        private readonly RepositorioTipoInmueble repo;

        public TipoInmuebleController(IConfiguration configuration)
        {
            repo = new RepositorioTipoInmueble(configuration);
        }

        public IActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            return View(lista);
        }

        public IActionResult Details(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TipoInmueble tipo)
        {
            if (ModelState.IsValid)
            {
                repo.Alta(tipo);
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        public IActionResult Edit(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TipoInmueble tipo)
        {
            if (id != tipo.Id) return NotFound();
            if (ModelState.IsValid)
            {
                repo.Modificacion(tipo);
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var tipo = repo.ObtenerPorId(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repo.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

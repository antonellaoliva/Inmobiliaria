using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario repoPropietario;

        public PropietarioController(IConfiguration configuration)
        {
            repoPropietario = new RepositorioPropietario(configuration);

        }
        public IActionResult Index(int pagina = 1)
        {
            int tamPagina = 10;

            var lista = repoPropietario.ObtenerPaginado(pagina, tamPagina);
            int totalPropietarios = repoPropietario.ContarPropietarios();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalPropietarios / tamPagina);

            return View(lista);
        }


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


            if (repoPropietario.ExisteDNI(p.DNI))
                ModelState.AddModelError("Dni", "Este DNI ya está registrado.");

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
            if (repoPropietario.ExisteDNI(p.DNI, p.Id))
                ModelState.AddModelError("Dni", "Este DNI ya está registrado.");

            if (id != p.Id) return BadRequest();

            if (!ModelState.IsValid) return View(p);
            repoPropietario.Modificar(p);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var p = repoPropietario.ObtenerPorId(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoPropietario.Baja(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
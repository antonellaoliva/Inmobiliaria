using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioPropietario repoPropietario;
        private readonly RepositorioTipoInmueble repoTipoInmueble;

        public InmuebleController(IConfiguration configuration)
        {
            repoInmueble = new RepositorioInmueble(configuration);
            repoPropietario = new RepositorioPropietario(configuration);
            repoTipoInmueble = new RepositorioTipoInmueble(configuration);
        }


        public IActionResult Disponibles(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inmuebles = repoInmueble.ObtenerDisponibles(fechaInicio, fechaFin);
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            return View(inmuebles);
        }



        public IActionResult Index(int pagina = 1)
        {
            int tamPagina = 10;
            var totalRegistros = repoInmueble.Contar();
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamPagina);

            var lista = repoInmueble.ObtenerPaginado(pagina, tamPagina);

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(lista);
        }


        public IActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }


        public IActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre");
            return View(new Inmueble());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join("<br>", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                ViewBag.Errores = errores;

                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre", inmueble.TipoInmuebleId);
                return View(inmueble);
            }

            try
            {
                int id = repoInmueble.Alta(inmueble);
                if (id > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se pudo guardar el inmueble.");
                    ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                    ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre", inmueble.TipoInmuebleId);
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre", inmueble.TipoInmuebleId);
                return View(inmueble);
            }
        }



        public IActionResult Edit(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre", inmueble.TipoInmuebleId);
            return View(inmueble);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                ViewBag.Tipos = new SelectList(repoTipoInmueble.ObtenerTodos(), "Id", "Nombre", inmueble.TipoInmuebleId);

                return View(inmueble);
            }
            repoInmueble.Modificacion(inmueble);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoInmueble.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
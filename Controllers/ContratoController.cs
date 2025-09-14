using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace INMOBILIARIA__Oliva_Perez.Models
{
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repoContrato;
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioInquilino repoInquilino;

        public ContratoController(IConfiguration configuration)
        {
            // Inicializo los repositorios
            repoContrato = new RepositorioContrato(configuration);
            repoInmueble = new RepositorioInmueble(configuration);
            repoInquilino = new RepositorioInquilino(configuration);
        }

        public IActionResult Index()
        {
            var lista = repoContrato.ObtenerTodos();

            var listaParaVista = lista.Select(c => new
            {
                c.Id,
                InmuebleDireccion = repoInmueble.ObtenerPorId(c.InmuebleId)?.Direccion ?? "",
                InquilinoNombre = repoInquilino.ObtenerPorId(c.InquilinoId)?.Nombre ?? "",
                c.FechaInicio,
                c.FechaFin,
                c.Monto
            }).ToList();

            return View(listaParaVista);
        }
        

        public IActionResult Details(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null) return NotFound();

                var contratoParaVista = new
                {
                    contrato.Id,
                    InmuebleDireccion = repoInmueble.ObtenerPorId(contrato.InmuebleId)?.Direccion ?? "",
                    InquilinoNombre = repoInquilino.ObtenerPorId(contrato.InquilinoId)?.Nombre ?? "",
                    contrato.FechaInicio,
                    contrato.FechaFin,
                    contrato.Monto
                };

            return View(contratoParaVista);
        }

        public IActionResult Create()
        {
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos() ?? new List<Inmueble>();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos() ?? new List<Inquilino>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repoContrato.Alta(contrato);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion", contrato.InmuebleId);
            ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre", contrato.InquilinoId);
            return View(contrato);
        }

        public IActionResult Edit(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null) return NotFound();
            ViewBag.Inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion", contrato.InmuebleId);
            ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre", contrato.InquilinoId);
            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                repoContrato.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = new SelectList(repoInmueble.ObtenerTodos(), "Id", "Direccion", contrato.InmuebleId);
            ViewBag.Inquilinos = new SelectList(repoInquilino.ObtenerTodos(), "Id", "Nombre", contrato.InquilinoId);
            return View(contrato);
        }

        public IActionResult Delete(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null) return NotFound();

            var inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            var inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);

            ViewBag.InmuebleDireccion = inmueble?.Direccion;
            ViewBag.InquilinoNombre = inquilino?.Nombre;

            return View(contrato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoContrato.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

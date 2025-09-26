using Microsoft.AspNetCore.Mvc;
using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;


namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private readonly RepositorioContrato repoContrato;
        private readonly RepositorioUsuario repoUsuario;
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioInquilino repoInquilino;

        public ContratoController(IConfiguration configuration)
        {
            repoContrato = new RepositorioContrato(configuration);
            repoUsuario = new RepositorioUsuario(configuration);
            repoInmueble = new RepositorioInmueble(configuration);
            repoInquilino = new RepositorioInquilino(configuration);

        }


        public IActionResult Index(int pagina = 1)
        {
            int tamPagina = 10;
            var contratos = repoContrato.ObtenerPaginado(pagina, tamPagina);
            int totalContratos = repoContrato.Contar();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalContratos / tamPagina);

            return View(contratos);
        }


        public IActionResult Details(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null)
                return NotFound();

            ViewBag.Usuarios = repoUsuario.ObtenerDiccionarioUsuarios();
            ViewBag.EsAdmin = User.IsInRole("Administrador");


            return View(contrato);
        }


        public IActionResult Create()
        {
            ViewBag.Inmuebles = repoContrato.ObtenerTodosInmuebles();
            ViewBag.Inquilinos = repoContrato.ObtenerTodosInquilinos();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Contrato contrato)
        {

            if (ModelState.IsValid)
            {
                try

                {
                    int usuarioId = ObtenerUsuarioIdLogueado();
                    repoContrato.Alta(contrato, usuarioId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.Inmuebles = repoContrato.ObtenerTodosInmuebles();
            ViewBag.Inquilinos = repoContrato.ObtenerTodosInquilinos();
            return View(contrato);
        }



        public IActionResult Edit(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null)
                return NotFound();

            ViewBag.Inmuebles = repoContrato.ObtenerTodosInmuebles();
            ViewBag.Inquilinos = repoContrato.ObtenerTodosInquilinos();
            return View(contrato);
        }


        [HttpPost]
        public IActionResult Edit(int id, Contrato contrato)
        {
            if (id != contrato.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    repoContrato.Modificacion(contrato);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.Inmuebles = repoContrato.ObtenerTodosInmuebles();
            ViewBag.Inquilinos = repoContrato.ObtenerTodosInquilinos();
            return View(contrato);
        }

        private int ObtenerUsuarioIdLogueado()
        {

            var claim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (claim != null && int.TryParse(claim.Value, out int usuarioId))
                return usuarioId;


            throw new Exception("No se pudo obtener el Id del usuario logueado. Asegúrese de estar autenticado.");
        }



        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null)
                return NotFound();

            return View(contrato);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            int usuarioId = ObtenerUsuarioIdLogueado();
            repoContrato.Baja(id, usuarioId);
            return RedirectToAction(nameof(Index));
        }

      
        public IActionResult Renovar(int id)
        {
            var contrato = repoContrato.ObtenerPorId(id);
            if (contrato == null || contrato.Estado != EstadoContrato.Activo)
            {
                TempData["Error"] = "El contrato no existe o ya no está activo.";
                return RedirectToAction(nameof(Index));
            }

           
            var modelo = new Contrato
            {
                InmuebleId = contrato.InmuebleId,
                InquilinoId = contrato.InquilinoId,
                FechaInicio = contrato.FechaFin.AddDays(1),
                FechaFin = contrato.FechaFin.AddYears(2),   
                Monto = contrato.Monto
            };

            ViewBag.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
            ViewBag.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Renovar(Contrato contrato)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
                ViewBag.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
                return View(contrato);
            }

            
            if (repoContrato.ExisteSuperposicion(contrato.InmuebleId, contrato.FechaInicio, contrato.FechaFin))
            {
                ModelState.AddModelError("", "El inmueble ya tiene un contrato en esas fechas.");
                ViewBag.Inmueble = repoInmueble.ObtenerPorId(contrato.InmuebleId);
                ViewBag.Inquilino = repoInquilino.ObtenerPorId(contrato.InquilinoId);
                return View(contrato);
            }

            var usuarioId = ObtenerUsuarioIdLogueado();
            contrato.CreadoPor = usuarioId;
            contrato.CreadoEn = DateTime.Now;
            contrato.Estado = EstadoContrato.Activo;

            repoContrato.Alta(contrato, usuarioId);

            TempData["Mensaje"] = "Contrato renovado correctamente.";
            return RedirectToAction(nameof(Index));
        }

            public IActionResult Terminar(int id)
            {
                var contrato = repoContrato.ObtenerPorId(id);
                if (contrato == null || contrato.Estado != EstadoContrato.Activo)
                {
                    TempData["Error"] = "El contrato no existe o ya no está activo.";
                    return RedirectToAction(nameof(Index));
                }

                return View(contrato);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult TerminarConfirmado(int id)
            {
                var contrato = repoContrato.ObtenerPorId(id);
                if (contrato == null || contrato.Estado != EstadoContrato.Activo)
                {
                    TempData["Error"] = "El contrato no existe o ya no está activo.";
                    return RedirectToAction(nameof(Index));
                }

                contrato.Estado = EstadoContrato.Finalizado;
                contrato.TerminadoPor = ObtenerUsuarioIdLogueado();
                contrato.TerminadoEn = DateTime.Now;

                repoContrato.Modificacion(contrato);

                TempData["Mensaje"] = "Contrato terminado anticipadamente.";
                return RedirectToAction(nameof(Index));
            }


    }
}





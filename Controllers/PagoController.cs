using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly RepositorioPago repoPago;
        private readonly RepositorioUsuario repoUsuario;
        private readonly RepositorioContrato repoContrato;
        private readonly RepositorioInquilino repoInquilino;

        public PagoController(IConfiguration configuration)
        {
            repoPago = new RepositorioPago(configuration);
            repoUsuario = new RepositorioUsuario(configuration);
            repoContrato = new RepositorioContrato(configuration);
            repoInquilino = new RepositorioInquilino(configuration);
        }

        public IActionResult Index()
        {
            var pagos = repoPago.ObtenerTodos();
            ViewBag.Usuarios = repoUsuario.ObtenerDiccionarioUsuarios();
            return View(pagos);
        }

        public IActionResult Create()
        {
            ViewBag.Contratos = new SelectList(ObtenerContratosConInfo(), "Id", "Texto");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pago pago)
        {
            if (ModelState.IsValid)
            {
                int usuarioId = ObtenerUsuarioIdLogueado();
                pago.CreadoPor = usuarioId;
                pago.CreadoEn = DateTime.Now;
                repoPago.Crear(pago);
                return RedirectToAction("Index");
            }

            ViewBag.Contratos = new SelectList(ObtenerContratosConInfo(), "Id", "Texto");
            return View(pago);
        }

        public IActionResult Edit(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

            ViewBag.Contratos = new SelectList(ObtenerContratosConInfo(), "Id", "Texto", pago.ContratoId);
            return View(pago);
        }

        [HttpPost]
        public IActionResult Edit(Pago pago)
        {
            if (ModelState.IsValid)
            {
                repoPago.Editar(pago);
                return RedirectToAction("Index");
            }
            ViewBag.Contratos = new SelectList(ObtenerContratosConInfo(), "Id", "Texto", pago.ContratoId);
            return View(pago);
        }

        public IActionResult Details(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

            ViewBag.Usuarios = repoUsuario.ObtenerDiccionarioUsuarios();
            ViewBag.EsAdmin = User.IsInRole("Administrador");
            return View(pago);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var pago = repoPago.ObtenerPorId(id);
            if (pago == null) return NotFound();

            return View(pago);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            int usuarioLogueadoId = ObtenerUsuarioIdLogueado();
            repoPago.Anular(id, usuarioLogueadoId);
            return RedirectToAction("Index");
        }

        private int ObtenerUsuarioIdLogueado()
        {

            var claim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (claim != null && int.TryParse(claim.Value, out int usuarioId))
                return usuarioId;


            throw new Exception("No se pudo obtener el Id del usuario logueado. Asegúrese de estar autenticado.");
        }


        private List<dynamic> ObtenerContratosConInfo()
        {
            var contratos = repoContrato.ObtenerTodos();
            var lista = new List<dynamic>();
            foreach (var c in contratos)
            {
                var inquilino = repoInquilino.ObtenerPorId(c.InquilinoId);
                string nombreInquilino = inquilino != null ? $"{inquilino.Nombre} {inquilino.Apellido}" : "Desconocido";

                lista.Add(new { Id = c.Id, Texto = $"Contrato {c.Id} - Inquilino {nombreInquilino}" });
            }
            return lista;
        }

    }
}

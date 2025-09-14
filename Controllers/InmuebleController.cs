using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Mvc;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly RepositorioInmueble repoInmueble;
        private readonly RepositorioPropietario repoPropietario;

        public InmuebleController(IConfiguration configuration)
        {
            repoInmueble = new RepositorioInmueble(configuration);
            repoPropietario = new RepositorioPropietario(configuration);
        }


        public IActionResult Index()
        {
            var inmuebles = repoInmueble.ObtenerTodos();
            return View(inmuebles);
        }

       
        public IActionResult Details(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }

        
        // public IActionResult Create()
        // {
        //     ViewBag.Propietarios = repoPropietario.ObtenerTodos();
        //     ViewBag.Usos = Enum.GetValues(typeof(UsoInmueble));
        //     return View();
        // }


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public IActionResult Create(Inmueble inmueble)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         ViewBag.Propietarios = repoPropietario.ObtenerTodos();
        //         return View(inmueble);
        //     }
        //     repoInmueble.Alta(inmueble);
        //     return RedirectToAction(nameof(Index));
        // }

        public IActionResult Create()
{
    ViewBag.Propietarios = repoPropietario.ObtenerTodos();
    return View(new Inmueble());
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Inmueble inmueble)
{
    // Depuración: mostrar errores si ModelState es inválido
    if (!ModelState.IsValid)
    {
        var errores = string.Join("<br>", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));
        ViewBag.Errores = errores;

        ViewBag.Propietarios = repoPropietario.ObtenerTodos();
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
            return View(inmueble);
        }
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
        ViewBag.Propietarios = repoPropietario.ObtenerTodos();
        return View(inmueble);
    }
}


        
        public IActionResult Edit(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            return View(inmueble);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                return View(inmueble);
            }
            repoInmueble.Modificacion(inmueble);
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Delete(int id)
        {
            var inmueble = repoInmueble.ObtenerPorId(id);
            if (inmueble == null)
                return NotFound();
            return View(inmueble);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repoInmueble.Baja(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

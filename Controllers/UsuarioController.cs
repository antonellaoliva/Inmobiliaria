using INMOBILIARIA__Oliva_Perez.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace INMOBILIARIA__Oliva_Perez.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;
        private readonly RepositorioUsuario repositorio;

        public UsuarioController(IWebHostEnvironment env, IConfiguration config, RepositorioUsuario repo)
        {
            environment = env;
            configuration = config;
            repositorio = repo;
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var usuario = repositorio.ObtenerTodos();
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Details(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            usuario.Password = Hash(usuario.Password);
            usuario.Avatar = "/Uploads/default-avatar.png";

            string error;
            var id = repositorio.Alta(usuario, out error);
            usuario.Id = id;

            if (id == 0)
            {
                ModelState.AddModelError("Email", error);
                return View(usuario);
            }


            if (usuario.AvatarFile != null)
            {
                string ruta = GuardarAvatar(usuario.AvatarFile, id);
                usuario.Avatar = ruta;
                repositorio.Modificacion(usuario, out error); 
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var original = repositorio.ObtenerPorId(id);
            usuario.Password = original.Password; 
            usuario.Avatar = original.Avatar ?? "/Uploads/default-avatar.png";

            if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
            {
                
                if (!string.IsNullOrEmpty(original.Avatar) && !original.Avatar.EndsWith("default-avatar.png"))
                {
                    EliminarAvatar(original);
                }

                
                string ruta = GuardarAvatar(usuario.AvatarFile, id);
                usuario.Avatar = ruta;
            }
            else
            {
                
                usuario.Avatar = original.Avatar ?? "/Uploads/default-avatar.png";
            }


            string error;
            var result = repositorio.Modificacion(usuario, out error);

            if (result == 0)
            {
                ModelState.AddModelError("Email", error);
                return View(usuario);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            var usuario = repositorio.ObtenerPorId(id);
            EliminarAvatar(usuario);
            repositorio.Baja(id);
            return RedirectToAction(nameof(Index));
        }

        
        [Authorize]
        public IActionResult Perfil()
        {
            var email = User.Identity.Name;
            var usuario = repositorio.ObtenerPorEmail(email);
            return View(usuario);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Perfil(Usuario usuario, string? nuevaClave)
        {
            var actual = repositorio.ObtenerPorEmail(User.Identity.Name);

            if (usuario.Id != actual.Id)
                return RedirectToAction("Index", "Home");

            usuario.Rol = actual.Rol;
            usuario.Email = actual.Email;
            usuario.Password = !string.IsNullOrWhiteSpace(nuevaClave) ? Hash(nuevaClave) : actual.Password;

            if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
            {
                
                if (!string.IsNullOrEmpty(actual.Avatar) && !actual.Avatar.EndsWith("default-avatar.png"))
                    EliminarAvatar(actual);

                usuario.Avatar = GuardarAvatar(usuario.AvatarFile, usuario.Id);
            }
            else
            {
                
                if (usuario.Avatar.EndsWith("default-avatar.png") && !actual.Avatar.EndsWith("default-avatar.png"))
                    EliminarAvatar(actual);

                
                usuario.Avatar = usuario.Avatar ?? "/Uploads/default-avatar.png";
            }

            string error;
            var result = repositorio.Modificacion(usuario, out error);

            if (result == 0)
            {
                ModelState.AddModelError("", error);
                return View(usuario);
            }

            TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Perfil");
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string clave)
        {
            var returnUrl = string.IsNullOrWhiteSpace(TempData["returnUrl"]?.ToString()) 
                ? "/" 
                : TempData["returnUrl"].ToString();



            var usuario = repositorio.ObtenerPorEmail(email);
            if (usuario == null || usuario.Password != Hash(clave))
            {
                ModelState.AddModelError("", "Email o clave incorrecta");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("Id", usuario.Id.ToString()),
                new Claim("NombreCompleto", usuario.Nombre + " " + usuario.Apellido)
                
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            if (string.IsNullOrEmpty(returnUrl))
            returnUrl = "/";

            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }


        private string GuardarAvatar(IFormFile archivo, int id)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = $"avatar_{id}{Path.GetExtension(archivo.FileName)}";
            string fullPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }

            return "/Uploads/" + fileName;
        }

        private void EliminarAvatar(Usuario u)
        {
            if (string.IsNullOrWhiteSpace(u.Avatar) || u.Avatar.EndsWith("default-avatar.png"))
                return;

            string ruta = Path.Combine(environment.WebRootPath, u.Avatar.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }
        }


        [AllowAnonymous]
        public IActionResult AccesoDenegado()
        {
            return View();
        }


        private string Hash(string clave)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: clave,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
        }
    }
}




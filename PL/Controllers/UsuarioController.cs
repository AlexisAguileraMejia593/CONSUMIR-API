using Microsoft.AspNetCore.Mvc;


namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Login_registrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            ML.Result result = BL.Usuario.GetByEmail(username);
            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (password == usuario.Password)
                {
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "Contraseña Incorrecta";
                    ViewBag.Correo = false;
                    return RedirectToAction("Login_registrar", "Usuario");
                }
            }
            else
            {
                ViewBag.Mensaje = "No existe la cuenta";
                ViewBag.Correo = false;
                return PartialView("Modal");
            }

        }

    }
}
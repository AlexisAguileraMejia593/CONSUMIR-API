using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace PL.Controllers
{
    public class OlvideContraseña : Controller
    {
        public IActionResult Index()
        {
              return View();
        }
        public IActionResult RecuperarContraseña(string username)
        {
            string emailOrigen = "alexaguilera992000@gmail.com";

            MailMessage mailMessage = new MailMessage
       (emailOrigen, username, "RecuperarContraseña", "<p>Correo para recuperar contraseña</p>");    

            mailMessage.IsBodyHtml = true;

            string url = "http://192.168.0.104/Usuario/RecuperarPassword/ " + System.Web.HttpUtility.UrlEncode(username);

            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "mmzv ajfh midx pxmy");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            ViewBag.Modal = "show";
            ViewBag.Mensaje = "Se ha enviado un correo de confirmacion a tu correo electronico";
            return View();
        }
    }
}

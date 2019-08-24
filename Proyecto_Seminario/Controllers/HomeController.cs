using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Seminario.Models;

namespace Proyecto_Seminario.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext db = new ModelContext();

        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["session_token"] != null)
            {
                try
                {
                    GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(Request.Cookies["session_token"]);

                    if (payload.ExpirationTimeSeconds > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        Usuarios usuario = db.Usuarios.Where(item => item.UsuarioEmail == payload.Email).FirstOrDefault();

                        if (usuario != null)
                        {
                            return View();
                            
                        }
                        else
                        {
                            return RedirectToAction("Index", "Authentication");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                catch (InvalidJwtException exc)
                {
                    return RedirectToAction("Index", "Authentication");
                }
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

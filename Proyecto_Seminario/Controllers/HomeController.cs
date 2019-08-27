using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Services;

namespace Proyecto_Seminario.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext db = new ModelContext();

        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    return View();
                }
                else
                {
                    TokenManager.removeCookies(Response);
                    return RedirectToAction("Index", "Authentication");
                }
            }
            else
            {
                TokenManager.removeCookies(Response);
                return RedirectToAction("Index", "Authentication");
            }
        }

        public IActionResult Log_out()
        {
            TokenManager.removeCookies(Response);

            return Ok();
        }

    }
}

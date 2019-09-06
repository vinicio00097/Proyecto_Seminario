using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Services;

namespace Proyecto_Seminario.Controllers
{
    [Route("Home/Usuarios/")]
    public class UsuariosController : Controller
    {
        private ModelContext modelContext = new ModelContext();
  
        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    var usuarios = modelContext.Usuarios.Where(usuario => usuario.IdUsuario.ToString() != TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value);

                    return Ok(new JsonMessage(
                        "success",
                        "31",
                        usuarios,
                        "Usuarios listos."));
                }
                else
                {
                    TokenManager.removeCookies(Response);
                    return NotFound(new JsonMessage(
                     "fail",
                     "0",
                     null,
                     "Acceso no autorizado."));
                }
            }
            else
            {
                TokenManager.removeCookies(Response);
                return NotFound(new JsonMessage(
                 "fail",
                 "0",
                 null,
                 "Acceso no autorizado."));
            }
        }


        // POST: Usuarios/Create
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] Usuarios usuario)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        await modelContext.Usuarios.AddAsync(usuario);
                        await modelContext.SaveChangesAsync();

                        return Ok(new JsonMessage(
                            "success",
                            "32",
                            usuario,
                            "Usuario agregado."));
                    }
                    else
                    {
                        TokenManager.removeCookies(Response);
                        return NotFound(new JsonMessage(
                         "fail",
                         "0",
                         null,
                         "Acceso no autorizado."));
                    }
                }
                else
                {
                    TokenManager.removeCookies(Response);
                    return NotFound(new JsonMessage(
                     "fail",
                     "0",
                     null,
                     "Acceso no autorizado."));
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: Usuarios/Edit
        [HttpPut("Edit")]
        public async Task<ActionResult> Edit([FromBody] Usuarios usuario)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        modelContext.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await modelContext.SaveChangesAsync();

                        return Ok(new JsonMessage(
                            "success",
                            "33",
                            usuario,
                            "Usuario editado."));
                    }
                    else
                    {
                        TokenManager.removeCookies(Response);
                        return NotFound(new JsonMessage(
                         "fail",
                         "0",
                         null,
                         "Acceso no autorizado."));
                    }
                }
                else
                {
                    TokenManager.removeCookies(Response);
                    return NotFound(new JsonMessage(
                     "fail",
                     "0",
                     null,
                     "Acceso no autorizado."));
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: Usuarios/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Usuarios usuario=modelContext.Usuarios.Where(user=>user.IdUsuario==id).FirstOrDefault();

                        Debug.WriteLine("putaaaa");
                        modelContext.Usuarios.Remove(usuario);
                        await modelContext.SaveChangesAsync();

                        return Ok(new JsonMessage(
                            "success",
                            "39",
                            usuario,
                            "Usuario eliminado."));
                    }
                    else
                    {
                        TokenManager.removeCookies(Response);
                        return NotFound(new JsonMessage(
                         "fail",
                         "0",
                         null,
                         "Acceso no autorizado."));
                    }
                }
                else
                {
                    TokenManager.removeCookies(Response);
                    return NotFound(new JsonMessage(
                     "fail",
                     "0",
                     null,
                     "Acceso no autorizado."));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
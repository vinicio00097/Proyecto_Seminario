using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Services;

namespace Proyecto_Seminario.Controllers
{
    [Route("Home/Tareas/")]
    public class TasksController : Controller
    {
        private ModelContext modelContext = new ModelContext();
        // GET: Tasks
        public async Task<ActionResult> IndexAsync()
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    string Id_Usuario = TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value;

                    var tasks = modelContext.InstanciasplantillasPasosDetalle.Select(task => new
                    {
                        task.IdPlantillaPasoDetalle,
                        task.InstanciaPlantilla,
                        paso = new
                        {
                            task.PasoNavigation.IdPasoinstancia,
                            task.PasoNavigation.Nombre,
                            task.PasoNavigation.Descripcion
                        },
                        estado = modelContext.Acciones.Where(accion => accion.IdAccion == task.Estado).
                            Select(accion=>new
                            {
                                accion.IdAccion,
                                accion.Nombre
                            }).FirstOrDefault(),
                        usuario_accion = 
                            modelContext.Usuarios.Where(usuario => usuario.IdUsuario == task.UsuarioAccion).
                            Select(usuario=>new
                            {
                                usuario.IdUsuario,
                                usuario.Nombres,
                                usuario.Apellidos
                            }).FirstOrDefault(),
                        participantes = task.PasosinstanciasUsuariosDetalle.Select(participante => new
                        {
                            participante.UsuarioNavigation.IdUsuario,
                            participante.UsuarioNavigation.Nombres,
                            participante.UsuarioNavigation.Apellidos,
                        }),
                        task.FechaInicio,
                        task.FechaFin
                    });

                    return Ok(new JsonMessage(
                        "success",
                        "41",
                        tasks,
                        "Tareas listas."));
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

        /*
        // GET: Tasks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
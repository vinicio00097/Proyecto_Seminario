using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Services;

namespace Proyecto_Seminario.Controllers
{
    [Route("Home/InstanciasPlantillas/")]
    public class InstanciasPlantillasController : Controller
    {
        private ModelContext modelContext = new ModelContext();

        // GET: InstanciasPlantillas
        public async Task<ActionResult> Index()
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    var plantillas = modelContext.Instanciasplantillas.Select(item => new
                    {
                        item.IdInstanciaPlantilla,
                        item.Nombre,
                        item.Descripcion,
                        item.Estado,
                        item.Iniciada,
                        Campos_Datos = item.InstanciasplantillasDatosDetalle.Select(campoDato => new
                        {
                            campoDato.IdInstanciaPlantillaDato,
                            campoDato.Instanciaplantilla,
                            campoDato.NombreCampo,
                            campoDato.Dato
                        }).OrderBy(item2 => item2.IdInstanciaPlantillaDato),
                        Pasos = item.InstanciasplantillasPasosDetalle.Select(paso => new
                        {
                            paso.PasoNavigation.IdPasoinstancia,
                            paso.IdPlantillaPasoDetalle,
                            paso.PasoNavigation.Nombre,
                            paso.PasoNavigation.Descripcion,
                            UsuarioAccion = new
                            {
                                paso.UsuarioAccionNavigation.IdUsuario,
                                paso.UsuarioAccionNavigation.Nombres,
                                paso.UsuarioAccionNavigation.Apellidos
                            },
                            Estado = new
                            {
                                paso.EstadoNavigation.IdAccion,
                                paso.EstadoNavigation.Nombre
                            },
                            Pasos_Datos = paso.PasoNavigation.PasosinstanciasDatosDetalle.Select(pasos_datos => new
                            {
                                pasos_datos.IdPasosinstanciasDatos,
                                pasos_datos.InstanciaPlantillaDatoNavigation.IdInstanciaPlantillaDato,
                                pasos_datos.SoloLectura,
                                pasos_datos.InstanciaPlantillaDatoNavigation.NombreCampo,
                                pasos_datos.InstanciaPlantillaDatoNavigation.Dato
                            }),
                            Pasos_Usuarios = paso.PasosUsuariosDetalle.Select(pasos_usuarios => new
                            {
                                pasos_usuarios.IdPasosUsuarios,
                                pasos_usuarios.UsuarioNavigation.IdUsuario,
                                pasos_usuarios.UsuarioNavigation.Nombres,
                                pasos_usuarios.UsuarioNavigation.Apellidos
                            })
                        })
                    });

                    return Ok(new JsonMessage(
                        "success",
                        "21",
                        plantillas,
                        "Instancias plantillas listas."));
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

        // POST: InstanciasPlantillas/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] dynamic plantilla)
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    var data = JsonConvert.SerializeObject(plantilla);
                    /*Instanciasplantillas newInstanciasplantilla = new Instanciasplantillas();
                    newInstanciasplantilla.Nombre = plantilla.Nombre;
                    newInstanciasplantilla.Descripcion = plantilla.Descripcion;
                    newInstanciasplantilla.Usuario = int.Parse( TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value);
                    newInstanciasplantilla.Iniciada = "0";
                    newInstanciasplantilla.Estado = "0";
                    modelContext.Instanciasplantillas.Add(newInstanciasplantilla);
                    await modelContext.SaveChangesAsync();
                    Debug.WriteLine(plantilla.PlantillasCamposDetalle.Count);
                    Debug.WriteLine(plantilla.PlantillasPasosDetalle.Count);
                    foreach (PlantillasPasosDetalle item in plantilla.PlantillasPasosDetalle)
                    {
                        Pasosinstancias newPasosinstancias = new Pasosinstancias();
                        Debug.WriteLine(item.PasoNavigation.Nombre);
                    }*/



                    return Ok(new JsonMessage(
                        "success",
                        "22",
                        data,
                        "Instancia de plantilla agregada."
                    ));
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


        // POST: InstanciasPlantillas/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: InstanciasPlantillas/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
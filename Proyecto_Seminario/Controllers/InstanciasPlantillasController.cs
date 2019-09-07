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
                    string Id_Usuario = TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value;

                    var plantillas = modelContext.Instanciasplantillas.Where(plantilla => plantilla.Usuario.ToString() == Id_Usuario).Select(item => new
                    {
                        item.IdInstanciaPlantilla,
                        item.Nombre,
                        item.Descripcion,
                        item.Estado,
                        item.Iniciada,
                        UsuarioNavigation = new
                        {
                            item.UsuarioNavigation.IdUsuario,
                            item.UsuarioNavigation.Nombres,
                            item.UsuarioNavigation.Apellidos
                        },
                        Datos = item.InstanciasplantillasDatosDetalle.Select(campoDato => new
                        {
                            campoDato.IdInstanciaPlantillaDato,
                            campoDato.Instanciaplantilla,
                            campoDato.NombreCampo,
                            campoDato.TipoDato,
                            TipoDatoNavigation = new
                            {
                                campoDato.TipoDatoNavigation.IdTipoDato,
                                campoDato.TipoDatoNavigation.Nombre
                            },
                            campoDato.DatoString,
                            campoDato.DatoInteger,
                            campoDato.DatoDate
                        }).OrderBy(item2 => item2.IdInstanciaPlantillaDato),
                        Pasos = item.InstanciasplantillasPasosDetalle.Select(paso => new
                        {
                            paso.PasoNavigation.IdPasoinstancia,
                            paso.IdPlantillaPasoDetalle,
                            paso.PasoNavigation.Nombre,
                            paso.PasoNavigation.Descripcion,
                            UsuarioAccion = modelContext.Usuarios.Where(user => user.IdUsuario == paso.UsuarioAccion).Select(user => new
                            {
                                user.IdUsuario,
                                user.Nombres,
                                user.Apellidos
                            }).FirstOrDefault(),
                            EstadoNavigation = modelContext.Acciones.Where(accion => accion.IdAccion == paso.Estado).Select(accion => new
                            {
                                accion.IdAccion,
                                accion.Nombre
                            }).FirstOrDefault(),
                            Datos = paso.PasoNavigation.PasosinstanciasDatosDetalle.Select(pasos_datos => new
                            {
                                pasos_datos.InstanciaPlantillaDatoNavigation.IdInstanciaPlantillaDato,
                                pasos_datos.InstanciaPlantillaDatoNavigation.Instanciaplantilla,
                                pasos_datos.SoloLectura,
                                pasos_datos.InstanciaPlantillaDatoNavigation.NombreCampo,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoString,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoInteger,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoDate,
                            }),
                            Usuarios = paso.PasosUsuariosDetalle.Select(pasos_usuarios => new
                            {
                                pasos_usuarios.IdPasosUsuarios,
                                pasos_usuarios.UsuarioNavigation.IdUsuario,
                                pasos_usuarios.UsuarioNavigation.Nombres,
                                pasos_usuarios.UsuarioNavigation.Apellidos
                            })
                        }).OrderBy(paso=>paso.IdPasoinstancia)
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
        public async Task<IActionResult> Create([FromBody] Plantillas plantilla)
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    var data = plantilla;
                    Instanciasplantillas newInstanciasplantilla = new Instanciasplantillas();
                    newInstanciasplantilla.Nombre = plantilla.Nombre;
                    newInstanciasplantilla.Descripcion = plantilla.Descripcion;
                    newInstanciasplantilla.Usuario = int.Parse( TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value);
                    newInstanciasplantilla.Iniciada = "0";
                    newInstanciasplantilla.Estado = "0";
                    newInstanciasplantilla.UsuarioNavigation = modelContext.Usuarios.Where(user=>user.IdUsuario==newInstanciasplantilla.Usuario).FirstOrDefault();

                    await modelContext.Instanciasplantillas.AddAsync(newInstanciasplantilla);
                    await modelContext.SaveChangesAsync();

                    foreach (PlantillasPasosDetalle item in plantilla.PlantillasPasosDetalle)
                    {
                        Pasosinstancias newPasoInstancia = new Pasosinstancias
                        {
                            Nombre = item.PasoNavigation.Nombre,
                            Descripcion = item.PasoNavigation.Descripcion,
                        };
                        await modelContext.Pasosinstancias.AddAsync(newPasoInstancia);
                        await modelContext.SaveChangesAsync();


                        InstanciasplantillasPasosDetalle newInstanciasplantillasPasosDetalle = new InstanciasplantillasPasosDetalle
                        {
                            InstanciaPlantilla = newInstanciasplantilla.IdInstanciaPlantilla,
                            Paso = newPasoInstancia.IdPasoinstancia,
                        };
                        await modelContext.InstanciasplantillasPasosDetalle.AddAsync(newInstanciasplantillasPasosDetalle);
                        await modelContext.SaveChangesAsync();

                        newInstanciasplantillasPasosDetalle.PasoNavigation = newPasoInstancia;
                        newInstanciasplantilla.InstanciasplantillasPasosDetalle.Add(newInstanciasplantillasPasosDetalle);
                    }

                    foreach (PlantillasCamposDetalle item in plantilla.PlantillasCamposDetalle)
                    {
                        InstanciasplantillasDatosDetalle newInstanciasplantillasDatosDetalle = new InstanciasplantillasDatosDetalle();
                        newInstanciasplantillasDatosDetalle.Instanciaplantilla = newInstanciasplantilla.IdInstanciaPlantilla;
                        newInstanciasplantillasDatosDetalle.TipoDato = item.TipoDatoNavigation.IdTipoDato;
                        newInstanciasplantillasDatosDetalle.NombreCampo = item.NombreCampo;

                        await modelContext.InstanciasplantillasDatosDetalle.AddAsync(newInstanciasplantillasDatosDetalle);
                        await modelContext.SaveChangesAsync();
                    }

                    return Ok(new JsonMessage(
                        "success",
                        "22",
                        newInstanciasplantilla,
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
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(int id,[FromBody] List<InstanciaPaso> pasosInicializados)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Debug.WriteLine(JsonConvert.SerializeObject(pasosInicializados));

                        return Ok(new JsonMessage(
                            "success",
                            "23",
                            pasosInicializados,
                            "Proceso iniciado."));
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
            catch(Exception e)
            {
                Debug.WriteLine(e);

                return NotFound(new JsonMessage(
                 "fail",
                 "0",
                 null,
                 "Acceso no autorizado."));
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
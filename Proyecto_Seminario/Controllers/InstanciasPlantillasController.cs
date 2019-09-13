using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Models.ModifiedModel;
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
                            paso.FechaInicio,
                            paso.FechaFin,
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
                            Datos_Pasos = paso.PasoNavigation.PasosinstanciasDatosDetalle.Select(pasos_datos => new
                            {
                                pasos_datos.InstanciaPlantillaDatoNavigation.IdInstanciaPlantillaDato,
                                pasos_datos.InstanciaPlantillaDatoNavigation.Instanciaplantilla,
                                pasos_datos.SoloLectura,
                                pasos_datos.InstanciaPlantillaDatoNavigation.NombreCampo,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoString,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoInteger,
                                pasos_datos.InstanciaPlantillaDatoNavigation.DatoDate,
                            }),
                            Usuarios = paso.PasosinstanciasUsuariosDetalle.Select(pasos_usuarios => new
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
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var data = plantilla;
                        Instanciasplantillas newInstanciasplantilla = new Instanciasplantillas();
                        newInstanciasplantilla.Nombre = plantilla.Nombre;
                        newInstanciasplantilla.Descripcion = plantilla.Descripcion;
                        newInstanciasplantilla.Usuario = int.Parse(TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value);
                        newInstanciasplantilla.Iniciada = "0";
                        newInstanciasplantilla.Estado = "0";
                        newInstanciasplantilla.UsuarioNavigation = modelContext.Usuarios.Where(user => user.IdUsuario == newInstanciasplantilla.Usuario).FirstOrDefault();

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
                            "Proceso agregado."
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
            catch(Exception exc)
            {
                return NotFound(new JsonMessage(
                 "fail",
                 "25",
                 null,
                 "Ha ocurrido un error. Contacte a soporte. Error: " + exc));
            }
        }


        // POST: InstanciasPlantillas/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(int id,[FromBody] InstanciaPlantilla instanciaPlantilla)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Instanciasplantillas instanciaplantilla = modelContext.Instanciasplantillas.Where(instancia => instancia.IdInstanciaPlantilla == id).FirstOrDefault();

                        if (instanciaplantilla == null)
                        {
                            return NotFound(new JsonMessage(
                             "fail",
                             "20",
                             null,
                             "El proceso no se puede iniciar porque ha sido eliminado."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject( instanciaPlantilla));
                        /*foreach(Dato dato in instanciaPlantilla.Datos)
                        {
                            InstanciasplantillasDatosDetalle toUpdate = modelContext.
                                InstanciasplantillasDatosDetalle.
                                Where(datoDB => datoDB.IdInstanciaPlantillaDato == dato.IdInstanciaPlantillaDato).
                                FirstOrDefault();

                            toUpdate.DatoString = dato.DatoString;
                            toUpdate.DatoInteger = dato.DatoInteger;
                            toUpdate.DatoDate = dato.DatoDate;

                            modelContext.Entry(toUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();
                        }

                        foreach(InstanciaPaso paso in instanciaPlantilla.Pasos)
                        {
                            InstanciasplantillasPasosDetalle instanciasplantillasPasosDetalle =
                                modelContext.InstanciasplantillasPasosDetalle.Where(pasoDetalle => pasoDetalle.IdPlantillaPasoDetalle == paso.IdPlantillaPasoDetalle).
                                FirstOrDefault();
                            instanciasplantillasPasosDetalle.FechaInicio = paso.Fecha_Inicio;
                            instanciasplantillasPasosDetalle.FechaFin = paso.Fecha_Fin;

                            modelContext.Entry(instanciasplantillasPasosDetalle).State =
                                Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            foreach(Dato dato in paso.Datos_Pasos)
                            {
                                PasosinstanciasDatosDetalle pasosinstanciasDatosDetalle = new PasosinstanciasDatosDetalle();
                                pasosinstanciasDatosDetalle.InstanciaPlantillaDato = dato.IdInstanciaPlantillaDato;
                                pasosinstanciasDatosDetalle.Paso = paso.IdPasoInstancia;
                                pasosinstanciasDatosDetalle.SoloLectura = dato.SoloLectura == "true" ? "1" : "0";

                                await modelContext.PasosinstanciasDatosDetalle.AddAsync(pasosinstanciasDatosDetalle);
                                await modelContext.SaveChangesAsync();
                            }

                            foreach(Usuarios usuario in paso.Usuarios)
                            {
                                PasosUsuariosDetalle pasosUsuariosDetalle = new PasosUsuariosDetalle();
                                pasosUsuariosDetalle.PlantillaPasoDetalle = paso.IdPlantillaPasoDetalle;
                                pasosUsuariosDetalle.Usuario = usuario.IdUsuario;

                                await modelContext.PasosUsuariosDetalle.AddAsync(pasosUsuariosDetalle);
                                await modelContext.SaveChangesAsync();
                            }

                            Debug.WriteLine(JsonConvert.SerializeObject(paso));
                        }

                        instanciaplantilla.Iniciada = "1";
                        modelContext.Entry(instanciaplantilla).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await modelContext.SaveChangesAsync();*/

                        return Ok(new JsonMessage(
                        "success",
                        "23",
                        instanciaplantilla,
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
            catch(Exception exc)
            {
                return NotFound(new JsonMessage(
                 "fail",
                 "25",
                 null,
                 "Ha ocurrido un error. Contacte a soporte. Error: "+exc));
            }
        }

        // DELETE: InstanciasPlantillas/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Instanciasplantillas instanciaplantilla = modelContext.Instanciasplantillas.Where(instancia => instancia.IdInstanciaPlantilla == id).FirstOrDefault();

                        if (instanciaplantilla == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "20",
                            null,
                            "El proceso ya ha sido eliminado."));
                        }

                        modelContext.Instanciasplantillas.Remove(instanciaplantilla);
                        await modelContext.SaveChangesAsync();

                        return Ok(new JsonMessage(
                        "success",
                        "29",
                        instanciaplantilla,
                        "Proceso eliminado."));
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
            catch(Exception exc)
            {
                return NotFound(new JsonMessage(
                "fail",
                "25",
                null,
                "Ha ocurrido un error inesperado. Contacte a soporte. Error: "+exc));
            }
        }
    }
}
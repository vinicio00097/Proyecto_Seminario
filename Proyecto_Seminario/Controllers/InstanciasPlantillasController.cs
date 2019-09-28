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
using Proyecto_Seminario.Models.ModifiedModel.Plantillas;
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

        [HttpGet("List")]
        public async Task<ActionResult> AllInstances()
        {
            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    string Id_Usuario = TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value;

                    var plantillas = modelContext.Instanciasplantillas.Select(item => new
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
                        }).OrderBy(paso => paso.IdPasoinstancia)
                    }).Where(process=>process.UsuarioNavigation.IdUsuario.ToString()!=Id_Usuario);

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

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(decimal? id)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var proceso = modelContext.Instanciasplantillas.Where(procesoItem => procesoItem.IdInstanciaPlantilla == id).Select(item => new
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
                            }).OrderBy(paso => paso.IdPasoinstancia)
                        }).FirstOrDefault();

                        if (proceso == null)
                        {
                            return NotFound(new JsonMessage(
                             "fail",
                             "20",
                             null,
                             "No se encontró el proceso que está tratando de ver, puede ser que haya sido eliminada."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(proceso));
                        return Ok(new JsonMessage(
                            "success",
                            "21",
                            proceso,
                            "Proceso listo."));
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
            catch (Exception exc)
            {
                return NotFound(new JsonMessage(
                "fail",
                "25",
                null,
                "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }
        }

        // POST: InstanciasPlantillas/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Plantilla plantilla)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {

                        Plantillas verifyTemplate = modelContext.Plantillas.Where(plantillaVerify => plantillaVerify.IdPlantilla == plantilla.IdPlantilla).FirstOrDefault();

                        if (verifyTemplate == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "20",
                            null,
                            "El proceso no se puede crear porque ha sido eliminada la plantilla."));
                        }

                        Instanciasplantillas newInstanciasplantilla = new Instanciasplantillas
                        {
                            Nombre = plantilla.Nombre,
                            Descripcion = plantilla.Descripcion,
                            Usuario = int.Parse(TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value),
                            Iniciada = "0",
                            Estado = "0"
                        };
                        newInstanciasplantilla.UsuarioNavigation = modelContext.Usuarios.Where(user => user.IdUsuario == newInstanciasplantilla.Usuario).FirstOrDefault();

                        await modelContext.Instanciasplantillas.AddAsync(newInstanciasplantilla);
                        await modelContext.SaveChangesAsync();

                        foreach (Campo item in plantilla.Campos)
                        {
                            InstanciasplantillasDatosDetalle newInstanciasplantillasDatosDetalle = new InstanciasplantillasDatosDetalle();
                            newInstanciasplantillasDatosDetalle.Instanciaplantilla = newInstanciasplantilla.IdInstanciaPlantilla;
                            newInstanciasplantillasDatosDetalle.TipoDato = item.TipoDatoNavigation.IdTipoDato;
                            newInstanciasplantillasDatosDetalle.NombreCampo = item.NombreCampo;

                            await modelContext.InstanciasplantillasDatosDetalle.AddAsync(newInstanciasplantillasDatosDetalle);
                            await modelContext.SaveChangesAsync();

                            item.IdOrder = item.IdPlantillaCampo;
                            item.IdPlantillaCampo = newInstanciasplantillasDatosDetalle.IdInstanciaPlantillaDato;
                        }

                        foreach (Paso item in plantilla.Pasos)
                        {
                            Pasosinstancias newPasoInstancia = new Pasosinstancias
                            {
                                Nombre = item.Nombre,
                                Descripcion = item.Nombre,
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

                            foreach(Campo campo in item.Datos_Pasos)
                            {
                                PasosinstanciasDatosDetalle newPasosinstanciasDatosDetalle = new PasosinstanciasDatosDetalle
                                {
                                    InstanciaPlantillaDato = plantilla.Campos.Where(idDato => idDato.IdOrder == campo.IdPlantillaCampo).FirstOrDefault().IdPlantillaCampo,
                                    Paso = newPasoInstancia.IdPasoinstancia,
                                    SoloLectura = campo.SoloLectura
                                };

                                await modelContext.PasosinstanciasDatosDetalle.AddAsync(newPasosinstanciasDatosDetalle);
                                await modelContext.SaveChangesAsync();
                            }

                            foreach (Usuario usuario in item.Usuarios)
                            {
                                PasosinstanciasUsuariosDetalle newPasosinstanciasUsuariosDetalle = new PasosinstanciasUsuariosDetalle();
                                newPasosinstanciasUsuariosDetalle.PlantillaPasoDetalle = newInstanciasplantillasPasosDetalle.IdPlantillaPasoDetalle;
                                newPasosinstanciasUsuariosDetalle.Usuario = usuario.IdUsuario;

                                await modelContext.PasosinstanciasUsuariosDetalle.AddAsync(newPasosinstanciasUsuariosDetalle);
                                await modelContext.SaveChangesAsync();
                            }

                            newInstanciasplantillasPasosDetalle.PasoNavigation = newPasoInstancia;
                            newInstanciasplantilla.InstanciasplantillasPasosDetalle.Add(newInstanciasplantillasPasosDetalle);
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(plantilla));

                        var startedInstanceTemplate = modelContext.Instanciasplantillas.Where(instanceTemplate => instanceTemplate.IdInstanciaPlantilla == newInstanciasplantilla.IdInstanciaPlantilla).Select(item => new
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
                            }).OrderBy(item2 => item2.IdInstanciaPlantillaDato)
                        }).FirstOrDefault();

                        return Ok(new JsonMessage(
                            "success",
                            "22",
                            startedInstanceTemplate,
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
                        foreach(Dato dato in instanciaPlantilla.Datos)
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

                        instanciaplantilla.Iniciada = "1";
                        modelContext.Entry(instanciaplantilla).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await modelContext.SaveChangesAsync();

                        var instanciaplantillaReady = modelContext.Instanciasplantillas.Where(proceso=>proceso.IdInstanciaPlantilla==instanciaPlantilla.IdInstanciaPlantilla).Select(item => new
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
                            }).OrderBy(paso => paso.IdPasoinstancia)
                        }).FirstOrDefault();

                        return Ok(new JsonMessage(
                        "success",
                        "23",
                        instanciaplantillaReady,
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
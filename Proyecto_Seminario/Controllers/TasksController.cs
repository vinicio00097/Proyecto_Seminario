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

                    var tasks2 = modelContext.PasosinstanciasUsuariosDetalle.Where(item=>item.Usuario.ToString()==Id_Usuario&&
                    item.PlantillaPasoDetalleNavigation.InstanciaPlantillaNavigation.Iniciada=="1"&&
                    item.PlantillaPasoDetalleNavigation.InstanciaPlantillaNavigation.Estado == "0").
                        Select(task => new
                    {
                            task.PlantillaPasoDetalleNavigation.IdPlantillaPasoDetalle,
                            task.PlantillaPasoDetalleNavigation.InstanciaPlantilla,
                            task.PlantillaPasoDetalleNavigation.PasoNavigation.IdPasoinstancia,
                            task.PlantillaPasoDetalleNavigation.PasoNavigation.Nombre,
                            task.PlantillaPasoDetalleNavigation.PasoNavigation.Descripcion,
                            Datos_Pasos =task.PlantillaPasoDetalleNavigation.PasoNavigation.PasosinstanciasDatosDetalle.Select(dato_paso=>new
                            {
                                dato_paso.IdPasosinstanciasDatos,
                                dato_paso.InstanciaPlantillaDatoNavigation.IdInstanciaPlantillaDato,
                                dato_paso.InstanciaPlantillaDatoNavigation.NombreCampo,
                                dato_paso.SoloLectura,
                                dato_paso.InstanciaPlantillaDatoNavigation.TipoDato,
                                dato_paso.InstanciaPlantillaDatoNavigation.DatoString,
                                dato_paso.InstanciaPlantillaDatoNavigation.DatoInteger,
                                dato_paso.InstanciaPlantillaDatoNavigation.DatoDate,
                                TipoDatoNavigation =new
                                {
                                    dato_paso.InstanciaPlantillaDatoNavigation.TipoDatoNavigation.IdTipoDato,
                                    dato_paso.InstanciaPlantillaDatoNavigation.TipoDatoNavigation.Nombre

                                },
                            }),
                            task.PlantillaPasoDetalleNavigation.Estado,
                            estadoNavigation = modelContext.Acciones.Where(accion => accion.IdAccion == task.PlantillaPasoDetalleNavigation.Estado).
                            Select(accion => new
                            {
                                accion.IdAccion,
                                accion.Nombre
                            }).FirstOrDefault(),
                            usuarioAccion=task.PlantillaPasoDetalleNavigation.UsuarioAccion,
                            usuarioAccionNavigation =
                            modelContext.Usuarios.Where(usuario => usuario.IdUsuario == task.PlantillaPasoDetalleNavigation.UsuarioAccion).
                            Select(usuario => new
                            {
                                usuario.IdUsuario,
                                usuario.Nombres,
                                usuario.Apellidos
                            }).FirstOrDefault(),
                            usuarios = task.PlantillaPasoDetalleNavigation.PasosinstanciasUsuariosDetalle.Select(participante => new
                            {
                                participante.UsuarioNavigation.IdUsuario,
                                participante.UsuarioNavigation.Nombres,
                                participante.UsuarioNavigation.Apellidos,
                            }),
                            task.PlantillaPasoDetalleNavigation.FechaInicio,
                            task.PlantillaPasoDetalleNavigation.FechaFin
                        }).OrderBy(task=>task.IdPlantillaPasoDetalle);

                    return Ok(new JsonMessage(
                        "success",
                        "41",
                        tasks2,
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

        [HttpPut("Edit/Start/{id}")]
        public async Task<ActionResult> Start(int id,[FromBody] InstanciaPaso instanciaPaso)
        {
            //try
            //{
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var verifyStep = modelContext.InstanciasplantillasPasosDetalle.
                            Where(paso => paso.IdPlantillaPasoDetalle == id&&paso.UsuarioAccion==null&&
                            (paso.Estado == null
                            || (paso.EstadoNavigation.Nombre == "Regresar" || paso.EstadoNavigation.Nombre == "Regresado")
                            || (paso.EstadoNavigation.Nombre == "Redireccionar" || paso.EstadoNavigation.Nombre == "Redireccionado"))&&
                            paso.InstanciaPlantillaNavigation.Iniciada=="1"&&paso.InstanciaPlantillaNavigation.Estado=="0").FirstOrDefault();

                        if (verifyStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo iniciar la tarea porque ya ha sido iniciada o el proceso de ésta se ha detenido"));
                        }

                        string Id_Usuario = TokenManager.getClaims(Request.Cookies["session_token"]).FindFirst("user_id").Value;

                        var allSteps = modelContext.InstanciasplantillasPasosDetalle.
                           Where(step => step.InstanciaPlantilla == instanciaPaso.InstanciaPlantilla)
                           .OrderBy(step=>step.IdPlantillaPasoDetalle).ToList();

                        int indexofTaskToApprove = allSteps.ToList().FindIndex(hola => hola.IdPlantillaPasoDetalle == verifyStep.IdPlantillaPasoDetalle);

                        if (indexofTaskToApprove == 0)
                        {
                            verifyStep.UsuarioAccion = decimal.Parse(Id_Usuario);
                            verifyStep.FechaInicio = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                            modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            instanciaPaso.UsuarioAccion = int.Parse(Id_Usuario);
                            instanciaPaso.UsuarioAccionNavigation = instanciaPaso.Usuarios.Where(user => user.IdUsuario.ToString() == Id_Usuario).FirstOrDefault();
                        }
                        else
                        {
                            InstanciasplantillasPasosDetalle previosApprove = allSteps.ElementAt(indexofTaskToApprove - 1);
                            Acciones verifiedState = modelContext.Acciones.Where(accion => accion.Nombre == "Aprobar" || accion.Nombre == "Aprobado").FirstOrDefault();

                            if (previosApprove.Estado == verifiedState.IdAccion)
                            {
                                verifyStep.UsuarioAccion = decimal.Parse(Id_Usuario);
                                verifyStep.FechaInicio = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                                modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                await modelContext.SaveChangesAsync();

                                instanciaPaso.UsuarioAccion = int.Parse(Id_Usuario);
                                instanciaPaso.UsuarioAccionNavigation = instanciaPaso.Usuarios.Where(user => user.IdUsuario.ToString() == Id_Usuario).FirstOrDefault();
                            }
                            else
                            {
                                return NotFound(new JsonMessage(
                                "fail",
                                "40",
                                null,
                                "No se pudo iniciar la tarea porque la tarea previa a ésta no ha sido aprobada."));
                            }
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "43",
                            instanciaPaso,
                            "Tarea iniciada."));
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
            /*}
            catch (Exception exc)
            {
                return NotFound(new JsonMessage(
                 "fail",
                 "45",
                 null,
                 "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }*/
        }

        private async Task overwriteDataFieldsAsync(List<Dato> datos)
        {
            foreach(Dato dato in datos.FindAll(item=>item.SoloLectura=="0"))
            {
                InstanciasplantillasDatosDetalle instanciasplantillasDato = modelContext.InstanciasplantillasDatosDetalle.
                    Where(datoProceso => datoProceso.IdInstanciaPlantillaDato == dato.IdInstanciaPlantillaDato).FirstOrDefault();

                instanciasplantillasDato.DatoString = dato.DatoString;
                instanciasplantillasDato.DatoInteger = dato.DatoInteger;
                instanciasplantillasDato.DatoDate = dato.DatoDate;

                modelContext.Entry(instanciasplantillasDato).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await modelContext.SaveChangesAsync();

            }
        }

        [HttpPut("Edit/Approve/{id}")]
        public async Task<ActionResult> Approve(int id, [FromBody] InstanciaPaso instanciaPaso)
        {
            //try
            //{
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var verifyStep = modelContext.InstanciasplantillasPasosDetalle.
                            Where(paso => paso.IdPlantillaPasoDetalle == id &&
                            (paso.Estado == null
                            || (paso.EstadoNavigation.Nombre == "Regresar" || paso.EstadoNavigation.Nombre == "Regresado")
                            || (paso.EstadoNavigation.Nombre == "Redireccionar" || paso.EstadoNavigation.Nombre == "Redireccionado"))&&
                            paso.InstanciaPlantillaNavigation.Iniciada == "1" && paso.InstanciaPlantillaNavigation.Estado == "0").FirstOrDefault();

                        if (verifyStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo aprobar la tarea porque ya ha sido aprobada o el proceso de ésta se ha detenido."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(instanciaPaso));

                        var allSteps = modelContext.InstanciasplantillasPasosDetalle.
                            Where(step => step.InstanciaPlantilla == instanciaPaso.InstanciaPlantilla).
                            OrderBy(step=>step.IdPlantillaPasoDetalle).ToList();

                        int indexofTaskToApprove = allSteps.ToList().FindIndex(hola => hola.IdPlantillaPasoDetalle == verifyStep.IdPlantillaPasoDetalle);

                        if (indexofTaskToApprove == 0)
                        {
                            Acciones verifiedState = modelContext.Acciones.Where(accion => accion.Nombre == "Aprobar" || accion.Nombre == "Aprobado").FirstOrDefault();
                            verifyStep.Estado = verifiedState.IdAccion;
                            verifyStep.FechaFin = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));

                            if (allSteps.Count() == 1)
                            {
                                Instanciasplantillas proceso = modelContext.Instanciasplantillas.
                                    Where(procesoItem => procesoItem.IdInstanciaPlantilla == verifyStep.InstanciaPlantilla).FirstOrDefault();

                                proceso.Estado = "1";

                                modelContext.Entry(proceso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                await modelContext.SaveChangesAsync();
                            }

                            await overwriteDataFieldsAsync(instanciaPaso.Datos_Pasos);

                            modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            instanciaPaso.Estado = verifiedState.IdAccion;
                            instanciaPaso.EstadoNavigation = verifiedState;
                        }
                        else
                        {
                            InstanciasplantillasPasosDetalle previosApprove = allSteps.ElementAt(indexofTaskToApprove - 1);
                            Acciones verifiedState = modelContext.Acciones.Where(accion => accion.Nombre == "Aprobar" || accion.Nombre == "Aprobado").FirstOrDefault();

                            if (previosApprove.Estado == verifiedState.IdAccion)
                            {
                                verifyStep.Estado = verifiedState.IdAccion;
                                verifyStep.FechaFin = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));

                                if (allSteps.Count()-1 == indexofTaskToApprove)
                                {
                                    Instanciasplantillas proceso = modelContext.Instanciasplantillas.
                                        Where(procesoItem => procesoItem.IdInstanciaPlantilla == verifyStep.InstanciaPlantilla).FirstOrDefault();

                                    proceso.Estado = "1";

                                    modelContext.Entry(proceso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    await modelContext.SaveChangesAsync();
                                }

                                await overwriteDataFieldsAsync(instanciaPaso.Datos_Pasos);

                                modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                await modelContext.SaveChangesAsync();

                                instanciaPaso.Estado = verifiedState.IdAccion;
                                instanciaPaso.EstadoNavigation = verifiedState;
                            }
                            else
                            {
                                return NotFound(new JsonMessage(
                                "fail",
                                "40",
                                null,
                                "No se pudo aprobar la tarea porque la tarea previa a esta no ha sido aprobada."));
                            }
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "43",
                            instanciaPaso,
                            "Tarea aprobada."));
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
            /*}
            catch (Exception exc)
            {
                return NotFound(new JsonMessage(
                 "fail",
                 "45",
                 null,
                 "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }*/
        }

        [HttpPut("Edit/Reject/{id}")]
        public async Task<ActionResult> Reject(int id, [FromBody] InstanciaPaso instanciaPaso)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var verifyStep = modelContext.InstanciasplantillasPasosDetalle.
                            Where(paso => paso.IdPlantillaPasoDetalle == id && (paso.Estado == null
                            ||(paso.EstadoNavigation.Nombre=="Regresar"|| paso.EstadoNavigation.Nombre == "Regresado")
                            || (paso.EstadoNavigation.Nombre == "Redireccionar" || paso.EstadoNavigation.Nombre == "Redireccionado"))&&
                            paso.InstanciaPlantillaNavigation.Iniciada == "1" && paso.InstanciaPlantillaNavigation.Estado == "0").FirstOrDefault();

                        if (verifyStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo rechazar la tarea porque ya ha sido rechazada o el proceso de ésta se ha detenido."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(instanciaPaso));

                        var allSteps = modelContext.InstanciasplantillasPasosDetalle.
                            Where(step => step.InstanciaPlantilla == instanciaPaso.InstanciaPlantilla).
                            OrderBy(step => step.IdPlantillaPasoDetalle).ToList();

                        int indexofTaskToApprove = allSteps.ToList().FindIndex(hola => hola.IdPlantillaPasoDetalle == verifyStep.IdPlantillaPasoDetalle);

                        if (indexofTaskToApprove == 0)
                        {
                            Acciones verifiedState = modelContext.Acciones.Where(accion => accion.Nombre == "Rechazar" || accion.Nombre == "Rechazado").FirstOrDefault();
                            verifyStep.Estado = verifiedState.IdAccion;
                            verifyStep.FechaFin = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));

                            Instanciasplantillas proceso = modelContext.Instanciasplantillas.
                                Where(procesoItem => procesoItem.IdInstanciaPlantilla == verifyStep.InstanciaPlantilla).FirstOrDefault();

                            proceso.Estado = "1";

                            modelContext.Entry(proceso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            instanciaPaso.Estado = verifiedState.IdAccion;
                            instanciaPaso.EstadoNavigation = verifiedState;
                        }
                        else
                        {
                            InstanciasplantillasPasosDetalle previousApprove = allSteps.ElementAt(indexofTaskToApprove - 1);
                            Acciones verifiedPreviousState = modelContext.Acciones.Where(accion => accion.Nombre == "Aprobar" || accion.Nombre == "Aprobado").FirstOrDefault();

                            if (previousApprove.Estado == verifiedPreviousState.IdAccion)
                            {
                                Acciones rejectState = modelContext.Acciones.Where(accion => accion.Nombre == "Rechazar" || accion.Nombre == "Rechazado").FirstOrDefault();
                                verifyStep.Estado = rejectState.IdAccion;
                                verifyStep.FechaFin = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));

                                Instanciasplantillas proceso = modelContext.Instanciasplantillas.
                                    Where(procesoItem => procesoItem.IdInstanciaPlantilla == verifyStep.InstanciaPlantilla).FirstOrDefault();

                                proceso.Estado = "1";

                                modelContext.Entry(proceso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                await modelContext.SaveChangesAsync();

                                modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                await modelContext.SaveChangesAsync();

                                instanciaPaso.Estado = rejectState.IdAccion;
                                instanciaPaso.EstadoNavigation = rejectState;
                            }
                            else
                            {
                                return NotFound(new JsonMessage(
                                "fail",
                                "40",
                                null,
                                "No se pudo rechazar la tarea porque la tarea previa a esta no ha sido aprobada."));
                            }
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "43",
                            instanciaPaso,
                            "Tarea rechazada."));
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
                 "45",
                 null,
                 "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }
        }

        [HttpPut("Edit/Return/{id}")]
        public async Task<ActionResult> Return(int id, [FromBody] InstanciaPaso instanciaPaso)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var verifyStep = modelContext.InstanciasplantillasPasosDetalle.
                            Where(paso => paso.IdPlantillaPasoDetalle == id && (paso.Estado == null
                            || (paso.EstadoNavigation.Nombre == "Regresar" || paso.EstadoNavigation.Nombre == "Regresado")
                            || (paso.EstadoNavigation.Nombre == "Redireccionar" || paso.EstadoNavigation.Nombre == "Redireccionado")) &&
                            paso.InstanciaPlantillaNavigation.Iniciada == "1" && paso.InstanciaPlantillaNavigation.Estado == "0").FirstOrDefault();

                        if (verifyStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo regresar la tarea porque ya ha sido regresada o el proceso de ésta se ha detenido."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(instanciaPaso));

                        var allSteps = modelContext.InstanciasplantillasPasosDetalle.
                            Where(step => step.InstanciaPlantilla == instanciaPaso.InstanciaPlantilla).
                            OrderBy(step => step.IdPlantillaPasoDetalle).ToList();

                        int indexofTaskToApprove = allSteps.ToList().FindIndex(hola => hola.IdPlantillaPasoDetalle == verifyStep.IdPlantillaPasoDetalle);

                        if (indexofTaskToApprove != 0)
                        {
                            Acciones returnedState = modelContext.Acciones.Where(accion => accion.Nombre == "Regresar" || accion.Nombre == "Regresado").FirstOrDefault();
                            verifyStep.Estado = returnedState.IdAccion;
                            verifyStep.UsuarioAccion = null;

                            modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            InstanciasplantillasPasosDetalle previousApprove = allSteps.ElementAt(indexofTaskToApprove - 1);
                            previousApprove.Estado = null;
                            modelContext.Entry(previousApprove).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await modelContext.SaveChangesAsync();

                            instanciaPaso.Estado = returnedState.IdAccion;
                            instanciaPaso.EstadoNavigation = returnedState;
                        }
                        else
                        {
                            return NotFound(new JsonMessage(
                                "fail",
                                "40",
                                null,
                                "No se pudo retornar la tarea porque esta es la primer tarea del proceso."));
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "43",
                            instanciaPaso,
                            "Tarea retornada."));
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
                 "45",
                 null,
                 "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }
        }

        [HttpPut("Edit/Redirect/{id}")]
        public async Task<ActionResult> Redirect(int id, [FromBody] InstanciaPaso instanciaPaso)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        var verifyStep = modelContext.InstanciasplantillasPasosDetalle.
                            Where(paso => paso.IdPlantillaPasoDetalle == id && (paso.Estado == null
                            || (paso.EstadoNavigation.Nombre == "Regresar" || paso.EstadoNavigation.Nombre == "Regresado")
                            || (paso.EstadoNavigation.Nombre == "Redireccionar" || paso.EstadoNavigation.Nombre == "Redireccionado")) &&
                            paso.InstanciaPlantillaNavigation.Iniciada == "1" && paso.InstanciaPlantillaNavigation.Estado == "0").FirstOrDefault();

                        if (verifyStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo redireccionar la tarea porque ya ha sido redireccionada o el proceso de ésta se ha detenido."));
                        }

                        Usuarios newUserStep = modelContext.Usuarios.Where(user => user.IdUsuario == instanciaPaso.UsuarioAccion).FirstOrDefault();
                        Acciones redirectedState = modelContext.Acciones.Where(accion => accion.Nombre == "Redireccionar" || accion.Nombre == "Redireccionado").FirstOrDefault();

                        if (newUserStep == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "40",
                            null,
                            "No se pudo redireccionar la tarea porque el usuario seleccionado no se ha encontrado."));
                        }

                        verifyStep.UsuarioAccion = newUserStep.IdUsuario;
                        verifyStep.Estado = redirectedState.IdAccion;
                        modelContext.Entry(verifyStep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await modelContext.SaveChangesAsync();

                        instanciaPaso.UsuarioAccion = newUserStep.IdUsuario;
                        instanciaPaso.UsuarioAccionNavigation = newUserStep;
                        instanciaPaso.Estado = redirectedState.IdAccion;
                        instanciaPaso.EstadoNavigation = redirectedState;

                        return Ok(new JsonMessage(
                            "success",
                            "43",
                            instanciaPaso,
                            "Tarea redireccionada."));
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
                 "45",
                 null,
                 "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
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
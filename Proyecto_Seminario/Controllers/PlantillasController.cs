using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Seminario;
using Proyecto_Seminario.Models;
using Proyecto_Seminario.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics;
using Newtonsoft.Json;
using Proyecto_Seminario.Models.ModifiedModel;
using Proyecto_Seminario.Models.ModifiedModel.Plantillas;

namespace Proyecto_Seminario.Controllers
{
    [Route("Home/Plantillas/")]
    public class PlantillasController : Controller
    {
        private ModelContext modelContext = new ModelContext();

        // GET: Plantillas
        public async Task<IActionResult> Index()
        {

            if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
            {
                if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                {
                    var plantillas = modelContext.Plantillas.Select(item => new
                    {
                        item.IdPlantilla,
                        item.Nombre,
                        item.Descripcion,
                        Campos = item.PlantillasCamposDetalle.Select(campo=>new
                        {
                            campo.IdPlantillaCampo,
                            campo.Plantilla,
                            campo.NombreCampo,
                            campo.TipoDato,
                            TipoDatoNavigation = new
                            {
                                campo.TipoDatoNavigation.IdTipoDato,
                                campo.TipoDatoNavigation.Nombre
                            }
                        }).OrderBy(item2=>item2.IdPlantillaCampo),
                        Pasos = item.PlantillasPasosDetalle.Select(paso => new
                        {
                            paso.IdPlantillaPaso,
                            paso.PasoNavigation.IdPaso,
                            paso.Plantilla,
                            paso.PasoNavigation.Nombre,
                            paso.PasoNavigation.Descripcion,
                            Datos_Pasos = paso.PasoNavigation.PasosDatosDetalle.Select(pasos_datos => new
                            {
                                pasos_datos.IdPasoDato,
                                pasos_datos.PlantillaCampoNavigation.IdPlantillaCampo,
                                pasos_datos.PlantillaCampoNavigation.Plantilla,
                                pasos_datos.PlantillaCampoNavigation.NombreCampo,
                                pasos_datos.SoloLectura,
                                pasos_datos.PlantillaCampoNavigation.TipoDato,
                                TipoDatoNavigation = new
                                {
                                    pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.IdTipoDato,
                                    pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.Nombre
                                }
                            }),
                            Usuarios = paso.PasosUsuariosDetalle.Select(pasos_usuarios => new
                            {
                                pasos_usuarios.IdPasoUsuario,
                                pasos_usuarios.UsuarioNavigation.IdUsuario,
                                pasos_usuarios.UsuarioNavigation.Nombres,
                                pasos_usuarios.UsuarioNavigation.Apellidos
                            })
                        }).OrderBy(paso=>paso.IdPlantillaPaso)
                    });

                    Debug.WriteLine(JsonConvert.SerializeObject(plantillas));
                    return Ok(new JsonMessage(
                        "success",
                        "11",
                        plantillas,
                        "Plantillas listas."));
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
                        var plantilla = modelContext.Plantillas.Select(item => new
                        {
                            item.IdPlantilla,
                            item.Nombre,
                            item.Descripcion,
                            Campos = item.PlantillasCamposDetalle.Select(campo => new
                            {
                                campo.IdPlantillaCampo,
                                campo.Plantilla,
                                campo.NombreCampo,
                                campo.TipoDato,
                                TipoDatoNavigation = new
                                {
                                    campo.TipoDatoNavigation.IdTipoDato,
                                    campo.TipoDatoNavigation.Nombre
                                }
                            }).OrderBy(item2 => item2.IdPlantillaCampo),
                            Pasos = item.PlantillasPasosDetalle.Select(paso => new
                            {
                                paso.IdPlantillaPaso,
                                paso.PasoNavigation.IdPaso,
                                paso.Plantilla,
                                paso.PasoNavigation.Nombre,
                                paso.PasoNavigation.Descripcion,
                                Datos_Pasos = paso.PasoNavigation.PasosDatosDetalle.Select(pasos_datos => new
                                {
                                    pasos_datos.IdPasoDato,
                                    pasos_datos.PlantillaCampoNavigation.IdPlantillaCampo,
                                    pasos_datos.PlantillaCampoNavigation.Plantilla,
                                    pasos_datos.PlantillaCampoNavigation.NombreCampo,
                                    pasos_datos.SoloLectura,
                                    pasos_datos.PlantillaCampoNavigation.TipoDato,
                                    TipoDatoNavigation = new
                                    {
                                        pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.IdTipoDato,
                                        pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.Nombre
                                    }
                                }),
                                Usuarios = paso.PasosUsuariosDetalle.Select(pasos_usuarios => new
                                {
                                    pasos_usuarios.IdPasoUsuario,
                                    pasos_usuarios.UsuarioNavigation.IdUsuario,
                                    pasos_usuarios.UsuarioNavigation.Nombres,
                                    pasos_usuarios.UsuarioNavigation.Apellidos
                                })
                            }).OrderBy(paso=>paso.IdPlantillaPaso)
                        }).Where(hola => hola.IdPlantilla == id).FirstOrDefault();

                        if (plantilla == null)
                        {
                            return NotFound(new JsonMessage(
                             "fail",
                             "10",
                             null,
                             "No se encontró la plantilla que está tratando de ver, puede ser que haya sido eliminada."));
                        }

                        Debug.WriteLine(JsonConvert.SerializeObject(plantilla));
                        return Ok(new JsonMessage(
                            "success",
                            "11",
                            plantilla,
                            "Plantillas listas."));
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
                "15",
                null,
                "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]Plantilla plantilla)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Debug.WriteLine(JsonConvert.SerializeObject(plantilla));

                        Plantillas newPlantilla = new Plantillas
                        {
                            Nombre = plantilla.Nombre,
                            Descripcion = plantilla.Descripcion
                        };

                        await modelContext.Plantillas.AddAsync(newPlantilla);
                        await modelContext.SaveChangesAsync();
                        plantilla.IdPlantilla = newPlantilla.IdPlantilla;


                        foreach(Campo campo in plantilla.Campos)
                        {
                            PlantillasCamposDetalle plantillasCamposDetalle = new PlantillasCamposDetalle
                            {
                                Plantilla = newPlantilla.IdPlantilla,
                                NombreCampo = campo.NombreCampo,
                                TipoDato = campo.TipoDato
                            };

                            await modelContext.PlantillasCamposDetalle.AddAsync(plantillasCamposDetalle);
                            await modelContext.SaveChangesAsync();
                            campo.IdPlantillaCampo = plantillasCamposDetalle.IdPlantillaCampo;

                        }

                        foreach(Paso paso in plantilla.Pasos)
                        {
                            Pasos newPaso = new Pasos
                            {
                                Nombre = paso.Nombre,
                                Descripcion = paso.Descripcion
                            };

                            await modelContext.Pasos.AddAsync(newPaso);
                            await modelContext.SaveChangesAsync();
                            paso.IdPaso = int.Parse(newPaso.IdPaso.ToString());

                            PlantillasPasosDetalle newPlantillasPasosDetalle = new PlantillasPasosDetalle
                            {
                                Plantilla = newPlantilla.IdPlantilla,
                                Paso = newPaso.IdPaso
                            };

                            await modelContext.PlantillasPasosDetalle.AddAsync(newPlantillasPasosDetalle);
                            await modelContext.SaveChangesAsync();
                            paso.IdPlantillaPaso = int.Parse( newPlantillasPasosDetalle.IdPlantillaPaso.ToString());

                            foreach(Campo dato in paso.Datos_Pasos)
                            {
                                decimal idPlantillaCampo = plantilla.Campos.Where(campo => campo.IdOrder == dato.IdOrder).FirstOrDefault().IdPlantillaCampo;

                            PasosDatosDetalle pasosDatosDetalle = new PasosDatosDetalle
                                {

                                    PlantillaCampo = idPlantillaCampo,
                                    Paso = newPaso.IdPaso,
                                    SoloLectura = dato.SoloLectura == null || dato.SoloLectura == "0" ? "0" : "1"
                            };

                                await modelContext.PasosDatosDetalle.AddAsync(pasosDatosDetalle);
                                await modelContext.SaveChangesAsync();
                                dato.IdPasoDato = pasosDatosDetalle.IdPasoDato;
                                dato.IdPlantillaCampo = idPlantillaCampo;
                            }

                            foreach(Usuario usuario in paso.Usuarios)
                            {
                                PasosUsuariosDetalle pasosUsuariosDetalle = new PasosUsuariosDetalle();
                                pasosUsuariosDetalle.PlantillaPasoDetalle = newPlantillasPasosDetalle.IdPlantillaPaso;
                                pasosUsuariosDetalle.Usuario = usuario.IdUsuario;

                                await modelContext.PasosUsuariosDetalle.AddAsync(pasosUsuariosDetalle);
                                await modelContext.SaveChangesAsync();
                                usuario.IdPasoUsuario = pasosUsuariosDetalle.IdPasoUsuario;
                            }
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "12",
                            plantilla,
                            "Plantilla agregada."));
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
                "15",
                null,
                "Ha ocurrido un error. Contacte a soporte.Error: " + exc));
            }
            
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(decimal? id,[FromBody] Plantilla plantilla)
        {
            try
            {
                var verifyTemplate = modelContext.Plantillas.Where(item => item.IdPlantilla == id).FirstOrDefault();

                if (verifyTemplate == null)
                {
                    return NotFound(new JsonMessage(
                     "fail",
                     "10",
                     null,
                     "No se actualizó porque la plantilla ya ha sido eliminada."));
                }

                verifyTemplate.Nombre = plantilla.Nombre;
                verifyTemplate.Descripcion = plantilla.Descripcion;

                modelContext.Entry(verifyTemplate).State = EntityState.Modified;
                await modelContext.SaveChangesAsync();

                var fieldsToDelete = modelContext.PlantillasCamposDetalle.Where(campo => campo.Plantilla == verifyTemplate.IdPlantilla).ToList();
                var stepsToDelete = modelContext.PlantillasPasosDetalle.Where(paso => paso.Plantilla == verifyTemplate.IdPlantilla).ToList();
                Dictionary<decimal, decimal> fieldsDictionary = new Dictionary<decimal, decimal>();

                foreach(PlantillasCamposDetalle campo in fieldsToDelete)
                {
                    modelContext.PlantillasCamposDetalle.Remove(campo);
                    await modelContext.SaveChangesAsync();
                }

                foreach(PlantillasPasosDetalle paso in stepsToDelete)
                {
                    modelContext.PlantillasPasosDetalle.Remove(paso);
                    await modelContext.SaveChangesAsync();
                }



                foreach (Campo campo in plantilla.Campos)
                {
                    PlantillasCamposDetalle plantillasCamposDetalle = new PlantillasCamposDetalle
                    {
                        Plantilla = verifyTemplate.IdPlantilla,
                        NombreCampo = campo.NombreCampo,
                        TipoDato = campo.TipoDato
                    };

                    await modelContext.PlantillasCamposDetalle.AddAsync(plantillasCamposDetalle);
                    await modelContext.SaveChangesAsync();

                    if (campo.IdPlantillaCampo != 0)
                    {
                        fieldsDictionary.Add(campo.IdPlantillaCampo,plantillasCamposDetalle.IdPlantillaCampo);
                    }
                    else
                    {
                        fieldsDictionary.Add(campo.IdOrder, plantillasCamposDetalle.IdPlantillaCampo);
                    }
                }

                foreach (Paso paso in plantilla.Pasos)
                {
                    Pasos newPaso = new Pasos
                    {
                        Nombre = paso.Nombre,
                        Descripcion = paso.Descripcion
                    };

                    await modelContext.Pasos.AddAsync(newPaso);
                    await modelContext.SaveChangesAsync();
                    paso.IdPaso = int.Parse(newPaso.IdPaso.ToString());

                    PlantillasPasosDetalle newPlantillasPasosDetalle = new PlantillasPasosDetalle
                    {
                        Plantilla = verifyTemplate.IdPlantilla,
                        Paso = newPaso.IdPaso
                    };

                    await modelContext.PlantillasPasosDetalle.AddAsync(newPlantillasPasosDetalle);
                    await modelContext.SaveChangesAsync();
                    paso.IdPlantillaPaso = int.Parse(newPlantillasPasosDetalle.IdPlantillaPaso.ToString());

                    foreach (Campo dato in paso.Datos_Pasos)
                    {
                        decimal idPlantillaCampo;

                        if (dato.IdPlantillaCampo != 0)
                        {
                            idPlantillaCampo = fieldsDictionary[dato.IdPlantillaCampo];
                        }
                        else
                        {
                            idPlantillaCampo = fieldsDictionary[dato.IdOrder];
                        }

                        PasosDatosDetalle pasosDatosDetalle = new PasosDatosDetalle
                        {

                            PlantillaCampo = idPlantillaCampo,
                            Paso = newPaso.IdPaso,
                            SoloLectura = dato.SoloLectura==null||dato.SoloLectura=="0"?"0":"1"
                        };

                        await modelContext.PasosDatosDetalle.AddAsync(pasosDatosDetalle);
                        await modelContext.SaveChangesAsync();
                        dato.IdPasoDato = pasosDatosDetalle.IdPasoDato;
                        dato.IdPlantillaCampo = idPlantillaCampo;
                    }

                    foreach (Usuario usuario in paso.Usuarios)
                    {
                        PasosUsuariosDetalle pasosUsuariosDetalle = new PasosUsuariosDetalle();
                        pasosUsuariosDetalle.PlantillaPasoDetalle = newPlantillasPasosDetalle.IdPlantillaPaso;
                        pasosUsuariosDetalle.Usuario = usuario.IdUsuario;

                        await modelContext.PasosUsuariosDetalle.AddAsync(pasosUsuariosDetalle);
                        await modelContext.SaveChangesAsync();
                        usuario.IdPasoUsuario = pasosUsuariosDetalle.IdPasoUsuario;
                    }
                }

                var updatedTemplate = modelContext.Plantillas.Select(item => new
                {
                    item.IdPlantilla,
                    item.Nombre,
                    item.Descripcion,
                    Campos = item.PlantillasCamposDetalle.Select(campo => new
                    {
                        campo.IdPlantillaCampo,
                        campo.Plantilla,
                        campo.NombreCampo,
                        campo.TipoDato,
                        TipoDatoNavigation = new
                        {
                            campo.TipoDatoNavigation.IdTipoDato,
                            campo.TipoDatoNavigation.Nombre
                        }
                    }).OrderBy(item2 => item2.IdPlantillaCampo),
                    Pasos = item.PlantillasPasosDetalle.Select(paso => new
                    {
                        paso.IdPlantillaPaso,
                        paso.PasoNavigation.IdPaso,
                        paso.Plantilla,
                        paso.PasoNavigation.Nombre,
                        paso.PasoNavigation.Descripcion,
                        Datos_Pasos = paso.PasoNavigation.PasosDatosDetalle.Select(pasos_datos => new
                        {
                            pasos_datos.IdPasoDato,
                            pasos_datos.PlantillaCampoNavigation.IdPlantillaCampo,
                            pasos_datos.PlantillaCampoNavigation.Plantilla,
                            pasos_datos.SoloLectura,
                            pasos_datos.PlantillaCampoNavigation.TipoDato,
                            TipoDatoNavigation = new
                            {
                                pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.IdTipoDato,
                                pasos_datos.PlantillaCampoNavigation.TipoDatoNavigation.Nombre
                            }
                        }),
                        Usuarios = paso.PasosUsuariosDetalle.Select(pasos_usuarios => new
                        {
                            pasos_usuarios.IdPasoUsuario,
                            pasos_usuarios.UsuarioNavigation.IdUsuario,
                            pasos_usuarios.UsuarioNavigation.Nombres,
                            pasos_usuarios.UsuarioNavigation.Apellidos
                        })
                    }).OrderBy(paso => paso.IdPlantillaPaso)
                }).Where(hola => hola.IdPlantilla == id).FirstOrDefault();


                Debug.WriteLine(JsonConvert.SerializeObject(plantilla));

                return Ok(new JsonMessage(
                "success",
                "13",
                updatedTemplate,
                "Plantilla agregada."));
            }
            catch(Exception exc)
            {
                return NotFound(new JsonMessage(
                   "fail",
                   "15",
                   null,
                   "Ha ocurrido un error. Contacte a soporte.Error: " + exc));

            }
        }

        // GET: Plantillas/Details/5
        /*public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillas = await _context.Plantillas
                .FirstOrDefaultAsync(m => m.IdPlantilla == id);
            if (plantillas == null)
            {
                return NotFound();
            }

            return View(plantillas);
        }

        // GET: Plantillas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plantillas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlantilla,Nombre,Descripcion")] Plantillas plantillas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantillas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantillas);
        }

        // GET: Plantillas/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillas = await _context.Plantillas.FindAsync(id);
            if (plantillas == null)
            {
                return NotFound();
            }
            return View(plantillas);
        }

        // POST: Plantillas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("IdPlantilla,Nombre,Descripcion")] Plantillas plantillas)
        {
            if (id != plantillas.IdPlantilla)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantillas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillasExists(plantillas.IdPlantilla))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(plantillas);
        }

        // GET: Plantillas/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillas = await _context.Plantillas
                .FirstOrDefaultAsync(m => m.IdPlantilla == id);
            if (plantillas == null)
            {
                return NotFound();
            }

            return View(plantillas);
        }*/

        //POST: Plantillas/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Plantillas plantilla = modelContext.Plantillas.Where(item => item.IdPlantilla == id).FirstOrDefault();

                        if (plantilla == null)
                        {
                            return NotFound(new JsonMessage(
                            "fail",
                            "10",
                            null,
                            "La plantilla ya ha sido eliminada."));
                        }

                        modelContext.Plantillas.Remove(plantilla);
                        await modelContext.SaveChangesAsync();

                        return Ok(new JsonMessage(
                            "success",
                            "19",
                            plantilla,
                            "Plantilla eliminada."));
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
                "15",
                null,
                "Ha ocurrido un error. Contacte a soporte.Error: "+exc));
            }
        }


    }
}

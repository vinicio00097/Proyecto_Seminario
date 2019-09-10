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
                        PlantillasCamposDetalle = item.PlantillasCamposDetalle.Select(campo=>new
                        {
                            campo.IdPlantillaCampo,
                            campo.Plantilla,
                            campo.NombreCampo,
                            TipoDatoNavigation = new
                            {
                                campo.TipoDatoNavigation.IdTipoDato,
                                campo.TipoDatoNavigation.Nombre
                            }
                        }).OrderBy(item2=>item2.IdPlantillaCampo),
                        PlantillasPasosDetalle = item.PlantillasPasosDetalle.Select(paso => new
                        {
                            paso.IdPlantillaPaso,
                            paso.Plantilla,
                            paso.Paso,
                            PasoNavigation=new
                            {
                                paso.PasoNavigation.IdPaso,
                                paso.PasoNavigation.Nombre,
                                paso.PasoNavigation.Descripcion
                            },
                        })
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]Plantillas plantilla)
        {
            try
            {
                if (Request.Cookies["oauth_session_token"] != null && Request.Cookies["session_token"] != null)
                {
                    if (await TokenManager.ValidateGoogleToken(Request.Cookies["oauth_session_token"]) && TokenManager.ValidateToken(Request.Cookies["session_token"]))
                    {
                        Debug.WriteLine(JsonConvert.SerializeObject(plantilla));

                        Plantillas newPlantilla = new Plantillas();
                        newPlantilla.Nombre = plantilla.Nombre;
                        newPlantilla.Descripcion = plantilla.Descripcion;

                        await modelContext.Plantillas.AddAsync(newPlantilla);
                        await modelContext.SaveChangesAsync();

                        for (int indexCampo = 0; indexCampo < plantilla.PlantillasCamposDetalle.Count; indexCampo++)
                        {
                            plantilla.PlantillasCamposDetalle.ElementAt(indexCampo).IdPlantillaCampo = indexCampo + 1;
                            plantilla.PlantillasCamposDetalle.ElementAt(indexCampo).Plantilla = newPlantilla.IdPlantilla;

                            await modelContext.PlantillasCamposDetalle.AddAsync(plantilla.PlantillasCamposDetalle.ElementAt(indexCampo));
                            await modelContext.SaveChangesAsync();

                            plantilla.PlantillasCamposDetalle.ElementAt(indexCampo).TipoDatoNavigation =
                                modelContext.TiposDatos.Where(tipo => tipo.IdTipoDato ==
                                plantilla.PlantillasCamposDetalle.ElementAt(indexCampo).TipoDato).FirstOrDefault();
                            newPlantilla.PlantillasCamposDetalle.Add(plantilla.PlantillasCamposDetalle.ElementAt(indexCampo));
                        }

                        for (int indexPaso = 0; indexPaso < plantilla.PlantillasPasosDetalle.Count; indexPaso++)
                        {
                            Pasos paso = new Pasos
                            {
                                Nombre = plantilla.PlantillasPasosDetalle.ElementAt(indexPaso).PasoNavigation.Nombre,
                                Descripcion = plantilla.PlantillasPasosDetalle.ElementAt(indexPaso).PasoNavigation.Descripcion
                            };

                            await modelContext.Pasos.AddAsync(paso);
                            await modelContext.SaveChangesAsync();

                            PlantillasPasosDetalle newPlantillasPasosDetalle = new PlantillasPasosDetalle
                            {
                                IdPlantillaPaso = indexPaso + 1,
                                Plantilla = newPlantilla.IdPlantilla,
                                Paso = paso.IdPaso
                            };

                            await modelContext.PlantillasPasosDetalle.AddAsync(newPlantillasPasosDetalle);
                            await modelContext.SaveChangesAsync();

                            newPlantilla.PlantillasPasosDetalle.Add(newPlantillasPasosDetalle);
                        }

                        return Ok(new JsonMessage(
                            "success",
                            "12",
                            newPlantilla,
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

       /* // GET: Plantillas/Edit/5
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

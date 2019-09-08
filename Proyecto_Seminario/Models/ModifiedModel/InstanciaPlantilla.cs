using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models.ModifiedModel
{
    public class InstanciaPlantilla
    {
        public decimal IdInstanciaPlantilla { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Iniciada { get; set; }
        public string Descripcion { get; set; }
        public List<Dato> Datos { get; set; }
        public List<InstanciaPaso> Pasos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models.ModifiedModel
{
    public class Plantilla
    {
        public decimal IdPlantilla { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Campo> Campos { get; set; }
        public List<Paso> Pasos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models.ModifiedModel
{
    public class Dato
    {
        public int IdInstanciaPlantillaDato { get; set; }
        public int Instanciaplantilla { get; set; }
        public string SoloLectura { get; set; }
        public string NombreCampo { get; set; }
        public string DatoString { get; set; }
        public int? DatoInteger { get; set; }
        public DateTime? DatoDate { get; set; }
    }
}

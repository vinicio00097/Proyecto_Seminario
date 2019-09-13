using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models.ModifiedModel
{
    public class Campo
    {
        public decimal IdPlantillaCampo { get; set; }
        public decimal IdPasoDato { get; set; }
        public decimal IdOrder { get; set; }
        public int Plantilla { get; set; }
        public string SoloLectura { get; set; }
        public string NombreCampo { get; set; }
        public int TipoDato { get; set; }
        public TiposDatos TipoDatoNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PasosDatosDetalle
    {
        public decimal IdPasoDato { get; set; }
        public decimal PlantillaCampo { get; set; }
        public decimal Paso { get; set; }
        public string SoloLectura { get; set; }

        public virtual Pasos PasoNavigation { get; set; }
        public virtual PlantillasCamposDetalle PlantillaCampoNavigation { get; set; }
    }
}

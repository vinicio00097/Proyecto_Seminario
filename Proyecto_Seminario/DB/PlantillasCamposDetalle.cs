using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PlantillasCamposDetalle
    {
        public decimal IdPlantillaCampo { get; set; }
        public decimal Plantilla { get; set; }
        public string NombreCampo { get; set; }

        public virtual Plantillas PlantillaNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PlantillasPasosDetalle
    {
        public decimal IdPlantillaPaso { get; set; }
        public decimal Plantilla { get; set; }
        public decimal Paso { get; set; }

        public virtual Pasos PasoNavigation { get; set; }
        public virtual Plantillas PlantillaNavigation { get; set; }
    }
}

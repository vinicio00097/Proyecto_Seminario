using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PlantillasPasosDetalle
    {
        public PlantillasPasosDetalle()
        {
            PasosUsuariosDetalle = new HashSet<PasosUsuariosDetalle>();
        }

        public decimal Plantilla { get; set; }
        public decimal Paso { get; set; }
        public decimal IdPlantillaPaso { get; set; }

        public virtual Pasos PasoNavigation { get; set; }
        public virtual Plantillas PlantillaNavigation { get; set; }
        public virtual ICollection<PasosUsuariosDetalle> PasosUsuariosDetalle { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Plantillas
    {
        public Plantillas()
        {
            PlantillasCamposDetalle = new HashSet<PlantillasCamposDetalle>();
            PlantillasPasosDetalle = new HashSet<PlantillasPasosDetalle>();
        }

        public decimal IdPlantilla { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<PlantillasCamposDetalle> PlantillasCamposDetalle { get; set; }
        public virtual ICollection<PlantillasPasosDetalle> PlantillasPasosDetalle { get; set; }
    }
}

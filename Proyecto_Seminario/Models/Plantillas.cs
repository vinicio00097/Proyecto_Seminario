using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PlantillasModle
    {

        public decimal idPlantilla { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public virtual ICollection<PlantillasCamposDetalle> PlantillasCamposDetalle { get; set; }
        public virtual ICollection<PlantillasPasosDetalle> PlantillasPasosDetalle { get; set; }
    }
}

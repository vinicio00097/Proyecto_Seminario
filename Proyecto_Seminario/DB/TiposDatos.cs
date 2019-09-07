using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class TiposDatos
    {
        public TiposDatos()
        {
            InstanciasplantillasDatosDetalle = new HashSet<InstanciasplantillasDatosDetalle>();
            PlantillasCamposDetalle = new HashSet<PlantillasCamposDetalle>();
        }

        public decimal IdTipoDato { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<InstanciasplantillasDatosDetalle> InstanciasplantillasDatosDetalle { get; set; }
        public virtual ICollection<PlantillasCamposDetalle> PlantillasCamposDetalle { get; set; }
    }
}

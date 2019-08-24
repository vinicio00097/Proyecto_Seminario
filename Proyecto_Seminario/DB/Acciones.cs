using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Acciones
    {
        public Acciones()
        {
            InstanciasplantillasPasosDetalle = new HashSet<InstanciasplantillasPasosDetalle>();
        }

        public string Nombre { get; set; }
        public decimal IdAccion { get; set; }

        public virtual ICollection<InstanciasplantillasPasosDetalle> InstanciasplantillasPasosDetalle { get; set; }
    }
}

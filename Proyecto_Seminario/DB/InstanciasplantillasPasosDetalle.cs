using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class InstanciasplantillasPasosDetalle
    {
        public InstanciasplantillasPasosDetalle()
        {
            PasosUsuariosDetalle = new HashSet<PasosUsuariosDetalle>();
        }

        public decimal InstanciaPlantilla { get; set; }
        public decimal Paso { get; set; }
        public decimal? Estado { get; set; }
        public decimal IdPlantillaPasoDetalle { get; set; }

        public virtual Acciones EstadoNavigation { get; set; }
        public virtual Instanciasplantillas InstanciaPlantillaNavigation { get; set; }
        public virtual Pasosinstancias PasoNavigation { get; set; }
        public virtual ICollection<PasosUsuariosDetalle> PasosUsuariosDetalle { get; set; }
    }
}

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
        public decimal? UsuarioAccion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public virtual Acciones EstadoNavigation { get; set; }
        public virtual Instanciasplantillas InstanciaPlantillaNavigation { get; set; }
        public virtual Pasosinstancias PasoNavigation { get; set; }
        public virtual Usuarios UsuarioAccionNavigation { get; set; }
        public virtual ICollection<PasosUsuariosDetalle> PasosUsuariosDetalle { get; set; }
    }
}

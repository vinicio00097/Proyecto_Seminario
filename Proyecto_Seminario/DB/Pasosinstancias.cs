using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Pasosinstancias
    {
        public Pasosinstancias()
        {
            InstanciasplantillasPasosDetalle = new HashSet<InstanciasplantillasPasosDetalle>();
            PasosinstanciasDatosDetalle = new HashSet<PasosinstanciasDatosDetalle>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal IdPasoinstancia { get; set; }
        

        public virtual ICollection<InstanciasplantillasPasosDetalle> InstanciasplantillasPasosDetalle { get; set; }
        public virtual ICollection<PasosinstanciasDatosDetalle> PasosinstanciasDatosDetalle { get; set; }
    }
}

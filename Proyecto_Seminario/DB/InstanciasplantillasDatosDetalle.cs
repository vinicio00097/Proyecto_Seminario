using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class InstanciasplantillasDatosDetalle
    {
        public InstanciasplantillasDatosDetalle()
        {
            PasosinstanciasDatosDetalle = new HashSet<PasosinstanciasDatosDetalle>();
        }

        public decimal Instanciaplantilla { get; set; }
        public string NombreCampo { get; set; }
        public string Dato { get; set; }
        public decimal IdInstanciaPlantillaDato { get; set; }

        public virtual Instanciasplantillas InstanciaplantillaNavigation { get; set; }
        public virtual ICollection<PasosinstanciasDatosDetalle> PasosinstanciasDatosDetalle { get; set; }
    }
}

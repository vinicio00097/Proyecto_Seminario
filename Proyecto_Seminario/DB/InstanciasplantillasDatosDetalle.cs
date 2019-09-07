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
        public string DatoString { get; set; }
        public decimal IdInstanciaPlantillaDato { get; set; }
        public decimal TipoDato { get; set; }
        public decimal? DatoInteger { get; set; }
        public DateTime? DatoDate { get; set; }

        public virtual Instanciasplantillas InstanciaplantillaNavigation { get; set; }
        public virtual TiposDatos TipoDatoNavigation { get; set; }
        public virtual ICollection<PasosinstanciasDatosDetalle> PasosinstanciasDatosDetalle { get; set; }
    }
}

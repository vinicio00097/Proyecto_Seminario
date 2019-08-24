using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PasosinstanciasDatosDetalle
    {
        public decimal InstanciaPlantillaDato { get; set; }
        public decimal Paso { get; set; }
        public string SoloLectura { get; set; }
        public decimal IdPasosinstanciasDatos { get; set; }

        public virtual InstanciasplantillasDatosDetalle InstanciaPlantillaDatoNavigation { get; set; }
        public virtual Pasosinstancias PasoNavigation { get; set; }
    }
}

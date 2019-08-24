using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Instanciasplantillas
    {
        public Instanciasplantillas()
        {
            InstanciasplantillasDatosDetalle = new HashSet<InstanciasplantillasDatosDetalle>();
            InstanciasplantillasPasosDetalle = new HashSet<InstanciasplantillasPasosDetalle>();
        }

        public string Nombre { get; set; }
        public decimal Usuario { get; set; }
        public string Estado { get; set; }
        public decimal IdInstanciaPlantilla { get; set; }

        public virtual Usuarios UsuarioNavigation { get; set; }
        public virtual ICollection<InstanciasplantillasDatosDetalle> InstanciasplantillasDatosDetalle { get; set; }
        public virtual ICollection<InstanciasplantillasPasosDetalle> InstanciasplantillasPasosDetalle { get; set; }
    }
}

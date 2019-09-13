using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PasosinstanciasUsuariosDetalle
    {
        public decimal PlantillaPasoDetalle { get; set; }
        public decimal Usuario { get; set; }
        public decimal IdPasosUsuarios { get; set; }

        public virtual InstanciasplantillasPasosDetalle PlantillaPasoDetalleNavigation { get; set; }
        public virtual Usuarios UsuarioNavigation { get; set; }
    }
}

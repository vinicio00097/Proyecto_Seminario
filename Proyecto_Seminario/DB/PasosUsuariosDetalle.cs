﻿using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class PasosUsuariosDetalle
    {
        public decimal IdPasoUsuario { get; set; }
        public decimal PlantillaPasoDetalle { get; set; }
        public decimal Usuario { get; set; }

        public virtual PlantillasPasosDetalle PlantillaPasoDetalleNavigation { get; set; }
        public virtual Usuarios UsuarioNavigation { get; set; }
    }
}

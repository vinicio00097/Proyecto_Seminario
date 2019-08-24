using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Rangos
    {
        public Rangos()
        {
            Usuarios = new HashSet<Usuarios>();
        }

        public decimal IdRango { get; set; }
        public string Nombre { get; set; }
        public decimal Nivel { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }
    }
}

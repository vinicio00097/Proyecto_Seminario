using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Usuarios
    {
        public Usuarios()
        {
            Instanciasplantillas = new HashSet<Instanciasplantillas>();
            InstanciasplantillasPasosDetalle = new HashSet<InstanciasplantillasPasosDetalle>();
            PasosUsuariosDetalle = new HashSet<PasosUsuariosDetalle>();
        }

        public decimal IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string UsuarioEmail { get; set; }
        public decimal Rango { get; set; }

        public virtual Rangos RangoNavigation { get; set; }
        public virtual ICollection<Instanciasplantillas> Instanciasplantillas { get; set; }
        public virtual ICollection<InstanciasplantillasPasosDetalle> InstanciasplantillasPasosDetalle { get; set; }
        public virtual ICollection<PasosUsuariosDetalle> PasosUsuariosDetalle { get; set; }
    }
}

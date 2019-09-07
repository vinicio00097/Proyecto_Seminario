using Proyecto_Seminario.Models.ModifiedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models
{
    public class InstanciaPaso
    {
        public int IdPasoInstancia { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Dato> Datos { get; set; }
        public List<Usuarios> Usuarios { get; set; }
    }
}

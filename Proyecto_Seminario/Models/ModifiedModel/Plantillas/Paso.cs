using Proyecto_Seminario.Models.ModifiedModel;
using Proyecto_Seminario.Models.ModifiedModel.Plantillas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models
{
    public class Paso
    {
        public int IdPaso{ get; set; }
        public int IdPlantillaPaso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Campo> Datos_Pasos { get; set; }
        public List<Usuario> Usuarios { get; set; }
    }
}

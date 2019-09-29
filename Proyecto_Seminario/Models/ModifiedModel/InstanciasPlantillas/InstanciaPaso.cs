using Proyecto_Seminario.Models.ModifiedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models
{
    public class InstanciaPaso
    {
        public decimal IdPasoInstancia { get; set; }
        public decimal IdPlantillaPasoDetalle { get; set; }
        public decimal InstanciaPlantilla { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? Estado { get; set; }
        public Acciones EstadoNavigation { get; set; }
        public decimal? UsuarioAccion { get; set; }
        public Usuarios UsuarioAccionNavigation { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<Dato> Datos_Pasos { get; set; }
        public List<Usuarios> Usuarios { get; set; }
    }
}

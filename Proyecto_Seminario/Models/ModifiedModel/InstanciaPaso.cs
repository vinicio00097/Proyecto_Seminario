﻿using Proyecto_Seminario.Models.ModifiedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Seminario.Models
{
    public class InstanciaPaso
    {
        public int IdPasoInstancia { get; set; }
        public int IdPlantillaPasoDetalle { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public List<Dato> Datos_Pasos { get; set; }
        public List<Usuarios> Usuarios { get; set; }
    }
}

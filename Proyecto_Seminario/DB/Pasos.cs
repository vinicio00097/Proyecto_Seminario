﻿using System;
using System.Collections.Generic;

namespace Proyecto_Seminario
{
    public partial class Pasos
    {
        public Pasos()
        {
            PasosDatosDetalle = new HashSet<PasosDatosDetalle>();
            PlantillasPasosDetalle = new HashSet<PlantillasPasosDetalle>();
        }

        public decimal IdPaso { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<PasosDatosDetalle> PasosDatosDetalle { get; set; }
        public virtual ICollection<PlantillasPasosDetalle> PlantillasPasosDetalle { get; set; }
    }
}

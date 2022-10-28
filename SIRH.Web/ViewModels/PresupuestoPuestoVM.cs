using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class PresupuestoPuestoVM
    {
        public CPuestoDTO Puesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CAreaDTO Area { get; set; }
        public CActividadDTO Actividad { get; set; }
        public CContenidoPresupuestarioDTO Contenido { get; set; }
    }
}

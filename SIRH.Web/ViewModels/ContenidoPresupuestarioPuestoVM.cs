using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class ContenidoPresupuestarioPuestoVM
    {
        public CContenidoPresupuestarioDTO ContenidoPresupuestario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class EstudioPuestoVM
    {
        public CPuestoDTO Puesto { get; set; }
        public CUbicacionPuestoDTO UbicacionPuesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CEstudioPuestoDTO EstudioPuesto { get; set; }
        public CErrorDTO Error { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
    }
}

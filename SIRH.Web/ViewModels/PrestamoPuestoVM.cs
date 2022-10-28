using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class PrestamoPuestoVM
    {
        public CPrestamoPuestoDTO PrestamoPuesto { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CAddendumPrestamoPuestoDTO AddendumPrestamo { get; set; }

        public SelectList EntidadesAdscritas { get; set; }
        [DisplayName("Entidad Adscrita")]
        public int EntidadAdscritaSeleccionada { get; set; }

        public SelectList EntidadesGubernamentales { get; set; }
        [DisplayName("Entidad Gubernamental")]
        public int EntidadGubernamentalSeleccionada { get; set; }
    }
}

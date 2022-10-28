using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaHistorialCalificacionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        public CCalificacionDTO Calificacion { get; set; }

        public CNombramientoDTO Nombramiento { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CDetallePuestoDTO DetallePuesto { get; set; }
        public List<CCalificacionNombramientoDTO> Calificaciones { get; set; }

        public CCalificacionNombramientoDTO DetalleCNombramiento { get; set; }
        public CDetalleCalificacionNombramientoDTO DetalleCalificacionNombramiento { get; set; }

        public CExpedienteFuncionarioDTO Expediente { get; set; }

        public string CedulaSearch { get; set; }

        public string MensajeTSE { get; set; }

        public string MensajeFuncionario { get; set; }

        public CErrorDTO Error { get; set; }

        public bool EsAdministrador { get; set; }

        public int PeriodoAgregar { get; set; }

        public string NotaAgregar { get; set; }

    }
}

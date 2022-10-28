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
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioRegistroExtrasVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CRegistroTiempoExtraDTO RegistroTiempoExtra { get; set; }
        public CDesgloseSalarialDTO Desglose1 { get; set; }
        public CDesgloseSalarialDTO Desglose2 { get; set; } 
        public CNombramientoDTO Nombramineto { get; set; }
        public List<CDetalleTiempoExtraDTO> DetalleExtras { get; set; }
        public List<CDetalleTiempoExtraDTO> DetalleExtrasGuardados { get; set; }
        public string tituloSaved { get; set; }
        public bool Doble { get; set; }
        public EstadoDetalleExtraEnum? EstadoDetalles { get; set; }
        public string FechaMin { get; set; }
        public string FechaMax { get; set; }
        [DisplayName("Archivo de justificación")]
        public HttpPostedFileBase File { get; set; }
        public SelectList ListaMeses { get; set; }
        public string MesActual { get; set; }

        public string UsuarioEnvia { get; set; }

        public SelectList ListaClases { get; set; }
        public string ClaseActual { get; set; }

        public string MesActualPago { get; set; }

        public SelectList ListaPresupuesto { get; set; }
        public string PresupuestoSeleccionado { get; set; }

        public bool HayArchivo { get; set; }
        public bool YaExiste { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public List<decimal> H0 { get; set; }
        public List<decimal> H1 { get; set; }
        public List<decimal> H2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH0 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH0Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH1 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH1Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalHorasH2Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH0 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH0Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH1 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH1Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalH2Ver { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalPagar { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalPagarVer { get; set; }
        public string Area { get; set; }
        public string Actividad { get; set; }
    }
}

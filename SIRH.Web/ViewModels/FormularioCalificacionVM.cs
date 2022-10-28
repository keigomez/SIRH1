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
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FormularioCalificacionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CCalificacionDTO Calificacion { get; set; }

        public List<CCalificacionNombramientoDTO> CNombramientoB { get; set; }

        public CCalificacionNombramientoDTO CalificacionNombramiento { get; set; }

        public CDetallePuestoDTO DetallePuesto { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CExpedienteFuncionarioDTO Expediente { get; set; }

        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        public CPeriodoCalificacionDTO PeriodoCalificacion { get; set; }

        public List<CCatalogoPreguntaDTO> CatalogoPregunta { get; set; }

        public List<CDetalleCalificacionNombramientoDTO> Detalle { get; set; }

        public List<CCalificacionNombramientoFuncionarioDTO> FuncionariosCalificar { get; set; }

        public SelectList Funcionarios { get; set; }
        public string FuncionarioSeleccionado { get; set; }

        public string Periodos { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; }

        public int CodigoCN { get; set; }

        public int CalificacionFinal { get; set; }

        public decimal PuntuacionFinal { get; set; }
        public string NombreFormulario { get; set; }
        public string CalificacionFinalLetra { get; set; }

        public string CedulaBuscar { get; set; }

        public bool EsAnulable { get; set; }

        public bool EsAdministrador { get; set; }

        public List<CCalificacionReglaTecnicaDTO> ReglasTecnicas { get; set; }

        public HttpPostedFileBase File { get; set; }
        [RegularExpression(@"^.*\.(pdf)$", ErrorMessage = "Extensión de imagen no válida, solo se admite pdf.")]
        public string Imagen { get; set; }

        public CErrorDTO Error { get; set; }

    }
}

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
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioCarreraVM
    {
        public string TituloFieldSet { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CCursoGradoDTO CursoGrado { get; set; }
        public CCursoCapacitacionDTO CursoCapacitacion { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string Imagen { get; set; }

        [DisplayName("Calificación de servicio")]
        public string Calficacion { get; set; }

        public SelectList GradosAcademicos { get; set; }
        [DisplayName("Grado Académico")]
        public int GradoAcademicoSeleccionado { get; set; }

        public SelectList TipoCurso { get; set; }
        [DisplayName("Tipo de Curso")]
        public string CursoSeleccionado { get; set; }

        public SelectList EntidadesEducativas { get; set; }
        [DisplayName("Entidad educativa que emite")]
        public int EntidadEducativaSeleccionada { get; set; }

        public SelectList Modalidades { get; set; }
        [DisplayName("Modalidad")]
        public int ModalidadSeleccionada { get; set; }

        [DisplayName("Horas acumuladas")]
        public int HorasAcumuladas { get; set; }

        [DisplayName("Porcentaje total")]
        public int Porcentaje { get; set; }

        [DisplayName("Puntaje total")]
        public int Puntaje { get; set; }

        [DisplayName("Total a pagar")]
        public decimal TotalAPagar { get; set; }

        public int PuntosEspecializada { get; set; }
        public int PuntosPublicaciones { get; set; }
        public int PuntosLibros { get; set; }

        public int PorcentajeRiesgo { get; set; }
        public int PorcentajeCurso { get; set; }
        public int PorcentajeInstruccionOficial { get; set; }

        public int HorasPartipacion { get; set; }
        public int HorasAprovechamiento { get; set; }
        public int HorasInstruccion { get; set; }

        public int CursoGradoActual { get; set; }

        public CErrorDTO Error { get; set; }
    }
}

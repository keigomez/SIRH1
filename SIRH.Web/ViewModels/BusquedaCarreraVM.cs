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
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class BusquedaCarreraVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CDetalleContratacionDTO Contratacion { get; set; }
        public CCursoGradoDTO CursoGrado { get; set; }
        public CCursoCapacitacionDTO CursoCapacitacion { get; set; }
        public List<BusquedaCarreraVM> Cursos { get; set; }

        public string NombreGrado { get; set; }
        public string Tipo { get; set; }

        [DisplayName("Nombre del título obtenido")]
        public string TituloCurso { get; set; }

        [DisplayName("Total de horas")]
        public int TotalHoras { get; set; }

        [DisplayName("Horas acumuladas")]
        public int HorasAcumuladas { get; set; }    //prop auxiliar

        [DisplayName("Total de puntos")]
        public int TotalPuntos { get; set; } //prop auxiliar

        [DisplayName("Porcentaje total")]
        public int TotalPorcentaje { get; set; } //prop auxiliar

        [DisplayName("Puntos adicionales")]
        public int PuntosAdicionales { get; set; }  //prop auxiliar

        [DisplayName("Porcentaje adicional")]
        public int PorcentajeAdicional { get; set; }    //prop auxiliar

        public SelectList EntidadesEducativas { get; set; }
        [DisplayName("Entidad educativa")]
        public int EntidadEducativaSeleccionada { get; set; }

        public SelectList TipoCurso { get; set; }
        [DisplayName("Tipo de curso")]
        public string CursoSeleccionado { get; set; }

        [DisplayName("Número de resolución")]
        public string NumeroResolucion { get; set; }

        [DisplayName("Desde (fecha emisión)")]
        public DateTime FechaDesde { get; set; }

        [DisplayName("Desde (fecha de ingreso al régimen policial)")]
        public DateTime FechaRegimenDesde { get; set; }

        [DisplayName("Hasta (fecha de ingreso al régimen policial)")]
        public DateTime FechaRegimenHasta { get; set; }

        [DisplayName("Hasta (fecha emisión)")]
        public DateTime FechaHasta { get; set; }

        public SelectList GradosAcademicos { get; set; }
        [DisplayName("Grado académico")]
        public int GradoAcademicoSeleccionado { get; set; }

        public SelectList Modalidades { get; set; }
        [DisplayName("Modalidad")]
        public int ModalidadSeleccionada { get; set; }
    }
}

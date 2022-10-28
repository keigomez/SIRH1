using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;
using System;

namespace SIRH.Web.ViewModels
{
    public class FormularioReporteMarcas
    {
        [DisplayName("Datos del Funcionario")]
        public CFuncionarioDTO Funcionario { get; set; }

        [DisplayName("Fecha de inicio")]
        public DateTime FechaI { get; set; }

        [DisplayName("Fecha de final (opcional)")]
        public DateTime FechaF { get; set; }

        [DisplayName("Mostrar los dias sin marcas")]
        public bool MostarSinMarcas { get; set; }

        [DisplayName("Departamentos")]
        public string DepartamentosSeleccion { get; set; }
        public SelectList Departamentos { get; set; }

        public string TipoReporte { get; set; }
    }
}
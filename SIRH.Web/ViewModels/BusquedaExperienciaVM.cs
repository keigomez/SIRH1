using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class BusquedaExperienciaVM
    {     
        public string  Cedula { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetalleContratacionDTO Detalle { get; set; }
        public CCalificacionDTO Calificacion { get; set; }
        public CPeriodoEscalaSalarialDTO Periodo { get; set; }
        public CDetallePuntosDTO Puntos { get; set; }
        public CErrorDTO Error { get; set; }
    }
}
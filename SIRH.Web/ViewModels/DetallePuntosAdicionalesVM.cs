using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class DetallePuntosAdicionalesVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public int PuntosAdicionales { get; set; }
        
        public int Puntos { get; set; }

        public string Observaciones { get; set; }

        [DisplayName ("Número de documento")]
        public string numDoc { get; set; }
        public CErrorDTO Error { get; set; }
    }
}
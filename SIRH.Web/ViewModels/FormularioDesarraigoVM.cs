using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class FormularioDesarraigoVM
    {
        public CDesarraigoDTO Desarraigo { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionTrabajo { get; set; }
        public List<CFacturaDesarraigoDTO> Facturas { get; set; }
        public List<CContratoArrendamientoDTO> ContratosArrendamiento { get; set; }

        [DisplayName("N° de Carta")]
        public string NumCartaPresentacion { get; set; }
        //[DisplayName("40% Salario Base")]
        //public decimal CaculoSalario { get; set; }

        [DisplayName("Estado del Desarraigo")]
        public string EstadoSeleccion { get; set; }
        public SelectList Estado { get; set; }

        public FormularioFacturaDesarraigoVM GetModelFacturas { get { return new FormularioFacturaDesarraigoVM { CodigoFactura = null }; } }
        public FormularioContratoArrendamientoVM GetModelContratos { get { return new FormularioContratoArrendamientoVM(); } }
    }
}
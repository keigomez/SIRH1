using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FormularioMotivoDeduccionVM
    {
        public CRegistroIncapacidadDTO Incapacida { get; set; }
        public CDetalleDeduccionViaticoCorridoDTO DetalleD { get; set; }
        public CCartaPresentacionDTO Carta { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CViaticoCorridoDTO ViaticoCorrido { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionTrabajo { get; set; }
        public List<CFacturaDesarraigoDTO> Facturas { get; set; }
        public List<CContratoArrendamientoDTO> ContratosArrendamiento { get; set; }
        public List<CViaticoCorridoDTO> ViaticoCorridoList { get; set; }
        [DisplayName("N° de Carta de Presentación")]
        public string NumCartaPresentacion { get; set; }
        //[DisplayName("30% Salario Base")]
        //public decimal CaculoSalario { get; set; }

        [DisplayName("Estado del Viatico Corrido")]
        public string EstadoSeleccion { get; set; }
        public SelectList Estado { get; set; }

       public FormularioFacturaDesarraigoVM GetModelFacturas { get { return new FormularioFacturaDesarraigoVM { CodigoFactura = null }; } }
        public FormularioContratoArrendamientoVM GetModelContratos { get { return new FormularioContratoArrendamientoVM(); } }

        [DisplayName("Incapacidades")]
        public string IncapacidadSeleccion { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccion { get; set; }
        [DisplayName("Provincias")]
        public string ProvinciaSeleccion { get; set; }

        public SelectList Incapacidades { get; set; }
        public SelectList Cantones { get; set; }
        public CDistritoDTO Dist { get; set; }

        public String Formulario { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechInicioViaticoCorridoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechInicioViaticoCorridoF { get; set; }

        public Double TotalMA { get; set; }

    }
}
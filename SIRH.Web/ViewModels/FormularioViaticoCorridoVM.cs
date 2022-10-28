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
    public class FormularioViaticoCorridoVM
    {
        public CErrorDTO Error { get; set; }

        public List<CViaticoCorridoReintegroDTO> ReintegroList { get; set; }
        public List<CMovimientoViaticoCorridoDTO> MovimientoList { get; set; }
        public CMovimientoViaticoCorridoDTO MovimientoViaticoCorrido { get; set; }
        public List<CDetalleDeduccionViaticoCorridoDTO> Deduccion { get; set; }
        public CDetalleEliminacionViaticoCorridoGastoTransporteDTO Eliminacion { get; set; }
        public CDireccionDTO Direccion { get; set; }
        public CRegistroIncapacidadDTO Incapacidad { get; set; }
        public List<CDetalleDeduccionViaticoCorridoDTO> DetalleD { get; set; }
        public CDetalleEliminacionViaticoCorridoGastoTransporteDTO DetalleEliminacion { get; set; }
        public CMovimientoViaticoCorridoDTO MovimientoVC { get; set; }
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

        [DisplayName("Estado del Viático Corrido")]
        public string EstadoSeleccion { get; set; }
        public SelectList Estado { get; set; }

       public FormularioFacturaDesarraigoVM GetModelFacturas { get { return new FormularioFacturaDesarraigoVM { CodigoFactura = null }; } }
        public FormularioContratoArrendamientoVM GetModelContratos { get { return new FormularioContratoArrendamientoVM(); } }

        [DisplayName("Distrito")]
        public string DistritoSeleccion { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccion { get; set; }
        [DisplayName("Provincias")]
        public string ProvinciaSeleccion { get; set; }

        public SelectList Distritos { get; set; }
        public SelectList Cantones { get; set; }
        public SelectList Provincias { get; set; }
        public CDistritoDTO Dist { get; set; }

        public String Formulario { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechInicioViaticoCorridoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechInicioViaticoCorridoF { get; set; }
        [DisplayName("Incapacidades")]
        public string IncapacidadSeleccion { get; set; }
        public SelectList Incapacidades { get; set; }
        //public string mes;

        public string MesSeleccion { get; set; }
        public List<SelectListItem> MesesViatico { get; set; }

        public string Fechalimite;
        public string Mensaje { get; set; }
        public string Cabinas;

        public string ReservaRecurso { get; set; }
        public Double TotalMA { get; set; }
        
        public int diasRebajo { get; set; }
        public int diasReintegro { get; set; }
        public Double TotalRebajo { get; set; }
        public Double TotalReintegro { get; set; }

        //Tipo SelectList para poder ponerlos en el dropdown en la vista (_FormularioAsignaciónGT)
        [DisplayName("Codigos Presupuestarios")]
        public SelectList CodigosPresupuestoList { get; set; }
        [DisplayName("Codigos Presupuestarios")]
        public string PresupuestoSelected { get; set; }

        public int MesPago { get; set; }
        public int AnioPago { get; set; }

        public CCatDiaViaticoGastoDTO CatalogoDia { get; set; }
        public List<CCatDiaViaticoGastoDTO> ListaCatalogoDia { get; set; }
    }
}
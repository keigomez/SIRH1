using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;
using SIRH.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FormularioGastoTransporteVM
    {
        public CErrorDTO Error { get; set; }

        public List<CDetalleAsignacionGastoTransporteDTO> Rutas { get; set; }
        public List<CMovimientoGastoTransporteDTO> MovimeintoList { get; set; }
        public CMovimientoGastoTransporteDTO MovimientoGastoTransporte { get; set; }
        public List<CDetalleDeduccionGastoTransporteDTO> Deduccion { get; set; }
        public CDetalleEliminacionViaticoCorridoGastoTransporteDTO Eliminacion { get; set; }
        public List<CDetalleAsignacionGastoTransporteModificadaDTO> detalleAGT { get; set; }//public List<CDetalleAsignacionGastoTransporteDTO> detalleAGT { get; set; }
        public CDireccionDTO Direccion { get; set; }
        public CRegistroIncapacidadDTO Incapacida { get; set; }
        public List<CDetalleDeduccionGastoTransporteDTO> DetalleD { get; set; }
        public CDetalleEliminacionViaticoCorridoGastoTransporteDTO DetalleEliminacion { get; set; }
        public CMovimientoGastoTransporteDTO MovimientoGT { get; set; }
        public CCartaPresentacionDTO Carta { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CGastoTransporteDTO GastoTransporte { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionTrabajo { get; set; }
        public List<CFacturaDesarraigoDTO> Facturas { get; set; }
        public List<CContratoArrendamientoDTO> ContratosArrendamiento { get; set; }
        /// <summary>
        /// Almacena los gastos de transporte de meses anteriores de un funcionario.
        /// </summary>
        public List<CGastoTransporteDTO> GastoTransporteList { get; set; }

        [DisplayName("N° de Carta de Presentación")]
        public string NumCartaPresentacion { get; set; }
        //[DisplayName("30% Salario Base")]
        //public decimal CaculoSalario { get; set; }

        /// <summary>
        /// Estado seleccionado por el usuario para el GT
        /// </summary>
        [DisplayName("Estado del Gasto de Transporte")]
        public string EstadoSeleccion { get; set; }
        /// <summary>
        /// Lista de estados que se agregarán en el combobox de la vista.
        /// </summary>
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
        public CDistritoDTO Dist { get; set; }

        public String Formulario { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechInicioViaticoCorridoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechInicioViaticoCorridoF { get; set; }
        [DisplayName("Incapacidades")]
        public string IncapacidadSeleccion { get; set; }
        public SelectList Incapacidades { get; set; }
        public string mes;
        public string Fechalimite;
        public string Mensaje;

        //ACM added
        /// <summary>
        /// Mes seleccionado para deducción de GT
        /// </summary>
        public string MesSeleccion { get; set; }
        /// <summary>
        /// Lista de meses durante los que se asignó el gasto de transporte
        /// </summary>
        public List<SelectListItem> MesesGasto { get; set; }
        public Double TotalMA { get; set; }

        public string ReservaRecurso { get; set; }
        public int diasRebajo { get; set; }
        public int diasReintegro { get; set; }
        public Double TotalRebajo { get; set; }
        public Double TotalReintegro { get; set; }

        //Tipo SelectList para poder ponerlos en el dropdown en la vista (_FormularioAsignaciónGT)
        [DisplayName("Codigos Presupuestarios")]
        public SelectList CodigosPresupuestoList { get; set; }
        [DisplayName("Codigos Presupuestarios")]
        public string PresupuestoSelected { get; set; }

        //------------------------------------------------------------------------
        //Para manejo de la lista de rutas obtenidas de ARESEP y las paginas del resultado de búsqueda
        //------------------------------------------------------------------------
        //Lista de rutas cargadas desde el json de aresep
        public List<RutasARESEPModel> RutasARESEP { get; set; }
        public int TotalRutas { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string CodigoSearch { get; set; }
        public string NombreSearch { get; set; }

        public int MesPago { get; set; }
        public int AnioPago { get; set; }
    }
}
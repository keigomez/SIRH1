using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class PedimentoPuestoVM
    {
        public CPedimentoPuestoDTO PedimentoPuesto { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionPuesto { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CErrorDTO Error { get; set; }
        public int Edad { get; set; }
        public CHistorialEstadoCivilDTO EstadoCivil { get; set; }
        public List<CPedimentoPuestoDTO> ListaPedimentos { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }

        [DisplayName("Fecha pedimento desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha pedimento hasta")]
        public DateTime FechaEmisionHasta { get; set; }
    }
}

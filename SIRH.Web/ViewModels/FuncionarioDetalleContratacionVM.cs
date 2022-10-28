using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioDetalleContratacionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CPedimentoPuestoDTO PedimentoPuesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CCuentaBancariaDTO CuentaBancaria { get; set; }
        public CDetalleContratacionDTO DetalleContratación { get; set; }

        public SelectList EntidadesFinancieras { get; set; }
        [DisplayName("Entidad Financiera")]
        public int EntidadSeleccionada { get; set; }

    }
}

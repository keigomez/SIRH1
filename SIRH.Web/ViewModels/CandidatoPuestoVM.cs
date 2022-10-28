using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class CandidatoPuestoVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CPedimentoPuestoDTO PedimentoPuesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CEstadoFuncionarioDTO EstadoFuncionario { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
    }
}

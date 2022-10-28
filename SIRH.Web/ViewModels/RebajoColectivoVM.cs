using SIRH.DTO;
using System.Collections.Generic;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class RebajoColectivoVM
    {
        public CRegistroVacacionesDTO Registro { get; set; }
        [DisplayName("Funcionarios policiales")]
        public string funcionariosPoliciales { get; set; }
        [DisplayName("Funcionarios de seguridad")]
        public string funcionariosSeguridad { get; set; }
        public bool seleccionPolicial { get; set; }
        public bool seleccionSeguridad { get; set; }
        public string numTransaccion { get; set; }

        public List<CFuncionarioDTO> funcionariosInconsistencias { get; set; }
        public List<CRegistroVacacionesDTO> registrosInconsistentes { get; set; }



    }
}
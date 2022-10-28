using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
namespace SIRH.Web.ViewModels
{
    public class FormularioMarcasAsistenciaVM
    {
        //Funcionario
        public CFuncionarioDTO Funcionario { get; set; }

        public List<CFuncionarioDTO> FuncionarioAux { get; set; }

        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CDesgloseSalarialDTO DesgloseSalarial { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        //Error
        public CErrorDTO Error { get; set; }

        //Dispositivos
        public CDispositivoDTO Dispositivo { get; set; }
        public List<CDispositivoDTO> ListaDispositivos { get; set; }

        //Jornada laboral
        public CTipoJornadaDTO JornadaLaboral { get; set; }

        //Empleado
        public CEmpleadoDTO Empleado { get; set; }

        public CDetalleNombramientoDTO DetalleNombramiento { get; set; }

        public SelectList CatalogoMotivoBaja { get; set; }
        [DisplayName("Motivo baja")]
        public string MotivoBajaSeleccionado { get; set; }

        public CMotivoBajaDTO MotivoBaja { get; set; }

    }
}

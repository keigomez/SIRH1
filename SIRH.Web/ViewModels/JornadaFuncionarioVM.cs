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

namespace SIRH.Web.ViewModels
{
    public class JornadaFuncionarioVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CTipoJornadaDTO Jornada { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public string Accion { get; set; }
    }
}

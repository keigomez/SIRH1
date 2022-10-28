using System;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class PerfilBasicoFuncionarioVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CHistorialEstadoCivilDTO EstadoCivil { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CDireccionDTO Direccion { get; set; }
        public int Edad { get; set; }
        public List<CTipoContactoDTO> TiposContacto { get; set; }
        public List<CInformacionContactoDTO> DatosContacto { get; set; }
        public SelectList Distritos { get; set; }
        [DisplayName("Distrito")]
        public string DistritoSeleccionado { get; set; }

        public SelectList Cantones { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccionado { get; set; }

        public SelectList EstadosCiviles { get; set; }

        public SelectList Provincias { get; set; }
        [DisplayName("Provincia")]
        public string ProvinciaSeleccionada { get; set; }

        public SelectList Sexo { get; set; }
        [DisplayName("Sexo")]
        public string SexoSeleccionado { get; set; }

        public string MensajeTSE { get; set; }
        public string MensajePoderJudicial { get; set; }
        public string MensajeOferente { get; set; }

        public CErrorDTO Error { get; set; }
    }
}

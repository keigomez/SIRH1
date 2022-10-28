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
using System.Collections.Generic;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class PerfilUsuarioVM
    {
        public CUsuarioDTO Usuario { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public List<CCatPermisoDTO> Permisos { get; set; }
        public List<CPerfilDTO> Perfiles { get; set; }
        [DisplayName("Permisos")]
        public SelectList Permiso { get; set; }
        [DisplayName("Perfiles")]
        public SelectList Perfil { get; set; }
        
        public string PermisoSeleccionado { get; set; }
        public string PerfilSeleccionado { get; set; }
        [DisplayName("Observación")]
        public string Observacion { get; set; }

        public CCatPermisoDTO PermisoNuevo { get; set; }
        public CPerfilDTO PerfilNuevo { get; set; }

    }
}

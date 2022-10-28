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
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FormularioSalarioEscolarVM
    {
        public CCatalogoRemuneracionDTO CatalogoRemuneracion { get; set; }

        [DisplayName("Número de resolución")]
        [Required(ErrorMessage = "Debe digitar el número de resolución")]
        public string Descripcion { get; set; }
        //Error
        public CErrorDTO Error { get; set; }

    }
}

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
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.Models
{
    public class MovimientoPuestoModel
    {
        public CMovimientoPuestoDTO MovimientoPuesto { get; set; }
        public CFuncionarioDTO Funcionario { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public SelectList MotivosMovimiento { get; set; }

        [DisplayName("Motivo de movimiento")]
        public int MotivoSeleccionado { get; set; }
        [DisplayName("N° Oficio")]
        public string Oficio { get; set; }
    }
}

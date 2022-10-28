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
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class OrdenMovimientoVM
    {
        public COrdenMovimientoDTO Orden { get; set; }

        public COrdenMovimientoDeclaracionDTO Declaracion { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }

        public CPuestoDTO Puesto { get; set; }
       
        //[DisplayName("Cédula")]
        //public string Cedula { get; set; }

        public SelectList Tipos { get; set; }
        [DisplayName("Tipo Movimiento")]
        public string TipoSeleccionado { get; set; }

        public SelectList Motivos { get; set; }
        [DisplayName("Motivo")]
        public string MotivoSeleccionado { get; set; }

       
        [DisplayName("Fecha Rige")]
        public DateTime FechaRige { get; set; }

        [DisplayName("Fecha Vence")]
        public DateTime? FechaVence { get; set; }


        //[DisplayName("Nombre")]
        //public string Nombre { get; set; }

        //[DisplayName("Cuenta Cliente")]
        //public string Cuenta { get; set; }

    
        [DisplayName("Código")]
        public string CodPresupuesto { get; set; }

        //[DisplayName("No. Pedimento")]
        //public string CodPedimento { get; set; }

        [DisplayName("Observaciones Nombramiento")]
        public string ObservacionesNombramiento { get; set; }

        public List<string> Mensaje { get; set; }

        public CErrorDTO Error { get; set; }

        public bool permisoRevisar { get; set; }
        public bool permisoAprobar { get; set; }
        public bool permisoAnular  { get; set; }
        public bool permisoNombramiento{ get; set; }
    }
}
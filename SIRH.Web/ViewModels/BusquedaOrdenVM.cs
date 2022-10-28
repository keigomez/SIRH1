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
    public class BusquedaOrdenVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public COrdenMovimientoDTO Orden { get; set; }
        public List<OrdenMovimientoVM> Ordenes { get; set; }

        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        public SelectList Tipos { get; set; }
        [DisplayName("Tipo Movimiento")]
        public string TipoSeleccionado { get; set; }

        public SelectList Motivos { get; set; }
        [DisplayName("Motivo")]
        public string MotivoSeleccionado { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }

        [DisplayName("Fecha Rige")]
        public DateTime FechaRige { get; set; }

        [DisplayName("Fecha Vence")]
        public DateTime FechaVence { get; set; }
    }
}
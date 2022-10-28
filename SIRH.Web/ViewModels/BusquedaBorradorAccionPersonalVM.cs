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
    public class BusquedaBorradorAccionPersonalVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CBorradorAccionPersonalDTO Borrador { get; set; }
        public CDetalleBorradorAccionPersonalDTO Detalle { get; set; }
        public List<FormularioBorradorAccionPersonalVM> Borradores { get; set; }

        public SelectList Tipos { get; set; }
        [DisplayName("Tipo Accion")]
        public int TipoSeleccionado { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }

        [DisplayName("Fecha Rige")]
        public DateTime FechaRigeDesde { get; set; }

        [DisplayName("Fecha hasta")]
        public DateTime FechaRigeHasta { get; set; }

        [DisplayName("Fecha Vence desde")]
        public DateTime FechaVenceDesde { get; set; }

        [DisplayName("Fecha hasta")]
        public DateTime FechaVenceHasta { get; set; }


        [DisplayName("Fecha Integra")]
        public DateTime FechaRigeIntegraDesde { get; set; }

        [DisplayName("Fecha hasta")]
        public DateTime FechaRigeIntegraHasta { get; set; }

        [DisplayName("Fecha Vence Integra desde")]
        public DateTime FechaVenceIntegraDesde { get; set; }

        [DisplayName("Fecha hasta")]
        public DateTime FechaVenceIntegraHasta { get; set; }

    }
}
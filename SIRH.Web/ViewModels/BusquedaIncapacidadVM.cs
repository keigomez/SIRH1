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
    public class BusquedaIncapacidadVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CRegistroIncapacidadDTO RegistroIncapacidad { get; set; }
        public List<FormularioRegistroIncapacidadVM> Incapacidades { get; set; }

        [DisplayName("Fecha Rige desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha Rige hasta")]
        public DateTime FechaEmisionHasta { get; set; }

        [DisplayName("Fecha Vence desde")]
        public DateTime FechaVenceDesde { get; set; }

        [DisplayName("Fecha Vence hasta")]
        public DateTime FechaVenceHasta { get; set; }

        [DisplayName("Fecha Bitácora desde")]
        public DateTime FechaBitacoraDesde { get; set; }

        [DisplayName("Fecha Bitácora hasta")]
        public DateTime FechaBitacoraHasta { get; set; }

        public SelectList Tipos { get; set; }
        [DisplayName("Tipo Incapacidad")]
        public int TipoSeleccionado { get; set; }

        public SelectList Entidades { get; set; }
        [DisplayName("Entidad que emite")]
        public int EntidadSeleccionada { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }
    }
}
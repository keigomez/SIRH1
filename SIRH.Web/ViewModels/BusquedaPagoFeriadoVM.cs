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
    public class BusquedaPagoFeriadoVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPagoFeriadoDTO PagoFeriado { get; set; }
        public CPagoExtraordinarioDTO PagoExtraordinario { get; set; }

        public List<CPagoFeriadoDTO> PagosFeriado { get; set; }
        public List<CPagoExtraordinarioDTO> PagosExtraordinarios { get; set; }
        public List<CFuncionarioDTO> Funcionarios { get; set; }
        public List<CEstadoTramiteDTO> EstadosTramite { get; set; }

        public CDiaPagadoDTO DiaPagadoAuxiliar { get; set; }
        public CCatalogoDiaDTO CatalogoDiaAuxiliar { get; set; }
        public CEstadoTramiteDTO EstadoTramite { get; set; }
        public CRemuneracionEfectuadaPFDTO SalEscolarEfectuado { get; set; }

        [DisplayName("Consecutivo")]
        public int Consecutivo { get; set; }

        [DisplayName("Fecha del trámite")]
        public DateTime FechaTramite { get; set; }


        public SelectList DiaFeriado { get; set; }
        [DisplayName("Días feriados")]
        public string DiaSeleccionado { get; set; }

        public SelectList DiaAsueto { get; set; }
        [DisplayName("Días de asueto")]
        public string DiaAsuetoSeleccionado { get; set; }

        [DisplayName("Fecha de trámite desde")]
        public DateTime FechaTramiteDesde { get; set; }

        [DisplayName("Fecha de trámite hasta")]
        public DateTime FechaTramitenHasta { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }

    }
}

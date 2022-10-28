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
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.RegistroIncapacidad
{
    public class IncapacidadRptData
    {
        public string Funcionario { get; set; }
        public string Codigo { get; set; }
        public string EntidadMedica { get; set; }
        public string TipoIncapacidad { get; set; }
        public string FechaRige { get; set; }
        public string FechaVencimiento { get; set; }
        public string Estado { get; set; }
        public string NumExpediente { get; set; }
        public string MontoTotalSubsidio { get; set; }
        public string MontoTotalRebaja { get; set; }
        public string MtoSalario { get; set; }


        public string FecRige { get; set; }
        public string PorSubsidio { get; set; }
        public string PorRebaja { get; set; }
        public string MtoSalarioDia { get; set; }
        public string MtoSubsidio { get; set; }
        public string MtoRebaja { get; set; }

        public string Observaciones { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static IncapacidadRptData GenerarDatosReporte(FormularioRegistroIncapacidadVM dato, int idDetalle, string filtros)
        {
            return new IncapacidadRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre.TrimEnd() + " " +
                              dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Codigo = dato.Incapacidad.CodIncapacidad.ToString(),
                EntidadMedica = dato.EntidadMedica.DescripcionEntidadMedica,
                TipoIncapacidad = dato.Incapacidad.TipoIncapacidad.DescripcionTipoIncapacidad,
                FechaRige = dato.Incapacidad.FecRige.ToShortDateString(),
                FechaVencimiento = dato.Incapacidad.FecVence.ToShortDateString(),
                Estado = dato.Incapacidad.DetalleEstadoIncapacidad,
                NumExpediente = dato.Expediente.NumeroExpediente.ToString(),
                MtoSalario = dato.Incapacidad.MtoSalario.ToString("#,#.00#;(#,#.00#)"),
                MontoTotalSubsidio = dato.Incapacidad.MontoTotalSubsidio.ToString("#,#.00#;(#,#.00#)"),
                MontoTotalRebaja = dato.Incapacidad.MontoTotalRebaja.ToString("#,#.00#;(#,#.00#)"),
                Observaciones = dato.Incapacidad.ObsIncapacidad,
                //FecRige = dato.Incapacidad.Detalles[idDetalle].FecRige,
                //PorSubsidio = dato.Incapacidad.Detalles[idDetalle].PorSubsidio.ToString("#,#.00#;(#,#.00#)"),
                //PorRebaja = dato.Incapacidad.Detalles[idDetalle].PorRebaja.ToString("#,#.00#;(#,#.00#)"),
                //MtoSalarioDia = dato.Incapacidad.Detalles[idDetalle].MtoSalarioDia.ToString("#,#.00#;(#,#.00#)"),
                //MtoSubsidio = dato.Incapacidad.Detalles[idDetalle].MtoSubsidio.ToString("#,#.00#;(#,#.00#)"),
                //MtoRebaja = dato.Incapacidad.Detalles[idDetalle].MtoRebaja.ToString("#,#.00#;(#,#.00#)"),
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros
            };
        }
    }
}
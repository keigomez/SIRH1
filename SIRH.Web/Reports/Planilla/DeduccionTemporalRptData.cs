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

namespace SIRH.Web.Reports.Planilla
{
    public class DeduccionTemporalRptData
    {
        public string Funcionario { get; set; }
        public string CodPuesto { get; set; }
        public string NumExpediente { get; set; }
        public string Dias { get; set; }
        public string Horas { get; set; }
        public string NumeroDocumento { get; set; }
        public string Explicacion { get; set; }
        public string FechaRige { get; set; }
        public string FechaVence { get; set; }
        public string MontoDeduccion { get; set; }
        public string MesProceso { get; set; }
        public string Periodo { get; set; }
        public string FechaActualizacion { get; set; }
        public string Estado { get; set; }
        public string TipoDeduccion { get; set; }
        public string FechaBitacora { get; set; }
        public string UsuarioBitacora { get; set; }
        public string MostrarDato { get; set; }

        internal static DeduccionTemporalRptData GenerarDatosReporte(FormularioDeduccionTemporalVM dato, string filtros)
        {
            var estado = "";
            switch (dato.Deduccion.Estado)
            {
                case 0:
                    estado = "Registrada";
                    break;
                case 1:
                    estado = "Aprobada";
                    break;
                case 2:
                    estado = "Anulada";
                    break;
            }

            return new DeduccionTemporalRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " +
                              dato.Funcionario.PrimerApellido.TrimEnd() + " " +
                              dato.Funcionario.SegundoApellido.TrimEnd() + " " +
                              dato.Funcionario.Nombre.TrimEnd() + " " ,
                CodPuesto = dato.Puesto.CodPuesto.TrimEnd(),
                NumExpediente = dato.Expediente != null ? dato.Expediente.NumeroExpediente.ToString() : "",
                Dias = dato.Deduccion.Dias.ToString(),
                Horas = dato.Deduccion.Horas.ToString(),
                NumeroDocumento = dato.Deduccion.NumeroDocumento != null ? dato.Deduccion.NumeroDocumento : "",
                Explicacion = dato.Deduccion.Explicacion,
                FechaRige = dato.Deduccion.FechaRige.ToShortDateString(),
                FechaVence = dato.Deduccion.FechaVence.HasValue ?
                                    dato.Deduccion.FechaVence.Value.Year > 1 ?
                                        dato.Deduccion.FechaVence.Value.ToShortDateString()
                                        : ""
                                    : "",
                MontoDeduccion = dato.Deduccion.MontoDeduccion.ToString("#,#.00#;(#,#.00#)"),
                MesProceso = dato.Deduccion.MesProceso.ToString(),
                Periodo = dato.Deduccion.Periodo != null ? dato.Deduccion.Periodo : "",
                Estado = estado,
                TipoDeduccion = dato.DatoTipoDeduccion.DetalleTipoDeduccionTemporal,
                FechaBitacora = dato.Bitacora != null ? dato.Bitacora.FechaEjecucion.ToString() : "",
                UsuarioBitacora = dato.Bitacora != null ? dato.Bitacora.Usuario.NombreUsuario : "",
                MostrarDato = dato.MostrarDato == true ? "1" : "0"
            };
        }
    }
    
}
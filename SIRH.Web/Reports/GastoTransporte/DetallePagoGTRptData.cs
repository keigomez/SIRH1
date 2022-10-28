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
using System.Collections.Generic;

namespace SIRH.Web.Reports.GastoTransporte
{
    public class DetallePagoGTRptData
    {
        public string montoM { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Provincia { get; set; }
        public string ProvinciaC { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Carta { get; set; }

        public string MontoGasto { get; set; }
        public string MontoTotalRebaja { get; set; }
        public string MontoTotalReintegro { get; set; }
        public string MontoPago { get; set; }

        public string Observaciones { get; set; }

        public string HojaIndividualizada { get; set; }
        public string NumBoleta { get; set; }
        public string FecPago { get; set; }
        public string FecRebajo { get; set; }
        public string MotivoRebajo { get; set; }
        public string MontoRebajo { get; set; }
        public string CodGasto { get; set; }
        public string ReservaRecurso { get; set; }
        public string NumDiasRebajo { get; set; }
        public string NumDiasReintegro { get; set; }

        public string EsReintegro { get; set; }

        internal static DetallePagoGTRptData GenerarDatosReporteGT(FormularioGastoTransporteVM dato, string filtros, int i)
        {
            return new DetallePagoGTRptData
            {
                CedulaFuncionario = dato.Funcionario.Cedula,
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd() + " " + dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),

                Provincia = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia + " - " +
                            dato.UbicacionTrabajo.Distrito.Canton.NomCanton + " - " +
                            dato.UbicacionTrabajo.Distrito.NomDistrito,

                ProvinciaC = dato.UbicacionContrato.Distrito.Canton.Provincia.NomProvincia + " - " +
                             dato.UbicacionContrato.Distrito.Canton.NomCanton + " - " +
                             dato.UbicacionContrato.Distrito.NomDistrito,

                FechaInicio = dato.GastoTransporte.FecInicioDTO.ToShortDateString(),
                FechaFinal = dato.GastoTransporte.FecFinDTO.ToShortDateString(),
                Carta = dato.NumCartaPresentacion != null ? dato.NumCartaPresentacion : "",

                montoM = Convert.ToDecimal(dato.GastoTransporte.MontGastoTransporteDTO).ToString("#,#.00#;(#,#.00#)"),

                MontoGasto = Convert.ToDecimal(dato.GastoTransporte.MontGastoTransporteDTO).ToString("#,#.00#;(#,#.00#)"),
                MontoTotalRebaja = dato.TotalRebajo.ToString("#,#.00#;(#,#.00#)"),
                MontoTotalReintegro = dato.TotalReintegro.ToString("#,#.00#;(#,#.00#)"),
                MontoPago = dato.GastoTransporte.Pagos[0].MonPago.ToString("#,#.00#;(#,#.00#)"),

                Observaciones = dato.GastoTransporte.ObsGastoTransporteDTO,

                FecPago = dato.GastoTransporte.Pagos[0].FecPago.ToShortDateString(),
                HojaIndividualizada = dato.GastoTransporte.Pagos[0].HojaIndividualizada != null ? dato.GastoTransporte.Pagos[0].HojaIndividualizada : "",
                NumBoleta = dato.GastoTransporte.Pagos[0].NumBoleta != null ? dato.GastoTransporte.Pagos[0].NumBoleta : "",
                ReservaRecurso = dato.GastoTransporte.Pagos[0].ReservaRecurso != null ? dato.GastoTransporte.Pagos[0].ReservaRecurso : "",

                NumDiasRebajo = dato.diasRebajo.ToString(),

                FecRebajo = dato.GastoTransporte.Pagos[0].Detalles != null ? dato.GastoTransporte.Pagos[0].Detalles[i].FecDiaPago : "",
                MotivoRebajo = dato.GastoTransporte.Pagos[0].Detalles != null ? dato.GastoTransporte.Pagos[0].Detalles[i].TipoDetalleDTO.DescripcionTipo : "",
                MontoRebajo = dato.GastoTransporte.Pagos[0].Detalles != null ? dato.GastoTransporte.Pagos[0].Detalles[i].MonPago.ToString("#,#.00#;(#,#.00#)") : "",

                NumDiasReintegro = dato.diasReintegro.ToString(),
                CodGasto = dato.GastoTransporte.CodigoGastoTransporte,
                EsReintegro = (dato.GastoTransporte.Pagos[0].Detalles[i].TipoDetalleDTO.IdEntidad == 5) ? "1" :  "0"
            };
        }
    }
}
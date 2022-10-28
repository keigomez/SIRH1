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
using System.Globalization;

namespace SIRH.Web.Reports.ViaticoCorrido
{
    public class PagoMesRptData
    {
        public string MesPago { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string CodigoPresupuestario { get; set; }
        public string MontoViatico { get; set; }
        public string MontoPago { get; set; }
        public string ReservaRecurso { get; set; }
        public string NumBoleta { get; set; }

        public decimal MontoTotal { get; set; }
      
        internal static PagoMesRptData GenerarDatosReporteVC(FormularioViaticoCorridoVM dato, string filtros, int i)
        {
            string mes = "";
            if (dato.MesSeleccion != null)
            {
                var fecha = Convert.ToDateTime(dato.MesSeleccion);
                mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
            }
                

            return new PagoMesRptData
            {
                CedulaFuncionario = dato.ViaticoCorrido.NombramientoDTO.Funcionario.Cedula,
                NombreFuncionario = dato.ViaticoCorrido.NombramientoDTO.Funcionario.PrimerApellido.TrimEnd() + " " + dato.ViaticoCorrido.NombramientoDTO.Funcionario.SegundoApellido.TrimEnd() + " " + dato.ViaticoCorrido.NombramientoDTO.Funcionario.Nombre.TrimEnd(),
                MesPago = mes,
                MontoViatico = Convert.ToDecimal(dato.ViaticoCorrido.MontViaticoCorridoDTO).ToString("#,#.00#;(#,#.00#)"),
                MontoTotal = dato.ViaticoCorrido.Pagos[0].MonPago,
                MontoPago = dato.ViaticoCorrido.Pagos[0].MonPago.ToString("#,#.00#;(#,#.00#)"),
                CodigoPresupuestario = dato.ViaticoCorrido.PresupuestoDTO.CodigoPresupuesto != null ? dato.ViaticoCorrido.PresupuestoDTO.CodigoPresupuesto : "",
                NumBoleta = dato.ViaticoCorrido.Pagos[0].NumBoleta != null ? dato.ViaticoCorrido.Pagos[0].NumBoleta : "",
                ReservaRecurso = dato.ViaticoCorrido.Pagos[0].ReservaRecurso != null ? dato.ViaticoCorrido.Pagos[0].ReservaRecurso : "",
            };
        }

        internal static PagoMesRptData GenerarDatosReporteGT(FormularioGastoTransporteVM dato, string filtros, int i)
        {
            string mes = "";
            if (dato.MesSeleccion != null)
            {
                var fecha = Convert.ToDateTime(dato.MesSeleccion);
                mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
            }


            return new PagoMesRptData
            {
                CedulaFuncionario = dato.GastoTransporte.NombramientoDTO.Funcionario.Cedula,
                NombreFuncionario = dato.GastoTransporte.NombramientoDTO.Funcionario.PrimerApellido.TrimEnd() + " " + dato.GastoTransporte.NombramientoDTO.Funcionario.SegundoApellido.TrimEnd() + " " + dato.GastoTransporte.NombramientoDTO.Funcionario.Nombre.TrimEnd(),
                MesPago = mes,
                MontoViatico = Convert.ToDecimal(dato.GastoTransporte.MontGastoTransporteDTO).ToString("#,#.00#;(#,#.00#)"),
                MontoTotal = dato.GastoTransporte.Pagos[0].MonPago,
                MontoPago = dato.GastoTransporte.Pagos[0].MonPago.ToString("#,#.00#;(#,#.00#)"),
                CodigoPresupuestario = dato.GastoTransporte.PresupuestoDTO.CodigoPresupuesto != null ? dato.GastoTransporte.PresupuestoDTO.CodigoPresupuesto : "",
                NumBoleta = dato.GastoTransporte.Pagos[0].NumBoleta != null ? dato.GastoTransporte.Pagos[0].NumBoleta : "",
                ReservaRecurso = dato.GastoTransporte.Pagos[0].ReservaRecurso != null ? dato.GastoTransporte.Pagos[0].ReservaRecurso : "",
            };
        }
    }
}

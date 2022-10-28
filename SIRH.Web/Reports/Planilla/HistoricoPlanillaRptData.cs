using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.Web.ViewModels;
using SIRH.DTO;

namespace SIRH.Web.Reports.Planilla
{
    public class HistoricoPlanillaRptData
    {
        public string Funcionario { get; set; }
        public string Periodo { get; set; }
        public string FechaPago { get; set; }
        public string Salario { get; set; }
        public string Giro { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }
        public string Total { get; set; }
        public string PromedioQuincena { get; set; }
        public string PromedioDiario { get; set; }

        internal static HistoricoPlanillaRptData GenerarDatosReporte(CHistoricoPlanillaDTO dato, decimal total, decimal promQuincena, decimal promDia, string filtros)
        {
            return new HistoricoPlanillaRptData
            {
                Funcionario = dato.Cedula + " - " + dato.Nombre,
                Periodo = dato.FechaCorrida,
                FechaPago = dato.FechaPeriodo,
                Salario = " ₡ " +
                                Convert.ToDecimal(dato.SalarioQuincenal).ToString("#,#.00#;(#,#.00#)"),
                Giro = "INTEGRA",
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
                Total = " ₡ " + total.ToString("#,#.00#;(#,#.00#)"),
                PromedioQuincena = " ₡ " + promQuincena.ToString("#,#.00#;(#,#.00#)"),
                PromedioDiario = " ₡ " + promDia.ToString("#,#.00#;(#,#.00#)")
            };
        }
    }
}
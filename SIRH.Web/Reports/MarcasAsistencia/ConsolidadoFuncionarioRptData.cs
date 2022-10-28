using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.DTO;

namespace SIRH.Web.Reports.RelojMarcador
{

    public class ConsolidadoFuncionarioRptData
    {
        public string FechaMarca { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string EntradaTarde { get; set; }
        public string SalidaAntes { get; set; }
        public string Laborado { get; set; }
        public string Falta { get; set; }
        public string id { get; set; }

        private static string Faltas(List<CMarcacionesDTO> marcas, DateTime FI, DateTime FF){
            if (FI.Year <= 1)
                return "";
            var minutosTarde = (marcas[0].FechaHoraMarca.TimeOfDay - FI.TimeOfDay).TotalMinutes;
            if (marcas.Count == 1)
                return "AU-M";
            if (minutosTarde > 20)
                return "T-20";
            if (minutosTarde > 5)
                return "T-05";
            return "";
        }

        internal static ConsolidadoFuncionarioRptData GenerarDatosReporteAsistencia(List<CMarcacionesDTO> marcas,DateTime FI,DateTime FF){
            return new ConsolidadoFuncionarioRptData
            {
                FechaMarca = marcas[0].FechaHoraMarca.ToShortDateString() + "  " + marcas[0].FechaHoraMarca.ToString("dddd").ToUpper(),
                Entrada = marcas[0].FechaHoraMarca.ToString("HH:mm"),
                Salida = marcas.Count == 2? marcas[1].FechaHoraMarca.ToString("HH:mm"):"xx:xx",
                EntradaTarde = marcas[0].FechaHoraMarca.TimeOfDay <= FI.TimeOfDay || FI.Year<=1 ? "" : (marcas[0].FechaHoraMarca.TimeOfDay - FI.TimeOfDay).ToString(),
                SalidaAntes = marcas.Count == 2 ? (marcas[1].FechaHoraMarca.TimeOfDay >= FF.TimeOfDay ? "" : (FF.TimeOfDay - marcas[1].FechaHoraMarca.TimeOfDay).ToString()) : "xx:xx:xx",
                Laborado = marcas.Count == 2 ? (marcas[1].FechaHoraMarca.TimeOfDay - marcas[0].FechaHoraMarca.TimeOfDay).ToString():"xx:xx:xx",
                Falta = Faltas(marcas,FI,FF),
                id=""
            };
        }

        internal static ConsolidadoFuncionarioRptData GenerarDatosReporteAsistenciaSinMarca(DateTime fecha, bool sinMarca){
            var day = fecha.ToString("dddd");
            if (day == "domingo" || day == "sábado")
                return new ConsolidadoFuncionarioRptData
                {
                    FechaMarca = sinMarca == true ? fecha.ToShortDateString() + "  " + day.ToUpper() : "No hay marcas",
                    id=""
                 };
            else{
                string marcaN = fecha.Date > DateTime.Now.Date ? "*" : "";
                return new ConsolidadoFuncionarioRptData
                {
                    FechaMarca = sinMarca == true ? fecha.ToShortDateString() + "  " + day.ToUpper() : "No hay marcas",
                    Entrada = "xx:xx" + marcaN,
                    Salida = "xx:xx" + marcaN,
                    EntradaTarde = "xx:xx" + marcaN,
                    SalidaAntes = "xx:xx:xx" + marcaN,
                    Laborado = "xx:xx:xx" + marcaN,
                    Falta = marcaN == "*"?"":"AU-T",
                    id=""
                };
            }
        }
    }
}
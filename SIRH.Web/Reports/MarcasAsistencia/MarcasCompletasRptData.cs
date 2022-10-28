using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.DTO;

namespace SIRH.Web.Reports.RelojMarcador
{

    public class MarcasCompletasRptData
    {
        public string FechaMarca { get; set; }
        public string HoraMarca { get; set; }

        internal static MarcasCompletasRptData GenerarDatosReporteAsistencia(List<CMarcacionesDTO> marcas)
        {
            return new MarcasCompletasRptData
            {
                FechaMarca = marcas[0].FechaHoraMarca.ToShortDateString() + "  " + marcas[0].FechaHoraMarca.ToString("dddd").ToUpper(),
                HoraMarca = string.Join("\t\t\t\t\t", marcas.Select(M => M.FechaHoraMarca.TimeOfDay.ToString()).ToArray())
            };
        }

        internal static MarcasCompletasRptData GenerarDatosReporteAsistenciaSinMarca(DateTime fecha, bool sinMarca)
        {
            var day = fecha.ToString("dddd");
            return new MarcasCompletasRptData
            {
                FechaMarca = sinMarca == true ? (fecha.ToShortDateString() + "  " + day.ToUpper()) : "No hay marcas",
                HoraMarca = (day == "domingo" || day == "sábado")?"":new String('+', 13).Replace("+", "xx:xx:xx\t\t\t\t\t")
            };
        }
    }
}
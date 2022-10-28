using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class InconsistenciaRpt
    {
        public string numTransaccion { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string cantidadDias { get; set; }


        internal static InconsistenciaRpt GenerarDatosReporte(CRegistroVacacionesDTO registroVacaciones)
        {
            return new InconsistenciaRpt
            {
                numTransaccion = registroVacaciones.NumeroTransaccion,
                fechaInicio = registroVacaciones.FechaRige.ToShortDateString(),
                fechaFin = registroVacaciones.FechaVence.ToShortDateString(),
                cantidadDias=registroVacaciones.Dias.ToString()
            };
        }
    }
}
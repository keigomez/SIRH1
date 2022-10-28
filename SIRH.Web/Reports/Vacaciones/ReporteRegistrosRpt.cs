using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class ReporteRegistrosRpt
    {
        public string cedulaFuncionario { get; set; }
        public string nombreFuncionario { get; set; }
        public string periodoVacaciones { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string dias { get; set; }
        public string numeroDocumento { get; set; }
        public string estado { get; set; }

        internal static ReporteRegistrosRpt GenerarDatosReporte(CRegistroVacacionesDTO registroVacaciones, CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodo)
        {
            return new ReporteRegistrosRpt
            {
                cedulaFuncionario = funcionario.Cedula,
                nombreFuncionario = funcionario.Nombre + funcionario.PrimerApellido + funcionario.SegundoApellido,
                periodoVacaciones = periodo.Periodo,
                fechaInicio = registroVacaciones.FechaRige.ToShortDateString(),

                fechaFin = registroVacaciones.FechaVence.ToShortDateString(),
                dias = Convert.ToString(registroVacaciones.Dias),

                numeroDocumento = registroVacaciones.NumeroTransaccion,
                estado = Convert.ToString(registroVacaciones.Estado)
            };
        }
    }
}
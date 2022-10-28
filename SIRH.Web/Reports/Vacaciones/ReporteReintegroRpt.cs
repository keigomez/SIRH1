using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class ReporteReintegroRpt
    {
        public string cedulaFuncionario { get; set; }
        public string nombreFuncionario { get; set; }
        public string periodoVacaciones { get; set; }
        public string fechaRige { get; set; }
        public string obs { get; set; }
        public string numeroDocumento { get; set; }
        public string dias { get; set; }

        internal static ReporteReintegroRpt GenerarDatosReporte(CReintegroVacacionesDTO registroVacaciones, CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodo)
        {
            return new ReporteReintegroRpt
            {
                cedulaFuncionario = funcionario.Cedula,
                nombreFuncionario = funcionario.Nombre + funcionario.PrimerApellido + funcionario.SegundoApellido,
                periodoVacaciones = periodo.Periodo,
                fechaRige = Convert.ToDateTime(registroVacaciones.FechaRige).ToShortDateString(),
                obs = registroVacaciones.Observaciones,
                numeroDocumento = registroVacaciones.SolReintegro,
                dias = Convert.ToString(registroVacaciones.CantidadDias)
            };
        }
    }
}
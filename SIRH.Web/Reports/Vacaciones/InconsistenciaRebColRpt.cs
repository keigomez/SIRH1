using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class InconsistenciaRebColRpt
    {
        public string cedulaFuncionario { get; set; }
        public string nombreFuncionario { get; set; }
        public string registroVacaciones { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }


        internal static InconsistenciaRebColRpt GenerarDatosReporte(CRegistroVacacionesDTO registroVacaciones, CFuncionarioDTO funcionario)
        {
            return new InconsistenciaRebColRpt
            {
                cedulaFuncionario = funcionario.Cedula,
                nombreFuncionario = funcionario.Nombre + funcionario.PrimerApellido + funcionario.SegundoApellido,
                registroVacaciones = registroVacaciones.NumeroTransaccion,
                fechaInicio = registroVacaciones.FechaRige.ToShortDateString(),
                fechaFin = registroVacaciones.FechaVence.ToShortDateString()
            };
        }
    }
}
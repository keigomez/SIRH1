using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class DatosFuncionarioRpt
    {
        public string cedulaFuncionario { get; set; }
        public string nombreFuncionario { get; set; }

        public string fecVacaciones { get; set; }

        internal static DatosFuncionarioRpt GenerarDatosReporte(CFuncionarioDTO funcionario)
        {
            return new DatosFuncionarioRpt
            {
                cedulaFuncionario = funcionario.Cedula,
                nombreFuncionario = funcionario.Nombre + funcionario.PrimerApellido + funcionario.SegundoApellido,
                fecVacaciones = funcionario.Mensaje
            };
        }
    }
}

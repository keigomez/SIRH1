using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.DTO;

namespace SIRH.Web.Reports.RelojMarcador
{

    public class FuncionarioPorDepartamentoRptData
    {
        public string Funcionario { get; set; }
        public string Detalle { get; set; }
        public string Departamento { get; set; }
        public string Codigo { get; set; }
        public string Autor { get; set; }


        internal static FuncionarioPorDepartamentoRptData GenerarDatosReporteAsistencia(CFuncionarioDTO fun, CDetalleContratacionDTO detalle, CUbicacionAdministrativaDTO ubicacion)
        {
            return new FuncionarioPorDepartamentoRptData
            {
                Funcionario = fun.Cedula + " - " + fun.Nombre + " " + fun.PrimerApellido + " " + fun.SegundoApellido,
                Codigo = fun.Mensaje == null ? "INDEF" : fun.Mensaje,
                Detalle =  detalle.EstadoMarcacion == true? "Si":(detalle.EstadoMarcacion == false)?"No":"INDEF",
                Departamento = ubicacion.Departamento != null ? ubicacion.Departamento.NomDepartamento : "INDEF",
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
            };
        }
    }
}
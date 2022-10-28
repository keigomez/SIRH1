using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using SIRH.Web.Helpers;

namespace SIRH.Web.Reports.Funcionarios
{

    public class ExFuncionariosRptData
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaCese { get; set; }
        public string FechaNacimiento { get; set; }
        public string Puesto { get; set; }
        public string Clase { get; set; }
        public string Especialidad { get; set; }
        public string OcupacionReal { get; set; }
        public string CodPresupuestario { get; set; }
        public string CodPolicial { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Seccion { get; set; }
        //********************************************Reporte Funcionario*********************************************
        public string Genero { get; set; }
        public string EstadoFuncionario { get; set; }
        
        public string UltimoPeriodo { get; set; }
        public string NumeroExpediente { get; set; }

        internal static ExFuncionariosRptData GenerarDatosReporteExFuncionario(CEMUExfuncionarioDTO funcionario)
        {
            
            return new ExFuncionariosRptData
            {
                Cedula = funcionario.Cedula,
                Nombre = funcionario.Nombre.TrimEnd() + " " + funcionario.PrimerApellido.TrimEnd() + " " + funcionario.SegundoApellido.TrimEnd(),
                FechaIngreso = funcionario.FechaIngreso != null ? Convert.ToDateTime(funcionario.FechaIngreso).ToShortDateString() : "",
                FechaCese = funcionario.FechaCese != null ? Convert.ToDateTime(funcionario.FechaCese).ToShortDateString() : "",
                FechaNacimiento = funcionario.FechaCumple != null ? Convert.ToDateTime(funcionario.FechaCumple).ToShortDateString() : "",
                Puesto = funcionario.PuestoPropiedad,
                Clase = funcionario.ClasePuesto,
                CodPresupuestario = funcionario.Programa,
                CodPolicial = funcionario.CodInspectores,
                Division = funcionario.Division,
                Direccion = funcionario.Direccion,
                Departamento = funcionario.Departamento,
                Seccion=funcionario.Seccion,
                Genero = funcionario.Sexo,
                EstadoFuncionario = funcionario.DescEstado,
                NumeroExpediente = funcionario.NumeroExpediente,
                UltimoPeriodo = funcionario.UltimoPeriodo
            };
        }


    }
}
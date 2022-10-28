using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Carrera
{
    public class FuncionarioPolicialRPTData
    {
        public string Funcionario { get; set; }

        public string FechaIngreso { get; set; }

        public string  FechaIngresoRegimen { get; set; }

        public string CodPolicial { get; set; }

        public string CodPuesto { get; set; }

        public string Clase { get; set; }

        public string Especialidad { get; set; }

        public string Autor { get; set; }

        public string Filtros { get; set; }

        internal static FuncionarioPolicialRPTData GenerarDatosReporte(BusquedaFuncionarioPolicialVM dato, string filtros)
        {
            return new FuncionarioPolicialRPTData
            {

                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " + dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                FechaIngreso = dato.DetalleContratacion.FechaIngreso.ToShortDateString(),
                FechaIngresoRegimen = dato.DetalleContratacion.FechaRegimenPolicial.ToShortDateString(),
                CodPolicial = dato.DetalleContratacion.CodigoPolicial.ToString(),
                Clase = dato.DetallePuesto.Clase.DesClase,
                Especialidad = dato.DetallePuesto.Especialidad.DesEspecialidad,

                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros

            };
        }
    }

}
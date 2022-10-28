using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Carrera
{
    public class CursoCapacitacionRptData
    {
        static WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        public string Funcionario { get; set; }
        public string TituloObtenido { get; set; }
        public string TotalHoras { get; set; }
        public string Incentivo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Modalidad { get; set; }
        public string EntidadEducativa { get; set; }
        public string NumResolucion { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static CursoCapacitacionRptData GenerarDatosReporte(BusquedaCarreraVM dato, string filtros)
        {
            return new CursoCapacitacionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                TituloObtenido = dato.CursoCapacitacion.DescripcionCapacitacion,
                Incentivo = dato.CursoCapacitacion.TotalPuntos.ToString(),
                Modalidad = dato.CursoCapacitacion.Modalidad.Descripcion,
                FechaInicio = dato.CursoCapacitacion.FechaInicio.ToShortDateString(),
                FechaFinal = dato.CursoCapacitacion.FechaFinal.ToShortDateString(),
                NumResolucion = dato.CursoCapacitacion.Resolucion,
                TotalHoras = dato.CursoCapacitacion.TotalHoras.ToString(),
                EntidadEducativa = dato.CursoCapacitacion.EntidadEducativa.DescripcionEntidad,
                Autor = principal.Identity.Name,
                Filtros = filtros
            };
        }

        internal static CursoCapacitacionRptData GenerarDatosReporteIndividual(FuncionarioCarreraVM dato, string filtros)
        {
            return new CursoCapacitacionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                TituloObtenido = dato.CursoCapacitacion.DescripcionCapacitacion,
                Incentivo = dato.CursoCapacitacion.TotalPuntos.ToString(),
                Modalidad = dato.CursoCapacitacion.Modalidad.Descripcion,
                FechaInicio = dato.CursoCapacitacion.FechaInicio.ToShortDateString(),
                FechaFinal = dato.CursoCapacitacion.FechaFinal.ToShortDateString(),
                NumResolucion = dato.CursoCapacitacion.Resolucion,
                TotalHoras = dato.CursoCapacitacion.TotalHoras.ToString(),
                EntidadEducativa = dato.CursoCapacitacion.EntidadEducativa.DescripcionEntidad,
                Autor = principal.Identity.Name,
                Filtros = filtros
            };
        }
    }
}

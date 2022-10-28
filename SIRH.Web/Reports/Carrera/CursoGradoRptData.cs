using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Carrera
{
    public class CursoGradoRptData
    {
        static WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        public string Funcionario { get; set; }
        public string TituloObtenido { get; set; }
        public string Incentivo { get; set; }
        public string TipoGrado { get; set; }
        public string FechaEmision { get; set; }
        public string NumResolucion { get; set; }
        public string EntidadEducativa { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static CursoGradoRptData GenerarDatosReporte(BusquedaCarreraVM dato, string filtros)
        {
            return new CursoGradoRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                TituloObtenido = dato.CursoGrado.CursoGrado,
                Incentivo = dato.CursoGrado.Incentivo.ToString(),
                TipoGrado = dato.NombreGrado,
                FechaEmision = dato.CursoGrado.FechaEmision.ToShortDateString(),
                NumResolucion = dato.CursoGrado.Resolucion,
                EntidadEducativa = dato.CursoGrado.EntidadEducativa.DescripcionEntidad,
                Autor = principal.Identity.Name,
                Filtros = filtros
            };
        }

        internal static CursoGradoRptData GenerarDatosReporteIndividual(FuncionarioCarreraVM dato, string filtros)
        {
            return new CursoGradoRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                TituloObtenido = dato.CursoGrado.CursoGrado,
                Incentivo = dato.CursoGrado.Incentivo.ToString(),
                TipoGrado = dato.CursoSeleccionado.ToString(),
                FechaEmision = dato.CursoGrado.FechaEmision.ToShortDateString(),
                NumResolucion = dato.CursoGrado.Resolucion,
                EntidadEducativa = dato.CursoGrado.EntidadEducativa.DescripcionEntidad,
                Autor = principal.Identity.Name,
                Filtros = filtros
            };
        }
    }
}

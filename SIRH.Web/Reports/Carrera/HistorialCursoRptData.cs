using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Carrera
{
    public class HistorialCursoRptData
    {
        public string Funcionario { get; set; }

        public string NombreCurso { get; set; }

        public decimal TotalHoras { get; set; }

        public string TipoCurso { get; set; }

        public string ImpartidoEn { get; set; }

        public string FecRige { get; set; }

        public string FecFinal { get; set; }

        public string Resolucion { get; set; }
       
        public string Autor { get; set; }

        public string Filtros { get; set; }


        internal static HistorialCursoRptData GenerarDatosReporte(BusquedaHistoricoCursosVM dato, string filtros)
        {
            return new HistorialCursoRptData
            {
                Funcionario = dato.Curso.Cedula + " - " + dato.Curso.Nombre,
                NombreCurso = dato.Curso.NombreCurso,
                TipoCurso = dato.Curso.TipoCurso,
                TotalHoras = dato.Curso.TotalHoras,
                ImpartidoEn = dato.Curso.ImpartidoEn,
                FecRige = dato.Curso.FecRige.ToShortDateString(),
                FecFinal = dato.Curso.FecFinal.ToShortDateString(),    
                Resolucion = dato.Curso.Resolucion,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros
            };
        }
    }

}
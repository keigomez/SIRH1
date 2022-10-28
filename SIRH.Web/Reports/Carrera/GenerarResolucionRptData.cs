using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Carrera
{
    public class GenerarResolucionRptData
    {
        public string Fecha { get; set; }
        public string NumResolucion { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string FechaRige { get; set; }
        public string FechaVence { get; set; }
        public decimal PuntosActuales { get; set; }
        public decimal PuntosAdicionales { get; set; }
        public decimal PuntosTotales { get; set; }
        public decimal MontoPagar { get; set; }
        public decimal MontoPunto { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal SalarioBase { get; set; }
        public string Autor { get; set; }


        internal static GenerarResolucionRptData GenerarDatosReporte(FuncionarioResolucionVM model)
        {
             DateTime f = DateTime.Now;

            GenerarResolucionRptData registro = new GenerarResolucionRptData();

            registro.Fecha = "a las " + f.Hour.ToString() + ":" + f.ToString("mm") + " del día " + f.Day.ToString() + " de " + f.ToString("MMMM") + " del " + f.Year.ToString() + ".";
            registro.NumResolucion = model.NumResolucion;
            registro.Nombre = model.Funcionario.PrimerApellido.TrimEnd() + " " + model.Funcionario.SegundoApellido.TrimEnd() + " " + model.Funcionario.Nombre.TrimEnd();
            registro.Cedula = model.Funcionario.Cedula;
            registro.FechaRige = model.FecRige.Year > 1 ? model.FecRige.ToShortDateString() : "SIN ESPECIFICAR";
            registro.FechaVence = model.FecVence.Year > 1 ? model.FecVence.ToShortDateString() : "SIN ESPECIFICAR";
            registro.PuntosTotales = model.PuntosTotales;
            registro.PuntosAdicionales = model.PuntosAdicionales;
            registro.PuntosActuales = model.PuntosActuales;
            registro.MontoPagar = model.MontoPagar;
            registro.SalarioBase = model.SalarioBase;
            registro.MontoPunto = model.ValorPunto;
            registro.MontoTotal = model.MontoTotal;

            registro.Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            return registro;

        }
    }
}
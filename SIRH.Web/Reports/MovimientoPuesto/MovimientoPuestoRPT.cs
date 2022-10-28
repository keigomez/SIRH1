using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.MovimientoPuesto
{
    public class MovimientoPuestoRPT
    {
        public string NumPuesto { get; set; }
        public string Clase { get; set; }
        public string Especialidad { get; set; }
        public string MotivoMovimiento { get; set; }
        public string FechaMovimiento { get; set; }
        public string FechaVencimiento { get; set; }
        public string Usuario { get; set; }

        internal static MovimientoPuestoRPT GenerarReporteMovimientoPuesto(MovimientoPuestoVM mv)
        {
            return new MovimientoPuestoRPT
            {
                NumPuesto = mv.Puesto.CodPuesto,
                Clase = mv.Puesto.DetallePuesto.Clase.DesClase,
                Especialidad = mv.Puesto.DetallePuesto.Especialidad.DesEspecialidad,
                MotivoMovimiento = mv.MotivoMovimiento.DesMotivo,
                FechaMovimiento = Convert.ToDateTime(mv.FechaMovimiento).ToShortDateString(),
                FechaVencimiento = Convert.ToDateTime(mv.FechaVencimiento).Year > 1 ? Convert.ToDateTime(mv.FechaVencimiento).ToShortDateString() : "",
                Usuario = System.Security.Principal.WindowsIdentity.GetCurrent().Name
            };
        }
    }
}
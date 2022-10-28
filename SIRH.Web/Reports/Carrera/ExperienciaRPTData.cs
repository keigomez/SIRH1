using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Carrera
{
    public class ExperienciaRPTData
    {

        public decimal TotalPuntos { get; set; }
        public string Puesto { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public decimal ValorPunto { get; set; }

        public decimal HorasAprovechamiento { get; set; }
        public decimal HorasExcAprovechamiento { get; set; }
        public decimal PuntosAprovechamiento { get; set; }
        public int CursosAprovechamiento { get; set; }

        public decimal HorasAprovechamientoLey { get; set; }
        public decimal HorasExcAprovechamientoLey { get; set; }
        public decimal PuntosAprovechamientoLey { get; set; }
        public int CursosAprovechamientoLey { get; set; }

        public decimal HorasParticipacion { get; set; }
        public decimal HorasExcParticipacion { get; set; }
        public decimal PuntosParticipacion { get; set; }
        public int CursosParticipacion { get; set; }

        public decimal HorasParticipacionLey { get; set; }
        public decimal HorasExcParticipacionLey { get; set; }
        public decimal PuntosParticipacionLey { get; set; }
        public int CursosParticipacionLey { get; set; }

        public decimal HorasInstruccion { get; set; }
        public decimal PuntosInstruccion { get; set; }
        public int CursosInstruccion { get; set; }

        public decimal PuntosGrado { get; set; }
        public string Grado { get; set; }

        public decimal PuntosAdicionales { get; set; }

        public string Calificacion { get; set; }
        public string Autor { get; set; }


        internal static ExperienciaRPTData GenerarDatosReporte(BusquedaExperienciaVM model)
        {
            return new ExperienciaRPTData {

                TotalPuntos = model.Puntos.TotalPuntos,
                Puesto = model.Puesto.CodPuesto,
                Nombre = model.Funcionario.PrimerApellido.TrimEnd() + " " + model.Funcionario.SegundoApellido.TrimEnd() + " " + model.Funcionario.Nombre.TrimEnd(),
                Cedula = model.Funcionario.Cedula,
                ValorPunto = model.Periodo.MontoPuntoCarrera,

                HorasAprovechamiento = model.Puntos.HorasAprovechamiento,
                HorasExcAprovechamiento = model.Puntos.HorasExcAprovechamiento,
                PuntosAprovechamiento = model.Puntos.PuntosAprovechamiento,
                CursosAprovechamiento = model.Puntos.CursosAprovechamiento,

                HorasAprovechamientoLey = model.Puntos.HorasAprovechamientoLey,
                HorasExcAprovechamientoLey = model.Puntos.HorasExcAprovechamientoLey,
                PuntosAprovechamientoLey = model.Puntos.PuntosAprovechamientoLey,
                CursosAprovechamientoLey = model.Puntos.CursosAprovechamientoLey,

                HorasParticipacion = model.Puntos.HorasParticipacion,
                HorasExcParticipacion = model.Puntos.HorasExcParticipacion,
                PuntosParticipacion = model.Puntos.PuntosParticipacion,
                CursosParticipacion = model.Puntos.CursosParticipacion,

                HorasParticipacionLey = model.Puntos.HorasParticipacionLey,
                HorasExcParticipacionLey = model.Puntos.HorasExcParticipacionLey,
                PuntosParticipacionLey = model.Puntos.PuntosParticipacionLey,
                CursosParticipacionLey = model.Puntos.CursosParticipacionLey,

                HorasInstruccion = model.Puntos.HorasInstruccion,
                PuntosInstruccion = model.Puntos.PuntosInstruccion,
                CursosInstruccion = model.Puntos.CursosInstruccion,

                PuntosGrado = model.Puntos.PuntosGrado,
                Grado = model.Puntos.Grado,
                PuntosAdicionales = model.Puntos.PuntosAdicionales,
                Calificacion = model.Calificacion.DesCalificacion,

                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name

            };

        }
    }
}
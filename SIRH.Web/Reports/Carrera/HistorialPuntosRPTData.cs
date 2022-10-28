using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Carrera
{
    public class HistorialPuntosRPTData
    {
        public string Funcionario { get; set; }
        public decimal TotalPuntos { get; set; }
        public int ValorPunto { get; set; }
        public int AnnoExpPR { get; set; }
        public int Aprovechamiento { get; set; }
        public int Participacion { get; set; }
        public int ParticipacionInstruccion { get; set; }
        public string CalificacionServicio { get; set; }
        public decimal HorasAprovechamiento { get; set; }
        public decimal HorasParticipacion { get; set; }
        public decimal HorasInstruccion { get; set; }
        public decimal PtosGrado { get; set; }
        public decimal PtosAprovechamiento { get; set; }
        public decimal PtosExpPR { get; set; }
        public decimal PtosParticipacion { get; set; }
        public decimal PtosInstruccion { get; set; }
        public decimal PtosOtros { get; set; }
        public string ExplOtros { get; set; }
        public string Observacion { get; set; }
        public int MesCarrera { get; set; }
        public int AnnoCarrera { get; set; }
        public string FecRigePago { get; set; }
        public decimal HsExcPar { get; set; }
        public decimal HsExcApr { get; set; }
        public string NumResolucion { get; set; }
        public string FecResolucion { get; set; }
        public string Periodo { get; set; }
        public string Grado { get; set; }
        public string Nivel { get; set; }
        public decimal AnnoExpEst { get; set; }
        public decimal AprovechamientoEst { get; set; }
        public decimal ParticipacionEst { get; set; }
        public decimal HoraAprEst { get; set; }
        public decimal HoraParEst { get; set; }
        public decimal PtosGrEst { get; set; }
        public decimal PtosAprEst { get; set; }
        public decimal PtosExpEst { get; set; }
        public decimal PtosParEst { get; set; }
        public decimal PtosOtrosEst { get; set; }
        public decimal ExcParEst { get; set; }
        public decimal ExcAprEst { get; set; }
        public decimal CAprovechamiento { get; set; }
        public decimal CParticipacion { get; set; }
        public decimal CInstruccion { get; set; }
        public decimal ExpDoc { get; set; }
        public string Movimiento { get; set; }
        public string Observacion1 { get; set; }
        public string Observacion2 { get; set; }
        public string Observacion3 { get; set; }
        public string Fecha { get; set; }
        public decimal PtosPublicacion { get; set; }
        public decimal PtosExpDoc { get; set; }
        public decimal PtosOrgInt { get; set; }
        public string RigeEst { get; set; }
        public string CalificacionEst { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static HistorialPuntosRPTData GenerarDatosReporte(BusquedaHistoricoPuntosVM dato, string filtros) 
        {
            HistorialPuntosRPTData obj = new HistorialPuntosRPTData
            {
                Funcionario = dato.Puntos.Cedula + " - " + dato.Puntos.Nombre,
                TotalPuntos = dato.Puntos.TotalPuntos,
                ValorPunto = dato.Puntos.ValorPunto,
                AnnoExpPR = dato.Puntos.AnnoExpPR,
                Aprovechamiento = dato.Puntos.Aprovechamiento,
                Participacion = dato.Puntos.Participacion,
                ParticipacionInstruccion = dato.Puntos.ParticipacionInstruccion,
                CalificacionServicio = dato.Puntos.CalificacionServicio,
                HorasAprovechamiento = dato.Puntos.HorasAprovechamiento,
                HorasParticipacion = dato.Puntos.HorasParticipacion,
                HorasInstruccion = dato.Puntos.HorasInstruccion,
                PtosGrado = dato.Puntos.PtosGrado,
                PtosAprovechamiento = dato.Puntos.PtosAprovechamiento,
                PtosExpPR = dato.Puntos.PtosExpPR,
                PtosParticipacion = dato.Puntos.PtosParticipacion,
                PtosInstruccion = dato.Puntos.PtosInstruccion,
                PtosOtros = dato.Puntos.PtosOtros,
                ExplOtros = dato.Puntos.ExplOtros,
                Observacion = dato.Puntos.Observacion,
                MesCarrera = dato.Puntos.MesCarrera,
                AnnoCarrera = dato.Puntos.AnnoCarrera,
                FecRigePago = dato.Puntos.FecRigePago.ToShortDateString(),
                HsExcPar = dato.Puntos.HsExcPar,
                HsExcApr = dato.Puntos.HsExcApr,
                NumResolucion = dato.Puntos.NumResolucion,
                FecResolucion = dato.Puntos.FecResolucion.ToShortDateString(),
                Periodo = dato.Puntos.Periodo,
                Grado = dato.Puntos.Grado,
                Nivel = dato.Puntos.Nivel,
                AnnoExpEst = dato.Puntos.AnnoExpEst,
                AprovechamientoEst = dato.Puntos.AprovechamientoEst,
                ParticipacionEst = dato.Puntos.ParticipacionEst,
                HoraAprEst = dato.Puntos.HoraAprEst,
                HoraParEst = dato.Puntos.HoraParEst,
                PtosGrEst = dato.Puntos.PtosGrEst,
                PtosAprEst = dato.Puntos.PtosAprEst,
                PtosExpEst = dato.Puntos.PtosExpEst,
                PtosParEst = dato.Puntos.PtosParEst,
                PtosOtrosEst = dato.Puntos.PtosOtrosEst,
                ExcParEst = dato.Puntos.ExcParEst,
                ExcAprEst = dato.Puntos.ExcAprEst,
                CAprovechamiento = dato.Puntos.CAprovechamiento,
                CParticipacion = dato.Puntos.CParticipacion,
                CInstruccion = dato.Puntos.CInstruccion,
                ExpDoc = dato.Puntos.ExpDoc,
                Movimiento = dato.Puntos.Movimiento,
                Observacion1 = dato.Puntos.Observacion1,
                Observacion2 = dato.Puntos.Observacion2,
                Observacion3 = dato.Puntos.Observacion3,
                Fecha = dato.Puntos.Fecha.ToShortDateString(),
                PtosPublicacion = dato.Puntos.PtosPublicacion,
                PtosExpDoc = dato.Puntos.PtosExpDoc,
                PtosOrgInt = dato.Puntos.PtosOrgInt,
                RigeEst = dato.Puntos.RigeEst.ToShortDateString(),
                CalificacionEst = dato.Puntos.CalificacionEst,

                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros

            };
            if (dato.Puntos.FecRigePago.Year <= 1)
                obj.FecRigePago = "";
            if (dato.Puntos.Fecha.Year <= 1)
                obj.Fecha = "";

            return obj;
        }
    }
}
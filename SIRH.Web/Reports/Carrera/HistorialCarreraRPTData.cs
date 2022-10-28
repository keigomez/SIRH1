using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Carrera
{
    public class HistorialCarreraRPTData
    {           
        public int Puesto { get; set; }         
        public string Funcionario { get; set; }            
        public int Clase { get; set; }         
        public int Codigo { get; set; }         
        public int Division { get; set; }         
        public int Direccion { get; set; }         
        public string Ubicacion { get; set; }        
        public decimal TotalPuntos { get; set; }         
        public int Departamento { get; set; }         
        public decimal ValorPunto { get; set; }         
        public int AnnoExperienciaPR { get; set; }         
        public int Aprovechamiento { get; set; }         
        public int Participacion { get; set; }         
        public int ParticipacionInstruccion { get; set; }         
        public string CalificacionServicio { get; set; }         
        public decimal HorasAprovechamiento { get; set; }         
        public decimal HorasParticipacion { get; set; }         
        public decimal HorasInstruccion { get; set; }         
        public decimal PtosGrado { get; set; }         
        public decimal PtosAprovechamiento { get; set; }         
        public decimal PtosExperienciaPR { get; set; }         
        public decimal PtosParticipacion { get; set; }         
        public decimal PtosInstruccion { get; set; }         
        public decimal PtosOtros { get; set; }         
        public string ExplicacionOtros { get; set; }         
        public string Observacion { get; set; }         
        public string PSS1Feri { get; set; }         
        public string PSS1Feve { get; set; }         
        public string PSS2Feri { get; set; }         
        public string PSS2Feve { get; set; }         
        public string PSS3Feri { get; set; }         
        public string PSS3Feve { get; set; }         
        public int MesCarrera { get; set; }         
        public int AnnoCarrera { get; set; }         
        public string FecRigePago { get; set; }         
        public decimal HorasExcParticipacion { get; set; }         
        public decimal HorasExcAprovechamiento { get; set; }         
        public string NumResolucion { get; set; }         
        public string FecResolucion { get; set; }         
        public string Periodo { get; set; }         
        public string Grado { get; set; }         
        public string Nivel { get; set; }         
        public int AnnoExperienciaEst { get; set; }         
        public int AprovechamientoEst { get; set; }         
        public int ParticipacionEst { get; set; }         
        public int ParticipacionInstruccionEst { get; set; }         
        public decimal HorasAprovechamientoEst { get; set; }         
        public decimal HorasParticipacionEst { get; set; }         
        public decimal HorasInstruccionEst { get; set; }         
        public decimal PtosGradoEst { get; set; }         
        public decimal PtosAprovechamientoEst { get; set; }         
        public decimal PtosExperienciaEst { get; set; }         
        public decimal PtosParticipacionEst { get; set; }         
        public decimal PtosInstruccionEst { get; set; }         
        public decimal PtosOtroEst { get; set; }         
        public decimal ExcParticipacionEst { get; set; }         
        public decimal ExcAprovechamientoEst { get; set; }         
        public string Marca { get; set; }         
        public string Curso1 { get; set; }         
        public string Curso2 { get; set; }         
        public string Curso3 { get; set; }         
        public string Curso4 { get; set; }         
        public string Curso5 { get; set; }         
        public string Curso6 { get; set; }         
        public string Movimiento { get; set; }         
        public string Curso7 { get; set; }         
        public string Curso8 { get; set; }         
        public string Observacion1 { get; set; }         
        public string Observacion2 { get; set; }         
        public string Observacion3 { get; set; }        
        public string Fecha { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static HistorialCarreraRPTData GenerarDatosReporte(BusquedaHistoricoCarreraVM dato, string filtros) 
        {
            HistorialCarreraRPTData obj = new HistorialCarreraRPTData
            {
                Funcionario = dato.Carrera.Cedula + " - " + dato.Carrera.Nombre,
                Puesto = dato.Carrera.Puesto,
                Clase = dato.Carrera.Clase,
                Codigo = dato.Carrera.Codigo,
                Division = dato.Carrera.Division,
                Direccion = dato.Carrera.Direccion,
                Ubicacion = dato.Carrera.Ubicacion,
                TotalPuntos = dato.Carrera.TotalPuntos,
                Departamento = dato.Carrera.Departamento,
                ValorPunto = dato.Carrera.ValorPunto,
                AnnoExperienciaPR = dato.Carrera.AnnoExperienciaPR,
                Aprovechamiento = dato.Carrera.Aprovechamiento,
                Participacion = dato.Carrera.Participacion,
                ParticipacionInstruccion = dato.Carrera.ParticipacionInstruccion,
                CalificacionServicio = dato.Carrera.CalificacionServicio,
                HorasAprovechamiento = dato.Carrera.HorasAprovechamiento,
                HorasParticipacion = dato.Carrera.HorasParticipacion,
                HorasInstruccion = dato.Carrera.HorasInstruccion,
                PtosGrado = dato.Carrera.PtosGrado,
                PtosAprovechamiento = dato.Carrera.PtosAprovechamiento,
                PtosExperienciaPR = dato.Carrera.PtosExperienciaPR,
                PtosParticipacion = dato.Carrera.PtosParticipacion,
                PtosInstruccion = dato.Carrera.PtosInstruccion,
                PtosOtros = dato.Carrera.PtosOtros,
                ExplicacionOtros = dato.Carrera.ExplicacionOtros,
                Observacion = dato.Carrera.Observacion,
                PSS1Feri = dato.Carrera.PSS1Feri.ToShortDateString(),
                PSS1Feve = dato.Carrera.PSS1Feve.ToShortDateString(),
                PSS2Feri = dato.Carrera.PSS2Feri.ToShortDateString(),
                PSS2Feve = dato.Carrera.PSS2Feve.ToShortDateString(),
                PSS3Feri = dato.Carrera.PSS3Feri.ToShortDateString(),
                PSS3Feve = dato.Carrera.PSS3Feve.ToShortDateString(),
                MesCarrera = dato.Carrera.MesCarrera,
                AnnoCarrera = dato.Carrera.AnnoCarrera,
                FecRigePago = dato.Carrera.FecRigePago.ToShortDateString(),
                HorasExcParticipacion = dato.Carrera.HorasExcParticipacion,
                HorasExcAprovechamiento = dato.Carrera.HorasExcAprovechamiento,
                NumResolucion = dato.Carrera.NumResolucion,
                FecResolucion = dato.Carrera.FecResolucion.ToShortDateString(),
                Periodo = dato.Carrera.Periodo,
                Grado = dato.Carrera.Grado,
                Nivel = dato.Carrera.Nivel,
                AnnoExperienciaEst = dato.Carrera.AnnoExperienciaEst,
                AprovechamientoEst = dato.Carrera.AprovechamientoEst,
                ParticipacionEst = dato.Carrera.ParticipacionEst,
                ParticipacionInstruccionEst = dato.Carrera.ParticipacionInstruccionEst,
                HorasAprovechamientoEst = dato.Carrera.HorasAprovechamientoEst,
                HorasParticipacionEst = dato.Carrera.HorasParticipacionEst,
                HorasInstruccionEst = dato.Carrera.HorasInstruccionEst,
                PtosGradoEst = dato.Carrera.PtosGradoEst,
                PtosAprovechamientoEst = dato.Carrera.PtosAprovechamientoEst,
                PtosExperienciaEst = dato.Carrera.PtosExperienciaEst,
                PtosParticipacionEst = dato.Carrera.PtosParticipacionEst,
                PtosInstruccionEst = dato.Carrera.PtosInstruccionEst,
                PtosOtroEst = dato.Carrera.PtosOtroEst,
                ExcParticipacionEst = dato.Carrera.ExcParticipacionEst,
                ExcAprovechamientoEst = dato.Carrera.ExcAprovechamientoEst,
                Marca = dato.Carrera.Marca,                
                Curso1 = dato.Carrera.Curso1,
                Curso2 = dato.Carrera.Curso2,
                Curso3 = dato.Carrera.Curso3,
                Curso4 = dato.Carrera.Curso4,
                Curso5 = dato.Carrera.Curso5,
                Curso6 = dato.Carrera.Curso6,
                Curso7 = dato.Carrera.Curso7,
                Curso8 = dato.Carrera.Curso8,
                Movimiento = dato.Carrera.Movimiento,
                Observacion1 = dato.Carrera.Observacion1,
                Observacion2 = dato.Carrera.Observacion2,
                Observacion3 = dato.Carrera.Observacion3,
                Fecha = dato.Carrera.Fecha.ToShortDateString(),

                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros

            };

            if (dato.Carrera.FecRigePago.Year <= 1)
                obj.FecRigePago = "";
            if (dato.Carrera.Fecha.Year <= 1)
                obj.Fecha = "";
            return obj;
        }
    }
}
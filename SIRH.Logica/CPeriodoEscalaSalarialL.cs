using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPeriodoEscalaSalarialL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPeriodoEscalaSalarialL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Metodos

        public CBaseDTO GuardarPeriodoEscalaSalarial(CPeriodoEscalaSalarialDTO periodo)
        {
            try
            {
                var periodoDatos = ConvertirPeriodoEscalaSalarialADatos(periodo);

                var resultado = new CPeriodoEscalaSalarialD(contexto).GuardarPeriodoEscalaSalarial(periodoDatos);

                if (resultado.Codigo > 0)
                {
                    return ConvertirPeriodoEscalaSalarialADTO(((PeriodoEscalaSalarial)resultado.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO 
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }
        }

        internal static CPeriodoEscalaSalarialDTO ConvertirPeriodoEscalaSalarialADTO(PeriodoEscalaSalarial periodoEscalaSalarial)
        {
            if (periodoEscalaSalarial.FecCierre != null)
            {
                return new CPeriodoEscalaSalarialDTO
                {
                    IdEntidad = periodoEscalaSalarial.PK_Periodo,
                    FechaInicial = Convert.ToDateTime(periodoEscalaSalarial.FecInicial),
                    FechaCierre = Convert.ToDateTime(periodoEscalaSalarial.FecCierre),
                    NumeroResolucion = periodoEscalaSalarial.NumResolucion,
                    MontoPuntoCarrera = Convert.ToDecimal(periodoEscalaSalarial.MtoPuntoCarrera)
                };
            }
            else
            {
                return new CPeriodoEscalaSalarialDTO
                {
                    IdEntidad = periodoEscalaSalarial.PK_Periodo,
                    FechaInicial = Convert.ToDateTime(periodoEscalaSalarial.FecInicial),
                    NumeroResolucion = periodoEscalaSalarial.NumResolucion,
                    MontoPuntoCarrera = Convert.ToDecimal(periodoEscalaSalarial.MtoPuntoCarrera)
                };
            }
        }

        private PeriodoEscalaSalarial ConvertirPeriodoEscalaSalarialADatos(CPeriodoEscalaSalarialDTO periodo)
        {
            if (periodo.FechaCierre.Year != 1)
            {
                return new PeriodoEscalaSalarial
                {
                    NumResolucion = periodo.NumeroResolucion,
                    FecInicial = periodo.FechaInicial,
                    FecCierre = periodo.FechaCierre,
                    MtoPuntoCarrera = periodo.MontoPuntoCarrera
                };
            }
            else
            {
                return new PeriodoEscalaSalarial
                {
                    NumResolucion = periodo.NumeroResolucion,
                    FecInicial = periodo.FechaInicial,
                    MtoPuntoCarrera = periodo.MontoPuntoCarrera
                };
            }
        }

        #endregion
    }
}

using SIRH.Datos;
using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Logica
{
    public class CPuntosL
    {

        #region variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPuntosL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos


        public List<CBaseDTO> CargarDatosPuntos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();



            CPuntosD intermedio = new CPuntosD(contexto);

            var puntos = intermedio.CargarDatosPuntos();

            if (puntos.Codigo > 0)
            {
                var datosPuntos = (List<C_EMU_Puntos>)puntos.Contenido;
                foreach (var punto in datosPuntos)
                {
                    respuesta.Add(ConvertirDatosPuntosADTO(punto));
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontró ningún registro de puntos" });
            }

            return respuesta;
        }


        public CBaseDTO CargarDatosPuntosPorId(int id)
        {
            CBaseDTO respuesta = new CBaseDTO();

            CPuntosD intermedio = new CPuntosD(contexto);

            var punto = intermedio.CargarDatosPuntosPorId(id);

            if (punto.Codigo > 0)
            {
                respuesta = ConvertirDatosPuntosADTO((C_EMU_Puntos)punto.Contenido);
            }
            else
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = "No se encontró ningún registro de puntos" };
            }

            return respuesta;
        }


        public List<CBaseDTO> BuscarDatosPuntos(CPuntosDTO punto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CPuntosD intermedio = new CPuntosD(contexto);

            List<C_EMU_Puntos> datosPuntos = new List<C_EMU_Puntos>();

            if (punto.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarDatosPuntos(datosPuntos, punto.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosPuntos = (List<C_EMU_Puntos>)resultado.Contenido;
                }
            }

            if (punto.Nombre != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarDatosPuntos(datosPuntos, punto.Nombre, "Nombre"));
                if (resultado.Codigo > 0)
                {
                    datosPuntos = (List<C_EMU_Puntos>)resultado.Contenido;
                }
            }


            if (datosPuntos.Count > 0)
            {
                foreach (var item in datosPuntos)
                {

                    var datoCarrera = ConvertirDatosPuntosADTO(item);
                    respuesta.Add(datoCarrera);

                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        internal static CPuntosDTO ConvertirDatosPuntosADTO(C_EMU_Puntos punto)
        {
            CPuntosDTO respuesta;
            respuesta = new CPuntosDTO
            {
                ID = Convert.ToInt32(punto.ID),
                Nombre = Convert.ToString(punto.Nombre),
                Cedula = Convert.ToString(punto.Cedula),              
                TotalPuntos = Convert.ToDecimal(punto.TotalPuntos),
                ValorPunto = Convert.ToInt32(punto.ValorPunto),
                AnnoExpPR = Convert.ToInt32(punto.AnnoExpPR),
                Aprovechamiento = Convert.ToInt32(punto.Aprovechamiento),
                Participacion = Convert.ToInt32(punto.Participacion),
                ParticipacionInstruccion = Convert.ToInt32(punto.ParticipacionInstruccion),
                CalificacionServicio = Convert.ToString(punto.CalificacionServicio),
                HorasAprovechamiento = Convert.ToDecimal(punto.HorasAprovechamiento),
                HorasParticipacion = Convert.ToDecimal(punto.HorasParticipacion),
                HorasInstruccion = Convert.ToDecimal(punto.HorasInstruccion),
                PtosGrado = Convert.ToDecimal(punto.PtosGrado),
                PtosAprovechamiento = Convert.ToDecimal(punto.PtosAprovechamiento),
                PtosExpPR = Convert.ToDecimal(punto.PtosExpPR),
                PtosParticipacion = Convert.ToDecimal(punto.PtosParticipacion),
                PtosInstruccion = Convert.ToDecimal(punto.PtosInstruccion),
                PtosOtros = Convert.ToDecimal(punto.PtosOtros),
                ExplOtros = Convert.ToString(punto.ExplicacionOtros),
                Observacion = Convert.ToString(punto.Observacion),              
                MesCarrera = Convert.ToInt32(punto.MesCarrera),
                AnnoCarrera = Convert.ToInt32(punto.AnnoCarrera),
                FecRigePago = Convert.ToDateTime(punto.FecRigePago),
                HsExcPar = Convert.ToDecimal(punto.HsExcPar),
                HsExcApr = Convert.ToDecimal(punto.HsExcApr),
                NumResolucion = Convert.ToString(punto.NumResolucion),
                FecResolucion = Convert.ToDateTime(punto.FecResolucion),
                Periodo = Convert.ToString(punto.Periodo),
                Grado = Convert.ToString(punto.Grado),
                Nivel = Convert.ToString(punto.Nivel),  
                AnnoExpEst = Convert.ToInt32(punto.AnnoExpEst),
                AprovechamientoEst = Convert.ToInt32(punto.AprovechamientoEst),
                ParticipacionEst = Convert.ToInt32(punto.ParticipacionEst),
                HoraAprEst = Convert.ToDecimal(punto.HoraAprEst),
                HoraParEst = Convert.ToDecimal(punto.HoraParEst),            
                PtosGrEst = Convert.ToDecimal(punto.PtosGrEst),
                PtosAprEst = Convert.ToDecimal(punto.PtosAprEst),
                PtosExpEst = Convert.ToDecimal(punto.PtosExpEst),
                PtosParEst = Convert.ToDecimal(punto.PtosParEst),               
                PtosOtrosEst = Convert.ToDecimal(punto.PtosOtrEst),
                ExcParEst = Convert.ToDecimal(punto.ExcParEst),
                ExcAprEst = Convert.ToDecimal(punto.ExcAprEst),
                CAprovechamiento = Convert.ToDecimal(punto.CAprovechamiento),
                CParticipacion = Convert.ToDecimal(punto.CParticipacion),
                CInstruccion = Convert.ToDecimal(punto.CInstruccion),
                ExpDoc = Convert.ToDecimal(punto.ExpDoc),
                Movimiento = Convert.ToString(punto.Movimiento),
                Observacion1 = Convert.ToString(punto.Observacion1),
                Observacion2 = Convert.ToString(punto.Observacion2),
                Observacion3 = Convert.ToString(punto.Observacion3),
                Fecha = Convert.ToDateTime(punto.Fecha),
                PtosPublicacion = Convert.ToDecimal(punto.PtosPublicacion),
                PtosExpDoc = Convert.ToDecimal(punto.PtosExpDoc),
                PtosOrgInt = Convert.ToDecimal(punto.PtosOrgInt),
                RigeEst = Convert.ToDateTime(punto.RigeEst),
                CalificacionEst = Convert.ToString(punto.CalficacionEst)
            };
            return respuesta;
        }



        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCalificacionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region constructor

        public CCalificacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        internal static CCalificacionDTO ConvertirDatosCCalificacionADto(Calificacion item)
        {

            return new CCalificacionDTO
            {

             IdEntidad= item.PK_Calificacion,
             DesCalificacion = item.DesCalificacion,

            };
        }

      

        public CBaseDTO GuardarCalificacion(string cedula, CCalificacionDTO calificacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionD intermedio = new CCalificacionD(contexto);

                Calificacion datosCalificacion = new Calificacion
                {
                    DesCalificacion = calificacion.DesCalificacion,                    
                };

                respuesta = intermedio.GuardarCalificacion(cedula, datosCalificacion);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }

        internal static Calificacion ConvertirDTOCalificacionADatos(CCalificacionDTO calificacionDTO)
        {
            return new Calificacion
            {
                PK_Calificacion = calificacionDTO.IdEntidad,
                DesCalificacion = calificacionDTO.DesCalificacion,
            };
        }

        public List<CBaseDTO> CargarCalificaciones()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CCalificacionD intermedio = new CCalificacionD(contexto);

            var calificaciones = intermedio.CargarCalificaciones();
            if (calificaciones != null)
            {
                foreach (var item in calificaciones)
                {
                    respuesta.Add(new CCalificacionDTO { IdEntidad = item.PK_Calificacion, DesCalificacion = item.DesCalificacion });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron cantones" });
            }

            return respuesta;
        }

       
       



        #endregion
    }
}

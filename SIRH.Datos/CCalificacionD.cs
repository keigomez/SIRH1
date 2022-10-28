using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCalificacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CCalificacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda calificacion en la BD
        /// </summary>
        /// <returns>Retorna las calificaciones</returns>
        
        public int GuardarCalificacion(Calificacion Calificacion)
        {
            entidadBase.Calificacion.Add(Calificacion);
            return Calificacion.PK_Calificacion;
        }

        public CRespuestaDTO GuardarCalificacion(string cedula, Calificacion calificacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Calificacion.Add(calificacion);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = calificacion
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        /// <summary>
        /// Obtiene la lista de calificaciones de la BD
        /// </summary>
        /// <returns>Retorna una lista de calificaciones</returns>
        
        public List<Calificacion> CargarCalificaciones()
        {
            List<Calificacion> resultados = new List<Calificacion>();

            resultados = entidadBase.Calificacion.ToList();

            return resultados;
        }

        /// <summary>
        /// Carga las calificaciones de la BD
        /// </summary>
        /// <returns>Retorna calificacion</returns>
        public Calificacion CargarCalificacionPorID(int idCalificacion)
        {
            Calificacion resultado = new Calificacion();

            resultado = entidadBase.Calificacion.Where(R => R.PK_Calificacion == idCalificacion).FirstOrDefault();

            return resultado;
        }

        #endregion
    }
}
//public bool HasChanges(UbicacionAdministrativa temp)
//{
//    bool resultado = false;
//    if (entidadBase.UbicacionAdministrativa.Where(R => R.idUbicacionAdministrativa == temp.idUbicacionAdministrativa).Count() > 0)
//    {
//        if(
//    }
//    return resultado;
//}
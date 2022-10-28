using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstadoDesarraigoD
    {
        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoDesarraigoD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO BuscarEstadoCodigo(int idEstado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoDesarraigo
                                                .FirstOrDefault(E => E.PK_EstadoDesarraigo == idEstado);
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún estado de desarraigo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO BuscarEstadoDesarraigoNombre(string nombre)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (nombre=="Vencido por incapacidad") { nombre = "Vencido_Incap"; }
                else
                    if (nombre == "Vencido por vacaciones") { nombre = "Vencido_Vac"; }
                else
                    if  (nombre == "Vencido por permiso sin salario") { nombre = "Vencido_PSS"; }
                var datosEntidad = entidadesBase.EstadoDesarraigo
                                                .FirstOrDefault(E => E.NomEstadoDesarraigo == nombre);
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún estado de desarraigo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo=-1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ListarEstadoDesarraigo()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoDesarraigo.ToList();
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró estados de desarraigo");
                }
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

        #endregion
    }
}

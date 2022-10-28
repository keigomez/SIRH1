using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCantonD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion


        #region Constructor

        public CCantonD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda Canton en la BD
        /// </summary>
        /// <returns>Retorna el Canton</returns>

        public int GuardarCanton(Canton Canton)
        {
            entidadBase.Canton.Add(Canton);
            return Canton.PK_Canton;
        }
        /// <summary>
        /// Obtiene la lista de Cantones de la BD
        /// </summary>
        /// <returns>Retorna una lista de cantones</returns>
        public List<Canton> CargarCantones()
        {
            List<Canton> resultados = new List<Canton>();

            resultados = entidadBase.Canton.ToList();

            return resultados;
        }
        /// <summary>
        /// Carga el Canton de la BD
        /// </summary>
        /// <returns>Retorna Canton</returns>
        public Canton CargarCantonIdProvincia(int idProvincia)
        {
            Canton resultado = new Canton();

            resultado = entidadBase.Canton.Where(R => R.Provincia.PK_Provincia == idProvincia).FirstOrDefault();

            return resultado;
        }

        // CANTON POR ID CANTON
        public Canton CargarCantonId(int idCanton) 
        {
            Canton resultado = new Canton();

            resultado = entidadBase.Canton.Where(R => R.PK_Canton == idCanton).FirstOrDefault();

            return resultado;

        }

        public CRespuestaDTO BuscarCanton(int codCanton)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.Canton.Include("Provincia").Where(C => C.PK_Canton == codCanton).FirstOrDefault();

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
                    throw new Exception("No se encontró el cantón indicado");
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

        public CRespuestaDTO BuscarCantonProvincia(int codProvincia)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.Canton.Include("Provincia").Where(C => C.Provincia.PK_Provincia == codProvincia).ToList();

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
                    throw new Exception("No se encontró los cantones indicados");
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


        public CRespuestaDTO ListarCantones()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.Canton.Include("Provincia").ToList();

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
                    throw new Exception("No se encontraron cantones registrados");
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
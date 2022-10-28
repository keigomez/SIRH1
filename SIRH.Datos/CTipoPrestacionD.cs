using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Datos
{
    public class CTipoPrestacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoPrestacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos


        public CRespuestaDTO ListarTipoPrestacion()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.TipoPrestacion.ToList();

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
                    throw new Exception("No se encontró ningún tipo de prestacion legal");
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


        public List<TipoPrestacion> RetornarTiposPrestacionP()
        {
            return entidadBase.TipoPrestacion.ToList();
        }


        public CRespuestaDTO ObtenerTipoPrestacion(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var tipo = entidadBase.TipoPrestacion.Where(C => C.PK_TipoPrestacion == codigo).FirstOrDefault();

                if (tipo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningún tipo de prestación." }
                    };
                    return respuesta;
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



        public CRespuestaDTO RetornarTipoPrestaciones()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = entidadBase.TipoPrestacion.ToList();

                if (tipo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo
                    };
                }
                else
                {
                  
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Prestaciones"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }




        #endregion
    }
}

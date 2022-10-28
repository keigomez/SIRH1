using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadGubernamentalD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadGubernamentalD(SIRHEntities entidadGlobal)

        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO GuardarEntidadGubernamental(string puesto, EntidadGubernamental gubernamental)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.EntidadGubernamental.Add(gubernamental);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = gubernamental
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
    

        public CRespuestaDTO BuscarEntidadGubernamental(int codEntidadGubernamental)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.EntidadGubernamental.Where(M => M.PK_EntidadGubernamental == codEntidadGubernamental).FirstOrDefault();

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
                    throw new Exception("No se encontró ninguna Entidad Gubernamental");
                }
            }
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO ListarEntidadesGubernamentales()
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosEntidad = entidadBase.EntidadGubernamental.ToList();
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
                    throw new Exception("No se encontraron entidades gubernamentales");
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

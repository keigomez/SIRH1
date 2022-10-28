using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadAdscritaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadAdscritaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO GuardarEntidadAdscrita(string puesto, EntidadAdscrita adscrita)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.EntidadAdscrita.Add(adscrita);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = adscrita
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
      
        public CRespuestaDTO BuscarEntidadAdscrita(int codEntidadAdscrita)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.EntidadAdscrita.Where(M => M.PK_EntidadAdscrita == codEntidadAdscrita).FirstOrDefault();

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
                    throw new Exception("No se encontró ninguna Entidad Adscrita");
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

        //http://192.168.153.6/psiws/wsConsultaPersona.asmx?WSDL
        //http://192.168.153.6/psiws/wsConsultaPersona.asmx?WSDL
        //http://192.168.153.6/psiws/wsConsultaPersona.asmx?WSDL
        //http://192.168.153.6/psiws/wsConsultaPersona.asmx?WSDL

        public CRespuestaDTO ListarEntidadesAdscritas()
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosEntidad = entidadBase.EntidadAdscrita.ToList();
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
                    throw new Exception("No se encontraron entidades adscritas");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;


namespace SIRH.DatosMarcasReloj
{
    public class CDispositivoD
    {
        #region Variables

        private EmpresasDataDB1Entities entidadBaseEmpresas = new EmpresasDataDB1Entities();
        private MasterTASEntities entidadBaseTAS = new MasterTASEntities();
        private SIRHEntities entidadBaseSIRH = new SIRHEntities();

        #endregion

        #region Constructor

        public CDispositivoD(EmpresasDataDB1Entities entidadGlobal, MasterTASEntities entidadTAS, SIRHEntities entidadSIRH)
        {
            entidadBaseEmpresas = entidadGlobal;
            entidadBaseTAS = entidadTAS;
            entidadBaseSIRH = entidadSIRH;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Busca el dispositivo indicado por parámetros en la base de datos del reloj marcador
        /// </summary>
        /// <param name="dispositivos">Entidad de dispositivo en la cual se especifica el id del reloj que se desea buscar</param>
        /// <returns>Retorna el dispositivo encontrado</returns>
        public CRespuestaDTO BuscarDispositivo(Dispositivos dispositivos)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseTAS.Dispositivos.Where(D => D.IdDispositivo == dispositivos.IdDispositivo).FirstOrDefault();

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
                    throw new Exception("No se encontró el dispositivo indicado");
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

        /// <summary>
        /// Busca todos los dispositivos registrados en la base de datos del reloj marcador
        /// </summary>
        /// <returns>Retorna los dispositivos encontrados</returns>
        public CRespuestaDTO ListarDispositivos()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseTAS.Dispositivos.ToList();

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
                    throw new Exception("No se encontraron dispositivos almacenados en la base de datos");
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

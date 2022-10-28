using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;


namespace SIRH.DatosMarcasReloj
{
    public class CMotivoBajaD
    {
        #region Variables

        private EmpresasDataDB1Entities entidadBaseEmpresas = new EmpresasDataDB1Entities();
        private MasterTASEntities entidadBaseTAS = new MasterTASEntities();
        private SIRHEntities entidadBaseSIRH = new SIRHEntities();

        #endregion

        #region Constructor

        public CMotivoBajaD(EmpresasDataDB1Entities entidadEmpresas, MasterTASEntities entidadTAS, SIRHEntities entidadSIRH)
        {
            entidadBaseEmpresas = entidadEmpresas;
            entidadBaseTAS = entidadTAS;
            entidadBaseSIRH = entidadSIRH;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Busca el motivo de baja que recibe por parametros
        /// </summary>
        /// <param name="empleadoDispositivo">Entidad que tiene definido el id del motivo de baja que se desea buscar</param>
        /// <returns>Retorna el motivo de baja indicado por parámetros</returns>
        public CRespuestaDTO BuscarMotivoBaja(MotivoBaja motivoBaja)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.MotivoBaja.Where(MB => MB.IdBaja == motivoBaja.IdBaja).FirstOrDefault();

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
                    throw new Exception("No se encontró el motivo de baja indicado");
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
        /// Busca todos los motivo de baja que recibe por parametros
        /// </summary>
        /// <param name="empleadoDispositivo">Entidad que tiene definido el id del motivo de baja que se desea buscar</param>
        /// <returns>Retorna el motivo de baja indicado por parámetros</returns>
        public CRespuestaDTO ListarMotivosBaja()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.MotivoBaja.ToList();

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
                    throw new Exception("No se encontraron motivos de baja almacenados en la base de datos");
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

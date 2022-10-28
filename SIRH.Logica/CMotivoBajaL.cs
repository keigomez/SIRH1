using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.DatosMarcasReloj;

namespace SIRH.Logica
{
    public class CMotivoBajaL
    {
        #region Variables

        SIRHEntities contextoSIRH;
        EmpresasDataDB1Entities contextoEmpresas;
        MasterTASEntities contextoMasterTAS;

        #endregion

        #region Constructor

        public CMotivoBajaL()
        {
            contextoSIRH = new SIRHEntities();
            contextoEmpresas = new EmpresasDataDB1Entities();
            contextoMasterTAS = new MasterTASEntities();
        }
        #endregion

        #region Métodos

        internal static CMotivoBajaDTO ConvertirDatosMotivoBajaADTO(MotivoBaja item)
        {
            return new CMotivoBajaDTO
            {
                IdEntidad = item.IdBaja,
                Descripcion = item.Descripcion
            };
        }

        /// <summary>
        /// Busca el motivo de baja que recibe por parametros
        /// </summary>
        /// <param name="motivoBaja">DTO que tiene definido el id del motivo de baja que se desea buscar</param>
        /// <returns>Retorna el motivo de baja indicado por parámetros</returns>
        public CBaseDTO BuscarMotivoBaja(CMotivoBajaDTO motivoBaja)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMotivoBajaD intermedio = new CMotivoBajaD(contextoEmpresas, contextoMasterTAS, contextoSIRH);


                var motivoBajaEntidad = new MotivoBaja
                {
                    IdBaja = motivoBaja.IdEntidad
                };

                var bajaAux = intermedio.BuscarMotivoBaja(motivoBajaEntidad);

                if (bajaAux.Codigo > 0)
                {
                    respuesta = ConvertirDatosMotivoBajaADTO((MotivoBaja)bajaAux.Contenido);
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)bajaAux).Contenido;
                    throw new Exception();
                }
                return respuesta;
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Busca todos los motivo de baja que recibe por parametros
        /// </summary>
        /// <param name="empleadoDispositivo">DTO que tiene definido el id del motivo de baja que se desea buscar</param>
        /// <returns>Retorna el motivo de baja indicado por parámetros</returns>
        public List<CBaseDTO> ListarMotivoBaja()
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CMotivoBajaD intermedio = new CMotivoBajaD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                var datos = intermedio.ListarMotivosBaja();

                if (datos.Codigo > 0)
                {
                    foreach (var motivo in (List<MotivoBaja>)datos.Contenido)
                    {
                        var datoMotivoBaja = ConvertirDatosMotivoBajaADTO((MotivoBaja)motivo);

                        respuesta.Add(datoMotivoBaja);
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)datos.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return respuesta;
        }


        #endregion
    }
}

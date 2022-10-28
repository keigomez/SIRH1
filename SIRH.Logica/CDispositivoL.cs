using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.DatosMarcasReloj;

namespace SIRH.Logica
{
    public class CDispositivoL
    {
        #region Variables

        SIRHEntities contextoSIRH;
        EmpresasDataDB1Entities contextoEmpresas;
        MasterTASEntities contextoMasterTAS;

        #endregion

        #region Constructor

        public CDispositivoL()
        {
            contextoSIRH = new SIRHEntities();
            contextoEmpresas = new EmpresasDataDB1Entities();
            contextoMasterTAS = new MasterTASEntities();
        }
        #endregion

        #region Métodos

        internal static CDispositivoDTO ConvertirDatosDispositivoADTO(Dispositivos item)
        {
            return new CDispositivoDTO
            {
                IdEntidad = item.IdDispositivo,
                Ubicacion = item.Ubicacion,
                Descripcion = item.Descripcion
            };
        }

        /// <summary>
        /// Busca el dispositivo indicado por parámetros en la base de datos del reloj marcador
        /// </summary>
        /// <param name="dispositivos">DTO del dispositivo en el cual se especifica el id del reloj que se desea buscar</param>
        /// <returns>Retorna el dispositivo encontrado</returns>
        public CBaseDTO BuscarDispositivo(CDispositivoDTO dispositivo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDispositivoD intermedio = new CDispositivoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);


                var dispositivosEntidad = new Dispositivos
                {
                    IdDispositivo = dispositivo.IdEntidad
                };

                var dispositivoEnt = intermedio.BuscarDispositivo(dispositivosEntidad);

                if (dispositivoEnt.Codigo > 0)
                {
                    respuesta = ConvertirDatosDispositivoADTO((Dispositivos)dispositivoEnt.Contenido);
                    return respuesta;

                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)dispositivoEnt).Contenido;
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Busca todos los dispositivos registrados en la base de datos del reloj marcador
        /// </summary>
        /// <returns>Retorna los dispositivos encontrados</returns>
        public List<CBaseDTO> ListarDispositivos()
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDispositivoD intermedio = new CDispositivoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                var datos = intermedio.ListarDispositivos();


                if (datos.Codigo > 0)
                {
                    foreach (var dispositivo in (List<Dispositivos>)datos.Contenido)
                    {
                        var datoDispositivo = ConvertirDatosDispositivoADTO((Dispositivos)dispositivo);

                        respuesta.Add(datoDispositivo);
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

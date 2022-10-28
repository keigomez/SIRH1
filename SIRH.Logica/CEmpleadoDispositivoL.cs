using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.DatosMarcasReloj;

namespace SIRH.Logica
{
    public class CEmpleadoDispositivoL
    {
        #region Variables

        SIRHEntities contextoSIRH;
        EmpresasDataDB1Entities contextoEmpresas;
        MasterTASEntities contextoMasterTAS;

        #endregion

        #region Constructor

        public CEmpleadoDispositivoL()
        {
            contextoSIRH = new SIRHEntities();
            contextoEmpresas = new EmpresasDataDB1Entities();
            contextoMasterTAS = new MasterTASEntities();
        }
        #endregion

        #region Métodos

        internal static CEmpleadoDispositivoDTO ConvertirDatosEmpleadoDispositivoADTO(EmpleadoDispositivo item)
        {
            
            return new CEmpleadoDispositivoDTO
            {
                Dispositivo = item.IdDispositivo.ToString(),
                Digitos = item.CodigoAcceso
            };
        }

        /// <summary>
        /// Asigna al empleado un reloj marcador
        /// </summary>
        /// <param name="empleadoDispositivo">DTO que almacenará en la base de datos</param>
        /// <param name="empleado">DTO que tiene definido el número de cédula del funcionario al que se desea asignar el dispositivo</param>
        /// <param name="dispositivo">DTO que tiene definido el id del dispositivo que se asignará al funcionario</param>
        /// <returns>Retorna el disposivo asignado</returns>
        public CBaseDTO AsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CEmpleadoDispositivoD intermedio = new CEmpleadoDispositivoD(contextoEmpresas, contextoMasterTAS);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var dispositivoEntidad = new Dispositivos
                {
                    IdDispositivo = dispositivo.IdEntidad
                };

                var empleadoDispositivoEntidad = new EmpleadoDispositivo
                {

                    CodigoAcceso = empleado.Digitos
                };

                var respuestaAux = intermedio.AsignarRelojMarcador(empleadoDispositivoEntidad, empleadoEntidad, dispositivoEntidad);

                if (respuestaAux.Codigo<1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAux).Contenido;
                    throw new Exception();
                }
                else
                {
                    respuesta = ConvertirDatosEmpleadoDispositivoADTO((EmpleadoDispositivo) respuestaAux.Contenido);
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Des asigna al empleado un reloj marcador
        /// </summary>
        /// <param name="empleado">DTO que tiene definido el número de cédula del funcionario al que se desea des asignar el dispositivo</param>
        /// <param name="dispositivo">DTO que tiene definido el id del dispositivo que se des-asignara al funcionario</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public CBaseDTO DesAsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CEmpleadoDispositivoD intermedio = new CEmpleadoDispositivoD(contextoEmpresas, contextoMasterTAS);
                var empleadoEntidad = new Empleado
                {
                   CodigoEmpleado=empleado.CodigoEmpleado
                };

                var dispositivoEntidad = new Dispositivos
                {
                    IdDispositivo = dispositivo.IdEntidad,
                    Descripcion = dispositivo.Descripcion
                };

                var respuestaAux = intermedio.DesAsignarRelojMarcador(empleadoEntidad, dispositivoEntidad);

                if (respuestaAux.Codigo<1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAux).Contenido;
                    throw new Exception();
                }
                else
                {
                    respuesta = ConvertirDatosEmpleadoDispositivoADTO((EmpleadoDispositivo)respuestaAux.Contenido);
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Busca los relojes asignados  a un funcionario
        /// </summary>
        /// <param name="empleado">DTO que tiene definido el número de cédula del funcionario al que se desea buscar el o los dispositivos asignados</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public List<CBaseDTO> BuscarDispositivosAsignados(CEmpleadoDTO empleado)
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEmpleadoDispositivoD intermedio = new CEmpleadoDispositivoD(contextoEmpresas, contextoMasterTAS);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var datos = intermedio.BuscarDispositivosAsignados(empleadoEntidad);

                if (datos.Codigo > 0)
                {
                    foreach (var empleadoDisp in (List<EmpleadoDispositivo>)datos.Contenido)
                    {
                        var datoEmpleadoDispositivo = ConvertirDatosEmpleadoDispositivoADTO((EmpleadoDispositivo)empleadoDisp);

                        respuesta.Add(datoEmpleadoDispositivo);
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)datos.Contenido);
                }
                return respuesta;
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }
       
        #endregion
    }
}

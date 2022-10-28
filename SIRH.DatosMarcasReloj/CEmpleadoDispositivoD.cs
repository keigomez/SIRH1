using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using System.Data.Objects;

namespace SIRH.DatosMarcasReloj
{
    public class CEmpleadoDispositivoD
    {
        #region Variables

        private EmpresasDataDB1Entities entidadBaseEmpresa = new EmpresasDataDB1Entities();
        private MasterTASEntities entidadBaseTAS = new MasterTASEntities();

        #endregion

        #region Constructor

        public CEmpleadoDispositivoD(EmpresasDataDB1Entities entidadGlobal, MasterTASEntities entidadTAS)
        {
            entidadBaseEmpresa = entidadGlobal;
            entidadBaseTAS = entidadTAS;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Asigna al empleado un reloj marcador
        /// </summary>
        /// <param name="empleadoDispositivo">Entidad que almacenará en la base de datos</param>
        /// <param name="empleado">Entidad que tiene definido el número de cédula del funcionario al que se desea asignar el dispositivo</param>
        /// <param name="dispositivo">Entidad que tiene definido el id del dispositivo que se asignará al funcionario</param>
        /// <returns>Retorna el disposivo asignado</returns>
        public CRespuestaDTO AsignarRelojMarcador(EmpleadoDispositivo empleadoDispositivo, Empleado empleado, Dispositivos dispositivo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {

           
                var empleadoEntidad = entidadBaseEmpresa.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();
                
                if (empleadoEntidad != null)
                {
                    var dispositivoEntidad = entidadBaseTAS.Dispositivos.Where(D => D.IdDispositivo == dispositivo.IdDispositivo).FirstOrDefault();
                    if (dispositivoEntidad != null)
                    {
                        var dispositivoAsignadoPrevio =  entidadBaseEmpresa.EmpleadoDispositivo.Where(ED=>ED.CodigoEmpleado == empleadoEntidad.CodigoEmpleado && ED.IdDispositivo == dispositivoEntidad.IdDispositivo).FirstOrDefault();

                        if (dispositivoAsignadoPrevio == null)
                        {
                            empleadoDispositivo.CodigoEmpleado = empleado.CodigoEmpleado;
                            empleadoDispositivo.IdDispositivo = dispositivoEntidad.IdDispositivo;
                            entidadBaseEmpresa.EmpleadoDispositivo.Add(empleadoDispositivo);
                            
                            entidadBaseEmpresa.SaveChanges();

                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = empleadoDispositivo
                            };

                            return respuesta;
                        }
                        else {
                            throw new Exception("Uno o varios dispositivos ya fueron asignados al funcionario indicado");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el dispositivo indicado");
                    }
                }
                else
                {
                    throw new Exception("No se pudo almacenar correctamente el empleado en la base de datos del reloj marcador");
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
        /// Des asigna al empleado un reloj marcador
        /// </summary>
        /// <param name="empleado">Entidad que tiene definido el número de cédula del funcionario al que se desea des asignar el dispositivo</param>
        /// <param name="dispositivo">Entidad que tiene definido el id del dispositivo que se des-asignara al funcionario</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public CRespuestaDTO DesAsignarRelojMarcador(Empleado empleado, Dispositivos dispositivo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var empleadoEntidad = entidadBaseEmpresa.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();
                if (empleadoEntidad != null)
                {
                    var dispositivoEntidad = entidadBaseTAS.Dispositivos.Where(D => D.IdDispositivo == dispositivo.IdDispositivo).FirstOrDefault();
                    if (dispositivoEntidad != null)
                    {
                        var empleadoDispositivoAux = entidadBaseEmpresa.EmpleadoDispositivo
                            .Where(ED => ED.IdDispositivo == dispositivoEntidad.IdDispositivo && ED.CodigoEmpleado == empleadoEntidad.CodigoEmpleado);

                        if (empleadoDispositivoAux != null)
                        {
                            //entidadBaseEmpresa.DeleteObject(empleadoDispositivoAux);
                            //entidadBaseEmpresa.SaveChanges();

                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = empleadoEntidad
                            };

                            return respuesta;
                        }
                        else
                        {
                            throw new Exception("No se encontró ningún dispositivo asignado al usuario especificado");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el dispositivo indicado");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el empleado indicado");
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
        /// Busca los relojes asignados  a un funcionario
        /// </summary>
        /// <param name="empleadoDispositivo">Entidad que tiene definido el número de cédula del funcionario al que se desea buscar el o los dispositivos asignados</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public CRespuestaDTO BuscarDispositivosAsignados(Empleado empleado)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var empleadoEntidad = entidadBaseEmpresa.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();
                if (empleadoEntidad != null)
                {

                    var dispositivosEntidad = entidadBaseEmpresa.EmpleadoDispositivo.Where(DS => DS.CodigoEmpleado == empleado.CodigoEmpleado).ToList();

                    if (dispositivosEntidad != null)
                    {

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = dispositivosEntidad
                        };

                        return respuesta;

                    }
                    else
                    {
                        throw new Exception("No se encontró el dispositivo indicado");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el empleado indicado");
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

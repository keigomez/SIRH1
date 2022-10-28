using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.DatosMarcasReloj
{
    class CEntradaSalidaD
    {
        #region Variables

        private EmpresasDataDB1Entities entidadBaseEmpresas = new EmpresasDataDB1Entities();
        private MasterTASEntities entidadBaseTAS = new MasterTASEntities();

        #endregion

        #region Constructor

        public CEntradaSalidaD(EmpresasDataDB1Entities entidadEmpresas, MasterTASEntities entidadTAS)
        {
            entidadBaseEmpresas = entidadEmpresas;
            entidadBaseTAS = entidadTAS;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO DescargarMarcasEmpleado(Empleado empleado)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                if (datosEntidad != null)
                {
                    var marcas = entidadBaseEmpresas.EntradaSalida.Where(ES=>ES.CodigoEmpleado==datosEntidad.CodigoEmpleado);
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = datosEntidad
                        };
                        return respuesta;
                    
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

        public CRespuestaDTO DescargarMarcasEmpleadoTipoOperacion(Empleado empleado, string tipoOperacion)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                if (datosEntidad != null)
                {
                    var marcas = entidadBaseEmpresas.EntradaSalida.Where(ES => ES.CodigoEmpleado == datosEntidad.CodigoEmpleado && ES.TipoOperacion.Equals(tipoOperacion));
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;

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

        public CRespuestaDTO DescargarMarcasEmpleadoFecha(Empleado empleado, DateTime fecha1, DateTime fecha2)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                if (datosEntidad != null)
                {
                    if (fecha1.Year>1 && fecha2.Year<1)
                    {

                        var marcas = entidadBaseEmpresas.EntradaSalida.Where(ES => ES.CodigoEmpleado == datosEntidad.CodigoEmpleado && ES.Fecha > fecha1);
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = marcas
                        };
                        return respuesta;
                    }
                    else if (fecha1.Year > 1 && fecha2.Year > 1)
                    {
                        var marcas = entidadBaseEmpresas.EntradaSalida.Where(ES => ES.CodigoEmpleado == datosEntidad.CodigoEmpleado && ES.Fecha > fecha1 && ES.Fecha < fecha2);
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = marcas
                        };
                        return respuesta;
                    }
                    else {
                        throw new Exception("Error con las fechas");
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

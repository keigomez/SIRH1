using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.DatosMarcasReloj
{
    public class CEmpleadoD
    {
        #region Variables

        private EmpresasDataDB1Entities entidadBaseEmpresas = new EmpresasDataDB1Entities();
        private MasterTASEntities entidadBaseTAS = new MasterTASEntities();
        private SIRHEntities entidadBaseSIRH = new SIRHEntities();

        #endregion

        #region Constructor

        public CEmpleadoD(EmpresasDataDB1Entities entidadGlobal, MasterTASEntities entidadTAS, SIRHEntities entidadSIRH)
        {
            entidadBaseEmpresas = entidadGlobal;
            entidadBaseTAS = entidadTAS;
            entidadBaseSIRH = entidadSIRH;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca el empleado activo en la base de datos del reloj marcador a partir de la entidad tipo empleado que recibe por parámetros
        /// </summary>
        /// <param name="empleado">Entidad de empleado en la cual se especifica el número de cédula que se desea buscar</param>
        /// <returns>Retorna el empleado encontrado</returns>
        public CRespuestaDTO BuscarEmpleadoActivo(Empleado empleado)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                if (datosEntidad != null)
                {
                    if (datosEntidad.Estatus == 1)
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
                        throw new Exception("El empleado no se encuentra activo");
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
        /// Almacena el empleado que recibe por parametros en la base de datos del reloj marcador
        /// </summary>
        /// <param name="funcionario">Entidad de funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">Entidad de empleado que se desea almacenar</param>
        /// <returns>Retorna el empleado almacenado</returns>
        public CRespuestaDTO AlmacenarEmpleado(Funcionario funcionario, Empleado empleado)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                
                var contratacionEntidad = entidadBaseSIRH.DetalleContratacion
                    .Where(DC => DC.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).OrderByDescending(DC => DC.PK_DetalleContratacion).FirstOrDefault();
                if (contratacionEntidad != null)
                {
                    contratacionEntidad.MarcaAsistencia = true;
                    empleado.CodigoAcceso = ObtenerConsecutivo().ToString();

                    if (Convert.ToInt32(empleado.CodigoAcceso) > 1)
                    {
                       if (empleado.CodigoAcceso == "1641")
                        {
                            empleado.CodigoAcceso = "1642";
                        }
                        entidadBaseEmpresas.Empleado.Add(empleado);
                        entidadBaseSIRH.SaveChanges();
                        entidadBaseEmpresas.SaveChanges();
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = empleado
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("No se pudo obtener un código de acceso");
                    }
                }
                else
                {
                    throw new Exception("No se pudo encontrar un detalle de contratación para el funcionario indicado");
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
        /// Da de baja al empleado que recibe por parámetros
        /// </summary>
        /// <param name="funcionario">Entidad de funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">Entidad de empleado que se desea dar de baja</param>
        /// <param name="detNombramiento">Detalle del nombramiento del empleado</param>
        /// <param name="motivoBaja">Id del motivo de baja</param>
        /// <returns>Retorna el empleado dado de baja</returns>
        public CRespuestaDTO DesactivarEmpleado(Funcionario funcionario, Empleado empleado, /*DetalleNombramiento detNombramiento*/ MotivoBaja motivoBaja)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                if (datosEntidad != null)
                {
                    var funcionarioEntidad = entidadBaseSIRH.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();
                    var nombramientoEntidad = entidadBaseSIRH.Nombramiento.Where(N => N.Funcionario.PK_Funcionario == funcionarioEntidad.PK_Funcionario).FirstOrDefault();
                    //var detNombramientoEntidad = null;//entidadBaseSIRH.DetalleNombramiento.Where(DN => DN.Nombramiento.PK_Nombramiento == nombramientoEntidad.PK_Nombramiento).FirstOrDefault();

                    /*if (detNombramientoEntidad != null)
                    {
                        var motivoBajaEntidad = entidadBaseEmpresas.MotivoBaja.Where(MB => MB.IdBaja == motivoBaja.IdBaja).FirstOrDefault();
                        if (motivoBajaEntidad != null)
                        {
                            var contratacionEntidad = entidadBaseSIRH.DetalleContratacion
                                .Where(DC => DC.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();
                            if (contratacionEntidad != null)
                            {
                                if (contratacionEntidad != null)
                                {
                                   
                                    DateTime thisDay = DateTime.Now;


                                    detNombramientoEntidad.ObsJornada = detNombramiento.ObsJornada + "\nMotivo de baja: " + motivoBajaEntidad.Descripcion;
                                    contratacionEntidad.MarcaAsistencia = false;
                                    datosEntidad.Estatus = 0;
                                    datosEntidad.FechaBaja = thisDay;
                                    datosEntidad.MotivoBaja = (short)motivoBajaEntidad.IdBaja;
                                   


                                    entidadBaseSIRH.SaveChanges();
                                    entidadBaseEmpresas.SaveChanges();
                               

                                    respuesta = new CRespuestaDTO
                                    {
                                        Codigo = 1,
                                        Contenido = datosEntidad
                                    };
                                    return respuesta;
                                   
                                }
                                else
                                {
                                    throw new Exception("El detalle de nombramiento del funcionario es inválido");
                                }
                            }
                            else
                            {
                                throw new Exception("El funcionario no tiene un nombramiento asociado");
                            }
                        }
                        else
                        {
                            throw new Exception("El motivo de baja es inválido");
                        }
                    }
                    else
                    {
                        throw new Exception("El detalle de contratación es inválido");
                    }*/
                    throw new Exception("Not implemented yet");
                }
                else
                {
                    throw new Exception("El empleado indicado no se encuentra registrado en la base de datos");
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

        private int ObtenerConsecutivo()
        {
            try
            {
                string maxAux = "";
                var datosEmpleados = (entidadBaseEmpresas.Empleado.Where(E => E.CodigoAcceso != "7777" && E.CodigoAcceso != "9997" && E.CodigoAcceso != "9996" && E.CodigoAcceso.Length == 4)).ToList();
                maxAux = datosEmpleados.Max(E => E.CodigoAcceso);
                bool aux = true;
                string codigoEmpleadoA = "";
                while (aux)
                {
                    codigoEmpleadoA = "";
                    var empleadoAux = entidadBaseEmpresas.Empleado.Where(E => E.CodigoAcceso == maxAux).FirstOrDefault();
                    if (empleadoAux.CodigoEmpleado.Length == 9)
                    {
                        if (empleadoAux.CodigoEmpleado[1] == '0')
                        {
                            string codigoEmpleadoAux = "00" + empleadoAux.CodigoEmpleado[0];
                            string ultimosDigitos = empleadoAux.CodigoEmpleado.Substring(2);
                            codigoEmpleadoAux = codigoEmpleadoAux + ultimosDigitos;
                            codigoEmpleadoA = codigoEmpleadoAux;
                        }
                        else
                        {
                            codigoEmpleadoA = "0" + empleadoAux.CodigoEmpleado;
                        }
                    }

                    var funcionarioAux = entidadBaseSIRH.Funcionario.Where(E => E.IdCedulaFuncionario == codigoEmpleadoA);
                    if (funcionarioAux.Count() < 1)
                    {
                        maxAux = datosEmpleados.Max(E => E.CodigoAcceso);
                        datosEmpleados = datosEmpleados.Where(E => E.CodigoAcceso != maxAux).ToList();
                    }
                    else if (funcionarioAux.Count() > 0)
                    {
                        aux = false;
                    }
                    else if (maxAux == "1620")
                    {
                        return 0;
                    }
                }
                if (Convert.ToInt32(maxAux) > 9999)
                {
                    return 0;
                }

                return Convert.ToInt32(maxAux) + 1;

            }
            catch (Exception e)
            {
                return 0;

            }
        }

        /// <summary>
        /// Lista los empleados dados de alta en el sistema
        /// </summary>
        /// <returns>Retorna la lista de empleados activos</returns>
        public CRespuestaDTO ListarEmpleadosActivos()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.Estatus == 1).ToList();

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
                    throw new Exception("No se encontraron empleados activos");
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
        /// Lista los empleados dados de baja en el sistema
        /// </summary>
        /// <returns>Retorna la lista de empleados inactivos</returns>
        public CRespuestaDTO ListarEmpleadosInactivos()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.Estatus == 0).ToList();

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
                    throw new Exception("No se encontraron empleados inactivos");
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
        /// Da de alta al empleado que recibe por parámetros
        /// </summary>
        /// <param name="funcionario">Entidad de funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">Entidad de empleado que se desea dar de alta</param>
        /// <param name="detNombramiento">Detalle del nombramiento del empleado</param>
        /// <returns>Retorna el empleado dado de alta</returns>
        public CRespuestaDTO ActivarEmpleado(Funcionario funcionario, Empleado empleado /*DetalleNombramiento detNombramiento*/)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var funcionarioEntidad = entidadBaseSIRH.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionarioEntidad != null)
                {
                    var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

                    if (datosEntidad != null)
                    {
                        var contratacionEntidad = entidadBaseSIRH.DetalleContratacion
                            .Where(DC => DC.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();
                        if (contratacionEntidad != null)
                        {

                            var nombramientoEntidad = entidadBaseSIRH.Nombramiento.Where(N => N.Funcionario.PK_Funcionario == funcionarioEntidad.PK_Funcionario).FirstOrDefault();

                            /*var detNombramientoEntidad = entidadBaseSIRH.DetalleNombramiento.Where(DN => DN.Nombramiento.PK_Nombramiento == nombramientoEntidad.PK_Nombramiento).FirstOrDefault();

                            if (detNombramientoEntidad != null)
                            {

                                detNombramientoEntidad.ObsJornada = detNombramiento.ObsJornada;
                                contratacionEntidad.MarcaAsistencia = true;
                                datosEntidad.Estatus = 1;
                                datosEntidad.MotivoBaja = 0;
                                datosEntidad.FechaBaja = DateTime.Now;
                                entidadBaseSIRH.SaveChanges();

                                entidadBaseEmpresas.SaveChanges();


                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = datosEntidad
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("El detalle de nombramiento del funcionario es inválido o no existe");
                            }*/
                            throw new Exception("Not implemented yet");
                        }
                        else
                        {
                            throw new Exception("El funcionario indicado no cuentra con una contratación asignada");
                        }
                    }
                    else
                    {
                        throw new Exception("El empleado indicado no se encuentra registrado en la base de datos del reloj marcador");
                    }
                }
                else
                {
                    throw new Exception("El funcionario indicado no se encuentra registrado en la base de datos");
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
        /// Realiza una búsqueda de empleados registrados en la BD del reloj marcador
        /// </summary>
        /// <param name="datosPrevios">Lista de empleados encontrados según algún filtro</param>
        /// <param name="parametro">Valor del filtro de búsqueda</param>
        /// <param name="elemento">Tipo de filtro</param>
        /// <returns>Retorna los empleados encontrados según el filtro especificado</returns>
        public CRespuestaDTO SearchEmpleados(List<Empleado> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro);
                if (datosPrevios.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPrevios
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Carga los empleados que corresponden con el filtro indicado
        /// </summary>
        /// <param name="datosPrevios">Lista de empleados encontrados según algún filtro</param>
        /// <param name="parametro">Valor del filtro de búsqueda</param>
        /// <param name="elemento">Tipo de filtro</param>
        /// <returns>Retorna los empleados encontrados según el filtro especificado</returns>
        private List<Empleado> CargarDatos(string elemento, List<Empleado> datosPrevios, object parametro)
        {
            string filtro = "";

            try
            {
                if (parametro.GetType().Name.Equals("String"))
                {
                    filtro = parametro.ToString();
                }
                else
                {
                    string auxx = parametro.ToString();
                    if (parametro.ToString() == "0")
                    {
                        filtro = "0";
                    }
                    else
                    {
                        filtro = "1";
                    }
                }

                if (datosPrevios.Count < 1)
                {
                    switch (elemento)
                    {
                        case "Cedula":
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                        .Where(E => E.CodigoEmpleado == filtro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        case "Estado":
                            short auxFiltro = Convert.ToSByte(filtro);
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                         .Where(EM => EM.Estatus == auxFiltro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        case "Nombre":
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                        .Where(EM => EM.Nombre1 == filtro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        case "Apellido1":
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                        .Where(EM => EM.Apellido1 == filtro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        case "Apellido2":
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                        .Where(EM => EM.Apellido2 == filtro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        case "Codigo":
                            datosPrevios = entidadBaseEmpresas.Empleado
                                                        .Where(EM => EM.CodigoAcceso == filtro).OrderBy(e => e.Nombre1).ToList();
                            break;
                        default:
                            datosPrevios = new List<Empleado>();
                            break;
                    }
                }
                else
                {
                    switch (elemento)
                    {
                        case "Cedula":
                            datosPrevios = datosPrevios.Where(E => E.CodigoEmpleado == filtro).ToList();
                            break;
                        case "Estado":
                            short auxFiltro = Convert.ToSByte(filtro);
                            datosPrevios = datosPrevios.Where(EM => EM.Estatus == auxFiltro).ToList();
                            break;
                        case "Nombre":
                            datosPrevios = datosPrevios.Where(E => E.Nombre1 == filtro).ToList();
                            break;
                        case "Apellido1":
                            datosPrevios = datosPrevios.Where(EM => EM.Apellido1 == filtro).ToList();
                            break;
                        case "Apellido2":
                            datosPrevios = datosPrevios.Where(EM => EM.Apellido2 == filtro).ToList();
                            break;
                        case "Codigo":
                            datosPrevios = datosPrevios.Where(EM => EM.CodigoAcceso == filtro).ToList();
                            break;
                        default:
                            datosPrevios = new List<Empleado>();
                            break;
                    }
                }

                return datosPrevios;
            }
            catch (Exception e)
            {
                return datosPrevios;
            }
        }

        /// <summary>
        /// Busca el empleado especificado en el parámetro
        /// </summary>
        /// <param name="empleado">Entidad que tiene definido el número de cédula del funcionario que se desea buscar</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public CRespuestaDTO BuscarEmpleado(Empleado empleado)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBaseEmpresas.Empleado.Where(E => E.CodigoEmpleado == empleado.CodigoEmpleado).FirstOrDefault();

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

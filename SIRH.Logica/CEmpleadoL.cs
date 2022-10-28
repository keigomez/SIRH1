using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.DatosMarcasReloj;

namespace SIRH.Logica
{
    public class CEmpleadoL
    {
        #region Variables

        SIRHEntities contextoSIRH;
        EmpresasDataDB1Entities contextoEmpresas;
        MasterTASEntities contextoMasterTAS;

        #endregion

        #region Constructor

        public CEmpleadoL()
        {
            contextoSIRH = new SIRHEntities();
            contextoEmpresas = new EmpresasDataDB1Entities();
            contextoMasterTAS = new MasterTASEntities();
        }
        #endregion

        #region Métodos

        internal static CEmpleadoDTO ConvertirDatosEmpleadoADTO(Empleado item)
        {
            return new CEmpleadoDTO
            {
                IdEntidad = Convert.ToInt32(item.CodigoEmpleado),
                CodigoEmpleado = item.CodigoEmpleado,
                PrimerNombre = item.Nombre1,
                SegundoNombre = item.Nombre2,
                ApellidoMaterno = item.Apellido2,
                ApellidoPaterno = item.Apellido1,
                Estado = item.Estatus,
                Digitos = item.CodigoAcceso
            };
        }

        /// <summary>
        /// Procesa los datos del empleado para después almacenarlo en la base de datos
        /// </summary>
        /// <param name="funcionario">DTO del funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">DTO del empleado que se desea almacenar</param>
        /// <returns>Retorna el empleado almacenado</returns>
        public CBaseDTO AgregarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {

                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                string cedulaTransformada = ConvertirCedula(funcionario.Cedula);

                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = cedulaTransformada,
                    TipoDocumento=0,
                    NoDocumento="",
                    Nombre1 = empleado.PrimerNombre,
                    Nombre2 = empleado.SegundoNombre,
                    Apellido1 = empleado.ApellidoPaterno,
                    Apellido2 = empleado.ApellidoMaterno,
                    ApellidoCasada = "",
                    Sexo="M",
                    FechaNacimiento=DateTime.Now,
                    Edad=0,
                    EstadoCivil=2,
                    FechaInicioLabores=DateTime.Now,
                    Area=49,
                    Puesto=62,
                    TipoHorario=1,
                    PoliticaEmpresa=1,
                    PeriodoNominal=0,
                    Extras=0,
                    Supervisor=0,
                    Permanente=1,
                    Fotografia =new byte[0],
                    MarcaSoloTExtra = 0,
                    NoRotacionesDescanso=0,
                    NoDiasDescanso=0,
                    NoRotacion=0,
                    SeguroSocial="",
                    IdTributaria="",
                    Salario=0,
                    FianzaPoliza="",
                    Probidad="",
                    Identificacion1="",
                    Identificacion2="",
                    MotivoBaja=0,
                    FechaBaja= DateTime.Now,
                    PuestoNominal="",
                    PreparacionAcademica="",
                    Colegiado="",
                    Direccion="",
                    Telefono="",
                    PartidaPresupuesto="",
                    Banco="",
                    CuentaBanco="",
                    Planilla=1,
                    Clasificacion=1,
                    Estatus = Convert.ToSByte(1)
                };

                var funcionarioEntidad = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                var respuestaAux = intermedio.AlmacenarEmpleado(funcionarioEntidad, empleadoEntidad);

                if (respuestaAux.Codigo<1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAux).Contenido;
                    throw new Exception();
                }
                else
                {
                    respuesta = ConvertirDatosEmpleadoADTO((Empleado)respuestaAux.Contenido);
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Da de baja al empleado que recibe por parámetros
        /// </summary>
        /// <param name="funcionario">DTO del funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">DTO del empleado que se desea dar de baja</param>
        /// <param name="detNombramiento">Detalle del nombramiento del empleado</param>
        /// <param name="motivoBaja">Id del motivo de baja</param>
        /// <returns>Retorna el empleado dado de baja</returns>
        public CBaseDTO DesactivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario, 
            CDetalleNombramientoDTO detalleNombramiento, CMotivoBajaDTO motivoBaja)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var funcionarioEntidad = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                var motivoBajaEntidad = new MotivoBaja
                {
                    IdBaja = motivoBaja.IdEntidad
                };

                /*var detalleNombramientoEntidad = new DetalleNombramiento
                {
                    ObsJornada = detalleNombramiento.ObservacionesTipoJornada
                };*/

                var respuestaAux = intermedio.DesactivarEmpleado(funcionarioEntidad, empleadoEntidad/*, detalleNombramientoEntidad*/, motivoBajaEntidad);

                if (respuestaAux.Codigo<1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAux).Contenido;
                    throw new Exception();
                }
                else
                {
                    respuesta = ConvertirDatosEmpleadoADTO((Empleado)respuestaAux.Contenido);
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }


        public List<CBaseDTO> BuscarEmpleadoActivo(CEmpleadoDTO empleado)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var respuestaAux = intermedio.BuscarEmpleadoActivo(empleadoEntidad);

                if (respuestaAux.Codigo > 0)
                {
                    var empleadoAuxiliar = ConvertirDatosEmpleadoADTO((Empleado)respuestaAux.Contenido);

                    respuesta.Add(empleadoAuxiliar);
                    return respuesta;
                }
                else 
                {
                    respuesta.Add(((CErrorDTO)respuestaAux.Contenido));
                    throw new Exception();
                }
                
            }

            catch (Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Lista los empleados dados de alta en el sistema
        /// </summary>
        /// <returns>Retorna la lista de empleados activos</returns>
        public List<CBaseDTO> ListarEmpleadosActivos()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                var empleadosEncontrados = intermedio.ListarEmpleadosActivos();

                if (empleadosEncontrados.Codigo > 0)
                {
                    foreach (var empleado in (List<Empleado>)empleadosEncontrados.Contenido)
                    {


                        var datoEmpleado = ConvertirDatosEmpleadoADTO(empleado);
                        resultado.Add(datoEmpleado);

                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)empleadosEncontrados.Contenido);

                }

            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }

        /// <summary>
        /// Lista los empleados dados de baja en el sistema
        /// </summary>
        /// <returns>Retorna la lista de empleados inactivos</returns>
        public List<CBaseDTO> ListarEmpleadosInactivos()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                var empleadosEncontrados = intermedio.ListarEmpleadosInactivos();

                if (empleadosEncontrados.Codigo > 0)
                {
                    foreach (var empleado in (List<Empleado>)empleadosEncontrados.Contenido)
                    {


                        var datoEmpleado = ConvertirDatosEmpleadoADTO(empleado);
                        resultado.Add(datoEmpleado);

                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)empleadosEncontrados.Contenido);

                }

            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }

        /// <summary>
        /// Da de alta al empleado que recibe por parámetros
        /// </summary>
        /// <param name="funcionario">DTO del funcionario para obtener el detalle de contratación en SIRH</param>
        /// <param name="empleado">DTO del empleado que se desea dar de alta</param>
        /// <param name="detNombramiento">Detalle del nombramiento del empleado</param>
        /// <returns>Retorna el empleado dado de alta</returns>
        public CBaseDTO ActivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario, CDetalleNombramientoDTO detalleNombramiento)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var funcionarioEntidad = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                /*var detalleNombramientoEntidad = new DetalleNombramiento
                {
                    ObsJornada = detalleNombramiento.ObservacionesTipoJornada
                };*/

                var respuestaAux = intermedio.ActivarEmpleado(funcionarioEntidad, empleadoEntidad/*, detalleNombramientoEntidad*/);

                if (respuestaAux.Codigo<1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAux).Contenido;
                    throw new Exception();
                }
                else
                {
                    respuesta = ConvertirDatosEmpleadoADTO((Empleado)respuestaAux.Contenido);
                    return respuesta;
                }
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }


        private string ConvertirCedula(string cedula) {
            if (cedula[1] == '0')
            {
                string codigoEmpleadoAux = cedula[2]+"0";
                string ultimosDigitos = cedula.Substring(3);
                cedula = codigoEmpleadoAux + ultimosDigitos;
                return cedula;
            }
            else
            {
                cedula = cedula.Substring(1);
                return cedula;
            }
        }


        private string ConvertirCedulaSIRH(string cedula)
        {
            if (cedula[1] == '0')
            {
                string codigoEmpleadoAux = cedula[0]+"";
                string ultimosDigitos = cedula.Substring(2);
                cedula = codigoEmpleadoAux + ultimosDigitos;
                return "00"+cedula;
            }
            else
            {
                cedula = "0"+cedula;
                return cedula;
            }
        }

        /// <summary>
        /// Realiza una búsqueda de empleados registrados en la BD del reloj marcador
        /// </summary>
        /// <param name="empleado">Empleado que se desea buscar</param>
        /// <param name="empleado">Filtros de búsqueda</param>
        /// <returns>Retorna los empleados encontrados según el filtro especificado</returns>
        public List<CBaseDTO> SearchEmpleado(CEmpleadoDTO empleado,
                                               List<string> argumentos)
        {
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();

                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);

                List<Empleado> datosTramites = new List<Empleado>();

                if (!string.IsNullOrEmpty(empleado.CodigoEmpleado))
                {

                    empleado.CodigoEmpleado = ConvertirCedula(empleado.CodigoEmpleado);
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.CodigoEmpleado, "Cedula"));

                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                        if (datosTramites.Count < 1) {
                            datosTramites = new List<Empleado>();
                            throw new Exception("No se encontraron resultados para los parámetros especificados.");
                        }
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                        throw new Exception("No se encontraron resultados para los parámetros especificados.");
                    }
                }

                if (!string.IsNullOrEmpty(empleado.Digitos))
                {
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.Digitos, "Codigo"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                        if (datosTramites.Count < 1)
                        {
                            datosTramites = new List<Empleado>();
                            throw new Exception("No se encontraron resultados para los parámetros especificados.");
                        }
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                    }
                }

                if (empleado.Estado > -1)
                {
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.Estado, "Estado"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                    }
                }

                if (!string.IsNullOrEmpty(empleado.PrimerNombre))
                {
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.PrimerNombre, "Nombre"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                    }
                }

                if (!string.IsNullOrEmpty(empleado.ApellidoPaterno))
                {
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.ApellidoPaterno, "Apellido1"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                    }
                }

                if (!string.IsNullOrEmpty(empleado.ApellidoMaterno))
                {
                    var resultado = ((CRespuestaDTO)intermedio.SearchEmpleados(datosTramites, empleado.ApellidoMaterno, "Apellido2"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Empleado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<Empleado>();
                    }
                }

                if (datosTramites.Count > 0)
                {
                    foreach (var item in datosTramites)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();

                        var datoEmpleado = ConvertirDatosEmpleadoADTO(item);
                       
                        CEmpleadoDTO tempPago = datoEmpleado;

                        if (tempPago.CodigoEmpleado.Length > 5)
                        {
                            tempPago.CodigoEmpleado = ConvertirCedulaSIRH(tempPago.CodigoEmpleado);
                        }

                        if (tempPago.CodigoEmpleado.Length < 6 || tempPago.CodigoEmpleado.Length > 9)
                        {
                            respuesta.Add(tempPago);
                        }
                    }
                }
                else
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                    respuesta=temp;
                }

                return respuesta;
            }
            catch (Exception e)
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                return respuesta;
            }
        }

        /// <summary>
        /// Busca el empleado especificado en el parámetro
        /// </summary>
        /// <param name="empleado">DTO que tiene definido el número de cédula del funcionario que se desea buscar</param>
        /// <returns>Retorna el empleado indicado por parámetros</returns>
        public List<CBaseDTO> BuscarEmpleado(CEmpleadoDTO empleado)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEmpleadoD intermedio = new CEmpleadoD(contextoEmpresas, contextoMasterTAS, contextoSIRH);
                var empleadoEntidad = new Empleado
                {
                    CodigoEmpleado = empleado.CodigoEmpleado
                };

                var respuestaAux = intermedio.BuscarEmpleado(empleadoEntidad);

                if (respuestaAux.Codigo > 0)
                {
                    var empleadoAuxiliar = ConvertirDatosEmpleadoADTO((Empleado)respuestaAux.Contenido);

                    respuesta.Add(empleadoAuxiliar);
                    return respuesta;
                }
                else
                {
                    respuesta.Add(((CErrorDTO)respuestaAux.Contenido));
                    throw new Exception();
                }
            }

            catch (Exception e)
            {
                return respuesta;
            }
        }


        #endregion
    }
}

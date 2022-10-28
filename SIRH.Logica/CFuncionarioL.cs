using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CFuncionarioL
    {
        #region Variables

        SIRHEntities contexto;
        CFuncionarioD funcDescarga;
        VariablesGlobales var;

        #endregion

        public CFuncionarioL()
        {
            contexto = new SIRHEntities();
        }

        internal static CFuncionarioDTO ConvertirDatosFuncionarioADTO(Funcionario item)
        {
            return new CFuncionarioDTO
            {
                IdEntidad = item.PK_Funcionario,
                Cedula = item.IdCedulaFuncionario,
                Nombre = item.NomFuncionario,
                PrimerApellido = item.NomPrimerApellido,
                SegundoApellido = item.NomSegundoApellido,
                Sexo = item.IndSexo != "" ? GeneroEnum.Indefinido : (GeneroEnum)Convert.ToInt32(item.IndSexo)
            };
        }

        //28/11/2016....Internal Static CFormacionAcademicaDTO.....
        internal static CFormacionAcademicaDTO ConvertirDatosFormacionAcademicaADTO(FormacionAcademica item)
        {
            return new CFormacionAcademicaDTO
            {
                IdEntidad = item.PK_FormacionAcademica,
                Fecha = Convert.ToDateTime(item.FecRegistro)
            };
        }

        internal static CFuncionarioDTO FuncionarioGeneral(Funcionario item)
        {
            CFuncionarioDTO respuesta = new CFuncionarioDTO
            {
                IdEntidad = item.PK_Funcionario,
                Cedula = item.IdCedulaFuncionario,
                FechaNacimiento = Convert.ToDateTime(item.FecNacimiento),
                Nombre = item.NomFuncionario,
                PrimerApellido = item.NomPrimerApellido,
                SegundoApellido = item.NomSegundoApellido,
                Sexo = (GeneroEnum)Convert.ToInt32(item.IndSexo),
            };

            if (item.EstadoFuncionario != null)
            {
                respuesta.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                };
            }
            else
            {
                respuesta.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    Mensaje = "El funcionario no tiene un estado establecido"
                };
            }
            return respuesta;
        }

        //INSERT DE DATOS PERSONALES DE FUNCIONARIO...23/11/2016            OBJETOS QUE vienen desde la web...   
        //INSERTADO EN ICFuncionarioService y CFuncionarioService el 25/01/2017  

        public List<CBaseDTO> BuscarFuncionarioProfesional(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            var datosDB = intermedio.BuscarFuncionarioProfesional(cedula);
            if (datosDB.Codigo != -1)
            {
                Funcionario dato = (Funcionario)datosDB.Contenido;
                CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                respuesta.Add(funcionario);

                Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false).FirstOrDefault(); ;

                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                respuesta.Add(puesto);

                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                detallePuesto.Clase = new CClaseDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                    DesClase = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase
                };

                detallePuesto.Especialidad = new CEspecialidadDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                    DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                };

                if (datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }

                respuesta.Add(detallePuesto);

                var datoDetalle = CDetalleContratacionL.ConvertirDetalleContratacionADTO
                                    (dato.DetalleContratacion.Where(Q => Q.FecCese == null)
                                    .FirstOrDefault());

                if (datoDetalle != null)
                {
                    respuesta.Add(datoDetalle);
                }
                else
                {
                    respuesta.Add(null);
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)datosDB.Contenido);
            }

            return respuesta;
        }

        public List<CBaseDTO> BuscarFuncionarioPolicial(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            var datosDB = intermedio.BuscarFuncionarioPolicial(cedula);
            if (datosDB.Codigo != -1)
            {
                Funcionario dato = (Funcionario)datosDB.Contenido;
                CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                respuesta.Add(funcionario);

                Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false).FirstOrDefault(); ;

                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                respuesta.Add(puesto);

                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                detallePuesto.Clase = new CClaseDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                    DesClase = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase
                };

                detallePuesto.Especialidad = new CEspecialidadDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                    DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                };

                if (datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }

                respuesta.Add(detallePuesto);

                var datoDetalle = CDetalleContratacionL.ConvertirDetalleContratacionADTO
                                    (dato.DetalleContratacion.Where(Q => Q.FecCese == null)
                                    .FirstOrDefault());

                if (datoDetalle != null)
                {
                    respuesta.Add(datoDetalle);
                }
                else
                {
                    respuesta.Add(null);
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)datosDB.Contenido);
            }

            return respuesta;
        }

        public List<CBaseDTO> GuardarDatosPersonalesFuncionario(CFuncionarioDTO funcionario,
                                                               CHistorialEstadoCivilDTO estadoCivil,
                                                               List<CInformacionContactoDTO> informacion,
                                                               CDireccionDTO direccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                Funcionario funcionarioEC = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula,
                    NomFuncionario = funcionario.Nombre.Length > 15 ? funcionario.Nombre.Substring(0,15) : funcionario.Nombre,
                    NomPrimerApellido = funcionario.PrimerApellido,
                    NomSegundoApellido = funcionario.SegundoApellido,
                    FecNacimiento = Convert.ToDateTime(funcionario.FechaNacimiento),
                    IndSexo = Convert.ToString(funcionario.Sexo) == "Masculino" ? "1" : "2",
                    EstadoFuncionario = contexto.EstadoFuncionario.FirstOrDefault(Q => Q.PK_EstadoFuncionario == 20)
                };
                var datos = intermedio.GuardarDatosPersonalesFuncionario(funcionarioEC);
                //si se insertó funcionario diferente de ERRORES....
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                //ENTONCES...INSERTE HISTORIAL ESTADO CIVIL....
                {
                    CHistorialEstadoCivilD intermedioEstadoCivil = new CHistorialEstadoCivilD(contexto);
                    estadoCivil.FecIncio = DateTime.Now;
                    estadoCivil.FecFin = DateTime.Now;
                    HistorialEstadoCivil historialEC = new HistorialEstadoCivil
                    {
                        FecInicio = estadoCivil.FecIncio,
                        FecFin = estadoCivil.FecFin,
                        CatEstadoCivil = contexto.CatEstadoCivil.Where(Q => Q.DesEstadoCivil.ToLower() == estadoCivil.CatEstadoCivil.DesEstadoCivil.ToLower()).FirstOrDefault(),
                        Funcionario = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario == funcionario.Cedula).FirstOrDefault()
                    };
                    //aquí pongo el nombre del OBJETO que tengo en el parentesis del método de CHistorialEstadoCivilD y la cedula, mas el historialestadocivil de datos
                    var datosHistorialEC = intermedioEstadoCivil.GuardarHistorialEstadoCivil(funcionario.Cedula, historialEC);
                    if (datosHistorialEC.Contenido.GetType() != typeof(CErrorDTO))
                    {
                        //SI inserta bien los datos, a respuesta se agrega estadoCivil
                        respuesta.Add(estadoCivil);
                    }
                    else
                    {
                        respuesta.Add((CErrorDTO)datos.Contenido);
                    }

                    foreach (var item in informacion)
                    {
                        if (item.DesContenido != null)
                        {
                            CInformacionContactoD intermedioInfoContacto = new CInformacionContactoD(contexto);
                            InformacionContacto informacContacto = new InformacionContacto
                            {
                                DesAdicional = item.DesAdicional,
                                DesContenido = item.DesContenido,
                                TipoContacto = contexto.TipoContacto.Where(Q => Q.PK_TipoContacto == item.TipoContacto.IdEntidad).FirstOrDefault(),
                                Funcionario = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario == funcionario.Cedula).FirstOrDefault()
                            };
                            var datosInformacion = intermedioInfoContacto.GuardarInformacionContacto(funcionario.Cedula, informacContacto);
                            if (datosInformacion.Contenido.GetType() != typeof(CErrorDTO))
                            {
                                //SI inserta bien los datos, a respuesta se agrega información
                                respuesta.Add(item);
                            }
                            else
                            {
                                //AQUI VA SOLO EL OBJETO: ((CErrorDTO)datos.Contenido)
                                respuesta.Add((CErrorDTO)datos.Contenido);
                            }
                        }
                    }

                    //Ahora inserta los datos de la dirección

                    CDireccionD intermedioDireccion = new CDireccionD(contexto);
                    var distrito = new Distrito();

                    if (direccion.Distrito.IdEntidad > 0)
                    {
                        distrito = contexto.Distrito.Where(Q => Q.PK_Distrito == direccion.Distrito.IdEntidad).FirstOrDefault();
                    }
                    else
                    {
                        var datosUbicacion = direccion.Distrito.Mensaje.Split('-');
                        var datoProvincia = datosUbicacion[0];
                        var datoCanton = datosUbicacion[1];
                        var datoDistrito = direccion.Distrito.NomDistrito;

                        distrito = contexto.Distrito.Where(Q => Q.NomDistrito == datoDistrito &&
                                                                Q.Canton.NomCanton == datoCanton &&
                                                                Q.Canton.Provincia.NomProvincia == datoProvincia).FirstOrDefault();
                    }

                    Direccion dir = new Direccion
                    {
                        DirExacta = direccion.DirExacta,
                        FK_Distrito = distrito.PK_Distrito,
                        Funcionario = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario == funcionario.Cedula).FirstOrDefault()
                    };

                    var resultado = intermedioDireccion.GuardarDireccion(dir);

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                //es una nueva lista con un solo elemento en este caso es el Error
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }
        }

        public List<CFuncionarioDTO> BusquedaFuncionarioLogica(string cedula, string nombre, string primerApellido, string segundoApellido)
        {
            List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();

            Funcionario funcbusca =
                new Funcionario
                {
                    IdCedulaFuncionario = cedula,
                    NomFuncionario = nombre,
                    NomPrimerApellido = primerApellido,
                    NomSegundoApellido = segundoApellido
                };
            CFuncionarioD funcionarioResultado = new CFuncionarioD(contexto);

            var funcionarioBD = funcionarioResultado.BuscarFuncionario(funcbusca).Distinct().ToList();

            foreach (var item in funcionarioBD)
            {
                CFuncionarioDTO temp = new CFuncionarioDTO();
                temp.IdEntidad = item.PK_Funcionario;
                temp.Cedula = item.IdCedulaFuncionario;
                temp.Nombre = item.NomFuncionario;
                temp.PrimerApellido = item.NomPrimerApellido;
                temp.SegundoApellido = item.NomSegundoApellido;
                temp.Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino;
                temp.FechaNacimiento = Convert.ToDateTime(item.FecNacimiento);
                temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                };
                resultado.Add(temp);
            }

            return resultado;
        }

        /// <summary>
        /// [0] Funcionario
        /// [1] Estado Funcionario
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public CFuncionarioDTO DescargarFuncionario(string cedula)
        {
            CFuncionarioDTO resultado = new CFuncionarioDTO();

            funcDescarga = new CFuncionarioD(contexto);
            Funcionario temp = funcDescarga.BuscarFuncionarioCedula(cedula);

            resultado.Cedula = temp.IdCedulaFuncionario;
            resultado.Nombre = temp.NomFuncionario;
            resultado.PrimerApellido = temp.NomPrimerApellido;
            resultado.SegundoApellido = temp.NomSegundoApellido;
            resultado.Sexo = (GeneroEnum)Convert.ToInt32(temp.IndSexo);
            resultado.FechaNacimiento = Convert.ToDateTime(temp.FecNacimiento);
            resultado.EstadoFuncionario = new CEstadoFuncionarioDTO { IdEntidad = temp.EstadoFuncionario.PK_EstadoFuncionario,
                DesEstadoFuncionario = temp.EstadoFuncionario.DesEstadoFuncionario };

            return resultado;
        }

        public List<CFuncionarioDTO> EnviarFuncionarioDetallePuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal)
        {
            List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();

            CFuncionarioD datos = new CFuncionarioD(contexto);

            var item = datos.BuscarFuncionarioDetallePuesto(codPuesto, codClase, codEspecialidad, codOcupacionReal);

            foreach (var aux in item)
            {
                CFuncionarioDTO temp = new CFuncionarioDTO();

                temp.Cedula = aux.IdCedulaFuncionario;
                temp.Nombre = aux.NomFuncionario;
                temp.PrimerApellido = aux.NomPrimerApellido;
                temp.SegundoApellido = aux.NomSegundoApellido;
                temp.Sexo = (aux.IndSexo == "1" || aux.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(aux.IndSexo) : GeneroEnum.Masculino;
                temp.FechaNacimiento = Convert.ToDateTime(aux.FecNacimiento);
                temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = aux.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = aux.EstadoFuncionario.DesEstadoFuncionario
                };

                resultado.Add(temp);
            }

            return resultado;
        }

        public List<CFuncionarioDTO> EnviarFuncionarioUbicacion(int codDivision, int codDireccion, int codDepartamento, int codSeccion, string codPresupuesto)
        {
            List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();

            CFuncionarioD datos = new CFuncionarioD(contexto);

            var item = datos.BuscarFuncionarioUbicacion(codDivision, codDireccion, codDepartamento, codSeccion, codPresupuesto);

            foreach (var aux in item)
            {
                CFuncionarioDTO temp = new CFuncionarioDTO();

                temp.Cedula = aux.IdCedulaFuncionario;
                temp.Nombre = aux.NomFuncionario;
                temp.PrimerApellido = aux.NomPrimerApellido;
                temp.SegundoApellido = aux.NomSegundoApellido;
                temp.Sexo = (aux.IndSexo == "1" || aux.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(aux.IndSexo) : GeneroEnum.Masculino;
                temp.FechaNacimiento = Convert.ToDateTime(aux.FecNacimiento);
                temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = aux.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = aux.EstadoFuncionario.DesEstadoFuncionario
                };

                resultado.Add(temp);
            }

            return resultado;
        }

        public CFuncionarioDTO EnviarDatosPersonales(string nombre, string apellido1, string apellido2)
        {
            CFuncionarioDTO resultado = new CFuncionarioDTO();

            CFuncionarioD datos = new CFuncionarioD(contexto);

            var temp = datos.BuscarFuncionarioNombre(nombre, apellido1, apellido2);

            if (temp != null)
            {
                if (temp.DetalleAcceso.FirstOrDefault() != null)
                {
                    resultado.Mensaje = "Existente";
                }
                else
                {
                    resultado.Cedula = temp.IdCedulaFuncionario;
                }
            }

            return resultado;
        }

        public List<CFuncionarioDTO> BusquedaGeneralFuncionarios(List<string> palabras)
        {
            List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();

            CFuncionarioD datos = new CFuncionarioD(contexto);

            var item = datos.BusquedaGeneralFuncionarios(palabras);

            foreach (var aux in item)
            {
                CFuncionarioDTO temp = new CFuncionarioDTO();

                temp.Cedula = aux.IdCedulaFuncionario;
                temp.Nombre = aux.NomFuncionario;
                temp.PrimerApellido = aux.NomPrimerApellido;
                temp.SegundoApellido = aux.NomSegundoApellido;
                temp.Sexo = (aux.IndSexo == "1" || aux.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(aux.IndSexo) : GeneroEnum.Masculino;
                temp.FechaNacimiento = Convert.ToDateTime(aux.FecNacimiento);
                temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = aux.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = aux.EstadoFuncionario.DesEstadoFuncionario
                };

                resultado.Add(temp);
            }

            return resultado;
        }

        public List<CBaseDTO> BuscarFuncionarioPorCodigoPuesto(string codPuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CFuncionarioD consulta = new CFuncionarioD(contexto);

            var dato = consulta.BuscarFuncionarioPorCodigoPuesto(codPuesto);

            CFuncionarioDTO funcionario = ConstruirFuncionario(dato);

            respuesta.Add(funcionario);

            var datoNombramiento = dato.Nombramiento.FirstOrDefault();

            CNombramientoDTO nombramiento = ConstruirNombramiento(datoNombramiento);

            var datoDetalle = dato.DetalleContratacion.FirstOrDefault();

            CDetalleContratacionDTO detalle = ConstruirDetalleContratacion(datoDetalle);

            respuesta.Add(nombramiento);

            respuesta.Add(detalle);

            return respuesta;
        }

        private static CDetalleContratacionDTO ConstruirDetalleContratacion(DetalleContratacion datoDetalle)
        {
            CDetalleContratacionDTO detalle = new CDetalleContratacionDTO
            {
                IdEntidad = datoDetalle.PK_DetalleContratacion,
                FechaIngreso = Convert.ToDateTime(datoDetalle.FecIngreso),
                FechaCese = Convert.ToDateTime(datoDetalle.FecCese),
                FechaMesAumento = datoDetalle.FecMesAumento,
                NumeroAnniosServicioPublico = Convert.ToInt32(datoDetalle.NumAnniosServicioPublico),
                NumeroAnualidades = Convert.ToInt32(datoDetalle.NumAnualidades)
            };
            return detalle;
        }

        private static CCatEstadoCivilDTO ConstruirEstadoCivil(CatEstadoCivil estadoCivil)
        {
            CCatEstadoCivilDTO estadoC = new CCatEstadoCivilDTO
            {
                IdEntidad = estadoCivil.PK_CatEstadoCivil,
                DesEstadoCivil = estadoCivil.DesEstadoCivil
            };
            return estadoC;
        }

        private static CNombramientoDTO ConstruirNombramiento(Nombramiento datoNombramiento)
        {
            CNombramientoDTO nombramiento = new CNombramientoDTO
            {
                IdEntidad = datoNombramiento.PK_Nombramiento,
                FecNombramiento = Convert.ToDateTime(datoNombramiento.FecNombramiento),
                FecRige = Convert.ToDateTime(datoNombramiento.FecRige),
                FecVence = Convert.ToDateTime(datoNombramiento.FecVence),
                EstadoNombramiento = new CEstadoNombramientoDTO
                {
                    IdEntidad = datoNombramiento.EstadoNombramiento.PK_EstadoNombramiento,
                    DesEstado = datoNombramiento.EstadoNombramiento.DesEstado
                }
            };
            return nombramiento;
        }

        private static CFuncionarioDTO ConstruirFuncionario(Funcionario dato)
        {
            CFuncionarioDTO funcionario = new CFuncionarioDTO
            {
                IdEntidad = dato.PK_Funcionario,
                Cedula = dato.IdCedulaFuncionario,
                Nombre = dato.NomFuncionario,
                PrimerApellido = dato.NomPrimerApellido,
                SegundoApellido = dato.NomSegundoApellido,
                FechaNacimiento = Convert.ToDateTime(dato.FecNacimiento),
                Sexo = (dato.IndSexo == "1" || dato.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(dato.IndSexo) : GeneroEnum.Indefinido,
                EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = dato.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = dato.EstadoFuncionario.DesEstadoFuncionario
                }
            };
            return funcionario;
        }

        public List<CBaseDTO> BuscarFuncionarioNombramiento(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CFuncionarioD consulta = new CFuncionarioD(contexto);

            var dato = consulta.BuscarFuncionarioNombramiento(cedula);

            CFuncionarioDTO funcionario = ConstruirFuncionario(dato);

            var datoNombramiento = dato.Nombramiento.FirstOrDefault();

            CNombramientoDTO nombramiento = ConstruirNombramiento(datoNombramiento);

            var datoDetalle = dato.DetalleContratacion.FirstOrDefault();

            CDetalleContratacionDTO detalle = ConstruirDetalleContratacion(datoDetalle);

            respuesta.Add(funcionario);

            respuesta.Add(nombramiento);

            respuesta.Add(detalle);

            return respuesta;
        }

        public List<CBaseDTO> BuscarFuncionarioDesgloceSalarial(string cedula, string periodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                string pBase = CRegistroTEL.FormatearPeriodoBase(periodo);
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                CRespuestaDTO resultado = intermedio.BuscarFuncionarioDesgloceSalarial(cedula);
                if (resultado.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
                Funcionario dato = (Funcionario)resultado.Contenido;
                if (dato.EstadoFuncionario?.PK_EstadoFuncionario != (int)EstadosFuncionario.Activo)
                {
                    throw new Exception("El funcionario no se encuentra activo, por lo que no puede registrarle extras");
                }
                CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                respuesta.Add(funcionario);
                int mes = Convert.ToInt32(pBase.Substring(0, 2));
                int annio = Convert.ToInt32(pBase.Substring(2, 4));
                DateTime inicioExtras = new DateTime(annio, mes, 1);
                DateTime segundasExtras = inicioExtras.AddDays(1);
                var datoNombramiento = dato.Nombramiento.Where(Q => Q.DesgloseSalarial.Where(P => P.IndPeriodo == inicioExtras || P.IndPeriodo == segundasExtras).Count() > 0).FirstOrDefault();
                //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => (N.FecVence.HasValue == false || N.FecVence >= inicioExtras) 
                //                                                        && N.FecRige <= inicioExtras
                //                                                        && N.DesgloseSalarial.Where(P => P.IndPeriodo == inicioExtras 
                //                                                       /* || P.IndPeriodo == segundasExtras*/).Count() > 0).OrderByDescending(N => N.FecRige).FirstOrDefault();
                if (datoNombramiento == null)
                {
                    throw new Exception("No se ha encontrado un nombramiento valido en el periodo " + periodo + " para el funcionario");
                }
                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                respuesta.Add(puesto);

                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                if (datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad != null)
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad.DesEspecialidad
                    };
                }
                else
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        DesEspecialidad = "NO TIENE"
                    };
                }


                detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.PK_Escala,
                    SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                    MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.MtoAumento)
                };
                if (datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase != null)
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase.PK_Clase,
                        DesClase = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase.DesClase,
                    };
                }
                else
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        DesClase = "NO TIENE"
                    };
                }
                if (datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }

                respuesta.Add(detallePuesto);

                DateTime periodoPago = Convert.ToDateTime(CRegistroTEL.DefinirDesglocePago(Convert.ToDateTime("1/" + pBase.Substring(0, 2) + "/" + pBase.Substring(2))));
                DesgloseSalarial desgloseDatos = datoNombramiento.DesgloseSalarial?.FirstOrDefault(D => Convert.ToDateTime(D.IndPeriodo) == periodoPago);
                CDesgloseSalarialDTO desglose1 = desgloseDatos != null ? CDesgloseSalarialL.DesgloseSalarialGeneral(desgloseDatos) : null;
                periodoPago = Convert.ToDateTime(CRegistroTEL.DefinirDesglocePago(Convert.ToDateTime("16/" + pBase.Substring(0, 2) + "/" + pBase.Substring(2))));
                desgloseDatos = datoNombramiento.DesgloseSalarial?.FirstOrDefault(D => Convert.ToDateTime(D.IndPeriodo) == periodoPago);
                CDesgloseSalarialDTO desglose2 = desgloseDatos != null ? CDesgloseSalarialL.DesgloseSalarialGeneral(desgloseDatos) : null;
                respuesta.Add(desglose1);
                respuesta.Add(desglose2);
                if (desglose1 == null || desglose2 == null)
                {
                    throw new Exception("Ha ocurrido un error al obtener el salario, contacte con el encargado.");
                }

                CNombramientoDTO nombramiento = new CNombramientoDTO
                {
                    IdEntidad = datoNombramiento.PK_Nombramiento,
                    FecRige = Convert.ToDateTime(datoNombramiento.FecRige),
                    FecVence = Convert.ToDateTime(datoNombramiento.FecVence)
                };
                respuesta.Add(nombramiento);
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO BuscarFuncionarioBase(string cedula)
        {
            CBaseDTO respuesta;
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.BuscarFuncionarioBase(cedula);
                if (resultado.Codigo > 0)
                {
                    respuesta = FuncionarioGeneral((Funcionario)resultado.Contenido);
                    return respuesta;
                }
                else
                {
                    respuesta = (CErrorDTO)resultado.Contenido;
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        public CBaseDTO BuscarFuncionarioNuevo(string cedula)
        {
            CBaseDTO respuesta;
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = contexto.Funcionario.Where(F => F.IdCedulaFuncionario == cedula).FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = FuncionarioGeneral((Funcionario)resultado);
                    return respuesta;
                }
                else
                {
                    respuesta = new CErrorDTO { MensajeError = "No se encontró ningún funcionario asociado a la cédula indicada" };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        internal List<CBaseDTO> DevolverDTOSExfuncionario(C_EMU_EXFUNCIONARIOS item)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            respuesta.Add(new CFuncionarioDTO
            {
                Cedula = item.CEDULA,
                PrimerApellido = item.PRIMER_APELLIDO.TrimEnd(),
                SegundoApellido = item.SEGUNDO_APELLIDO.TrimEnd(),
                EstadoFuncionario = new CEstadoFuncionarioDTO { IdEntidad = 1 },
                FechaNacimiento = Convert.ToDateTime(item.FECHA_CUMPLE),
                Nombre = item.NOMBRE.TrimEnd(),
                Sexo = (GeneroEnum)Convert.ToInt32(item.SEXO)
            });

            int estadocivil = Convert.ToInt32(item.ESTADO_CIVIL);

            respuesta.Add(new CHistorialEstadoCivilDTO
            {
                CatEstadoCivil = new CCatEstadoCivilDTO { DesEstadoCivil = contexto.CatEstadoCivil.Where(
                                                                            Q => Q.PK_CatEstadoCivil == estadocivil)
                                                                            .FirstOrDefault() != null ?
                                                                            contexto.CatEstadoCivil.Where(
                                                                            Q => Q.PK_CatEstadoCivil == estadocivil)
                                                                            .FirstOrDefault().DesEstadoCivil : ""
                                                        }
            });

            respuesta.Add(new CInformacionContactoDTO
            {
                DesContenido = ""
            });

            int provincia = Convert.ToInt32(item.PROVINCIA_D);

            var datoDistrito = contexto.Distrito.Where(D => D.CodPostalDistrito == item.DISTRITO &&
                                                            D.Canton.CodPostalCanton == item.CANTON_D &&
                                                            D.Canton.Provincia.PK_Provincia == provincia).FirstOrDefault();

            respuesta.Add(new CDireccionDTO
            {
                Distrito = new CDistritoDTO { IdEntidad = datoDistrito.PK_Distrito,
                                              NomDistrito = datoDistrito.NomDistrito,
                    Canton = new CCantonDTO { IdEntidad = datoDistrito.Canton.PK_Canton,
                                              NomCanton = datoDistrito.Canton.NomCanton,
                        Provincia = new CProvinciaDTO { IdEntidad = provincia,
                                                        NomProvincia = datoDistrito.Canton.Provincia.NomProvincia
                                                    }
                                              }
                },
                DirExacta = item.SENAS
            });

            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarFuncionarioOferente(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.BuscarFuncionarioOferente(cedula);
                if (resultado.Codigo > 0)
                {
                    var resultados = DevolverDTOSExfuncionario((C_EMU_EXFUNCIONARIOS)resultado.Contenido);

                    if (resultados.Count > 0)
                    {
                        respuesta.Add(new List<CBaseDTO> { resultados.ElementAt(0) });
                        respuesta.Add(new List<CBaseDTO> { resultados.ElementAt(1) });
                        respuesta.Add(new List<CBaseDTO> { resultados.ElementAt(2) });
                        respuesta.Add(new List<CBaseDTO>());
                        respuesta.Add(new List<CBaseDTO>());
                        respuesta.Add(new List<CBaseDTO> { resultados.ElementAt(3) });
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = "No se encontraron resultados suficientes para registrar al funcionario desde los registros históricos del Ministerio" } });
                        return respuesta;
                    }

                    //respuesta.Add(new List<CBaseDTO> { FuncionarioGeneral((Funcionario)resultado.Contenido) });


                    //respuesta.Add(new List<CBaseDTO> { CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(((Funcionario)resultado.Contenido).HistorialEstadoCivil.LastOrDefault()) });

                    //var contactos = ((Funcionario)resultado.Contenido).InformacionContacto;
                    //if (contactos.Count > 0)
                    //{
                    //    List<CBaseDTO> contactosData = new List<CBaseDTO>();
                    //    foreach (var item in contactos)
                    //    {
                    //        contactosData.Add(CInformacionContactoL.ConvertirInformacionContactoADTO(item));
                    //    }
                    //    respuesta.Add(contactosData);
                    //}
                    //else
                    //{
                    //    respuesta.Add(new List<CBaseDTO> { new CInformacionContactoDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                    //}

                    //if (((Funcionario)resultado.Contenido).EstadoFuncionario.PK_EstadoFuncionario != 20)
                    //{
                    //    respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(((Funcionario)resultado.Contenido).Nombramiento.LastOrDefault()) });

                    //    respuesta.Add(new List<CBaseDTO> { CPuestoL.PuestoGeneral(((Funcionario)resultado.Contenido).Nombramiento.LastOrDefault().Puesto) });

                    //    respuesta.Add(new List<CBaseDTO> { CDireccionL.ConvertirDireccionADTO(((Funcionario)resultado.Contenido).Direccion.FirstOrDefault()) });
                    //}

                    return respuesta;
                }
                else
                {
                    var funcionarioSIRH = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                    if (funcionarioSIRH != null)
                    {
                        respuesta.Add(new List<CBaseDTO> { new CErrorDTO { Codigo = 5, MensajeError = "La cédula indicada ya se encuentra registrada en el sistema, si es un funcionario que vuelve a laborar para el Ministerio, utilice el formulario de Detalle de Contratación para reactivarlo" } });
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { (CErrorDTO)resultado.Contenido });
                        return respuesta;
                    }
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }
        }

        public List<CBaseDTO> BuscarFuncionarioJornada(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.BuscarFuncionarioJornada(cedula);
                if (resultado.Codigo > 0)
                {
                    //Agrega los datos básicos del funcionario
                    respuesta.Add(FuncionarioGeneral((Funcionario)resultado.Contenido));
                    //Agrega los datos del nombramiento
                    var nombramiento = ((Funcionario)resultado.Contenido).Nombramiento
                                            .Where(N => N.FecVence == null).FirstOrDefault();
                    respuesta.Add(CNombramientoL.NombramientoGeneral(nombramiento));
                    //Agrega los datos del puesto
                    respuesta.Add(CPuestoL.PuestoGeneral(nombramiento.Puesto));
                    //Agrega los datos de la jornada
                    var jornada = ((Funcionario)resultado.Contenido).DetalleContratacion.FirstOrDefault();

                    if (jornada != null)
                    {
                        if (jornada.TipoJornada != null)
                        {
                            respuesta.Add(CTipoJornadaL.TipoJornadaGeneral
                                                            (jornada.TipoJornada));
                        }
                        else
                        {
                            respuesta.Add(new CTipoJornadaDTO());
                        }
                    }
                    else
                    {
                        respuesta.Add(new CTipoJornadaDTO());
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> BuscarFuncionarioUsuario(string nombreUsuario)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.BuscarFuncionarioUsuario(nombreUsuario);
                if (resultado.Codigo > 0)
                {
                    //Agrega los datos básicos del funcionario
                    respuesta.Add(FuncionarioGeneral((Funcionario)resultado.Contenido));
                    //Agrega los datos del nombramiento
                    var nombramiento = ((Funcionario)resultado.Contenido).Nombramiento
                                            .Where(N => N.FecVence == null).FirstOrDefault();
                    respuesta.Add(CNombramientoL.NombramientoGeneral(nombramiento));
                    //Agrega los datos del puesto
                    respuesta.Add(CPuestoL.ConstruirPuesto(nombramiento.Puesto, new CPuestoDTO()));
                    //Agrega los datos de la jornada
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(nombramiento.Puesto
                                                            .DetallePuesto.FirstOrDefault()));                     

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
        //
        public List<List<CBaseDTO>> FuncionariosConCauciones()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.FuncionariosConCauciones();
                if (resultado.Codigo > 0)
                {
                    var lista = (List<Funcionario>)resultado.Contenido;
                    foreach (var item in lista)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(FuncionarioGeneral((Funcionario)item));
                        var nombramiento = ((Funcionario)item).Nombramiento
                                            .Where(N => N.FecVence == null || N.FecVence > DateTime.Now).OrderByDescending(Q => Q.FecRige).FirstOrDefault();
                        if (nombramiento != null) {
                            if(nombramiento.Puesto != null)
                            {
                                if (nombramiento.Puesto.DetallePuesto != null) {

                                    if (nombramiento.Puesto.DetallePuesto.FirstOrDefault() != null) {

                                        temp.Add(CPuestoL.ConstruirPuesto(nombramiento.Puesto, new CPuestoDTO()));
                                        temp.Add(CDetallePuestoL.ConstruirDetallePuesto(nombramiento.Puesto
                                                                            .DetallePuesto.FirstOrDefault()));
                                        if (nombramiento.Caucion != null && nombramiento.Caucion.Count > 0)
                                        {
                                            var caucion = nombramiento.Caucion.Where(Q => Q.IndEstadoPoliza == 1 || Q.IndEstadoPoliza == 4).OrderByDescending(Q => Q.FecVence).FirstOrDefault();
                                            if (caucion != null)
                                            {
                                                temp.Add(CCaucionL.ConvertirDatosCaucionADto(caucion));
                                                ((CCaucionDTO)temp.Last()).DetalleEstadoPoliza = CCaucionL.DefinirEstadoPoliza(((CCaucionDTO)temp.Last()).EstadoPoliza);
                                            }
                                            else
                                            {
                                                temp.Add(new CBaseDTO { IdEntidad = -1, Mensaje = "El funcionario no tiene cauciones activas" });
                                            }
                                        }
                                        else
                                        {
                                            temp.Add(new CBaseDTO { IdEntidad = -1, Mensaje = "El funcionario no tiene cauciones activas" });
                                        }
                                        respuesta.Add(temp);
                                    }

                                    
                                }
                            }
                            
                        }
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { MensajeError = error.Message });
                respuesta.Add(temp);
                return respuesta;
            }
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoPrincipal(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuesto(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                    respuesta.Add(funcionario);

                    Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderBy(N => N.FecRige).LastOrDefault();

                    if (datoNombramiento == null)
                    {
                        throw new Exception("El funcionario no tiene un Nombramiento activo");
                    }

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                    var datoDetalleContrato = dato.DetalleContratacion.FirstOrDefault();

                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                        DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                    };


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                            DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt16(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault()?.EscalaSalarial.IndCategoria),
                            SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault()?.EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault()?.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                            }
                        };
                    }
                    else
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            Mensaje = "No se encontro la escala salarial"
                        };
                    }



                    decimal monSalarioBaseCalculo = detallePuesto.EscalaSalarial.SalarioBase;
                    // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                    if (datoDetalleContrato != null)
                    {
                        if (datoDetalleContrato.CodPolicial > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            }
                        }
                    }

                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * monSalarioBaseCalculo) / 100;

                        detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                        {
                            IdEntidad = item.PK_DetallePuestoRubro,
                            Componente = new CComponenteSalarialDTO
                            {
                                IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                                DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                            },
                            PorValor = item.PorValor,
                            MtoValor = monto
                        });
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    respuesta.Add(detallePuesto);

                    if (datoDetalleContrato != null)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO
                        {
                            NumeroAnualidades = 0,
                            FechaMesAumento = "1"
                        });
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)datosDB.Contenido);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }


            return respuesta;
        }

        public List<CFuncionarioDTO> BuscarFuncionarioParam(string cedula, string nombre, string apellido1, string apellido2,
                                             string codpuesto, string codclase, string codespecialidad,
                                             int codnivel, string coddivision, string coddireccion, string coddepartamento,
                                             string codseccion, string codpresupuesto, string codestado, string codcostos)
        {
            try
            {
                List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();


                List<Funcionario> acumulado = new List<Funcionario>();


                Funcionario funcbusca =
                    new Funcionario
                    {
                        IdCedulaFuncionario = cedula,
                        NomFuncionario = nombre,
                        NomPrimerApellido = apellido1,
                        NomSegundoApellido = apellido2,
                        EstadoFuncionario = new EstadoFuncionario
                        {
                            DesEstadoFuncionario = codestado
                        }
                    };
                CFuncionarioD funcionarioResultado = new CFuncionarioD(contexto);

                if (cedula != "" || apellido1 != "" || apellido2 != "" || nombre != "" || codestado != "")
                {
                    acumulado = funcionarioResultado.BuscarFuncionario(funcbusca).Distinct().ToList();
                }


                if (codpuesto != "" || codclase != "" || codespecialidad != "" || codnivel != 0)
                {
                    acumulado = funcionarioResultado.BuscarFuncionarioDetallePuestoPreCargado(acumulado, codpuesto, codclase != "" ? Convert.ToInt32(codclase) : 0,
                                                                                                      codespecialidad != "" ? Convert.ToInt32(codespecialidad) : 0,
                                                                                                      codnivel);
                }


                if (coddivision != "" || coddireccion != "" || coddepartamento != "" || codseccion != "" || codpresupuesto != "" || codcostos != "")
                {
                    acumulado = funcionarioResultado.BuscarFuncionarioUbicacionPrecargado(acumulado, coddivision != "" ? Convert.ToInt32(coddivision) : 0,
                                                                                                         coddireccion != "" ? Convert.ToInt32(coddireccion) : 0,
                                                                                                         coddepartamento != "" ? Convert.ToInt32(coddepartamento) : 0,
                                                                                                         codseccion != "" ? Convert.ToInt32(codseccion) : 0, codpresupuesto,
                                                                                                         codcostos);
                }


                foreach (var item in acumulado)
                {
                    CFuncionarioDTO temp = new CFuncionarioDTO();
                    temp.Cedula = item.IdCedulaFuncionario;
                    temp.Nombre = item.NomFuncionario;
                    temp.PrimerApellido = item.NomPrimerApellido;
                    temp.SegundoApellido = item.NomSegundoApellido;
                    temp.Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino;
                    temp.FechaNacimiento = Convert.ToDateTime(item.FecNacimiento);
                    temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                    {
                        IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                        DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                    };
                    resultado.Add(temp);
                }


                return resultado;
            }
            catch (Exception error)
            {
                return new List<CFuncionarioDTO> { new CFuncionarioDTO { Mensaje = error.Message } };
            }
        }


        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuesto(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                    var expediente = CExpedienteL.ConvertirExpedienteADTO(contexto.ExpedienteFuncionario.FirstOrDefault(Q => Q.Funcionario.IdCedulaFuncionario == funcionario.Cedula));
                    if (expediente != null)
                    {
                        funcionario.Mensaje = expediente.NumeroExpediente.ToString();
                    }
                    else
                    {
                        funcionario.Mensaje = "NO TIENE";
                    }
                    respuesta.Add(funcionario);

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                    var datoDetalleContrato = dato.DetalleContratacion.FirstOrDefault();

                    //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                    Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();

                    if (datoNombramiento == null)
                        throw new Exception("El funcionario no tiene un Nombramiento activo");


                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                            DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                        };
                    }
                    else
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = 0,
                            DesClase = "SD"
                        };
                    }

                    
                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                            DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                        };
                    }
                    else {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                            SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                            }
                        };
                    }
                    else
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = -1,
                            SalarioBase = 0,
                            MontoAumentoAnual = 0,
                            Periodo = new CPeriodoEscalaSalarialDTO { IdEntidad = -1 }
                        };
                    }


                    decimal monSalarioBaseCalculo = detallePuesto.EscalaSalarial.SalarioBase;
                    // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                    if (datoDetalleContrato != null)
                    {
                        if (datoDetalleContrato.CodPolicial > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            }
                        }
                    }

                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * monSalarioBaseCalculo) / 100;

                        detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                        {
                            IdEntidad = item.PK_DetallePuestoRubro,
                            Componente = new CComponenteSalarialDTO
                            {
                                IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                                DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                            },
                            PorValor = item.PorValor,
                            MtoValor = monto
                        });
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    respuesta.Add(detallePuesto);

                    if (datoDetalleContrato != null)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO {
                                            NumeroAnualidades = 0,
                                            FechaMesAumento = "1"});
                    }

                }
                else
                {
                    respuesta.Add((CErrorDTO)datosDB.Contenido);
                }
            }
            catch(Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
            

            return respuesta;
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoVacaciones(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuestoVacaciones(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                    var expediente = CExpedienteL.ConvertirExpedienteADTO(contexto.ExpedienteFuncionario.FirstOrDefault(Q => Q.Funcionario.IdCedulaFuncionario == funcionario.Cedula));
                    if (expediente != null)
                    {
                        funcionario.Mensaje = expediente.NumeroExpediente.ToString();
                    }
                    else
                    {
                        funcionario.Mensaje = "NO TIENE";
                    }
                    respuesta.Add(funcionario);

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                    var datoDetalleContrato = dato.DetalleContratacion.FirstOrDefault();

                    //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                    Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();

                    if (datoNombramiento == null)
                    {
                        datoNombramiento = dato.Nombramiento.OrderByDescending(x => x.FecRige).FirstOrDefault();
                    }
                    if (datoNombramiento == null)
                    {
                        throw new Exception("El funcionario no tiene un Nombramiento activo");
                    }


                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                            DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                        };
                    }
                    else
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = 0,
                            DesClase = "SD"
                        };
                    }


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                            DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                            SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                            }
                        };
                    }
                    else
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = -1,
                            SalarioBase = 0,
                            MontoAumentoAnual = 0,
                            Periodo = new CPeriodoEscalaSalarialDTO { IdEntidad = -1 }
                        };
                    }


                    decimal monSalarioBaseCalculo = detallePuesto.EscalaSalarial.SalarioBase;
                    // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                    if (datoDetalleContrato != null)
                    {
                        if (datoDetalleContrato.CodPolicial > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            }
                        }
                    }

                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * monSalarioBaseCalculo) / 100;

                        detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                        {
                            IdEntidad = item.PK_DetallePuestoRubro,
                            Componente = new CComponenteSalarialDTO
                            {
                                IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                                DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                            },
                            PorValor = item.PorValor,
                            MtoValor = monto
                        });
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    respuesta.Add(detallePuesto);

                    if (datoDetalleContrato != null)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO
                        {
                            NumeroAnualidades = 0,
                            FechaMesAumento = "1"
                        });
                    }

                }
                else
                {
                    respuesta.Add((CErrorDTO)datosDB.Contenido);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }


            return respuesta;
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoPV(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuestoVacaciones(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                    respuesta.Add(funcionario);
                    Nombramiento datoNombramiento = null;
                    //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                    if (funcionario.EstadoFuncionario.IdEntidad == 1)
                    {
                        datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();
                    }
                    else
                    {
                        datoNombramiento = dato.Nombramiento.OrderByDescending(x => x.FecRige).FirstOrDefault();
                    }

                    if (datoNombramiento == null)
                        throw new Exception("El funcionario no tiene un Nombramiento activo");

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                    var datoDetalleContrato = dato.DetalleContratacion.FirstOrDefault();


                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                            DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                        };
                    }
                    else
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = 0,
                            DesClase = "SD"
                        };
                    }


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                            DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                            SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                            }
                        };
                    }
                    else
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = -1,
                            SalarioBase = 0,
                            MontoAumentoAnual = 0,
                            Periodo = new CPeriodoEscalaSalarialDTO { IdEntidad = -1 }
                        };
                    }


                    decimal monSalarioBaseCalculo = detallePuesto.EscalaSalarial.SalarioBase;
                    // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                    if (datoDetalleContrato != null)
                    {
                        if (datoDetalleContrato.CodPolicial > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            }
                        }
                    }

                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * monSalarioBaseCalculo) / 100;

                        detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                        {
                            IdEntidad = item.PK_DetallePuestoRubro,
                            Componente = new CComponenteSalarialDTO
                            {
                                IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                                DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                            },
                            PorValor = item.PorValor,
                            MtoValor = monto
                        });
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    respuesta.Add(detallePuesto);

                    
                    if (datoDetalleContrato != null)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO
                        {
                            NumeroAnualidades = 0,
                            FechaMesAumento = "1"
                        });
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)datosDB.Contenido);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }


            return respuesta;
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoPropiedad(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuesto(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    CFuncionarioDTO funcionario = FuncionarioGeneral(dato);
                    respuesta.Add(funcionario);

                    var datoDetalleContrato = CDetalleContratacionL.ConvertirDetalleContratacionADTO
                                      (dato.DetalleContratacion.Where(Q => Q.FecCese == null)
                                      .FirstOrDefault());

                    Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false).OrderBy(N => N.FecRige).LastOrDefault();

                    if (datoNombramiento == null)
                    {
                        throw new Exception("El funcionario no tiene un Nombramiento en Propiedad");
                    }

                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                        DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                    };

                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                    };

                    detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                        CategoriaEscala = Convert.ToInt16(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                        SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                        MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),

                        Periodo = new CPeriodoEscalaSalarialDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                            MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                        }
                    };

                    decimal monSalarioBaseCalculo = detallePuesto.EscalaSalarial.SalarioBase;
                    // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                    if (datoDetalleContrato != null)
                    {
                        if (datoDetalleContrato.CodigoPolicial > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            }
                        }
                    }

                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * monSalarioBaseCalculo) / 100;

                        detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                        {
                            IdEntidad = item.PK_DetallePuestoRubro,
                            Componente = new CComponenteSalarialDTO
                            {
                                IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                                DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                            },
                            PorValor = item.PorValor,
                            MtoValor = monto
                        });
                    }

                    if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    respuesta.Add(detallePuesto);

                  

                    if (datoDetalle != null)
                    {
                        respuesta.Add(datoDetalleContrato);
                    }
                    else
                    {
                        respuesta.Add(null);
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)datosDB.Contenido);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }


            return respuesta;
        }
        public CBaseDTO BuscarDesgloceSalarialPF(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            int aux = 0;
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var dato = intermedio.BuscarFuncionarioDesgloceSalarialPF(cedula);

                if (dato.Count > 0)
                {
                    foreach (DesgloseSalarial desglose in (List<DesgloseSalarial>)dato)
                    {

                        if (Convert.ToInt32(desglose.IndPeriodo) > aux)
                            respuesta = (CDesgloseSalarialL.DesgloseSalarialGeneral(desglose));
                        aux = Convert.ToInt32(desglose.IndPeriodo);
                    }
                }
                else {
                    throw new Exception("El funcionario no tiene un desglose salarial activo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
            return respuesta;
        }


        public List<CBaseDTO> BuscarPuestoPF(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            int aux = 0;
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                List<Puesto> dato = intermedio.BuscarFuncionarioPuestoPF(cedula);

                foreach (Puesto puesto in dato)
                {


                    respuesta.Add(CPuestoL.PuestoGeneral(puesto));
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
            return respuesta;
        }




        public CRespuestaDTO BuscarFuncionarioPuntosCarreraProfesional(string cedula)
        {
            decimal totalPuntos = 0;

            CRespuestaDTO respuesta;
            CRespuestaDTO respuestaEP = new CRespuestaDTO();

            CFormacionAcademicaL intermedioFA = new CFormacionAcademicaL();
            var dato = intermedioFA.BuscarDatosCarreraCedula(cedula);

            try
            {
                var grado = (CCursoGradoDTO)dato[3];
                totalPuntos += grado.Incentivo;
            }
            catch { }

            try
            {
                var capacitacion = (CCursoCapacitacionDTO)dato[5];
                totalPuntos += capacitacion.TotalPuntos;
            }
            catch { }
                    
            CExperienciaProfesionalD intermedioEP = new CExperienciaProfesionalD(contexto);
            respuestaEP = intermedioEP.BuscarExperienciaProfesionalCedula(cedula);

            try
            {
                var datoEP = ((Funcionario)respuestaEP.Contenido).FormacionAcademica.FirstOrDefault().ExperienciaProfesional;

                foreach (var item in datoEP)
                {
                    totalPuntos += Convert.ToDecimal(item.IndPuntosAsignados);
                }
            }
            catch { }
                       

            respuesta = new CRespuestaDTO
            {
                Codigo = 1,
                Contenido = totalPuntos
            };

            return respuesta;
        }

        public List<CFuncionarioDTO> BuscarFuncionarioParam(string cedula, string nombre, string apellido1, string apellido2,
                                             string codpuesto, string codclase, string codespecialidad,
                                             string codocupacion, string coddivision, string coddireccion, string coddepartamento,
                                             string codseccion, string codpresupuesto)
        {
            try
            {
                List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();


                List<Funcionario> acumulado = new List<Funcionario>();


                Funcionario funcbusca =
                    new Funcionario
                    {
                        IdCedulaFuncionario = cedula,
                        NomFuncionario = nombre,
                        NomPrimerApellido = apellido1,
                        NomSegundoApellido = apellido2
                    };
                CFuncionarioD funcionarioResultado = new CFuncionarioD(contexto);


                if (cedula != "" || apellido1 != "" || apellido2 != "" || nombre != "")
                {
                    acumulado = funcionarioResultado.BuscarFuncionario(funcbusca).Distinct().ToList();
                }


                if (codpuesto != "" || codclase != "" || codespecialidad != "" || codocupacion != "")
                {
                    acumulado = funcionarioResultado.BuscarFuncionarioDetallePuestoPreCargado(acumulado, codpuesto, codclase != "" ? Convert.ToInt32(codclase) : 0,
                                                                                                      codespecialidad != "" ? Convert.ToInt32(codespecialidad) : 0,
                                                                                                      codocupacion != "" ? Convert.ToInt32(codocupacion) : 0);
                }


                if (coddivision != "" || coddireccion != "" || coddepartamento != "" || codseccion != "" || codpresupuesto != "")
                {
                    acumulado = funcionarioResultado.BuscarFuncionarioUbicacionPrecargado(acumulado, coddivision != "" ? Convert.ToInt32(coddivision) : 0,
                                                                                                         coddireccion != "" ? Convert.ToInt32(coddireccion) : 0,
                                                                                                         coddepartamento != "" ? Convert.ToInt32(coddepartamento) : 0,
                                                                                                         codseccion != "" ? Convert.ToInt32(codseccion) : 0, codpresupuesto);
                }


                foreach (var item in acumulado)
                {
                    CFuncionarioDTO temp = new CFuncionarioDTO();
                    temp.IdEntidad = item.PK_Funcionario;
                    temp.Cedula = item.IdCedulaFuncionario;
                    temp.Nombre = item.NomFuncionario;
                    temp.PrimerApellido = item.NomPrimerApellido;
                    temp.SegundoApellido = item.NomSegundoApellido;
                    temp.Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino;
                    temp.FechaNacimiento = Convert.ToDateTime(item.FecNacimiento);
                    temp.EstadoFuncionario = new CEstadoFuncionarioDTO
                    {
                        IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                        DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                    };
                    resultado.Add(temp);
                }


                return resultado;
            }
            catch (Exception error)
            {
                return new List<CFuncionarioDTO> { new CFuncionarioDTO { Mensaje = error.Message } };
            }
        }

        internal static CTipoContactoDTO ConvertirTipoContactoADTO(TipoContacto item)
        {
            return new CTipoContactoDTO
            {
                IdEntidad = item.PK_TipoContacto,
                DesTipoContacto = item.DesTipoContacto,
            };
        }

        public List<CBaseDTO> ListarTipoContacto()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CTipoContactoD intermedio = new CTipoContactoD(contexto);
                var datos = intermedio.CargarTiposDeContacto();
                if (datos != null)
                {
                    
                    foreach (var item in datos)
                    {
                        respuesta.Add(ConvertirTipoContactoADTO(item));
                    }

                    return respuesta;
                    
                    
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> ListarFuncionariosActivos(bool guardasSeguridad, bool oficialesTransito)
        {
            List<CBaseDTO> respuesta;
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultado = intermedio.ListarFuncionariosActivos(guardasSeguridad, oficialesTransito);
                if (resultado.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();

                    var listaFuncionarios = ((List<ListaNombramientosActivos>)resultado.Contenido);

                    foreach (var item in listaFuncionarios)
                    {
                        var dato = ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario);
                        respuesta.Add(dato);
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message,
                    Mensaje = error.Message
                });
                return respuesta;
            }
        }

        public CBaseDTO ActualizarInformacionBasicaFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion,
                                                               List<CInformacionContactoDTO> contacto, CHistorialEstadoCivilDTO historial)
        {
            try
            {
                CBaseDTO respuesta = new CBaseDTO();
                CFuncionarioD intermedio = new CFuncionarioD(contexto);

                var datoFuncionario = contexto.Funcionario.FirstOrDefault(F => F.IdCedulaFuncionario == funcionario.Cedula);
                respuesta = FuncionarioGeneral(datoFuncionario);

                int contador = 0;

                if (funcionario != null)
                {
                    if (direccion.Mensaje == null)
                    {
                        var datosUbicacion = direccion.Distrito.Mensaje.Split('-');
                        var datoProvincia = datosUbicacion[0];
                        var datoCanton = datosUbicacion[1];
                        var datoDistrito = direccion.Distrito.NomDistrito;

                        direccion.Distrito.IdEntidad = contexto.Distrito.FirstOrDefault(Q => Q.NomDistrito == datoDistrito &&
                                                                Q.Canton.NomCanton == datoCanton &&
                                                                Q.Canton.Provincia.NomProvincia == datoProvincia).PK_Distrito;

                        var cambioDireccion = intermedio.ActualizarDireccionDomicilio(direccion, funcionario.Cedula);
                        if (cambioDireccion.Codigo < 0)
                        {
                            respuesta.Mensaje = ((CErrorDTO)cambioDireccion.Contenido).MensajeError;
                        }
                    }
                    else
                    {
                        contador++;
                    }

                    var datosContacto = contacto.Where(I => I.Mensaje == null).ToList();

                    if (datosContacto.Count()  > 0)
                    {
                        foreach (var item in datosContacto)
                        {
                            var cambioContacto = intermedio.ActualizarInformacionContacto(item, funcionario.Cedula);
                            if (cambioContacto.Codigo < 0)
                            {
                                respuesta.Mensaje += "-" + ((CErrorDTO)cambioContacto.Contenido).MensajeError;
                            }
                        }
                    }
                    else
                    {
                        contador++;
                    }

                    if (historial.Mensaje == null)
                    {
                        HistorialEstadoCivil datoHistorial = new HistorialEstadoCivil
                        {
                            FK_CatEstadoCivil = contexto.CatEstadoCivil.FirstOrDefault(C => C.DesEstadoCivil == historial.CatEstadoCivil.DesEstadoCivil).PK_CatEstadoCivil,
                            FK_Funcionario = contexto.Funcionario.FirstOrDefault(F => F.IdCedulaFuncionario == funcionario.Cedula).PK_Funcionario,
                            FecInicio = DateTime.Now
                        };
                        var cambioHistorial = intermedio.ActualizarEstadoCivil(datoHistorial);
                        if (cambioHistorial.Codigo < 0)
                        {
                            respuesta.Mensaje = ((CErrorDTO)cambioHistorial.Contenido).MensajeError;
                        }
                    }
                    else
                    {
                        contador++;
                    }
                }
                if (contador > 0)
                {
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se realizó ningún cambio a la información del funcionario");
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }
        }

        public List<CBaseDTO> BuscarInformacionBasicaFuncionario(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);

                var resultado = intermedio.BuscarInformacionBasicaFuncionario(cedula);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(FuncionarioGeneral(((Funcionario)resultado.Contenido)));
                    respuesta.Add(CDireccionL.ConvertirDireccionADTO(((Funcionario)resultado.Contenido).Direccion.FirstOrDefault()));
                    respuesta.Add(CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(((Funcionario)resultado.Contenido).HistorialEstadoCivil.FirstOrDefault()));
                    foreach (var item in ((Funcionario)resultado.Contenido).InformacionContacto)
                    {
                        respuesta.Add(CInformacionContactoL.ConvertirInformacionContactoADTO(item));
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO>
                {
                    new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    }
                };
            }
        }

        //public List<CFuncionarioDTO> BuscarFuncionarioParam(string cedula, string nombre, string apellido1, string apellido2,
        //                                     string codpuesto, string codclase, string codespecialidad,
        //                                     string codocupacion, string coddivision, string coddireccion, string coddepartamento,
        //                                     string codseccion, string codpresupuesto)
        //{
        //    try
        //    {
        //        List<CFuncionarioDTO> resultado = new List<CFuncionarioDTO>();

        //        List<Funcionario> acumulado = new List<Funcionario>();

        //        Funcionario funcbusca =
        //            new Funcionario
        //            {
        //                IdCedulaFuncionario = cedula,
        //                NomFuncionario = nombre,
        //                NomPrimerApellido = apellido1,
        //                NomSegundoApellido = apellido2
        //            };
        //        CFuncionarioD funcionarioResultado = new CFuncionarioD(contexto);

        //        if (cedula != "" || apellido1 != "" || apellido2 != "" || nombre != "")
        //        {
        //            acumulado = funcionarioResultado.BuscarFuncionario(funcbusca).Distinct().ToList();
        //        }

        //        if (codpuesto != "" || codclase != "" || codespecialidad != "" || codocupacion != "")
        //        {
        //            acumulado = funcionarioResultado.BuscarFuncionarioDetallePuestoPreCargado(acumulado, codpuesto, codclase != "" ? Convert.ToInt32(codclase) : 0,
        //                                                                                              codespecialidad != "" ? Convert.ToInt32(codespecialidad) : 0,
        //                                                                                              codocupacion != "" ? Convert.ToInt32(codocupacion) : 0);
        //        }

        //        if (coddivision != "" || coddireccion != "" || coddepartamento != "" || codseccion != "" || codpresupuesto != "")
        //        {
        //            acumulado = funcionarioResultado.BuscarFuncionarioUbicacionPrecargado(acumulado, coddivision != "" ? Convert.ToInt32(coddivision) : 0,
        //                                                                                                 coddireccion != "" ? Convert.ToInt32(coddireccion) : 0,
        //                                                                                                 coddepartamento != "" ? Convert.ToInt32(coddepartamento) : 0,
        //                                                                                                 codseccion != "" ? Convert.ToInt32(codseccion) : 0, codpresupuesto);
        //        }

        //        foreach (var item in acumulado)
        //        {
        //            CFuncionarioDTO temp = new CFuncionarioDTO();
        //            temp.Cedula = item.IdCedulaFuncionario;
        //            temp.Nombre = item.NomFuncionario;
        //            temp.PrimerApellido = item.NomPrimerApellido;
        //            temp.SegundoApellido = item.NomSegundoApellido;
        //            temp.Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino;
        //            temp.FechaNacimiento = Convert.ToDateTime(item.FecNacimiento);
        //            temp.EstadoFuncionario = new CEstadoFuncionarioDTO
        //            {
        //                IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
        //                DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
        //            };
        //            resultado.Add(temp);
        //        }

        //        return resultado;
        //    }
        //    catch (Exception error)
        //    {
        //        return new List<CFuncionarioDTO> { new CFuncionarioDTO { Mensaje = error.Message } }; 
        //    }
        //}

        public List<CEMUExfuncionarioDTO> BuscarExfuncionarioFiltros(string cedula, string nombre, string primerApellido, string segundoApellido, string codPuesto)
        {
            var ced = "";
            var ClasePuesto = "";
            var CodInspectores = "";
            var Departamento = "";
            var Direccion ="";
            var Division = "";
            var Seccion = "";
            var DescEstado = "";

            List <CEMUExfuncionarioDTO> respuesta = new List<CEMUExfuncionarioDTO>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                CClaseD intermedioClase = new CClaseD(contexto);
                CDivisionD intermedioDivision = new CDivisionD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CDepartamentoD intermedioDepartamento = new CDepartamentoD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CEstadoFuncionarioD intermedioEstado = new CEstadoFuncionarioD(contexto);

                var datos = intermedio.BuscarExfuncionarioFiltros(cedula, nombre, primerApellido, segundoApellido, codPuesto);
                if (datos != null)
                {
                    foreach (var item in datos)
                    {
                        ced = item.CEDULA;
                        //if (ced == "0050860829")
                        //    ced = "";  

                        try
                        {   ClasePuesto = item.CLASE_PUESTO != null ? item.CLASE_PUESTO + " - " + intermedioClase.CargarClasePorID(Convert.ToInt16(item.CLASE_PUESTO)).DesClase : "";   }
                        catch (Exception err)
                        {   ClasePuesto = item.CLASE_PUESTO;    }

                        try
                        {    CodInspectores = item.CODIGO_INSPECTORES != null ? item.CODIGO_INSPECTORES : "";   }
                        catch (Exception err)
                        {    CodInspectores = item.CODIGO_INSPECTORES;    }

                        try
                        {   Departamento = item.DEPARTAMENTO != null && item.DEPARTAMENTO != "000" ? item.DEPARTAMENTO + " - " + intermedioDepartamento.CargarDepartamentoPorID(Convert.ToInt32(item.DEPARTAMENTO)).NomDepartamento : "";   }
                        catch (Exception err)
                        {   Departamento = item.DEPARTAMENTO;  }

                        try
                        {   Direccion = item.DIRECCION != null ? (Convert.ToInt32(item.DIRECCION) > 0 ? item.DIRECCION + " - " + intermedioDireccion.CargarDireccionGeneralPorID(Convert.ToInt32(item.DIRECCION)).NomDireccion : "") : "";  }
                        catch (Exception err)
                        {   Direccion = item.DIRECCION; }

                        try
                        {    Division = item.DIVISION != null ? item.DIVISION + " - " + intermedioDivision.CargarDivisionPorID(Convert.ToInt16(item.DIVISION)).NomDivision : "";    }
                        catch (Exception err)
                        {   Division = "";  }

                        try
                        {   Seccion = item.SECCION != null && item.SECCION != "000" ? item.SECCION + " - " + intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(item.SECCION)).NomSeccion : "";   }
                        catch (Exception err)
                        {   Seccion = "";   }

                        try
                        {
                            DescEstado = item.ESTADO_FUNC != null && item.ESTADO_FUNC != 99 ?
                                                item.ESTADO_FUNC != 1 ? 
                                                    intermedioEstado.CargarEstadoFuncionarioPorID(Convert.ToInt32(item.ESTADO_FUNC)).DesEstadoFuncionario
                                                    : "Traslado"
                                                : "";
                        }
                        catch (Exception err)
                        { DescEstado = ""; }

                        respuesta.Add(new CEMUExfuncionarioDTO
                        {
                            Cedula = item.CEDULA,
                            ClasePuesto = ClasePuesto, //item.CLASE_PUESTO != null ? item.CLASE_PUESTO + " - " +  intermedioClase.CargarClasePorID(Convert.ToInt16(item.CLASE_PUESTO)).DesClase : "",
                            CodInspectores = CodInspectores,//item.CODIGO_INSPECTORES != null ? item.CODIGO_INSPECTORES : "",
                            Departamento = Departamento, //item.DEPARTAMENTO != null && item.DEPARTAMENTO != "000" ? item.DEPARTAMENTO + " - " +  intermedioDepartamento.CargarDepartamentoPorID(Convert.ToInt32(item.DEPARTAMENTO)).NomDepartamento : "",
                            Direccion = Direccion, //item.DIRECCION != null ? (Convert.ToInt32(item.DIRECCION) > 0 ? item.DIRECCION + " - " +  intermedioDireccion.CargarDireccionGeneralPorID(Convert.ToInt32(item.DIRECCION)).NomDireccion : "") : "",
                            Division = Division, //item.DIVISION != null ? item.DIVISION + " - " + intermedioDivision.CargarDivisionPorID(Convert.ToInt16(item.DIVISION)).NomDivision : "",
                            EstadoCivil = item.ESTADO_CIVIL,
                            EstadoFunc = item.ESTADO_FUNC == null ? 0 : (int)item.ESTADO_FUNC,
                            DescEstado = DescEstado.ToUpper(),
                            FechaCese = item.FECHA_CESE == null ? item.FECHA_CESE : Convert.ToDateTime(item.FECHA_CESE),
                            FechaCumple = item.FECHA_CUMPLE == null ? item.FECHA_CUMPLE : Convert.ToDateTime(item.FECHA_CUMPLE),
                            FechaIngreso = item.FECHA_INGRESO == null ? item.FECHA_INGRESO : Convert.ToDateTime(item.FECHA_INGRESO),
                            Nombre = item.NOMBRE,
                            NumeroExpediente = item.NUMERO_EXPEDIENTE,
                            PrimerApellido = item.PRIMER_APELLIDO,
                            PuestoPropiedad = item.PUESTO_PROPIEDAD,
                            Programa = item.PROGRAMA,
                            Seccion = Seccion, //item.SECCION != null && item.SECCION != "000" ? item.SECCION + " - " + intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(item.SECCION)).NomSeccion : "",
                            SegundoApellido = item.SEGUNDO_APELLIDO,
                            Sexo = item.SEXO,
                            SubDivision = item.SUBDIVISION,
                            UltimoPeriodo = item.ULTIMO_PERIODO
                        });
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CEMUExfuncionarioDTO>();
                return respuesta;
            }
        }

        public List<CPuestoDTO> EnviarFuncionarioPuestoDetallePuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal, string estadoPuesto, string confianza, int codNivel)
        {
            List<CPuestoDTO> resultado = new List<CPuestoDTO>();

            CFuncionarioD datos = new CFuncionarioD(contexto);

            var item = datos.BuscarFuncionarioPuestoDetallePuesto(codPuesto, codClase, codEspecialidad, codOcupacionReal, estadoPuesto, confianza, codNivel);

            foreach (var aux in item)
            {
                CPuestoDTO temp = new CPuestoDTO
                {
                    CodPuesto = aux.CodPuesto,
                    UbicacionAdministrativa = CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(aux.UbicacionAdministrativa),
                    NivelOcupacional = Convert.ToInt32(aux.IndNivelOcupacional)
                };

                if (aux.DetallePuesto != null)
                {
                    CClaseDTO clase = new CClaseDTO();
                    CEspecialidadDTO especialidad = new CEspecialidadDTO();
                    CSubEspecialidadDTO subespecialidad = new CSubEspecialidadDTO();
                    var datoDetalle = aux.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    if(datoDetalle == null)
                        datoDetalle = aux.DetallePuesto.FirstOrDefault();

                    if (datoDetalle.Clase != null)
                    {
                        clase.DesClase = datoDetalle.Clase.DesClase;
                        clase.IdEntidad = datoDetalle.Clase.PK_Clase;
                    }
                    if (datoDetalle.Especialidad != null)
                    {
                        especialidad.DesEspecialidad = datoDetalle.Especialidad.DesEspecialidad;
                        especialidad.IdEntidad = datoDetalle.Especialidad.PK_Especialidad;
                    }
                    if (datoDetalle.SubEspecialidad != null)
                    {
                        subespecialidad.DesSubEspecialidad = datoDetalle.SubEspecialidad.DesSubEspecialidad;
                        subespecialidad.IdEntidad = datoDetalle.SubEspecialidad.PK_SubEspecialidad;
                    }

                    temp.DetallePuesto = new CDetallePuestoDTO
                    {
                        Clase = clase,
                        Especialidad = especialidad,
                        SubEspecialidad = subespecialidad
                    };
                    temp.EstadoPuesto = new CEstadoPuestoDTO
                    {
                        IdEntidad = aux.EstadoPuesto.PK_EstadoPuesto,
                        DesEstadoPuesto = aux.EstadoPuesto.DesEstadoPuesto
                    };
                }

                if (aux.Nombramiento.Count() > 0)
                {
                    var nombramientoPropiedad = aux.Nombramiento.Where(N => N.FecVence == null && N.FK_EstadoNombramiento != 15).OrderBy(N => N.FecRige).FirstOrDefault();
                    var nombramientoOcupante = aux.Nombramiento.Where(N => N.FecVence >= DateTime.Now && N.FK_EstadoNombramiento != 15).OrderBy(N => N.FecRige).FirstOrDefault();

                    if (nombramientoOcupante != null)
                    {
                        temp.Nombramiento = new CNombramientoDTO
                        {
                            IdEntidad = nombramientoOcupante.PK_Nombramiento,
                            FecRige = Convert.ToDateTime(nombramientoOcupante.FecRige),
                            FecVence = Convert.ToDateTime(nombramientoOcupante.FecVence),
                            Funcionario = new CFuncionarioDTO
                            {
                                Nombre = nombramientoOcupante.Funcionario.NomFuncionario,
                                PrimerApellido = nombramientoOcupante.Funcionario.NomPrimerApellido,
                                SegundoApellido = nombramientoOcupante.Funcionario.NomSegundoApellido,
                                Cedula = nombramientoOcupante.Funcionario.IdCedulaFuncionario,
                                Sexo = (nombramientoOcupante.Funcionario.IndSexo == "1" ||
                                nombramientoOcupante.Funcionario.IndSexo == "2") ?
                                (GeneroEnum)Convert.ToInt32(nombramientoOcupante.Funcionario.IndSexo) : GeneroEnum.Masculino
                            }
                            //IdEntidad = aux.Nombramiento.OrderByDescending(N => N.FecRige).First().PK_Nombramiento,
                            //FecRige = Convert.ToDateTime(aux.Nombramiento.OrderByDescending(N => N.FecRige).First().FecRige),
                            //FecVence = Convert.ToDateTime(aux.Nombramiento.OrderByDescending(N => N.FecRige).First().FecVence),
                            //Funcionario = new CFuncionarioDTO
                            //{
                            //    Nombre = aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.NomFuncionario,
                            //    PrimerApellido = aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.NomPrimerApellido,
                            //    SegundoApellido = aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.NomSegundoApellido,
                            //    Cedula = aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.IdCedulaFuncionario,
                            //    Sexo = (aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.IndSexo == "1" ||
                            //    aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.IndSexo == "2") ?
                            //    (GeneroEnum)Convert.ToInt32(aux.Nombramiento.OrderByDescending(N => N.FecRige).First().Funcionario.IndSexo) : GeneroEnum.Masculino
                            //}
                        };
                        if (nombramientoPropiedad != null)
                        {
                            temp.Nombramiento.Mensaje = nombramientoPropiedad.Funcionario.IdCedulaFuncionario + "-" + nombramientoPropiedad.Funcionario.NomFuncionario + " " + nombramientoPropiedad.Funcionario.NomPrimerApellido + " " + nombramientoPropiedad.Funcionario.NomSegundoApellido;
                        }
                    }
                    else
                    {
                        if (nombramientoPropiedad != null)
                        {
                            temp.Nombramiento = new CNombramientoDTO
                            {
                                IdEntidad = nombramientoPropiedad.PK_Nombramiento,
                                FecRige = Convert.ToDateTime(nombramientoPropiedad.FecRige),
                                FecVence = Convert.ToDateTime(nombramientoPropiedad.FecVence),
                                Funcionario = new CFuncionarioDTO
                                {
                                    Nombre = nombramientoPropiedad.Funcionario.NomFuncionario,
                                    PrimerApellido = nombramientoPropiedad.Funcionario.NomPrimerApellido,
                                    SegundoApellido = nombramientoPropiedad.Funcionario.NomSegundoApellido,
                                    Cedula = nombramientoPropiedad.Funcionario.IdCedulaFuncionario,
                                    Sexo = (nombramientoPropiedad.Funcionario.IndSexo == "1" ||
                                nombramientoPropiedad.Funcionario.IndSexo == "2") ?
                                (GeneroEnum)Convert.ToInt32(nombramientoPropiedad.Funcionario.IndSexo) : GeneroEnum.Masculino
                                }
                            };
                        }
                    }
                }
                resultado.Add(temp);
            }
            return resultado;
        }

        //Obtener calificación actual de funcionario por cédula
        public CBaseDTO CargarCalificacionActual(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);

                var resultado = intermedio.CargarCalificacionActual(cedula);

                if (resultado.Codigo > 0)
                {
                    if (((Nombramiento)resultado.Contenido).CalificacionNombramiento.Count() > 0)
                    {
                        respuesta = CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(((Nombramiento)resultado.Contenido).CalificacionNombramiento.FirstOrDefault());
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("El funcionario no tiene calificación aún");
                    }
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return
                    new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    };
            }
        }
    }
}

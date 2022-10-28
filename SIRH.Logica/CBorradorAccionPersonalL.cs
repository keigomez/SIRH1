using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CBorradorAccionPersonalL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CBorradorAccionPersonalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CBorradorAccionPersonalDTO ConvertirDatosBorradorADto(BorradorAccionPersonal item)
        {
            return new CBorradorAccionPersonalDTO
            {
                IdEntidad = item.PK_Borrador,
                UsuarioAsignado = item.UsuarioAsignado,
                NumOficio = item.NumOficio,
                ObsJustificacion = item.ObsJustificacion,
                EstadoBorrador = new CEstadoBorradorDTO
                {
                    IdEntidad = item.EstadoBorrador.PK_EstadoBorrador,
                    DesEstadoBorrador = item.EstadoBorrador.DesEstadoBorrador
                }   
            };
        }


        internal static string DefinirEstadoBorrador(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Registrado";
                    break;
                case 2:
                    respuesta = "En Análisis";
                    break;
                case 3:
                    respuesta = "En Revisión";
                    break;
                case 4:
                    respuesta = "Aprobación RRHH";
                    break;
                case 5:
                    respuesta = "Aprobación Ministro";
                    break;
                case 6:
                    respuesta = "Aprobación Servicio Civil";
                    break;
                case 7:
                    respuesta = "Finalizado";
                    break;
                case 8:
                    respuesta = "Anulado";
                    break;
                default:
                    respuesta = "Indefinido";
                    break;
            }
            return respuesta;
        }


        internal static string DefinirCodigoMovimiento(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Recepción";
                    break;
                case 2:
                    respuesta = "Analista";
                    break;
                case 3:
                    respuesta = "Técnico";
                    break;
                case 4:
                    respuesta = "Cierre";
                    break;
                default:
                    respuesta = "Indefinido";
                    break;
            }
            return respuesta;
        }


        //Se registró en ICBorradorAccionPersonalService y CBorradorAccionPersonalService
        public CBaseDTO AgregarSolicitud(CBorradorAccionPersonalDTO registro,
                                        string codUsuarioEnvia, string codUsuarioRecibe)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);

                BorradorAccionPersonal datosRegistro = new BorradorAccionPersonal
                {
                    NumOficio = registro.NumOficio,
                    UsuarioAsignado = codUsuarioRecibe,
                    ObsJustificacion = registro.ObsJustificacion,
                };

                var estadoBor = intermedioEstado.CargarEstadoBorradorPorID(1);

                if (estadoBor.Codigo != -1)
                {
                    datosRegistro.EstadoBorrador = (EstadoBorrador)estadoBor.Contenido;
                }

                respuesta = intermedio.GuardarSolicitud(datosRegistro, codUsuarioEnvia, codUsuarioRecibe);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }


        //Se registró en ICBorradorAccionPersonalService y CBorradorAccionPersonalService
        public CBaseDTO AgregarBorrador(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                                        CTipoAccionPersonalDTO tipo, CBorradorAccionPersonalDTO registro,
                                        CDetalleBorradorAccionPersonalDTO detalle,
                                        string codUsuarioEnvia, string codUsuarioRecibe)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);

                // Detalle
                DetalleBorradorAccionPersonal datosDetalle = new DetalleBorradorAccionPersonal
                {
                    FecRige = detalle.FecRige,
                    FecVence = detalle.FecVence,
                    FecRigeIntegra = detalle.FecRigeIntegra,
                    FecVenceIntegra = detalle.FecVenceIntegra,
                    CodClase = detalle.CodClase,
                    CodPuesto = detalle.CodPuesto,
                    NumHoras = detalle.NumHoras,
                    Disfrutado = detalle.Disfrutado.ToString(),
                    Autorizado = detalle.Autorizado.ToString(),
                    IndCategoria = detalle.Categoria,
                    MtoSalarioBase = detalle.MtoSalarioBase,
                    MtoAumentosAnuales = detalle.MtoAumentosAnuales,
                    MtoRecargo = detalle.MtoRecargo,
                    MtoGradoGrupo = detalle.NumGradoGrupo,
                    PorProhibicion = detalle.PorProhibicion,
                    MtoOtros = detalle.MtoOtros,
                    Categoria = detalle.Categoria.ToString()
                };

                var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID(tipo.IdEntidad);

                if (entidadTipo.Codigo != -1)
                {
                    datosDetalle.TipoAccionPersonal = (TipoAccionPersonal)entidadTipo.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadTipo).Contenido;
                    throw new Exception();
                }

              
                var estadoNombramiento = intermedioNombramiento.CargarNombramientoCedula(funcionario.Cedula);

                if (estadoNombramiento.PK_Nombramiento != -1)
                {
                    datosDetalle.Nombramiento = estadoNombramiento;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoNombramiento.PK_Nombramiento) };

                    throw new Exception();
                }

                var estadoPrograma = intermedioPrograma.CargarProgramaPorID(detalle.Programa.IdEntidad);
                if (estadoPrograma.PK_Programa != -1)
                {
                    datosDetalle.Programa = estadoPrograma;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoPrograma.PK_Programa) };

                    throw new Exception();
                }

                if (detalle.Seccion != null)
                {
                    var estadoSeccion = intermedioSeccion.CargarSeccionPorID(detalle.Seccion.IdEntidad);

                    if (estadoSeccion != null)
                    {
                        if (estadoSeccion.PK_Seccion != -1)
                        {
                            datosDetalle.Seccion = estadoSeccion;
                        }
                        else
                        {
                            respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoSeccion.PK_Seccion) };

                            throw new Exception();
                        }
                    }
                    else
                    {
                        datosDetalle.Seccion = null;
                    }
                }
                else
                {
                    datosDetalle.Seccion = null;
                }

                respuesta = intermedio.GuardarBorrador(registro.IdEntidad, datosDetalle);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() != typeof(CErrorDTO))
                {
                    registro.EstadoBorrador = new CEstadoBorradorDTO
                    {
                        IdEntidad = 2,
                    };


                    //// Movimiento 
                    CMovimientoBorradorAccionPersonalDTO movimiento = new CMovimientoBorradorAccionPersonalDTO
                    {
                        CodMovimiento = 2,
                        FecMovimiento = DateTime.Now,
                        UsuarioEnvia = codUsuarioEnvia,
                        UsuarioRecibe = codUsuarioRecibe,
                        ObsMovimiento = "Registro Borrador"
                    };

                   respuesta = ActualizarEstado(registro, movimiento);

                   if (respuesta.GetType() != typeof(CErrorDTO))
                   {
                       return respuesta = new CRespuestaDTO
                       {
                           Codigo = 1,
                           Contenido = registro.IdEntidad
                       };
                   }
                   else
                   {
                       respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                       throw new Exception();
                   }
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }


        public CBaseDTO EditarDetalleBorrador(CFuncionarioDTO funcionario,
                                            CEstadoBorradorDTO estado,
                                            CTipoAccionPersonalDTO tipo, 
                                            CBorradorAccionPersonalDTO registro, 
                                            CDetalleBorradorAccionPersonalDTO detalle,
                                            CMovimientoBorradorAccionPersonalDTO movimiento)
        {
            CBaseDTO respuesta;

            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);

               

                 // Detalle
                DetalleBorradorAccionPersonal datosDetalle = new DetalleBorradorAccionPersonal
                {
                    FecRige = detalle.FecRige,
                    FecVence = detalle.FecVence,
                    FecRigeIntegra = detalle.FecRigeIntegra,
                    FecVenceIntegra = detalle.FecVenceIntegra,
                    CodClase = detalle.CodClase,
                    CodPuesto = detalle.CodPuesto,
                    NumHoras = detalle.NumHoras,
                    Disfrutado = detalle.Disfrutado.ToString(),
                    Autorizado = detalle.Autorizado.ToString(),
                    IndCategoria = detalle.Categoria,
                    MtoSalarioBase = detalle.MtoSalarioBase,
                    MtoAumentosAnuales = detalle.MtoAumentosAnuales,
                    MtoRecargo = detalle.MtoRecargo,
                    MtoGradoGrupo = detalle.NumGradoGrupo,
                    PorProhibicion = detalle.PorProhibicion,
                    MtoOtros = detalle.MtoOtros
                };

                var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID(tipo.IdEntidad);

                if (entidadTipo.Codigo != -1)
                {
                    datosDetalle.TipoAccionPersonal = (TipoAccionPersonal)entidadTipo.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadTipo).Contenido;
                    throw new Exception();
                }

              
                var estadoNombramiento = intermedioNombramiento.CargarNombramientoCedula(funcionario.Cedula);

                if (estadoNombramiento.PK_Nombramiento != -1)
                {
                    datosDetalle.Nombramiento = estadoNombramiento;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoNombramiento.PK_Nombramiento) };

                    throw new Exception();
                }

                var estadoPrograma = intermedioPrograma.CargarProgramaPorID(detalle.Programa.IdEntidad);
                if (estadoPrograma.PK_Programa != -1)
                {
                    datosDetalle.Programa = estadoPrograma;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoPrograma.PK_Programa) };

                    throw new Exception();
                }

                if (detalle.Seccion != null)
                {
                    var estadoSeccion = intermedioSeccion.CargarSeccionPorID(detalle.Seccion.IdEntidad);

                    if (estadoSeccion != null)
                    {
                        if (estadoSeccion.PK_Seccion != -1)
                        {
                            datosDetalle.Seccion = estadoSeccion;
                        }
                        else
                        {
                            respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoSeccion.PK_Seccion) };

                            throw new Exception();
                        }
                    }
                    else
                    {
                        datosDetalle.Seccion = null;
                    }
                }
                else
                {
                    datosDetalle.Seccion = null;
                }


                MovimientoBorradorAccionPersonal datosMovimiento = new MovimientoBorradorAccionPersonal
                {
                    BorradorAccionPersonal = new BorradorAccionPersonal
                    {
                        PK_Borrador = registro.IdEntidad
                    },
                    CodMovimiento = movimiento.CodMovimiento,
                    FecMovimiento = DateTime.Now,
                    UsuarioEnvia = movimiento.UsuarioEnvia,
                    UsuarioRecibe = movimiento.UsuarioRecibe,
                    ObsMovimiento = movimiento.ObsMovimiento
                };


                var dato = intermedio.ActualizarDetalleBorrador(registro.IdEntidad, datosDetalle, datosMovimiento);

                if (dato.Codigo > 0)
                {
                    respuesta = new CBorradorAccionPersonalDTO { IdEntidad = Convert.ToInt32(dato.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)dato.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }


        

        //Se registró en ICBorradorAccionPersonalService y CBorradorAccionPersonalService
        public List<CBaseDTO> ObtenerBorrador(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CDetalleBorradorAccionPersonalD intermedioDetalle = new CDetalleBorradorAccionPersonalD(contexto);
                //CUsuarioD intermedioUsuario = new CUsuarioD(contexto);

                var registro = intermedio.ObtenerBorrador(codigo);

                if (registro.Codigo > 0)
                {
                    // Borrador  00
                    var datoRegistro = ConvertirDatosBorradorADto((BorradorAccionPersonal)registro.Contenido);
                    respuesta.Add(datoRegistro);


                    // Estado Borrador 01
                    var datoEstado = intermedioEstado.CargarEstadoBorradorPorID
                        (((BorradorAccionPersonal)registro.Contenido).EstadoBorrador.PK_EstadoBorrador);

                    respuesta.Add(CEstadoBorradorL.ConvertirEstadoBorradorADto((EstadoBorrador)datoEstado.Contenido));


                    // Usuario Asignado 02
                    var datoUsuario = intermedioFuncionario.BuscarFuncionarioUsuario(((BorradorAccionPersonal)registro.Contenido).UsuarioAsignado);
                    respuesta.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO((Funcionario)datoUsuario.Contenido));


                    // Detalle Borrador  03
                    var datoDetalle = intermedioDetalle.ObtenerDetalle
                     (((BorradorAccionPersonal)registro.Contenido).PK_Borrador);

                    if (datoDetalle.Codigo > 0)
                    {
                        respuesta.Add(CDetalleBorradorAccionPersonalL.ConvertirDatosDetalleADto((DetalleBorradorAccionPersonal)datoDetalle.Contenido));
                        
                        // Tipo Acción Personal 04
                        var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID
                           (((DetalleBorradorAccionPersonal)datoDetalle.Contenido).TipoAccionPersonal.PK_TipoAccionPersonal);

                        respuesta.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto((TipoAccionPersonal)entidadTipo.Contenido));


                        // Funcionario 05
                        var funcionario = ((DetalleBorradorAccionPersonal)datoDetalle.Contenido).Nombramiento.Funcionario;

                        respuesta.Add(new CFuncionarioDTO
                        {
                            Cedula = funcionario.IdCedulaFuncionario,
                            Nombre = funcionario.NomFuncionario,
                            PrimerApellido = funcionario.NomPrimerApellido,
                            SegundoApellido = funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        });
                    }
                    else
                    {
                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No tiene Detalle registrado"
                        });

                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No tiene Tipo de Acción de Personal"
                        });

                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No tiene Funcionario asociado"
                        });
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)registro.Contenido);
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


        //Se registró en ICBorradorAccionPersonalService y CBorradorAccionPersonalService
        public List<List<CBaseDTO>> BuscarSolicitud(CBorradorAccionPersonalDTO registro)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            List<BorradorAccionPersonal> datosRegistro = new List<BorradorAccionPersonal>();

            if (registro != null)
            {
                var resultado = intermedio.BuscarSolicitud(registro);

                if (resultado.Codigo > 0)
                {
                    datosRegistro = (List<BorradorAccionPersonal>)resultado.Contenido;
                }
            }

            if (datosRegistro.Count > 0)
            {
                foreach (var item in datosRegistro)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    CBorradorAccionPersonalDTO tempBorradorAccionPersonal = ConvertirDatosBorradorADto(item);
                    temp.Add(tempBorradorAccionPersonal);

                    CEstadoBorradorDTO tempEstado = CEstadoBorradorL.ConvertirEstadoBorradorADto(item.EstadoBorrador);
                    temp.Add(tempEstado);

                    // Usuario Asignado
                    var datoUsuario = intermedioFuncionario.BuscarFuncionarioUsuario(item.UsuarioAsignado);
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO((Funcionario)datoUsuario.Contenido));

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }



        //Se registró en ICBorradorAccionPersonalService y CBorradorAccionPersonalService
        public List<List<CBaseDTO>> BuscarBorrador( CFuncionarioDTO funcionario,
                                                    CBorradorAccionPersonalDTO registro,
                                                    CDetalleBorradorAccionPersonalDTO detalle,
                                                    List<DateTime> fechasRige,
                                                    List<DateTime> fechasVence)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);

            List<BorradorAccionPersonal> datosRegistro = new List<BorradorAccionPersonal>();

            if (registro != null || funcionario != null || detalle != null
               || fechasRige.Count() > 0 || fechasVence.Count() > 0)
            {
                var resultado = intermedio.BuscarBorrador(funcionario, registro, detalle, fechasRige, fechasVence);

                if (resultado.Codigo > 0)
                {
                    datosRegistro = (List<BorradorAccionPersonal>)resultado.Contenido;
                }
            }

            if (datosRegistro.Count > 0)
            {
                foreach (var item in datosRegistro)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    CBorradorAccionPersonalDTO tempBorradorAccionPersonal = ConvertirDatosBorradorADto(item);
                    temp.Add(tempBorradorAccionPersonal);

                    temp.Add(CDetalleBorradorAccionPersonalL.ConvertirDatosDetalleADto(item.DetalleBorradorAccionPersonal.FirstOrDefault()));

                    temp.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto(item.DetalleBorradorAccionPersonal.FirstOrDefault().TipoAccionPersonal));

                    CEstadoBorradorDTO tempestado = CEstadoBorradorL.ConvertirEstadoBorradorADto(item.EstadoBorrador);
                    temp.Add(tempestado);

                    CFuncionarioDTO tempFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.DetalleBorradorAccionPersonal.FirstOrDefault().Nombramiento.Funcionario);
                    temp.Add(tempFuncionario);

                    // Usuario Asignado
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    var datoUsuario = intermedioFuncionario.BuscarFuncionarioUsuario(item.UsuarioAsignado);
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO((Funcionario)datoUsuario.Contenido));

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }


        public CBaseDTO ActualizarEstado(CBorradorAccionPersonalDTO registro, CMovimientoBorradorAccionPersonalDTO movimiento)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);

                var borradorBD = new BorradorAccionPersonal
                {
                    PK_Borrador = registro.IdEntidad,
                    EstadoBorrador = new EstadoBorrador
                    {
                        PK_EstadoBorrador = movimiento.CodMovimiento,
                        DesEstadoBorrador = DefinirEstadoBorrador(movimiento.CodMovimiento)
                    },
                    NumOficio = registro.NumOficio,
                    UsuarioAsignado = registro.UsuarioAsignado,
                    ObsJustificacion = registro.ObsJustificacion
                };

                MovimientoBorradorAccionPersonal movimientoBD = new MovimientoBorradorAccionPersonal
                {
                    BorradorAccionPersonal = new BorradorAccionPersonal
                    {
                        PK_Borrador = registro.IdEntidad
                    },
                    CodMovimiento = movimiento.CodMovimiento,
                    FecMovimiento = DateTime.Now,
                    UsuarioEnvia    = movimiento.UsuarioEnvia,
                    UsuarioRecibe = movimiento.UsuarioRecibe,
                    ObsMovimiento = movimiento.ObsMovimiento
                };

                var datosMovimiento = intermedio.ActualizarEstado(borradorBD, movimientoBD);

                if (datosMovimiento.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = registro.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosMovimiento.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        public List<List<CBaseDTO>> FuncionariosConBorradores()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CBorradorAccionPersonalD intermedio = new CBorradorAccionPersonalD(contexto);
                var resultado = intermedio.FuncionariosConBorradores();
                if (resultado.Codigo > 0)
                {
                    var lista = (List<Funcionario>)resultado.Contenido;

                    foreach (var item in lista)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item));
                        var nombramiento = ((Funcionario)item).Nombramiento
                                            .Where(N => N.FecVence == null).FirstOrDefault();
                        temp.Add(CPuestoL.ConstruirPuesto(nombramiento.Puesto, new CPuestoDTO()));
                        temp.Add(CDetallePuestoL.ConstruirDetallePuesto(nombramiento.Puesto
                                                            .DetallePuesto.FirstOrDefault()));
                        if (nombramiento.DetalleBorradorAccionPersonal.Count > 0)
                        {
                            temp.Add(CBorradorAccionPersonalL.ConvertirDatosBorradorADto(nombramiento.DetalleBorradorAccionPersonal.FirstOrDefault().BorradorAccionPersonal));
                            //((CCaucionDTO)temp.Last()).DetalleEstadoPoliza = CCaucionL.DefinirEstadoPoliza(((CCaucionDTO)temp.Last()).EstadoPoliza);        
                        }
                        else
                        {
                            temp.Add(new CBaseDTO { IdEntidad = -1, Mensaje = "El funcionario no tiene borradores de acción de personal registrados" });
                        }
                        respuesta.Add(temp);
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

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using System.Data;
using SIRH.DTO;

//Cambio para subirle a Orlando
namespace SIRH.Logica
{
    public class CPuestoL
    {
        #region Variables

        public CPuestoD puestoDescarga;
        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPuestoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CPuestoDTO PuestoGeneral(Puesto item)
        {
            CPuestoDTO respuesta = new CPuestoDTO
            {
                IdEntidad = item.PK_Puesto,
                CodPuesto = item.CodPuesto,
                UbicacionAdministrativa = CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(item.UbicacionAdministrativa),
                EstadoPuesto = item.EstadoPuesto != null ? new CEstadoPuestoDTO
                {
                    IdEntidad = item.EstadoPuesto.PK_EstadoPuesto,
                    DesEstadoPuesto = item.EstadoPuesto.DesEstadoPuesto
                } : new CEstadoPuestoDTO()
            };

            return respuesta;
        }
        internal static CPuestoDTO ConvertirCPuestoGeneralDatosaDTO(Puesto item)
        {
            CPuestoDTO respuesta = new CPuestoDTO
            {
                IdEntidad = item.PK_Puesto,
                CodPuesto = item.CodPuesto,
                UbicacionAdministrativa = CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(item.UbicacionAdministrativa),

            };

            return respuesta;
        }

        internal static Puesto ConvertirCPuestoGeneralDTOaDatos(CPuestoDTO item)
        {
            Puesto respuesta = new Puesto
            {
                PK_Puesto = item.IdEntidad,
                CodPuesto = item.CodPuesto,
                UbicacionAdministrativa = CUbicacionAdministrativaL.ConvertirDTOUbicacionAdministrativaADatos(item.UbicacionAdministrativa)

            };

            return respuesta;
        }
        internal static CPuestoDTO ConstruirPuesto(Puesto entrada, CPuestoDTO salida)
        {
            salida.IdEntidad = entrada.PK_Puesto;
            salida.CodPuesto = entrada.CodPuesto;
            salida.ObservacionesPuesto = entrada.ObsPuesto;
            salida.PuestoConfianza = Convert.ToBoolean(entrada.IndPuestoConfianza) == true ? true : false;
            salida.NivelOcupacional = Convert.ToInt32(entrada.IndNivelOcupacional);
            
            if (entrada.EstadoPuesto != null)
            {
                salida.EstadoPuesto = new CEstadoPuestoDTO
                {
                    IdEntidad = entrada.EstadoPuesto.PK_EstadoPuesto,
                    DesEstadoPuesto = entrada.EstadoPuesto.DesEstadoPuesto
                };
            }
            else
            {
                salida.EstadoPuesto = new CEstadoPuestoDTO
                {
                    Mensaje = "No se encontró el estado del puesto"
                };
            }
            if (entrada.UbicacionAdministrativa != null)
            {
                salida.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                {
                    IdEntidad = entrada.UbicacionAdministrativa.PK_UbicacionAdministrativa,
                    DesObservaciones = entrada.UbicacionAdministrativa.DesObservaciones
                };
                if (entrada.UbicacionAdministrativa.Division != null)
                {
                    salida.UbicacionAdministrativa.Division = new CDivisionDTO
                    {
                        IdEntidad = entrada.UbicacionAdministrativa.Division.PK_Division,
                        NomDivision = entrada.UbicacionAdministrativa.Division.NomDivision
                    };
                }
                else
                {
                    salida.UbicacionAdministrativa.Division = new CDivisionDTO
                    {
                        Mensaje = "No se encontró la división"
                    };
                }
                if (entrada.UbicacionAdministrativa.DireccionGeneral != null)
                {
                    salida.UbicacionAdministrativa.DireccionGeneral = new CDireccionGeneralDTO
                    {
                        IdEntidad = entrada.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral,
                        NomDireccion = entrada.UbicacionAdministrativa.DireccionGeneral.NomDireccion
                    };
                }
                else
                {
                    salida.UbicacionAdministrativa.DireccionGeneral = new CDireccionGeneralDTO
                    {
                        Mensaje = "No se encontró la dirección general"
                    };
                }
                if (entrada.UbicacionAdministrativa.Departamento != null)
                {
                    salida.UbicacionAdministrativa.Departamento = new CDepartamentoDTO
                    {
                        IdEntidad = entrada.UbicacionAdministrativa.Departamento.PK_Departamento,
                        NomDepartamento = entrada.UbicacionAdministrativa.Departamento.NomDepartamento
                    };
                }
                else
                {
                    salida.UbicacionAdministrativa.Departamento = new CDepartamentoDTO
                    {
                        Mensaje = "No se encontró el departamento"
                    };
                }
                if (entrada.UbicacionAdministrativa.Seccion != null)
                {
                    salida.UbicacionAdministrativa.Seccion = new CSeccionDTO
                    {
                        IdEntidad = entrada.UbicacionAdministrativa.Seccion.PK_Seccion,
                        NomSeccion = entrada.UbicacionAdministrativa.Seccion.NomSeccion
                    };
                }
                else
                {
                    salida.UbicacionAdministrativa.Seccion = new CSeccionDTO
                    {
                        Mensaje = "No se encontró la sección"
                    };
                }
                if (entrada.UbicacionAdministrativa.Presupuesto != null)
                {
                    salida.UbicacionAdministrativa.Presupuesto = new CPresupuestoDTO
                    {
                        IdEntidad = entrada.UbicacionAdministrativa.Presupuesto.PK_Presupuesto,
                        IdDireccionPresupuestaria = entrada.UbicacionAdministrativa.Presupuesto.IdDireccionPresupuestaria,
                        Area = entrada.UbicacionAdministrativa.Presupuesto.CodArea,
                        Actividad = entrada.UbicacionAdministrativa.Presupuesto.CodActividad,
                        CodigoPresupuesto = entrada.UbicacionAdministrativa.Presupuesto.IdPresupuesto
                    };
                }
                else
                {
                    salida.UbicacionAdministrativa.Presupuesto = new CPresupuestoDTO
                    {
                        Mensaje = "No se encontró el presupuesto"
                    };
                }
            }
            else
            {
                salida.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                {
                    Mensaje = "No se encontró ubicación administrativa"
                };
            }

            salida.FamiliaPuesto = new CFamiliaPuestoDTO
            {
                IdEntidad = entrada.FamiliaPuestos.PK_Familia,
                DesFamilia = entrada.FamiliaPuestos.DesFamilia,
                DesObservaciones = entrada.FamiliaPuestos.DesObservaciones
            };

            return salida;
        }

        //Se insertó en CPuestoSercice y ICPuestoService el 27/01/2017
        public List<CBaseDTO> DetallePuestoPorCodigo(string codPuesto)
        {
            List<CBaseDTO> retorno = new List<CBaseDTO>();

            CPuestoDTO respuesta = new CPuestoDTO();
            CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();
            CUbicacionPuestoDTO ubicacionPuesto;

            puestoDescarga = new CPuestoD(contexto);

            var datoDescargado = puestoDescarga.DetallePuestoPorCodigo(codPuesto);
            

            if (datoDescargado != null)
            {
                var detalleDescargado = datoDescargado.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).FirstOrDefault();
                var ubicacionDescargada = datoDescargado.RelPuestoUbicacion;
                respuesta = ConstruirPuesto(datoDescargado, respuesta);
                detallePuesto = ConstruirDetallePuesto(detalleDescargado, detallePuesto);
                retorno.Add(respuesta);
                retorno.Add(detallePuesto);
                foreach (var item in ubicacionDescargada)
                {
                    ubicacionPuesto = new CUbicacionPuestoDTO();
                    ubicacionPuesto = ConstruirUbicacionPuesto(item, ubicacionPuesto);
                    retorno.Add(ubicacionPuesto);
                }
            }
            else
            {
                respuesta = new CPuestoDTO
                {
                    Mensaje = "No se encontraron resultados."
                };
                retorno.Add(respuesta);
            }

            return retorno;
        }

        private CUbicacionPuestoDTO ConstruirUbicacionPuesto(RelPuestoUbicacion entrada, CUbicacionPuestoDTO salida)
        {
            salida.IdEntidad = entrada.UbicacionPuesto.PK_UbicacionPuesto;
            salida.ObsUbicacionPuesto = entrada.UbicacionPuesto.ObsUbicacionPuesto;
            salida.TipoUbicacion = new CTipoUbicacionDTO
            {
                IdEntidad = entrada.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion,
                DesTipoUbicacion = entrada.UbicacionPuesto.TipoUbicacion.DesTipoUbicacion
            };
            salida.Distrito = new CDistritoDTO
            {
                IdEntidad = entrada.UbicacionPuesto.Distrito.PK_Distrito,
                NomDistrito = entrada.UbicacionPuesto.Distrito.NomDistrito,
                Canton = new CCantonDTO
                {
                    IdEntidad = entrada.UbicacionPuesto.Distrito.Canton.PK_Canton,
                    NomCanton = entrada.UbicacionPuesto.Distrito.Canton.NomCanton,
                    Provincia = new CProvinciaDTO
                    {
                        IdEntidad = entrada.UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia,
                        NomProvincia = entrada.UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                    }
                }
            };
            return salida;
        }

        //Se insertó en CPuestoSercice y ICPuestoService el 27/01/2017
        public List<CBaseDTO> DetallePuestoPorCedula(string cedula)
        {
            try
            {
                List<CBaseDTO> retorno = new List<CBaseDTO>();

                CPuestoDTO respuesta = new CPuestoDTO();
                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();
                CUbicacionPuestoDTO ubicacionPuesto;

                puestoDescarga = new CPuestoD(contexto);

                var datoDescargado = puestoDescarga.DetallePuestoCedula(cedula);

                if (datoDescargado != null)
                {
                    if(datoDescargado.DetallePuesto != null)
                    {
                        if (datoDescargado.RelPuestoUbicacion != null) {
                            var detalleDescargado = datoDescargado.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                            var ubicacionDescargada = datoDescargado.RelPuestoUbicacion;

                            if (detalleDescargado == null)
                                throw new Exception("No se encontró Detalle Puesto para el funcionario" + " " + cedula);

                            respuesta = ConstruirPuesto(datoDescargado, respuesta);
                            detallePuesto = ConstruirDetallePuesto(detalleDescargado, detallePuesto);
                            retorno.Add(respuesta);
                            retorno.Add(detallePuesto);
                            foreach (var item in ubicacionDescargada)
                            {
                                ubicacionPuesto = new CUbicacionPuestoDTO();
                                ubicacionPuesto = ConstruirUbicacionPuesto(item, ubicacionPuesto);
                                retorno.Add(ubicacionPuesto);
                            }
                        }else
                        {
                            throw new Exception("No se encontró Relación Ubicación Puesto para el funcionario"+ " "+cedula);
                        }

                    }else
                    {
                        throw new Exception("No se encontró Detalle Puesto para el funcionario" + " " + cedula);
                    }
                    
                }
                else
                {
                    respuesta = new CPuestoDTO
                    {
                        Mensaje = "No se encontraron resultados."
                    };
                    retorno.Add(respuesta);
                }

                return retorno;
            }
            catch(Exception error)
            {
                List<CBaseDTO> retorno = new List<CBaseDTO>();
                retorno.Add(new CErrorDTO {
                    MensajeError = error.Message
                });
                return retorno;
            }
        }

        private CDetallePuestoDTO ConstruirDetallePuesto(DetallePuesto entrada, CDetallePuestoDTO salida)
        {
            salida.IdEntidad = entrada.PK_DetallePuesto;
            salida.Categoria = Convert.ToInt32(entrada.EscalaSalarial.IndCategoria);
            salida.PorDedicacion = Convert.ToDecimal(entrada.PorDedicacion);
            salida.PorProhibicion = Convert.ToDecimal(entrada.PorProhibicion);
            if (entrada.Clase != null)
            {
                salida.Clase = new CClaseDTO
                {
                    IdEntidad = entrada.Clase.PK_Clase,
                    DesClase = entrada.Clase.DesClase
                };
            }
            else
            {
                salida.Clase = new CClaseDTO { Mensaje = "No se encontró la clase" };
            }
            if (entrada.Especialidad != null)
            {
                salida.Especialidad = new CEspecialidadDTO
                {
                    IdEntidad = entrada.Especialidad.PK_Especialidad,
                    DesEspecialidad = entrada.Especialidad.DesEspecialidad
                };
            }
            else
            {
                salida.Especialidad = new CEspecialidadDTO { Mensaje = "No se encontró la clase" };
            }
            if (entrada.OcupacionReal != null)
            {
                salida.OcupacionReal = new COcupacionRealDTO
                {
                    IdEntidad = entrada.OcupacionReal.PK_OcupacionReal,
                    DesOcupacionReal = entrada.OcupacionReal.DesOcupacionReal
                };
            }
            else
            {
                salida.OcupacionReal = new COcupacionRealDTO { Mensaje = "No se encontró la ocupación real" };
            }
            if (entrada.SubEspecialidad != null)
            {
                salida.SubEspecialidad = new CSubEspecialidadDTO
                {
                    IdEntidad = entrada.SubEspecialidad.PK_SubEspecialidad,
                    DesSubEspecialidad = entrada.SubEspecialidad.DesSubEspecialidad
                };
            }
            else
            {
                salida.SubEspecialidad = new CSubEspecialidadDTO { Mensaje = "No se encontró la subespecialidad" };
            }
            if (entrada.EscalaSalarial != null)
            {
                salida.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = entrada.EscalaSalarial.PK_Escala,
                    SalarioBase = Convert.ToDecimal(entrada.EscalaSalarial.MtoSalarioBase),
                    MontoAumentoAnual = Convert.ToDecimal(entrada.EscalaSalarial.MtoAumento),
                    CategoriaEscala = Convert.ToInt32(entrada.EscalaSalarial.IndCategoria)
                };
            }

            salida.PorDedicacion = Convert.ToDecimal(entrada.PorDedicacion);
            salida.PorProhibicion = Convert.ToDecimal(entrada.PorProhibicion);

            return salida;
        }

        //Se insertó en ICPuestoService y CPuestoService el 27/01/2017
        public List<CBaseDTO> CargarPuestoActivo(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            puestoDescarga = new CPuestoD(contexto);
            var item = puestoDescarga.DetallePuestoCedula(cedula);


            if (item != null)
            {
                resultado.Add(new CPuestoDTO { IdEntidad = item.PK_Puesto, CodPuesto = item.CodPuesto, EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto } });
                resultado.Add(new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto, DesEstadoPuesto = item.EstadoPuesto.DesEstadoPuesto });


                resultado.Add(new CClaseDTO
                {
                    IdEntidad = item.DetallePuesto.FirstOrDefault().Clase != null ? item.DetallePuesto.FirstOrDefault().Clase.PK_Clase: 0,
                    DesClase = item.DetallePuesto.FirstOrDefault().Clase != null ? item.DetallePuesto.FirstOrDefault().Clase.DesClase : ""
                });
                resultado.Add(new CEspecialidadDTO
                {
                    IdEntidad = item.DetallePuesto.FirstOrDefault().Especialidad != null ? item.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad : 0,
                    DesEspecialidad = item.DetallePuesto.FirstOrDefault().Especialidad != null ? item.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad : "NO TIENE"
                });
                if (item.DetallePuesto.FirstOrDefault().SubEspecialidad != null)
                {
                    resultado.Add(new CSubEspecialidadDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                        DesSubEspecialidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
                if (item.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    resultado.Add(new COcupacionRealDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = item.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
                resultado.Add(new CDetallePuestoDTO
                {
                    IdEntidad = item.DetallePuesto.FirstOrDefault().PK_DetallePuesto
                });
            }


            return resultado;
        }

        //INSERTADO EN ICPuestoService y CPuestoService el 26/01/2017
        public List<CPuestoDTO> BuscarPuestoParams(string codpuesto, int clase, int especialidad, int ocupacion)
        {
            List<CPuestoDTO> resultado = new List<CPuestoDTO>();

            puestoDescarga = new CPuestoD(contexto);

            var item = puestoDescarga.CargarPuestosDetalle(codpuesto, clase, especialidad, ocupacion);

            foreach (var aux in item)
            {
                CPuestoDTO temp = new CPuestoDTO();
                temp.CodPuesto = aux.CodPuesto;
                temp.IdEntidad = aux.PK_Puesto;
                temp.EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = aux.EstadoPuesto.PK_EstadoPuesto };
                temp.UbicacionAdministrativa = new CUbicacionAdministrativaDTO { IdEntidad = aux.UbicacionAdministrativa.PK_UbicacionAdministrativa };
            }

            return resultado;
        }

        //INSERTADO EN ICPuestoService y CPuestoService (DEIVERT)
        public CPuestoDTO BuscarPuestoCodigo(string codPuesto)
        {
            //EN ESTA VARIABLE ALMACENAMOS LOS DATOS QUE VIENEN DESDE LA BD Y QUE VAMOS A ENVIAR A LA CAPA INTERFACE
            CPuestoDTO resultado = new CPuestoDTO();

            //AQUI VAMOS A ALMACENAR TEMPORALMENTE, LOS DATOS EXTRAÍDOS DESDE LA BD
            Puesto puestoBD;

            //ESTA ES LA VARIABLE INTERMEDIA QUE NOS VA A PERMITIR ACCEDER A LOS METODOS DE LA CAPA DE DATOS
            puestoDescarga = new CPuestoD(contexto);

            //EN ESTA OPERACIÓN, UTILIZAMOS LA VARIABLE DE CAPA DE DATOS PARA EXTRAER LA INFORMACIÓN DEL PUESTO, ALMACENANDOLA
            //EN LA VARIABLE TEMPORAL
            puestoBD = puestoDescarga.CargarPuestoParam(codPuesto);


            //POR ULTIMO SE AGREGA LA FILA O LAS FILAS NECESARIAS EN LA TABLA SEGUN LAS COLUMNAS CREADAS CON LA INFORMACION DE LA VARIABLE
            //TEMPORAL
            resultado.CodPuesto = puestoBD.CodPuesto;
            resultado.IdEntidad = puestoBD.PK_Puesto;
            resultado.EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = puestoBD.EstadoPuesto.PK_EstadoPuesto };
            resultado.UbicacionAdministrativa = new CUbicacionAdministrativaDTO { IdEntidad = puestoBD.UbicacionAdministrativa.PK_UbicacionAdministrativa };

            //SE RETORNA EL RESULTADO
            return resultado;
        }

        public int ActualizarMovimientoPuesto(string codPuesto, string numOficio, int motivoVacante, DateTime fechaVacante, int idEstadoPuesto)
        {
            int resultado = 0;

            Puesto puestoBD;

            EstadoPuesto estadoBD;

            CEstadoPuestoD estadoPuesto = new CEstadoPuestoD(contexto);

            puestoDescarga = new CPuestoD(contexto);

            puestoBD = puestoDescarga.CargarPuestoParam(codPuesto);

            estadoBD = estadoPuesto.CargarEstadoPuestoPorID(idEstadoPuesto);

            puestoBD.EstadoPuesto = estadoBD;

            MotivoMovimiento motivoMovBD;
            CMotivoMovimientoD motivoMov = new CMotivoMovimientoD(contexto);

            motivoMovBD = motivoMov.CargarMotivoMovimientoPorPuesto(codPuesto);

            MovimientoPuesto movimientoBD = new MovimientoPuesto();
            CMovimientoPuestoD movimientoPuesto = new CMovimientoPuestoD(contexto);

            movimientoBD.Puesto = puestoBD;
            movimientoBD.CodOficio = numOficio;
            movimientoBD.MotivoMovimiento = motivoMovBD;
            movimientoBD.FecMovimiento = Convert.ToDateTime(fechaVacante);

            resultado = movimientoPuesto.GuardarMovimientoPuesto(movimientoBD);

            return resultado;
        }

        public List<CBaseDTO> DescargarPuestoVacante(string codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);

                var resultado = intermedio.DescargarPuestoVacante(codigo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                    respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault().UbicacionPuesto));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> DescargarPuestoCompleto(string codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);

                var resultado = intermedio.DescargarPuestoCompleto(codigo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                    if (((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault() != null)
                        respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault().UbicacionPuesto));
                    else
                        respuesta.Add(new CUbicacionPuestoDTO());

                    respuesta.Add(CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(((Puesto)resultado.Contenido).UbicacionAdministrativa));

                    if (((Puesto)resultado.Contenido).Nombramiento.Count > 0)
                    {
                        var ultimoNombramiento = ((Puesto)resultado.Contenido).Nombramiento.Where(Q => Q.FecVence == null || Q.FecVence > DateTime.Now).OrderByDescending(O => O.FecRige).FirstOrDefault();
                        if (ultimoNombramiento != null)
                        {
                            respuesta.Add(CFuncionarioL.FuncionarioGeneral(ultimoNombramiento.Funcionario));
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
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> DescargarPuestoPedimento(string pedimento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);

                var resultado = intermedio.DescargarPuestoPedimento(pedimento);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                    if (((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault() != null)
                    {
                        respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                 .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1
                                                                                    && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                        respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                    .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 2
                                                                                       && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                    }
                    else
                    {
                        respuesta.Add(new CUbicacionPuestoDTO());
                        respuesta.Add(new CUbicacionPuestoDTO());

                    }
                    respuesta.Add(CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(((Puesto)resultado.Contenido).PedimentoPuesto.FirstOrDefault()));
                    respuesta.Add(CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(((Puesto)resultado.Contenido).UbicacionAdministrativa));

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> DescargarPerfilPuestoAccionesFuncionario(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);
                var resultado = intermedio.DescargarPerfilPuestoAccionesFuncionario(cedula);

                if (resultado.Codigo > 0)
                {
                    respuesta = ConstruirPuestoAcciones(resultado);

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> DescargarPerfilPuestoAcciones(string codPuesto)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);
                var resultado = intermedio.DescargarPerfilPuestoAcciones(codPuesto);
                Nombramiento funcionarioPropiedad = null;
                Nombramiento funcionarioInterino = null;

                if (resultado.Codigo > 0)
                {
                    //Almacena el funcionario
                    //Busca el nombramiento activo o bien el último nombramiento asociado al puesto
                    respuesta = ConstruirPuestoAcciones(resultado);

                    // Busca los datos del Propietario y Ocupante actuales del Puesto
                    var resultadoOcupante = intermedio.DescargarPuestoOcupantes(codPuesto);
                    if (resultadoOcupante[2].Codigo > 0)
                    {
                        funcionarioPropiedad = (Nombramiento)resultadoOcupante[2].Contenido;
                        respuesta.Add(new List<CBaseDTO> {
                            new CNombramientoDTO
                            {
                                FecRige = Convert.ToDateTime(funcionarioPropiedad.FecRige),
                                FecVence = Convert.ToDateTime(funcionarioPropiedad.FecVence),
                                Funcionario = new CFuncionarioDTO
                                {
                                    Cedula = funcionarioPropiedad.Funcionario.IdCedulaFuncionario,
                                    Nombre = funcionarioPropiedad.Funcionario.NomFuncionario,
                                    PrimerApellido = funcionarioPropiedad.Funcionario.NomPrimerApellido,
                                    SegundoApellido = funcionarioPropiedad.Funcionario.NomSegundoApellido,
                                    Sexo = (GeneroEnum)Convert.ToInt32(funcionarioPropiedad.Funcionario.IndSexo)
                                },
                                Puesto = new CPuestoDTO
                                {
                                    CodPuesto = funcionarioPropiedad.Puesto.CodPuesto
                                },
                                EstadoNombramiento = new CEstadoNombramientoDTO
                                {
                                    IdEntidad = funcionarioPropiedad.EstadoNombramiento.PK_EstadoNombramiento,
                                    DesEstado = funcionarioPropiedad.EstadoNombramiento.DesEstado
                                }
                            }
                        });
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { IdEntidad = -1 } });
                    }
                    if (resultadoOcupante[1].Codigo > 0)
                    {
                        funcionarioInterino = (Nombramiento)(resultadoOcupante[1].Contenido);
                        if (funcionarioPropiedad == null)
                        {
                            respuesta.Add(new List<CBaseDTO> {
                            new CNombramientoDTO
                            {
                                FecRige = Convert.ToDateTime(funcionarioInterino.FecRige),
                                FecVence = Convert.ToDateTime(funcionarioInterino.FecVence),
                                Funcionario = new CFuncionarioDTO
                                {
                                    Cedula = funcionarioInterino.Funcionario.IdCedulaFuncionario,
                                    Nombre = funcionarioInterino.Funcionario.NomFuncionario,
                                    PrimerApellido = funcionarioInterino.Funcionario.NomPrimerApellido,
                                    SegundoApellido = funcionarioInterino.Funcionario.NomSegundoApellido,
                                    Sexo = (GeneroEnum)Convert.ToInt32(funcionarioInterino.Funcionario.IndSexo)
                                },
                                EstadoNombramiento = new CEstadoNombramientoDTO
                                {
                                    IdEntidad = funcionarioInterino.EstadoNombramiento.PK_EstadoNombramiento,
                                    DesEstado = funcionarioInterino.EstadoNombramiento.DesEstado
                                }
                            }
                            });
                        }
                        else
                        { 
                            if (funcionarioInterino.Funcionario.IdCedulaFuncionario != funcionarioPropiedad.Funcionario.IdCedulaFuncionario)
                            {
                                respuesta.Add(new List<CBaseDTO> {
                                new CNombramientoDTO
                                {
                                    FecRige = Convert.ToDateTime(funcionarioInterino.FecRige),
                                    FecVence = Convert.ToDateTime(funcionarioInterino.FecVence),
                                    Funcionario = new CFuncionarioDTO
                                    {
                                        Cedula = funcionarioInterino.Funcionario.IdCedulaFuncionario,
                                        Nombre = funcionarioInterino.Funcionario.NomFuncionario,
                                        PrimerApellido = funcionarioInterino.Funcionario.NomPrimerApellido,
                                        SegundoApellido = funcionarioInterino.Funcionario.NomSegundoApellido,
                                        Sexo = (GeneroEnum)Convert.ToInt32(funcionarioInterino.Funcionario.IndSexo)
                                    },
                                    EstadoNombramiento = new CEstadoNombramientoDTO
                                    {
                                        IdEntidad = funcionarioInterino.EstadoNombramiento.PK_EstadoNombramiento,
                                        DesEstado = funcionarioInterino.EstadoNombramiento.DesEstado
                                    }
                                }
                                });
                            }
                            else
                            {
                                var nombramientoActual = contexto.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == funcionarioInterino.Funcionario.IdCedulaFuncionario
                                                                                    && (N.FecVence == null || N.FecVence >= DateTime.Now)).OrderByDescending(F => F.FecRige).FirstOrDefault();
                                if (nombramientoActual.Puesto.CodPuesto == funcionarioPropiedad.Puesto.CodPuesto && funcionarioPropiedad.FK_EstadoNombramiento == 1)
                                {
                                    respuesta.Add(new List<CBaseDTO> {
                                    new CNombramientoDTO
                                    {
                                        FecRige = Convert.ToDateTime(funcionarioInterino.FecRige),
                                        FecVence = Convert.ToDateTime(funcionarioInterino.FecVence),
                                        Funcionario = new CFuncionarioDTO
                                        {
                                            Cedula = funcionarioInterino.Funcionario.IdCedulaFuncionario,
                                            Nombre = funcionarioInterino.Funcionario.NomFuncionario,
                                            PrimerApellido = funcionarioInterino.Funcionario.NomPrimerApellido,
                                            SegundoApellido = funcionarioInterino.Funcionario.NomSegundoApellido,
                                            Sexo = (GeneroEnum)Convert.ToInt32(funcionarioInterino.Funcionario.IndSexo)
                                        },
                                        EstadoNombramiento = new CEstadoNombramientoDTO
                                        {
                                            IdEntidad = funcionarioInterino.EstadoNombramiento.PK_EstadoNombramiento,
                                            DesEstado = funcionarioInterino.EstadoNombramiento.DesEstado
                                        }
                                    }
                                    });
                                }
                                else
                                {
                                    respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { IdEntidad = -1 } });
                                }
                            }
                        }
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { IdEntidad = -1 } });
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
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }
        }

        private List<List<CBaseDTO>> ConstruirPuestoAcciones(CRespuestaDTO resultado)
        {
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
                //FECHA LIMITE DE DÍAS PARA CERRAR EL NOMBRAMIENTO
                DateTime fechaLimite = DateTime.Now.AddDays(-5);
                var nombramiento = ((Puesto)resultado.Contenido).Nombramiento.OrderByDescending(Q=>Q.FecRige).FirstOrDefault(Q => (Q.FecVence == null || Q.FecVence >= fechaLimite)
                && Q.FK_EstadoNombramiento != 6 && Q.FK_EstadoNombramiento != 7 && Q.FK_EstadoNombramiento != 8 && Q.FK_EstadoNombramiento != 10 && Q.FK_EstadoNombramiento != 15);
                if (nombramiento != null)
                {
                    var colaborador = CFuncionarioL.FuncionarioGeneral(nombramiento.Funcionario);
                    colaborador.Mensaje = nombramiento.Funcionario.ExpedienteFuncionario.FirstOrDefault() != null ?
                                          nombramiento.Funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente.ToString() :
                                          "Sin Expediente";
                    respuesta.Add(new List<CBaseDTO> { colaborador });
                    respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento) });
                    respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(nombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
                }
                else
                {
                    var ultimoNombramiento = ((Puesto)resultado.Contenido).Nombramiento.OrderByDescending(Q => Q.FecVence).ThenBy(Q => Q.FecRige).FirstOrDefault();
                    if (ultimoNombramiento != null)
                    {
                        respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral(ultimoNombramiento.Funcionario) });
                        respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(ultimoNombramiento) });
                        respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(ultimoNombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
                        nombramiento = ultimoNombramiento; // Asignar el nombramiento
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CFuncionarioDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" , Sexo = GeneroEnum.Indefinido} });
                        respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { Mensaje = "Este puesto no ha registrado ningún nombramiento" } });
                        respuesta.Add(new List<CBaseDTO> { new CDetalleContratacionDTO { Mensaje = "No hay contratos asociados a este puesto " } });
                    }
                }

                //Almacena el puesto
                var puesto = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                respuesta.Add(new List<CBaseDTO> { puesto });

                //Almacena el detalle de puesto
                var detallePuesto = ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault(), new CDetallePuestoDTO());
                respuesta.Add(new List<CBaseDTO> { detallePuesto });

                //Almacena la ubicación del puesto
                var ubicacionesPuesto = ((Puesto)resultado.Contenido).RelPuestoUbicacion;

                List<CBaseDTO> ubicaciones = new List<CBaseDTO>();

                if (ubicacionesPuesto != null)
                {
                    foreach (var item in ubicacionesPuesto)
                    {
                        ubicaciones.Add(ConstruirUbicacionPuesto(item, new CUbicacionPuestoDTO()));
                    }
                    respuesta.Add(ubicaciones);
                }
                else
                {
                    ubicaciones.Add(new CUbicacionPuestoDTO());
                    ubicaciones.Add(new CUbicacionPuestoDTO());
                }

                //Almacenas las acciones del puesto
                //Almacena el estudio de puesto
                var estudioPuesto = ((Puesto)resultado.Contenido).EstudioPuesto.FirstOrDefault(Q => Q.FecResolucion != null);
                if (estudioPuesto != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CEstudioPuestoL.ConvertirDatosEstudioPuestoADTO(estudioPuesto) });
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CEstudioPuestoDTO { IdEntidad = -1 } });
                }

                ////Almacena el pedimento de puesto
                //respuesta.Add(new List<CBaseDTO> { new CPedimentoPuestoDTO { IdEntidad = -1 } });
                //Se debe agregar una fecha de cierre al pedimento y un estado
                //Revisar si los pedimentos tienen diferentes puntos
                var pedimento = ((Puesto)resultado.Contenido).PedimentoPuesto.FirstOrDefault(Q => Q.DetalleCambioPedimento.FirstOrDefault() != null || Q.DetalleCambioPedimento.FirstOrDefault(D => D.EstadoPedimento.PK_EstadoPedimento != 4) != null);
                if (pedimento != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(pedimento) });
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CPedimentoPuestoDTO { IdEntidad = -1 } });
                }

                //Almacena el prestamo de puesto
                var prestamo = ((Puesto)resultado.Contenido).PrestamoPuesto.FirstOrDefault(Q => Q.FecFinConvenio > DateTime.Now);
                if (prestamo != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CPrestamoPuestoL.ConvertirDatosPrestamoPuestoADTO(prestamo) });
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CPrestamoPuestoDTO { IdEntidad = -1 } });
                }

                //Almacena el contenido presupuestario
                var contenido = ((Puesto)resultado.Contenido).DetallePuesto.LastOrDefault().ContenidoPresupuestario;
                if (contenido != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CContenidoPresupuestarioL.ConvertirDatosContenidoADTO(contenido) });
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CContenidoPresupuestarioDTO { IdEntidad = -1 } });
                }

                //A PARTIR DE AQUI Y HASTA EL RETURN ESTABA COMENTADO

                if (nombramiento != null)
                {
                    var estadocivil = nombramiento.Funcionario.HistorialEstadoCivil;
                    List<CBaseDTO> estadosCiviles = new List<CBaseDTO>();
                    foreach (var item in estadocivil)
                    {
                        estadosCiviles.Add(CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(item));
                    }
                    respuesta.Add(estadosCiviles);
                    var contactos = nombramiento.Funcionario.InformacionContacto;
                    List<CBaseDTO> detalleContactos = new List<CBaseDTO>();
                    foreach (var item in contactos)
                    {
                        detalleContactos.Add(CInformacionContactoL.ConvertirInformacionContactoADTO(item));
                    }
                    respuesta.Add(detalleContactos);



                    List<CBaseDTO> detalleCalificaciones = new List<CBaseDTO>();

                    var historico = new CCalificacionNombramientoD(contexto).ListarCalificacionHistoricoCedula(nombramiento.Funcionario.IdCedulaFuncionario);

                    foreach (var item in historico)
                    {
                        detalleCalificaciones.Add(new CCalificacionNombramientoDTO
                        {
                            Periodo = new CPeriodoCalificacionDTO { IdEntidad = Convert.ToInt32(item.Periodo.Substring(0, 4)) },
                            CalificacionDTO = new CCalificacionDTO { DesCalificacion = item.DesCalificacion },
                            FecRatificacionDTO = DateTime.Now
                        });
                    }

                    var calificaciones = nombramiento.CalificacionNombramiento;
                    if (calificaciones.Count() > 0)
                    {
                        foreach (var item in calificaciones.Where((Q => Q.IndEstado == 1)).ToList())
                        {
                            detalleCalificaciones.Add(CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item));
                        }
                    }
                    //else
                    //{
                    //   detalleCalificaciones.Add(new CCalificacionNombramientoDTO { IdEntidad = -1 });
                    //}

                    respuesta.Add(detalleCalificaciones);

                    if (nombramiento.Funcionario.Direccion.FirstOrDefault() != null)
                    {
                        respuesta.Add(new List<CBaseDTO> { CDireccionL.ConvertirDireccionADTO(nombramiento.Funcionario.Direccion.FirstOrDefault()) });
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CDireccionDTO() });
                    }

                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CHistorialEstadoCivilDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                    respuesta.Add(new List<CBaseDTO> { new CInformacionContactoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                    respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                    respuesta.Add(new List<CBaseDTO> { new CDireccionDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                }

                return respuesta;

            }
            catch (Exception error)
            {
                return new List<List<CBaseDTO>> { new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } } };
            }
        }

        public CBaseDTO ActualizarObservacionesPuestoCaucion(string codpuesto, string observaciones)
        {
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);
                var resultado = intermedio.ActualizarObservacionesPuestoCaucion(codpuesto, observaciones);
                if (resultado.Codigo > 0)
                {
                    return new CPuestoDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    MensajeError = error.Message
                };
            }
        }

        public List<CBaseDTO> CargarPuestoActivoPP(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            puestoDescarga = new CPuestoD(contexto);
            var item = puestoDescarga.DetallePuestoCedula(cedula);
            if (item != null)
            {
                resultado.Add(new CPuestoDTO { IdEntidad = item.PK_Puesto, CodPuesto = item.CodPuesto, NivelOcupacional = item.IndNivelOcupacional != null ? (int)item.IndNivelOcupacional : 0, EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto } });
                resultado.Add(new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto, DesEstadoPuesto = item.EstadoPuesto.DesEstadoPuesto });
                if (item.DetallePuesto.FirstOrDefault().Clase != null)
                {
                    resultado.Add(new CClaseDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                        DesClase = item.DetallePuesto.FirstOrDefault().Clase.DesClase
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la clase" });
                }

                if (item.DetallePuesto.FirstOrDefault().Especialidad != null)
                {
                    resultado.Add(new CEspecialidadDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = item.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la especialidad" });
                }

                if (item.DetallePuesto.FirstOrDefault().SubEspecialidad != null)
                {
                    resultado.Add(new CSubEspecialidadDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                        DesSubEspecialidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la subespecialidad" });
                }
                if (item.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    resultado.Add(new COcupacionRealDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = item.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la ocupación real" });
                }
                resultado.Add(new CDetallePuestoDTO
                {
                    IdEntidad = item.DetallePuesto.FirstOrDefault().PK_DetallePuesto
                });
            }
            return resultado;
        }

        public List<List<CBaseDTO>> DescargarPerfilPuestoAccionesFuncionarioPP(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);
                var resultado = intermedio.DescargarPerfilNombramientoAccionesFuncionario(cedula);
                if (resultado.Codigo > 0)
                {
                    respuesta = ConstruirPuestoAccionesB(resultado);
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }
        }

        private List<List<CBaseDTO>> ConstruirPuestoAccionesPP(CRespuestaDTO resultado)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            var nombramiento = ((Puesto)resultado.Contenido).Nombramiento.OrderByDescending(Q => Q.FecRige).FirstOrDefault(Q => Q.FecVence == null);
            if (nombramiento != null)
            {
                respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral(nombramiento.Funcionario) });
                respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento) });
                respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(nombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
            }
            else
            {
                var ultimoNombramiento = ((Puesto)resultado.Contenido).Nombramiento.LastOrDefault();
                if (ultimoNombramiento != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral(ultimoNombramiento.Funcionario) });
                    respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(ultimoNombramiento) });
                    respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(ultimoNombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
                    nombramiento = ultimoNombramiento; // Asignar el nombramiento
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CFuncionarioDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto", Sexo = GeneroEnum.Indefinido } });
                    respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { Mensaje = "Este puesto no ha registrado ningún nombramiento" } });
                    respuesta.Add(new List<CBaseDTO> { new CDetalleContratacionDTO { Mensaje = "No hay contratos asociados a este puesto " } });
                }
            }

            //Almacena el puesto
            var puesto = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
            respuesta.Add(new List<CBaseDTO> { puesto });

            //Almacena el detalle de puesto
            var detallePuesto = ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.LastOrDefault(), new CDetallePuestoDTO());
            respuesta.Add(new List<CBaseDTO> { detallePuesto });

            //Almacena la ubicación del puesto
            var ubicacionesPuesto = ((Puesto)resultado.Contenido).RelPuestoUbicacion;
            List<CBaseDTO> ubicaciones = new List<CBaseDTO>();
            foreach (var item in ubicacionesPuesto)
            {
                ubicaciones.Add(ConstruirUbicacionPuesto(item, new CUbicacionPuestoDTO()));
            }
            respuesta.Add(ubicaciones);

            //Almacenas las acciones del puesto
            //Almacena el estudio de puesto
            var estudioPuesto = ((Puesto)resultado.Contenido).EstudioPuesto.FirstOrDefault(Q => Q.FecResolucion != null);
            if (estudioPuesto != null)
            {
                respuesta.Add(new List<CBaseDTO> { CEstudioPuestoL.ConvertirDatosEstudioPuestoADTO(estudioPuesto) });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CEstudioPuestoDTO { IdEntidad = -1 } });
            }

            ////Almacena el pedimento de puesto
            respuesta.Add(new List<CBaseDTO> { new CPedimentoPuestoDTO { IdEntidad = -1 } });

            //var pedimento = ((Puesto)resultado.Contenido).PedimentoPuesto.FirstOrDefault(Q => Q.EstadoPedimento.PK_EstadoPedimento == 1);
            //if (pedimento != null)
            //{
            //    respuesta.Add(new List<CBaseDTO> { CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(pedimento) });
            //}
            //else
            //{
            //    respuesta.Add(new List<CBaseDTO> { new CPedimentoPuestoDTO { IdEntidad = -1 } });
            //}

            //Almacena el prestamo de puesto
            var prestamo = ((Puesto)resultado.Contenido).PrestamoPuesto.FirstOrDefault(Q => Q.FecFinConvenio > DateTime.Now);
            if (prestamo != null)
            {
                respuesta.Add(new List<CBaseDTO> { CPrestamoPuestoL.ConvertirDatosPrestamoPuestoADTO(prestamo) });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CPrestamoPuestoDTO { IdEntidad = -1 } });
            }


            if (nombramiento != null)
            {
                var estadocivil = nombramiento.Funcionario.HistorialEstadoCivil;
                List<CBaseDTO> estadosCiviles = new List<CBaseDTO>();
                foreach (var item in estadocivil)
                {
                    estadosCiviles.Add(CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(item));
                }
                respuesta.Add(estadosCiviles);
                var contactos = nombramiento.Funcionario.InformacionContacto;
                List<CBaseDTO> detalleContactos = new List<CBaseDTO>();
                foreach (var item in contactos)
                {
                    detalleContactos.Add(CInformacionContactoL.ConvertirInformacionContactoADTO(item));
                }
                respuesta.Add(detalleContactos);



                List<CBaseDTO> detalleCalificaciones = new List<CBaseDTO>();

                var historico = new CCalificacionNombramientoD(contexto).ListarCalificacionHistoricoCedula(nombramiento.Funcionario.IdCedulaFuncionario);

                foreach (var item in historico)
                {
                    detalleCalificaciones.Add(new CCalificacionNombramientoDTO
                    {
                        Periodo = new CPeriodoCalificacionDTO { IdEntidad = Convert.ToInt32(item.Periodo.Substring(0, 4)) },
                        CalificacionDTO = new CCalificacionDTO { DesCalificacion = item.DesCalificacion }
                    });
                }

                var calificaciones = nombramiento.CalificacionNombramiento;
                if (calificaciones.Count() > 0)
                {
                    foreach (var item in calificaciones.Where((Q => Q.IndEstado == 1)).ToList())
                    {
                        detalleCalificaciones.Add(CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item));
                    }
                }
                //else
                //{
                //   detalleCalificaciones.Add(new CCalificacionNombramientoDTO { IdEntidad = -1 });
                //}

                respuesta.Add(detalleCalificaciones);

                respuesta.Add(new List<CBaseDTO> { CDireccionL.ConvertirDireccionADTO(nombramiento.Funcionario.Direccion.FirstOrDefault()) });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CHistorialEstadoCivilDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CInformacionContactoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CDireccionDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
            }
            //Almacena el contenido presupuestario
            //REVISAR ESTOS CASOS DE USO PARA VER COMO FUNCIONAN, PROBABLEMENTE HAY QUE HACER CAMBIOS EN LA BD
            return respuesta;
        }

        private List<List<CBaseDTO>> ConstruirPuestoAccionesB(CRespuestaDTO resultado)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            var nombramiento = ((Nombramiento)resultado.Contenido);
            if (nombramiento != null)
            {
                var funcionario = CFuncionarioL.FuncionarioGeneral(nombramiento.Funcionario);
                funcionario.Mensaje = contexto.ExpedienteFuncionario.Where(Q => Q.FK_Funcionario == funcionario.IdEntidad).FirstOrDefault() != null ?
                                        contexto.ExpedienteFuncionario.Where(Q => Q.FK_Funcionario == funcionario.IdEntidad).FirstOrDefault().numExpediente.ToString() 
                                        : "SIN REGISTRO";
                respuesta.Add(new List<CBaseDTO> { funcionario });
                respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento) });
                respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(nombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
            }
            else
            {
                var ultimoNombramiento = ((Nombramiento)resultado.Contenido);
                if (ultimoNombramiento != null)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral(ultimoNombramiento.Funcionario) });
                    respuesta.Add(new List<CBaseDTO> { CNombramientoL.ConvertirDatosNombramientoADTO(ultimoNombramiento) });
                    respuesta.Add(new List<CBaseDTO> { CDetalleContratacionL.ConvertirDetalleContratacionADTO(ultimoNombramiento.Funcionario.DetalleContratacion.FirstOrDefault()) });
                    nombramiento = ultimoNombramiento; // Asignar el nombramiento
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CFuncionarioDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto", Sexo = GeneroEnum.Indefinido } });
                    respuesta.Add(new List<CBaseDTO> { new CNombramientoDTO { Mensaje = "Este puesto no ha registrado ningún nombramiento" } });
                    respuesta.Add(new List<CBaseDTO> { new CDetalleContratacionDTO { Mensaje = "No hay contratos asociados a este puesto " } });
                }
            }

            //Almacena el puesto
            var puesto = ConstruirPuesto(((Nombramiento)resultado.Contenido).Puesto, new CPuestoDTO());
            respuesta.Add(new List<CBaseDTO> { puesto });

            //Almacena el detalle de puesto
            var detallePuesto = ConstruirDetallePuesto(((Nombramiento)resultado.Contenido).Puesto.DetallePuesto.LastOrDefault(), new CDetallePuestoDTO());
            respuesta.Add(new List<CBaseDTO> { detallePuesto });

            //Almacena la ubicación del puesto
            var ubicacionesPuesto = ((Nombramiento)resultado.Contenido).Puesto.RelPuestoUbicacion;
            List<CBaseDTO> ubicaciones = new List<CBaseDTO>();
            foreach (var item in ubicacionesPuesto)
            {
                ubicaciones.Add(ConstruirUbicacionPuesto(item, new CUbicacionPuestoDTO()));
            }
            respuesta.Add(ubicaciones);

            //Almacenas las acciones del puesto
            //Almacena el estudio de puesto
            var estudioPuesto = ((Nombramiento)resultado.Contenido).Puesto.EstudioPuesto.FirstOrDefault(Q => Q.FecResolucion != null);
            if (estudioPuesto != null)
            {
                respuesta.Add(new List<CBaseDTO> { CEstudioPuestoL.ConvertirDatosEstudioPuestoADTO(estudioPuesto) });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CEstudioPuestoDTO { IdEntidad = -1 } });
            }

            ////Almacena el pedimento de puesto
            respuesta.Add(new List<CBaseDTO> { new CPedimentoPuestoDTO { IdEntidad = -1 } });

            var prestamo = ((Nombramiento)resultado.Contenido).Puesto.PrestamoPuesto.FirstOrDefault(Q => Q.FecFinConvenio > DateTime.Now);
            if (prestamo != null)
            {
                respuesta.Add(new List<CBaseDTO> { CPrestamoPuestoL.ConvertirDatosPrestamoPuestoADTO(prestamo) });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CPrestamoPuestoDTO { IdEntidad = -1 } });
            }


            if (nombramiento != null)
            {
                var estadocivil = nombramiento.Funcionario.HistorialEstadoCivil;
                List<CBaseDTO> estadosCiviles = new List<CBaseDTO>();
                foreach (var item in estadocivil)
                {
                    estadosCiviles.Add(CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(item));
                }
                respuesta.Add(estadosCiviles);
                var contactos = nombramiento.Funcionario.InformacionContacto;
                List<CBaseDTO> detalleContactos = new List<CBaseDTO>();
                foreach (var item in contactos)
                {
                    detalleContactos.Add(CInformacionContactoL.ConvertirInformacionContactoADTO(item));
                }
                respuesta.Add(detalleContactos);



                List<CBaseDTO> detalleCalificaciones = new List<CBaseDTO>();

                var historico = new CCalificacionNombramientoD(contexto).ListarCalificacionHistoricoCedula(nombramiento.Funcionario.IdCedulaFuncionario);

                foreach (var item in historico)
                {
                    detalleCalificaciones.Add(new CCalificacionNombramientoDTO
                    {
                        Periodo = new CPeriodoCalificacionDTO { IdEntidad = Convert.ToInt32(item.Periodo.Substring(0, 4)) },
                        CalificacionDTO = new CCalificacionDTO { DesCalificacion = item.DesCalificacion }
                    });
                }

                var calificaciones = nombramiento.CalificacionNombramiento;
                if (calificaciones.Count() > 0)
                {
                    foreach (var item in calificaciones.Where((Q => Q.IndEstado == 1)).ToList())
                    {
                        detalleCalificaciones.Add(CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item));
                    }
                }
                //else
                //{
                //   detalleCalificaciones.Add(new CCalificacionNombramientoDTO { IdEntidad = -1 });
                //}

                respuesta.Add(detalleCalificaciones);

                respuesta.Add(new List<CBaseDTO> { nombramiento.Funcionario.Direccion.Count > 0 ? CDireccionL.ConvertirDireccionADTO(nombramiento.Funcionario.Direccion.FirstOrDefault()) : new CDireccionDTO () });
            }
            else
            {
                respuesta.Add(new List<CBaseDTO> { new CHistorialEstadoCivilDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CInformacionContactoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
                respuesta.Add(new List<CBaseDTO> { new CDireccionDTO { Mensaje = "Ningún funcionario ha sido asignado a este puesto" } });
            }
            //Almacena el contenido presupuestario
            //REVISAR ESTOS CASOS DE USO PARA VER COMO FUNCIONAN, PROBABLEMENTE HAY QUE HACER CAMBIOS EN LA BD
            return respuesta;
        }

        public string[] DatosOrdenMovimiento(string cedula, string codpuesto)
        {
            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                string nombre, cedulaf, numpuesto, nomclase, nomespecialidad, nomocupacion, nomcodigopres, nomdivision,
                    nomdireccion, nomdepartamento, nomseccion, datonombramiento;

                var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);
                var puesto = intermedioPuesto.DescargarPuestoCompleto(codpuesto);


                if (funcionario != null)
                {
                    cedulaf = funcionario.IdCedulaFuncionario;
                    nombre = funcionario.NomPrimerApellido.TrimEnd() + " " + funcionario.NomSegundoApellido.TrimEnd() + " " + funcionario.NomFuncionario.TrimEnd();
                }
                else
                {
                    cedulaf = "0";
                    nombre = "";
                }

                if (puesto.Codigo > 0)
                {
                    var datosPuesto = (Puesto)puesto.Contenido;
                    var estadopuesto = contexto.EstadoPuesto.Where(E => E.PK_EstadoPuesto == datosPuesto.FK_EstadoPuesto).FirstOrDefault();
                    var nombramiento = contexto.Nombramiento.Where(N => N.FK_Puesto == datosPuesto.PK_Puesto && (N.FecVence == null || N.FecVence >= DateTime.Now)).OrderByDescending(O => O.FecRige).FirstOrDefault();
                    DetallePuesto datoDetalle = datosPuesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).OrderBy(O => O.FecRige).FirstOrDefault();

                    numpuesto = datosPuesto.CodPuesto;
                    nomdivision = datosPuesto.UbicacionAdministrativa.Division != null ? datosPuesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd() : "";
                    nomdireccion = datosPuesto.UbicacionAdministrativa.DireccionGeneral != null ? datosPuesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd() : "";
                    nomdepartamento = datosPuesto.UbicacionAdministrativa.Departamento != null ? datosPuesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "";
                    nomseccion = datosPuesto.UbicacionAdministrativa.Seccion != null ? datosPuesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : "";
                    nomcodigopres = datosPuesto.UbicacionAdministrativa.Presupuesto != null ? datosPuesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto.TrimEnd() : "";
                    nomclase = datoDetalle.Clase != null ? datoDetalle.Clase.DesClase : "NO TIENE";
                    nomespecialidad = datoDetalle.Especialidad != null ? datoDetalle.Especialidad.DesEspecialidad : "NO TIENE";
                    nomocupacion = datoDetalle.OcupacionReal != null ? datoDetalle.OcupacionReal.DesOcupacionReal : "NO TIENE";
                    if (nombramiento != null)
                    {
                        var estadonombramiento = contexto.EstadoNombramiento.Where(E => E.PK_EstadoNombramiento == nombramiento.FK_EstadoNombramiento).FirstOrDefault();
                        var funcionarionombramiento = contexto.Funcionario.Where(Q => Q.PK_Funcionario == nombramiento.FK_Funcionario).FirstOrDefault();
                        datonombramiento = "Este puesto tiene un nombramiento activo de tipo: " + estadonombramiento.DesEstado + ". Asociado al funcionario " + funcionarionombramiento.NomFuncionario.TrimEnd() + " " + funcionarionombramiento.NomPrimerApellido.TrimEnd() + " " + funcionarionombramiento.NomSegundoApellido.TrimEnd();
                    }
                    else
                    {
                        datonombramiento = "Este puesto no tiene ningún nombramiento activo. Su estado es: " + estadopuesto.DesEstadoPuesto;
                    }

                }
                else
                {
                    throw new Exception("No se encontró el puesto indicado con el número suministrado, por favor revise los datos");
                }

                return new string[]
                    { nombre, cedulaf, numpuesto, nomclase, nomespecialidad, nomocupacion, nomcodigopres, nomdivision,
                    nomdireccion, nomdepartamento, nomseccion, datonombramiento };

            }
            catch (Exception error)
            {
                return new string[] { "ERROR", error.Message };
            }

        }

        public List<CBaseDTO> DatosUbicacionDetallePuesto(string codpuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPuestoD intermedio = new CPuestoD(contexto);
                var resultado = intermedio.DatosUbicacionDetallePuesto(codpuesto);

                if (resultado.Codigo != -1)
                {
                    var puesto = ConstruirPuesto((Puesto)resultado.Contenido, new CPuestoDTO());

                    respuesta.Add(puesto);

                    var detallepuesto = ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto
                                                                                            .OrderBy(D => D.FecRige)
                                                                                            .FirstOrDefault(D => D.IndEstadoDetallePuesto == 1 || D.IndEstadoDetallePuesto == null), new CDetallePuestoDTO());

                    detallepuesto.Mensaje = DefinirNivelOcupacional(puesto.NivelOcupacional);

                    respuesta.Add(detallepuesto);

                    var ubicacionContrato = CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.Where(R => R.UbicacionPuesto.FK_TipoUbicacion == 1).FirstOrDefault().UbicacionPuesto);

                    respuesta.Add(ubicacionContrato);

                    var ubicacionTrabajo = CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.Where(R => R.UbicacionPuesto.FK_TipoUbicacion == 2).FirstOrDefault().UbicacionPuesto);

                    respuesta.Add(ubicacionTrabajo);

                    var nombramiento = ((Puesto)resultado.Contenido).Nombramiento.OrderByDescending(F => F.FecRige).FirstOrDefault(N => N.FecVence == null || N.FecVence >= DateTime.Now);

                    if (nombramiento != null)
                    {
                        respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento));
                        respuesta.Add(CFuncionarioL.FuncionarioGeneral(nombramiento.Funcionario));
                    }
                    else
                    {
                        respuesta.Add(new CBaseDTO { Mensaje = "El puesto no tiene ningún nombramiento activo" });
                    }

                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        private string DefinirNivelOcupacional(int nivel)
        {
            switch (nivel)
            {
                case 1:
                    return "Superior";
                case 2:
                    return "Ejecutivo";
                case 3:
                    return "Profesional";
                case 4:
                    return "Técnico";
                case 5:
                    return "Administrativo";
                case 6:
                    return "Servicios";
                case 7:
                    return "Policial";
                default:
                    return "Indefinido";
            }
        }

        public List<CBaseDTO> CargarPuestoPropiedad(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            puestoDescarga = new CPuestoD(contexto);
            var item = puestoDescarga.DescargarPuestoPropiedad(cedula);
            if (item != null)
            {
                resultado.Add(new CPuestoDTO { IdEntidad = item.PK_Puesto, CodPuesto = item.CodPuesto, NivelOcupacional = item.IndNivelOcupacional != null ? (int)item.IndNivelOcupacional : 0, EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto } });
                resultado.Add(new CEstadoPuestoDTO { IdEntidad = item.EstadoPuesto.PK_EstadoPuesto, DesEstadoPuesto = item.EstadoPuesto.DesEstadoPuesto });
                if (item.DetallePuesto.FirstOrDefault().Clase != null)
                {
                    resultado.Add(new CClaseDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                        DesClase = item.DetallePuesto.FirstOrDefault().Clase.DesClase
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la clase" });
                }

                if (item.DetallePuesto.FirstOrDefault().Especialidad != null)
                {
                    resultado.Add(new CEspecialidadDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = item.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la especialidad" });
                }

                if (item.DetallePuesto.FirstOrDefault().SubEspecialidad != null)
                {
                    resultado.Add(new CSubEspecialidadDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                        DesSubEspecialidad = item.DetallePuesto.FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la subespecialidad" });
                }
                if (item.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    resultado.Add(new COcupacionRealDTO
                    {
                        IdEntidad = item.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = item.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontro la ocupación real" });
                }
                resultado.Add(new CDetallePuestoDTO
                {
                    IdEntidad = item.DetallePuesto.FirstOrDefault().PK_DetallePuesto
                });
            }
            return resultado;
        }

        public CBaseDTO ActualizarDatosPuesto(CDetallePuestoDTO detallePuesto, CUbicacionAdministrativaDTO ubicacionAdmin, CUbicacionPuestoDTO ubicacionContrato,
                                              CUbicacionPuestoDTO ubicacionTrabajo, CPuestoDTO puesto)
        {
            CPuestoDTO respuesta = new CPuestoDTO();
            try
            {
                CDetallePuestoD intermedioDetallePuesto = new CDetallePuestoD(contexto);
                CUbicacionPuestoD intermedioUbicacionPuesto = new CUbicacionPuestoD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);

                if (detallePuesto != null)
                {
                    DetallePuesto nuevoDetalle = DatosActualizarDetallePuesto(detallePuesto);

                    var resultado = intermedioDetallePuesto.ActualizarDetallePuesto(puesto.CodPuesto, nuevoDetalle);

                    if (resultado.Codigo > 0)
                    {
                        respuesta = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }

                if (ubicacionAdmin != null)
                {
                    int div, dir, dep, sec, pres;
                    if (ubicacionAdmin.Division.NomDivision.Contains("-"))
                    {
                        div = Convert.ToInt32(ubicacionAdmin.Division.NomDivision.Split('-')[0]);
                        dir = ubicacionAdmin.DireccionGeneral != null ? ubicacionAdmin.DireccionGeneral.NomDireccion != null ? ubicacionAdmin.DireccionGeneral.NomDireccion != "" ? Convert.ToInt32(ubicacionAdmin.DireccionGeneral.NomDireccion.Split('-')[0]) : 0 : 0 : 0;
                        dep = ubicacionAdmin.Departamento != null ? ubicacionAdmin.Departamento.NomDepartamento != null ? ubicacionAdmin.Departamento.NomDepartamento != "" ? Convert.ToInt32(ubicacionAdmin.Departamento.NomDepartamento.Split('-')[0]) : 0 : 0 : 0;
                        sec = Convert.ToInt32(ubicacionAdmin.Seccion.NomSeccion.Split('-')[0]);
                    }
                    else
                    {
                        div = ubicacionAdmin.Division.IdEntidad;
                        dir = ubicacionAdmin.DireccionGeneral != null ? ubicacionAdmin.DireccionGeneral.IdEntidad : 0;
                        dep = ubicacionAdmin.Departamento != null ? ubicacionAdmin.Departamento.IdEntidad : 0;
                        sec = ubicacionAdmin.Seccion.IdEntidad;
                    }

                    pres = contexto.Presupuesto.FirstOrDefault(P => P.IdPresupuesto == ubicacionAdmin.Presupuesto.CodigoPresupuesto).PK_Presupuesto;

                    pres = pres == ubicacionAdmin.Presupuesto.IdEntidad ? ubicacionAdmin.Presupuesto.IdEntidad : pres;

                    var ubicacionBD = new UbicacionAdministrativa();

                    if (dir != 0 && dep != 0)
                    {
                        ubicacionBD = contexto.UbicacionAdministrativa.FirstOrDefault(U => U.FK_Division == div && U.FK_DireccionGeneral == dir
                                      && U.FK_Departamento == dep && U.FK_Seccion == sec && U.FK_Presupuesto == pres);

                        if (ubicacionBD == null)
                        {
                            ubicacionBD = new UbicacionAdministrativa
                            {
                                Departamento = contexto.Departamento.FirstOrDefault(D => D.PK_Departamento == dep),
                                DireccionGeneral = contexto.DireccionGeneral.FirstOrDefault(D => D.PK_DireccionGeneral == dir),
                                Division = contexto.Division.FirstOrDefault(D => D.PK_Division == div),
                                Presupuesto = contexto.Presupuesto.FirstOrDefault(P => P.PK_Presupuesto == pres),
                                Seccion = contexto.Seccion.FirstOrDefault(S => S.PK_Seccion == sec),
                                DesObservaciones = GenerarCentroCostos(div, dir, dep, sec)
                            };
                        }
                    }
                    else
                    {
                        if (dir != 0 && dep == 0)
                        {
                            ubicacionBD = contexto.UbicacionAdministrativa.FirstOrDefault(U => U.FK_Division == div && U.FK_DireccionGeneral == dir
                                            && U.FK_Departamento == null && U.FK_Seccion == sec && U.FK_Presupuesto == pres);

                            if (ubicacionBD == null)
                            {
                                ubicacionBD = new UbicacionAdministrativa
                                {
                                    DireccionGeneral = contexto.DireccionGeneral.FirstOrDefault(D => D.PK_DireccionGeneral == dir),
                                    Division = contexto.Division.FirstOrDefault(D => D.PK_Division == div),
                                    Presupuesto = contexto.Presupuesto.FirstOrDefault(P => P.PK_Presupuesto == pres),
                                    Seccion = contexto.Seccion.FirstOrDefault(S => S.PK_Seccion == sec),
                                    DesObservaciones = GenerarCentroCostos(div, dir, dep, sec)
                                };
                            }
                        }
                        else
                        {
                            if (dir == 0 && dep != 0)
                            {
                                ubicacionBD = contexto.UbicacionAdministrativa.FirstOrDefault(U => U.FK_Division == div && U.FK_DireccionGeneral == null
                                                && U.FK_Departamento == dep && U.FK_Seccion == sec && U.FK_Presupuesto == pres);

                                if (ubicacionBD == null)
                                {
                                    ubicacionBD = new UbicacionAdministrativa
                                    {
                                        Departamento = contexto.Departamento.FirstOrDefault(D => D.PK_Departamento == dep),
                                        Division = contexto.Division.FirstOrDefault(D => D.PK_Division == div),
                                        Presupuesto = contexto.Presupuesto.FirstOrDefault(P => P.PK_Presupuesto == pres),
                                        Seccion = contexto.Seccion.FirstOrDefault(S => S.PK_Seccion == sec),
                                        DesObservaciones = GenerarCentroCostos(div, dir, dep, sec)
                                    };
                                }
                            }
                            else
                            {
                                ubicacionBD = contexto.UbicacionAdministrativa.FirstOrDefault(U => U.FK_Division == div && U.FK_DireccionGeneral == null
                                                && U.FK_Departamento == null && U.FK_Seccion == sec && U.FK_Presupuesto == pres);
                                if (ubicacionBD == null)
                                {
                                    ubicacionBD = new UbicacionAdministrativa
                                    {
                                        Division = contexto.Division.FirstOrDefault(D => D.PK_Division == div),
                                        Presupuesto = contexto.Presupuesto.FirstOrDefault(P => P.PK_Presupuesto == pres),
                                        Seccion = contexto.Seccion.FirstOrDefault(S => S.PK_Seccion == sec),
                                        DesObservaciones = GenerarCentroCostos(div, dir, dep, sec)
                                    };
                                }
                            }
                        }
                    }

                    var resultado = intermedioPuesto.ActualizarUbicacionAdministrativa(puesto.CodPuesto, ubicacionBD);

                    if (resultado.Codigo > 0)
                    {
                        respuesta = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }

                if (puesto.Mensaje == "Cambio")
                {
                    var resultado = intermedioPuesto.ActualizarNivelOcupacional(puesto.CodPuesto, puesto.NivelOcupacional);

                    if (resultado.Codigo > 0)
                    {
                        respuesta = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }

                if (ubicacionContrato != null)
                {
                    string[] datosContrato = ubicacionContrato.Mensaje.Split('-');

                    var nomDistrito = datosContrato[2].TrimEnd();
                    var canton = datosContrato[1].TrimEnd();
                    var provincia = datosContrato[0].TrimEnd();

                    var distrito = contexto.Distrito.FirstOrDefault(D => D.NomDistrito == nomDistrito && D.Canton.NomCanton == canton
                                   && D.Canton.Provincia.NomProvincia == provincia);

                    if (distrito != null)
                    {
                        ubicacionContrato.Distrito = new CDistritoDTO();
                        ubicacionContrato.Distrito.IdEntidad = distrito.PK_Distrito;
                        ubicacionContrato.TipoUbicacion = new CTipoUbicacionDTO { IdEntidad = 1 };

                        var resultado = intermedioUbicacionPuesto.ModificarUbicacionPuesto(puesto.CodPuesto, ubicacionContrato);

                        if (resultado.Codigo > 0)
                        {
                            respuesta = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró la ubicación administrativa indicada para este cambio");
                    }
                }

                if (ubicacionTrabajo != null)
                {
                    string[] datosContrato = ubicacionTrabajo.Mensaje.Split('-');

                    var nomDistrito = datosContrato[2].TrimEnd();
                    var canton = datosContrato[1].TrimEnd();
                    var provincia = datosContrato[0].TrimEnd();

                    var distrito = contexto.Distrito.FirstOrDefault(D => D.NomDistrito == nomDistrito && D.Canton.NomCanton == canton
                                   && D.Canton.Provincia.NomProvincia == provincia);

                    if (distrito != null)
                    {
                        ubicacionTrabajo.Distrito = new CDistritoDTO();
                        ubicacionTrabajo.Distrito.IdEntidad = distrito.PK_Distrito;
                        ubicacionTrabajo.TipoUbicacion = new CTipoUbicacionDTO { IdEntidad = 2 };

                        var resultado = intermedioUbicacionPuesto.ModificarUbicacionPuesto(puesto.CodPuesto, ubicacionTrabajo);

                        if (resultado.Codigo > 0)
                        {
                            respuesta = ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO());
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró la ubicación administrativa indicada para este cambio");
                    }
                }

                return respuesta;
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        private string GenerarCentroCostos(int div, int dir, int dep, int sec)
        {
            string cadena = div.ToString();

            if (dir != 0)
            {
                cadena += dir.ToString().Length == 3 ? dir.ToString() : dir.ToString().Length == 2 ? "0" + dir.ToString() : dir.ToString().Length == 1 ? "00" + dir.ToString() : "000";
            }
            else
            {
                cadena += "000";
            }
            if (dep != 0)
            {
                cadena += dep.ToString().Length == 3 ? dep.ToString() : dep.ToString().Length == 2 ? "0" + dep.ToString() : dep.ToString().Length == 1 ? "00" + dep.ToString() : "000";
            }
            else
            {
                cadena += "000";
            }

            cadena += sec.ToString().Length == 3 ? sec.ToString() : sec.ToString().Length == 2 ? "0" + sec.ToString() : sec.ToString().Length == 1 ? "00" + sec.ToString() : "000";

            return cadena;
        }

        private DetallePuesto DatosActualizarDetallePuesto(CDetallePuestoDTO detallePuesto)
        {
            DetallePuesto nuevoDetalle = new DetallePuesto();
            Clase claseNueva;
            Especialidad especialidadNueva;
            OcupacionReal ocupacionNueva;
            SubEspecialidad subespecialidadNueva;

            if (detallePuesto.Clase != null && detallePuesto.Clase.DesClase != null  && char.IsNumber(Convert.ToChar(detallePuesto.Clase.DesClase.ElementAt(0))))
            {
                int codClase = Convert.ToInt32(detallePuesto.Clase.DesClase.Split('-')[0]);
                claseNueva = contexto.Clase.FirstOrDefault(C => C.PK_Clase == codClase);
                nuevoDetalle.Clase = claseNueva;
            }
            else
            {
                claseNueva = contexto.Clase.FirstOrDefault(C => C.PK_Clase == detallePuesto.Clase.IdEntidad);
                nuevoDetalle.Clase = claseNueva;
            }

            if (detallePuesto.Especialidad != null && detallePuesto.Especialidad.DesEspecialidad != null && char.IsNumber(Convert.ToChar(detallePuesto.Especialidad.DesEspecialidad.ElementAt(0))))
            {
                int codEspecialidad = Convert.ToInt32(detallePuesto.Especialidad.DesEspecialidad.Split('-')[0]);
                especialidadNueva = contexto.Especialidad.FirstOrDefault(C => C.PK_Especialidad == codEspecialidad);
                nuevoDetalle.Especialidad = especialidadNueva;
            }
            else
            {
                especialidadNueva = contexto.Especialidad.FirstOrDefault(C => C.PK_Especialidad == detallePuesto.Especialidad.IdEntidad);
                nuevoDetalle.Especialidad = especialidadNueva;
            }

            if (detallePuesto.SubEspecialidad != null && detallePuesto.SubEspecialidad.DesSubEspecialidad != null && char.IsNumber(Convert.ToChar(detallePuesto.SubEspecialidad.DesSubEspecialidad.ElementAt(0))))
            {
                int codSubEspecialidad = Convert.ToInt32(detallePuesto.SubEspecialidad.DesSubEspecialidad.Split('-')[0]);
                subespecialidadNueva = contexto.SubEspecialidad.FirstOrDefault(C => C.PK_SubEspecialidad == codSubEspecialidad);
                nuevoDetalle.SubEspecialidad = subespecialidadNueva;
            }
            else
            {
                subespecialidadNueva = contexto.SubEspecialidad.FirstOrDefault(C => C.PK_SubEspecialidad == detallePuesto.SubEspecialidad.IdEntidad);
                nuevoDetalle.SubEspecialidad = subespecialidadNueva;
            }

            if (detallePuesto.OcupacionReal != null && detallePuesto.OcupacionReal.DesOcupacionReal != null && char.IsNumber(Convert.ToChar(detallePuesto.OcupacionReal.DesOcupacionReal.ElementAt(0))))
            {
                int codOcupacion = Convert.ToInt32(detallePuesto.OcupacionReal.DesOcupacionReal.Split('-')[0]);
                ocupacionNueva = contexto.OcupacionReal.FirstOrDefault(C => C.PK_OcupacionReal == codOcupacion);
                nuevoDetalle.OcupacionReal = ocupacionNueva;
            }
            else
            {
                ocupacionNueva = contexto.OcupacionReal.FirstOrDefault(C => C.PK_OcupacionReal == detallePuesto.OcupacionReal.IdEntidad);
                nuevoDetalle.OcupacionReal = ocupacionNueva;
            }
            return nuevoDetalle;
        }

        #endregion

    }
}
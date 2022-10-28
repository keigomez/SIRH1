using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{//
    public class CCalificacionNombramientoL
    {
        #region Variables

        SIRHEntities contexto;
        CCalificacionNombramientoD calificacionDescarga;

        #endregion

        #region constructor

        public CCalificacionNombramientoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCalificacionNombramientoDTO ConvertirDatosCCalificacionNombramientoLADto(CalificacionNombramiento item)
        {
            //List<CCalificacionCapacitacionDTO> listaCap = new List<CCalificacionCapacitacionDTO>();
            //if (item.CalificacionCapacitacion != null)
            //{
            //    foreach (var capacitacion in item.CalificacionCapacitacion)
            //    {
            //        listaCap.Add(CCalificacionCapacitacionL.ConvertirDatosCapacitacionADto(capacitacion));
            //    }
            //}
            return new CCalificacionNombramientoDTO
            {
                IdEntidad = item.PK_CalificacionNombramiento,
                Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                CalificacionDTO = CCalificacionL.ConvertirDatosCCalificacionADto(item.Calificacion),
                NombramientoDTO = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                UsrEvaluadorDTO = item.UsrEvaluador,
                FecCreacionDTO = Convert.ToDateTime(item.FecCreacion),
                IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                IndFormularioDTO = Convert.ToInt32(item.IndFormulario),
                IndEntregadoDTO = Convert.ToBoolean(item.IndEntregado),
                IndConformidadDTO = Convert.ToBoolean(item.IndConformidad),
                IndRatificacionDTO = Convert.ToInt32(item.IndRatificado),           
                FecRatificacionDTO = Convert.ToDateTime(item.FecRatificacion),
                ObsGeneralDTO = item.ObsGeneral,
                ObsCapacitacionDTO = item.ObsCapacitacion,
                ObsJustificacionCapacitacionDTO = item.ObsJustificacionCapacitacion,
                JefeInmediato = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeInmediato),
                    Sexo = GeneroEnum.Indefinido
                },
                JefeSuperior = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeSuperior),
                    Sexo = GeneroEnum.Indefinido
                }
                //DetalleCapacitacion = listaCap
            };
        }

        internal static CCalificacionNombramientoFuncionarioDTO ConvertirDatosCCalificacionFuncionarioADto(CalificacionNombramientoFuncionarios item)
        {
            List<CCalificacionCapacitacionDTO> listaCap = new List<CCalificacionCapacitacionDTO>();
            if (item.CalificacionCapacitacion != null)
            {
                foreach (var capacitacion in item.CalificacionCapacitacion.Where(Q => Q.IndEstado == 1).ToList())
                {
                    listaCap.Add(CCalificacionCapacitacionL.ConvertirDatosCapacitacionADto(capacitacion));
                }
            }
            return new CCalificacionNombramientoFuncionarioDTO
            {
                IdEntidad = item.PK_CalificacionNombramiento,
                Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                Nombramiento = new CNombramientoDTO { IdEntidad = item.FK_Nombramiento },
                Puesto = new CPuestoDTO { IdEntidad = Convert.ToInt32(item.FK_Puesto) },
                Funcionario = new CFuncionarioDTO { IdEntidad = Convert.ToInt32(item.FK_Funcionario), Sexo = GeneroEnum.Indefinido },
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                indSeccion = Convert.ToInt32(item.FK_Seccion),
                indDepartamento = Convert.ToInt32(item.FK_Departamento),
                indDireccion = Convert.ToInt32(item.FK_DireccionGeneral),
                indDivision = Convert.ToInt32(item.FK_Division),
                indOcupacion = Convert.ToInt32(item.FK_OcupacionReal),
                indDetallePuesto = Convert.ToInt32(item.FK_DetallePuesto),
                JefeInmediato = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeInmediato),
                    Sexo = GeneroEnum.Indefinido
                },
                JefeSuperior = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeSuperior),
                    Sexo = GeneroEnum.Indefinido
                },
                NumTipo = item.NumTipo,
                PorAutoEvaluacion = item.PorAutoEvaluacion,
                DetalleCapacitacion = listaCap
            };
        }

        internal static CalificacionNombramiento ConvertirDTOCalificacionNombramientoADatos(CCalificacionNombramientoDTO intemCN)
        {
            return new CalificacionNombramiento
            {
                PK_CalificacionNombramiento = intemCN.IdEntidad,
                PeriodoCalificacion = new PeriodoCalificacion
                {
                    PK_PeriodoCalificacion = intemCN.Periodo.IdEntidad
                },
                Nombramiento = new Nombramiento
                {
                    PK_Nombramiento = intemCN.NombramientoDTO.IdEntidad
                },
                UsrEvaluador = intemCN.UsrEvaluadorDTO,
                FecCreacion = intemCN.FecCreacionDTO,
                IndEstado = intemCN.IndEstadoDTO,
                ObsGeneral = intemCN.ObsGeneralDTO,
                ObsCapacitacion = intemCN.ObsCapacitacionDTO,
                ObsJustificacionCapacitacion = intemCN.ObsJustificacionCapacitacionDTO,
                IdJefeInmediato = intemCN.JefeInmediato.IdEntidad,
                IdJefeSuperior = intemCN.JefeSuperior.IdEntidad,
                IndFormulario = intemCN.IndFormularioDTO,
                IndConformidad = intemCN.IndConformidadDTO ? 1 : 0,
                IndRatificado = intemCN.IndRatificacionDTO,
                FecRatificacion = intemCN.FecRatificacionDTO,
                IndEntregado = intemCN.IndEntregadoDTO ? 1 : 0
            };
        }

        internal static CPeriodoCalificacionDTO ConvertirDatosPeriodoDto(PeriodoCalificacion item)
        {
            return new CPeriodoCalificacionDTO
            {
                IdEntidad = item.PK_PeriodoCalificacion,
                FecRige = item.FecRige,
                FecVence = item.FecVence,
                FecRigeReglaTecnica = item.FecRigeReglaTecnica,
                FecVenceReglaTecnica = item.FecVenceReglaTecnica
            };
        }

        internal static CCalificacionNombramientoFuncionarioDTO ConvertirDatosCalificacionFuncionarioDto(USP_LISTAR_FUNCIONARIOS_CALIFICACION_Result item)
        {
            return new CCalificacionNombramientoFuncionarioDTO
            {
                IdEntidad = item.PK_CalificacionNombramiento,
                Periodo = new CPeriodoCalificacionDTO { IdEntidad = item.FK_PeriodoCalificacion },
                Nombramiento = new CNombramientoDTO
                {
                    IdEntidad = item.FK_Nombramiento
                },
                Puesto = new CPuestoDTO
                {
                    IdEntidad = Convert.ToInt32(item.FK_Puesto)
                },
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                indSeccion = Convert.ToInt32(item.FK_Seccion),
                indDepartamento = Convert.ToInt32(item.FK_Departamento),
                indDireccion = Convert.ToInt32(item.FK_DireccionGeneral),
                indDivision = Convert.ToInt32(item.FK_Division),
                NumTipo = Convert.ToInt32(item.NumTipo),
                Funcionario = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.FK_Funcionario),
                    Nombre = item.Nombre,
                    Cedula = item.IdCedulaFuncionario,
                    Sexo = GeneroEnum.Indefinido
                },
                JefeInmediato = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeInmediato),
                    Nombre = item.NombreJefeInmediato,
                    Cedula = item.IdCedulaJefeInmediato,
                    Sexo = GeneroEnum.Indefinido
                },
                JefeSuperior = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeSuperior),
                    Nombre = item.NombreJefeSuperior,
                    Cedula = item.IdCedulaJefeSuperior,
                    Sexo = GeneroEnum.Indefinido
                }
            };
        }

        public List<CBaseDTO> DescargarCalificacionCedula(string cedula, int periodo)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            calificacionDescarga = new CCalificacionNombramientoD(contexto);
            var calificacion = calificacionDescarga.ListarCalificacionNombramientoCedula(cedula).Where(Q => Q.IndEstado == 1 && Q.FK_PeriodoCalificacion == periodo).FirstOrDefault();
           
            if(calificacion != null)
            {
                CCalificacionNombramientoDTO temp = new CCalificacionNombramientoDTO();
                temp.IdEntidad = calificacion.PK_CalificacionNombramiento;
                temp = ConvertirDatosCCalificacionNombramientoLADto(calificacion);
                temp.Periodo = new CPeriodoCalificacionDTO
                {
                    IdEntidad = calificacion.PeriodoCalificacion.PK_PeriodoCalificacion,
                    FecRige = calificacion.PeriodoCalificacion.FecRige,
                    FecVence = calificacion.PeriodoCalificacion.FecVence,
                    FecRigeReglaTecnica = calificacion.PeriodoCalificacion.FecRigeReglaTecnica,
                    FecVenceReglaTecnica = calificacion.PeriodoCalificacion.FecVenceReglaTecnica
                };
                temp.CalificacionDTO = new CCalificacionDTO
                {
                    IdEntidad = calificacion.Calificacion.PK_Calificacion,
                    DesCalificacion = calificacion.Calificacion.DesCalificacion
                };
                resultado.Add(temp);
            }
            else
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = "No existe una calificación para el funcionario en ese periodo"
                });
            }

            return resultado;
        }

        public List<CBaseDTO> DescargarCalificacionesCedula(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
           
            calificacionDescarga = new CCalificacionNombramientoD(contexto);
            var calificacion = calificacionDescarga.ListarCalificacionNombramientoCedula(cedula);

            foreach (var item in calificacion)
            {
                CCalificacionNombramientoDTO temp = new CCalificacionNombramientoDTO();
                temp.IdEntidad = item.PK_CalificacionNombramiento;
                temp.Periodo = new CPeriodoCalificacionDTO
                {
                    IdEntidad = item.PeriodoCalificacion.PK_PeriodoCalificacion,
                    FecRige = item.PeriodoCalificacion.FecRige,
                    FecVence = item.PeriodoCalificacion.FecVence,
                    FecRigeReglaTecnica = item.PeriodoCalificacion.FecRigeReglaTecnica,
                    FecVenceReglaTecnica = item.PeriodoCalificacion.FecVenceReglaTecnica
                };
                temp.CalificacionDTO = new CCalificacionDTO
                {
                    IdEntidad = item.Calificacion.PK_Calificacion,
                    DesCalificacion = item.Calificacion.DesCalificacion
                };
                resultado.Add(temp);
            }

            return resultado;
        }


        public List<CBaseDTO> DescargarFuncionarioCalificarDetallePuesto(string cedula, int periodo)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            CCalificacionNombramientoFuncionarioDTO datoCalifica = new CCalificacionNombramientoFuncionarioDTO { IdEntidad = 0 };
            datoCalifica.JefeInmediato = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
            datoCalifica.JefeSuperior = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };

            CNombramientoL intermedioNombramiento = new CNombramientoL();
            CFuncionarioL intermedioFuncionario = new CFuncionarioL();
            CFuncionarioD intermedioFD = new CFuncionarioD(contexto);
            CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

            try
            {
                var datoFuncionario = intermedioFD.BuscarFuncionarioCedula(cedula);
                if (datoFuncionario == null)
                {
                    throw new Exception("No existe funcionario con ese número de cédula");
                }
                else
                {
                    // Buscar el Nombramiento en la tabla CalificacionNombramientoFuncionarios
                    calificacionDescarga = new CCalificacionNombramientoD(contexto);
                    var calificacion = calificacionDescarga.DescargarFuncionarioCalificar(cedula, periodo);

                    if (calificacion.Codigo > 0)  // El funcionario está registrado para la Evaluación del periodo
                    {
                        var dato = (CalificacionNombramientoFuncionarios)calificacion.Contenido;
                        //  Buscar el Detalle del Nombramiento

                        resultado = intermedioNombramiento.DescargarNombramientoDetallePuesto(dato.FK_Nombramiento);
                        datoCalifica = (ConvertirDatosCCalificacionFuncionarioADto(dato));
                        if (datoCalifica.JefeInmediato.IdEntidad > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datoCalifica.JefeInmediato.IdEntidad));
                            datoCalifica.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            datoCalifica.JefeInmediato.Nombre = datoCalifica.JefeInmediato.Nombre.TrimEnd() + " " + datoCalifica.JefeInmediato.PrimerApellido.TrimEnd() + " " + datoCalifica.JefeInmediato.SegundoApellido.TrimEnd();
                        }
                        if (datoCalifica.JefeSuperior.IdEntidad > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datoCalifica.JefeSuperior.IdEntidad));
                            datoCalifica.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            datoCalifica.JefeSuperior.Nombre = datoCalifica.JefeSuperior.Nombre.TrimEnd() + " " + datoCalifica.JefeSuperior.PrimerApellido.TrimEnd() + " " + datoCalifica.JefeSuperior.SegundoApellido.TrimEnd();
                        }
                        datoCalifica.Mensaje = "";
                    }
                    else  // El funcionario NO está registrado para la Evaluación del periodo
                    {
                        // Buscar el Nombramiento Actual
                        CAccionPersonalL intermedioAccion = new CAccionPersonalL();

                        var datoAcc = intermedioAccion.BuscarFuncionarioDetallePuesto(cedula);
                        if (datoAcc.FirstOrDefault().GetType() == typeof(CErrorDTO))
                        {
                            datoCalifica.Mensaje = ((CErrorDTO)datoAcc.FirstOrDefault()).MensajeError;
                        }
                        else
                        {
                            datoCalifica.Mensaje = "El funcionario no está registrado para la evaluación del periodo " + periodo;
                            resultado = intermedioNombramiento.DescargarNombramientoDetallePuesto(((CNombramientoDTO)datoAcc[4]).IdEntidad);
                        }
                    }

                    resultado.Add(datoCalifica);
                }
            }
            catch (Exception error)
            {
                if (resultado.Count > 0)
                {
                    resultado.Clear();
                }
                resultado.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return resultado;
        }

        public List<CBaseDTO> DescargarNombramientoFuncionarioCalificar(string cedula, int periodo)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            CCalificacionNombramientoFuncionarioDTO datoCalifica = new CCalificacionNombramientoFuncionarioDTO { IdEntidad = 0 };
            datoCalifica.JefeInmediato = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
            datoCalifica.JefeSuperior = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };

            CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();
            CFuncionarioL intermedioFuncionario = new CFuncionarioL();
            CFuncionarioD intermedioFD = new CFuncionarioD(contexto);
            CDetallePuestoD intermedioDP = new CDetallePuestoD(contexto);
            CSeccionL intermedioSeccion = new CSeccionL();
            CDepartamentoL intermedioDep = new CDepartamentoL();
            CDireccionGeneralL intermedioDir = new CDireccionGeneralL();
            CDivisionL intermedioDiv = new CDivisionL();
            CExpedienteL intermedioExp = new CExpedienteL();
            CListaNombramientoActivoL intermedioLNA = new CListaNombramientoActivoL();
            CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

            try
            {
                var datoFuncionario = intermedioFD.BuscarFuncionarioCedula(cedula);
                if (datoFuncionario == null)
                {
                    throw new Exception("No existe funcionario con ese número de cédula");
                }
                else
                {
                    // Buscar el Nombramiento en la tabla CalificacionNombramientoFuncionarios
                    calificacionDescarga = new CCalificacionNombramientoD(contexto);
                    var calificacion = calificacionDescarga.DescargarFuncionarioCalificar(cedula, periodo);

                    if (calificacion.Codigo > 0)  // El funcionario está registrado para la Evaluación del periodo
                    {
                        datoCalifica = ConvertirDatosCCalificacionFuncionarioADto((CalificacionNombramientoFuncionarios)calificacion.Contenido);

                        datoCalifica.Funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(datoFuncionario);

                        if (datoCalifica.JefeInmediato.IdEntidad > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datoCalifica.JefeInmediato.IdEntidad));
                            datoCalifica.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            datoCalifica.JefeInmediato.Nombre = datoCalifica.JefeInmediato.Nombre.TrimEnd() + " " + datoCalifica.JefeInmediato.PrimerApellido.TrimEnd() + " " + datoCalifica.JefeInmediato.SegundoApellido.TrimEnd();
                        }
                        if (datoCalifica.JefeSuperior.IdEntidad > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datoCalifica.JefeSuperior.IdEntidad));
                            datoCalifica.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            datoCalifica.JefeSuperior.Nombre = datoCalifica.JefeSuperior.Nombre.TrimEnd() + " " + datoCalifica.JefeSuperior.PrimerApellido.TrimEnd() + " " + datoCalifica.JefeSuperior.SegundoApellido.TrimEnd();
                        }
                        datoCalifica.Mensaje = "";


                        Nombramiento datoNombramiento = intermedioNombramiento.CargarNombramiento(datoCalifica.Nombramiento.IdEntidad);
                        CPuestoDTO puesto = new CPuestoDTO();
                        datoCalifica.Puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);

                        //[0] CCalificacionNombramientoFuncionarioDTO
                        resultado.Add(datoCalifica);

                        //[1] DetallePuesto
                        var datoDP = CDetallePuestoL.ConstruirDetallePuesto(intermedioDP.CargarDetallePuesto(datoCalifica.indDetallePuesto));
                        resultado.Add(datoDP);

                        //[2] Seccion
                        if (datoCalifica.indSeccion > 0)
                        {
                            var datoSeccion = intermedioSeccion.DescargarSeccions(datoCalifica.indSeccion, "");
                            resultado.Add(datoSeccion.FirstOrDefault());
                        }
                        else
                        {
                            resultado.Add(new CSeccionDTO { IdEntidad = 0, NomSeccion = "" });
                        }


                        //[3] Departamento
                        if (datoCalifica.indDepartamento > 0)
                        {
                            var datoDep = intermedioDep.DescargarDepartamentos(datoCalifica.indDepartamento, "");
                            resultado.Add(datoDep.FirstOrDefault());
                        }
                        else
                        {
                            resultado.Add(new CDepartamentoDTO { IdEntidad = 0, NomDepartamento = "" });
                        }

                        //[4] DireccionGeneral
                        var datoDir = intermedioDir.DescargarDireccionGenerals(datoCalifica.indDireccion, "");
                        resultado.Add(datoDir.FirstOrDefault());

                        //[5] División
                        var datoDiv = intermedioDiv.DescargarDivisions(datoCalifica.indDivision, "");
                        resultado.Add(datoDiv.FirstOrDefault());

                        //[6] Expediente
                        var datoExp = intermedioExp.ObtenerExpedientePorCedulaFuncionario(cedula);
                        resultado.Add(datoExp.FirstOrDefault());

                        //[7] Permiso
                        var datoPermiso = new CMetaIndividualEvidenciaL();
                        resultado.Add(datoPermiso.ObtenerPermiso(datoCalifica.Funcionario));

                        //[8] ListaNombramientoActual
                        var datoLista = new CListaNombramientosActivosDTO
                        {
                            Funcionario = new CFuncionarioDTO { IdEntidad = 0, Cedula = cedula, Sexo = GeneroEnum.Indefinido }
                        };

                        var listado = intermedioLNA.BuscarNombramientos(datoLista);
                        if (listado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            resultado.Add(listado.FirstOrDefault());
                        else
                            resultado.Add(datoLista);
                    }
                    else  // El funcionario NO está registrado para la Evaluación del periodo
                    {
                        throw new Exception(((CErrorDTO)calificacion.Contenido).MensajeError);
                    }

                    resultado.Add(datoCalifica);
                }
            }
            catch (Exception error)
            {
                if (resultado.Count > 0)
                {
                    resultado.Clear();
                }
                resultado.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return resultado;
        }

        public List<CBaseDTO> GuardarCalificacionFuncionario(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                //instancia a datos de CCalificacionNombramientoD
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);
                //CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);

                // inserta en CcalificacionNombramientoD
                CalificacionNombramiento datosCalificacionNombramientoBD = ConvertirDTOCalificacionNombramientoADatos(CalificacionCN);
                datosCalificacionNombramientoBD.FecRatificacion = null;
                datosCalificacionNombramientoBD.Calificacion = contexto.Calificacion.FirstOrDefault(Q => Q.PK_Calificacion == CalificacionCN.CalificacionDTO.IdEntidad);

                // Periodo Evaluación
                var entidadPeriodo = intermedioCalificacionN.ObtenerPeriodoCalificacion(CalificacionCN.Periodo.IdEntidad);

                if (entidadPeriodo.Codigo != -1)
                {
                    datosCalificacionNombramientoBD.PeriodoCalificacion = (PeriodoCalificacion)entidadPeriodo.Contenido;
                }
                else
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPeriodo).Contenido).MensajeError);
                }

                var insertaCN = intermedioCalificacionN.AgregarCalificacionNombramiento(datosCalificacionNombramientoBD);
                
                //pregunto si da error
                if (insertaCN.Codigo > 0)
                {
                    respuesta.Add(CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto((CalificacionNombramiento)insertaCN.Contenido));
                    
                    // instancia de CDetalleCalificacionNombramientoD
                    CDetalleCalificacionNombramientoD intermedioDetalleCN = new CDetalleCalificacionNombramientoD(contexto);
                    CCatalogoPreguntaD intermedioPregunta = new CCatalogoPreguntaD(contexto);

                    CCalificacionCapacitacionL intermedioCapacitacion = new CCalificacionCapacitacionL();

                    // inserta CDetalleCalificacionNombramientoD

                    foreach (var item in DetalleDCN)
                    {
                        DetalleCalificacion datosDetalle = new DetalleCalificacion
                        {
                            PK_DetalleCalificacion = item.IdEntidad,
                            NumNotasPregunta = item.NumNotasPorPreguntaDTO,
                            CalificacionNombramiento = ((CalificacionNombramiento)insertaCN.Contenido)
                        };

                        var entidadPregunta =  intermedioPregunta.ObtenerPreguntas(item.CatalogoPreguntaDTO.IdEntidad);
                        if (entidadPregunta.Codigo != -1)
                        {
                            datosDetalle.CatalogoPregunta = (CatalogoPregunta)entidadPregunta.Contenido;
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPregunta).Contenido).MensajeError);
                        }

                        //insert
                        var insertarDE = intermedioDetalleCN.AgregarDetalleCalificacionNombramiento(datosDetalle);
                        if (insertarDE.Codigo > 0)
                        {
                            respuesta.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoADto((DetalleCalificacion)insertarDE.Contenido));
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)respuesta[2]).MensajeError);
                        }
                    }

                    //// Detalle de necesidades de Capacitación
                    //foreach (var item in CalificacionCN.DetalleCapacitacion)
                    //{
                    //    //Agregar
                    //    item.CalificacionNombramientoDTO = new CCalificacionNombramientoDTO { IdEntidad = ((CalificacionNombramiento)insertaCN.Contenido).PK_CalificacionNombramiento };
                    //    var insertarCap = intermedioCapacitacion.GuardarCapacitacion(item); // .AgregarCapacitacion(datosCapacitacion);
                    //    if (insertarCap.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    //    {
                    //        respuesta.Add(item);
                    //    }
                    //    else
                    //    {
                    //        throw new Exception(((CErrorDTO)insertarCap.FirstOrDefault()).MensajeError);
                    //    }
                    //}
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                //respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public List<CBaseDTO> GuardarCalificacionModificada(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                //instancia a datos de CCalificacionNombramientoD
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);
                //CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);

                //// inserta en CcalificacionNombramientoD
                //CalificacionNombramiento datosCalificacionNombramientoBD = ConvertirDTOCalificacionNombramientoADatos(CalificacionCN);
                //datosCalificacionNombramientoBD.Calificacion = contexto.Calificacion.FirstOrDefault(Q => Q.PK_Calificacion == CalificacionCN.CalificacionDTO.IdEntidad);

                //// Periodo Evaluación
                //var entidadPeriodo = intermedioCalificacionN.ObtenerPeriodoCalificacion(CalificacionCN.Periodo.IdEntidad);

                //if (entidadPeriodo.Codigo != -1)
                //{
                //    datosCalificacionNombramientoBD.PeriodoCalificacion = (PeriodoCalificacion)entidadPeriodo.Contenido;
                //}
                //else
                //{
                //    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPeriodo).Contenido).MensajeError);
                //}

                
                var datos = intermedioCalificacionN.RatificarCalificacionNombramiento(CalificacionCN.IdEntidad, 2); // codRatificado (0-Default, 1-Ratificado, 2-Modificado)

                //pregunto si da error
                if (datos.Codigo > 0)
                {
                    respuesta.Add(CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto((CalificacionNombramiento)datos.Contenido));

                    // instancia de CDetalleCalificacionNombramientoD
                    CDetalleCalificacionNombramientoD intermedioDetalleCN = new CDetalleCalificacionNombramientoD(contexto);
                    CCatalogoPreguntaD intermedioPregunta = new CCatalogoPreguntaD(contexto);

                    // inserta CDetalleCalificacionNombramientoD

                    foreach (var item in DetalleDCN)
                    {
                        DetalleCalificacionModificada datosDetalle = new DetalleCalificacionModificada
                        {
                            PK_DetalleCalificacion = item.IdEntidad,
                            NumNotasPregunta = item.NumNotasPorPreguntaDTO,
                            CalificacionNombramiento = ((CalificacionNombramiento)datos.Contenido)
                        };

                        var entidadPregunta = intermedioPregunta.ObtenerPreguntas(item.CatalogoPreguntaDTO.IdEntidad);
                        if (entidadPregunta.Codigo != -1)
                        {
                            datosDetalle.CatalogoPregunta = (CatalogoPregunta)entidadPregunta.Contenido;
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPregunta).Contenido).MensajeError);
                        }

                        //insert
                        var insertarDE = intermedioDetalleCN.AgregarDetalleCalificacionModificada(datosDetalle);
                        if (insertarDE.Codigo > 0)
                        {
                            respuesta.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionModificadaADto((DetalleCalificacionModificada)insertarDE.Contenido));
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)respuesta[2]).MensajeError);
                        }
                    }
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public CBaseDTO EditarCalificacionNombramiento(int codCalificacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var datos = intermedio.EditarCalificacionNombramiento(codCalificacion);

                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datos.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datos.Contenido);
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

        public CBaseDTO EditarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                var datoPeriodo = intermedio.ObtenerPeriodoCalificacion(periodo.IdEntidad);
                var dato = new CalificacionNombramientoFuncionarios
                {
                    PeriodoCalificacion = (PeriodoCalificacion)datoPeriodo.Contenido,
                    FK_Funcionario = detalle.Funcionario.IdEntidad,
                    IdJefeInmediato = detalle.JefeInmediato.IdEntidad,
                    IdJefeSuperior = detalle.JefeSuperior.IdEntidad
                };

                var datos = intermedio.EditarCalificacionNombramientoFuncionario(dato);

                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datos.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datos.Contenido);
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

        public CBaseDTO RatificarCalificacionNombramiento(int codCalificacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var datos = intermedio.RatificarCalificacionNombramiento(codCalificacion, 1); // codRatificado (0-Default, 1-Ratificado, 2-Modificado)

                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datos.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datos.Contenido);
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


        public CBaseDTO RecibirCalificacionNombramiento(int codCalificacion, bool indEntregado, bool indConformidad)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var datos = intermedio.RecibirCalificacionNombramiento(codCalificacion, indEntregado ? 1 : 0, indConformidad ? 1 : 0);

                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datos.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datos.Contenido);
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
        public CBaseDTO AsignarJefatura(int periodo, int codFuncionario, int codJefatura)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var datos = intermedio.AsignarJefatura(periodo, codFuncionario, codJefatura);

                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datos.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datos.Contenido);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> ListarCalificaciones(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                bool ExisteFuncionario = false;
                string codPuesto = "";

                var resultadoFuncionario = intermedio.BuscarFuncionarioCedulaBase(cedula);

                if (resultadoFuncionario.Codigo != -1)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral((Funcionario)resultadoFuncionario.Contenido) });
                    ExisteFuncionario = true;
                }
                else
                {
                    var resultadoExFuncionario = intermedio.BuscarExFuncionarioCedula(cedula);
                    if(resultadoExFuncionario != null)
                    {
                        codPuesto = resultadoExFuncionario.PUESTO_PROPIEDAD.TrimEnd();

                        respuesta.Add(new List<CBaseDTO> { new CFuncionarioDTO {
                                                                Cedula = resultadoExFuncionario.CEDULA,
                                                                Nombre = resultadoExFuncionario.NOMBRE.TrimEnd(),
                                                                PrimerApellido = resultadoExFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                                                SegundoApellido = resultadoExFuncionario.SEGUNDO_APELLIDO.TrimEnd(),
                                                                Sexo = GeneroEnum.Indefinido
                                                            } });
                        ExisteFuncionario = true;
                    }
                }
       
                if (ExisteFuncionario)
                {
                    CCalificacionNombramientoD intermedioCalificaciones = new CCalificacionNombramientoD(contexto);
                    CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);

                    List<CBaseDTO> calificacionData = new List<CBaseDTO>();

                    var historico = intermedioCalificaciones.ListarCalificacionHistoricoCedula(cedula);
                    foreach (var item in historico)
                    {
                        var periodo = Convert.ToInt32(item.Periodo.Substring(0, 4));
                        calificacionData.Add(new CCalificacionNombramientoDTO
                        {
                            IdEntidad = item.ID,
                            Periodo = new CPeriodoCalificacionDTO { IdEntidad = periodo },
                            CalificacionDTO = new CCalificacionDTO {
                                                        IdEntidad = Convert.ToInt32(item.Calificacion),
                                                        DesCalificacion = item.DesCalificacion },
                            NombramientoDTO = new CNombramientoDTO { IdEntidad = 0 },
                            Nota = Convert.ToDecimal(item.Nota),
                            ObsGeneralDTO = item.DesJustificacion != null ? item.DesJustificacion : ""
                        });
                    }


                    var calificacion = intermedioCalificaciones.ListarCalificaciones(cedula);
                    if (calificacion != null)
                    {
                        
                        if (calificacion.Count() > 0) {
                            foreach (var item in calificacion)
                            {
                                decimal suma = 0;
                                int idCalificacion = 0;
                                string DesCalificacion = "";

                                if (item.DetalleCalificacionModificada.Count() > 0)
                                {
                                    suma = item.DetalleCalificacionModificada.Sum(Q => decimal.Parse(Q.NumNotasPregunta));
                                    ObtenerDetalleCalificacion(suma, out idCalificacion, out DesCalificacion);
                                }
                                else
                                {
                                    suma = item.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPregunta));
                                    DesCalificacion = item.Calificacion.DesCalificacion;
                                    idCalificacion = item.Calificacion.PK_Calificacion;
                                }


                                calificacionData.Add(new CCalificacionNombramientoDTO
                                {
                                    IdEntidad = item.PK_CalificacionNombramiento,
                                    Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                                    CalificacionDTO = CCalificacionL.ConvertirDatosCCalificacionADto(item.Calificacion),
                                    NombramientoDTO = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                                    IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                                    FecCreacionDTO = Convert.ToDateTime(item.FecCreacion),
                                    Nota = suma,
                                    CalificacionModificadoDTO = new CCalificacionDTO
                                                                {
                                                                    IdEntidad = idCalificacion,
                                                                    DesCalificacion = DesCalificacion
                                                                }
                                });
                            }
                        }
                        else {
                            var entidadNombramiento = intermedioNombramiento.CargarNombramientoCedula(cedula);

                            if (entidadNombramiento!= null && entidadNombramiento.PK_Nombramiento != -1)
                            {
                                calificacionData.Add(new CCalificacionNombramientoDTO
                                {
                                    NombramientoDTO = CNombramientoL.ConvertirDatosNombramientoADTO(entidadNombramiento)
                                });
                            }
                            else
                            {
                                calificacionData.Add(new CCalificacionNombramientoDTO());
                               // throw new Exception("No tiene un Nombramiento Válido");
                            }
                        }

                        respuesta.Add(calificacionData);
                        CPuestoD intermedioPuesto = new CPuestoD(contexto);
                        var resultadoPuesto = intermedioPuesto.DetallePuestoCedula(cedula);
                        if (resultadoPuesto != null)
                        {
                            respuesta.Add(new List<CBaseDTO> {CPuestoL.PuestoGeneral((Puesto)resultadoPuesto) });
                        }
                        else
                        {
                            var resultadoPuestoCodigo = intermedioPuesto.DetallePuestoPorCodigo(codPuesto);
                            if (resultadoPuestoCodigo != null)
                            {
                                respuesta.Add(new List<CBaseDTO> { CPuestoL.PuestoGeneral((Puesto)resultadoPuestoCodigo) });
                            }
                            else
                            {
                                respuesta.Add(new List<CBaseDTO> { new CPrestamoPuestoDTO() });
                            }
                        }

                        CDetallePuestoD intermedioDetallePuesto = new CDetallePuestoD(contexto);
                        var resultadoDetallePuesto = intermedioDetallePuesto.CargarDetallePuestoCedula(cedula);
                        if (resultadoDetallePuesto != null)
                        {
                            respuesta.Add(new List<CBaseDTO> { CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)resultadoDetallePuesto) });
                        }
                        else
                        {
                            var resultadoDetallePuestoCodigo = intermedioDetallePuesto.CargarDetallePuestoCodigo(codPuesto);
                            if (resultadoDetallePuestoCodigo != null)
                            {
                                respuesta.Add(new List<CBaseDTO> { CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)resultadoDetallePuestoCodigo) });
                            }
                            else
                            {
                                respuesta.Add(new List<CBaseDTO> { new CDetallePuestoDTO() });
                            }
                        }

                        CExpedienteD intermedioExpediente = new CExpedienteD(contexto);

                        var repuestaExp = intermedioExpediente.ObtenerExpedientePorCedulaFuncionario(cedula);
                        if (repuestaExp.Codigo != -1)
                        {
                            respuesta.Add(new List<CBaseDTO> {
                                            new CExpedienteFuncionarioDTO {  NumeroExpediente =  ((ExpedienteFuncionario)repuestaExp.Contenido).numExpediente }
                                          });
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> {
                                            new CExpedienteFuncionarioDTO {  NumeroExpediente =  0 }
                                          });
                            //respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = ((CErrorDTO)repuestaExp.Contenido).MensajeError } });
                            //throw new Exception();
                        }


                        // Jefaturas
                        CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                        var dJefatura = intermedioFuncionario.ObtenerJefaturaFuncionario(cedula);
                        var Jefes = ((List<CFuncionarioDTO>)dJefatura.Contenido);

                        CDetalleCalificacionNombramientoD intermedioDetalle = new CDetalleCalificacionNombramientoD(contexto);

                        // Jefe Inmediato
                        if (Jefes.ElementAt(0).Cedula != "")
                        {
                            var dato = CFuncionarioL.FuncionarioGeneral(intermedioFuncionario.BuscarFuncionarioCedula(Jefes.ElementAt(0).Cedula));
                            dato.Nombre = Jefes.ElementAt(0).Nombre;
                            respuesta.Add(new List<CBaseDTO> { dato });
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> {
                               new CFuncionarioDTO {IdEntidad =  Jefes.ElementAt(0).IdEntidad,
                                                    Cedula = Jefes.ElementAt(0).Cedula,
                                                    Nombre = Jefes.ElementAt(0).Nombre,
                                                    Sexo = GeneroEnum.Indefinido}
                                });
                        }


                        // Jefe Superior
                        if (Jefes.ElementAt(1).Cedula != "")
                        {
                            var dato = CFuncionarioL.FuncionarioGeneral(intermedioFuncionario.BuscarFuncionarioCedula(Jefes.ElementAt(1).Cedula));
                            dato.Nombre = Jefes.ElementAt(1).Nombre;
                            respuesta.Add(new List<CBaseDTO> { dato });
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> {
                               new CFuncionarioDTO {IdEntidad =  Jefes.ElementAt(1).IdEntidad,
                                                    Cedula = Jefes.ElementAt(1).Cedula,
                                                    Nombre = Jefes.ElementAt(1).Nombre,
                                                    Sexo = GeneroEnum.Indefinido}
                                });
                        }
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                    }
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError =  error.InnerException.Message + " " + error.Message + " " + error.InnerException.ToString()  } });
                return respuesta;
            }

            return respuesta;
        }

        public List<CBaseDTO> ListarCalificacionesJefatura(int periodo, string cedulaJefatura, string cedulaFuncionario)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            int idJefatura = 0;
            int idFuncionario = 0;
            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CCalificacionNombramientoD intermedioCalificaciones = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);
                CFuncionarioDTO jefatura = new CFuncionarioDTO();

                if (cedulaJefatura != null && cedulaJefatura != "")
                {
                    var resultadoFuncionario = intermedioFuncionario.BuscarFuncionarioCedulaBase(cedulaJefatura);
                    if (resultadoFuncionario.Codigo != -1)
                    {
                        idJefatura = ((Funcionario)resultadoFuncionario.Contenido).PK_Funcionario;
                    }
                }


                if (cedulaFuncionario != null && cedulaFuncionario != "")
                {
                    var resultadoFuncionario = intermedioFuncionario.BuscarFuncionarioCedulaBase(cedulaFuncionario);
                    if (resultadoFuncionario.Codigo != -1)
                    {
                        idFuncionario = ((Funcionario)resultadoFuncionario.Contenido).PK_Funcionario;
                    }
                }

                var calificacion = intermedioCalificaciones.ListarCalificacionesJefatura(periodo, idJefatura, idFuncionario);
                if (calificacion != null)
                {
                    if (calificacion.Count() > 0)
                    {
                        //List<CBaseDTO> calificacionData = new List<CBaseDTO>();
                        foreach (var item in calificacion.Where(C=> C.IndEstado == 1))
                        {
                            List<CDetalleCalificacionNombramientoDTO> listaDetalle = new List<CDetalleCalificacionNombramientoDTO>();
                            foreach (var detalle in item.DetalleCalificacion)
                            {
                                listaDetalle.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoADto(detalle));
                            }

                            List<CDetalleCalificacionNombramientoDTO> listaDetalleMod = new List<CDetalleCalificacionNombramientoDTO>();
                            foreach (var detalle in item.DetalleCalificacionModificada)
                            {
                                listaDetalleMod.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoModificadoADto(detalle));
                            }

                            var nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento);
                            var funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario);
                            funcionario.Nombre = funcionario.Nombre.TrimEnd() + " " + funcionario.PrimerApellido.TrimEnd() + " " + funcionario.SegundoApellido.TrimEnd();
                            nombramiento.Funcionario = funcionario;

                            // Buscar funcionario por id
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.IdJefeInmediato));
                            jefatura = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            //var jefatura = CFuncionarioL.FuncionarioGeneral(intermedioFuncionario.BuscarFuncionarioCedula(cedulaJefatura));
                            jefatura.Nombre = jefatura.Nombre.TrimEnd() + " " + jefatura.PrimerApellido.TrimEnd() + " " + jefatura.SegundoApellido.TrimEnd();

                            respuesta.Add(new CCalificacionNombramientoDTO
                            {
                                IdEntidad = item.PK_CalificacionNombramiento,
                                Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                                CalificacionDTO = CCalificacionL.ConvertirDatosCCalificacionADto(item.Calificacion),
                                NombramientoDTO = nombramiento,
                                IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                                IndEntregadoDTO = Convert.ToBoolean(item.IndEntregado),
                                IndConformidadDTO = Convert.ToBoolean(item.IndConformidad),
                                IndFormularioDTO = Convert.ToInt32(item.IndFormulario),
                                IndRatificacionDTO= Convert.ToInt32(item.IndRatificado),
                                FecCreacionDTO = Convert.ToDateTime(item.FecCreacion),
                                DetalleCalificacion = listaDetalle,
                                DetalleCalificacionModificado = listaDetalleMod,
                                JefeInmediato = jefatura,
                            });
                        }
                    }
                    else
                    {
                        respuesta.Add(new CCalificacionNombramientoDTO { Mensaje = "No se encontraron calificaciones para ese periodo" });
                    }
                }
                else
                {
                    respuesta.Add(new CCalificacionNombramientoDTO { Mensaje = "No se encontraron calificaciones para ese periodo" } );
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message } );
                return respuesta;
            }

            return respuesta;
        }

        public List<CBaseDTO> ListarCalificacionesPeriodo(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoDTO calificacion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionNombramientoD intermedioCalificaciones = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

                CFuncionarioDTO jefaturaInmediata = new CFuncionarioDTO();
                CFuncionarioDTO jefaturaSuperior = new CFuncionarioDTO();

                var resultado = intermedioCalificaciones.ListarCalificacionesPeriodo(periodo.IdEntidad);

                // Filtrar por Estado
                if (calificacion.IndEstadoDTO > -1)
                    resultado = resultado.Where(C => C.IndEstado == calificacion.IndEstadoDTO).ToList();

                // Filtrar por Entregado
                if (calificacion.IndRatificacionDTO > -1)
                    resultado = resultado.Where(C => C.IndRatificado == calificacion.IndRatificacionDTO).ToList();


                foreach (var item in resultado)
                {
                    List<CDetalleCalificacionNombramientoDTO> listaDetalle = new List<CDetalleCalificacionNombramientoDTO>();
                    foreach (var detalle in item.DetalleCalificacion)
                    {
                        listaDetalle.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoADto(detalle));
                    }

                    List<CDetalleCalificacionNombramientoDTO> listaDetalleMod = new List<CDetalleCalificacionNombramientoDTO>();
                    foreach (var detalle in item.DetalleCalificacionModificada)
                    {
                        listaDetalleMod.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoModificadoADto(detalle));
                    }

                    var nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento);
                    var funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario);
                    funcionario.Nombre = funcionario.Nombre.TrimEnd() + " " + funcionario.PrimerApellido.TrimEnd() + " " + funcionario.SegundoApellido.TrimEnd();
                    nombramiento.Funcionario = funcionario;

                    // Buscar funcionario por id
                    var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.IdJefeInmediato));
                    jefaturaInmediata = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    //var jefatura = CFuncionarioL.FuncionarioGeneral(intermedioFuncionario.BuscarFuncionarioCedula(cedulaJefatura));
                    jefaturaInmediata.Nombre = jefaturaInmediata.Nombre.TrimEnd() + " " + jefaturaInmediata.PrimerApellido.TrimEnd() + " " + jefaturaInmediata.SegundoApellido.TrimEnd();


                    if(item.IdJefeSuperior > 0)
                    {
                        dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.IdJefeSuperior));
                        jefaturaSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                        //var jefatura = CFuncionarioL.FuncionarioGeneral(intermedioFuncionario.BuscarFuncionarioCedula(cedulaJefatura));
                        jefaturaSuperior.Nombre = jefaturaSuperior.Nombre.TrimEnd() + " " + jefaturaSuperior.PrimerApellido.TrimEnd() + " " + jefaturaSuperior.SegundoApellido.TrimEnd();
                    }
                    else
                    {
                        jefaturaSuperior = new CFuncionarioDTO
                        {
                            Cedula = "0",
                            Nombre = "",
                            Sexo = GeneroEnum.Indefinido
                        };
                    }

                    respuesta.Add(new CCalificacionNombramientoDTO
                    {
                        IdEntidad = item.PK_CalificacionNombramiento,
                        Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                        CalificacionDTO = CCalificacionL.ConvertirDatosCCalificacionADto(item.Calificacion),
                        NombramientoDTO = nombramiento,
                        IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                        IndEntregadoDTO = Convert.ToBoolean(item.IndEntregado),
                        IndConformidadDTO = Convert.ToBoolean(item.IndConformidad),
                        IndFormularioDTO = Convert.ToInt32(item.IndFormulario),
                        IndRatificacionDTO = Convert.ToInt32(item.IndRatificado),
                        FecCreacionDTO = Convert.ToDateTime(item.FecCreacion),
                        DetalleCalificacion = listaDetalle,
                        DetalleCalificacionModificado = listaDetalleMod,
                        JefeInmediato = jefaturaInmediata,
                        JefeSuperior = jefaturaSuperior
                    });
                }




                        //if (calificacion.Count() > 0)
                        //{
                        //    foreach (var item in calificacion)
                        //    {
                        //        int idCalificacion = 0;
                        //        string DesCalificacion = "";

                        //        if (item.DetalleCalificacionModificada.Count() > 0)
                        //        {
                        //            var suma = item.DetalleCalificacionModificada.Sum(Q => decimal.Parse(Q.NumNotasPregunta));
                        //            ObtenerDetalleCalificacion(suma, out idCalificacion, out DesCalificacion);
                        //        }
                        //        else
                        //        {
                        //            DesCalificacion = item.Calificacion.DesCalificacion;
                        //            idCalificacion = item.Calificacion.PK_Calificacion;
                        //        }


                        //        calificacionData.Add(new CCalificacionNombramientoDTO
                        //        {
                        //            IdEntidad = item.PK_CalificacionNombramiento,
                        //            Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                        //            CalificacionDTO = CCalificacionL.ConvertirDatosCCalificacionADto(item.Calificacion),
                        //            NombramientoDTO = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                        //            IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                        //            FecCreacionDTO = Convert.ToDateTime(item.FecCreacion),
                        //            CalificacionModificadoDTO = new CCalificacionDTO
                        //            {
                        //                IdEntidad = idCalificacion,
                        //                DesCalificacion = DesCalificacion
                        //            }
                        //        });
                        //    }
                        //}
                     
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message } );
                return respuesta;
            }

            return respuesta;
        }

        public List<List<CBaseDTO>> ObtenerDatosEvaluacion(int periodo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            //try
            //{
            //    CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

            //    var dato = intermedio.ObtenerDatosGenerales(periodo);
            //    if (dato.Codigo != -1)
            //    {
            //        foreach (var item in (List<USP_OBTENER_DATOS_CALIFICACION_Result>)dato.Contenido)
            //        {
            //            CProcAlmacenadoDatosGeneralesDTO temp = new CProcAlmacenadoDatosGeneralesDTO();
            //            temp.Periodos = item.Periodo.ToString();
            //            temp.NombreInstitucion = item.Institucion.ToString();
            //            temp.CantPuestosInstitucionales = item.TotalPuestos.ToString();
            //            temp.Propiedad = item.TotalPropiedad.ToString();
            //            temp.Interinos = item.TotalInterino.ToString();
            //            temp.SinInterinos = "0";
            //            temp.CantidadPFRSC = item.FueraRSC.ToString();
            //            temp.CantidadPuestosFueraRSC = item.FueraRSC.ToString();
            //            temp.Excluidos = item.PuestosExcluidos.ToString();
            //            temp.PuestoConfianza = item.PuestosConfianza.ToString();
            //            temp.Exceptuados = item.PuestosExceptuados.ToString();
            //            temp.Oposicion = item.PuestosOposicion.ToString();
            //            temp.Otros = item.PuestosOtros.ToString();
            //            temp.FuncionariosDentroRSC = (item.TotalPuestos - item.FueraRSC).ToString();
            //            temp.Evaluados = item.Evaluados.ToString();
            //            temp.NoEvaluados = item.NoEvaluados.ToString();

            //            respuesta.Add(new List<CBaseDTO> { temp });
            //        }
            //    }
            //    else
            //    {
            //        respuesta.Add(new List<CBaseDTO> { new CRespuestaDTO { Mensaje = dato.Contenido.ToString() } });
            //    }

            //    var resultado = intermedio.ObtenerDatosCalificacion(periodo);
            //    if (resultado.Codigo > -1)
            //    {
            //        List<CBaseDTO> calificacionData = new List<CBaseDTO>();
            //        foreach (var item in (List<USP_OBTENER_RESULTADOS_CALIFICACION_Result>)resultado.Contenido)
            //        {
            //            calificacionData.Add(new CProcAlmacenadoDTO
            //            {
            //                Estratos = item.Estratos.ToString(),
            //                AbsExcelente = item.Excelente.ToString(),
            //                PorcExcelente = item.PorExc.ToString(),
            //                AbsMuyBueno = item.MuyBueno.ToString(),
            //                PorcMuyBueno = item.PorMB.ToString(),
            //                AbsBueno = item.Bueno.ToString(),
            //                PorcBueno = item.PorBue.ToString(),
            //                AbsRegular = item.Regular.ToString(),
            //                PorcRegular = item.PorReg.ToString(),
            //                AbsDeficiente = item.Deficiente.ToString(),
            //                PorcDeficiente = item.PorDef.ToString(),
            //                TotalEvaluacion = item.Total.ToString()
            //            });
            //        }

            //        respuesta.Add(calificacionData);
            //    }
            //    else
            //    {
            //        throw new Exception(resultado.Contenido.ToString());
            //    }
            //}
            //catch (Exception error)
            //{
            //    respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
            //    return respuesta;
            //}

            return respuesta;
        }

        public List<CBaseDTO> ListarFuncionarios(int idPeriodo, string cedulaFuncionario, string cedulaJefe, string cedulaJefeSuperior, int idSeccion, int idDireccion, int idDivision)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                //CFuncionarioD intermedio = new CFuncionarioD(contexto);
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                //var datos = intermedio.ListarFuncionariosJefatura(idPeriodo, cedula);
                var datos = intermedio.ListarFuncionariosCalificacion(idPeriodo);
                if (datos.Codigo > 0)
                {
                    var listado = (List<USP_LISTAR_FUNCIONARIOS_CALIFICACION_Result>)datos.Contenido;

                    if(cedulaFuncionario != "")
                        listado = listado.Where(Q =>Q.IdCedulaFuncionario == cedulaFuncionario).ToList();

                    if (cedulaJefe != "")
                        listado = listado.Where(Q => Q.IdCedulaJefeInmediato == cedulaJefe).ToList();
                    
                    if (cedulaJefeSuperior != "")
                        listado = listado.Where(Q => Q.IdCedulaJefeSuperior == cedulaJefeSuperior).ToList();

                    if (idSeccion > 0)
                        listado = listado.Where(Q => Q.FK_Seccion == idSeccion).ToList();

                    if (idDireccion > 0)
                        listado = listado.Where(Q => Q.FK_DireccionGeneral == idDireccion).ToList();

                    if (idDivision > 0)
                        listado = listado.Where(Q => Q.FK_Division == idDivision).ToList();

                    foreach (var item in listado)
                    {
                        respuesta.Add(ConvertirDatosCalificacionFuncionarioDto(item));
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

        public List<CBaseDTO> ListarFuncionariosJefatura(int idPeriodo, string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                var datos = intermedio.ListarFuncionariosJefatura(idPeriodo, cedula);

                if (datos.Codigo > 0)
                {
                    foreach (var item in (List<CFuncionarioDTO>)datos.Contenido)
                    {
                        item.Sexo = GeneroEnum.Indefinido;
                        respuesta.Add(item);
                    }
                    //foreach (var item in (List<USP_LISTAR_FUNCIONARIOS_CALIFICACION_JEFATURA_Result>) datos.Contenido)
                    //{
                    //    respuesta.Add(new CFuncionarioDTO
                    //    {
                    //        IdEntidad = item.PK_Funcionario,
                    //        Cedula = item.IdCedulaFuncionario,
                    //        Nombre = item.NomFuncionario,
                    //        Sexo = GeneroEnum.Indefinido
                    //    });
                    //}

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

        public List<CBaseDTO> ListarJefaturas(int idPeriodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                //CFuncionarioD intermedio = new CFuncionarioD(contexto);
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                //var datos = intermedio.ListarFuncionariosJefatura(idPeriodo, cedula);
                var datos = intermedio.ListarFuncionariosCalificacion(idPeriodo);
                if (datos.Codigo > 0)
                {
                    var listado = (List<USP_LISTAR_FUNCIONARIOS_CALIFICACION_Result>)datos.Contenido;
                    listado = listado.OrderBy(Q => Q.Nombre).ToList();
                    //listado = listado.Where(Q => Q.NumTipo != 1).OrderBy(Q => Q.Nombre).ToList();

                    foreach (var item in listado)
                    {
                        respuesta.Add(ConvertirDatosCalificacionFuncionarioDto(item));
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

        public List<CBaseDTO> ObtenerCalificacion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {              
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CDetallePuestoD intermedioDPuesto = new CDetallePuestoD(contexto);
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CFuncionarioL intermedioFuncionarioLog = new CFuncionarioL();
                CNombramientoL intermedioNombramientoL = new CNombramientoL();

                var calificacion = intermedio.ObtenerCalificacion(codigo);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosCCalificacionNombramientoLADto((CalificacionNombramiento)calificacion.Contenido);

                    // Jefe Inmediato
                    if (datosCalificacion.JefeInmediato.IdEntidad != null && datosCalificacion.JefeInmediato.IdEntidad != 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datosCalificacion.JefeInmediato.IdEntidad));
                        datosCalificacion.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }
                    else
                    {
                        datosCalificacion.JefeInmediato = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
                    }


                    // Jefe Superior
                    if (datosCalificacion.JefeSuperior.IdEntidad != null && datosCalificacion.JefeSuperior.IdEntidad != 0)
                    {
                        var dJefaturaSup = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datosCalificacion.JefeSuperior.IdEntidad));
                        datosCalificacion.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefaturaSup.Contenido));
                    }
                    else
                    {
                        datosCalificacion.JefeSuperior = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
                    }

                    // Detalle Calificación
                    List<CDetalleCalificacionNombramientoDTO> listadoDetalle = new List<CDetalleCalificacionNombramientoDTO>();
                    var datoC = (CalificacionNombramiento)calificacion.Contenido;

                    foreach (var item in datoC.DetalleCalificacion)
                    {
                        listadoDetalle.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoADto(item));
                    }

                    datosCalificacion.DetalleCalificacion = listadoDetalle;

                    // Detalle Calificación Modificado
                    List<CDetalleCalificacionNombramientoDTO> listadoDetalleModificado = new List<CDetalleCalificacionNombramientoDTO>();
                    var datoM = (CalificacionNombramiento)calificacion.Contenido;

                    foreach (var item in datoM.DetalleCalificacionModificada)
                    {
                        listadoDetalleModificado.Add(CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoModificadoADto(item));
                    }

                    datosCalificacion.DetalleCalificacionModificado = listadoDetalleModificado;

                    // 01 Calificación
                    respuesta.Add(datosCalificacion);

                    // 02 Funcionario
                    var funcionario = ((CalificacionNombramiento)calificacion.Contenido).Nombramiento.Funcionario;
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario));


                    //var datos = intermedioFuncionarioLog.BuscarFuncionarioDetallePuesto(funcionario.IdCedulaFuncionario);
                    var datos = intermedioNombramientoL.DescargarNombramientoDetallePuesto(datosCalificacion.NombramientoDTO.IdEntidad);

                    if (datos != null && datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        // 03 Puesto
                        respuesta.Add((CPuestoDTO)datos.ElementAt(1));
                        // 04 Detalle Puesto
                        respuesta.Add((CDetallePuestoDTO)datos.ElementAt(2));
                    }
                    else
                    {

                        // 03 Puesto
                        //var puesto = intermedioPuesto.DetallePuestoCedula(funcionario.IdCedulaFuncionario);
                        //respuesta.Add(CPuestoL.ConvertirCPuestoGeneralDatosaDTO((Puesto)puesto));
                        respuesta.Add(new CPuestoDTO());

                        // 04 Detalle Puesto
                        //var dPuesto = intermedioDPuesto.CargarDetallePuestoCedula(funcionario.IdCedulaFuncionario);
                        //respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)dPuesto));
                        respuesta.Add(new CDetallePuestoDTO());
                    }        

                    // 05 Expediente
                    respuesta.Add(new CExpedienteFuncionarioDTO { NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente });       
                }
                else
                {
                    respuesta.Add((CErrorDTO)calificacion.Contenido);
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

        public List<CBaseDTO> ObtenerCalificacionHistorico(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                
                var calificacion = intermedio.ObtenerCalificacionHistorico(codigo);
                if (calificacion.Codigo != -1)
                {
                    var item = (C_EMU_Calificacion)calificacion.Contenido;
                    var periodo = Convert.ToInt32(item.Periodo.Substring(0, 4));
                    respuesta.Add(new CCalificacionNombramientoDTO
                        {
                            IdEntidad = item.ID,
                            Periodo = new CPeriodoCalificacionDTO { IdEntidad = periodo },
                            CalificacionDTO = new CCalificacionDTO
                            {
                                IdEntidad = Convert.ToInt32(item.Calificacion),
                                DesCalificacion = item.DesCalificacion
                            },
                            NombramientoDTO = new CNombramientoDTO { IdEntidad = 0 },
                            Nota = Convert.ToDecimal(item.Nota),
                            ObsGeneralDTO = item.DesJustificacion != null ? item.DesJustificacion : "",
                            IndEstadoDTO = item.IndPolicia ? 1 : 0 
                        });
                }
                else
                {
                    respuesta.Add((CErrorDTO)calificacion.Contenido);
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

        public List<CBaseDTO> ObtenerCalificacionFuncionario(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CDetallePuestoD intermedioDPuesto = new CDetallePuestoD(contexto);
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CFuncionarioL intermedioFuncionarioLog = new CFuncionarioL();
                CNombramientoL intermedioNombramientoL = new CNombramientoL();

                var calificacion = intermedio.ObtenerCalificacionFuncionario(codigo);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosCalificacionFuncionarioDto((USP_LISTAR_FUNCIONARIOS_CALIFICACION_Result)calificacion.Contenido);

                    // Jefe Inmediato
                    if (datosCalificacion.JefeInmediato.IdEntidad != null && datosCalificacion.JefeInmediato.IdEntidad != 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datosCalificacion.JefeInmediato.IdEntidad));
                        datosCalificacion.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }
                    else
                    {
                        datosCalificacion.JefeInmediato = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
                    }


                    // Jefe Superior
                    if (datosCalificacion.JefeSuperior.IdEntidad != null && datosCalificacion.JefeSuperior.IdEntidad != 0)
                    {
                        var dJefaturaSup = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datosCalificacion.JefeSuperior.IdEntidad));
                        datosCalificacion.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefaturaSup.Contenido));
                    }
                    else
                    {
                        datosCalificacion.JefeSuperior = new CFuncionarioDTO { IdEntidad = 0, Nombre = "", Sexo = GeneroEnum.Indefinido };
                    }
                                       

                    // 01 Calificación
                    respuesta.Add(datosCalificacion);

                    // 02 Funcionario
                    var funcionario = ((CalificacionNombramiento)calificacion.Contenido).Nombramiento.Funcionario;
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario));


                    //var datos = intermedioFuncionarioLog.BuscarFuncionarioDetallePuesto(funcionario.IdCedulaFuncionario);
                    var datos = intermedioNombramientoL.DescargarNombramientoDetallePuesto(datosCalificacion.Nombramiento.IdEntidad);
                    if (datos != null && datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        // 03 Puesto
                        respuesta.Add((CPuestoDTO)datos.ElementAt(1));
                        // 04 Detalle Puesto
                        respuesta.Add((CDetallePuestoDTO)datos.ElementAt(2));
                    }
                    else
                    {

                        // 03 Puesto
                        //var puesto = intermedioPuesto.DetallePuestoCedula(funcionario.IdCedulaFuncionario);
                        //respuesta.Add(CPuestoL.ConvertirCPuestoGeneralDatosaDTO((Puesto)puesto));
                        respuesta.Add(new CPuestoDTO());

                        // 04 Detalle Puesto
                        //var dPuesto = intermedioDPuesto.CargarDetallePuestoCedula(funcionario.IdCedulaFuncionario);
                        //respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)dPuesto));
                        respuesta.Add(new CDetallePuestoDTO());
                    }

                    // 05 Expediente
                    respuesta.Add(new CExpedienteFuncionarioDTO { NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente });
                }
                else
                {
                    respuesta.Add((CErrorDTO)calificacion.Contenido);
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


        public List<CProcAlmacenadoDTO> DescargarDatosCC()
        {
            List<CProcAlmacenadoDTO> resultado = new List<CProcAlmacenadoDTO>();

            CCalificacionNombramientoD DatosDescarga = new CCalificacionNombramientoD(contexto);
            var datos = DatosDescarga.ListarDatosCCFRSC();
            if (datos != null)
            {
                resultado = datos;
            }
            else
            {
                throw new Exception((datos.ToString()));
            }
            return resultado;
        }

        public List<CProcAlmacenadoDatosGeneralesDTO> DescargarDatosGEvaluacion()
        {
            List<CProcAlmacenadoDatosGeneralesDTO> repuesta = new List<CProcAlmacenadoDatosGeneralesDTO>();

            CCalificacionNombramientoD DatosDescarga = new CCalificacionNombramientoD(contexto);
            var datos = DatosDescarga.ListarDatosGFRSC();
            if (datos != null)
            {
                repuesta = datos;
            }
            else
            {
                throw new Exception((datos.ToString()));
            }
            return repuesta;
        }
        
        public CBaseDTO AgregarPeriodoCalificacion(CPeriodoCalificacionDTO periodo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var dato = new PeriodoCalificacion
                {
                    PK_PeriodoCalificacion = periodo.IdEntidad,
                    FecRige = periodo.FecRige,
                    FecVence = Convert.ToDateTime(periodo.FecVence),
                    FecRigeReglaTecnica = periodo.FecRigeReglaTecnica,
                    FecVenceReglaTecnica = Convert.ToDateTime(periodo.FecVenceReglaTecnica)
                };

                var calificacion = intermedio.AgregarPeriodoCalificacion(dato);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosPeriodoDto((PeriodoCalificacion)calificacion.Contenido);
                    respuesta = datosCalificacion;
                }
                else
                {
                    respuesta= (CErrorDTO)calificacion.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta= new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public CBaseDTO ObtenerPeriodoCalificacion(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var calificacion = intermedio.ObtenerPeriodoCalificacion(codigo);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosPeriodoDto((PeriodoCalificacion)calificacion.Contenido);
                    respuesta = datosCalificacion;
                }
                else
                {
                    respuesta = (CErrorDTO)calificacion.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta =new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public CBaseDTO GenerarListadoFuncionariosPeriodo(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                respuesta = intermedio.GenerarListadoFuncionariosPeriodo(codigo);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta;
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

        public CBaseDTO GenerarListaReglaTecnica(int periodo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                respuesta = intermedio.GenerarListaReglaTecnica(periodo);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta;
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
        
        public CBaseDTO CargarArchivoReglaTecnica(CCalificacionReglaTecnicaDTO regla)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var dato = new CalificacionReglaTecnica
                {
                    PK_CalificacionReglaTecnica = regla.IdEntidad,
                    ImgDocumento = regla.ImagenDocumento
                };

                var calificacion = intermedio.CargarArchivoReglaTecnica(dato);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosPeriodoDto((PeriodoCalificacion)calificacion.Contenido);
                    respuesta = datosCalificacion;
                }
                else
                {
                    respuesta = (CErrorDTO)calificacion.Contenido;
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
        
        public CBaseDTO AsignarDirectorReglaTecnica(CCalificacionReglaTecnicaDTO regla)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var dato = new CalificacionReglaTecnica
                {
                    PK_CalificacionReglaTecnica = regla.IdEntidad,
                    IndDirector = regla.DirectorDTO.IdEntidad
                };

                var calificacion = intermedio.CargarArchivoReglaTecnica(dato);
                if (calificacion.Codigo != -1)
                {
                    var datosCalificacion = ConvertirDatosPeriodoDto((PeriodoCalificacion)calificacion.Contenido);
                    respuesta = datosCalificacion;
                }
                else
                {
                    respuesta = (CErrorDTO)calificacion.Contenido;
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
        
        public List<CBaseDTO> ListarReglaTecnica(int periodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CCalificacionNombramientoD intermedioCalificaciones = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);
                CDireccionGeneralDTO direccion = new CDireccionGeneralDTO();
                CFuncionarioDTO jefatura = new CFuncionarioDTO
                {
                    IdEntidad = 0,
                    Nombre = "",
                    Sexo = GeneroEnum.Indefinido
                };

                var calificacion = intermedioCalificaciones.ListarReglaTecnica(periodo);
                if (calificacion != null)
                {
                    if (calificacion.Codigo == -1)
                    {
                        throw new Exception(((CErrorDTO)calificacion.Contenido).MensajeError);
                    }

                    foreach (var item in (List<CalificacionReglaTecnica>) calificacion.Contenido)
                    {
                        //Buscar Dirección 
                        var dDireccion = intermedioDireccion.CargarDireccionGeneralPorID(item.IndDireccionGeneral);
                        direccion = CDireccionGeneralL.ConvertirDireccionGeneralADTO(dDireccion);

                        // Buscar funcionario por id
                        if (item.IndDirector > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.IndDirector));
                            jefatura = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            jefatura.Nombre = jefatura.Nombre.TrimEnd() + " " + jefatura.PrimerApellido.TrimEnd() + " " + jefatura.SegundoApellido.TrimEnd();
                        }

                        respuesta.Add(new CCalificacionReglaTecnicaDTO
                        {
                            IdEntidad = item.PK_CalificacionReglaTecnica,
                            Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                            DireccionDTO = direccion,
                            DirectorDTO = jefatura,
                            IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                            EmailDTO = item.DesCorreo,
                            NumFuncionariosDTO = Convert.ToInt32(item.NumFuncionarios),
                            NumExcelentesDTO = Convert.ToInt32(item.NumFuncionariosExcelentes),
                            PorcExcelentesDTO = (item.NumFuncionariosExcelentes * 100) / item.NumFuncionarios,
                            ImagenDocumento = item.ImgDocumento,                           
                        });
                    }
                }
                else
                {
                    respuesta.Add(new CErrorDTO { Mensaje = "No se encontró la lista" });
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }

            return respuesta;
        }
        
        public List<CBaseDTO> ObtenerReglaTecnica(int codRegla)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CCalificacionNombramientoD intermedioCalificaciones = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);
                CDireccionGeneralDTO direccion = new CDireccionGeneralDTO();
                CFuncionarioDTO jefatura = new CFuncionarioDTO
                {
                    IdEntidad = 0,
                    Nombre = "",
                    Sexo = GeneroEnum.Indefinido
                };

                var calificacion = intermedioCalificaciones.ObtenerReglaTecnica(codRegla);
                if (calificacion != null)
                {
                    foreach (var item in (List<CalificacionReglaTecnica>)calificacion.Contenido)
                    {
                        //Buscar Dirección 
                        var dDireccion = intermedioDireccion.CargarDireccionGeneralPorID(item.IndDireccionGeneral);
                        direccion = CDireccionGeneralL.ConvertirDireccionGeneralADTO(dDireccion);

                        // Buscar funcionario por id
                        if (item.IndDirector > 0)
                        {
                            var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.IndDirector));
                            jefatura = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                            jefatura.Nombre = jefatura.Nombre.TrimEnd() + " " + jefatura.PrimerApellido.TrimEnd() + " " + jefatura.SegundoApellido.TrimEnd();
                        }

                        respuesta.Add(new CCalificacionReglaTecnicaDTO
                        {
                            IdEntidad = item.PK_CalificacionReglaTecnica,
                            Periodo = ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                            DireccionDTO = direccion,
                            DirectorDTO = jefatura,
                            IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                            EmailDTO = item.DesCorreo,
                            NumFuncionariosDTO = Convert.ToInt32(item.NumFuncionarios),
                            NumExcelentesDTO = Convert.ToInt32(item.NumFuncionariosExcelentes),
                            PorcExcelentesDTO = (item.NumFuncionariosExcelentes * 100) / item.NumFuncionarios,
                            ImagenDocumento = item.ImgDocumento,
                        });
                    }
                }
                else
                {
                    respuesta.Add(new CErrorDTO { Mensaje = "No se encontró la lista" });
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }

            return respuesta;
        }

        private void ObtenerDetalleCalificacion (decimal suma, out int idCalificacion, out string DesCalificacion)
        {
            idCalificacion = 0;
            DesCalificacion = "";

            if (suma >= 95 && suma <= 100)
            {
                DesCalificacion = "Excelente";
                idCalificacion = 1;
            }
            else if (suma >= 85 && suma < 95)
            {
                DesCalificacion = "Muy Bueno";
                idCalificacion = 2;
            }
            else if (suma >= 75 && suma < 85)
            {
                DesCalificacion = "Bueno";
                idCalificacion = 3;
            }
            else if (suma < 75)
            {
                DesCalificacion = "Regular";
                idCalificacion = 4;
            }
            else if (suma == 0)
            {
                DesCalificacion = "Deficiente";
                idCalificacion = 5;
            }
        }


        public CBaseDTO AgregarCalificacionHistorico(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario, decimal nota, string justificacion, bool EsPolicia, int IndFormulario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                int idCalificacion = 0;
                string DesCalificacion = "";

                ObtenerDetalleCalificacion (nota, out idCalificacion, out DesCalificacion);

                var dato = new C_EMU_Calificacion
                {
                    Periodo = periodo.IdEntidad.ToString(),
                    Cedula = funcionario.Cedula,
                    Nota = nota,
                    Calificacion = Convert.ToInt16(idCalificacion),
                    DesCalificacion = DesCalificacion,
                    DesJustificacion = justificacion,
                    IndPolicia = EsPolicia,
                    IndFormulario = IndFormulario

                };

                var registro = intermedio.AgregarCalificacionHistorico(dato);
                if (registro.Codigo != -1)
                {
                    var datosCalificacion = (C_EMU_Calificacion)registro.Contenido;
                    
                    respuesta = new CCalificacionNombramientoDTO
                        {
                            IdEntidad = datosCalificacion.ID,
                            Periodo = new CPeriodoCalificacionDTO { IdEntidad = Convert.ToInt32(datosCalificacion.Periodo) },
                            CalificacionDTO = new CCalificacionDTO
                            {
                                IdEntidad = Convert.ToInt32(datosCalificacion.Calificacion),
                                DesCalificacion = datosCalificacion.DesCalificacion
                            },
                            NombramientoDTO = new CNombramientoDTO { IdEntidad = 0 },
                        };
                }
                else
                {
                    respuesta = (CErrorDTO)registro.Contenido;
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


        public CBaseDTO AgregarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var datoPeriodo = intermedio.ObtenerPeriodoCalificacion(periodo.IdEntidad);
                var dato = new CalificacionNombramientoFuncionarios
                {
                    PeriodoCalificacion = (PeriodoCalificacion)datoPeriodo.Contenido,
                    FK_Nombramiento = detalle.Nombramiento.IdEntidad,
                    FK_Puesto = detalle.Puesto.IdEntidad,
                    FK_Funcionario = detalle.Funcionario.IdEntidad,
                    FecRige = DateTime.Today,
                    FecVence = DateTime.Today,
                    FK_Seccion = detalle.indSeccion,
                    FK_Departamento = detalle.indDepartamento,
                    FK_DireccionGeneral = detalle.indDireccion,
                    FK_Division = detalle.indDivision,
                    FK_OcupacionReal = detalle.indOcupacion,
                    IdJefeInmediato = detalle.JefeInmediato.IdEntidad,
                    IdJefeSuperior = detalle.JefeSuperior.IdEntidad,
                    NumTipo = 1
                };

                var registro = intermedio.AgregarCalificacionNombramientoFuncionario(dato);
                if (registro.Codigo != -1)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro,
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)registro.Contenido;
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

        public CBaseDTO ActualizarCalificacionNombramientoFuncionario(CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);

                var dato = new CalificacionNombramientoFuncionarios
                {
                    PK_CalificacionNombramiento = detalle.IdEntidad,
                    FK_Nombramiento = detalle.Nombramiento.IdEntidad,
                    FK_Puesto = detalle.Puesto.IdEntidad,
                    FK_DetallePuesto = detalle.indDetallePuesto,
                    FK_Funcionario = detalle.Funcionario.IdEntidad,
                    FK_Seccion = detalle.indSeccion,
                    FK_Departamento = detalle.indDepartamento,
                    FK_DireccionGeneral = detalle.indDireccion,
                    FK_Division = detalle.indDivision
                };

                var registro = intermedio.ActualizarCalificacionNombramientoFuncionario(dato);
                if (registro.Codigo != -1)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)registro.Contenido;
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

        public List<CBaseDTO> ObtenerPeriodosCalificacion()
        {
            List<CBaseDTO> periodos = new List<CBaseDTO>();
            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                var respuesta = intermedio.ObtenerPeriodosCalificacion();
                if (respuesta.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
                foreach (var item in ((List<PeriodoCalificacion>)respuesta.Contenido).OrderByDescending(Q=> Q.PK_PeriodoCalificacion).ToList())
                {
                    periodos.Add(
                        new CPeriodoCalificacionDTO
                        {
                            IdEntidad = item.PK_PeriodoCalificacion
                        }
                    );
                }
                return periodos;
            }
            catch (Exception ex)
            {
                periodos.Clear();
                periodos.Add(new CErrorDTO { MensajeError = ex.Message });
                return periodos;
            }
        }
        public List<CBaseDTO> GenerarListaNoEvaluados(int periodo)
        {
            List<CBaseDTO> listado = new List<CBaseDTO>();
            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

                var datosNoEvaluados = intermedio.ObtenerFuncionariosNoEvaluadosPorPeriodo(periodo);
                if (datosNoEvaluados.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datosNoEvaluados.Contenido).MensajeError);
                }
                foreach (var item in (List<CCalificacionNombramientoFuncionarioDTO>)datosNoEvaluados.Contenido)
                {
                    if (item.JefeInmediato.IdEntidad > 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.JefeInmediato.IdEntidad));
                        item.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

                    if (item.JefeSuperior.IdEntidad > 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.JefeSuperior.IdEntidad));
                        item.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

                    listado.Add(item);
                }
                return listado;
            }
            catch (Exception ex)
            {
                listado.Clear();
                listado.Add(new CErrorDTO { MensajeError = ex.Message });
                return listado;
            }
        }

        public List<CBaseDTO> GenerarListaEvaluados(int periodo)
        {
            List<CBaseDTO> listado = new List<CBaseDTO>();
            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

                var datosEvaluados = intermedio.ObtenerFuncionariosEvaluadosPorPeriodo(periodo);
                if (datosEvaluados.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datosEvaluados.Contenido).MensajeError);
                }
                foreach (var item in (List<CCalificacionNombramientoDTO>)datosEvaluados.Contenido)
                {
                    if (item.JefeInmediato.IdEntidad > 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.JefeInmediato.IdEntidad));
                        item.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

                    if (item.JefeSuperior.IdEntidad > 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(item.JefeSuperior.IdEntidad));
                        item.JefeSuperior = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

                    //CCalificacionNombramientoDTO dato = new CCalificacionNombramientoDTO
                    //{
                    //    Funcionario = new CFuncionarioDTO
                    //    {
                    //        Cedula = item.cedulaFuncionario,
                    //        Nombre = item.nombreFuncionario,
                    //        PrimerApellido = item.primerApellidoFuncionario,
                    //        SegundoApellido = item.segundoApellidoFuncionario,
                    //        Sexo = (GeneroEnum)Convert.ToInt32(item.generoFuncionario)
                    //    },
                    //    CalificacionDTO = new CCalificacionDTO
                    //    {
                    //        IdEntidad = item.PK_Calificacion,
                    //        DesCalificacion = item.DesCalificacion
                    //    },
                    //    Puesto = new CPuestoDTO
                    //    {
                    //        CodPuesto = item.CodPuesto,
                    //        PuestoConfianza = Convert.ToBoolean(item.IndPuestoConfianza),
                    //        NivelOcupacional = Convert.ToInt32(item.IndNivelOcupacional)
                    //    },
                    //    JefeInmediato = item.cedulaInmediato != null ? new CFuncionarioDTO
                    //    {
                    //        Cedula = item.cedulaInmediato,
                    //        Nombre = item.nombreInmediato,
                    //        PrimerApellido = item.primerApellidoInmediato,
                    //        SegundoApellido = item.segundoApellidoInmediato,
                    //        Sexo = GeneroEnum.Indefinido
                    //    } : null,
                    //    JefeSuperior = item.cedulaSuperior != null ? new CFuncionarioDTO
                    //    {
                    //        Cedula = item.cedulaSuperior,
                    //        Nombre = item.nombreSuperior,
                    //        PrimerApellido = item.primerApellidoSuperior,
                    //        SegundoApellido = item.segundoApellidoSuperior,
                    //        Sexo = GeneroEnum.Indefinido
                    //    } : null,
                    //    IndFormularioDTO = item.IndFormulario,
                    //    FecCreacionDTO = item.FecCreacion,
                    //    IndRatificacionDTO = item.IndRatificado,
                    //    FecRatificacionDTO = Convert.ToDateTime(item.FecRatificacion),
                    //    IndEntregadoDTO = Convert.ToBoolean(item.IndEntregado),
                    //    IndConformidadDTO = Convert.ToBoolean(item.IndConformidad)
                    //};
                    listado.Add(item);
                }
                return listado;
            }
            catch (Exception ex)
            {
                listado.Clear();
                listado.Add(new CErrorDTO { MensajeError = ex.Message });
                return listado;
            }
        }


        public CRespuestaDTO ObtenerCantidadNoEvaluados(int periodo)
        {
            try
            {
                List<CBaseDTO> datos = GenerarListaNoEvaluados(periodo);
                if (datos.Count() == 0)
                {
                    throw new Exception("No existen datos del periodo " + periodo.ToString());
                }
                if (datos[0].GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)datos[0]).MensajeError);
                }
                return new CRespuestaDTO { Codigo = 1, Contenido = datos.Count() };
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public CRespuestaDTO ObtenerCantidadEvaluados(int periodo)
        {
            try
            {
                List<CBaseDTO> datos = GenerarListaEvaluados(periodo);
                if (datos.Count() == 0)
                {
                    throw new Exception("No existen datos del periodo " + periodo.ToString());
                }
                if (datos[0].GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)datos[0]).MensajeError);
                }
                return new CRespuestaDTO { Codigo = 1, Contenido = datos.Count() };
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public CRespuestaDTO AgregarAutoEvaluacion(int periodo, int codFuncionario, decimal nota)
        {
            try
            {
                CCalificacionNombramientoD intermedio = new CCalificacionNombramientoD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

                var respuesta = intermedio.AgregarAutoEvaluacion(periodo, codFuncionario, nota);
                if (respuesta.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }

                return new CRespuestaDTO { Codigo = 1, Contenido = codFuncionario };
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        #endregion
    }
}
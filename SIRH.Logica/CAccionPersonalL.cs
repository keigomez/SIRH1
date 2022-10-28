using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{//
    public class CAccionPersonalL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CAccionPersonalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        internal static CAccionPersonalDTO ConvertirDatosADto(AccionPersonal item)
        {
            return new CAccionPersonalDTO
            {
                IdEntidad = item.PK_AccionPersonal,
                NumAccion = item.NumAccion,
                AnioRige = Convert.ToInt16(item.AnioRige),
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = (item.FecVence.HasValue) ? Convert.ToDateTime(item.FecVence) : item.FecVence,
                FecRigeIntegra = Convert.ToDateTime(item.FecRigeIntegra),
                FecVenceIntegra = (item.FecVenceIntegra.HasValue) ? Convert.ToDateTime(item.FecVenceIntegra) : item.FecVenceIntegra, //Convert.ToDateTime(item.FecVenceIntegra),
                Estado = new CEstadoBorradorDTO
                {
                    IdEntidad = item.EstadoBorrador.PK_EstadoBorrador,
                    DesEstadoBorrador = item.EstadoBorrador.DesEstadoBorrador
                },
                TipoAccion = new CTipoAccionPersonalDTO
                {
                    IdEntidad = item.TipoAccionPersonal.PK_TipoAccionPersonal,
                    DesTipoAccion = item.TipoAccionPersonal.DesTipoAccion
                },
                Nombramiento = new CNombramientoDTO
                {
                    IdEntidad = Convert.ToInt32(item.Nombramiento.PK_Nombramiento),
                    Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(item.Nombramiento.Puesto)
                },
                Observaciones = item.Observaciones,
                CodigoModulo = item.CodModulo,
                CodigoObjetoEntidad = item.CodObjetoEntidad,
                IndDato = item.IndDato
            };
        }

        internal CAccionPersonalHistoricoDTO ConvertirDatosHistoricoADto(C_EMU_AccionPersonal item)
        {
            var desClase1 = "";
            var desClase2 = "";
            var desNombre = "";
            var desApellido1 = "";
            var desApellido2 = "";

            CClaseD intermedioClase = new CClaseD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try { desClase1 = intermedioClase.CargarClasePorID(Convert.ToInt32(item.Clase1)).DesClase.TrimEnd(); }
            catch { desClase1 = ""; }

            try   { desClase2 = intermedioClase.CargarClasePorID(Convert.ToInt32(item.Clase2)).DesClase.TrimEnd(); }
            catch { desClase2 = ""; }

            try
            {
                var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(item.Cedula);

                if (funcionario != null)
                {
                    desNombre = funcionario.NomFuncionario.TrimEnd();
                    desApellido1 = funcionario.NomPrimerApellido.TrimEnd();
                    desApellido2 = funcionario.NomSegundoApellido.TrimEnd();
                }
                else
                {
                    var exFuncionario = intermedioFuncionario.BuscarExFuncionarioCedula(item.Cedula);
                    if (exFuncionario != null) {
                        desNombre = exFuncionario.NOMBRE.TrimEnd();
                        desApellido1 = exFuncionario.PRIMER_APELLIDO.TrimEnd();
                        desApellido2 = exFuncionario.SEGUNDO_APELLIDO.TrimEnd();
                    }
                }
            }
            catch {
            }

            return new CAccionPersonalHistoricoDTO
            {
                IdEntidad = item.ID,
                NumAccion = item.NumAccion,
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                CodAccion = Convert.ToInt16(item.CodAccion),
                Explicacion = item.Explicacion,
                Cedula = item.Cedula,
                Nombre = desNombre,
                Apellido1 = desApellido1,
                Apellido2 = desApellido2,
                CodClase = item.Clase1,
                DesClase = desClase1,
                CodClase2 = item.Clase2,
                DesClase2 = desClase2,
                CodPuesto = item.NumPuesto1,
                CodPuesto2 = item.NumPuesto2,
                Disfrutado = Convert.ToInt32(item.Disfrutado1),
                Disfrutado2 = Convert.ToInt32(item.Disfrutado2),
                Autorizado = Convert.ToInt32(item.Autorizado1),
                Autorizado2 = Convert.ToInt32(item.Autorizado2),
                Categoria = item.Categoria1,
                Categoria2 = item.Categoria2,
                MtoSalarioBase = item.Salario2.HasValue ? Convert.ToDecimal(item.Salario1).ToString("#,##0.00") : "0.00",
                MtoAumentosAnuales = item.Aumentos2.HasValue ? Convert.ToDecimal(item.Aumentos1).ToString("#,##0.00") : "0.00",
                MtoRecargo = item.Recargo2.HasValue ? Convert.ToDecimal(item.Recargo1).ToString("#,##0.00") : "0.00",
                MtoGradoGrupo = item.Grupo2.HasValue ? Convert.ToDecimal(item.Grupo1).ToString("#,##0.00") : "0.00",
                MtoProhibicion = item.Prohibicion2.HasValue ? Convert.ToDecimal(item.Prohibicion1).ToString("#,##0.00") : "0.00",
                MtoOtros = item.Otros2.HasValue ? Convert.ToDecimal(item.Otros1).ToString("#,##0.00") : "0.00",
                MtoTotal =  (item.Salario1.HasValue ? Convert.ToDecimal(item.Salario1) : 0) +
                            (item.Aumentos1.HasValue ? Convert.ToDecimal(item.Aumentos1) : 0) +
                            (item.Recargo1.HasValue ? Convert.ToDecimal(item.Recargo1) : 0) +
                            (item.Grupo1.HasValue ? Convert.ToDecimal(item.Grupo1) : 0) +
                            (item.Prohibicion1.HasValue ? Convert.ToDecimal(item.Prohibicion1) : 0) +
                            (item.Otros1.HasValue ? Convert.ToDecimal(item.Otros1) : 0),


                MtoSalarioBase2 = item.Salario2.HasValue ? Convert.ToDecimal(item.Salario2).ToString("#,##0.00") : "0.00",
                MtoAumentosAnuales2 = item.Aumentos2.HasValue ? Convert.ToDecimal(item.Aumentos2).ToString("#,##0.00") : "0.00",
                MtoRecargo2 = item.Recargo2.HasValue ? Convert.ToDecimal(item.Recargo2).ToString("#,##0.00") : "0.00",
                MtoGradoGrupo2 = item.Grupo2.HasValue ? Convert.ToDecimal(item.Grupo2).ToString("#,##0.00") : "0.00",
                MtoProhibicion2 = item.Prohibicion2.HasValue ? Convert.ToDecimal(item.Prohibicion2).ToString("#,##0.00") : "0.00",
                MtoOtros2 = item.Otros2.HasValue ? Convert.ToDecimal(item.Otros2).ToString("#,##0.00") : "0.00",
                MtoTotal2 = (item.Salario2.HasValue ? Convert.ToDecimal(item.Salario2) : 0) +
                            (item.Aumentos2.HasValue ? Convert.ToDecimal(item.Aumentos2) : 0) +
                            (item.Recargo2.HasValue ? Convert.ToDecimal(item.Recargo2) : 0) +
                            (item.Grupo2.HasValue ? Convert.ToDecimal(item.Grupo2) : 0) +
                            (item.Prohibicion2.HasValue ? Convert.ToDecimal(item.Prohibicion2) : 0) +
                            (item.Otros2.HasValue ? Convert.ToDecimal(item.Otros2) : 0)
            };
        }
        
        internal static CDetalleAccionPersonalDTO ConvertirDatosDetalleADto(DetalleAccionPersonal item)
        {
            var porProh = Convert.ToDecimal(item.DetallePuesto.PorProhibicion != null ? item.DetallePuesto.PorProhibicion : 0);
            var porDed = Convert.ToDecimal(item.DetallePuesto.PorDedicacion != null ? item.DetallePuesto.PorDedicacion : 0);

            return new CDetalleAccionPersonalDTO
            {
                IdEntidad = item.PK_Detalle,
                CodNombramiento = item.CodNombramiento,
                CodPrograma = item.CodPrograma,
                CodSeccion = item.CodSeccion,
                CodEspecialidad = Convert.ToInt32(item.CodEspecialidad),
                CodSubespecialidad = Convert.ToInt32((item.CodSubespecialidad != null) ? item.CodSubespecialidad : 0),
                CodDetallePuesto = item.CodDetallePuesto,
                NumHoras = item.NumHoras,
                NumAnualidad = Convert.ToInt32(item.NumAnualidad),
                MtoAnual = Convert.ToDecimal(item.DetallePuesto.EscalaSalarial.MtoAumento),
                MtoPunto = Convert.ToDecimal(item.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera),
                PorProhibicion = porDed > porProh ? porDed : porProh,
                NumGradoGrupo = item.MtoGradoGrupo,
                MtoSalarioBase = Convert.ToDecimal(item.DetallePuesto.EscalaSalarial.MtoSalarioBase),
                MtoAumentosAnuales = Convert.ToDecimal(item.DetallePuesto.EscalaSalarial.MtoAumento) * Convert.ToInt32(item.NumAnualidad),
                MtoGradoGrupo = item.MtoGradoGrupo * Convert.ToDecimal(item.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera),
                MtoRecargo = Convert.ToDecimal(item.MtoRecargo),
                MtoOtros = item.MtoOtros,
                IndCategoria = Convert.ToInt32(item.DetallePuesto.EscalaSalarial.IndCategoria),
                Accion = new CAccionPersonalDTO
                {
                    IdEntidad = item.AccionPersonal.PK_AccionPersonal
                },
                DetallePuesto = new CDetallePuestoDTO
                {
                    IdEntidad= item.DetallePuesto.PK_DetallePuesto,
                    Clase = new CClaseDTO
                    {
                        IdEntidad = item.DetallePuesto.Clase.PK_Clase,
                        DesClase = item.DetallePuesto.Clase.DesClase
                    }
                }
            };
        }

        public CBaseDTO AgregarAccion(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                                      CTipoAccionPersonalDTO tipo, CAccionPersonalDTO registro,
                                      CDetalleAccionPersonalDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            var Lugar = "L";

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CEscalaSalarialD intermedioEscala = new CEscalaSalarialD(contexto);
                CPeriodoEscalaSalarialD intermedioPeriodo = new CPeriodoEscalaSalarialD(contexto);

                int indEscala = 0;
                int indClase = 0;
                
                Lugar = "L - AP";
                AccionPersonal datosRegistro = new AccionPersonal
                {
                    NumAccion = registro.NumAccion,
                    AnioRige = registro.AnioRige.ToString(),
                    FecRige = Convert.ToDateTime(registro.FecRige),
                    FecVence = registro.FecVence,
                    FecRigeIntegra = Convert.ToDateTime(registro.FecRigeIntegra),
                    FecVenceIntegra =registro.FecVenceIntegra,
                    Observaciones = registro.Observaciones,
                    CodModulo = registro.CodigoModulo,
                    CodObjetoEntidad = registro.CodigoObjetoEntidad,
                    IndDato = registro.IndDato
                };

                Lugar = "L - Tipo";
                var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID(tipo.IdEntidad);

                if (entidadTipo.Codigo != -1)
                {
                    datosRegistro.TipoAccionPersonal = (TipoAccionPersonal)entidadTipo.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadTipo).Contenido;
                    throw new Exception("Tipo");
                }

                // Detalle
                DetalleAccionPersonal datosDetalle = null;

                Lugar = "L - Detalle";

                var tipoAP = ((TipoAccionPersonal)entidadTipo.Contenido);
                if (tipoAP.IndCategoria == 2  // Acciones de Personal de modificación de rubros salariales
                    || tipoAP.PK_TipoAccionPersonal == 64 //Reaj. Aprobación desarraigo
                    || tipoAP.PK_TipoAccionPersonal == 65 //Reaj.Eliminación desarraigo
                    )
                {
                    if (detalle != null)
                    {
                        datosDetalle = new DetalleAccionPersonal
                        {
                            CodNombramiento = detalle.CodNombramiento,
                            CodPrograma = detalle.CodPrograma,
                            CodSeccion = detalle.CodSeccion,
                            CodEspecialidad = detalle.CodEspecialidad,
                            CodSubespecialidad = detalle.CodSubespecialidad,
                            NumHoras = detalle.NumHoras,
                            NumAnualidad = detalle.NumAnualidad,
                            MtoRecargo = detalle.MtoRecargo,
                            MtoGradoGrupo = detalle.NumGradoGrupo,
                            PorProhibicion = detalle.PorProhibicion,
                            MtoOtros = detalle.MtoOtros
                        };
                        var periodo = intermedioPeriodo.ObtenerPeriodoActual();
                        if(periodo.Codigo > 0 )
                        {
                            var escala = intermedioEscala.BuscarEscalaCategoriaPeriodo(detalle.IndCategoria, ((PeriodoEscalaSalarial)periodo.Contenido).PK_Periodo);
                            if (escala.Codigo > 0)
                            {
                                indEscala = ((EscalaSalarial)escala.Contenido).PK_Escala;
                            }
                        }
                       

                        //indEscala = ((EscalaSalarial)escala.Contenido).PK_Escala;
                        indClase = detalle.CodClase;
                    }
                }

                Lugar = "L - Nomb";
                var datoNombramiento = new Nombramiento();
                if (registro.Nombramiento == null)
                {
                    datoNombramiento = intermedio.CargarNombramientoCedula(funcionario.Cedula);
                }
                else
                {
                    datoNombramiento = intermedioNombramiento.CargarNombramiento(registro.Nombramiento.IdEntidad);
                }
                

                if (datoNombramiento != null)
                {
                    datosRegistro.Nombramiento = datoNombramiento;
                }
                else
                {
                    //respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = -1, Mensaje = "No existe ese nombramiento" };
                    new CErrorDTO { IdEntidad = -1, MensajeError = "No existe este nombramiento" };
                    throw new Exception("Nombramiento");
                }

                Lugar = "L - Estado";
                var estadoA = intermedioEstado.CargarEstadoBorradorPorID(7);

                if (estadoA.Codigo != -1)
                {
                    datosRegistro.EstadoBorrador = (EstadoBorrador)estadoA.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = -1, Mensaje ="Estado" };
                    throw new Exception("Estado");
                }

                Lugar = "L - Guardar";
                respuesta = intermedio.GuardarAccion(datosRegistro, datosDetalle, indClase, indEscala);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = ((CErrorDTO)((CRespuestaDTO)respuesta).Contenido);
                    throw new Exception("Guardar");
                }
                else
                {
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje += " " +  Lugar + " " + ex.Message +  " " +(ex.InnerException != null ? ex.InnerException.Message : "") +" Logica";
                return respuesta;
            }
        }

        //public CBaseDTO AgregarRubro(CDetallePuestoDTO detallePuesto, CDetalleAccionPersonalDTO detalle)
        //{
        //    CBaseDTO respuesta = new CBaseDTO();

        //    try
        //    {
        //        CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
        //        CDetallePuestoD intermedioDP = new CDetallePuestoD(contexto);

        //        var IndDetallePuestoAnterior = detallePuesto.IdEntidad;

        //        /// INCLUIR EL DETALLE PUESTO, LOS CAMPOS DE Prohibición y Dedicación
        //        /// 
        //        ///
        //        DetallePuesto datosDetalle = new DetallePuesto
        //        {
        //            FK_Puesto = detallePuesto.Puesto.IdEntidad,
        //            FK_Especialidad = detallePuesto.Especialidad.IdEntidad,
        //            FK_Clase = detallePuesto.Clase.IdEntidad,
        //            IndEstadoDetallePuesto = 1,
        //            FecRige = detallePuesto.FecRige,
        //            FK_Escala = detallePuesto.EscalaSalarial.IdEntidad,
        //            PorProhibicion = detallePuesto.PorProhibicion,
        //            PorDedicacion = detallePuesto.PorDedicacion,
        //            FK_Nombramiento = detallePuesto.FK_Nombramiento
        //        };

        //        if (detallePuesto.SubEspecialidad != null && detallePuesto.SubEspecialidad.IdEntidad > 0)
        //            datosDetalle.FK_SubEspecialidad = Convert.ToInt32(detallePuesto.SubEspecialidad.IdEntidad);

        //        if (detallePuesto.OcupacionReal != null && detallePuesto.OcupacionReal.IdEntidad > 0)
        //            datosDetalle.FK_OcupacionReal = Convert.ToInt32(detallePuesto.OcupacionReal.IdEntidad);

        //        respuesta = intermedioDP.GuardarDetallePuesto(datosDetalle);

        //        if (((CRespuestaDTO)respuesta).Codigo > 0)
        //        {
        //            detallePuesto.IdEntidad = ((DetallePuesto)((CRespuestaDTO)respuesta).Contenido).PK_DetallePuesto;
        //            if (detalle.PorBonificacion > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 3, detalle.PorBonificacion);

        //            if (detalle.PorCarreraPolicial > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 4, detalle.PorCarreraPolicial);

        //            if (detalle.PorConsulta > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 9, detalle.PorConsulta);

        //            if (detalle.PorCurso > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 10, detalle.PorCurso);

        //            if (detalle.PorDisponibilidad > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 13, detalle.PorDisponibilidad);

        //            if (detalle.PorGradoPolicial > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 16, detalle.PorGradoPolicial);

        //            if (detalle.PorIndCompetitividad > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 18, detalle.PorIndCompetitividad);

        //            if (detalle.PorQuinquenio > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 23, detalle.PorQuinquenio);

        //            if (detalle.PorInstruccionPolicial > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 25, detalle.PorRiesgoJudicial);

        //            if (detalle.PorRiesgo > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 30, detalle.PorRiesgo);

        //            if (detalle.PorPeligrosidad > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 35, detalle.PorPeligrosidad);

        //            if (detalle.MtoRecargo > 0)
        //                respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 32, detalle.MtoRecargo);

        //            if(IndDetallePuestoAnterior > 0)
        //                respuesta = intermedio.ActualizarEstadoDetallePuesto(IndDetallePuestoAnterior, 2);
        //        }

        //        return respuesta;

        //        //if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
        //        //{
        //        //    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
        //        //    throw new Exception();
        //        //}
        //        //else
        //        //{
        //        //    return respuesta;
        //        //}
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = (error.InnerException != null ? error.InnerException.Message : error.Message)
        //        };
        //        return respuesta;
        //    }
        //}
        public CBaseDTO AgregarRubro(CDetallePuestoDTO detallePuesto, CDetalleAccionPersonalDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                CDetallePuestoD intermedioDP = new CDetallePuestoD(contexto);

                var IndDetallePuestoAnterior = detallePuesto.IdEntidad;

                /// INCLUIR EL DETALLE PUESTO, LOS CAMPOS DE Prohibición y Dedicación
                /// 
                ///
                DetallePuesto datosDetalle = new DetallePuesto
                {
                    //FK_SubEspecialidad = Convert.ToInt32(detallePuesto.SubEspecialidad.IdEntidad),
                    //FK_OcupacionReal = Convert.ToInt32(detallePuesto.OcupacionReal.IdEntidad),
                    FK_Puesto = detallePuesto.Puesto.IdEntidad,
                    FK_Especialidad = detallePuesto.Especialidad.IdEntidad,
                    FK_Clase = detallePuesto.Clase.IdEntidad,
                    IndEstadoDetallePuesto = 1,
                    FecRige = detallePuesto.FecRige,
                    FK_Escala = detallePuesto.EscalaSalarial.IdEntidad,
                    PorProhibicion = detallePuesto.PorProhibicion,
                    PorDedicacion = detallePuesto.PorDedicacion,
                    FK_Nombramiento = detallePuesto.FK_Nombramiento
                };

                if (detallePuesto.SubEspecialidad != null && detallePuesto.SubEspecialidad.IdEntidad > 0)
                {
                    datosDetalle.FK_SubEspecialidad = Convert.ToInt32(detallePuesto.SubEspecialidad.IdEntidad);
                }

                if (detallePuesto.OcupacionReal != null && detallePuesto.OcupacionReal.IdEntidad > 0)
                {
                    datosDetalle.FK_OcupacionReal = Convert.ToInt32(detallePuesto.OcupacionReal.IdEntidad);
                }

                respuesta = intermedioDP.GuardarDetallePuesto(datosDetalle);

                if (((CRespuestaDTO)respuesta).Codigo > 0)
                {
                    detallePuesto.IdEntidad = ((DetallePuesto)((CRespuestaDTO)respuesta).Contenido).PK_DetallePuesto;

                    var datoRubro = new DetallePuestoRubro
                    {
                        FK_DetallePuesto = detallePuesto.IdEntidad,
                        FecRige = detallePuesto.FecRige.Value
                    };


                    if (detalle.PorBonificacion > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 3;
                        datoRubro.PorValor = detalle.PorBonificacion;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }


                    if (detalle.PorCarreraPolicial > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 4;
                        datoRubro.PorValor = detalle.PorCarreraPolicial;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }


                    if (detalle.PorConsulta > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 9;
                        datoRubro.PorValor = detalle.PorConsulta;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorCurso > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 10;
                        datoRubro.PorValor = detalle.PorCurso;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorDisponibilidad > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 13;
                        datoRubro.PorValor = detalle.PorDisponibilidad;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorGradoPolicial > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 16;
                        datoRubro.PorValor = detalle.PorGradoPolicial;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorIndCompetitividad > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 18;
                        datoRubro.PorValor = detalle.PorIndCompetitividad;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorQuinquenio > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 23;
                        datoRubro.PorValor = detalle.PorQuinquenio;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorInstruccionPolicial > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 25;
                        datoRubro.PorValor = detalle.PorQuinquenio;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorRiesgo > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 30;
                        datoRubro.PorValor = detalle.PorRiesgo;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.MtoRecargo > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 32;
                        datoRubro.PorValor = detalle.MtoRecargo;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    if (detalle.PorPeligrosidad > 0)
                    {
                        datoRubro.FK_ComponenteSalarial = 35;
                        datoRubro.PorValor = detalle.PorPeligrosidad;
                        respuesta = intermedio.GuardarDetalleRubro(datoRubro);
                    }

                    //if (detalle.PorBonificacion > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 3, detalle.PorBonificacion);

                    //if (detalle.PorCarreraPolicial > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 4, detalle.PorCarreraPolicial);

                    //if (detalle.PorConsulta > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 9, detalle.PorConsulta);

                    //if (detalle.PorCurso > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 10, detalle.PorCurso);

                    //if (detalle.PorDisponibilidad > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 13, detalle.PorDisponibilidad);

                    //if (detalle.PorGradoPolicial > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 16, detalle.PorGradoPolicial);

                    //if (detalle.PorIndCompetitividad > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 18, detalle.PorIndCompetitividad);

                    //if (detalle.PorQuinquenio > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 23, detalle.PorQuinquenio);

                    //if (detalle.PorInstruccionPolicial > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 25, detalle.PorRiesgoJudicial);

                    //if (detalle.PorRiesgo > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 30, detalle.PorRiesgo);

                    //if (detalle.PorPeligrosidad > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 35, detalle.PorPeligrosidad);

                    //if (detalle.MtoRecargo > 0)
                    //    respuesta = intermedio.GuardarDetalleRubro(detallePuesto.IdEntidad, 32, detalle.MtoRecargo);
                }

                respuesta = intermedio.ActualizarEstadoDetallePuesto(IndDetallePuestoAnterior, 2);

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
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = (error.InnerException != null ? error.InnerException.Message : "")
                };
                return respuesta;
            }
        }

        public List<CBaseDTO> ObtenerAccionOriginal(string numAccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CDetallePuestoD intermedioPuesto = new CDetallePuestoD(contexto);

                var registro = intermedio.ObtenerAccion(numAccion);

                if (registro.Codigo > 0)
                {
                    // Acción  00
                    var datoRegistro = ConvertirDatosADto((AccionPersonal)registro.Contenido);
                    respuesta.Add(datoRegistro);


                    // Estado Borrador 01
                    var datoEstado = intermedioEstado.CargarEstadoBorradorPorID
                        (((AccionPersonal)registro.Contenido).EstadoBorrador.PK_EstadoBorrador);

                    respuesta.Add(CEstadoBorradorL.ConvertirEstadoBorradorADto((EstadoBorrador)datoEstado.Contenido));


                    // Tipo Acción Personal 02
                    var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID
                        (((AccionPersonal)registro.Contenido).TipoAccionPersonal.PK_TipoAccionPersonal);

                    respuesta.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto((TipoAccionPersonal)entidadTipo.Contenido));


                    // Funcionario 03
                    var funcionario = ((AccionPersonal)registro.Contenido).Nombramiento.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    // Expediente 04
                    respuesta.Add(new CExpedienteFuncionarioDTO
                    {
                        NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente
                    });

                    // Detalle 05
                    var datoDetalle = intermedio.ObtenerDetalle
                     (((AccionPersonal)registro.Contenido).PK_AccionPersonal);

                    if (datoDetalle.Codigo > 0)
                    {              
                        CDetalleAccionPersonalDTO detalle = ConvertirDatosDetalleADto((DetalleAccionPersonal)datoDetalle.Contenido);

                        //CNombramientoDTO nombramiento = new CNombramientoDTO
                        //{
                        //    IdEntidad = detalle.CodNombramiento
                        //};

                        CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO
                        {
                            IdEntidad = detalle.DetallePuesto.IdEntidad
                        };
                        CDetallePuestoDTO detallePuestoAnterior = new CDetallePuestoDTO
                        {
                            IdEntidad = (detalle.CodDetallePuesto > 0) ? detalle.CodDetallePuesto : detalle.DetallePuesto.IdEntidad
                        };

                        detalle.DetallePuestoAnterior = CDetallePuestoL.ConstruirDetallePuesto(intermedioPuesto.CargarDetallePuesto(detallePuestoAnterior.IdEntidad));
                        
                        var detalleContratacion = ((AccionPersonal)registro.Contenido).Nombramiento.Funcionario.DetalleContratacion;
                        detalle.NumAnualidad = Convert.ToInt32(detalleContratacion.FirstOrDefault().NumAnualidades);
                        detalle.MtoAumentosAnuales = Convert.ToDecimal(detalle.MtoAnual * detalle.NumAnualidad);

                        var porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 3);
                        detalle.PorBonificacion = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 9);
                        detalle.PorConsulta = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 10);
                        detalle.PorCurso = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 13);
                        detalle.PorDisponibilidad = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 23);
                        detalle.PorQuinquenio = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 30);
                        detalle.PorRiesgo = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 35);
                        detalle.PorPeligrosidad = Convert.ToDecimal(porc.Contenido);

                        detalle.MtoProhibicion = (Convert.ToDecimal(detalle.MtoSalarioBase) * detalle.PorProhibicion) / 100;
                        detalle.MtoOtros = (detalle.MtoSalarioBase * (detalle.PorQuinquenio + detalle.PorCurso + detalle.PorDisponibilidad + detalle.PorRiesgo + detalle.PorPeligrosidad + detalle.PorConsulta + detalle.PorBonificacion)) / 100;
                        //detalle.MtoTotal = Convert.ToDecimal(detalle.MtoSalarioBase + detalle.MtoAumentosAnuales + detalle.MtoProhibicion +
                        //                                        detalle.MtoGradoGrupo + detalle.MtoOtros + detalle.MtoRecargo);

                        detalle.PorProhOriginal = (detalle.DetallePuestoAnterior.PorProhibicion > 0 ? detalle.DetallePuestoAnterior.PorProhibicion : detalle.DetallePuestoAnterior.PorDedicacion);
                        detalle.MtoProhibicionAnterior = (detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase * detalle.PorProhOriginal) / 100;

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 3);
                        detalle.PorBonificacionAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 9);
                        detalle.PorConsultaAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 10);
                        detalle.PorCursoAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 13);
                        detalle.PorDisponibilidadAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 23);
                        detalle.PorQuinquenioAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 30);
                        detalle.PorRiesgoAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 35);
                        detalle.PorPeligrosidadAnterior = Convert.ToDecimal(porc.Contenido);

                        detalle.MtoOtrosAnterior = (detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase * (detalle.PorQuinquenioAnterior + detalle.PorCursoAnterior + detalle.PorDisponibilidadAnterior + detalle.PorRiesgoAnterior + detalle.PorPeligrosidadAnterior + detalle.PorConsultaAnterior + detalle.PorBonificacionAnterior)) / 100;

                        respuesta.Add(detalle);
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

        public List<CBaseDTO> ObtenerAccion(string numAccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;
            var punto = "Inicio";
            var codNombramiento = 0;
            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                CEstadoBorradorD intermedioEstado = new CEstadoBorradorD(contexto);
                CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CDetallePuestoD intermedioDetallePuesto = new CDetallePuestoD(contexto);
                CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();
                CRegistroIncapacidadL intermedioIncapacidad = new CRegistroIncapacidadL();

                var registro = intermedio.ObtenerAccion(numAccion);

                if (registro.Codigo > 0)
                {
                    // Acción  00
                    punto = "00";
                    var datoRegistro = ConvertirDatosADto((AccionPersonal)registro.Contenido);
                    var datoDetalleAnterior = ((AccionPersonal)registro.Contenido).DetalleAccionPersonalAnterior.FirstOrDefault();

                    switch (datoRegistro.TipoAccion.IdEntidad)
                    {
                        case 5:  // Incapacidad
                        case 8:  // Prórroga de incapacidad
                            var datoIncapacidad = intermedioIncapacidad.ObtenerRegistroIncapacidad(datoRegistro.CodigoObjetoEntidad);
                            if (datoIncapacidad.Count() > 1)
                            {
                                var tipoIncapacidad = (CTipoIncapacidadDTO)datoIncapacidad.ElementAt(1);
                                var entidadMedica = (CEntidadMedicaDTO)datoIncapacidad.ElementAt(2);
                                datoRegistro.Observaciones = "Cód: " + datoRegistro.Observaciones + " " + entidadMedica.DescripcionEntidadMedica + " - " + tipoIncapacidad.DescripcionTipoIncapacidad;
                            }
                            break;


                        case 9:  // Prórroga de permiso con salario 
                        case 10: // permiso sin salario
                        case 11: // Prórroga de susp.Temporal
                        case 23: // Prórroga de nombramiento
                        case 48: // Prórroga de ascen. Interino. 
                        case 49: // Prórroga de descen.Interino.
                        case 50: // Prórroga permiso con sueldo.  
                        case 51: // Prórroga permuta interino.  //  
                        case 52: // Prórroga de traslado interino // 
                        case 54: // Reasignación. // 
                        case 80: // Regreso al puesto en propiedad
                            codNombramiento = datoRegistro.CodigoObjetoEntidad;
                            break;
                    }
                      
                    respuesta.Add(datoRegistro);


                    // Estado Borrador 01
                    punto = "01";
                    var datoEstado = intermedioEstado.CargarEstadoBorradorPorID
                        (((AccionPersonal)registro.Contenido).EstadoBorrador.PK_EstadoBorrador);

                    respuesta.Add(CEstadoBorradorL.ConvertirEstadoBorradorADto((EstadoBorrador)datoEstado.Contenido));


                    // Tipo Acción Personal 02
                    punto = "02";
                    var entidadTipo = intermedioTipo.CargarTipoAccionPersonalPorID
                        (((AccionPersonal)registro.Contenido).TipoAccionPersonal.PK_TipoAccionPersonal);

                    respuesta.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto((TipoAccionPersonal)entidadTipo.Contenido));


                    // Funcionario 03
                    punto = "03";
                    var funcionario = ((AccionPersonal)registro.Contenido).Nombramiento.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        IdEntidad = funcionario.PK_Funcionario,
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    // Expediente 04
                    punto = "04";
                    if (funcionario.ExpedienteFuncionario != null && funcionario.ExpedienteFuncionario.Count > 0)
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente
                        });
                    }
                    else
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            IdEntidad = -1,
                            NumeroExpediente = 0
                        });
                    }

                    punto = "Nomb";
                    var datoNombramiento = ((AccionPersonal)registro.Contenido).Nombramiento;

                    // Puesto 05
                    punto = "05";
                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);


                    // DetallePuesto 06
                    punto = "06";
                    DetallePuesto datoDetallePuesto;
                    //Buscar el detalle de puesto, según nombramiento. 
                    if (codNombramiento == 0)
                        codNombramiento = datoNombramiento.PK_Nombramiento;

                    var entidadDetallePuesto = intermedio.ObtenerDetallePuestoNombramiento(codNombramiento);
                    if (entidadDetallePuesto.Codigo > 0)
                        datoDetallePuesto = (DetallePuesto)entidadDetallePuesto.Contenido;
                    else
                        datoDetallePuesto = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoDetallePuesto.PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoDetallePuesto.PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoDetallePuesto.PorDedicacion);

                    punto = "Clase";
                    if (datoDetallePuesto.Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoDetallePuesto.Clase.PK_Clase,
                            DesClase = datoDetallePuesto.Clase.DesClase
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

                    punto = "Espec";
                    if (datoDetallePuesto.Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoDetallePuesto.Especialidad.PK_Especialidad,
                            DesEspecialidad = datoDetallePuesto.Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }

                    punto = "Subesp";
                    if (datoDetallePuesto.SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoDetallePuesto.SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoDetallePuesto.SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    punto = "Escala";
                    if (datoDetallePuesto.EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoDetallePuesto.EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoDetallePuesto.EscalaSalarial.IndCategoria),
                            SalarioBase = datoDetallePuesto.EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoDetallePuesto.EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoDetallePuesto.EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoDetallePuesto.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
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

                    punto = "Rubro";
                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetallePuesto.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * detallePuesto.EscalaSalarial.SalarioBase) / 100;

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

                    punto = "Ocup";
                    if (datoDetallePuesto.OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoDetallePuesto.OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoDetallePuesto.OcupacionReal.DesOcupacionReal
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

                    // Contrato 07
                    punto = "07";
                    if (funcionario.DetalleContratacion != null && funcionario.DetalleContratacion.Count > 0)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(funcionario.DetalleContratacion.First()));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO
                        {
                            NumeroAnualidades = 0,
                            FechaMesAumento = "1",
                            CodigoPolicial = 0
                        });
                    }


                    // Detalle 08
                    punto = "08";
                    var datoDetalle = intermedio.ObtenerDetalle(((AccionPersonal)registro.Contenido).PK_AccionPersonal);

                    if (datoDetalle.Codigo > 0)
                    {
                        decimal monSalarioBaseCalculo = 0;
                        decimal monSalarioBaseAnteriorCalculo = 0;

                        CDetalleAccionPersonalDTO detalle = ConvertirDatosDetalleADto((DetalleAccionPersonal)datoDetalle.Contenido);

                        switch(datoRegistro.TipoAccion.IdEntidad)
                        {
                            case 36: // Cambio de Categoría
                                var datoCategoria = intermedio.ObtenerCategoriaReasignacion(detalle.CodDetallePuesto);
                                if (datoCategoria.Codigo > 0 && Convert.ToInt32(datoCategoria.Contenido) > 0)
                                    detalle.IndCategoria = Convert.ToInt32(datoCategoria.Contenido);
                                break;
                        }
   
                        //CNombramientoDTO nombramiento = new CNombramientoDTO
                        //{
                        //    IdEntidad = detalle.CodNombramiento
                        //};
                        
                        detallePuesto = new CDetallePuestoDTO
                        {
                            IdEntidad = detalle.DetallePuesto.IdEntidad
                        };


                        // Puesto Anterior
                        CDetallePuestoDTO detallePuestoAnterior = new CDetallePuestoDTO();
                        if (datoDetalleAnterior != null && datoDetalleAnterior.CodDetallePuestoAnt > 0)
                        {
                            detallePuestoAnterior = new CDetallePuestoDTO
                            {
                                IdEntidad = datoDetalleAnterior.CodDetallePuestoAnt
                            };
                        }
                        else
                        {
                            detallePuestoAnterior = new CDetallePuestoDTO
                            {
                                IdEntidad = (detalle.CodDetallePuesto > 0) ? detalle.CodDetallePuesto : detalle.DetallePuesto.IdEntidad
                            };
                        }

                        var entrada = intermedio.CargarDetallePuesto(detallePuestoAnterior.IdEntidad);
                        CDetallePuestoDTO salida = new CDetallePuestoDTO();
                        salida.IdEntidad = entrada.PK_DetallePuesto;
                        salida.Clase = CClaseL.ConstruirClase(entrada.Clase);
                        if (entrada.Especialidad != null)
                            salida.Especialidad = CEspecialidadL.ConstruirEspecialidad(entrada.Especialidad);
                        else
                            salida.Especialidad = new CEspecialidadDTO();

                        salida.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = entrada.EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(entrada.EscalaSalarial.IndCategoria),
                            SalarioBase = Convert.ToDecimal(entrada.EscalaSalarial.MtoSalarioBase),
                            MontoAumentoAnual = Convert.ToDecimal(entrada.EscalaSalarial.MtoAumento)
                        };
                        salida.PorProhibicion = (entrada.PorProhibicion != null) ? Convert.ToDecimal(entrada.PorProhibicion) : 0;
                        salida.PorDedicacion = (entrada.PorDedicacion != null) ? Convert.ToDecimal(entrada.PorDedicacion) : 0;
                        if (entrada.OcupacionReal != null)
                        {
                            salida.OcupacionReal = COcupacionRealL.ConstruirOcupacionReal(entrada.OcupacionReal);
                        }
                        else
                        {
                            salida.OcupacionReal = new COcupacionRealDTO();
                        }
                        if (entrada.SubEspecialidad != null)
                        {
                            salida.SubEspecialidad = CSubEspecialidadL.ConvertirSubEspecialidadADTO(entrada.SubEspecialidad);
                        }
                        else
                        {
                            salida.SubEspecialidad = new CSubEspecialidadDTO();
                        }

                        salida.Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(intermedioPuesto.DescargarPuestoCodigo(entrada.Puesto.CodPuesto));
                        detalle.DetallePuestoAnterior = salida;

                        var detalleContratacion = ((AccionPersonal)registro.Contenido).Nombramiento.Funcionario.DetalleContratacion;
                        //detalle.NumAnualidad = Convert.ToInt32(detalleContratacion.FirstOrDefault().NumAnualidades);
                        detalle.MtoAumentosAnuales = Convert.ToDecimal(detalle.MtoAnual * detalle.NumAnualidad);

                        var porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 3);
                        detalle.PorBonificacion = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 4);
                        detalle.PorCarreraPolicial = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 9);
                        detalle.PorConsulta = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 10);
                        detalle.PorCurso = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 12);
                        detalle.PorDesarraigo = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 13);
                        detalle.PorDisponibilidad = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 16);
                        detalle.PorGradoPolicial = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 23);
                        detalle.PorQuinquenio = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 30);
                        detalle.PorRiesgo = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuesto, 35);
                        detalle.PorPeligrosidad = Convert.ToDecimal(porc.Contenido);

                        detalle.MtoProhibicion = (Convert.ToDecimal(detalle.MtoSalarioBase) * detalle.PorProhibicion) / 100;

                        /////////
                        monSalarioBaseCalculo = detalle.MtoSalarioBase;
                        // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                        if (funcionario.DetalleContratacion != null)
                        {
                            if (funcionario.DetalleContratacion.FirstOrDefault().CodPolicial > 0)
                            {
                                var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detalle.IndCategoria, 1);
                                if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                                }
                            }
                        }

                        detalle.MtoOtros = (monSalarioBaseCalculo * (detalle.PorQuinquenio + detalle.PorCurso + detalle.PorDisponibilidad + detalle.PorRiesgo + detalle.PorPeligrosidad + detalle.PorConsulta + detalle.PorBonificacion + detalle.PorGradoPolicial + detalle.PorCarreraPolicial)) / 100;

                        // El Desarraigo se calcula con otra Escala Salarial
                        if (detalle.PorDesarraigo > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detalle.IndCategoria, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            detalle.MtoOtros += (monSalarioBaseCalculo * detalle.PorDesarraigo) / 100;
                        }
                        
                        //detalle.MtoTotal = Convert.ToDecimal(detalle.MtoSalarioBase + detalle.MtoAumentosAnuales + detalle.MtoProhibicion +
                        //                                        detalle.MtoGradoGrupo + detalle.MtoOtros + detalle.MtoRecargo);

                        detalle.PorProhOriginal = (detalle.DetallePuestoAnterior.PorProhibicion > 0 ? detalle.DetallePuestoAnterior.PorProhibicion : detalle.DetallePuestoAnterior.PorDedicacion);
                        detalle.MtoProhibicionAnterior = (detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase * detalle.PorProhOriginal) / 100;

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 3);
                        detalle.PorBonificacionAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 4);
                        detalle.PorCarreraPolicialAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 9);
                        detalle.PorConsultaAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 10);
                        detalle.PorCursoAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 12);
                        detalle.PorDesarraigoAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 13);
                        detalle.PorDisponibilidadAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 16);
                        detalle.PorGradoPolicialAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 23);
                        detalle.PorQuinquenioAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 30);
                        detalle.PorRiesgoAnterior = Convert.ToDecimal(porc.Contenido);

                        porc = (CRespuestaDTO)ObtenerPorcentajeComponenteSalarial(detallePuestoAnterior, 35);
                        detalle.PorPeligrosidadAnterior = Convert.ToDecimal(porc.Contenido);

                        /////////
                        monSalarioBaseAnteriorCalculo = detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase;
                        // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                        if (funcionario.DetalleContratacion != null)
                        {
                            if (funcionario.DetalleContratacion.FirstOrDefault().CodPolicial > 0)
                            {
                                var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detalle.DetallePuestoAnterior.EscalaSalarial.CategoriaEscala, 1);
                                if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    monSalarioBaseAnteriorCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                                }
                            }
                        }
                        detalle.MtoOtrosAnterior = (monSalarioBaseAnteriorCalculo * (detalle.PorQuinquenioAnterior + detalle.PorCursoAnterior + detalle.PorDisponibilidadAnterior + detalle.PorRiesgoAnterior + detalle.PorPeligrosidadAnterior + detalle.PorConsultaAnterior + detalle.PorBonificacionAnterior + detalle.PorGradoPolicialAnterior + detalle.PorCarreraPolicialAnterior)) / 100;

                        // El Desarraigo se calcula con otra Escala Salarial
                        if (detalle.PorDesarraigoAnterior > 0)
                        {
                            var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detalle.IndCategoria, 1);
                            if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                            detalle.MtoOtrosAnterior += (monSalarioBaseCalculo * detalle.PorDesarraigoAnterior) / 100;
                        }

                        respuesta.Add(detalle);
                    }
                }
                else
                {
                    respuesta.Add( new CErrorDTO { Codigo = -1, Mensaje = registro.Contenido.ToString()  + punto });
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = (error.InnerException != null ? error.InnerException.Message : "") + punto
                });
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerAccionProrroga(int idTipo, string numCedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var registro = intermedio.ObtenerAccionProrroga(idTipo, numCedula);

                if (registro.Codigo > 0)
                {        
                    respuesta.Add((CAccionPersonalDTO)registro.Contenido);
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

        public CBaseDTO AnularAccion(CAccionPersonalDTO accion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                AccionPersonal accionBD = new AccionPersonal
                {
                    PK_AccionPersonal = accion.IdEntidad,
                    Observaciones = accion.Observaciones
                };

                respuesta = intermedio.AnularAccion(accionBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta = new CBaseDTO { IdEntidad = accion.IdEntidad }; ;
                }

                //var datosAccion = intermedio.AnularAccion(accionBD);

                //if (datosAccion.Codigo > 0)
                //{
                //    respuesta = new CBaseDTO { IdEntidad = accion.IdEntidad };
                //}
                //else
                //{
                //    respuesta = ((CErrorDTO)datosAccion.Contenido);
                //}
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

        public CBaseDTO AnularAccionModulo(CAccionPersonalDTO accion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                AccionPersonal accionBD = new AccionPersonal
                {
                    CodModulo = accion.CodigoModulo,
                    CodObjetoEntidad = accion.CodigoObjetoEntidad,
                    Observaciones = accion.Observaciones,
                    TipoAccionPersonal = new TipoAccionPersonal
                    {
                        PK_TipoAccionPersonal = accion.TipoAccion.IdEntidad
                    }
                };

                var datosAccion = intermedio.AnularAccionModulo(accionBD);

                if (datosAccion.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = accion.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosAccion.Contenido);
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

        public CBaseDTO ModificarObservaciones(CAccionPersonalDTO accion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                AccionPersonal accionBD = new AccionPersonal
                {
                    NumAccion = accion.NumAccion,
                    Observaciones = accion.Observaciones
                };

                respuesta = intermedio.ModificarObservaciones(accionBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta = new CBaseDTO { Mensaje = (((CRespuestaDTO)respuesta).Contenido).ToString() }; ;
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
        public CBaseDTO AprobarAccion(CAccionPersonalDTO accion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                AccionPersonal accionBD = new AccionPersonal
                {
                    PK_AccionPersonal = accion.IdEntidad,
                    Observaciones = accion.Observaciones
                };

                respuesta = intermedio.AprobarAccion(accionBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta = new CBaseDTO { IdEntidad = accion.IdEntidad }; ;
                }

                //var datosAccion = intermedio.AnularAccion(accionBD);

                //if (datosAccion.Codigo > 0)
                //{
                //    respuesta = new CBaseDTO { IdEntidad = accion.IdEntidad };
                //}
                //else
                //{
                //    respuesta = ((CErrorDTO)datosAccion.Contenido);
                //}
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

        public List<List<CBaseDTO>> BuscarAccion(CFuncionarioDTO funcionario, CPuestoDTO puesto, CAccionPersonalDTO accion, List<DateTime> fechas)
        {
            CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

            List<AccionPersonal> datos = new List<AccionPersonal>();
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            if (accion != null || funcionario != null || puesto != null|| fechas.Count() > 0 )
            {
                var resultado = intermedio.BuscarAccion(funcionario, puesto, accion, fechas);

                if (resultado.Codigo > 0)
                {
                    datos = (List<AccionPersonal>)resultado.Contenido;
                }
            }

            if (datos.Count > 0)
            {
                foreach (var item in datos)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    // Acción  00
                    temp.Add(ConvertirDatosADto(item));

                    // Estado Borrador 01
                    temp.Add(CEstadoBorradorL.ConvertirEstadoBorradorADto(item.EstadoBorrador));

                    // Tipo Acción Personal 02
                    temp.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto(item.TipoAccionPersonal));
                                       
                    // Funcionario 03
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));

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

        public List<List<CBaseDTO>> BuscarAccionProrroga(string tipoAccion, List<DateTime> fechas)
        {
            CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
            CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);

            List<AccionPersonal> datos = new List<AccionPersonal>();
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            var resultado = intermedio.BuscarAccionProrroga(tipoAccion, fechas);

            if (resultado.Codigo > 0)
            {
                datos = (List<AccionPersonal>)resultado.Contenido;
            }

            if (datos.Count > 0)
            {
                CFuncionarioDTO datoFuncionario = new CFuncionarioDTO();
                CDetallePuestoDTO datoDetallePuesto =new CDetallePuestoDTO();
                CNombramientoDTO datoNombramiento = new CNombramientoDTO();
                foreach (var item in datos)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    if(item.Nombramiento.PK_Nombramiento == 0)
                    {
                        var dato = BuscarFuncionarioDetallePuesto(item.Nombramiento.Funcionario.IdCedulaFuncionario);
                        if (dato.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            datoFuncionario = (CFuncionarioDTO)dato[0];
                            datoDetallePuesto = (CDetallePuestoDTO)dato[2];
                            datoNombramiento = (CNombramientoDTO)dato[4];
                            item.IndDato = datoDetallePuesto.PorDedicacion;
                            item.Nombramiento = CNombramientoL.ConvertirDatosNombramientoADATOS(datoNombramiento);
                        }
                    }
                    else
                    {
                        datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario);
                    }

                    try
                    {
                        // Acción  00
                        item.Nombramiento = intermedioNombramiento.CargarNombramiento(item.DetalleAccionPersonalAnterior.First().CodNombramiento);
                        temp.Add(ConvertirDatosADto(item));

                        // Estado Borrador 01
                        temp.Add(CEstadoBorradorL.ConvertirEstadoBorradorADto(item.EstadoBorrador));

                        // Tipo Acción Personal 02
                        temp.Add(CTipoAccionPersonalL.ConvertirTipoAccionPersonalADto(item.TipoAccionPersonal));

                        // Funcionario 03
                        temp.Add(datoFuncionario);

                        // Detalle Acción  04
                        var detalle = new CDetalleAccionPersonalDTO
                        {
                            DetallePuesto = new CDetallePuestoDTO(),
                            DetallePuestoAnterior = new CDetallePuestoDTO(),
                        };
                        // Puesto Nuevo
                        var entidadDetallePuesto = intermedio.ObtenerDetallePuestoNombramiento(item.DetalleAccionPersonalAnterior.First().CodNombramiento);
                        detalle.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)entidadDetallePuesto.Contenido);
                        detalle.DetallePuesto.Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(((DetallePuesto)entidadDetallePuesto.Contenido).Puesto);

                        // Puesto Anterior
                        entidadDetallePuesto = intermedio.ObtenerDetallePuestoNombramiento(item.DetalleAccionPersonalAnterior.First().CodNombramientoAnt);
                        detalle.DetallePuestoAnterior = CDetallePuestoL.ConstruirDetallePuesto((DetallePuesto)entidadDetallePuesto.Contenido);
                        detalle.DetallePuestoAnterior.Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(((DetallePuesto)entidadDetallePuesto.Contenido).Puesto);

                        temp.Add(detalle);

                        respuesta.Add(temp);
                    }
                   catch
                    {

                    }
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

        public List<List<CBaseDTO>> BuscarAccionDesarraigo(bool registrarAprobacion)
        {
            CDesarraigoD intermedio = new CDesarraigoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();

            CAccionPersonalDTO accion = new CAccionPersonalDTO();
            CDetalleAccionPersonalDTO detalleAccion = new CDetalleAccionPersonalDTO();
            List<Desarraigo> datosDesarraigo = new List<Desarraigo>();
            List<Desarraigo> datosDesarraigoVencidos = new List<Desarraigo>();
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CDesarraigoDTO desarraigo = new CDesarraigoDTO();

            if (registrarAprobacion)
                desarraigo.EstadoDesarraigo = new CEstadoDesarraigoDTO { IdEntidad = 4 }; // Aprobado
            else
                desarraigo.EstadoDesarraigo = new CEstadoDesarraigoDTO { IdEntidad = 7 }; // Suspendido

            var resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigo, desarraigo.EstadoDesarraigo.IdEntidad, "Estado"));
            if (resultado.Codigo > 0)
                datosDesarraigo = (List<Desarraigo>)resultado.Contenido;
            else
            {
                datosDesarraigo.Clear();
            }


            if (registrarAprobacion)
                datosDesarraigo = datosDesarraigo.Where(Q => Q.NumAccionAprobado == "").ToList();
            else
            {
                datosDesarraigo = datosDesarraigo.Where(Q => Q.NumAccionEliminado == "").ToList();

                // Buscar también los Contratos que ya se vencieron
                desarraigo.EstadoDesarraigo.IdEntidad = 4; // Aprobado
                resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigoVencidos, desarraigo.EstadoDesarraigo.IdEntidad, "Estado"));
                if (resultado.Codigo > 0)
                {
                    datosDesarraigoVencidos = (List<Desarraigo>)resultado.Contenido;
                    datosDesarraigoVencidos = datosDesarraigoVencidos.Where(Q => Q.NumAccionAprobado != ""
                                                                            && Q.NumAccionEliminado == ""
                                                                            && Q.FecFinDesarraigo < DateTime.Today).ToList();
                    datosDesarraigo.AddRange(datosDesarraigoVencidos);
                }
                else
                {
                    datosDesarraigoVencidos.Clear();
                }
            }



            if (datosDesarraigo.Count > 0)
            {
                foreach (var item in datosDesarraigo)
                {
                    var datoPuesto = intermedioPuesto.DetallePuestoPorCodigo(item.Nombramiento.Puesto.CodPuesto);

                    List<CBaseDTO> temp = new List<CBaseDTO>();


                    accion = new CAccionPersonalDTO
                    {
                        AnioRige = item.FecInicDesarraigo.Year,
                        FecRige = registrarAprobacion ?
                                    item.FecInicDesarraigo :
                                    item.DetalleDesarraigoEliminacion.FirstOrDefault().FecEliminacion,
                        FecRigeIntegra = registrarAprobacion ?
                                            item.FecInicDesarraigo :
                                            item.DetalleDesarraigoEliminacion.FirstOrDefault().FecEliminacion,
                        Nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                        TipoAccion = new CTipoAccionPersonalDTO { IdEntidad = registrarAprobacion ? 64 : 65 },
                        NumAccion = item.NumAccionAprobado,
                        Estado = new CEstadoBorradorDTO { IdEntidad = 7 }, // Aprobado
                        IndDato = 0,
                        CodigoModulo = 5,
                        CodigoObjetoEntidad = item.PK_Desarraigo
                    };

                    var puesto = ((CPuestoDTO)datoPuesto.ElementAt(0));
                    var detallePuesto = ((CDetallePuestoDTO)datoPuesto.ElementAt(1));

                    detalleAccion = new CDetalleAccionPersonalDTO
                    {
                        CodClase = detallePuesto.Clase.IdEntidad,
                        CodDetallePuesto = detallePuesto.IdEntidad,
                        CodNombramiento = item.Nombramiento.PK_Nombramiento,
                        CodEspecialidad = detallePuesto.Especialidad.IdEntidad,
                        CodSubespecialidad = detallePuesto.SubEspecialidad.IdEntidad,
                        DetallePuesto = detallePuesto
                    };

                    if (registrarAprobacion)
                    {
                        accion.FecVence = item.FecFinDesarraigo;
                        accion.FecVenceIntegra = item.FecFinDesarraigo;
                        accion.Observaciones = "REAJUSTE DE SOBRESUELDOS PARA RECONOCERLE EL DESARRAIGO";// SEGÚN CONTRATO N. " + item.NumContrato;
                        detalleAccion.PorDesarraigoAnterior = 0;
                        detalleAccion.PorDesarraigo = 40;
                        detalleAccion.MtoOtros = 40;
                        accion.IndDato = 40;
                    }
                    else
                    {
                        accion.Observaciones = "REAJUSTE DE SOBRESUELDOS PARA ELIMINAR EL DESARRAIGO. " + item.DetalleDesarraigoEliminacion.FirstOrDefault().ObsEliminacion.TrimEnd();   // SEGÚN CONTRATO N. " + item.NumContrato;
                        detalleAccion.PorDesarraigoAnterior = 40;
                        detalleAccion.PorDesarraigo = 0;
                        detalleAccion.MtoOtros = 0;
                        accion.IndDato = 0;
                    }

                    // Acción  00
                    temp.Add(accion);

                    // Acción  01
                    temp.Add(detalleAccion);

                    // Funcionario 02
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));

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

        public List<CBaseDTO> BuscarHistorial(CAccionPersonalHistoricoDTO accion, List<DateTime> fechas)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<C_EMU_AccionPersonal> datos = new List<C_EMU_AccionPersonal>();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var registro = intermedio.BuscarHistorial(accion, fechas);

                if (registro.Codigo > 0)
                {
                    datos = (List<C_EMU_AccionPersonal>)registro.Contenido;
                }

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        CAccionPersonalHistoricoDTO tempH = ConvertirDatosHistoricoADto((C_EMU_AccionPersonal)item);
                        respuesta.Add(tempH);
                    }
                }
                else
                {
                    respuesta.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontraron resultados para los parámetros especificados."
                    });
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

        public List<CBaseDTO> ObtenerAccionHistorico(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var registro = intermedio.ObtenerAccionHistorico(codigo);

                if (registro.Codigo > 0)
                {
                    // Acción  00
                    var datoRegistro = ConvertirDatosHistoricoADto((C_EMU_AccionPersonal)registro.Contenido);
                    respuesta.Add(datoRegistro);
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
        
        public CRespuestaDTO ObtenerPorcentajeComponenteSalarial(CDetallePuestoDTO detalle, int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var datos = intermedio.ObtenerPorcentajeComponenteSalarial(detalle, codigo);
                if (datos.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Convert.ToDecimal(datos.Contenido)
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = 0
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = 0
                };
            }
            return respuesta;
        }
        
        public CRespuestaDTO ObtenerPorcentajeComponenteSalarial(CFuncionarioDTO funcionario, int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var datos = intermedio.ObtenerPorcentajeComponenteSalarial(funcionario, codigo);
                if (datos.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Convert.ToDecimal(datos.Contenido)
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = 0
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = 0
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ObtenerCategoriaClase(int codClase)
        {
            CRespuestaDTO respuesta;

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var datos = intermedio.ObtenerCategoriaClase(codClase);
                if (datos.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Convert.ToDecimal(datos.Contenido)
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = 0
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = 0
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ObtenerSalario(string numCedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var datos = intermedio.ObtenerSalario(numCedula);
                if (datos.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos.Contenido
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = 0
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = 0
                };
            }
            return respuesta;
        }
             
        public CBaseDTO CargarAccionHistorico()
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var dato = intermedio.CargarAccionHistorico();
               
                if (dato.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato.Contenido
                    };
                }
                else
                {
                    respuesta = respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = 0
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.InnerException.Message
                };
            }

            return respuesta;
        }

        // Es el mismo método de la clase CFuncionarioL, con la diferencia que se trae el último nombramiento de la persona
        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CAccionPersonalD intermedioAP = new CAccionPersonalD(contexto);
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();
            var listaNombramientos = new List<int>();
            try
            {
                // [0] CFuncionarioDTO
                var datoFuncionario = intermedio.BuscarFuncionarioCedula(cedula);
                if (datoFuncionario != null)
                {
                    CFuncionarioDTO funcionario = CFuncionarioL.FuncionarioGeneral(datoFuncionario);
                    respuesta.Add(funcionario);
                }
                else
                {
                    throw new Exception("No existe funcionario con ese número de cédula");
                }

                var datosDB = intermedioAP.BuscarFuncionarioDetallePuesto(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    Nombramiento datoNombramiento;
                    if (dato.Nombramiento != null && dato.Nombramiento.Count > 0)
                    {
                        listaNombramientos = dato.Nombramiento.Select(N => N.PK_Nombramiento).ToList();
                        datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();

                        if (datoNombramiento == null)
                        {
                            datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                            if (datoNombramiento == null)
                            {
                                throw new Exception("El funcionario no tiene un Nombramiento");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("El funcionario no tiene un Nombramiento");
                    }
                    //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();

                    var datoDetalleContrato = datoFuncionario.DetalleContratacion.FirstOrDefault();


                    // [1] CPuestoDTO
                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);

                    //DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.FK_Nombramiento != null && listaNombramientos.Contains(DP.FK_Nombramiento.Value)).OrderBy(DP => DP.IndEstadoDetallePuesto).FirstOrDefault();
                    if (datoDetalle == null)
                        datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoDetalle.PK_DetallePuesto; // datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoDetalle.PorProhibicion); // Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoDetalle.PorDedicacion); //Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                    if (datoDetalle.Clase != null) //if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoDetalle.Clase.PK_Clase, // datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                            DesClase = datoDetalle.Clase.DesClase //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
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


                    if (datoDetalle.Especialidad != null)//if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.Especialidad.PK_Especialidad, //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                            DesEspecialidad = datoDetalle.Especialidad.DesEspecialidad //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoDetalle.SubEspecialidad != null)//(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.SubEspecialidad.PK_SubEspecialidad,//datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoDetalle.SubEspecialidad.DesSubEspecialidad //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoDetalle.EscalaSalarial != null)//(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoDetalle.EscalaSalarial.PK_Escala, //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoDetalle.EscalaSalarial.IndCategoria), //Convert.ToInt32(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                            SalarioBase = datoDetalle.EscalaSalarial.MtoSalarioBase.Value, // datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoDetalle.EscalaSalarial.MtoAumento),//Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),
                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,// datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera) //Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
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
                        {
                            // El Desarraigo se calcula con otra Escala Salarial
                            if (item.FK_ComponenteSalarial == 12)
                            {
                                var monSalarioBaseDesarraigo = monSalarioBaseCalculo;
                                var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(detallePuesto.EscalaSalarial.CategoriaEscala, 1);
                                if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    monSalarioBaseDesarraigo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                                monto = (item.PorValor * monSalarioBaseDesarraigo) / 100;
                            }
                            else
                                monto = (item.PorValor * monSalarioBaseCalculo) / 100;
                        }

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

                    if (datoDetalle.OcupacionReal != null)//(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = datoDetalle.OcupacionReal.PK_OcupacionReal, //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                            DesOcupacionReal = datoDetalle.OcupacionReal.DesOcupacionReal //datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    // [2] CDetallePuestoDTO
                    respuesta.Add(detallePuesto);

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();

                    // [3] CDetalleContratacionDTO
                    if (datoDetalleContrato != null)
                    {
                        respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                    }
                    else
                    {
                        respuesta.Add(new CDetalleContratacionDTO
                        {
                            NumeroAnualidades = 0,
                            FechaMesAumento = "1",
                            CodigoPolicial = 0
                        });
                    }

                    // [4] CNombramientoDTO
                    respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(datoNombramiento));

                    // [5] CDetalleAccionPersonalDTO
                    respuesta.Add(new CDetalleAccionPersonalDTO
                    {
                        MtoSalarioBase = monSalarioBaseCalculo
                    });
                }
                else
                {
                    throw new Exception("No existe funcionario con ese número de cédula");
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


        // Es el mismo método de la clase CFuncionarioL, con la diferencia que se trae el último nombramiento con distinto puesto
        public List<CBaseDTO> BuscarFuncionarioDetallePuestoAnterior(string cedula, int codPuestoActual, int codNombramiento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CAccionPersonalD intermedioAP = new CAccionPersonalD(contexto);

            try
            {
                var datosDB = intermedio.BuscarFuncionarioDetallePuesto(cedula);
                if (datosDB.Codigo != -1)
                {
                    Funcionario dato = (Funcionario)datosDB.Contenido;
                    Nombramiento datoNombramiento;
                    if (dato.Nombramiento != null && dato.Nombramiento.Count > 0)
                    {
                        if (codNombramiento > 0)
                            datoNombramiento = dato.Nombramiento
                                                .Where(Q => Q.PK_Nombramiento == codNombramiento)
                                                .FirstOrDefault();
                        else
                            datoNombramiento = dato.Nombramiento
                                                .Where(Q => Q.FK_Puesto != codPuestoActual && Q.FK_Funcionario == dato.PK_Funcionario)
                                                .OrderByDescending(N => N.FecVence ?? DateTime.MaxValue)
                                                .ThenByDescending(x => x.FecRige)
                                                .FirstOrDefault();

                        if (datoNombramiento == null)
                            throw new Exception("El funcionario no tiene un Nombramiento");
                    }
                    else
                    {
                        throw new Exception("El funcionario no tiene un Nombramiento");
                    }
                    //Nombramiento datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderBy(N => N.FecRige).LastOrDefault();
                    //Nombramiento datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();

                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                    respuesta.Add(puesto);



                    DetallePuesto datoDetalle;
                    //Buscar el detalle de puesto, según nombramiento. 
                    var entidadDetallePuesto = intermedioAP.ObtenerDetallePuestoNombramiento(datoNombramiento.PK_Nombramiento);
                    if (entidadDetallePuesto.Codigo > 0)
                        datoDetalle = (DetallePuesto)entidadDetallePuesto.Contenido;
                    else
                        datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                                        
                   // DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoDetalle.PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoDetalle.PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoDetalle.PorDedicacion);

                    if (datoDetalle.Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoDetalle.Clase.PK_Clase,
                            DesClase = datoDetalle.Clase.DesClase
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


                    if (datoDetalle.Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.Especialidad.PK_Especialidad,
                            DesEspecialidad = datoDetalle.Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoDetalle.SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoDetalle.SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoDetalle.EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoDetalle.EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoDetalle.EscalaSalarial.IndCategoria),
                            SalarioBase = datoDetalle.EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoDetalle.EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
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


                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * detallePuesto.EscalaSalarial.SalarioBase) / 100;

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

                    if (detallePuesto.OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = detallePuesto.OcupacionReal.IdEntidad,
                            DesOcupacionReal = detallePuesto.OcupacionReal.DesOcupacionReal
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

                    // Con esta instrucción se omiten los Exempleados
                    //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                    var datoFuncionario = intermedio.BuscarFuncionarioCedula(cedula);
                    var datoDetalleContrato = datoFuncionario.DetalleContratacion.FirstOrDefault();

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

                    respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(datoNombramiento));
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

        public List<CBaseDTO> BuscarAccionDetalleAnterior(int codAccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;

            CFuncionarioD intermedio = new CFuncionarioD(contexto);
            CAccionPersonalD intermedioAP = new CAccionPersonalD(contexto);
            CNombramientoD intermedioN = new CNombramientoD(contexto);

            try
            {
                // Buscar si la Acción tiene DetalleAccionPersonalAnterior
                var datoAccionDetalle = intermedioAP.ObtenerDetalleAnterior(codAccion);
                if (datoAccionDetalle.Codigo > 0)
                {
                    var dato = (DetalleAccionPersonalAnterior)datoAccionDetalle.Contenido;
                    // Buscar si la Acción tiene DetalleAccionPersonalAnterior
                    var datoNombramiento = intermedioN.CargarNombramiento(dato.CodNombramientoAnt);
                    CPuestoDTO puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);

                    //[0] CPuestoDTO Anterior
                    respuesta.Add(puesto);

                    DetallePuesto datoDetalle;
                    //Buscar el detalle de puesto.
                    var entidadDetallePuesto = intermedioAP.ObtenerDetallePuestoCodigo(dato.CodDetallePuestoAnt);
                    if (entidadDetallePuesto.Codigo > 0)
                        datoDetalle = (DetallePuesto)entidadDetallePuesto.Contenido;
                    else
                        datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                    CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoDetalle.PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoDetalle.PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoDetalle.PorDedicacion);

                    if (datoDetalle.Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoDetalle.Clase.PK_Clase,
                            DesClase = datoDetalle.Clase.DesClase
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


                    if (datoDetalle.Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.Especialidad.PK_Especialidad,
                            DesEspecialidad = datoDetalle.Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoDetalle.SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoDetalle.SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoDetalle.EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoDetalle.EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoDetalle.EscalaSalarial.IndCategoria),
                            SalarioBase = datoDetalle.EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoDetalle.EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
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


                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * detallePuesto.EscalaSalarial.SalarioBase) / 100;

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

                    if (detallePuesto.OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = detallePuesto.OcupacionReal.IdEntidad,
                            DesOcupacionReal = detallePuesto.OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    //[01] CDetallePuestoDTO Anterior
                    respuesta.Add(detallePuesto);


                    /// PUESTO ACTUAL
                    datoNombramiento = intermedioN.CargarNombramiento(dato.CodNombramiento);
                    puesto = new CPuestoDTO();
                    puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);

                    //[02] CPuestoDTO Actual
                    respuesta.Add(puesto);

                    //Buscar el detalle de puesto.
                    entidadDetallePuesto = intermedioAP.ObtenerDetallePuestoCodigo(dato.CodDetallePuesto);
                    if (entidadDetallePuesto.Codigo > 0)
                        datoDetalle = (DetallePuesto)entidadDetallePuesto.Contenido;
                    else
                        datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                    detallePuesto = new CDetallePuestoDTO();

                    detallePuesto.IdEntidad = datoDetalle.PK_DetallePuesto;
                    detallePuesto.PorProhibicion = Convert.ToDecimal(datoDetalle.PorProhibicion);
                    detallePuesto.PorDedicacion = Convert.ToDecimal(datoDetalle.PorDedicacion);

                    if (datoDetalle.Clase != null)
                    {
                        detallePuesto.Clase = new CClaseDTO
                        {
                            IdEntidad = datoDetalle.Clase.PK_Clase,
                            DesClase = datoDetalle.Clase.DesClase
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


                    if (datoDetalle.Especialidad != null)
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.Especialidad.PK_Especialidad,
                            DesEspecialidad = datoDetalle.Especialidad.DesEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                    }


                    if (datoDetalle.SubEspecialidad != null)
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                        {
                            IdEntidad = datoDetalle.SubEspecialidad.PK_SubEspecialidad,
                            DesSubEspecialidad = datoDetalle.SubEspecialidad.DesSubEspecialidad
                        };
                    }
                    else
                    {
                        detallePuesto.SubEspecialidad = new CSubEspecialidadDTO { IdEntidad = -1 };
                    }

                    if (datoDetalle.EscalaSalarial != null)
                    {
                        detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                        {
                            IdEntidad = datoDetalle.EscalaSalarial.PK_Escala,
                            CategoriaEscala = Convert.ToInt32(datoDetalle.EscalaSalarial.IndCategoria),
                            SalarioBase = datoDetalle.EscalaSalarial.MtoSalarioBase.Value,
                            MontoAumentoAnual = Convert.ToDecimal(datoDetalle.EscalaSalarial.MtoAumento),

                            Periodo = new CPeriodoEscalaSalarialDTO
                            {
                                IdEntidad = datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                                MontoPuntoCarrera = Convert.ToDecimal(datoDetalle.EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
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


                    detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                    foreach (var item in datoDetalle.DetallePuestoRubro)
                    {
                        if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                            monto = item.PorValor;
                        else   // Tipo Porcentual
                            monto = (item.PorValor * detallePuesto.EscalaSalarial.SalarioBase) / 100;

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

                    if (detallePuesto.OcupacionReal != null)
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            IdEntidad = detallePuesto.OcupacionReal.IdEntidad,
                            DesOcupacionReal = detallePuesto.OcupacionReal.DesOcupacionReal
                        };
                    }
                    else
                    {
                        detallePuesto.OcupacionReal = new COcupacionRealDTO
                        {
                            DesOcupacionReal = "NO TIENE"
                        };
                    }

                    //[04] CDetallePuestoDTO Actual
                    respuesta.Add(detallePuesto);

                    //[05] CNombramientoDTO
                    respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(datoNombramiento));
                }
                else
                {
                   throw new Exception(datoAccionDetalle.Contenido.ToString());
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

        #endregion
    }
}
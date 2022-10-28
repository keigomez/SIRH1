using System.Collections.Generic;
using SIRH.DTO;
using SIRH.Logica;
using System;

namespace SIRH.Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CCalificacion" in both code and config file together.
    public class CCalificacionService : ICCalificacionService
    {
        //-----------------------------------------------------------CalificacionDTO------------------------------------------------------//
        /*public CBaseDTO GuardarCalificacion(CCalificacionDTO calificacion)
        {
            CCalificacionL respuesta = new CCalificacionL();

            return respuesta.GuardarCalificacion(calificacion);
        }*/
        public CBaseDTO GuardarCalificacion(string cedula,CCalificacionDTO item)
        {
            CCalificacionL respuesta = new CCalificacionL();
            return respuesta.GuardarCalificacion(cedula,item);
        }

        public List<CBaseDTO> CargarCalificaciones()
        {
            CCalificacionL respuesta = new CCalificacionL();
            return respuesta.CargarCalificaciones();
        }

        public List<CBaseDTO> DescargarCalificacionesCedula(string cedula)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarCalificacionesCedula(cedula);
        }

        public List<CBaseDTO> DescargarCalificacionCedula(string cedula, int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarCalificacionCedula(cedula, periodo);
        }

        public List<CBaseDTO> GuardarCalificacionFuncionario(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GuardarCalificacionFuncionario(CalificacionCN, DetalleDCN);
        }

        public List<CBaseDTO> GuardarCalificacionModificada(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GuardarCalificacionModificada(CalificacionCN, DetalleDCN);
        }

        public List<CBaseDTO> ObtenerCalificacion(int codigo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerCalificacion(codigo);
        }

        public List<CBaseDTO> ObtenerCalificacionHistorico(int codigo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerCalificacionHistorico(codigo);
        }

        public List<CBaseDTO> DescargarFuncionarioCalificarDetallePuesto(string cedula, int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarFuncionarioCalificarDetallePuesto(cedula, periodo);
        }

        public List<CBaseDTO> DescargarNombramientoFuncionarioCalificar(string cedula, int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarNombramientoFuncionarioCalificar(cedula, periodo);
        }

        public CBaseDTO RatificarCalificacionNombramiento(int codigo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.RatificarCalificacionNombramiento(codigo);
        }

        public CBaseDTO RecibirCalificacionNombramiento(int codigo, bool indEntregado, bool indConformidad)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.RecibirCalificacionNombramiento(codigo, indEntregado, indConformidad);
        }

        public List<CProcAlmacenadoDTO> DescargarDatosCC()
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarDatosCC();
        }

        public List<CProcAlmacenadoDatosGeneralesDTO> DescargarDatosGEvaluacion()
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.DescargarDatosGEvaluacion();
        }

        public CBaseDTO EditarCalificacionNombramiento(int codCalificacion)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.EditarCalificacionNombramiento(codCalificacion);
        }

        public CBaseDTO EditarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.EditarCalificacionNombramientoFuncionario(periodo, detalle);
        }

        public CBaseDTO AsignarJefatura(int periodo, int codFuncionario, int codJefatura)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AsignarJefatura(periodo, codFuncionario, codJefatura);
        }

        public List<List<CBaseDTO>> ListarCalificaciones(string cedula)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarCalificaciones(cedula);
        }

        public List<CBaseDTO> ListarCalificacionesJefatura(int periodo, string cedulaJefatura, string cedulaFuncionario)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarCalificacionesJefatura(periodo, cedulaJefatura, cedulaFuncionario);
        }

        public List<CBaseDTO> ListarCalificacionesPeriodo(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoDTO calificacion)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarCalificacionesPeriodo(periodo, calificacion);
        }

        public List<CBaseDTO> ListarFuncionarios(int idPeriodo, string cedulaFuncionario, string cedulaJefe, string cedulaJefeSuperior, int idSeccion, int idDireccion, int idDivision)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarFuncionarios(idPeriodo, cedulaFuncionario, cedulaJefe, cedulaJefeSuperior, idSeccion, idDireccion, idDivision);
        }

        public List<CBaseDTO> ListarJefaturas(int idPeriodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarJefaturas(idPeriodo);
        }

        public List<CBaseDTO> ListarFuncionariosJefatura(int idPeriodo, string cedula)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarFuncionariosJefatura(idPeriodo, cedula);
        }
        //----------------------------------Catálogo Preguntas--------------------------------------------------------//

        public CBaseDTO AgregarPregunta(CCatalogoPreguntaDTO pregunta) {
            CCatalogoPreguntaL respuesta = new CCatalogoPreguntaL();
            return respuesta.AgregarPregunta(pregunta);
        }

        public CBaseDTO EditarPregunta(CCatalogoPreguntaDTO pregunta) {
            CCatalogoPreguntaL respuesta = new CCatalogoPreguntaL();
            return respuesta.EditarPregunta(pregunta);
        }


        public CBaseDTO ObtenerPreguntas(int codPregunta) {
            CCatalogoPreguntaL respuesta = new CCatalogoPreguntaL();
            return respuesta.ObtenerPreguntas(codPregunta);
        }

        public List<CBaseDTO> ListarPreguntas(int tipoFormulario) {
            CCatalogoPreguntaL respuesta = new CCatalogoPreguntaL();
            return respuesta.ListarPreguntas(tipoFormulario);
        }
        //---------------------------------------------------------------------------------------------------------------//


        public CBaseDTO AgregarPeriodoCalificacion(CPeriodoCalificacionDTO periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AgregarPeriodoCalificacion(periodo);
        }

        public CBaseDTO ObtenerPeriodoCalificacion(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerPeriodoCalificacion(periodo);
        }
        
        public CBaseDTO GenerarListadoFuncionariosPeriodo(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GenerarListadoFuncionariosPeriodo(periodo);
        }

        public CBaseDTO GenerarListaReglaTecnica(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GenerarListaReglaTecnica(periodo);
        }

        public CBaseDTO CargarArchivoReglaTecnica(CCalificacionReglaTecnicaDTO regla)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.CargarArchivoReglaTecnica(regla);
        }

        public CBaseDTO AsignarDirectorReglaTecnica(CCalificacionReglaTecnicaDTO regla)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AsignarDirectorReglaTecnica(regla);
        }

        public List<CBaseDTO> ListarReglaTecnica(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ListarReglaTecnica(periodo);
        }

        public List<CBaseDTO> ObtenerReglaTecnica(int codRegla)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerReglaTecnica(codRegla);
        }

        public List<List<CBaseDTO>> ObtenerDatosEvaluacion(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerDatosEvaluacion(periodo);
        }

        public CBaseDTO AgregarCalificacionHistorico(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario, decimal nota, string justificacion, bool EsPolicia, int IndFormulario)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AgregarCalificacionHistorico(periodo, funcionario, nota, justificacion, EsPolicia, IndFormulario);
        }

        public CBaseDTO AgregarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AgregarCalificacionNombramientoFuncionario(periodo, detalle);
        }

        public CBaseDTO ActualizarCalificacionNombramientoFuncionario(CCalificacionNombramientoFuncionarioDTO detalle)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ActualizarCalificacionNombramientoFuncionario(detalle);
        }

        public List<CBaseDTO> ObtenerPeriodosCalificacion()
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerPeriodosCalificacion();
        }
        public CRespuestaDTO ObtenerCantidadNoEvaluados(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerCantidadNoEvaluados(periodo);
        }
        public List<CBaseDTO> GenerarListaNoEvaluados(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GenerarListaNoEvaluados(periodo);
        }
        public CRespuestaDTO ObtenerCantidadEvaluados(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.ObtenerCantidadEvaluados(periodo);
        }
        public List<CBaseDTO> GenerarListaEvaluados(int periodo)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.GenerarListaEvaluados(periodo);
        }

        public CRespuestaDTO AgregarAutoEvaluacion(int periodo, int codFuncionario, decimal nota)
        {
            CCalificacionNombramientoL respuesta = new CCalificacionNombramientoL();
            return respuesta.AgregarAutoEvaluacion(periodo, codFuncionario, nota);
        }

        ///////////
        ///////////
        // Detalle de las Necesidades de Capacitación
        ///////////
        ///////////
        public List<CBaseDTO> ListarTiposPoliticas()
        {
            CTipoPoliticaPublicaL respuesta = new CTipoPoliticaPublicaL();
            return respuesta.RetornarTipos();
        }

        public List<CBaseDTO> ObtenerTipoPoliticaPublica(int codigo)
        {
            CTipoPoliticaPublicaL respuesta = new CTipoPoliticaPublicaL();
            return respuesta.ObtenerTipo(codigo);
        }


        ///////////
        ///////////
        // Evaluación por Objetivos
        ///////////
        ///////////

        /*ObjetivoInstitucional*/
        public List<CBaseDTO> AgregarObjetivo(CObjetivoCalificacionDTO objetivo)
        {
            CObjetivoCalificacionL respuesta = new CObjetivoCalificacionL();
            return respuesta.GuardarObjetivo(objetivo);
        }

        public List<CBaseDTO> ObtenerObjetivo(int codigo)
        {
            CObjetivoCalificacionL respuesta = new CObjetivoCalificacionL();
            return respuesta.ObtenerObjetivo(codigo);
        }

        public List<CBaseDTO> ListarObjetivos(CObjetivoCalificacionDTO objetivo)
        {
            CObjetivoCalificacionL respuesta = new CObjetivoCalificacionL();
            return respuesta.BuscarObjetivos(objetivo);
        }


        /*MetaObjetivoInstitucional*/
        public List<CBaseDTO> AgregarMeta(CMetaObjetivoCalificacionDTO meta)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.GuardarMeta(meta);
        }

        public List<CBaseDTO> ObtenerMeta(int codigo)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.ObtenerMeta(codigo);
        }

        public List<CBaseDTO> ObtenerUbicacion(int idSeccion)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.ObtenerUbicacion(idSeccion);
        }

        public List<List<CBaseDTO>> ListarMetas(CFuncionarioDTO funcionario, CObjetivoCalificacionDTO objetivo, CMetaObjetivoCalificacionDTO meta,
                                                       List<DateTime> fechasInicio, List<DateTime> fechasVencimiento, CSeccionDTO seccion)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.BuscarMetas(funcionario, objetivo,  meta, fechasInicio, fechasVencimiento, seccion);
        }

        public CBaseDTO ModificarMeta(CMetaObjetivoCalificacionDTO meta)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.ModificarMeta(meta);
        }

        public CBaseDTO AnularMeta(CMetaObjetivoCalificacionDTO meta)
        {
            CMetaObjetivoCalificacionL respuesta = new CMetaObjetivoCalificacionL();
            return respuesta.AnularMeta(meta);
        }


        /*Metas Individuales*/
        public List<CBaseDTO> AgregarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.GuardarMeta(meta);
        }

        public List<CBaseDTO> ObtenerMetaIndividual(int codigo)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.ObtenerMeta(codigo);
        }

        public List<CBaseDTO> ListarMetasIndividuales(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo, CMetaIndividualCalificacionDTO meta,
                                                       List<DateTime> fechasInicio, List<DateTime> fechasVencimiento)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.BuscarMetas(funcionario, periodo, meta, fechasInicio, fechasVencimiento);
        }

        public CBaseDTO ModificarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.ModificarMeta(meta);
        }

        public CBaseDTO HabilitarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.HabilitarMeta(meta);
        }

        public CBaseDTO IniciarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.IniciarMeta(meta);
        }

        public CBaseDTO CompletarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.ModificarEstadoMeta(meta, "Completado");
        }

        public CBaseDTO CerrarMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.ModificarEstadoMeta(meta, "Cerrado");
        }

        public CBaseDTO AnularMetaIndividual(CMetaIndividualCalificacionDTO meta)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.ModificarEstadoMeta(meta, "Anulado");
        }

        public List<List<CBaseDTO>> CargarAsignarMeta(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario, CFuncionarioDTO jefe)
        {
            CMetaIndividualCalificacionL respuesta = new CMetaIndividualCalificacionL();
            return respuesta.CargarAsignarMeta(periodo, funcionario, jefe);
        }

        /*Evidencias*/
        public List<CBaseDTO> AgregarEvidencia(CMetaIndividualEvidenciaDTO evidencia)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.GuardarEvidencia(evidencia);
        }

        public CBaseDTO ModificarEvidencia(CMetaIndividualEvidenciaDTO evidencia)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.ModificarEvidencia(evidencia);
        }

        public List<CBaseDTO> ObtenerEvidencia(int codigo)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.ObtenerEvidencia(codigo);
        }

        public List<List<CBaseDTO>> ListarEvidencias(CFuncionarioDTO funcionario, CMetaIndividualEvidenciaDTO evidencia,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta, CPeriodoCalificacionDTO periodo)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.BuscarEvidencias(funcionario, evidencia, fechasRegistro, meta, periodo);
        }

        public CBaseDTO InsertarPermiso(CFuncionarioDTO funcionario)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.InsertarPermiso(funcionario);
        }
        public CBaseDTO AnularPermiso(CFuncionarioDTO funcionario)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.AnularPermiso(funcionario);
        }
        public CBaseDTO ObtenerPermiso(CFuncionarioDTO funcionario)
        {
            CMetaIndividualEvidenciaL respuesta = new CMetaIndividualEvidenciaL();
            return respuesta.ObtenerPermiso(funcionario);
        }

        /*Informe*/
        public List<CBaseDTO> AgregarInforme(CMetaIndividualInformeDTO Informe)
        {
            CMetaIndividualInformeL respuesta = new CMetaIndividualInformeL();
            return respuesta.GuardarInforme(Informe);
        }

        public CBaseDTO ModificarInforme(CMetaIndividualInformeDTO Informe)
        {
            CMetaIndividualInformeL respuesta = new CMetaIndividualInformeL();
            return respuesta.ModificarInforme(Informe);
        }

        public List<CBaseDTO> ObtenerInforme(int codigo)
        {
            CMetaIndividualInformeL respuesta = new CMetaIndividualInformeL();
            return respuesta.ObtenerInforme(codigo);
        }

        public List<List<CBaseDTO>> ListarInforme(CFuncionarioDTO funcionario, CMetaIndividualInformeDTO Informe,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta, CPeriodoCalificacionDTO periodo)
        {
            CMetaIndividualInformeL respuesta = new CMetaIndividualInformeL();
            return respuesta.BuscarInformes(funcionario, Informe, fechasRegistro, meta, periodo);
        }

        /*Observaciones de las competencias*/
        public List<CBaseDTO> GuardarObservacion(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo, CCalificacionObservacionesDTO observaciones)
        {
            CCalificacionObservacionesL respuesta = new CCalificacionObservacionesL();
            return respuesta.GuardarObservacion(funcionario, periodo, observaciones);
        }

        public CBaseDTO ModificarObservacion(CCalificacionObservacionesDTO observaciones)
        {
            CCalificacionObservacionesL respuesta = new CCalificacionObservacionesL();
            return respuesta.ModificarObservacion(observaciones);
        }

        public List<CBaseDTO> ObtenerObservacion(int codigo)
        {
            CCalificacionObservacionesL respuesta = new CCalificacionObservacionesL();
            return respuesta.ObtenerObservacion(codigo);
        }

        public List<List<CBaseDTO>> ListarObservaciones(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario,
                                                            CCalificacionObservacionesDTO observaciones, List<DateTime> fechasRegistro)
        {
            CCalificacionObservacionesL respuesta = new CCalificacionObservacionesL();
            return respuesta.BuscarCalificacionObservaciones(periodo,funcionario, observaciones, fechasRegistro);
        }

        public List<CBaseDTO> ListarMetaPrioridades()
        {
            CCatMetaPrioridadL respuesta = new CCatMetaPrioridadL();
            return respuesta.ListarPrioridades();
        }

        public List<CBaseDTO> ListarMetaEstados()
        {
            CCatMetaEstadoL respuesta = new CCatMetaEstadoL();
            return respuesta.DescargarEstado();
        }

        /*Actividades Funcionario*/
        public List<CBaseDTO> AgregarActividad(CActividadFuncionarioDTO actividad)
        {
            CActividadFuncionarioL respuesta = new CActividadFuncionarioL();
            return respuesta.GuardarActividad(actividad);
        }

        public CBaseDTO ModificarActividad(CActividadFuncionarioDTO actividad)
        {
            CActividadFuncionarioL respuesta = new CActividadFuncionarioL();
            return respuesta.ModificarActividad(actividad);
        }

        public List<CBaseDTO> ObtenerActividad(int codigo)
        {
            CActividadFuncionarioL respuesta = new CActividadFuncionarioL();
            return respuesta.ObtenerActividad(codigo);
        }

        public List<CBaseDTO> ListarActividad(CFuncionarioDTO funcionario, CActividadFuncionarioDTO actividad,
                                                       List<DateTime> fechasRegistro)
        {
            CActividadFuncionarioL respuesta = new CActividadFuncionarioL();
            return respuesta.BuscarActividades(actividad, funcionario, fechasRegistro);
        }


        public List<CBaseDTO> GuardarEncargado(CCalificacionEncargadoDTO encargado)
        {
            CCalificacionEncargadoL respuesta = new CCalificacionEncargadoL();
            return respuesta.GuardarEncargado(encargado);
        }

        public CBaseDTO ModificarEncargado(CCalificacionEncargadoDTO encargado)
        {
            CCalificacionEncargadoL respuesta = new CCalificacionEncargadoL();
            return respuesta.ModificarEncargado(encargado);
        }

        public List<CBaseDTO> BuscarEncargado(CFuncionarioDTO funcionario, CUbicacionAdministrativaDTO ubicacion)
        {
            CCalificacionEncargadoL respuesta = new CCalificacionEncargadoL();
            return respuesta.BuscarEncargado(funcionario,ubicacion);
        }

        public List<CBaseDTO> ListarTiposIndicador()
        {
            CTipoIndicadorMetaL respuesta = new CTipoIndicadorMetaL();
            return respuesta.RetornarTipos();
        }

        // SEGUIMIENTO
        public CBaseDTO AgregarCalificacionSeguimiento(CDetalleCalificacionSeguimientoDTO detalle)
        {
            CDetalleCalificacionSeguimientoL respuesta = new CDetalleCalificacionSeguimientoL();
            return respuesta.GuardarSeguimiento(detalle);
        }

        public CBaseDTO ModificarCalificacionSeguimiento(CDetalleCalificacionSeguimientoDTO detalle)
        {
            CDetalleCalificacionSeguimientoL respuesta = new CDetalleCalificacionSeguimientoL();
            return respuesta.ModificarSeguimiento(detalle);
        }

        public List<CBaseDTO> ObtenerCalificacionSeguimiento(int codigo)
        {
            CDetalleCalificacionSeguimientoL respuesta = new CDetalleCalificacionSeguimientoL();
            return respuesta.ObtenerDetalleSeguimiento(codigo);
        }
        public List<CBaseDTO> ListarCalificacionSeguimiento(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo)
        {
            CDetalleCalificacionSeguimientoL respuesta = new CDetalleCalificacionSeguimientoL();
            return respuesta.ListarSeguimientos(funcionario, periodo);
        }


        // CAPACITACIÓN
        public List<CBaseDTO> AgregarCapacitacion(CCalificacionCapacitacionDTO capacitacion)
        {
            CCalificacionCapacitacionL respuesta = new CCalificacionCapacitacionL();
            return respuesta.GuardarCapacitacion(capacitacion);
        }

        public CBaseDTO AnularCapacitacion(CCalificacionCapacitacionDTO capacitacion)
        {
            CCalificacionCapacitacionL respuesta = new CCalificacionCapacitacionL();
            return respuesta.AnularCapacitacion(capacitacion);
        }
    }
}
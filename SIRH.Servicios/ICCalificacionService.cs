using System.Collections.Generic;
using System.ServiceModel;
using SIRH.DTO;
using System;

namespace SIRH.Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICCalificacion" in both code and config file together.
    [ServiceContract]
    public interface ICCalificacionService
    {
        //------------------------------------CalificacionDTO-------------------------------------------------------------//
       /* [OperationContract]*/
       /* CBaseDTO GuardarCalificacion(CCalificacionDTO calificacion);*/ 

        [OperationContract]
        CBaseDTO GuardarCalificacion(string cedula,CCalificacionDTO item);

        [OperationContract]
        List<CBaseDTO> CargarCalificaciones();

        //------------------------------------CalificacionDTO NombramientoDTO------------------------------------------------//
        [OperationContract]
        List<CBaseDTO> DescargarCalificacionesCedula(string cedula);

        [OperationContract]
        List<CBaseDTO> DescargarCalificacionCedula(string cedula, int periodo);

        [OperationContract]
        List<CBaseDTO> DescargarFuncionarioCalificarDetallePuesto(string cedula, int periodo);

        [OperationContract]
        List<CBaseDTO> DescargarNombramientoFuncionarioCalificar(string cedula, int periodo);

        [OperationContract]
        List<CBaseDTO> GuardarCalificacionFuncionario(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN);

        [OperationContract]
        List<CBaseDTO> GuardarCalificacionModificada(CCalificacionNombramientoDTO CalificacionCN, List<CDetalleCalificacionNombramientoDTO> DetalleDCN);
        
        [OperationContract]
        List<CBaseDTO> ObtenerCalificacion(int codigo);

        [OperationContract]
        List<CBaseDTO> ObtenerCalificacionHistorico(int codigo);

        [OperationContract]
        CBaseDTO RatificarCalificacionNombramiento(int codigo);

        [OperationContract]
        CBaseDTO RecibirCalificacionNombramiento(int codigo, bool indEntregado, bool indConformidad);

        [OperationContract]
        List<CProcAlmacenadoDTO> DescargarDatosCC();
        [OperationContract]
        List<CProcAlmacenadoDatosGeneralesDTO> DescargarDatosGEvaluacion();

        [OperationContract]
        CBaseDTO EditarCalificacionNombramiento(int codCalificacion);

        [OperationContract]
        CBaseDTO EditarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle);

        [OperationContract]
        CBaseDTO AsignarJefatura(int periodo, int codFuncionario, int codJefatura);

        [OperationContract]
        List<List<CBaseDTO>> ListarCalificaciones(string cedula);

        [OperationContract]
        List<CBaseDTO> ListarFuncionarios(int idPeriodo, string cedulaFuncionario, string cedulaJefe, string cedulaJefeSuperior, int idSeccion, int idDireccion, int idDivision);

        [OperationContract]
        List<CBaseDTO> ListarJefaturas(int idPeriodo);

        [OperationContract]
        List<CBaseDTO> ListarFuncionariosJefatura(int idPeriodo, string cedula);

        [OperationContract]
        List<CBaseDTO> ListarCalificacionesJefatura(int periodo, string cedulaJefatura, string cedulaFuncionario);

        [OperationContract]
        List<CBaseDTO> ListarCalificacionesPeriodo(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoDTO calificacion);

        //----------------------------------Catalogo Pregruntas--------------------------------------------------------//
        [OperationContract]
        CBaseDTO AgregarPregunta(CCatalogoPreguntaDTO pregunta);

        [OperationContract]
        CBaseDTO EditarPregunta(CCatalogoPreguntaDTO pregunta);


        [OperationContract]
        CBaseDTO ObtenerPreguntas(int codPregunta);

        [OperationContract]
        List<CBaseDTO> ListarPreguntas(int tipoFormulario);
        //---------------------------------------------------------------------------------------------------------------//

        [OperationContract]
        CBaseDTO AgregarPeriodoCalificacion(CPeriodoCalificacionDTO periodo);

        [OperationContract]
        CBaseDTO ObtenerPeriodoCalificacion(int periodo);

        [OperationContract]
        CBaseDTO GenerarListadoFuncionariosPeriodo(int periodo);

        [OperationContract]
        CBaseDTO GenerarListaReglaTecnica(int periodo);

        [OperationContract]
        CBaseDTO CargarArchivoReglaTecnica(CCalificacionReglaTecnicaDTO regla);

        [OperationContract]
        CBaseDTO AsignarDirectorReglaTecnica(CCalificacionReglaTecnicaDTO regla);

        [OperationContract]
        List<CBaseDTO> ListarReglaTecnica(int periodo);

        [OperationContract]
        List<CBaseDTO> ObtenerReglaTecnica(int codRegla);
                
        [OperationContract]
        List<List<CBaseDTO>> ObtenerDatosEvaluacion(int periodo);

        [OperationContract]
        CBaseDTO AgregarCalificacionHistorico(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario, decimal nota, string justificacion, bool EsPolicia, int IndFormulario);

        [OperationContract]
        CBaseDTO AgregarCalificacionNombramientoFuncionario(CPeriodoCalificacionDTO periodo, CCalificacionNombramientoFuncionarioDTO detalle);

        [OperationContract]
        CBaseDTO ActualizarCalificacionNombramientoFuncionario(CCalificacionNombramientoFuncionarioDTO detalle);

        [OperationContract]
        List<CBaseDTO> ObtenerPeriodosCalificacion();

        [OperationContract]
        CRespuestaDTO ObtenerCantidadNoEvaluados(int periodo);

        [OperationContract]
        List<CBaseDTO> GenerarListaNoEvaluados(int periodo);

        [OperationContract]
        CRespuestaDTO ObtenerCantidadEvaluados(int periodo);

        [OperationContract]
        List<CBaseDTO> GenerarListaEvaluados(int periodo);

        [OperationContract]
        CRespuestaDTO AgregarAutoEvaluacion(int periodo, int codFuncionario, decimal nota);

        [OperationContract]
        List<CBaseDTO> ListarTiposPoliticas();
        [OperationContract]
        List<CBaseDTO> ObtenerTipoPoliticaPublica(int codigo);

        ///////////
        ///////////
        // Evaluación por Objetivos
        ///////////
        ///////////

        [OperationContract]
        List<CBaseDTO> AgregarObjetivo(CObjetivoCalificacionDTO objetivo);
        [OperationContract]
        List<CBaseDTO> ObtenerObjetivo(int codigo);
        [OperationContract]
        List<CBaseDTO> ListarObjetivos(CObjetivoCalificacionDTO objetivo);
        [OperationContract]

        List<CBaseDTO> AgregarMeta(CMetaObjetivoCalificacionDTO meta);
        [OperationContract]
        List<CBaseDTO> ObtenerMeta(int codigo);
        [OperationContract]
        List<CBaseDTO> ObtenerUbicacion(int idSeccion);
        [OperationContract]
        List<List<CBaseDTO>> ListarMetas(CFuncionarioDTO funcionario, CObjetivoCalificacionDTO objetivo, CMetaObjetivoCalificacionDTO meta,
                                         List<DateTime> fechasInicio, List<DateTime> fechasVencimiento, CSeccionDTO seccion);
        [OperationContract]
        CBaseDTO ModificarMeta(CMetaObjetivoCalificacionDTO meta);

        [OperationContract]
        CBaseDTO AnularMeta(CMetaObjetivoCalificacionDTO meta);

        /* Metas Individuales*/
        [OperationContract]
        List<CBaseDTO> AgregarMetaIndividual(CMetaIndividualCalificacionDTO meta);
        [OperationContract]
        List<CBaseDTO> ObtenerMetaIndividual(int codigo);
        [OperationContract]
        List<CBaseDTO> ListarMetasIndividuales(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo, CMetaIndividualCalificacionDTO meta,
                                                       List<DateTime> fechasInicio, List<DateTime> fechasVencimiento);
        [OperationContract]
        CBaseDTO ModificarMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        CBaseDTO HabilitarMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        CBaseDTO IniciarMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        CBaseDTO CompletarMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        CBaseDTO CerrarMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        CBaseDTO AnularMetaIndividual(CMetaIndividualCalificacionDTO meta);

        [OperationContract]
        List<List<CBaseDTO>> CargarAsignarMeta(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario, CFuncionarioDTO jefe);


        /* Evidencias*/

        [OperationContract]
        List<CBaseDTO> AgregarEvidencia(CMetaIndividualEvidenciaDTO evidencia);
        [OperationContract]
        List<CBaseDTO> ObtenerEvidencia(int codigo);
        [OperationContract]
        CBaseDTO ModificarEvidencia(CMetaIndividualEvidenciaDTO evidencia);
        [OperationContract]
        List<List<CBaseDTO>> ListarEvidencias(CFuncionarioDTO funcionario, CMetaIndividualEvidenciaDTO evidencia,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta, CPeriodoCalificacionDTO periodo);

        [OperationContract]
        CBaseDTO InsertarPermiso(CFuncionarioDTO funcionario);
        [OperationContract]
        CBaseDTO AnularPermiso(CFuncionarioDTO funcionario);
        [OperationContract]
        CBaseDTO ObtenerPermiso(CFuncionarioDTO funcionario);

        /*Informes*/
        [OperationContract]
        List<CBaseDTO> AgregarInforme(CMetaIndividualInformeDTO Informe);
        [OperationContract]
        List<CBaseDTO> ObtenerInforme(int codigo);
        [OperationContract]
        CBaseDTO ModificarInforme(CMetaIndividualInformeDTO Informe);
        [OperationContract]
        List<List<CBaseDTO>> ListarInforme(CFuncionarioDTO funcionario, CMetaIndividualInformeDTO Informe,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta, CPeriodoCalificacionDTO periodo);


        [OperationContract]
        List<CBaseDTO> GuardarObservacion(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo, CCalificacionObservacionesDTO observaciones);
        [OperationContract]
        CBaseDTO ModificarObservacion(CCalificacionObservacionesDTO observaciones);
        [OperationContract]
        List<CBaseDTO> ObtenerObservacion(int codigo);
        [OperationContract]
        List<List<CBaseDTO>> ListarObservaciones(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario,
                                                 CCalificacionObservacionesDTO observaciones, List<DateTime> fechasRegistro);
        [OperationContract]
        List<CBaseDTO> ListarMetaPrioridades();
        [OperationContract]
        List<CBaseDTO> ListarMetaEstados();

        /*Actividades Funcionario*/
        [OperationContract]
        List<CBaseDTO> AgregarActividad(CActividadFuncionarioDTO actividad);
        [OperationContract]
        List<CBaseDTO> ObtenerActividad(int codigo);
        [OperationContract]
        CBaseDTO ModificarActividad(CActividadFuncionarioDTO actividad);
        [OperationContract]
        List<CBaseDTO> ListarActividad(CFuncionarioDTO funcionario, CActividadFuncionarioDTO actividad,
                                                       List<DateTime> fechasRegistro);


        [OperationContract]
        List<CBaseDTO> GuardarEncargado(CCalificacionEncargadoDTO encargado);
        [OperationContract]
        CBaseDTO ModificarEncargado(CCalificacionEncargadoDTO registro);
        [OperationContract]
        List<CBaseDTO> BuscarEncargado(CFuncionarioDTO funcionario, CUbicacionAdministrativaDTO ubicacion);

        [OperationContract]
        List<CBaseDTO> ListarTiposIndicador();

        // SEGUIMIENTO 
        [OperationContract]
        CBaseDTO AgregarCalificacionSeguimiento(CDetalleCalificacionSeguimientoDTO detalle);

        [OperationContract]
        CBaseDTO ModificarCalificacionSeguimiento(CDetalleCalificacionSeguimientoDTO detalle);

        [OperationContract]
        List<CBaseDTO> ObtenerCalificacionSeguimiento(int codigo);

        [OperationContract]
        List<CBaseDTO> ListarCalificacionSeguimiento(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo);

        // CAPACITACIÓN
        [OperationContract]
        List<CBaseDTO> AgregarCapacitacion(CCalificacionCapacitacionDTO capacitacion);

        [OperationContract]
        CBaseDTO AnularCapacitacion(CCalificacionCapacitacionDTO capacitacion);
    }
}

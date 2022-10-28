using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICPuestoService" here, you must also update the reference to "ICPuestoService" in App.config.
    [ServiceContract]
    public interface ICPuestoService
    {
        //Para subir
        [OperationContract]
        List<List<CBaseDTO>> BuscarHistorialUbicacionTrabajo(string codPuesto);

        [OperationContract]
        List<CBaseDTO> DetalleMovimiento(int codMovimiento);

        [OperationContract]
        CBaseDTO ModificarUbicacionPuesto(CPuestoDTO puesto, CUbicacionPuestoDTO ubicacion);

        [OperationContract]
        List<List<CBaseDTO>> GetLocalizacion(bool cantones, int canton, bool distritos, bool provincias, int provincia);

        [OperationContract]
        List<CBaseDTO> DescargarUbicacionTrabajoPedimento(string codPuesto);

        [OperationContract]
        List<List<CBaseDTO>> DescargarPerfilPuestoAcciones(string codPuesto);

        [OperationContract]
        List<List<CBaseDTO>> DescargarPerfilPuestoAccionesFuncionario(string cedula);

        [OperationContract]
        List<CBaseDTO> ListarMotivosMovimiento();

        [OperationContract]
        List<CBaseDTO> DescargarPuestoPedimento(string pedimento);

        [OperationContract]
        List<CBaseDTO> DescargarPuestoVacante(string codigo);

        [OperationContract]
        List<CBaseDTO> DescargarPuestoCompleto(string codigo);

        [OperationContract]
        List<CMotivoMovimientoDTO> DescargarMotivoMovimientoCompleto();
        [OperationContract]
        List<List<CBaseDTO>> CargarUbicacionPuesto(string cedula);
        [OperationContract]
        List<CDireccionGeneralDTO> DescargarDireccionGenerals(int codigo, string nombre);
        [OperationContract]
        List<CDepartamentoDTO> DescargarDepartamentos(int codigo, string nombre);
        [OperationContract]
        List<CClaseDTO> DescargarClases(int codigo, string nombre);
        [OperationContract]
        List<CBaseDTO> BuscarDatosPuesto(string codPuesto);
        [OperationContract]
        List<CBaseDTO> DescargarUbicacionAdministrativa(string cedula);
        [OperationContract]
        List<CSeccionDTO> DescargarSeccions(int codigo, string nombre);
        [OperationContract]
        List<COcupacionRealDTO> DescargarOcupacionReals(int codigo, string nombre);
        [OperationContract]
        List<CEspecialidadDTO> DescargarEspecialidades(int codigo, string nombre);
        [OperationContract]
        List<CSubEspecialidadDTO> DescargarSubEspecialidades(string nombre);
        [OperationContract]
        List<CBaseDTO> ListarCategoriasEscalaSalarial();
        [OperationContract]
        List<CBaseDTO> BuscarEscalaCategoria(int indCategoria);
        [OperationContract]
        List<CBaseDTO> ListarEntidadesGubernamentales();
        [OperationContract]
        CBaseDTO BuscarEntidadGubernamental(int codigo);
        [OperationContract]
        List<CBaseDTO> ListarEntidadesAdscritas();
        [OperationContract]
        CBaseDTO BuscarEntidadAdscrita(int codigo);
        [OperationContract]
        CMotivoMovimientoDTO CargarMotivoMovimientoPorPuesto(string NumeroPuesto);
        [OperationContract]
        CMotivoMovimientoDTO CargarMotivoMovimientoPorCedula(string Cedula);
        [OperationContract]
        CMovimientoPuestoDTO RetornarMovimientoPuestoEspecifico(string numeroPuesto);
        [OperationContract]
        CMovimientoPuestoDTO RetornarMovimientoPuestoEspecificoOficio(string CodOficio);
        [OperationContract]
        CBaseDTO GuardarMovimientoPuesto(CMovimientoPuestoDTO entidad);
        [OperationContract]
        CBaseDTO InsertarMovimientoPuesto(CMovimientoPuestoDTO movimiento);
        [OperationContract]
        List<CBaseDTO> ListarEntidadEducativa();
        [OperationContract]
        CTipoPuestoDTO RetornarTipoPuestoEspecifico(string codPuesto);
        [OperationContract]
        CBaseDTO GuardarTareasPuesto(CTareasPuestoDTO tareas);
        [OperationContract]
        CBaseDTO BuscarTareasId(CTareasPuestoDTO tareasPuesto);
        [OperationContract]
        List<CBaseDTO> BuscarTareasCodPuesto(CTareasPuestoDTO tareas);
        [OperationContract]
        CBaseDTO BuscarContenidoPresupuestario(CContenidoPresupuestarioDTO contenido);
        [OperationContract]
        CBaseDTO BuscarEstudioPuestoPorPuesto(CEstudioPuestoDTO prestamo);
        [OperationContract]
        List<CBaseDTO> BuscarPrestamoPuestoPorPuesto(CPrestamoPuestoDTO prestamo);
        [OperationContract]
        CBaseDTO BuscarTituloFactorId(CTituloFactorDTO tituloFactor);
        [OperationContract]
        List<CPuestoDTO> BuscarPuestoParams(string codpuesto, int clase, int especialidad, int ocupacion);
        [OperationContract]
        List<CBaseDTO> DetallePuestoPorCodigo(string codPuesto);
        [OperationContract]
        List<CBaseDTO> DetallePuestoPorCedula(string cedula);
        [OperationContract]
        List<CBaseDTO> CargarPuestoActivo(string cedula);
        [OperationContract]
        List<CBaseDTO> BuscarTitulosFactorPorPuesto(CPuestoDTO puesto);
        [OperationContract]
        CBaseDTO GuardarTituloFactor(CTituloFactorDTO titulo);
        [OperationContract]
        CBaseDTO GuardarFactor(CTituloFactorDTO titulo, CFactorDTO factor);
        [OperationContract]
        CBaseDTO GuardarPrestamoPuesto(CPuestoDTO puesto, CPrestamoPuestoDTO prestamo);
        [OperationContract]
        List<CBaseDTO> GuardarCaracteristicasPuesto(CPuestoDTO puesto, CCaracteristicasPuestoDTO caracteristicas, CTareasPuestoDTO tareas, CFactorDTO factor, CTituloFactorDTO titulo);
        [OperationContract]
        CBaseDTO BuscarAddendumPrestamoPuesto(CAddendumPrestamoPuestoDTO addendum);
        [OperationContract]
        List<CBaseDTO> BuscarAddendumIdPrestamo(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum);
        [OperationContract]
        CBaseDTO GuardarNumAddendum(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum);
        [OperationContract]
        CBaseDTO GuardarNumeroRescision(CRescisionDTO rescisionP, CPrestamoPuestoDTO prestamo);
        [OperationContract]
        CBaseDTO GuardarNumResolucion(CDetallePuestoDTO detallePuesto, CContenidoPresupuestarioDTO contenido);
        [OperationContract]
        List<CBaseDTO> BuscarPedimentosPorPuesto(CPuestoDTO puesto);
        [OperationContract]
        CBaseDTO GuardarPedimentoPuesto(CPuestoDTO puesto, CPedimentoPuestoDTO pedimento);
        [OperationContract]
        CBaseDTO GuardarEstudioPuesto(CPuestoDTO puesto, CEstudioPuestoDTO estudio);
        [OperationContract]
        List<CBaseDTO> BuscarPuestoCodigo(string codPuesto);
        [OperationContract]
        List<CBaseDTO> BuscarPuestoCedula(string cedula);
        [OperationContract]
        List<CClaseDTO> BuscarClaseParams(int codClase, string nomClase);
        [OperationContract]
        List<CEspecialidadDTO> BuscarEspecialidadParams(int codEspecialidad, string nomEspecialidad);
        [OperationContract]
        List<COcupacionRealDTO> BuscarOcupacionParams(int codOcupacion, string nomOcupacion);
        [OperationContract]
        List<CDivisionDTO> BuscarDivisionParams(int codigo, string nombre);
        [OperationContract]
        List<CDireccionGeneralDTO> BuscarDireccionGeneralParams(int codigo, string nombre);
        [OperationContract]
        List<CDepartamentoDTO> BuscarDepartamentoParams(int codigo, string nombre);
        [OperationContract]
        List<CSeccionDTO> BuscarSeccionParams(int codigo, string nombre);
        [OperationContract]
        List<List<CPresupuestoDTO>> BuscarPresupuestoParams(string codigo);
        [OperationContract]
        List<CProgramaDTO> ObtenerCodigosProgramas();

        [OperationContract]
        List<List<CBaseDTO>> DescargarPerfilPuestoAccionesFuncionarioPP(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarCentroCostosParams(string centrocostos);

        [OperationContract]
        List<CBaseDTO> BuscarCodigoPresupuestarioParams(string codigopresupuesto);

        [OperationContract]
        List<CMovimientoPuestoDTO> BuscarMovimientoPuestosFiltros(string codPuesto, string numCedula, int motivoM, DateTime? fechaHasta, DateTime? fechaDesde);

        [OperationContract]
        List<CPuestoDTO> BuscarFuncionarioPuestoPuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal, string estadoPuesto, string confianza, int codNivel);

        [OperationContract]
        List<CBaseDTO> DatosUbicacionDetallePuesto(string codpuesto);


        [OperationContract]
        List<CBaseDTO> CargarPuestoPropiedad(string cedula);

        /*ORDEN DE MOVIMIENTO*/

        [OperationContract]
        List<CBaseDTO> DatosOrdenMovimiento(string cedula, string puesto);

        [OperationContract]
        CBaseDTO AgregarOrden(COrdenMovimientoDTO orden, COrdenMovimientoDeclaracionDTO declaracion);

        [OperationContract]
        CBaseDTO AnularOrden(COrdenMovimientoDTO orden);

        [OperationContract]
        CBaseDTO ActualizarOrden(COrdenMovimientoDTO orden);

        [OperationContract]
        List<CBaseDTO> ObtenerOrden(int codigo);

        [OperationContract]
        List<CBaseDTO> BuscarOrdenes(COrdenMovimientoDTO orden, List<DateTime> fechas);

        [OperationContract]
        List<CBaseDTO> ListarOrdenEstados();

        [OperationContract]
        List<CBaseDTO> BuscarSubespecialidadParam(int codigo, string descripcion);

        [OperationContract]
        List<CBaseDTO> UbicacionAdministrativaSeccion(int seccion);

        [OperationContract]
        CBaseDTO ActualizarDatosPuesto(CDetallePuestoDTO detallePuesto, CUbicacionAdministrativaDTO ubicacionAdmin, CUbicacionPuestoDTO ubicacionContrato,
                                              CUbicacionPuestoDTO ubicacionTrabajo, CPuestoDTO puesto);

        [OperationContract]
        List<List<CBaseDTO>> BuscarPedimentos(CPuestoDTO puesto, CPedimentoPuestoDTO pedimento,
                                                 List<DateTime> fechasEnvio);

        [OperationContract]
        List<CBaseDTO> BuscarPedimentoCodigo(int pedimento);
    }
}
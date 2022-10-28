using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Servicios
{
    // NOTE: If you change the class name "CFuncionarioService" here, you must also update the reference to "CFuncionarioService" in App.config.
    public class CFuncionarioService : ICFuncionarioService
    {
        //Comentario para cambios otra vez
        public CBaseDTO GuardarDireccionFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion)
        {
            CDireccionL logica = new CDireccionL();
            return logica.GuardarDireccionFuncionario(funcionario, direccion);
        }

        public List<List<CBaseDTO>> BuscarFuncionarioOferente(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioOferente(cedula);
        }

        public List<CBaseDTO> ListarEntidadesFinancieras()
        {
            CEntidadFinancieraL logica = new CEntidadFinancieraL();
            return logica.ListarEntidadesFinancieras();
        }

        public List<CBaseDTO> ListarEntidadesGubernamentales()
        {
            CEntidadGubernamentalL logica = new CEntidadGubernamentalL();
            return logica.ListarEntidadesGubernamentales();
        }

        public List<CBaseDTO> ListarEntidadesAdscritas()
        {
            CEntidadAdscritaL logica = new CEntidadAdscritaL();
            return logica.ListarEntidadesAdscritas();
        }

        public CBaseDTO BuscarEntidadFinanciera(int codigo)
        {
            CEntidadFinancieraL logica = new CEntidadFinancieraL();
            return logica.BuscarEntidadFinanciera(codigo);
        }

        public CBaseDTO GuardarInformacionContacto(CFuncionarioDTO funcionario, CInformacionContactoDTO informacionContacto)
        {
            CInformacionContactoL logica = new CInformacionContactoL();
            return logica.GuardarInformacionContacto(funcionario, informacionContacto);
        }

        public CBaseDTO GuardarCuentaBancariaFuncionario(CCuentaBancariaDTO cuenta)
        {
            CCuentaBancariaL logica = new CCuentaBancariaL();
            return logica.GuardarCuentaBancariaFuncionario(cuenta);
        }

        public List<CBaseDTO> DescargarDireccion(string cedula)
        {
            CDireccionL logica = new CDireccionL();
            return logica.DescargarDireccion(cedula);
        }

        public List<CBaseDTO> DescargarCatEstadoCivil(string cedula)
        {
            CCatEstadoCivilL logica = new CCatEstadoCivilL();
            return logica.DescargarCatEstadoCivil(cedula);
        }
        public List<CBaseDTO> EditarJornadaFuncionario(CTipoJornadaDTO jornada)
        {
            CTipoJornadaL logica = new CTipoJornadaL();
            return logica.EditarJornadaFuncionario(jornada);
        }

        public List<CBaseDTO> RegistrarJornadaFuncionario(CFuncionarioDTO funcionario,
                                                            CNombramientoDTO nombramiento,
                                                            CTipoJornadaDTO jornada)
        {
            CTipoJornadaL logica = new CTipoJornadaL();
            return logica.RegistrarJornadaFuncionario(funcionario, nombramiento, jornada);
        }

        public List<CBaseDTO> DescargarInformacionContacto(string cedula)
        {
            CInformacionContactoL logica = new CInformacionContactoL();
            return logica.DescargarInformacionContacto(cedula);
        }

        public CBaseDTO GuardarHistEstadoCivil(CFuncionarioDTO funcionario, CHistorialEstadoCivilDTO historialEstadoCivil)
        {
            CHistorialEstadoCivilL logica = new CHistorialEstadoCivilL();
            return logica.GuardarHistEstadoCivil(funcionario, historialEstadoCivil);
        }

        public List<CBaseDTO> GuardarDetalleContratacion(CDetalleContratacionDTO detalle, CCuentaBancariaDTO cuenta)
        {
            CDetalleContratacionL logica = new CDetalleContratacionL();
            return logica.GuardarDetalleContratacion(detalle, cuenta);
        }

        public CBaseDTO GuardarDetalleNombramiento(CDetalleNombramientoDTO detalleNombramiento)
        {
            CDetalleNombramientoL logica = new CDetalleNombramientoL();
            return logica.GuardarDetalleNombramiento(detalleNombramiento);
        }


        public List<CBaseDTO> GuardarDatosPersonalesFuncionario(CFuncionarioDTO funcionario, 
                                                                CHistorialEstadoCivilDTO estadoCivil, 
                                                                List<CInformacionContactoDTO> informacion,
                                                                CDireccionDTO direccion)
        {
            var saprissa = 0;
            saprissa = saprissa * 100;
            CFuncionarioL logica = new CFuncionarioL();
            return logica.GuardarDatosPersonalesFuncionario(funcionario, estadoCivil, informacion, direccion);
        }

        public List<List<CBaseDTO>> FuncionariosConCauciones()
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.FuncionariosConCauciones();
        }

        public List<CBaseDTO> BuscarFuncionarioUsuario(string nombreUsuario)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioUsuario(nombreUsuario);
        }

        public CBaseDTO BuscarFuncionarioBase(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioBase(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioJornada(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioJornada(cedula);
        }

        public CFuncionarioDTO DescargarPerfilFuncionarioBásico(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.DescargarFuncionario(cedula);
        }

        public List<CFuncionarioDTO> BuscarFuncionarioGeneral(List<string> query)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BusquedaGeneralFuncionarios(query);
        }

        public List<CFuncionarioDTO> BuscarFuncionarioParam(string cedula, string nombre, string apellido1, string apellido2)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BusquedaFuncionarioLogica(cedula, nombre, apellido1, apellido2);
        }

        public List<CFuncionarioDTO> BuscarFuncionarioPuesto(string codPuesto,
                                                                int codClase,
                                                                int codEspecialidad,
                                                                int codOcupacionReal)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.EnviarFuncionarioDetallePuesto(codPuesto, codClase, codEspecialidad, codOcupacionReal);
        }

        public List<CFuncionarioDTO> BuscarFuncionarioUbicacion(int division, int direccion, int departamento, int seccion, string presupuesto)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.EnviarFuncionarioUbicacion(division, direccion, departamento, seccion, presupuesto);
        }

        public List<CBaseDTO> BuscarFuncionarioDesgloceSalarial(string cedula, string periodo)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDesgloceSalarial(cedula, periodo);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuesto(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoVacaciones(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuestoVacaciones(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoPV(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuestoPV(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoPropiedad(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuestoPropiedad(cedula);
        }
        public List<CBaseDTO> BuscarFuncionarioSalario(string cedula)
        {
            CSalarioL logica = new CSalarioL();
            return logica.ObtenerSalario(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioProfesional(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioProfesional(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioPolicial(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioPolicial(cedula);
        }

        public CBaseDTO GuardarDetalleContratacionFuncionario(CDetalleContratacionDTO datos)
        {
            CDetalleContratacionL logica = new CDetalleContratacionL();
            return logica.GuardarDetalleContratacion(datos);
        }

        public List<CBaseDTO> CargarDetalleContratacionFuncionario(string cedula)
        {
            CDetalleContratacionL logica = new CDetalleContratacionL();
            return logica.CargarDetalleContratacionFuncionario(cedula);
        }

        /// <summary>
        /// [0][0] Funcionario
        /// [1][0...N] Estado Civil
        /// [2][0] Contacto
        /// [3][0] Localización
        /// [4][0] Detalle Contratación
        /// [5][0] Nombramiento
        /// [5][1] Estado Nombramiento
        /// [6][0...N] Calificaciones
        /// [7][0] Puesto
        /// [7][1] Estado Puesto
        /// [7][2] Clase
        /// [7][3] Especialidad
        /// [7][4] Subespecialidad
        /// [7][5] Ocupación Real
        /// [7][6] Detalle Puesto
        /// [8][.] Ubicación Administrativa
        /// [8][0] Presupuesto
        /// [8][1] Programa
        /// [8][2] Area
        /// [8][3] Actividad
        /// [8][4] División
        /// [8][5] Dirección General
        /// [8][6] Departamento
        /// [8][7] Sección
        /// [9...N][0] Ubicación Contrato
        /// [9...N][1] Tipo Ubicación
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> DescargarPerfilFuncionarioCompleto(string cedula)
        {
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();


            CFuncionarioL logica = new CFuncionarioL();
            CCatEstadoCivilL logicaEstadoCivil = new CCatEstadoCivilL();
            CInformacionContactoL logicaContacto = new CInformacionContactoL();
            CDireccionL logicaDireccion = new CDireccionL();
            CDetalleContratacionL logicaContrato = new CDetalleContratacionL();
            CNombramientoL logicaNombramiento = new CNombramientoL();
            CCalificacionNombramientoL logicaCalificacion = new CCalificacionNombramientoL();
            CPuestoL logicaPuesto = new CPuestoL();
            CUbicacionAdministrativaL logicaUbAdministrativa = new CUbicacionAdministrativaL();
            CUbicacionPuestoL logicaUbicacion = new CUbicacionPuestoL();


            List<CBaseDTO> listacontratos = new List<CBaseDTO>();
            List<CBaseDTO> funcionarioLista = new List<CBaseDTO>();
            List<CBaseDTO> listanombramiento = new List<CBaseDTO>();


            var datosGenerales = logica.DescargarFuncionario(cedula);
            if (datosGenerales.Sexo == 0)
            {
                datosGenerales.Sexo = GeneroEnum.Indefinido;
            }
            funcionarioLista.Add(datosGenerales);
            var historialEstadoCivil = logicaEstadoCivil.DescargarCatEstadoCivil(cedula);
            var datosContacto = logicaContacto.DescargarInformacionContacto(cedula) != null ? logicaContacto.DescargarInformacionContacto(cedula) : new List<CBaseDTO>();
            var direcciones = logicaDireccion.DescargarDireccion(cedula) != null ? logicaDireccion.DescargarDireccion(cedula) : new List<CBaseDTO>();
            var contrato = logicaContrato.DescargarDetalleContratacion(cedula);
            listacontratos.Add((CBaseDTO)contrato);
            var nombramientos = logicaNombramiento.DescargarNombramiento(cedula);
            listanombramiento.Add(nombramientos);
            var calificaciones = logicaCalificacion.DescargarCalificacionesCedula(cedula) != null ? logicaCalificacion.DescargarCalificacionesCedula(cedula) : new List<CBaseDTO>();
            //var puestos = new List<CBaseDTO> { nombramientos.Puesto };
            var puestos = logicaPuesto.CargarPuestoActivo(cedula);
            var ubicacionesAdmin = logicaUbAdministrativa.DescargarUbicacionAdministrativa(cedula);
            var ubicaciones = logicaUbicacion.CargarUbicacionPuesto(cedula);


            resultado.Add(funcionarioLista);
            resultado.Add(historialEstadoCivil);
            resultado.Add(datosContacto);
            resultado.Add(direcciones);
            resultado.Add(listacontratos);
            resultado.Add(listanombramiento);
            resultado.Add(calificaciones);
            resultado.Add(puestos);
            resultado.Add(ubicacionesAdmin);
            foreach (var item in ubicaciones)
            {
                resultado.Add(item);
            }


            return resultado;
        }


        public CBaseDTO BuscarFuncionarioPuntosCarreraProfesional(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioPuntosCarreraProfesional(cedula);
        }

        public List<CBaseDTO> ListarTiposContacto()
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.ListarTipoContacto();
       
        }

        public List<CFuncionarioDTO> BuscarFuncionarioCompleto(string cedula, string nombre, string apellido1, string apellido2,
                                             string codpuesto, string codclase, string codespecialidad,
                                             string codocupacion, string coddivision, string coddireccion, string coddepartamento,
                                             string codseccion, string codpresupuesto)
        {
            return new CFuncionarioL().BuscarFuncionarioParam(cedula, nombre, apellido1, apellido2, codpuesto, codclase, codespecialidad, codocupacion, coddivision, coddireccion,
                                                              coddepartamento, codseccion, codpresupuesto);
        }

        public List<CFuncionarioDTO> BuscarFuncionarioCompletoPP(string cedula, string nombre, string apellido1, string apellido2,
                                         string codpuesto, string codclase, string codespecialidad,
                                         int codnivel, string coddivision, string coddireccion, string coddepartamento,
                                         string codseccion, string codpresupuesto, string codestado, string codcostos)
        {
            return new CFuncionarioL().BuscarFuncionarioParam(cedula, nombre, apellido1, apellido2, codpuesto, codclase, codespecialidad, codnivel, coddivision, coddireccion,
                                                              coddepartamento, codseccion, codpresupuesto, codestado, codcostos);
        }

        public CBaseDTO BuscarFuncionarioNuevo(string cedula)
        {
            return new CFuncionarioL().BuscarFuncionarioNuevo(cedula);
        }

        public CBaseDTO ActualizarInformacionBasicaFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion,
                                                       List<CInformacionContactoDTO> contacto, CHistorialEstadoCivilDTO historial)
        {
            return new CFuncionarioL().ActualizarInformacionBasicaFuncionario(funcionario, direccion, contacto, historial);
        }


        public List<CBaseDTO> BuscarInformacionBasicaFuncionario(string cedula)
        {
            return new CFuncionarioL().BuscarInformacionBasicaFuncionario(cedula);
        }

        //public List<CFuncionarioDTO> BuscarFuncionarioCompleto(string cedula, string nombre, string apellido1, string apellido2,
        //                                     string codpuesto, string codclase, string codespecialidad,
        //                                     string codocupacion, string coddivision, string coddireccion, string coddepartamento,
        //                                     string codseccion, string codpresupuesto)
        //{
        //    return new CFuncionarioL().BuscarFuncionarioParam(cedula, nombre, apellido1, apellido2, codpuesto, codclase, codespecialidad, codocupacion, coddivision, coddireccion,
        //                                                      coddepartamento, codseccion, codpresupuesto);
        //}

        public List<CEMUExfuncionarioDTO> BuscarExfuncionarioFiltros(string cedula, string nombre, string primerApellido, string segundoApellido, string codPuesto)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarExfuncionarioFiltros(cedula, nombre, primerApellido, segundoApellido, codPuesto);
        }

        public List<List<CBaseDTO>> BuscarDetalleFuncionarioPolicial(CDetalleContratacionDTO detalle, List<DateTime> fechas)
        {
            return new CDetalleContratacionL().BuscarFuncionarioPolicial(detalle, fechas);
        }


        //Obtener calificacion actual de funcionario
        public CBaseDTO CargarCalificacionActual(string cedula)
        {
            return new CFuncionarioL().CargarCalificacionActual(cedula);
        }
    }
}
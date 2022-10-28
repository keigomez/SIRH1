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

        public CBaseDTO BuscarEntidadFinanciera(int codigo)
        {
            CEntidadFinancieraL logica = new CEntidadFinancieraL();
            return logica.BuscarEntidadFinanciera(codigo);
        }       

        public CBaseDTO GuardarInformacionContacto(CFuncionarioDTO funcionario, CInformacionContactoDTO informacionContacto)
        {
            CInformacionContactoL logica = new CInformacionContactoL();
            return logica.GuardarInformacionContacto(funcionario,informacionContacto); 
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
            return logica.GuardarDetalleContratacion(detalle,cuenta); 
        }               

        public CBaseDTO GuardarDetalleNombramiento(CDetalleNombramientoDTO detalleNombramiento)
        {
            CDetalleNombramientoL logica = new CDetalleNombramientoL();
            return logica.GuardarDetalleNombramiento(detalleNombramiento);  
        }
        
        public List<CBaseDTO> GuardarDatosPersonalesFuncionario(CFuncionarioDTO funcionario, CHistorialEstadoCivilDTO estadoCivil, List<CInformacionContactoDTO> informacion)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.GuardarDatosPersonalesFuncionario(funcionario, estadoCivil, informacion);
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

        public List<CFuncionarioDTO> BuscarFuncionarioUbicacion(int division, int direccion, int departamento, int seccion)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.EnviarFuncionarioUbicacion(division, direccion, departamento, seccion);
        }

        public List<CBaseDTO> BuscarFuncionarioDesgloceSalarial(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDesgloceSalarial(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuesto(cedula);
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
            funcionarioLista.Add(datosGenerales);
            var historialEstadoCivil = logicaEstadoCivil.DescargarCatEstadoCivil(cedula);
            var datosContacto = logicaContacto.DescargarInformacionContacto(cedula);
            var direcciones = logicaDireccion.DescargarDireccion(cedula);
            var contrato = logicaContrato.DescargarDetalleContratacion(cedula);
            listacontratos.Add((CBaseDTO)contrato);
            var nombramientos = logicaNombramiento.DescargarNombramiento(cedula);
            listanombramiento.Add(nombramientos);
            var calificaciones = logicaCalificacion.DescargarCalificacionesCedula(cedula);
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
    }
}

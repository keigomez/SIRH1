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
    // NOTE: If you change the class name "CPuestoService" here, you must also update the reference to "CPuestoService" in App.config.
    public class CPuestoService : ICPuestoService
    {
        public List<List<CBaseDTO>> BuscarHistorialUbicacionTrabajo(string codPuesto)
        {
            return (new CUbicacionPuestoL()).BuscarHistorialUbicacionTrabajo(codPuesto); 
        }

        public CBaseDTO ModificarUbicacionPuesto(CPuestoDTO puesto, CUbicacionPuestoDTO ubicacion)
        {
            return (new CUbicacionPuestoL()).ModificarUbicacionPuesto(puesto, ubicacion);
        }

        public List<List<CBaseDTO>> GetLocalizacion(bool cantones, int canton, bool distritos, bool provincias, int provincia)
        {
            return (new CUbicacionPuestoL()).GetLocalizacion(cantones, canton, distritos, provincias, provincia);
        }

        public List<CBaseDTO> DescargarUbicacionTrabajoPedimento(string codPuesto)
        {
            CUbicacionPuestoL logica = new CUbicacionPuestoL();
            return logica.DescargarUbicacionTrabajoPedimento(codPuesto);
        }

        public List<CBaseDTO> ListarMotivosMovimiento()
        {
            CMotivoMovimientoL logica = new CMotivoMovimientoL();
            return logica.ListarMotivosMovimiento();
        }

        public List<CBaseDTO> DescargarPuestoPedimento(string pedimento)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DescargarPuestoPedimento(pedimento);
        }

        public List<CBaseDTO> DescargarPuestoVacante(string codigo)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DescargarPuestoVacante(codigo);
        }

        public List<CBaseDTO> DescargarPuestoCompleto(string codigo)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DescargarPuestoCompleto(codigo);
        }

        public List<CBaseDTO> ListarEntidadEducativa()
        {
            CEntidadEducativaL logica = new CEntidadEducativaL();
            return logica.ListarEntidadEducativa();
        }

        public List<List<CBaseDTO>> CargarUbicacionPuesto(string cedula)
        {
            CUbicacionPuestoL logica = new CUbicacionPuestoL();
            return logica.CargarUbicacionPuesto(cedula);
        }

        public List<CDireccionGeneralDTO> DescargarDireccionGenerals(int codigo, string nombre)
        {
            CDireccionGeneralL logica = new CDireccionGeneralL();
            return logica.DescargarDireccionGenerals(codigo, nombre);
        }

        public List<CDivisionDTO> DescargarDivisions(int codigo, string nombre)
        {
            CDivisionL logica = new CDivisionL();
            return logica.DescargarDivisions(codigo, nombre);
        }

        public List<CDepartamentoDTO> DescargarDepartamentos(int codigo, string nombre)
        {
            CDepartamentoL logica = new CDepartamentoL();
            return logica.DescargarDepartamentos(codigo, nombre);
        }

        public List<CClaseDTO> DescargarClases(int codigo, string nombre)
        {
            CClaseL logica = new CClaseL();
            return logica.DescargarClases(codigo, nombre);
        }

        public List<CBaseDTO> BuscarDatosPuesto(string codPuesto)
        {
            CAdministracionPuestoL logica = new CAdministracionPuestoL();
            return logica.BuscarDatosPuesto(codPuesto);
        }

        public List<CBaseDTO> DescargarUbicacionAdministrativa(string cedula)
        {
            CUbicacionAdministrativaL logica = new CUbicacionAdministrativaL();
            return logica.DescargarUbicacionAdministrativa(cedula);
        }

        public List<CSeccionDTO> DescargarSeccions(int codigo, string nombre)
        {
            CSeccionL logica = new CSeccionL();
            return logica.DescargarSeccions(codigo, nombre);
        }

        public List<COcupacionRealDTO> DescargarOcupacionReals(int codigo, string nombre)
        {
            COcupacionRealL logica = new COcupacionRealL();
            return logica.DescargarOcupacionReals(codigo, nombre);
        }

        public List<CEspecialidadDTO> DescargarEspecialidades(int codigo, string nombre)
        {
            CEspecialidadL logica = new CEspecialidadL();
            return logica.DescargarEspecialidades(codigo, nombre);
        }

        public List<CBaseDTO> ListarEntidadesGubernamentales()
        {
            CEntidadGubernamentalL logica = new CEntidadGubernamentalL();
            return logica.ListarEntidadesGubernamentales();
        }

        public CBaseDTO BuscarEntidadGubernamental(int codigo)
        {
            CEntidadGubernamentalL logica = new CEntidadGubernamentalL();
            return logica.BuscarEntidadGubernamental(codigo);
        }

        public List<CBaseDTO> ListarEntidadesAdscritas()
        {
            CEntidadAdscritaL logica = new CEntidadAdscritaL();
            return logica.ListarEntidadesAdscritas(); 
        }

        public CBaseDTO BuscarEntidadAdscrita(int codigo)
        {
            CEntidadAdscritaL logica = new CEntidadAdscritaL();
            return logica.BuscarEntidadAdscrita(codigo); 
        }

        public CMotivoMovimientoDTO CargarMotivoMovimientoPorPuesto(string NumeroPuesto)
        {
            CMotivoMovimientoL logica = new CMotivoMovimientoL();
            return logica.CargarMotivoMovimientoPorPuesto(NumeroPuesto);
        }

        public List<CMotivoMovimientoDTO> DescargarMotivoMovimientoCompleto()
        {
            CMotivoMovimientoL logica = new CMotivoMovimientoL();
            return logica.DescargarMotivoMovimientoCompleto();
        }

        public CMotivoMovimientoDTO CargarMotivoMovimientoPorCedula(string Cedula)
        {
            CMotivoMovimientoL logica = new CMotivoMovimientoL();
            return logica.CargarMotivoMovimientoPorCedula(Cedula);
        }

        public CTipoPuestoDTO RetornarTipoPuestoEspecifico(string codPuesto)
        {
            CTipoPuestoL logica = new CTipoPuestoL();
            return logica.RetornarTipoPuestoEspecifico(codPuesto);
        }

        public CBaseDTO GuardarTareasPuesto(CTareasPuestoDTO tareas)
        {
            CTareasPuestoL logica = new CTareasPuestoL();
            return logica.GuardarTareasPuesto(tareas);
        }

        public CBaseDTO BuscarTareasId(CTareasPuestoDTO tareasPuesto)
        {
            CTareasPuestoL logica = new CTareasPuestoL();
            return logica.BuscarTareasId(tareasPuesto);
        }

        public List<CBaseDTO> BuscarTareasCodPuesto(CTareasPuestoDTO tareas)
        {
            CTareasPuestoL logica = new CTareasPuestoL();
            return logica.BuscarTareasCodPuesto(tareas);
        }
        
        public CBaseDTO BuscarContenidoPresupuestario(CContenidoPresupuestarioDTO contenido)
        {
            CContenidoPresupuestarioL logica = new CContenidoPresupuestarioL();
            return logica.BuscarContenidoPresupuestario(contenido); 
        }

        public List<CBaseDTO> BuscarEstudiosPorPuesto(CEstudioPuestoDTO estudio)
        {
            CEstudioPuestoL logica = new CEstudioPuestoL();
            return logica.BuscarEstudiosPorPuesto(estudio);
        }

        public CBaseDTO BuscarEstudioPuestoPorPuesto(CEstudioPuestoDTO estudio)
        {
            CEstudioPuestoL logica = new CEstudioPuestoL();
            return logica.BuscarEstudioPuestoPorPuesto(estudio);
        }

        public CBaseDTO GuardarEstudioPuesto(CPuestoDTO puesto, CEstudioPuestoDTO estudio)
        {
            CEstudioPuestoL logica = new CEstudioPuestoL();
            return logica.GuardarEstudioPuesto(puesto, estudio);
        }

        public CBaseDTO GuardarPrestamoPuesto(CPuestoDTO puesto, CPrestamoPuestoDTO prestamo)
        {
            CPrestamoPuestoL logica = new CPrestamoPuestoL();
            return logica.GuardarPrestamoPuesto(puesto, prestamo);
        }

        public List<CBaseDTO> BuscarPrestamoPuestoPorPuesto(CPrestamoPuestoDTO prestamo)
        {
            CPrestamoPuestoL logica = new CPrestamoPuestoL();
            return logica.BuscarPrestamoPuestoPorPuesto(prestamo);
        }

        public List<CBaseDTO> DetallePuestoPorCodigo(string codPuesto)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DetallePuestoPorCodigo(codPuesto); 
        }

        public List<CBaseDTO> DetallePuestoPorCedula(string cedula)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DetallePuestoPorCedula(cedula);
        }

        public List<CBaseDTO> CargarPuestoActivo(string cedula)
        {
            CPuestoL logica = new CPuestoL();
            return logica.CargarPuestoActivo(cedula);
        }

        public List<CPuestoDTO> BuscarPuestoParams(string codpuesto, int clase, int especialidad, int ocupacion)
        {
            CPuestoL logica = new CPuestoL();
            return logica.BuscarPuestoParams(codpuesto, clase, especialidad, ocupacion);
        }        

        public List<CBaseDTO> BuscarTitulosFactorPorPuesto(CPuestoDTO puesto)
        {
            CTituloFactorL logica = new CTituloFactorL();
            return logica.BuscarTitulosFactorPorPuesto(puesto);   
        }

        public CBaseDTO BuscarTituloFactorId(CTituloFactorDTO tituloFactor)
        {
            CTituloFactorL logica = new CTituloFactorL();
            return logica.BuscarTituloFactorId(tituloFactor);
        }

        public CBaseDTO GuardarTituloFactor(CTituloFactorDTO titulo)
        {
            CTituloFactorL logica = new CTituloFactorL();
            return logica.GuardarTituloFactor(titulo);  
        }

        public CBaseDTO BuscarFactorId(CFactorDTO factor)
        {
            CFactorL logica = new CFactorL();
            return logica.BuscarFactorId(factor);
        }

        public CBaseDTO GuardarFactor(CTituloFactorDTO titulo, CFactorDTO factor)
        {
            CFactorL logica = new CFactorL();
            return logica.GuardarFactor(titulo,factor); 
        }        

        public List<CBaseDTO> GuardarCaracteristicasPuesto(CPuestoDTO puesto, CCaracteristicasPuestoDTO caracteristicas, CTareasPuestoDTO tareas, CFactorDTO factor, CTituloFactorDTO titulo)
        {
            CCaracteristicasPuestoL logica = new CCaracteristicasPuestoL();
            return logica.GuardarCaracteristicasPuesto(puesto,caracteristicas,tareas,factor,titulo);
        }

        public List<CBaseDTO> BuscarAddendumIdPrestamo(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum)
        {
            CAddendumPrestamoPuestoL logica = new CAddendumPrestamoPuestoL();
            return logica.BuscarAddendumIdPrestamo(prestamo, addendum);
        }

        public CBaseDTO BuscarAddendumPrestamoPuesto(CAddendumPrestamoPuestoDTO addendum)
        {
            CAddendumPrestamoPuestoL logica = new CAddendumPrestamoPuestoL();
            return logica.BuscarAddendumPrestamoPuesto(addendum);
        }

        public CBaseDTO GuardarNumAddendum(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum)
        {
            CAddendumPrestamoPuestoL logica = new CAddendumPrestamoPuestoL();
            return logica.GuardarNumAddendum(prestamo, addendum);
        }

        public CBaseDTO GuardarNumeroRescision(CRescisionDTO rescisionP, CPrestamoPuestoDTO prestamo)
        {
            CRescisionL logica = new CRescisionL();
            return logica.GuardarNumeroRescision(rescisionP, prestamo);
        }

        public CBaseDTO GuardarNumResolucion(CDetallePuestoDTO detallePuesto, CContenidoPresupuestarioDTO contenido)
        {
            CContenidoPresupuestarioL logica = new CContenidoPresupuestarioL();
            return logica.GuardarNumResolucion(detallePuesto,contenido);   
        }

        public List<CBaseDTO> BuscarPedimentosPorPuesto(CPuestoDTO puesto)
        {
            CPedimentoPuestoL logica = new CPedimentoPuestoL();
            return logica.BuscarPedimentosPorPuesto(puesto);
        }

        public CBaseDTO GuardarPedimentoPuesto(CPuestoDTO puesto, CPedimentoPuestoDTO pedimento)
        {
            CPedimentoPuestoL logica = new CPedimentoPuestoL();
            return logica.GuardarPedimentoPuesto(puesto,pedimento);  
        }       

        public CBaseDTO InsertarMovimientoPuesto(CMovimientoPuestoDTO movimiento)
        {
            CMovimientoPuestoL logica = new CMovimientoPuestoL();
            return logica.InsertarMovimientoPuesto(movimiento);  
        }

        public CBaseDTO GuardarMovimientoPuesto(CMovimientoPuestoDTO entidad)
        {
            CMovimientoPuestoL logica = new CMovimientoPuestoL();
            return logica.GuardarMovimientoPuesto(entidad);
        }

        public CMovimientoPuestoDTO RetornarMovimientoPuestoEspecifico(string numeroPuesto)
        {
            CMovimientoPuestoL logica = new CMovimientoPuestoL();
            return logica.RetornarMovimientoPuestoEspecifico(numeroPuesto);
        }

        public CMovimientoPuestoDTO RetornarMovimientoPuestoEspecificoOficio(string CodOficio)
        {
            CMovimientoPuestoL logica = new CMovimientoPuestoL();
            return logica.RetornarMovimientoPuestoEspecificoOficio(CodOficio);
        }

        //una lista de CBaseDTO buscar el puesto por codigo, por codigo de puesto
        public List<CBaseDTO> BuscarPuestoCodigo(string codPuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
          //instanciar a Lógica
            CPuestoL logica = new CPuestoL();
            CFuncionarioL logicaFuncionario = new CFuncionarioL();
            
            //puesto es igual a detalle de puesto por codigo en lógica, con el codigo de puesto
            var puesto = logica.DetallePuestoPorCodigo(codPuesto);
            // datos funcionario es igual a (ruta)logicaFuncionario en BuscarFuncionario por Cod de puesto y que me traiga el código de puesto.
            var datosFuncionario = logicaFuncionario.BuscarFuncionarioPorCodigoPuesto(codPuesto);
            //para cada item en datos funcionario
            foreach (var item in datosFuncionario)
            {
                //en la respuesta agregue el item
                respuesta.Add(item);
            }
            //para cada item en puesto
            foreach (var item in puesto)
            {
                respuesta.Add(item);
            }
            
            return respuesta;
        }

        //lista de CBaseDTO buscar el puesto por cedula de tipo string
        public List<CBaseDTO> BuscarPuestoCedula(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CPuestoL logica = new CPuestoL();
            CFuncionarioL logicaFuncionario = new CFuncionarioL();

            var puesto = logica.DetallePuestoPorCedula(cedula);

            var datosFuncionario = logicaFuncionario.BuscarFuncionarioNombramiento(cedula);

            foreach (var item in datosFuncionario)
            {
                respuesta.Add(item);
            }

            foreach (var item in puesto)
            {
                respuesta.Add(item);
            }

            return respuesta;
        }       

        public List<CClaseDTO> BuscarClaseParams(int codClase, string nomClase)
        {
            CClaseL logica = new CClaseL();
            return logica.DescargarClases(codClase, nomClase);
        }

        public List<CEspecialidadDTO> BuscarEspecialidadParams(int codEspecialidad, string nomEspecialidad)
        {
            CEspecialidadL logica = new CEspecialidadL();
            return logica.DescargarEspecialidades(codEspecialidad, nomEspecialidad);
        }

        public List<COcupacionRealDTO> BuscarOcupacionParams(int codOcupacion, string nomOcupacion)
        {
            COcupacionRealL logica = new COcupacionRealL();
            return logica.DescargarOcupacionReals(codOcupacion, nomOcupacion);
        }

        public List<CDivisionDTO> BuscarDivisionParams(int codigo, string nombre)
        {
            CDivisionL logica = new CDivisionL();
            return logica.DescargarDivisions(codigo, nombre);
        }

        public List<CDireccionGeneralDTO> BuscarDireccionGeneralParams(int codigo, string nombre)
        {
            CDireccionGeneralL logica = new CDireccionGeneralL();
            return logica.DescargarDireccionGenerals(codigo, nombre);
        }

        public List<CDepartamentoDTO> BuscarDepartamentoParams(int codigo, string nombre)
        {
            CDepartamentoL logica = new CDepartamentoL();
            return logica.DescargarDepartamentos(codigo, nombre);
        }

        public List<CSeccionDTO> BuscarSeccionParams(int codigo, string nombre)
        {
            CSeccionL logica = new CSeccionL();
            return logica.DescargarSeccions(codigo, nombre);
        }

        public List<List<CBaseDTO>> DescargarPerfilPuestoAcciones(string codPuesto)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DescargarPerfilPuestoAcciones(codPuesto);
        }

        public List<List<CBaseDTO>> DescargarPerfilPuestoAccionesFuncionario(string cedula)
        {
            CPuestoL logica = new CPuestoL();
            return logica.DescargarPerfilPuestoAccionesFuncionario(cedula);
        }
    }
}

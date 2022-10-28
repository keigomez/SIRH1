using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Logica;
using SIRH.DTO;

namespace SIRH.Servicios
{
    public class CMarcacionService : ICMarcacionService
    {

        public List<List<CBaseDTO>> ReporteConsolidadoPorFuncionario(CFuncionarioDTO funcionario, List<DateTime> fechas)
        {
            return (new CMarcacionesL()).ReporteConsolidadoPorFuncionario(fechas, funcionario);
        }

        public List<List<CBaseDTO>> ReporteConsolidadoPorDepartamento(CDepartamentoDTO departamento, List<DateTime> fechas)
        {
            return (new CMarcacionesL()).ReporteConsolidadoPorDepartamento(departamento, fechas);
        }

        public List<List<CBaseDTO>> ReporteMarcacionesPorDia(List<DateTime> fechas, CFuncionarioDTO funcionario)
        {
            return (new CMarcacionesL()).ReporteMarcacionesPorDia(fechas, funcionario);
        }

        public List<CBaseDTO> ObtenerJornadaFuncionario(CFuncionarioDTO funcionario)
        {
            CTipoJornadaL logica = new CTipoJornadaL();
            return logica.ObtenerJornadaFuncionario(funcionario);
        }

        public List<List<CBaseDTO>> ReporteFuncionariosPorDepartamento(CDepartamentoDTO departamento) 
        {
            return (new CMarcacionesL()).ReporteFuncionariosPorDepartamento(departamento);
        }

        public List<CBaseDTO> ListarDepartamentos() {
            return new CDepartamentoL().ListarDepartamentos();
        }



        //Dispositivos----------------------------------------------------
        public CBaseDTO BuscarDispositivo(CDispositivoDTO dispositivo)
        {
            CDispositivoL logica = new CDispositivoL();
            return logica.BuscarDispositivo(dispositivo);
        }

        public List<CBaseDTO> ListarDispositivos()
        {
            CDispositivoL logica = new CDispositivoL();
            return logica.ListarDispositivos();
        }

        //Empleados------------------------------------------------------

        public CBaseDTO AgregarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.AgregarEmpleado(empleado, funcionario);
        }

        public CBaseDTO DesactivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario,CDetalleNombramientoDTO detalleNombramiento, CMotivoBajaDTO motivoBaja)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.DesactivarEmpleado(empleado, funcionario,detalleNombramiento, motivoBaja);
        }

        public List<CBaseDTO> BuscarEmpleadoActivo(CEmpleadoDTO empleado)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.BuscarEmpleadoActivo(empleado);
        }

        public List<CBaseDTO> BuscarEmpleado(CEmpleadoDTO empleado)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.BuscarEmpleado(empleado);
        }

        public CBaseDTO ActivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario, CDetalleNombramientoDTO detalleNombramiento)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.ActivarEmpleado(empleado, funcionario, detalleNombramiento);
        }

        public List<CBaseDTO> SearchEmpleado(CEmpleadoDTO empleado, List<string> argumentos)
        {
            CEmpleadoL logica = new CEmpleadoL();
            return logica.SearchEmpleado(empleado,argumentos);
        }

        public CBaseDTO ObtenerDetalleNombramientoFuncionario(CFuncionarioDTO funcionario) {
            CDetalleNombramientoL logica = new CDetalleNombramientoL();
            return logica.ObtenerDetalleNombramientoFuncionario(funcionario);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuesto(cedula);
        }

        //EmpleadoDispositivo

        public CBaseDTO AsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo)
        {
            CEmpleadoDispositivoL logica = new CEmpleadoDispositivoL();
            return logica.AsignarRelojMarcador(empleadoDispositivo, empleado, dispositivo);
        }

        public CBaseDTO DesAsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo)
        {
            CEmpleadoDispositivoL logica = new CEmpleadoDispositivoL();
            return logica.DesAsignarRelojMarcador(empleadoDispositivo, empleado, dispositivo);
        }

        public List<CBaseDTO> BuscarDispositivosAsignados(CEmpleadoDTO empleado)
        {
            CEmpleadoDispositivoL logica = new CEmpleadoDispositivoL();
            return logica.BuscarDispositivosAsignados(empleado);
        }

        //Motivo Baja

        public CBaseDTO BuscarMotivoBaja(CMotivoBajaDTO motivoBaja)
        {
            CMotivoBajaL logica = new CMotivoBajaL();
            return logica.BuscarMotivoBaja(motivoBaja);
        }

        public List<CBaseDTO> ListarMotivoBaja()
        {
            CMotivoBajaL logica = new CMotivoBajaL();
            return logica.ListarMotivoBaja();
        }

    }
}

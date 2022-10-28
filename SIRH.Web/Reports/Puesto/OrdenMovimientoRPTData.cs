using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Puesto
{
    public class OrdenMovimientoRPTData
    {
        public string NumeroOrden { get; set; }
        public string Movimiento { get; set; }
        public string NumeroPuesto { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string ClasePuesto { get; set; }
        public string EspecialidadPuesto { get; set; }
        public string DivisionPuesto { get; set; }
        public string DireccionPuesto { get; set; }
        public string DepartamentoPuesto { get; set; }
        public string SeccionPuesto { get; set; }
        public string CodigoPresupuestario { get; set; }
        public string CuentaCliente { get; set; }
        public string Estado { get; set; }
        public string Rige { get; set; }
        public string Vence { get; set; }
        public string Partida { get; set; }
        public string OcupacionPuesto { get; set; }
        public string NumeroPedimento { get; set; }
        public string Motivo { get; set; }
        public string Sustituye { get; set; }
        public string Observaciones { get; set; }
        public string Responsable { get; set; }
        public string Revisado { get; set; }
        public string Jefatura { get; set; }
        public string CedulaResponsable { get; set; }
        public string CedulaRevisado { get; set; }
        public string CedulaJefatura { get; set; }
        public string FechaCertificacion { get; set; }
        public string Academica { get; set; }
        public string Experiencia { get; set; }
        public string Capacitacion { get; set; }
        public string Licencias { get; set; }
        public string Colegiaturas { get; set; }

        internal static OrdenMovimientoRPTData GenerarDatosReporte(OrdenMovimientoVM dato, int idDetalle, string filtros)
        {
            var fSustituye = "";
            var fResponsableCedula = "";
            var fResponsableNombre = "";
            var fJefaturaCedula = "";
            var fJefaturaNombre = "";

            if (dato.Orden.FuncionarioSustituido.Cedula != null && dato.Orden.FuncionarioSustituido.Nombre != null)
            {
                fSustituye = dato.Orden.FuncionarioSustituido.Cedula + " " +
                            dato.Orden.FuncionarioSustituido.Nombre.TrimEnd() + " " +
                            dato.Orden.FuncionarioSustituido.PrimerApellido.TrimEnd() + " " +
                            dato.Orden.FuncionarioSustituido.SegundoApellido.TrimEnd();
            }


            if(dato.Orden.FuncionarioRevision.Cedula != null && dato.Orden.FuncionarioRevision.Nombre != null)
            {
                fResponsableCedula = dato.Orden.FuncionarioRevision.Cedula;
                fResponsableNombre = dato.Orden.FuncionarioRevision.Nombre.TrimEnd() + " " +
                            dato.Orden.FuncionarioRevision.PrimerApellido.TrimEnd() + " " +
                            dato.Orden.FuncionarioRevision.SegundoApellido.TrimEnd();
            }

            if (dato.Orden.FuncionarioJefatura.Cedula != null && dato.Orden.FuncionarioJefatura.Nombre != null)
            {
                fJefaturaCedula = dato.Orden.FuncionarioJefatura.Cedula;
                fJefaturaNombre = dato.Orden.FuncionarioJefatura.Nombre.TrimEnd() + " " +
                            dato.Orden.FuncionarioJefatura.PrimerApellido.TrimEnd() + " " +
                            dato.Orden.FuncionarioJefatura.SegundoApellido.TrimEnd();
            }

            return new OrdenMovimientoRPTData
            {
                NumeroOrden = dato.Orden.NumOrden != null ? dato.Orden.NumOrden : "",
                Movimiento = dato.Orden.TipoMovimiento.DesMotivo,
                NumeroPuesto = dato.Puesto.CodPuesto,
                Nombre = dato.Orden.FuncionarioOrden.Nombre.TrimEnd() + " " +
                            dato.Orden.FuncionarioOrden.PrimerApellido.TrimEnd() + " " +
                            dato.Orden.FuncionarioOrden.SegundoApellido.TrimEnd(),
                Cedula = dato.Orden.FuncionarioOrden.Cedula,
                ClasePuesto = dato.Puesto.DetallePuesto.Clase.DesClase.TrimEnd(),
                EspecialidadPuesto = dato.Puesto.DetallePuesto.Especialidad.DesEspecialidad != null ? dato.Puesto.DetallePuesto.Especialidad.DesEspecialidad.TrimEnd() :"",
                DivisionPuesto = dato.Puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd(),
                DireccionPuesto = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != null ? dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd(): "",
                DepartamentoPuesto = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null ? dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "",
                SeccionPuesto = dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion != null ? dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() :"",
                CodigoPresupuestario = dato.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto,
                CuentaCliente = dato.Orden.CuentaCliente,
                Estado = dato.Orden.Estado.DesEstado,
                Rige = dato.Orden.FecRige.ToShortDateString(),
                //Vence = dato.FechaVence != null ? dato.FechaVence.ToShortDateString() : "",
                Vence = dato.Orden.FecVence != null ? Convert.ToDateTime(dato.Orden.FecVence).ToShortDateString() : "",
                Partida = dato.Orden.DesPartidaPresupuestaria,
                OcupacionPuesto = dato.Puesto.DetallePuesto.OcupacionReal.DesOcupacionReal.TrimEnd(),
                NumeroPedimento = dato.Orden.Pedimento.NumeroPedimento != null ? dato.Orden.Pedimento.NumeroPedimento : "",
                Motivo = dato.Orden.MotivoMovimiento.DesMotivo,
                Sustituye = fSustituye,
                Observaciones = dato.Orden.DesObservaciones,
                Responsable = dato.Orden.FuncionarioResponsable.Nombre.TrimEnd() + " " +
                            dato.Orden.FuncionarioResponsable.PrimerApellido.TrimEnd() + " " +
                            dato.Orden.FuncionarioResponsable.SegundoApellido.TrimEnd(),
                CedulaResponsable = dato.Orden.FuncionarioResponsable.Cedula,
                Revisado = fResponsableNombre,
                CedulaRevisado = fResponsableCedula,
                Jefatura = fJefaturaNombre,
                CedulaJefatura = fJefaturaCedula,
                FechaCertificacion = dato.Declaracion.FechaCertificacion.ToShortDateString(),
                Academica = dato.Declaracion.Academica,
                Experiencia = dato.Declaracion.Experiencia,
                Capacitacion = dato.Declaracion.Capacitacion,
                Licencias = dato.Declaracion.Licencias,
                Colegiaturas = dato.Declaracion.Colegiaturas
            };
        }
    }
}
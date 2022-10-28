using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.Web.ViewModels;
using SIRH.Web.Helpers;

namespace SIRH.Web.Reports.Puesto
{

    public class PuestosRptData
    {
        public string Usuario { get; set; }
        public string Puesto { get; set; }
        public string Clase { get; set; }
        public string Especialidad { get; set; }
        public string OcupacionReal { get; set; }
        public string CodPresupuestario { get; set; }
        public string CodPolicial { get; set; }
        public string NivelOcupacional { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Seccion { get; set; }
        public string EstadoNombramiento { get; set; }
        public string EstadoPuesto { get; set; }
        public string Observaciones { get; set; }
        public string CedulaPropietario { get; set; }
        public string NombrePropietario { get; set; }
        public string CedulaOcupante { get; set; }
        public string NombreOcupante { get; set; }

        #region FILTROS BÚSQUEDA PUESTO
        public string FCedula { get; set; }
        public string FNombre { get; set; }
        public string FApellido1 { get; set; }
        public string FApellido2 { get; set; }
        public string FCodPuesto { get; set; }
        public string FClase { get; set; }
        public string FEspecialidad { get; set; }
        public string FNivelOcupacional { get; set; }
        public string FDivision { get; set; }
        public string FDireccionGeneral { get; set; }
        public string FDepartamento { get; set; }
        public string FSeccion { get; set; }
        public string FPresupuesto { get; set; }
        public string FCentroCostos { get; set; }
        public string Agrupacion { get; set; }
        #endregion
     
        internal static PuestosRptData GenerarDatosReportePorPuestos(PerfilFuncionarioVM dato)
        {
            return new PuestosRptData
            {
                Puesto = dato.Puesto.CodPuesto != null ? dato.Puesto.CodPuesto.TrimEnd().ToString() : "",
                Clase = dato.DetallePuesto.Clase != null ? dato.DetallePuesto.Clase.DesClase.TrimEnd().ToString() : "",
                Especialidad = dato.DetallePuesto.Especialidad != null ? dato.DetallePuesto.Especialidad.DesEspecialidad.TrimEnd().ToString() : "",
                OcupacionReal = dato.DetallePuesto.OcupacionReal != null ? dato.DetallePuesto.OcupacionReal.DesOcupacionReal.TrimEnd().ToString() : "",
                CodPresupuestario = dato.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto.ToString(),
                Division = dato.Puesto.UbicacionAdministrativa.Division != null ? dato.Puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd().ToString() : "",
                Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral != null ?
                                    dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != null ?
                                        dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd().ToString() 
                                        : ""
                                    :"",
                Departamento = dato.Puesto.UbicacionAdministrativa.Departamento != null ?
                                    dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null ?
                                        dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd().ToString() 
                                        : ""
                                    : "",
                Seccion = dato.Puesto.UbicacionAdministrativa.Seccion != null ? dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd().ToString() : "",
                EstadoPuesto = dato.Puesto.EstadoPuesto != null ? dato.Puesto.EstadoPuesto.DesEstadoPuesto.TrimEnd() : "",
                CedulaPropietario = dato.FuncionarioPropietario != null ? dato.FuncionarioPropietario.Funcionario.Cedula.TrimEnd().ToString() : "",
                NombrePropietario = dato.FuncionarioPropietario != null ?
                                        dato.FuncionarioPropietario.Funcionario.Nombre.TrimEnd() + " " + dato.FuncionarioPropietario.Funcionario.PrimerApellido.TrimEnd() + " " + dato.FuncionarioPropietario.Funcionario.SegundoApellido.TrimEnd()
                                        : "",
                CedulaOcupante = dato.FuncionarioOcupante != null ? dato.FuncionarioOcupante.Funcionario.Cedula.TrimEnd().ToString() : "",
                NombreOcupante = dato.FuncionarioOcupante != null ? 
                                        dato.FuncionarioOcupante.Funcionario.Nombre.TrimEnd() + " " + dato.FuncionarioOcupante.Funcionario.PrimerApellido.TrimEnd() + " " + dato.FuncionarioOcupante.Funcionario.SegundoApellido.TrimEnd()
                                        :"",
            };
        }
    }
}
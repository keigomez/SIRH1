using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.Web.ViewModels;
using SIRH.Web.Helpers;

namespace SIRH.Web.Reports.Funcionarios
{

    public class FuncionariosRptData
    {
        public string Cedula { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Puesto { get; set; }
        public string Clase { get; set; }
        public string Especialidad { get; set; }
        public string OcupacionReal { get; set; }
        public string Subespecialidad { get; set; }
        public string CodPresupuestario { get; set; }
        public string CodPolicial { get; set; }
        public string NivelOcupacional { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Seccion { get; set; }
        //********************************************Reporte Funcionario*********************************************
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string EstadoFuncionario { get; set; }
        public string TipoContacto { get; set; }
        public string Detalle { get; set; }
        public string DetalleAdicional { get; set; }
        public string ProvinciaDomicilio { get; set; }
        public string CantonDomicilio { get; set; }
        public string DistritoDomicilio { get; set; }
        public string DireccionDomicilio { get; set; }
        public string MesAumento { get; set; }
        public string CantAnualidades { get; set; }
        public string EstadoNombramiento { get; set; }
        public string Calificacion { get; set; }
        public string EstadoPuesto { get; set; }
        public string SalarioBase { get; set; }
        public string MontoAumento { get; set; }
        public string ProvinciaContrato { get; set; }
        public string CantonContrato { get; set; }
        public string DistritoContrato { get; set; }
        public string ProvinciaTrabajo { get; set; }
        public string CantonTrabajo { get; set; }
        public string DistritoTrabajo { get; set; }
        public string Observaciones { get; set; }

        #region FILTROS BUSQUEDA FUNCIONARIO
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

        //********************************************Reporte Funcionario Dinamico*********************************************
        #region VARIBLES REPORTE DINAMICO FUNCIONARIO
        public int AceptEdad { get; set; }
        public int AceptCodigo { get; set; }
        public int AceptDivision { get; set; }
        public int AceptDireccion { get; set; }
        public int AceptDepartamento { get; set; }
        public int AceptSeccion { get; set; }
        public int AceptClase { get; set; }
        public int AceptEspecialidad { get; set; }
        public int AceptFecha { get; set; }
        public int AceptContrato { get; set; }
        public int AceptTrabajo { get; set; }
        public int AceptOcupacion { get; set; }
        public int AceptNivel { get; set; }
        public int AceptPolicial { get; set; }
        public int AceptGenero { get; set; }
        public string Campo3 { get; set; }
        public string Campo4 { get; set; }
        public string Campo5 { get; set; }
        public string Campo6 { get; set; }
        public string Campo7 { get; set; }
        public string Campo8 { get; set; }
        public string Campo9 { get; set; }
        public string Campo10 { get; set; }
        public string Campo11 { get; set; }
        public string Campo12 { get; set; }
        public string Campo13 { get; set; }
        public string Campo14 { get; set; }
        public string Campo15 { get; set; }
        public string Campo16 { get; set; }
        public string Campo17 { get; set; }

        #endregion

        internal static FuncionariosRptData GenerarDatosReportePorFuncionarios(PerfilFuncionarioVM funcionario)
        {
            return new FuncionariosRptData
            {
                Cedula = funcionario.Funcionario.Cedula != null ? funcionario.Funcionario.Cedula.TrimEnd().ToString() : "",
                Nombre = funcionario.Funcionario.Nombre.TrimEnd() + " " + funcionario.Funcionario.PrimerApellido.TrimEnd() + " " + funcionario.Funcionario.SegundoApellido.TrimEnd(),
                FechaIngreso = funcionario.DetalleContrato.FechaIngreso != null ? funcionario.DetalleContrato.FechaIngreso.ToShortDateString() : "",
                FechaNacimiento = funcionario.Funcionario.FechaNacimiento != null ? funcionario.Funcionario.FechaNacimiento.ToShortDateString() : "",
                Puesto = funcionario.Puesto.CodPuesto != null ? funcionario.Puesto.CodPuesto.TrimEnd().ToString() : "",
                Clase = funcionario.DetallePuesto.Clase != null ? funcionario.DetallePuesto.Clase.DesClase.TrimEnd().ToString() : "",
                Especialidad = funcionario.DetallePuesto.Especialidad != null ? funcionario.DetallePuesto.Especialidad.DesEspecialidad.TrimEnd().ToString() : "",
                OcupacionReal = funcionario.DetallePuesto.OcupacionReal != null ? funcionario.DetallePuesto.OcupacionReal.DesOcupacionReal.TrimEnd().ToString() : "",
                CodPresupuestario = funcionario.Puesto.UbicacionAdministrativa.Presupuesto != null ? funcionario.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto : "",
                Division = funcionario.Puesto.UbicacionAdministrativa.Division.NomDivision != null ? funcionario.Puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd().ToString() : "",
                Direccion = funcionario.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != null ? funcionario.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd().ToString() : "",
                Departamento = funcionario.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null ? funcionario.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd().ToString() : "",
                Seccion = funcionario.Puesto.UbicacionAdministrativa.Seccion.NomSeccion != null ? funcionario.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd().ToString() : ""
            };
        }
        internal static FuncionariosRptData GenerarDatosReporteFuncionario(PerfilFuncionarioVM funcionario)
        {
            
            return new FuncionariosRptData
            {
                Cedula = funcionario.Funcionario.Cedula?.ToString(),
                Nombre = funcionario.Funcionario.Nombre?.TrimEnd() + " " + funcionario.Funcionario.PrimerApellido?.TrimEnd() + " " + funcionario.Funcionario.SegundoApellido?.TrimEnd(),
                FechaIngreso = funcionario.DetalleContrato?.FechaIngreso != null ? funcionario.DetalleContrato.FechaIngreso.ToShortDateString() : "",
                FechaNacimiento = funcionario.Funcionario.FechaNacimiento == DateTime.MinValue ? "" : funcionario.Funcionario.FechaNacimiento.ToShortDateString(),
                Puesto = funcionario.Puesto.CodPuesto.ToString(),
                Clase = funcionario.DetallePuesto?.Clase?.DesClase?.ToString(),
                Subespecialidad = funcionario.DetallePuesto?.SubEspecialidad?.DesSubEspecialidad?.ToString(),
                Especialidad = funcionario.DetallePuesto?.Especialidad?.DesEspecialidad?.ToString(),
                OcupacionReal = funcionario.DetallePuesto?.OcupacionReal?.DesOcupacionReal?.ToString(),
                CodPresupuestario = funcionario.Puesto.UbicacionAdministrativa?.Presupuesto?.CodigoPresupuesto?.ToString(),
                Division = funcionario.Puesto.UbicacionAdministrativa?.Division?.NomDivision?.ToString(),
                Direccion = funcionario.Puesto.UbicacionAdministrativa?.DireccionGeneral?.NomDireccion?.ToString(),
                Departamento = funcionario.Puesto.UbicacionAdministrativa?.Departamento?.NomDepartamento?.ToString(),
                Seccion=funcionario.Puesto.UbicacionAdministrativa?.Seccion?.NomSeccion?.ToString(),
                Genero = funcionario.Funcionario.Cedula == null ? "" : funcionario.Funcionario.Sexo.ToString(),
                EstadoCivil = funcionario.EstadoCivil?.OrderBy(Q => Q.FecFin)?.LastOrDefault()?.CatEstadoCivil?.DesEstadoCivil?.ToString(),
                EstadoFuncionario = funcionario.Funcionario.EstadoFuncionario?.DesEstadoFuncionario?.ToString(),
                TipoContacto = funcionario.InformacionContacto?.LastOrDefault()?.TipoContacto?.DesTipoContacto?.ToString(),
                Detalle = funcionario.InformacionContacto?.LastOrDefault()?.DesContenido?.ToString(),
                DetalleAdicional = funcionario.InformacionContacto?.LastOrDefault()?.DesAdicional?.ToString(),
                ProvinciaDomicilio = funcionario.Direccion?.Distrito?.Canton?.Provincia?.NomProvincia?.ToString(),
                CantonDomicilio = funcionario.Direccion?.Distrito?.Canton?.NomCanton?.ToString(),
                DistritoDomicilio = funcionario.Direccion?.Distrito?.NomDistrito?.ToString(),
                DireccionDomicilio = funcionario.Direccion?.DirExacta?.ToString(),
                MesAumento = funcionario.DetalleContrato?.FechaMesAumento?.ToString(),
                CantAnualidades = funcionario.DetalleContrato?.NumeroAnualidades != null ? funcionario.DetalleContrato?.NumeroAnualidades.ToString() : "",
                EstadoNombramiento = funcionario.Nombramiento?.EstadoNombramiento?.DesEstado?.ToString(),
                Calificacion = funcionario.Calificaciones?.OrderBy(Q => Q.Periodo?.IdEntidad)?.LastOrDefault()?.CalificacionDTO?.DesCalificacion?.ToString(),
                EstadoPuesto = funcionario.Puesto.EstadoPuesto?.DesEstadoPuesto?.ToString(),
                SalarioBase = funcionario.DetallePuesto.EscalaSalarial?.SalarioBase != null ? funcionario.DetallePuesto.EscalaSalarial?.SalarioBase.ToString() : "",
                MontoAumento = funcionario.DetallePuesto.EscalaSalarial?.MontoAumentoAnual != null ? funcionario.DetallePuesto.EscalaSalarial?.MontoAumentoAnual.ToString() : "",
                ProvinciaContrato = funcionario.UbicacionContrato?.Distrito?.Canton?.Provincia?.NomProvincia?.ToString(),
                CantonContrato = funcionario.UbicacionContrato?.Distrito?.Canton?.NomCanton?.ToString(),
                DistritoContrato = funcionario.UbicacionContrato?.Distrito?.NomDistrito?.ToString(),
                ProvinciaTrabajo = funcionario.UbicacionTrabajo?.Distrito?.Canton?.Provincia?.NomProvincia?.ToString(),
                CantonTrabajo = funcionario.UbicacionTrabajo?.Distrito?.Canton?.NomCanton?.ToString(),
                DistritoTrabajo = funcionario.UbicacionTrabajo?.Distrito?.NomDistrito?.ToString(),
                Observaciones = funcionario.UbicacionTrabajo?.ObsUbicacionPuesto?.ToString()
            };
        }

        /*
        [0] = Cedula *
        [1] = Nombre * 
        [2] = Puesto * 
        [3] = Codigo
        [4] = Division
        [5] = Direccion
        [6] = Departamento
        [7] = Seccion
        [8] = Clase
        [9] = Especialidad
        [10] = Fecha
        [11] = Edad
        [12] = Contrato
        [13] = Trabajo
        [14] = Ocupacion
        [15] = Nivel
        [16] = Policial
        [17] = Genero
        */
        internal static FuncionariosRptData GenerarDatosReporteFuncionarioParam(PerfilFuncionarioVM funcionario, List<bool> opcionesMostrar, List<string> filtros, string grupo)
        {
            FuncionariosRptData fun = new FuncionariosRptData
            {
                Cedula = funcionario.Funcionario.Cedula != null ? funcionario.Funcionario.Cedula.TrimEnd().ToString() : "",
                Nombre = funcionario.Funcionario.Nombre.TrimEnd() + " " + funcionario.Funcionario.PrimerApellido.TrimEnd() + " " + funcionario.Funcionario.SegundoApellido.TrimEnd(),
                Puesto = funcionario.Puesto.CodPuesto != null ? funcionario.Puesto.CodPuesto.TrimEnd().ToString() : "",
                Usuario = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
            };

            #region FILTROS
            fun.FCedula = filtros[0];
            fun.FNombre = filtros[1];
            fun.FApellido1 = filtros[2];
            fun.FApellido2 = filtros[3];
            fun.FCodPuesto = filtros[4];
            fun.FClase = filtros[5];
            fun.FEspecialidad = filtros[6];
            fun.FNivelOcupacional = filtros[7];
            fun.FDivision = filtros[8];
            fun.FDireccionGeneral = filtros[9];
            fun.FDepartamento = filtros[10];
            fun.Seccion = filtros[11];
            fun.FPresupuesto = filtros[12];
            fun.FCentroCostos = filtros[13];
            fun.Agrupacion = grupo;
            #endregion

            #region OPCIONES A MOSTRAR
            fun.AceptCodigo = Convert.ToInt32(opcionesMostrar[3]);
            fun.AceptDivision = Convert.ToInt32(opcionesMostrar[4]);
            fun.AceptDireccion = Convert.ToInt32(opcionesMostrar[5]);
            fun.AceptDepartamento = Convert.ToInt32(opcionesMostrar[6]);
            fun.AceptSeccion = Convert.ToInt32(opcionesMostrar[7]);
            fun.AceptClase = Convert.ToInt32(opcionesMostrar[8]);
            fun.AceptEspecialidad = Convert.ToInt32(opcionesMostrar[9]);
            fun.AceptFecha = Convert.ToInt32(opcionesMostrar[10]);
            fun.AceptEdad = Convert.ToInt32(opcionesMostrar[11]);
            fun.AceptContrato = Convert.ToInt32(opcionesMostrar[12]);
            fun.AceptTrabajo = Convert.ToInt32(opcionesMostrar[13]);
            fun.AceptOcupacion = Convert.ToInt32(opcionesMostrar[14]);
            fun.AceptNivel = Convert.ToInt32(opcionesMostrar[15]);
            fun.AceptPolicial = Convert.ToInt32(opcionesMostrar[16]);
            fun.AceptGenero = Convert.ToInt32(opcionesMostrar[17]);
            #endregion

            #region VARIABLES BOOL SELECCIONADA
            bool codigo = false;
            bool division = false;
            bool direccion = false;
            bool departamento = false;
            bool seccion = false;
            bool clase = false;
            bool especialidad = false;
            bool fecha = false;
            bool contrato = false;
            bool trabajo = false;
            bool ocupacion = false;
            bool nivel = false;
            bool policial = false;
            bool genero = false;
            #endregion

            #region CAMPO 3
            if (opcionesMostrar[11])
            {
                fun.Campo3 = funcionario.Funcionario.FechaNacimiento != null ? GetEdad(funcionario.Funcionario.FechaNacimiento).ToString() : "";
            }
            else if (opcionesMostrar[17])
            {
                fun.Campo3 = funcionario.Funcionario.Sexo.ToString();
                genero = true;
            }
            else if (opcionesMostrar[10])
            {
                fun.Campo3 = funcionario.DetalleContrato.FechaIngreso != null ? funcionario.DetalleContrato.FechaIngreso.ToShortDateString() : "";
                fecha = true;
            }
            else if (opcionesMostrar[4])
            {
                fun.Campo3 = funcionario.DivisionPuesto != null ? funcionario.DivisionPuesto.NomDivision.TrimEnd().ToString() : "";
                division = true;
            }
            else if (opcionesMostrar[5])
            {
                fun.Campo3 = funcionario.DireccionGeneralPuesto != null ? funcionario.DireccionGeneralPuesto.NomDireccion.TrimEnd().ToString() : "";
                direccion = true;
            }
            else if (opcionesMostrar[6])
            {
                fun.Campo3 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
                departamento = true;
            }
            else if (opcionesMostrar[7])
            {
                fun.Campo3 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8])
            {
                fun.Campo3 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9])
            {
                fun.Campo3 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14])
            {
                fun.Campo3 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3])
            {
                fun.Campo3 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15])
            {
                fun.Campo3 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16])
            {
                fun.Campo3 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12])
            {
                fun.Campo3 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "") 
                    +  (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13])
            {
                fun.Campo3 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "") 
                    +  (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo3 = "";
            }
            #endregion
            #region CAMPO 4
            if (opcionesMostrar[17] && !genero)
            {
                fun.Campo4 = funcionario.Funcionario.Sexo.ToString();
            }
            else if (opcionesMostrar[10] && !fecha)
            {
                fun.Campo4 = funcionario.DetalleContrato.FechaIngreso != null ? funcionario.DetalleContrato.FechaIngreso.ToShortDateString() : "";
                fecha = true;
            }
            else if (opcionesMostrar[4] && !division)
            {
                fun.Campo4 = funcionario.DivisionPuesto != null ? funcionario.DivisionPuesto.NomDivision.TrimEnd().ToString() : "";
                division = true;
            }
            else if (opcionesMostrar[5] && !direccion)
            {
                fun.Campo4 = funcionario.DireccionGeneralPuesto != null ? funcionario.DireccionGeneralPuesto.NomDireccion.TrimEnd().ToString() : "";
                direccion = true;
            }
            else if (opcionesMostrar[6] && !departamento)
            {
                fun.Campo4 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
                departamento = true;
            }
            else if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo4 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo4 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo4 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo4 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo4 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo4 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo4 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo4 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo4 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo4 = "";
            }
            #endregion
            #region CAMPO 5
            if (opcionesMostrar[10] && !fecha)
            {
                fun.Campo5 = funcionario.DetalleContrato.FechaIngreso != null ? funcionario.DetalleContrato.FechaIngreso.ToShortDateString() : "";
            }
            else if (opcionesMostrar[4] && !division)
            {
                fun.Campo5 = funcionario.DivisionPuesto != null ? funcionario.DivisionPuesto.NomDivision.TrimEnd().ToString() : "";
                division = true;
            }
            else if (opcionesMostrar[5] && !direccion)
            {
                fun.Campo5 = funcionario.DireccionGeneralPuesto != null ? funcionario.DireccionGeneralPuesto.NomDireccion.TrimEnd().ToString() : "";
                direccion = true;
            }
            else if (opcionesMostrar[6] && !departamento)
            {
                fun.Campo5 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
                departamento = true;
            }
            else if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo5 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo5 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo5 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo5 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo5 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo5 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo5 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo5 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo5 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo5 = "";
            }
            #endregion
            #region CAMPO 6
            if (opcionesMostrar[4] && !division)
            {
                fun.Campo6 = funcionario.DivisionPuesto != null ? funcionario.DivisionPuesto.NomDivision.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[5] && !direccion)
            {
                fun.Campo6 = funcionario.DireccionGeneralPuesto != null ? funcionario.DireccionGeneralPuesto.NomDireccion.TrimEnd().ToString() : "";
                direccion = true;
            }
            else if (opcionesMostrar[6] && !departamento)
            {
                fun.Campo6 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
                departamento = true;
            }
            else if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo6 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo6 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo6 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo6 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo6 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo6 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo6 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo6 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo6 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo6 = "";
            }
            #endregion
            #region CAMPO 7
            if (opcionesMostrar[5] && !direccion)
            {
                fun.Campo7 = funcionario.DireccionGeneralPuesto != null ? funcionario.DireccionGeneralPuesto.NomDireccion.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[6] && !departamento)
            {
                fun.Campo7 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
                departamento = true;
            }
            else if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo7 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo7 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo7 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo7 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo7 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo7 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo7 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo7 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo7 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo7 = "";
            }
            #endregion
            #region CAMPO 8
            if (opcionesMostrar[6] && !departamento)
            {
                fun.Campo8 = funcionario.DepartamentoPuesto != null ? funcionario.DepartamentoPuesto.NomDepartamento.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo8 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
                seccion = true;
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo8 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo8 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo8 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo8 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo8 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo8 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo8 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo8 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo8 = "";
            }
            #endregion
            #region CAMPO 9
            if (opcionesMostrar[7] && !seccion)
            {
                fun.Campo9 = funcionario.SeccionPuesto != null ? funcionario.SeccionPuesto.NomSeccion.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[8] && !clase)
            {
                fun.Campo9 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
                clase = true;
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo9 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo9 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo9 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo9 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo9 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo9 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo9 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo9 = "";
            }
            #endregion
            #region CAMPO 10
            if (opcionesMostrar[8] && !clase)
            {
                fun.Campo10 = funcionario.ClasePuesto != null ? funcionario.ClasePuesto.DesClase.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo10 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
                especialidad = true;
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo10 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo10 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo10 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo10 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo10 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo10 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo10 = "";
            }
            #endregion
            #region CAMPO 11
            if (opcionesMostrar[9] && !especialidad)
            {
                fun.Campo11 = funcionario.EspecialidadPuesto != null ? funcionario.EspecialidadPuesto.DesEspecialidad.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo11 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
                ocupacion = true;
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo11 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo11 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo11 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo11 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo11 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo11 = "";
            }
            #endregion
            #region CAMPO 12
            if (opcionesMostrar[14] && !ocupacion)
            {
                fun.Campo12 = funcionario.OcupacionRealPuesto != null ? funcionario.OcupacionRealPuesto.DesOcupacionReal.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo12 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
                codigo = true;
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo12 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo12 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo12 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo12 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo12 = "";
            }
            #endregion
            #region CAMPO 13
            if (opcionesMostrar[3] && !codigo)
            {
                fun.Campo13 = funcionario.PresupuestoPuesto.CodigoPresupuesto != null ? funcionario.PresupuestoPuesto.CodigoPresupuesto.TrimEnd().ToString() : "";
            }
            else if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo13 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
                nivel = true;
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo13 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo13 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo13 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo13 = "";
            }
            #endregion
            #region CAMPO 14
            if (opcionesMostrar[15] && !nivel)
            {
                fun.Campo14 = funcionario.Puesto.NivelOcupacional != 0 ? NivelOcupacionalHelper.ObtenerNombre(funcionario.Puesto.NivelOcupacional).ToString() : "";
            }
            else if (opcionesMostrar[16] && !policial)
            {
                fun.Campo14 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
                policial = true;
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo14 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo14 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo14 = "";
            }
            #endregion
            #region CAMPO 15
            if (opcionesMostrar[16] && !policial)
            {
                fun.Campo15 = funcionario.DetalleContrato.CodigoPolicial != 0 ? funcionario.DetalleContrato.CodigoPolicial.ToString() : "";
            }
            else if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo15 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
                contrato = true;
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo15 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo15 = "";
            }
            #endregion
            #region CAMPO 16
            if (opcionesMostrar[12] && !contrato)
            {
                fun.Campo16 = (funcionario.ProvinciaContrato != null ? funcionario.ProvinciaContrato.NomProvincia : "")
                    + (funcionario.CantonContrato != null ? ", " + funcionario.CantonContrato.NomCanton : "")
                    + (funcionario.DistritoContrato != null ? ", " + funcionario.DistritoContrato.NomDistrito : "");
            }
            else if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo16 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
                trabajo = true;
            }
            else
            {
                fun.Campo16 = "";
            }
            #endregion
            #region CAMPO 17
            if (opcionesMostrar[13] && !trabajo)
            {
                fun.Campo17 = (funcionario.ProvinciaTrabajo != null ? funcionario.ProvinciaTrabajo.NomProvincia : "")
                    + (funcionario.CantonTrabajo != null ? ", " + funcionario.CantonTrabajo.NomCanton : "")
                    + (funcionario.DistritoTrabajo != null ? ", " + funcionario.DistritoTrabajo.NomDistrito : "");
            }
            else
            {
                fun.Campo17 = "";
            }
            #endregion
            return fun;
        }
        private static int GetEdad(DateTime fechaNacimiento)
        {
            var diff = DateTime.Now - fechaNacimiento;
            return (int)(diff.TotalDays / 365.255); ;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;
using System.Globalization;

namespace SIRH.Logica
{
    public class CServiciosGeneralesL
    { 
        #region Variables

        SIRHEntities contexto;

        List<int> listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 33, 35, 36, 37, 38, 39 };
        List<int> listaCodigoJefatura = new List<int> { 100000, 300000, 400000, 500000, 700000, 900000, 20226 };

        #endregion

        #region Constructor

        public CServiciosGeneralesL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        //Se registró en ICServiciosGeneralesService y CServiciosGeneralesService
        public string[] ConsultaFuncionario(string cedula)
        {
            string[] respuesta;

            try
            {
                if (cedula.Length == 10)
                {
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    CPuestoD intermedioPuesto = new CPuestoD(contexto);
                    CSalarioL intermedioSalario = new CSalarioL();
                    CFuncionarioL intermedioFunc = new CFuncionarioL();

                    var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);

                    if (funcionario != null)
                    {
                        //var nombramiento = funcionario.Nombramiento.OrderByDescending(N => N.FecRige).ToList();

                        //var nombramiento = funcionario.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).ToList();
                        var nombramiento = funcionario.Nombramiento.OrderByDescending(N => N.FecRige).ToList();
                        if (nombramiento.Count() == 0)
                            throw new Exception("El funcionario no cuenta con un Nombramiento válido");


                        var puntosCarrera = intermedioFunc.BuscarFuncionarioPuntosCarreraProfesional(cedula);

                        var salario = intermedioSalario.ObtenerSalario(cedula);
                 
                        if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            var codPuesto = ((CSalarioDTO)salario.ElementAt(1)).Puesto.CodPuesto;
                            //var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(nombramiento[0].Puesto.CodPuesto).Contenido;
                            var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(codPuesto).Contenido;
                            DetallePuesto datoDetalle = puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                            string centroCosto = ""; //Division (00)+ Dirección(000) + Departamento(000) + Sección(000)
                            centroCosto += puesto.UbicacionAdministrativa.Division.PK_Division.ToString().PadLeft(2, '0');
                            centroCosto += puesto.UbicacionAdministrativa.DireccionGeneral != null ? puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.ToString().PadLeft(3, '0') : "000";
                            centroCosto += puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString().PadLeft(3, '0') : "000";
                            centroCosto += puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString().PadLeft(3, '0') : "000";

                            var tipoRegimen = "SD";
                            if (datoDetalle.Clase != null)
                            {
                                if (Enumerable.Range(8303, 8506).Contains(datoDetalle.Clase.PK_Clase) || Enumerable.Range(14765, 14830).Contains(datoDetalle.Clase.PK_Clase))
                                {
                                    tipoRegimen = "2"; // POLICIAL
                                }
                                else
                                {
                                    tipoRegimen = "1"; // M.O.P.T
                                }
                            }

                            if (puesto.DetallePuesto.FirstOrDefault().Especialidad != null) {
                                if(puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad == 179)
                                    tipoRegimen = "179"; // CONFIANZA
                            }

                            var X = funcionario.IdCedulaFuncionario;
                            X = funcionario.NomFuncionario.TrimEnd();
                            X = funcionario.NomPrimerApellido.TrimEnd();
                            X = funcionario.NomSegundoApellido.TrimEnd();
                            X = puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString().PadLeft(3, '0') : "000";
                            X = puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : "SD";
                            X = puesto.UbicacionAdministrativa.DireccionGeneral != null ? puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.ToString().PadLeft(2, '0') : "00";
                            X = puesto.UbicacionAdministrativa.DireccionGeneral != null ? puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd().ToUpper() : "SD";
                            X = funcionario.EstadoFuncionario.DesEstadoFuncionario.TrimEnd().ToUpper();
                            X = datoDetalle.Clase != null ? datoDetalle.Clase.DesClase.TrimEnd() : "SD"; //puesto.DetallePuesto.FirstOrDefault().Clase.DesClase.TrimEnd();
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2, '0') : "SD";
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.Canton.Provincia.NomProvincia : "SD";
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.Canton.CodPostalCanton : "SD";
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.Canton.NomCanton : "SD";
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.CodPostalDistrito : "SD";
                            X = funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.NomDistrito : "SD";
                            X = puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd().ToUpper();
                            X = "SD"; //Subdivision;
                            X = puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "SD";
                            X = puesto.CodPuesto.TrimEnd();
                            X = funcionario.Direccion.Count > 0 ?
                                     funcionario.Direccion.FirstOrDefault().DirExacta != null ? funcionario.Direccion.FirstOrDefault().DirExacta.TrimEnd() : "SD"
                                      : "SD";
                            X = funcionario.DetalleContratacion.Count > 0 ? (funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal != null ? funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal.TrimEnd() : "SD") : "SD";// Ubicación Real;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2, '0') 
                                        : "00"
                                    : "00"; //Provincia-Contrato
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                            puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                                        : "SD"
                                    :"SD"; //Provincia-Contrato

                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.CodPostalCanton
                                        : "00"
                                    : "00"; //Canton -Contrato;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.NomCanton
                                        : "SD"
                                    : "SD"; //Canton -Contrato;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.CodPostalDistrito
                                        : "00"
                                    : "00"; //Distrito -Contrato;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                    puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.NomDistrito
                                    : "SD"
                                : "SD"; //Distrito -Contrato;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                   puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                       puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2, '0')
                                       : "00"
                                   : "00"; //Provincia
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                            puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                                        : "SD"
                                    : "SD"; //Provincia
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.CodPostalCanton
                                        : "00"
                                    : "00"; //Canton ;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.NomCanton
                                        : "SD"
                                    : "SD"; //Canton ;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.CodPostalDistrito
                                        : "00"
                                    : "00"; //Distrito ;
                            X = puesto.RelPuestoUbicacion.Count > 0 ?
                                puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                    puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.NomDistrito
                                    : "SD"
                                : "SD"; //Distrito ;
                            X = "SD"; //Ruta -Nueva;
                            X = puesto.UbicacionAdministrativa.Presupuesto.Programa.DesPrograma; //Titulo -P;
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Day.ToString() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Month.ToString() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Year.ToString() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Day.ToString() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Month.ToString() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Year.ToString() : "SD";
                            X = datoDetalle.OcupacionReal != null ? datoDetalle.OcupacionReal.DesOcupacionReal.TrimEnd() : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Day.ToString() : "SD"; //día vacación;
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Month.ToString() : "SD"; //mes vacación;
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Year.ToString() : "SD";  //año vacación;
                            X = funcionario.DetalleContratacion.FirstOrDefault().NumAnualidades.ToString();
                            X = ((CSalarioDTO)salario.ElementAt(1)).MtoTotal.ToString(); //Salario Mensual;
                            X = funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.ToString() : "SD"; //Código Inspector; 
                            X = tipoRegimen; //Tipo Régimen;
                            X = puesto.UbicacionAdministrativa.Presupuesto.Programa.PK_Programa.ToString(); //Programa Presupuestario;
                            X = funcionario.IndSexo;
                            X = puesto.CaracteristicasPuesto.Count > 0 ? puesto.CaracteristicasPuesto.FirstOrDefault().DesCaracteristica : "SD";
                            X = Convert.ToDateTime(funcionario.FecNacimiento).ToShortDateString();
                            X = puesto.EstadoPuesto.DesEstadoPuesto != null ? puesto.EstadoPuesto.DesEstadoPuesto : "SD";
                            X = puesto.IndNivelOcupacional.HasValue ? puesto.IndNivelOcupacional.ToString() : "SD";
                            X = puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal.ToString() : "SD";
                            X = puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "SD";
                            X = puesto.DetallePuesto.FirstOrDefault().Clase != null ? puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase.ToString() : "SD";
                            X = puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad.ToString() : "SD";
                            X = puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().FecMesAumento != null ? funcionario.DetalleContratacion.FirstOrDefault().FecMesAumento : "SD";
                            X = funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal != null ? funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal : "SD";
                            X = puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString() : "SD";
                            X = puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto;
                            X = funcionario.HistorialEstadoCivil.Count() > 0 ? funcionario.HistorialEstadoCivil.LastOrDefault().CatEstadoCivil.DesEstadoCivil.ToUpper() : "SD";
                            X = funcionario.InformacionContacto.FirstOrDefault() != null ? funcionario.InformacionContacto.FirstOrDefault().DesContenido : "SD";
                            X = puntosCarrera.Contenido.ToString();

                            respuesta = new string[]
                               {
                                funcionario.IdCedulaFuncionario,
                                funcionario.NomFuncionario.TrimEnd(),
                                funcionario.NomPrimerApellido.TrimEnd(),
                                funcionario.NomSegundoApellido.TrimEnd(),
                                puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString().PadLeft(3, '0') : "000",
                                puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd(): "SD",
                                puesto.UbicacionAdministrativa.DireccionGeneral != null ?  puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.ToString().PadLeft(2, '0') : "00",
                                puesto.UbicacionAdministrativa.DireccionGeneral != null ?  puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd().ToUpper() : "SD",
                                funcionario.EstadoFuncionario.DesEstadoFuncionario.TrimEnd().ToUpper(),
                                datoDetalle.Clase != null ? datoDetalle.Clase.DesClase.TrimEnd() : "SD", //puesto.DetallePuesto.FirstOrDefault().Clase.DesClase.TrimEnd(),
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault() .Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2,'0') : "SD",
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault().Distrito.Canton.Provincia.NomProvincia : "SD",
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault() .Distrito.Canton.CodPostalCanton : "SD",
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault() .Distrito.Canton.NomCanton : "SD",
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault() .Distrito.CodPostalDistrito : "SD",
                                funcionario.Direccion.FirstOrDefault() != null ? funcionario.Direccion.FirstOrDefault() .Distrito.NomDistrito : "SD",
                                puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd().ToUpper(),
                                "SD", //Subdivision,
                                puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "SD",
                                puesto.CodPuesto.TrimEnd(),
                                (funcionario.Direccion != null && funcionario.Direccion.Count > 0 )?
                                        funcionario.Direccion.FirstOrDefault().DirExacta != null ? funcionario.Direccion.FirstOrDefault().DirExacta.TrimEnd() : "SD"
                                        : "SD",
                                funcionario.DetalleContratacion.Count > 0 ? (funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal != null ? funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal.TrimEnd() : "SD") : "SD",// Ubicación Real,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2, '0')
                                        : "00"
                                    : "00", //Provincia-Contrato
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                            puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                                        : "SD"
                                    :"SD", //Provincia-Contrato
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.CodPostalCanton
                                        : "00"
                                    : "00", //Canton -Contrato,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.Canton.NomCanton
                                        : "SD"
                                    : "SD", //Canton -Contrato,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.CodPostalDistrito
                                        : "00"
                                    : "00", //Distrito -Contrato,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).ToList().Count > 0 ?
                                    puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 1).UbicacionPuesto.Distrito.NomDistrito
                                    : "SD"
                                : "SD", //Distrito -Contrato,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                   puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                       puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia.ToString().PadLeft(2, '0')
                                       : "00"
                                   : "00", //Provincia
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                            puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                                        : "SD"
                                    : "SD", //Provincia
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.CodPostalCanton
                                        : "00"
                                    : "00", //Canton ,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.Canton.NomCanton
                                        : "SD"
                                    : "SD", //Canton ,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.CodPostalDistrito
                                        : "00"
                                    : "00", //Distrito ,
                                puesto.RelPuestoUbicacion.Count > 0 ?
                                    puesto.RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).ToList().Count > 0 ?
                                        puesto.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.FK_TipoUbicacion == 2).UbicacionPuesto.Distrito.NomDistrito
                                        : "SD"
                                    : "SD", //Distrito ,
                                    "SD", //Ruta -Nueva,
                                puesto.UbicacionAdministrativa.Presupuesto.Programa.DesPrograma, //Titulo -P,
                                funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Day.ToString() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Month.ToString() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Year.ToString() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Day.ToString() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Month.ToString() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Year.ToString() : "SD",
                                datoDetalle.OcupacionReal != null ?  datoDetalle.OcupacionReal.DesOcupacionReal.TrimEnd() : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Day.ToString() : "SD", //día vacación,
                                funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Month.ToString() : "SD", //mes vacación,
                                funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecVacaciones.Value.Year.ToString() : "SD",  //año vacación,
                                funcionario.DetalleContratacion.FirstOrDefault().NumAnualidades.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().NumAnualidades.ToString() : "SD",
                                ((CSalarioDTO)salario.ElementAt(1)).MtoTotal.ToString(), //Salario Mensual,
                                funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.ToString() : "SD", //Código Inspector, 
                                tipoRegimen, //Tipo Régimen,
                                puesto.UbicacionAdministrativa.Presupuesto.Programa.PK_Programa.ToString(), //Programa Presupuestario,
                                funcionario.IndSexo,
                                puesto.CaracteristicasPuesto.Count > 0 ? puesto.CaracteristicasPuesto.FirstOrDefault().DesCaracteristica : "SD",
                                Convert.ToDateTime(funcionario.FecNacimiento).ToShortDateString(),
                                puesto.EstadoPuesto.DesEstadoPuesto != null  ? puesto.EstadoPuesto.DesEstadoPuesto : "SD",
                                puesto.IndNivelOcupacional.HasValue  ? puesto.IndNivelOcupacional.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Clase != null ? puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().FecMesAumento != null ? funcionario.DetalleContratacion.FirstOrDefault().FecMesAumento : "SD",
                                funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal != null ? funcionario.DetalleContratacion.FirstOrDefault().IndUbicacionReal : "SD",
                                puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString() : "SD",
                                puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto,
                                funcionario.HistorialEstadoCivil.Count() > 0 ? funcionario.HistorialEstadoCivil.LastOrDefault().CatEstadoCivil.DesEstadoCivil.ToUpper() : "SD",
                                funcionario.InformacionContacto.FirstOrDefault() != null ? funcionario.InformacionContacto.FirstOrDefault().DesContenido : "SD",
                                puntosCarrera.Contenido.ToString()
                               };
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)salario.FirstOrDefault()).MensajeError);
                        }              
                    }
                    else
                    {
                        // Buscar en la Tabla de Exfuncionarios
                        var exFuncionario = intermedioFuncionario.BuscarExFuncionarioCedula(cedula);
                        if (exFuncionario != null)
                        {
                            CProvinciaD intermedioProvincia = new CProvinciaD(contexto);
                            CCantonD intermedioCanton = new CCantonD(contexto);
                            CDistritoD intermedioDistrito = new CDistritoD(contexto);
                            CDivisionD intermedioDivision = new CDivisionD(contexto);
                            CClaseD intermedioClase = new CClaseD(contexto);
                            CSeccionD intermedioSeccion = new CSeccionD(contexto);
                            CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                            CDepartamentoD intermedioDepartamento = new CDepartamentoD(contexto);
                            COcupacionRealD intermedioOcup = new COcupacionRealD(contexto);
                            CProgramaD intermedioPrograma = new CProgramaD(contexto);
                            CCatEstadoCivilD intermerdioEstadoCivil = new CCatEstadoCivilD(contexto);
                            Puesto puesto = null;

                            int idCanton = 0;

                            string can_D = "SD";
                            string dis_D = "SD";

                            string can_C = "SD";
                            string dis_C = "SD";

                            string can_T = "SD";
                            string dis_T = "SD";

                            string codSeccion = "SD";
                            string nomSeccion = "SD";


                            if (exFuncionario.PROVINCIA_D != null)
                            {
                                if (exFuncionario.CANTON_D != null)
                                {
                                    var listaC = ((List<Canton>)intermedioCanton.BuscarCantonProvincia(Convert.ToInt16(exFuncionario.PROVINCIA_D)).Contenido).Where(Q => Q.CodPostalCanton == exFuncionario.CANTON_D).ToList();
                                    if (listaC != null && listaC.Count > 0)
                                    {
                                        can_D = listaC[0].NomCanton;
                                        idCanton = listaC[0].PK_Canton;

                                        // Buscar Distrito
                                        if (exFuncionario.DISTRITO != null)
                                        {
                                            var listaD = intermedioDistrito.CargarDistritosPorCanton(idCanton).Where(Q => Q.CodPostalDistrito == exFuncionario.DISTRITO).ToList();
                                            if (listaD != null && listaD.Count > 0)
                                                dis_D = listaD[0].NomDistrito;
                                        }
                                    }
                                }
                            }


                            if (exFuncionario.PROVINCIA_C != null)
                            {
                                if (exFuncionario.CANTON_C != null)
                                {
                                    var listaC = ((List<Canton>)intermedioCanton.BuscarCantonProvincia(Convert.ToInt16(exFuncionario.PROVINCIA_C)).Contenido).Where(Q => Q.CodPostalCanton == exFuncionario.CANTON_C).ToList();
                                    if (listaC != null && listaC.Count > 0)
                                    {
                                        can_C = listaC[0].NomCanton;
                                        idCanton = listaC[0].PK_Canton;

                                        // Buscar Distrito
                                        if (exFuncionario.DISTRITO_C != null)
                                        {
                                            var listaD = intermedioDistrito.CargarDistritosPorCanton(idCanton).Where(Q => Q.CodPostalDistrito == exFuncionario.DISTRITO_C).ToList();
                                            if (listaD != null && listaD.Count > 0)
                                                dis_C = listaD[0].NomDistrito;
                                        }
                                    }
                                }
                            }


                            if (exFuncionario.PROVINCIA_T != null)
                            {
                                if (exFuncionario.CANTON_T != null)
                                {
                                    var listaC = ((List<Canton>)intermedioCanton.BuscarCantonProvincia(Convert.ToInt16(exFuncionario.PROVINCIA_T)).Contenido).Where(Q => Q.CodPostalCanton == exFuncionario.CANTON_T).ToList();
                                    if (listaC != null && listaC.Count > 0)
                                    {
                                        can_T = listaC[0].NomCanton;
                                        idCanton = listaC[0].PK_Canton;

                                        // Buscar Distrito
                                        if (exFuncionario.DISTRITO_T != null)
                                        {
                                            var listaD = intermedioDistrito.CargarDistritosPorCanton(idCanton).Where(Q => Q.CodPostalDistrito == exFuncionario.DISTRITO_T).ToList();
                                            if (listaD != null && listaD.Count > 0)
                                                dis_T = listaD[0].NomDistrito;
                                        }
                                    }
                                }
                            }

                            if (exFuncionario.PUESTO_PROPIEDAD != null)
                            {
                                var datosPuesto = intermedioPuesto.DescargarPuestoCompleto(exFuncionario.PUESTO_PROPIEDAD);
                                if (datosPuesto.Codigo > 0)
                                    puesto = (Puesto)datosPuesto.Contenido;
                                else
                                    throw new Exception(((CErrorDTO)datosPuesto.Contenido).MensajeError);
                            }

                            var titP = exFuncionario.TITULO_P != null ? exFuncionario.TITULO_P : "00000";
                            var fillerP = exFuncionario.FILLER_P != null ? exFuncionario.FILLER_P : "00"; 
                            var dirP = exFuncionario.DIRECCION_P != null ? exFuncionario.DIRECCION_P : "0000";
                            var codP = titP + fillerP + dirP;

                            if (exFuncionario.SECCION != null)
                            {
                                codSeccion = exFuncionario.SECCION.TrimEnd();
                                var datoSeccion = intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(exFuncionario.SECCION));
                                if (datoSeccion != null)
                                    nomSeccion = datoSeccion.NomSeccion;
                            }


                            var x = exFuncionario.CEDULA.TrimEnd();
                            x = exFuncionario.NOMBRE.TrimEnd();
                            x = exFuncionario.PRIMER_APELLIDO.TrimEnd();
                            x = exFuncionario.SEGUNDO_APELLIDO.TrimEnd();
                            x = codSeccion;//exFuncionario.SECCION != null ? exFuncionario.SECCION : "000";
                            x = nomSeccion;//  exFuncionario.SECCION != null && exFuncionario.SECCION != "000" ? intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(exFuncionario.SECCION)).NomSeccion : "SD";
                            x = exFuncionario.DIRECCION != null ? exFuncionario.DIRECCION : "000";
                            x = exFuncionario.DIRECCION != null ? (Convert.ToInt32(exFuncionario.DIRECCION) > 0 ? intermedioDireccion.CargarDireccionGeneralPorID(Convert.ToInt32(exFuncionario.DIRECCION)).NomDireccion : "SD") : "SD";
                            x = "EXFUNCIONARIO"; //exFuncionario.EstadoFuncionario.DesEstadoFuncionario.TrimEnd();
                            x = exFuncionario.CLASE_PUESTO != null ? intermedioClase.CargarClasePorID(Convert.ToInt16(exFuncionario.CLASE_PUESTO)).DesClase : "SD";
                            x = exFuncionario.PROVINCIA_D != null ? exFuncionario.PROVINCIA_D.TrimEnd() : "00";
                            x = exFuncionario.PROVINCIA_D != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_D)).NomProvincia : "SD";
                            x = exFuncionario.CANTON_D != null ? exFuncionario.CANTON_D.TrimEnd() : "00";
                            x = can_D; //exFuncionario.CANTON_D != null  && exFuncionario.PROVINCIA_D != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_D)).NomCanton : "SD";
                            x = exFuncionario.DISTRITO != null ? exFuncionario.DISTRITO.TrimEnd() : "00";
                            x = dis_D; //exFuncionario.DISTRITO != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO)).NomDistrito : "SD";
                            x = exFuncionario.DIVISION != null ? intermedioDivision.CargarDivisionPorID(Convert.ToInt16(exFuncionario.DIVISION)).NomDivision : "SD";
                            x = "SD";//exFuncionario.SUBDIVISION != null ?; //Subdivision;
                            x = exFuncionario.DEPARTAMENTO != null && exFuncionario.DEPARTAMENTO != "000" ? intermedioDepartamento.CargarDepartamentoPorID(Convert.ToInt32(exFuncionario.DEPARTAMENTO)).NomDepartamento : "SD";
                            x = exFuncionario.PUESTO_PROPIEDAD != null ? exFuncionario.PUESTO_PROPIEDAD.TrimEnd() : "SD";
                            x = exFuncionario.SENAS != null ? exFuncionario.SENAS.TrimEnd() : "SD"; //DirExacta
                            x = exFuncionario.UBIC_REAL != null ? exFuncionario.UBIC_REAL.TrimEnd() : "SD";// Ubicación Real;
                            x = exFuncionario.PROVINCIA_C != null ? exFuncionario.PROVINCIA_C.TrimEnd() : "00"; //Provincia-Contrato
                            x = exFuncionario.PROVINCIA_C != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_C)).NomProvincia : "SD"; //Provincia-Contrato
                            x = exFuncionario.CANTON_C != null ? exFuncionario.CANTON_C.TrimEnd() : "00"; //Canton -Contrato;
                            x = can_C; //exFuncionario.CANTON_C != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_C)).NomCanton : "SD"; //Canton -Contrato;
                            x = exFuncionario.DISTRITO_C != null ? exFuncionario.DISTRITO_C.TrimEnd() : "00"; //Distrito -Contrato;
                            x = dis_C; //exFuncionario.DISTRITO_C != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO_C)).NomDistrito : "SD"; //Distrito -Contrato;
                            x = exFuncionario.PROVINCIA_T != null ? exFuncionario.PROVINCIA_T.TrimEnd() : "00";
                            x = exFuncionario.PROVINCIA_T != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_T)).NomProvincia : "SD";
                            x = exFuncionario.CANTON_T != null ? exFuncionario.CANTON_T.TrimEnd() : "00";
                            x = can_T; //exFuncionario.CANTON_T != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_T)).NomCanton : "SD";
                            x = exFuncionario.DISTRITO_T != null ? exFuncionario.DISTRITO_T.TrimEnd() : "00";
                            x = dis_T; //exFuncionario.DISTRITO_T != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO_T)).NomDistrito : "SD";
                            x = "SD"; //Ruta -Nueva;
                            x = exFuncionario.TITULO_P != null ? (intermedioPrograma.CargarProgramaPorID(Convert.ToInt32(exFuncionario.TITULO_P)) != null ? intermedioPrograma.CargarProgramaPorID(Convert.ToInt32(exFuncionario.TITULO_P)).DesPrograma : "SD") : "SD"; //Titulo -P;
                            x = exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Day.ToString() : "SD";
                            x = exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Month.ToString() : "SD";
                            x = exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Year.ToString() : "SD";
                            x = exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Day.ToString() : "SD";
                            x = exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Month.ToString() : "SD";
                            x = exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Year.ToString() : "SD";
                            x = exFuncionario.OCUP_REAL != null ?
                                    intermedioOcup.CargarOcupacionRealPorID(Convert.ToInt32(exFuncionario.OCUP_REAL)) != null ?
                                        intermedioOcup.CargarOcupacionRealPorID(Convert.ToInt32(exFuncionario.OCUP_REAL)).DesOcupacionReal
                                        : "SD"
                                    : "SD";
                            x = exFuncionario.DIA_VACA != null ? exFuncionario.DIA_VACA.ToString() : "SD"; //día vacación;
                            x = exFuncionario.MES_VACA != null ? exFuncionario.MES_VACA.ToString() : "SD"; //mes vacación;
                            x = exFuncionario.ANO_VACA != null ? exFuncionario.ANO_VACA.ToString() : "SD"; //año vacación;
                            x = exFuncionario.PASOS_DISF != null ? exFuncionario.PASOS_DISF.ToString() : "SD"; //NumAnualidades
                            x = exFuncionario.SAL_MENSUAL != null ? exFuncionario.SAL_MENSUAL.ToString() : "0"; //Salario Mensual;
                            x = exFuncionario.CODIGO_INSPECTORES != null ? exFuncionario.CODIGO_INSPECTORES : "SD"; //Código Inspector; 
                            x = "SD"; //Tipo Régimen;
                            x = puesto != null ? puesto.UbicacionAdministrativa.Presupuesto.Programa.PK_Programa.ToString() : "SD"; //Programa Presupuestario;
                            x = exFuncionario.SEXO != null ? exFuncionario.SEXO : "SD";
                            x = puesto != null ? (puesto.CaracteristicasPuesto.Count() > 0 ? puesto.CaracteristicasPuesto.FirstOrDefault().DesCaracteristica : "SD") : "SD";
                            x = Convert.ToDateTime(exFuncionario.FECHA_CUMPLE).ToShortDateString();
                            x = puesto.EstadoPuesto.DesEstadoPuesto != null ? puesto.EstadoPuesto.DesEstadoPuesto : "SD";
                            x = puesto.IndNivelOcupacional.HasValue ? puesto.IndNivelOcupacional.ToString() : "SD";
                            x = puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal.ToString() : "SD";
                            x = puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "SD";
                            x = puesto.DetallePuesto.FirstOrDefault().Clase != null ? puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase.ToString() : "SD";
                            x = puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad.ToString() : "SD";
                            x = puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad : "SD";
                            x = exFuncionario.FECHA_INGRESO.HasValue ? exFuncionario.FECHA_INGRESO.Value.Month.ToString() : "SD";
                            x = exFuncionario.UBIC_REAL != null ? exFuncionario.UBIC_REAL : "SD";
                            x = puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString() : "SD";
                            x = codP;
                            x = exFuncionario.ESTADO_CIVIL != null ? intermerdioEstadoCivil.CargarCatEstadoCivilPorID(Convert.ToInt16(exFuncionario.ESTADO_CIVIL)).DesEstadoCivil.ToUpper() : "SD";
                            x = "SD";// Número Teléfono
                            x = exFuncionario.PUNTOS != null ? exFuncionario.PUNTOS.ToString() : "0";

                            respuesta = new string[]
                               {
                                exFuncionario.CEDULA.TrimEnd(),
                                exFuncionario.NOMBRE.TrimEnd(),
                                exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                exFuncionario.SEGUNDO_APELLIDO.TrimEnd(),
                                codSeccion, //exFuncionario.SECCION  != null ? exFuncionario.SECCION : "000",
                                nomSeccion, //exFuncionario.SECCION  != null && exFuncionario.SECCION != "000" ? intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(exFuncionario.SECCION)).NomSeccion : "SD",
                                exFuncionario.DIRECCION  != null ? exFuncionario.DIRECCION : "000",
                                exFuncionario.DIRECCION  != null ?  (Convert.ToInt32(exFuncionario.DIRECCION) > 0 ? intermedioDireccion.CargarDireccionGeneralPorID(Convert.ToInt32(exFuncionario.DIRECCION)).NomDireccion : "SD") : "SD",
                                "EXFUNCIONARIO", //exFuncionario.EstadoFuncionario.DesEstadoFuncionario.TrimEnd(),
                                exFuncionario.CLASE_PUESTO != null ? intermedioClase.CargarClasePorID(Convert.ToInt16(exFuncionario.CLASE_PUESTO)).DesClase: "SD",
                                exFuncionario.PROVINCIA_D != null ?  exFuncionario.PROVINCIA_D.TrimEnd() : "00",
                                exFuncionario.PROVINCIA_D != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_D)).NomProvincia : "SD",
                                exFuncionario.CANTON_D != null ?  exFuncionario.CANTON_D.TrimEnd() : "00",
                                can_D, //exFuncionario.CANTON_D != null  && exFuncionario.PROVINCIA_D != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_D)).NomCanton : "SD",
                                exFuncionario.DISTRITO != null ?  exFuncionario.DISTRITO.TrimEnd() : "00",
                                dis_D, //exFuncionario.DISTRITO != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO)).NomDistrito : "SD",
                                exFuncionario.DIVISION != null ? intermedioDivision.CargarDivisionPorID(Convert.ToInt16(exFuncionario.DIVISION)).NomDivision : "SD",
                                "SD",//exFuncionario.SUBDIVISION != null ?, //Subdivision,
                                exFuncionario.DEPARTAMENTO != null && exFuncionario.DEPARTAMENTO != "000" ? intermedioDepartamento.CargarDepartamentoPorID(Convert.ToInt32(exFuncionario.DEPARTAMENTO)).NomDepartamento : "SD",
                                exFuncionario.PUESTO_PROPIEDAD != null ?  exFuncionario.PUESTO_PROPIEDAD.TrimEnd() : "SD",
                                exFuncionario.SENAS != null ?  exFuncionario.SENAS.TrimEnd() : "SD", //DirExacta
                                exFuncionario.UBIC_REAL != null ?  exFuncionario.UBIC_REAL.TrimEnd() : "SD",// Ubicación Real,
                                exFuncionario.PROVINCIA_C != null ?  exFuncionario.PROVINCIA_C.TrimEnd() : "00", //Provincia-Contrato
                                exFuncionario.PROVINCIA_C != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_C)).NomProvincia : "SD", //Provincia-Contrato
                                exFuncionario.CANTON_C != null ?  exFuncionario.CANTON_C.TrimEnd() : "00", //Canton -Contrato,
                                can_C, //exFuncionario.CANTON_C != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_C)).NomCanton : "SD", //Canton -Contrato,
                                exFuncionario.DISTRITO_C != null ?  exFuncionario.DISTRITO_C.TrimEnd() : "00", //Distrito -Contrato,
                                dis_C, //exFuncionario.DISTRITO_C != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO_C)).NomDistrito : "SD", //Distrito -Contrato,
                                exFuncionario.PROVINCIA_T != null ?  exFuncionario.PROVINCIA_T.TrimEnd() : "00",
                                exFuncionario.PROVINCIA_T != null ? intermedioProvincia.CargarProvinciaId(Convert.ToInt16(exFuncionario.PROVINCIA_T)).NomProvincia  : "SD",
                                exFuncionario.CANTON_T != null ?  exFuncionario.CANTON_T.TrimEnd() : "00",
                                can_T, //exFuncionario.CANTON_T != null ? intermedioCanton.CargarCantonId(Convert.ToInt16(exFuncionario.CANTON_T)).NomCanton : "SD",
                                exFuncionario.DISTRITO_T != null ?  exFuncionario.DISTRITO_T.TrimEnd() : "00",
                                dis_T, //exFuncionario.DISTRITO_T != null ? intermedioDistrito.CargarDistritoId(Convert.ToInt16(exFuncionario.DISTRITO_T)).NomDistrito : "SD",
                                "SD", //Ruta -Nueva,
                                exFuncionario.TITULO_P != null ? (intermedioPrograma.CargarProgramaPorID(Convert.ToInt32(exFuncionario.TITULO_P)) != null ? intermedioPrograma.CargarProgramaPorID(Convert.ToInt32(exFuncionario.TITULO_P)).DesPrograma: "SD"): "SD", //Titulo -P,
                                exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Day.ToString(): "SD",
                                exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Month.ToString(): "SD",
                                exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Year.ToString(): "SD",
                                exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Day.ToString(): "SD",
                                exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Month.ToString(): "SD",
                                exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.Year.ToString(): "SD",
                                exFuncionario.OCUP_REAL != null ?
                                    intermedioOcup.CargarOcupacionRealPorID(Convert.ToInt32(exFuncionario.OCUP_REAL)) != null ?
                                        intermedioOcup.CargarOcupacionRealPorID(Convert.ToInt32(exFuncionario.OCUP_REAL)).DesOcupacionReal
                                        : "SD"
                                    : "SD",
                                exFuncionario.DIA_VACA != null ? exFuncionario.DIA_VACA.ToString(): "SD", //día vacación,
                                exFuncionario.MES_VACA != null ? exFuncionario.MES_VACA.ToString(): "SD", //mes vacación,
                                exFuncionario.ANO_VACA != null ? exFuncionario.ANO_VACA.ToString(): "SD", //año vacación,
                                exFuncionario.PASOS_DISF != null ? exFuncionario.PASOS_DISF.ToString(): "SD", //NumAnualidades
                                exFuncionario.SAL_MENSUAL != null ?  exFuncionario.SAL_MENSUAL.ToString(): "0", //Salario Mensual,
                                exFuncionario.CODIGO_INSPECTORES != null ? exFuncionario.CODIGO_INSPECTORES: "SD", //Código Inspector, 
                                "SD", //Tipo Régimen,
                                puesto != null ? puesto.UbicacionAdministrativa.Presupuesto.Programa.PK_Programa.ToString() : "SD", //Programa Presupuestario,
                                exFuncionario.SEXO != null ? exFuncionario.SEXO: "SD",
                                puesto != null ? (puesto.CaracteristicasPuesto.Count() > 0 ? puesto.CaracteristicasPuesto.FirstOrDefault().DesCaracteristica : "SD") : "SD",
                                Convert.ToDateTime(exFuncionario.FECHA_CUMPLE).ToShortDateString(),
                                puesto.EstadoPuesto.DesEstadoPuesto != null  ? puesto.EstadoPuesto.DesEstadoPuesto : "SD",
                                puesto.IndNivelOcupacional.HasValue  ? puesto.IndNivelOcupacional.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null ? puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Clase != null ? puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad.ToString() : "SD",
                                puesto.DetallePuesto.FirstOrDefault().Especialidad != null ? puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad : "SD",
                                exFuncionario.FECHA_INGRESO.HasValue ?  exFuncionario.FECHA_INGRESO.Value.Month.ToString(): "SD",
                                exFuncionario.UBIC_REAL != null ? exFuncionario.UBIC_REAL : "SD",
                                puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString() : "SD",
                                codP,
                                exFuncionario.ESTADO_CIVIL != null ? intermerdioEstadoCivil.CargarCatEstadoCivilPorID(Convert.ToInt16(exFuncionario.ESTADO_CIVIL)).DesEstadoCivil.ToUpper() : "SD",
                                "SD",// Número Teléfono
                                exFuncionario.PUNTOS != null ? exFuncionario.PUNTOS.ToString() : "0"
                               };
                        }
                        else {
                            respuesta = new string[] { "No se encontró funcionario con esa cédula" };
                        }                     
                    }
                }
                else
                {
                    respuesta = new string[] { "El número de cédula debe contener 10 dígitos" };
                }
            }
            catch (Exception error)
            {
                respuesta = new string []{ error.Message};
            }

            return respuesta;
        }

        public string[] ConsultaFuncionarioClase(string cedula, string clase1, string clase2, string clase3)
        {
            string[] respuesta = { "" };

            if (clase1 == null) { clase1 = ""; }
            if (clase2 == null) { clase2 = ""; }
            if (clase3 == null) { clase3 = ""; }

            try
            {
                if (cedula.Length == 10)
                {
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    CPuestoD intermedioPuesto = new CPuestoD(contexto);

                    var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);
                    //var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(funcionario.Nombramiento.LastOrDefault().Puesto.CodPuesto).Contenido;

                    if (funcionario != null)
                    {
                        var nombramiento = funcionario.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).ToList();
                        var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(nombramiento[0].Puesto.CodPuesto).Contenido;
                        DetallePuesto datoDetalle = puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();

                        //if (clase1 != "" && funcionario.Nombramiento.LastOrDefault().Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase.Contains(clase1))
                        if (clase1 != "" && datoDetalle.Clase.DesClase.Contains(clase1))
                        {
                            return new string[] { clase1 };
                        }
                        if (clase2 != "" && datoDetalle.Clase.DesClase.Contains(clase2))
                        {
                            return new string[] { clase2 };
                        }
                        if (clase3 != "" && datoDetalle.Clase.DesClase.Contains(clase3))
                        {
                            return new string[] { clase3 };
                        }
                    }
                    else
                    {
                        respuesta = new string[] { "No se encontró funcionario con esa cédula" };
                    }
                }
                else
                {
                    respuesta = new string[] { "El número de cédula debe contener 10 dígitos" };
                }
            }
            catch (Exception error)
            {
                respuesta = new string[] { error.Message };
            }

            return respuesta;
        }

        public List<string[]> ConsultaFuncionarioPolicial(decimal codPolicial)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datos = new CRespuestaDTO();

                if (codPolicial > 0)
                    datos = intermedioFuncionario.BuscarFuncionarioPolicial(codPolicial);
                else
                    datos = intermedioFuncionario.ListarFuncionarioPolicial();

                if (datos != null)
                {
                    if(datos.Codigo > -1)
                    {

                        if (codPolicial > 0)
                        {
                            var funcionario =(Funcionario)datos.Contenido;
                            {
                                mensaje = new string[]
                                {
                                    funcionario.IdCedulaFuncionario,
                                    funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.ToString() : "SD",
                                    funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.HasValue ? funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.ToString() : "SD",
                                    funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.HasValue ? funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : "SD",
                                    funcionario.NomFuncionario.TrimEnd(),
                                    funcionario.NomPrimerApellido.TrimEnd(),
                                    funcionario.NomSegundoApellido.TrimEnd(),
                                };

                                respuesta.Add(mensaje);
                            }
                        }
                        else
                        {
                            foreach (var funcionario in (List<Funcionario>)datos.Contenido)
                            {
                                mensaje = new string[]
                                {
                                    funcionario.IdCedulaFuncionario,
                                    funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().CodPolicial.ToString() : "SD",
                                    funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.HasValue ? funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.ToString() : "SD",
                                    funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.FK_Seccion.HasValue ? funcionario.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : "SD",
                                    funcionario.NomFuncionario.TrimEnd(),
                                    funcionario.NomPrimerApellido.TrimEnd(),
                                    funcionario.NomSegundoApellido.TrimEnd()
                                };

                                respuesta.Add(mensaje);
                            }
                        }
                       
                    }
                    else
                    {
                        if (codPolicial > 0)
                        {
                            CSeccionD intermedioSeccion = new CSeccionD(contexto);
                            var datosEx = intermedioFuncionario.BuscarExFuncionarioCodPolicial(codPolicial.ToString());
                            if (datosEx.Count > 0)
                            {
                                foreach (var exFuncionario in (List<C_EMU_EXFUNCIONARIOS>)datosEx)
                                {
                                    mensaje = new string[]
                                    {
                                exFuncionario.CEDULA.TrimEnd(),
                                exFuncionario.CODIGO_INSPECTORES.TrimEnd(),
                                exFuncionario.SECCION  != null ? exFuncionario.SECCION : "000",
                                exFuncionario.SECCION  != null && exFuncionario.SECCION != "000" ? intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(exFuncionario.SECCION)).NomSeccion.TrimEnd() : "SD",
                                exFuncionario.NOMBRE.TrimEnd(),
                                exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                exFuncionario.SEGUNDO_APELLIDO.TrimEnd()
                                    };

                                    respuesta.Add(mensaje);
                                }
                            }
                            else
                            {
                                mensaje = new string[] { "No se encontraron funcionarios" };
                                respuesta.Add(mensaje);
                            }
                        }
                        else
                        {
                            mensaje = new string[] { ((CErrorDTO)datos.Contenido).MensajeError };
                            respuesta.Add(mensaje);
                        }
                    }   
                }
                else
                {
                    if (codPolicial > 0)
                    {
                        CSeccionD intermedioSeccion = new CSeccionD(contexto);
                        var exfuncionario = intermedioFuncionario.BuscarExFuncionarioCodPolicial(codPolicial.ToString("0000"));
                        if(exfuncionario.Count > 0 )
                        {
                            foreach (var exFuncionario in (List<C_EMU_EXFUNCIONARIOS>)datos.Contenido)
                            {
                                mensaje = new string[]
                                {
                                exFuncionario.CEDULA.TrimEnd(),
                                exFuncionario.CODIGO_INSPECTORES.TrimEnd(),
                                exFuncionario.SECCION  != null ? exFuncionario.SECCION : "000",
                                exFuncionario.SECCION  != null && exFuncionario.SECCION != "000" ? intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(exFuncionario.SECCION)).NomSeccion.TrimEnd() : "SD",
                                exFuncionario.NOMBRE.TrimEnd(),
                                exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                exFuncionario.SEGUNDO_APELLIDO.TrimEnd()
                                };

                                respuesta.Add(mensaje);
                            }
                        }
                        else
                        {
                            mensaje = new string[] { "No se encontraron funcionarios" };
                            respuesta.Add(mensaje);
                        }
                    }
                    else
                    {
                        mensaje = new string[] { "No se encontraron funcionarios" };
                        respuesta.Add(mensaje);
                    } 
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public bool ConsultaFuncionarioPolicial(decimal codPolicial, string cedula)
        {
            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datos = intermedioFuncionario.BuscarFuncionarioPolicial(codPolicial);

                if (datos != null)
                {
                    if (datos.Codigo > -1  && ((Funcionario)datos.Contenido).IdCedulaFuncionario == cedula)  
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                return false; 
            }
        }

        public List<string[]> ConsultaFuncionarioNombre(string apellido1, string apellido2, int titP)
        {
            //List<string> palabras = new List<string>();
            //palabras.Add(apellido1);
            //palabras.Add(apellido2);

            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;
            bool boolIncluir;
            try
            {
                CFuncionarioL intermedioFuncionarioLog = new CFuncionarioL();
                CSeccionD intermedioSeccion = new CSeccionD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datos = intermedioFuncionarioLog.BusquedaFuncionarioLogica("", "", apellido1, apellido2);

                if (datos != null)
                {
                    foreach (var funcionario in datos)
                    {
                        boolIncluir = false;
                        //var nombramiento = contexto.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento) && N.FK_Funcionario == funcionario.IdEntidad).OrderByDescending(N => N.FecRige).ToList();
                        //var nombramiento = intermedioNombramiento.CargarNombramientoActualCedula(funcionario.Cedula);
                        string codSeccion = "000";
                        string descSeccion = "SD";

                        var dato = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.Cedula);

                        if (funcionario != null)
                        {
                            var nombramiento = dato.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).ToList();
                            if (nombramiento.Count() == 0)
                                throw new Exception("El funcionario no cuenta con un Nombramiento válido");

                            var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(nombramiento[0].Puesto.CodPuesto).Contenido;
                            codSeccion = puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString().PadLeft(3, '0') : "000";
                            descSeccion = puesto.UbicacionAdministrativa.Seccion != null ?  puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : "SD";

                            if (titP > 0)
                            {
                                if (puesto.UbicacionAdministrativa.Presupuesto.Programa.PK_Programa == titP)
                                {
                                    boolIncluir = true;
                                }
                            }
                            else
                            {
                                boolIncluir = true;
                            }
                        }

                        if (boolIncluir)
                        {
                            mensaje = new string[]
                            {
                                funcionario.Cedula,
                                funcionario.Nombre.TrimEnd(),
                                funcionario.PrimerApellido.TrimEnd(),
                                funcionario.SegundoApellido.TrimEnd(),
                                funcionario.EstadoFuncionario.DesEstadoFuncionario,
                                codSeccion,
                                descSeccion
                            };

                            respuesta.Add(mensaje);
                        }
                    }
                }

                var datosEx = intermedioFuncionario.BuscarExFuncionarioNombre("", apellido1, apellido2);

                if (datosEx != null)
                {
                    foreach (var funcionario in datosEx)
                    {
                        boolIncluir = false;

                        string codSeccion = "000";
                        string descSeccion = "SD";

                        if (titP > 0)
                        {
                            if (funcionario.TITULO_P == titP.ToString())
                            {
                                boolIncluir = true;
                            }
                        }
                        else
                        {
                            boolIncluir = true;
                        }

                        

                        if (boolIncluir)
                        {
                            if (funcionario.SECCION != "" && funcionario.SECCION != null)
                            {
                                codSeccion = funcionario.SECCION;
                                var seccion = intermedioSeccion.CargarSeccionPorID(Convert.ToInt16(codSeccion));
                                descSeccion = seccion != null ? seccion.NomSeccion.TrimEnd() : "SD";
                            }
                            else
                            {
                                codSeccion = "000";
                                descSeccion = "SD";
                            }


                            mensaje = new string[]
                            {
                                funcionario.CEDULA,
                                funcionario.NOMBRE.TrimEnd(),
                                funcionario.PRIMER_APELLIDO.TrimEnd(),
                                funcionario.SEGUNDO_APELLIDO.TrimEnd(),
                                "EXFUNCIONARIO", //funcionario.EstadoFuncionario.DesEstadoFuncionario,
                                codSeccion,
                                descSeccion
                            };

                            respuesta.Add(mensaje);
                        }
                    }
                }

                if (respuesta.Count() == 0)
                {
                    mensaje = new string[] { "No se encontraron funcionarios con esos datos" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaFuncionariosDireccion(int codDireccion, int codSeccion)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);

                var datos = intermedioFuncionario.BuscarFuncionarioUbicacion(0, codDireccion, 0, codSeccion, "0");
                               
                if (datos != null)
                {
                    foreach (var funcionario in datos)
                    {
                        mensaje = new string[]
                        {
                            funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Day.ToString() : "SD",
                            funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Month.ToString() : "SD",
                            funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecIngreso.Value.Year.ToString() : "SD",
                            funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Day.ToString() : "SD",
                            funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Month.ToString() : "SD",
                            funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Year.ToString() : "SD",
                            funcionario.IdCedulaFuncionario,
                            funcionario.NomFuncionario.TrimEnd(),
                            funcionario.NomPrimerApellido.TrimEnd(),
                            funcionario.NomSegundoApellido.TrimEnd()
                        };

                        respuesta.Add(mensaje);
                    }
                }
                else
                {
                    mensaje = new string[] { "No se encontraron funcionarios para esa Dirección y/o Sección" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }


        public List<string[]> ConsultaFuncionariosOcupacionReal(int codOcupacionReal, int codDivision, int codDireccion)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                //CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);

                var datos = intermedioFuncionario.BuscarFuncionarioUbicacion(codDivision, codDireccion, 0, 0, "0");

                if (datos != null)
                {
                    foreach (var funcionario in datos)
                    {
                        var detalle = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.IdCedulaFuncionario);
                        if (detalle != null)
                        {
                            if (detalle.Nombramiento.Where(Q => Q.Puesto.DetallePuesto.Where(DP => DP.FK_OcupacionReal == codOcupacionReal).Count() > 0).Count() > 0)
                            {
                                mensaje = new string[]
                                {
                                    funcionario.IdCedulaFuncionario,
                                    funcionario.NomFuncionario.TrimEnd(),
                                    funcionario.NomPrimerApellido.TrimEnd(),
                                    funcionario.NomSegundoApellido.TrimEnd(),
                                    funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Day.ToString() : "SD",
                                    funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Month.ToString() : "SD",
                                    funcionario.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? funcionario.DetalleContratacion.FirstOrDefault().FecCese.Value.Year.ToString() : "SD"
                                };

                                respuesta.Add(mensaje);
                            }
                        }
                    }
                }

                // Ex Funcionarios
                var datosEx = intermedioFuncionario.BuscarExFuncionarioOcupacionReal(codOcupacionReal.ToString());
                if (datosEx != null)
                {
                    datosEx = datosEx.Where(Q => Q.DIVISION == codDivision.ToString()).ToList();

                    if (codDireccion > 0)
                        datosEx = datosEx.Where(Q => Q.DIRECCION_P == codDireccion.ToString()).ToList();

                    foreach (var exFuncionario in datosEx)
                    {
                        mensaje = new string[]
                        {
                            exFuncionario.CEDULA,
                            exFuncionario.NOMBRE.TrimEnd(),
                            exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                            exFuncionario.SEGUNDO_APELLIDO.TrimEnd(),
                            exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Day.ToString(): "SD",
                            exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Month.ToString(): "SD",
                            exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Year.ToString(): "SD"
                        };

                        respuesta.Add(mensaje);
                    }
                }

                if (respuesta.Count == 0)
                {
                    mensaje = new string[] { "No se encontraron funcionarios" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaJefaturaDependencia(int codSeccion)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);

                var datos = intermedioFuncionario.BuscarFuncionarioUbicacion(0, 0, 0, codSeccion, "0");

                if (datos != null)
                {
                    foreach (var funcionario in datos)
                    {
                        // Solo Funcionarios Activos
                        if(funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1)
                        {
                            var nombramiento = intermedioNombramiento.CargarNombramientoActualCedula(funcionario.IdCedulaFuncionario);
                
                            if (nombramiento != null)
                            {
                                var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(nombramiento.Puesto.CodPuesto).Contenido;

                                if (puesto != null)
                                {
                                    // listaCodigoJefatura
                                    //if (puesto.DetallePuesto.FirstOrDefault().FK_OcupacionReal >= 100000 && puesto.DetallePuesto.FirstOrDefault().FK_OcupacionReal <= 900000)
                                    if (listaCodigoJefatura.Contains(Convert.ToInt32(puesto.DetallePuesto.FirstOrDefault().FK_OcupacionReal)))
                                    {
                                        mensaje = new string[]
                                        {
                                            funcionario.IdCedulaFuncionario,
                                            funcionario.NomFuncionario.TrimEnd(),
                                            funcionario.NomPrimerApellido.TrimEnd(),
                                            funcionario.NomSegundoApellido.TrimEnd(),
                                            puesto.DetallePuesto.FirstOrDefault().FK_OcupacionReal.ToString(),
                                            puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                                        };

                                        respuesta.Add(mensaje);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    mensaje = new string[] { "No se encontraron funcionarios para esa Sección" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaExFuncionarios(int mes, int anio)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datos = intermedioFuncionario.BuscarExFuncionario(mes,anio);

                if (datos != null)
                {
                    foreach (var funcionario in datos)
                    {
                        mensaje = new string[]
                        {
                            funcionario.IdCedulaFuncionario,
                            funcionario.NomFuncionario,
                            funcionario.NomPrimerApellido,
                            funcionario.NomSegundoApellido
                        };

                        respuesta.Add(mensaje);
                    }
                }
                else
                {
                    mensaje = new string[] { "No se encontraron funcionarios para esa Dirección" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaFuncionarioCalificaciones(string cedula)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            try
            {
                if (cedula.Length == 10)
                {
                    CCalificacionNombramientoD intermedioCalificacion = new CCalificacionNombramientoD(contexto);

                    var historico = intermedioCalificacion.ListarCalificacionHistoricoCedula(cedula);

                    foreach (var item in historico)
                    {
                        mensaje = new string[]  {
                                    item.Periodo,
                                    item.DesCalificacion.ToString(),
                                    item.Nota.ToString()
                                };

                        respuesta.Add(mensaje);
                    }

                    var calif = intermedioCalificacion.ListarCalificacionNombramientoCedula(cedula);

                    if (calif != null)
                    {
                        foreach (var item in calif.Where((Q => Q.IndEstado == 1)).ToList())
                        {
                            mensaje = new string[]  {
                                    item.PeriodoCalificacion.PK_PeriodoCalificacion.ToString(),
                                    item.Calificacion.DesCalificacion,
                                    item.DetalleCalificacion.Select(i => Decimal.Parse(i.NumNotasPregunta)).Sum().ToString()
                                };

                            respuesta.Add(mensaje);
                        }
                    }
                }
                else
                {
                    mensaje = new string[] { "El número de cédula debe contener 10 dígitos" };
                    respuesta.Add(mensaje);
                }

                if(respuesta.Count() == 0)
                {
                    mensaje = new string[] { "No se encontraron acciones de personal para ese Funcionario" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            //try
            //{
            //    //CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
            //    CCalificacionNombramientoD intermedioCalificacion = new CCalificacionNombramientoD(contexto);

            //    var calif = intermedioCalificacion.CargarCalificacionNombramientoCedula(cedula);

            //    if (calif != null)
            //    {
            //        respuesta = new string[]
            //        {
            //            calif.IndPeriodo,
            //            calif.Calificacion.DesCalificacion                        
            //        };
            //    }
            //    else
            //    {
            //        respuesta = new string[] { "No se encontraron calificaciones para ese funcionario" };
            //    }
            //}
            //catch (Exception error)
            //{
            //    respuesta = new string[] { error.Message };
            //}

            return respuesta;
        }

        public List<string[]> ConsultaFuncionarioAccionPersonal(string cedula)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            CFuncionarioDTO funcionario;
            CPuestoDTO puesto = new CPuestoDTO();
            CAccionPersonalDTO accion = new CAccionPersonalDTO();
            List<DateTime> fechas = new List<DateTime>();
            try
            {
                if (cedula.Length == 10)
                {
                    CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                    CTipoAccionPersonalD intermedioTipo = new CTipoAccionPersonalD(contexto);
                    
                    // HISTÓRICO
                    CAccionPersonalHistoricoDTO accionHistorico = new CAccionPersonalHistoricoDTO { Cedula = cedula };

                    var datosHistorico = intermedio.BuscarHistorial(accionHistorico, new List<DateTime>());

                    if (datosHistorico != null)
                    {
                        foreach (var item in (List<C_EMU_AccionPersonal>)datosHistorico.Contenido)
                        {
                            var desTipo = (TipoAccionPersonal)intermedioTipo.CargarTipoAccionPersonalPorID(Convert.ToInt16(item.CodAccion)).Contenido;

                            mensaje = new string[]
                            {
                                   item.NumAccion,
                                   item.FecRige.HasValue ? Convert.ToDateTime(item.FecRige).ToShortDateString() : "SD",
                                   item.FecVence.HasValue ? Convert.ToDateTime(item.FecVence).ToShortDateString() : "SD",
                                   desTipo.DesTipoAccion, // Tipo de Acción,
                                   item.CodAccion.ToString(), //Código de Acción, 
                                   item.Explicacion //Descripción de Acción
                            };

                            respuesta.Add(mensaje);
                        }
                    }

                    // ACCIONES 
                    funcionario = new CFuncionarioDTO { Cedula = cedula };

                    var datos = intermedio.BuscarAccion(funcionario, puesto, accion, fechas);

                    if (datos != null)
                    {
                        foreach (var item in (List<AccionPersonal>)datos.Contenido)
                        {
                            if (item.EstadoBorrador.PK_EstadoBorrador == 7 && item.EstadoBorrador.PK_EstadoBorrador == 8) // Activa
                            {
                                mensaje = new string[]
                                {
                                   item.NumAccion,
                                   item.FecRige.HasValue ? Convert.ToDateTime(item.FecRige).ToShortDateString() : "SD",
                                   item.FecVence.HasValue ? Convert.ToDateTime(item.FecVence).ToShortDateString() : "SD",
                                   item.TipoAccionPersonal.DesTipoAccion, // Tipo de Acción,
                                   item.TipoAccionPersonal.PK_TipoAccionPersonal.ToString(), //Código de Acción, 
                                   item.Observaciones //Descripción de Acción
                                };

                                respuesta.Add(mensaje);
                            }
                        }
                    }

                    if (respuesta.Count() == 0)
                    { 
                        mensaje = new string[] { "No se encontraron acciones de personal para ese Funcionario" };
                        respuesta.Add(mensaje);
                    }
                }
                else
                {
                    respuesta.Add(new string[] { "El número de cédula debe contener 10 dígitos" });
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaFuncionarioPermisoSinSalario(DateTime FechaInicial, DateTime FechaFinal, bool MenosMes)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            CFuncionarioDTO funcionario = new CFuncionarioDTO();
            CPuestoDTO puesto = new CPuestoDTO();

            CAccionPersonalDTO accion = new CAccionPersonalDTO
            {
                TipoAccion = new CTipoAccionPersonalDTO
                {
                    IdEntidad = 7  // Permiso sin Salario //  10 Prórroga
                }
            };

            List<DateTime> fechas = new List<DateTime>();

            fechas.Add(FechaInicial);
            fechas.Add(FechaFinal);

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);
                             
                var datos = intermedio.BuscarAccion(funcionario, puesto, accion, fechas);
                List<AccionPersonal> lista = (List<AccionPersonal>)datos.Contenido;

                lista = lista.Where(Q => Q.EstadoBorrador.PK_EstadoBorrador == 7).ToList();

                if(MenosMes)
                {
                    lista = lista.Where(Q => ((TimeSpan)(Convert.ToDateTime(Q.FecVence) - Convert.ToDateTime(Q.FecRige))).Days <= 30).ToList();
                }
                else
                {
                    lista = lista.Where(Q => ((TimeSpan)(Convert.ToDateTime(Q.FecVence) - Convert.ToDateTime(Q.FecRige))).Days > 30).ToList();
                }

                if (datos != null)
                {
                    foreach (var item in lista)
                    {
                        mensaje = new string[]
                        {
                               item.NumAccion,
                               item.Nombramiento.Funcionario.IdCedulaFuncionario,
                        };

                        respuesta.Add(mensaje);
                    }
                }
                else
                {
                    mensaje = new string[] { "No se encontraron acciones de personal para ese Funcionario" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ConsultaFuncionarioPermisoVacaciones(DateTime FechaInicial, DateTime FechaFinal)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            CFuncionarioDTO funcionario = new CFuncionarioDTO();
            CPuestoDTO puesto = new CPuestoDTO();

            CAccionPersonalDTO accion = new CAccionPersonalDTO
            {
                TipoAccion = new CTipoAccionPersonalDTO
                {
                    IdEntidad = 6  // Permiso con Salario //  9 Prórroga
                }
            };

            List<DateTime> fechas = new List<DateTime>();

            fechas.Add(FechaInicial);
            fechas.Add(FechaFinal);

            try
            {
                CAccionPersonalD intermedio = new CAccionPersonalD(contexto);

                var datos = intermedio.BuscarAccion(funcionario, puesto, accion, fechas);
                List<AccionPersonal> lista = (List<AccionPersonal>)datos.Contenido;

                lista = lista.Where(Q => Q.EstadoBorrador.PK_EstadoBorrador == 7).ToList();
                
                if (datos != null)
                {
                    foreach (var item in lista)
                    {
                        mensaje = new string[]
                        {
                            item.Nombramiento.Funcionario.IdCedulaFuncionario,
                            item.NumAccion  // Documento   
                        };

                        respuesta.Add(mensaje);
                    }
                }


                //  BUSCAR VACACIONES
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }


        public List<string[]> ConsultaViaticoCorrido(int anio, int mes, string cedula)
        {
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            List<string[]> respuesta = new List<string[]>();
            CRespuestaDTO dato;
            string[] mensaje;
            string nombre = "";
            long n;
            //object[] parametros;

            //if (cedula != null) {
            //    parametros = new object[] {
            //        "NumFuncionario",
            //        cedula};
            //}

            try
            {
                // Gasto
                CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
                dato = intermedioGasto.ListarGastoTransporte();
                var datosGasto = intermedioGasto.ListarGastoTransporteServicio(mes, anio, cedula);


                if (datosGasto.Codigo != -1)
                {
                    foreach (var gasto in (List<PagoGastoTransporte>)datosGasto.Contenido)
                    {
                        if(long.TryParse(gasto.ReservaRecurso, out n))
                        {
                            mensaje = new string[]
                            {
                                gasto.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario,
                                gasto.GastoTransporte.Nombramiento.Funcionario.NomFuncionario.TrimEnd() + " " +
                                        gasto.GastoTransporte.Nombramiento.Funcionario.NomPrimerApellido.TrimEnd() + " " +
                                        gasto.GastoTransporte.Nombramiento.Funcionario.NomSegundoApellido.TrimEnd(),// NOMBRE
                                "1", // TIPO-GASTO (1- Transporte, 2- Viático)
                                gasto.ReservaRecurso != "" ? gasto.ReservaRecurso : "SD", //RESERVA-RECURSO
                                //gasto.HojaIndividualizada != null ? gasto.HojaIndividualizada : "", //HOJA-INDIVIDUALIZADA
                                gasto.MonPago.ToString(), //MONTO
                                gasto.FecPago.ToShortDateString(), //FECHA-VENCE (Período del Viático Corrido)
                                gasto.NumBoleta != "" ? gasto.NumBoleta : "SD", //BOLETA-PAGO
                            };

                            respuesta.Add(mensaje);
                        }
                    }
                }



                //// Viático y Gasto Emulación
                //var datosHistorico = intermedioViatico.ListarViaticoHistorico(mes, anio, cedula);

                //if (datosHistorico != null)
                //{
                //    foreach (var item in datosHistorico)
                //    {
                //        nombre = "";

                //        var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(item.Cedula);
                //        if (funcionario != null)
                //        {
                //            nombre = funcionario.NomFuncionario.TrimEnd() + " " +
                //                     funcionario.NomPrimerApellido.TrimEnd() + " " +
                //                     funcionario.NomSegundoApellido.TrimEnd();
                //        }
                //        else
                //        {
                //            // Buscar en la Tabla de Exfuncionarios
                //            var exFuncionario = intermedioFuncionario.BuscarExFuncionarioCedula(item.Cedula);
                //            if (exFuncionario != null)
                //                nombre = exFuncionario.NOMBRE.TrimEnd() + " " +
                //                        exFuncionario.PRIMER_APELLIDO.TrimEnd() + " " +
                //                        exFuncionario.SEGUNDO_APELLIDO.TrimEnd();
                //        }

                //        mensaje = new string[]
                //        {
                //            item.Cedula,
                //            nombre,// NOMBRE
                //            item.TipoMov.ToString(), // TIPO-GASTO (1- Transporte, 2- Viático)
                //            item.DetalleViatico, //RESERVA-RECURSO
                //            item.MonActual.ToString(), //MONTO
                //            item.FecVence.ToShortDateString(), //FECHA-VENCE (Período del Viático Corrido)
                //            item.NumFactura != null && item.NumFactura != "" ? item.NumFactura : "SD", //BOLETA-PAGO
                //        };

                //        respuesta.Add(mensaje);
                //    }
                //}

                CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
                var datosViatico = intermedioViatico.ListarViaticoCorridoServicio(mes, anio, cedula);
                if (datosViatico.Codigo != -1)
                {
                    foreach (var pago in (List<PagoViaticoCorrido>)datosViatico.Contenido)
                    {
                        if(long.TryParse(pago.ReservaRecurso, out n))
                        {
                            mensaje = new string[]
                            {
                                pago.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario,
                                pago.ViaticoCorrido.Nombramiento.Funcionario.NomFuncionario.TrimEnd() + " " +
                                        pago.ViaticoCorrido.Nombramiento.Funcionario.NomPrimerApellido.TrimEnd() + " " +
                                        pago.ViaticoCorrido.Nombramiento.Funcionario.NomSegundoApellido.TrimEnd(),// NOMBRE
                                "2", // TIPO-GASTO (1- Transporte, 2- Viático)
                                pago.ReservaRecurso != "" ? pago.ReservaRecurso : "SD", //RESERVA-RECURSO
                                //pago.HojaIndividualizada != "" ? pago.HojaIndividualizada : "SD", //HOJA-INDIVIDUALIZADA
                                pago.MonPago.ToString(), //MONTO
                                pago.FecPago.ToShortDateString(), //FECHA-VENCE (Período del Viático Corrido)
                                pago.NumBoleta != "" ? pago.NumBoleta : "SD", //BOLETA-PAGO
                            };

                            respuesta.Add(mensaje);
                        }
                    }
                }

                if (respuesta.Count == 0)
                {
                    mensaje = new string[] { "No se encontraron registros para los datos suministrados" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public List<string[]> ActualizarViaticoCorrido(int anio, int mes, string cedula, string reserva, string numBoleta)
        {
            List<string[]> respuesta = new List<string[]>();
            CRespuestaDTO dato;
            string[] mensaje;

            try
            {
                // Gasto
                CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
                dato = intermedioGasto.ListarGastoTransporte();
                var datosGasto = intermedioGasto.ActualizarGastoTransporteServicio(mes, anio, cedula, reserva, numBoleta);


                if (datosGasto.Codigo != -1)
                {
                    mensaje = new string[]
                       {
                            "1" // (0- No actualizado, 1- actualizado) 
                       };

                    respuesta.Add(mensaje);
                    //foreach (var gasto in (List<GastoTransporte>)datosGasto.Contenido)
                    //{
                    //    mensaje = new string[]
                    //    {
                    //        "1" // (0- No actualizado, 1- actualizado) 
                    //    };

                    //    respuesta.Add(mensaje);
                    //}
                }
                else
                {
                    mensaje = new string[]
                          {
                            "0" // (0- No actualizado, 1- actualizado) 
                          };

                    respuesta.Add(mensaje);

                }

                // Viático
                CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
                var datosViatico = intermedioViatico.ActualizarViaticoCorridoServicio(mes, anio, cedula, reserva, numBoleta);
                if (datosViatico.Codigo != -1)
                {
                    mensaje = new string[]
                       {
                            "1" // (0- No actualizado, 1- actualizado) 
                       };

                    respuesta.Add(mensaje);
                    //foreach (var viatico in (List<PagoViaticoCorrido>)datosViatico.Contenido)
                    //{

                    //}
                }
                else
                {
                    mensaje = new string[]
                          {
                            "0" // (0- No actualizado, 1- actualizado) 
                          };

                    respuesta.Add(mensaje);
                }

                if (respuesta.Count == 0)
                {
                    mensaje = new string[] { "No se encontraron registros para los datos suministrados" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }


        //public List<string[]> ActualizarViaticoCorrido(int anio, int mes, string cedula, string reserva, string numBoleta)
        //{
        //    List<string[]> respuesta = new List<string[]>();
        //    CRespuestaDTO dato;
        //    string[] mensaje;

        //    try
        //    {
        //        // Gasto
        //        CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
        //        dato = intermedioGasto.ListarGastoTransporte();
        //        var datosGasto = intermedioGasto.ListarGastoTransporteServicio(mes, anio, cedula);


        //        if (datosGasto.Codigo != -1)
        //        {
        //            foreach (var gasto in (List<GastoTransporte>)datosGasto.Contenido)
        //            {
        //                mensaje = new string[]
        //                {

        //                };

        //                respuesta.Add(mensaje);
        //            }
        //        }


        //        // Viático
        //        CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
        //        var datosViatico = intermedioViatico.ActualizarViaticoCorridoServicio(mes, anio, cedula, reserva, numBoleta);
        //        if (datosViatico.Codigo != -1)
        //        {
        //            foreach (var viatico in (List<ViaticoCorrido>)datosViatico.Contenido)
        //            {
        //                mensaje = new string[]
        //                {
        //                   //anio.ToString(),
        //                   // mes.ToString(),
        //                   // cedula,
        //                   // reserva, //RESERVA-RECURSO
        //                   // hoja, //HOJA-INDIVIDUALIZADA
        //                   // numBoleta, //BOLETA-PAGO
        //                    "1" // (0- No actualizado, 1- actualizado) 
        //                };

        //                respuesta.Add(mensaje);
        //            }
        //        }
        //        else
        //        {
        //            mensaje = new string[]
        //                  {
        //                    //anio.ToString(),
        //                    //mes.ToString(),
        //                    //cedula,
        //                    //reserva, //RESERVA-RECURSO
        //                    //hoja, //HOJA-INDIVIDUALIZADA
        //                    //numBoleta, //BOLETA-PAGO
        //                    "0" // (0- No actualizado, 1- actualizado) 
        //                  };

        //            respuesta.Add(mensaje);

        //        }

        //        if (respuesta.Count == 0)
        //        {
        //            mensaje = new string[] { "No se encontraron registros para los datos suministrados" };
        //            respuesta.Add(mensaje);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        mensaje = new string[] { error.Message };
        //        respuesta.Add(mensaje);
        //    }

        //    return respuesta;
        //}

        public List<string[]> ConsultaVacaciones(string cedula)
        {
            List<string[]> respuesta = new List<string[]>();
            try
            {
                CRegistroVacacionesD intermedio = new CRegistroVacacionesD(contexto);

                var datos = intermedio.ConsultaVacacionesHistorial(cedula);
                var datosNuevos = intermedio.ConsultaVacaciones(cedula);

                if (datos.Codigo > 0)
                {
                    var lista = ((List<C_EMU_Vacaciones_Movimiento>)(datos.Contenido));

                    foreach (var item in lista)
                    {
                        respuesta.Add(new string[]
                            {
                                item.TIPO_REG != null ? item.TIPO_REG.ToString() : "SD",
                                item.DOCUMENTO != null ? item.DOCUMENTO.ToString() : "SD",
                                item.PERI_TRAN != null ? item.PERI_TRAN.ToString() : "SD",
                                item.DIAS != null ? item.DIAS.ToString() : "SD",
                                //item.DOCUMENTO != null ? item.DOCUMENTO.ToString() : "SD",
                                item.FECHA_RIGE != null ? Convert.ToDateTime(item.FECHA_RIGE).ToShortDateString() : "SD",
                                item.FECHA_VENCE != null ? Convert.ToDateTime(item.FECHA_VENCE).ToShortDateString() : "SD",
                                item.FEC_ACT != null ? Convert.ToDateTime(item.FEC_ACT).ToShortDateString() : "SD",
                                item.TIPO_SOLICITUD != null ? item.TIPO_SOLICITUD.TrimEnd().Length > 0 ? item.TIPO_SOLICITUD : "SD" : "SD"
                            });
                    }

                    var listaNuevos = ((List<RegistroVacaciones>)(datosNuevos.Contenido));

                    foreach (var item in listaNuevos)
                    {
                        respuesta.Add(new string[]
                            {
                                "SD",
                                item.NumTransaccion != null ? item.NumTransaccion.ToString() : "SD",
                                item.PeriodoVacaciones.IndPeriodo != null ? item.PeriodoVacaciones.IndPeriodo.ToString() : "SD",
                                item.CntDias > 0 ? item.CntDias.ToString() : "SD",
                                //"SD",
                                item.FecInicio != null ? Convert.ToDateTime(item.FecInicio).ToShortDateString() : "SD",
                                item.FecFin != null ? Convert.ToDateTime(item.FecFin).ToShortDateString() : "SD",
                                item.FecInicio != null ? Convert.ToDateTime(item.FecInicio).ToShortDateString() : "SD",
                                item.IndTipoTransaccion != null ? item.IndTipoTransaccion.ToString() : "SD"
                            });
                        if (item.ReintegroVacaciones.Count > 0)
                        {
                            foreach (var itemReintegro in item.ReintegroVacaciones)
                            {
                                respuesta.Add(new string[]
                                {
                                    "SD",
                                    itemReintegro.NumSolicitudReintegro != null ? itemReintegro.NumSolicitudReintegro.ToString() : "SD",
                                    itemReintegro.RegistroVacaciones.PeriodoVacaciones.IndPeriodo != null ? itemReintegro.RegistroVacaciones.PeriodoVacaciones.IndPeriodo.ToString() : "SD",
                                    itemReintegro.CntDias > 0 ? itemReintegro.CntDias.ToString() : "SD",
                                    //"SD",
                                    itemReintegro.FecInicio != null ? Convert.ToDateTime(itemReintegro.FecInicio).ToShortDateString() : "SD",
                                    itemReintegro.FecFin != null ? Convert.ToDateTime(itemReintegro.FecFin).ToShortDateString() : "SD",
                                    itemReintegro.FecInicio != null ? Convert.ToDateTime(itemReintegro.FecInicio).ToShortDateString() : "SD",
                                    "99"
                                });
                            }
                        }
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<string[]> { new string[] { error.Message } };
            }
        }

        public List<string[]> ConsultaSaldoVacaciones(string cedula)
        {
            List<string[]> respuesta = new List<string[]>();
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var periodos = intermedio.ListarPeriodosActivos(cedula);

                //var datos = intermedio.ConsultaSaldoHistoricoVacaciones(cedula);
                CRespuestaDTO datos = new CRespuestaDTO();
                var listaEx = new List<C_EMU_Vacaciones_Saldo>();

                if (periodos.Codigo > 0)
                {
                    var lista = ((List<PeriodoVacaciones>)(periodos.Contenido)).OrderBy(Q => Q.IndPeriodo);
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    var funcionario = intermedioFuncionario.BuscarFuncionarioDetallePuesto(cedula);
                    Funcionario funcionarioBD = null;
                    C_EMU_EXFUNCIONARIOS exFuncionario = null;
                    if (funcionario.Contenido != null) // if (funcionario.Codigo > 0)
                    {
                        funcionarioBD = ((Funcionario)funcionario.Contenido);
                    }
                    else
                    {
                        exFuncionario = contexto.C_EMU_EXFUNCIONARIOS.Where(Q => Q.CEDULA == cedula).FirstOrDefault();
                        datos = intermedio.ConsultaSaldoHistoricoVacaciones(cedula);
                        listaEx = ((List<C_EMU_Vacaciones_Saldo>)(datos.Contenido));
                    }

                    if (funcionarioBD != null)
                    {
                        foreach (var item in lista)
                        {
                            respuesta.Add(new string[]
                                {
                                    funcionarioBD.NomFuncionario != null ? funcionarioBD.NomFuncionario.ToString() : "SD",
                                    funcionarioBD.NomPrimerApellido != null ? funcionarioBD.NomPrimerApellido.ToString() : "SD",
                                    funcionarioBD.NomSegundoApellido != null ? funcionarioBD.NomSegundoApellido.ToString() : "SD",
                                    item.IndPeriodo != null ? item.IndPeriodo.ToString() : "SD",
                                    item.IndSaldo != null ? item.IndSaldo.ToString() : "SD",
                                    funcionarioBD.DetalleContratacion.FirstOrDefault() != null ? Convert.ToDateTime(funcionarioBD.DetalleContratacion.FirstOrDefault().FecIngreso).Year.ToString() : "SD",
                                    funcionarioBD.DetalleContratacion.FirstOrDefault() != null ? Convert.ToDateTime(funcionarioBD.DetalleContratacion.FirstOrDefault().FecIngreso).Year.ToString() : "SD",
                                    funcionarioBD.Nombramiento.FirstOrDefault() != null ? funcionarioBD.Nombramiento.FirstOrDefault().Puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString() : "SD",
                                    lista.Count().ToString()
                                });
                        }
                    }
                    else
                    {
                        foreach (var item in listaEx)
                        {
                            respuesta.Add(new string[]
                                {
                                    exFuncionario.NOMBRE != null ? exFuncionario.NOMBRE.ToString() : "SD",
                                    exFuncionario.PRIMER_APELLIDO != null ? exFuncionario.PRIMER_APELLIDO.ToString() : "SD",
                                    exFuncionario.SEGUNDO_APELLIDO != null ? exFuncionario.SEGUNDO_APELLIDO.ToString() : "SD",
                                    item.PERI_VACA != null ? item.PERI_VACA.ToString() : "SD",
                                    item.SALDO != null ? item.SALDO.ToString() : "SD",
                                    exFuncionario.FECHA_INGRESO != null ? Convert.ToDateTime(exFuncionario.FECHA_INGRESO).Year.ToString() : "SD",
                                    exFuncionario.FECHA_INGRESO != null ? Convert.ToDateTime(exFuncionario.FECHA_INGRESO).Year.ToString() : "SD",
                                    exFuncionario.SECCION != null ? exFuncionario.SECCION.ToString() : "SD",
                                    lista.Count().ToString()
                                });
                        }
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)periodos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<string[]> { new string[] { error.Message } };
            }
        }

        public List<string[]> ListarFuncionariosActivos()
        {
            List<string[]> respuesta = new List<string[]>();
            List<Funcionario> lista = new List<Funcionario>();

            string cedula = "SD";

            string codOcupReal = "SD";
            string desOcupReal = "SD";
            string codSeccion = "SD";
            string desSeccion = "SD";
            string codDireccion = "SD";
            string desDireccion = "SD";
            string clase = "SD";

            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);

                var datos = intermedio.ListarFuncionariosActivos();

                if (datos.Codigo > 0)
                {
                    lista = ((List<Funcionario>)(datos.Contenido));

                    foreach (var item in lista)
                    {

                        cedula = item.IdCedulaFuncionario;
                        //var a = 1;
                        //if (item.IdCedulaFuncionario == "0080720171")
                        //    a = 2;
                        codOcupReal = "SD";
                        desOcupReal = "SD";
                        codSeccion = "SD";
                        desSeccion = "SD";
                        codDireccion = "SD";
                        desDireccion = "SD";
                        clase = "SD";

                        var nombramiento = item.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).FirstOrDefault();

                        if (nombramiento != null)
                        {
                            if (nombramiento.Puesto.DetallePuesto.Count() > 0)
                            {
                                if (nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                                {
                                    codOcupReal = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal.ToString();
                                    desOcupReal = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal.ToString();
                                }
                                if (nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase != null)
                                {
                                    clase = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase;
                                }
                            }
                            if (nombramiento.Puesto.UbicacionAdministrativa != null)
                            {
                                if (nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral != null)
                                {
                                    codDireccion = nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.ToString();
                                    desDireccion = nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.ToString();
                                }
                                if (nombramiento.Puesto.UbicacionAdministrativa.Seccion != null)
                                {
                                    codSeccion = nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString("000");
                                    desSeccion = nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.ToString();
                                }
                            }
                        }                     

                        respuesta.Add(new string[]
                            {
                                item.IdCedulaFuncionario != null ? item.IdCedulaFuncionario.ToString() : "SD",
                                item.NomFuncionario != null ? item.NomFuncionario.TrimEnd() : "SD",
                                item.NomPrimerApellido != null ? item.NomPrimerApellido.TrimEnd() : "SD",
                                item.NomSegundoApellido != null ? item.NomSegundoApellido.TrimEnd() : "SD",
                                nombramiento != null ? nombramiento.Puesto.CodPuesto.ToString() : "SD",
                                clase,
                                codOcupReal,
                                desOcupReal,
                                codSeccion,
                                desSeccion,
                                codDireccion,
                                desDireccion
                            });
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<string[]> { new string[] { error.Message  + " " + cedula } };
            }
        }

        public List<string[]> ListarFuncionariosTotal()
        {
            List<string[]> respuesta = new List<string[]>();
            List<Funcionario> lista = new List<Funcionario>();
            List<string> listaCedulas = new List<string>();
            var cedula = "";
            var pausa = "";
            string centroCosto = ""; //Division (00)+ Dirección(000) + Departamento(000) + Sección(000)


            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);

                var datos = intermedio.ListarFuncionariosTotal();

                if (datos.Codigo > 0)
                {
                    lista = ((List<Funcionario>)(datos.Contenido));

                    foreach (var item in lista)
                    {
                        centroCosto = "";

                        cedula = item.IdCedulaFuncionario;
                        if (cedula == "0080720171")
                            pausa = "";
                        Nombramiento datoNombramiento = item.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();

                        if (datoNombramiento == null)
                        {
                            datoNombramiento = item.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                            //if (datoNombramiento == null)
                            //{
                            //    throw new Exception("El funcionario no tiene un Nombramiento");
                            //}
                        }


                        if (datoNombramiento != null)
                        {
                            var puesto = (Puesto)intermedioPuesto.DescargarPuestoCompleto(datoNombramiento.Puesto.CodPuesto).Contenido;
                            centroCosto += puesto.UbicacionAdministrativa.Division.PK_Division.ToString().PadLeft(2, '0');
                            centroCosto += puesto.UbicacionAdministrativa.DireccionGeneral != null ? puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.ToString().PadLeft(3, '0') : "000";
                            centroCosto += puesto.UbicacionAdministrativa.Departamento != null ? puesto.UbicacionAdministrativa.Departamento.PK_Departamento.ToString().PadLeft(3, '0') : "000";
                            centroCosto += puesto.UbicacionAdministrativa.Seccion != null ? puesto.UbicacionAdministrativa.Seccion.PK_Seccion.ToString().PadLeft(3, '0') : "000";
                        }

                        listaCedulas.Add(cedula);

                        respuesta.Add(new string[]
                            {
                                item.IdCedulaFuncionario != null ? item.IdCedulaFuncionario.ToString() : "SD",
                                item.NomFuncionario != null ? item.NomFuncionario.TrimEnd() : "SD",
                                item.NomPrimerApellido != null ? item.NomPrimerApellido.TrimEnd() : "SD",
                                item.NomSegundoApellido != null ? item.NomSegundoApellido.TrimEnd() : "SD",
                                centroCosto,
                                "SD", // Descripción
                                item.EstadoFuncionario.PK_EstadoFuncionario == 1 ? "Activo" :"ExFuncionario" ,
                                item.DetalleContratacion != null && item.DetalleContratacion.Count > 0 ?
                                    item.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? item.DetalleContratacion.FirstOrDefault().FecCese.Value.Day.ToString() : "SD" : 
                                    "SD",
                                item.DetalleContratacion != null && item.DetalleContratacion.Count > 0 ?
                                    item.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? item.DetalleContratacion.FirstOrDefault().FecCese.Value.Month.ToString() : "SD" :
                                    "SD",
                                item.DetalleContratacion != null && item.DetalleContratacion.Count > 0 ?
                                    item.DetalleContratacion.FirstOrDefault().FecCese.HasValue ? item.DetalleContratacion.FirstOrDefault().FecCese.Value.Year.ToString() : "SD" :
                                    "SD",
                                item.DetalleContratacion != null && item.DetalleContratacion.Count > 0 ?
                                    item.DetalleContratacion.FirstOrDefault().CodPolicial.HasValue ? item.DetalleContratacion.FirstOrDefault().CodPolicial.ToString() : "SD"  : //Código Inspector; 
                                    "SD",
                            });
                    }


                    /// Listar ExFuncionarios del Histórico
                    var datosEx = intermedio.ListarExFuncionariosTotal();
                    
                    if (datosEx != null)
                    {
                        foreach (var exFuncionario in datosEx)
                        {
                            if (exFuncionario.CEDULA != null && exFuncionario.NOMBRE != null && listaCedulas.Contains(exFuncionario.CEDULA.TrimEnd()) == false)
                            {
                                cedula = exFuncionario.CEDULA.TrimEnd();
                                if (cedula == "9999999999")
                                    pausa = "";

                                listaCedulas.Add(cedula);

                                var titP = exFuncionario.TITULO_P != null ? exFuncionario.TITULO_P : "00000";
                                var fillerP = exFuncionario.FILLER_P != null ? exFuncionario.FILLER_P : "00";
                                var dirP = exFuncionario.DIRECCION_P != null ? exFuncionario.DIRECCION_P : "0000";
                                var codP = titP + fillerP + dirP;

                                respuesta.Add(new string[]
                                    {
                                    exFuncionario.CEDULA.TrimEnd(),
                                    exFuncionario.NOMBRE.TrimEnd(),
                                    exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                    exFuncionario.SEGUNDO_APELLIDO.TrimEnd(),
                                    codP,
                                    "SD", // Descripción
                                    "ExFuncionario" ,
                                    exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Day.ToString(): "SD",
                                    exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Month.ToString(): "SD",
                                    exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.Year.ToString(): "SD",
                                    exFuncionario.CODIGO_INSPECTORES != null ? exFuncionario.CODIGO_INSPECTORES: "SD", //Código Inspector
                                    });
                            }
                        }
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<string[]> { new string[] { error.Message + "   " + cedula } };
            }
        }

        public List<string[]> ConsultaFuncionarioPago(string Cedula, DateTime FechaInicial, DateTime FechaFinal)
        {
            List<string[]> respuesta = new List<string[]>();
            string[] mensaje;

            CPlanillaD pagos = new CPlanillaD(contexto);

            CFuncionarioDTO funcionario = new CFuncionarioDTO();

            //List<DateTime> fechas = new List<DateTime>();

            //fechas.Add(FechaInicial);
            //fechas.Add(FechaFinal);

            try
            {
                var historico = pagos.ConsultarPagoFuncionarioHistorico(Cedula, FechaInicial, FechaFinal);

                if (historico != null)
                {
                    foreach (var item in historico)
                    {
                        var monto = (item.@base != null && item.@base != "NULL" ? Convert.ToDecimal(item.@base.Replace(',','.'), new CultureInfo("en-US")) : 0) +
                                    (item.aumentos != null && item.aumentos != "NULL" ? Convert.ToDecimal(item.aumentos.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.grupo != null && item.grupo != "NULL" ? Convert.ToDecimal(item.grupo.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.sobresueldos != null && item.sobresueldos != "NULL" ? Convert.ToDecimal(item.sobresueldos.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.prohibicion != null && item.prohibicion != "NULL" ? Convert.ToDecimal(item.prohibicion.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.dedicacion_exclusiva != null && item.dedicacion_exclusiva != "NULL" ? Convert.ToDecimal(item.dedicacion_exclusiva.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.peligrosidad != null && item.peligrosidad != "NULL" ? Convert.ToDecimal(item.peligrosidad.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.quinquenio != null && item.quinquenio != "NULL" ? Convert.ToDecimal(item.quinquenio.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.disponibilidad != null && item.disponibilidad != "NULL" ? Convert.ToDecimal(item.disponibilidad.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.grado_academico != null && item.grado_academico != "NULL" ? Convert.ToDecimal(item.grado_academico.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.riesgo_policial != null && item.riesgo_policial != "NULL" ? Convert.ToDecimal(item.riesgo_policial.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.curso_basico != null && item.curso_basico != "NULL" ? Convert.ToDecimal(item.curso_basico.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.instruccion_policial != null && item.instruccion_policial != "NULL" ? Convert.ToDecimal(item.instruccion_policial.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.bonificacion != null && item.bonificacion != "NULL" ? Convert.ToDecimal(item.bonificacion.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.consulta_externa != null && item.consulta_externa != "NULL" ? Convert.ToDecimal(item.consulta_externa.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.desarraigo != null && item.desarraigo != "NULL" ? Convert.ToDecimal(item.desarraigo.Replace(',', '.'), new CultureInfo("en-US")) : 0) +
                                    (item.otros_incentivos != null && item.otros_incentivos != "NULL" ? Convert.ToDecimal(item.otros_incentivos.Replace(',', '.'), new CultureInfo("en-US")) : 0);

                        mensaje = new string[]
                        {
                               item.fecha_periodo.ToString(),
                               "Salario",//item.DetalleDesgloseSalarial.FirstOrDefault().ComponenteSalarial.DesComponenteSalarial,
                               monto.ToString()
                        };

                        respuesta.Add(mensaje);
                    }
                }


                var datos = pagos.ConsultarPagoFuncionario(Cedula, FechaInicial, FechaFinal);

                if (datos != null)
                {
                    foreach (var item in datos)
                    {
                        mensaje = new string[]
                        {
                               item.IndPeriodo.ToString(),
                               item.DetalleDesgloseSalarial.FirstOrDefault().ComponenteSalarial.DesComponenteSalarial,
                               item.DetalleDesgloseSalarial.Sum(Q => Q.MtoPagocomponenteSalarial).ToString()
                        };

                        respuesta.Add(mensaje);
                    }
                }
                else
                {
                    mensaje = new string[] { "No se encontraron datos de planilla para ese Funcionario" };
                    respuesta.Add(mensaje);
                }
            }
            catch (Exception error)
            {
                mensaje = new string[] { error.Message };
                respuesta.Add(mensaje);
            }

            return respuesta;
        }

        public string[] ConsultaFuncionarioPuntosCarrera(string cedula)
        {
            string[] respuesta;

            try
            {
                if (cedula.Length == 10)
                {
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    CPuestoD intermedioPuesto = new CPuestoD(contexto);
                    CSalarioL intermedioSalario = new CSalarioL();
                    CFuncionarioL intermedioFunc = new CFuncionarioL();

                    var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);

                    if (funcionario != null)
                    {
                        var puntosCarrera = intermedioFunc.BuscarFuncionarioPuntosCarreraProfesional(cedula);

                        respuesta = new string[]
                              {
                                puntosCarrera.Contenido.ToString()
                              };
                    }
                    else
                    {
                        // Buscar en la Tabla de Exfuncionarios
                        var exFuncionario = intermedioFuncionario.BuscarExFuncionarioCedula(cedula);
                        if (exFuncionario != null)
                        {
                            respuesta = new string[]
                               {
                                exFuncionario.PUNTOS != null ? exFuncionario.PUNTOS.ToString() : "0"
                               };
                        }
                        else
                        {
                            respuesta = new string[] { "No se encontró funcionario con esa cédula" };
                        }
                    }
                }
                else
                {
                    respuesta = new string[] { "El número de cédula debe contener 10 dígitos" };
                }
            }
            catch (Exception error)
            {
                respuesta = new string[] { error.Message };
            }

            return respuesta;
        }

        public string[] GuardarRegistroVacaciones(string numeroSolicitud, int tipo, DateTime fechaInicio, DateTime fechaFinal, int estado, string periodoVacaciones, string cedula, double cantDias)
        {
            try
            {
                CRegistroVacacionesD intermedio = new CRegistroVacacionesD(contexto);

                RegistroVacaciones temp = new RegistroVacaciones
                {
                    FecInicio = fechaInicio,
                    FecFin = fechaFinal,
                    IndEstado = estado,
                    IndTipoTransaccion = tipo,
                    NumTransaccion = numeroSolicitud,
                    CntDias = Convert.ToDecimal(cantDias),
                    FecActualizacion = DateTime.Now
                };

                var resultado = intermedio.GuardarRegistroVacaciones(temp, cedula, periodoVacaciones, cantDias);

                if (resultado.Codigo != -1)
                {
                    return new string[] { "S", "ND" };
                }
                else
                {
                    throw new Exception(resultado.Mensaje);
                }
            }
            catch (Exception error)
            {
                return new string[] { "N", error.Message };
            }
        }

        public string[] RegistrarPeriodoVacaciones(string cedFuncionario, string periodo, double cntDias, DateTime fecha, int estado)
        {
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                PeriodoVacaciones temp = new PeriodoVacaciones
                {
                    IndPeriodo = periodo,
                    IndDiasDerecho = Convert.ToDecimal(cntDias),
                    FecCarga = fecha,
                    IndEstado = estado,
                    IndSaldo = cntDias
                };

                var resultado = intermedio.RegistrarPeriodoVacaciones(cedFuncionario, temp);

                if (resultado.Codigo > 0)
                {
                    return new string[] { "S", resultado.Mensaje };
                }
                else
                {
                    return new string[] { "N", ((CErrorDTO)resultado.Contenido).MensajeError };
                }
            }
            catch (Exception error)
            {
                return new string[] { "N", error.Message };
            }
        }

        public string[] RegistrarReintegroVacaciones(string cedula, string documento, string documentoReintegro, DateTime inicio, DateTime final, string periodo, int motivo, string observaciones, decimal cntDias)
        {
            try
            {
                CReintegroVacacionesD intermedio = new CReintegroVacacionesD(contexto);

                ReintegroVacaciones temp = new ReintegroVacaciones
                {
                    NumSolicitudReintegro = documentoReintegro,
                    CntDias = cntDias,
                    FecInicio = inicio,
                    FecFin = final,
                    IndMotivo = motivo,
                    ObsReintegro = observaciones,
                    FecActualizacion = DateTime.Now
                };

                var resultado = intermedio.RegistrarReintegroVacaciones(cedula, periodo, documento, temp, cntDias);

                if (resultado.Codigo > 0)
                {
                    return new string[] { "S", resultado.Mensaje };
                }
                else
                {
                    return new string[] { "N", ((CErrorDTO)resultado.Contenido).MensajeError };
                }
            }
            catch (Exception error)
            {
                return new string[] { "N", error.Message };
            }
        }

        public string[] ConsultaFuncionarioFechaNombramiento(string cedula)
        {
            string[] respuesta = { "" };

            var fecRige = "";
            var fecVence = "";
            var idPuesto = 0;
            try
            {
                if (cedula.Length == 10)
                {
                    CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                    
                    var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);
                    
                    if (funcionario != null)
                    {
                        var nombramiento = funcionario.Nombramiento
                                            .Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento))
                                            //.OrderByDescending(N => N.FecRige)
                                            .ToList();

                        if (nombramiento.Count() == 0)
                            throw new Exception("El funcionario no cuenta con un Nombramiento válido");


                        //
                        var dato = nombramiento.Where(Q => Q.FecVence == null).FirstOrDefault();

                        if (dato != null)
                        {
                            idPuesto = dato.FK_Puesto;
                            fecVence = "";
                        }
                        else
                        {
                            dato = nombramiento.OrderByDescending(N => N.FecVence).FirstOrDefault();
                            if (dato != null)
                            {
                                idPuesto = dato.FK_Puesto;
                                fecVence = dato.FecVence.ToString();
                            }
                        }


                        // Buscar la fecha rige más antigua
                        dato = nombramiento.Where(Q => Q.FK_Puesto == idPuesto)
                                            .OrderBy(N => N.FecRige)
                                            .FirstOrDefault();
                        if (dato != null)
                        {
                            idPuesto = dato.FK_Puesto;
                            fecRige = dato.FecRige.ToString();
                        }

                        respuesta = new string[]
                              {
                                  funcionario.IdCedulaFuncionario,
                                  funcionario.NomFuncionario.TrimEnd(),
                                  funcionario.NomPrimerApellido.TrimEnd(),
                                  funcionario.NomSegundoApellido.TrimEnd(),
                                  funcionario.EstadoFuncionario.DesEstadoFuncionario.TrimEnd().ToUpper(),
                                  fecRige,
                                  fecVence
                              };
                   }
                    else
                    {
                        var exFuncionario = intermedioFuncionario.BuscarExFuncionarioCedula(cedula);
                        if (exFuncionario != null)
                        {
                            respuesta = new string[]
                            {
                                exFuncionario.CEDULA.TrimEnd(),
                                exFuncionario.NOMBRE.TrimEnd(),
                                exFuncionario.PRIMER_APELLIDO.TrimEnd(),
                                exFuncionario.SEGUNDO_APELLIDO.TrimEnd(),
                                "EXFUNCIONARIO",
                                exFuncionario.FECHA_INGRESO != null ? exFuncionario.FECHA_INGRESO.Value.ToString() : "SD",
                                exFuncionario.FECHA_CESE != null ? exFuncionario.FECHA_CESE.Value.ToString() : "SD"
                            };                            
                        }
                        else
                        {
                            respuesta = new string[] { "No se encontró funcionario con esa cédula" };
                        }
                    }
                }
                else
                {
                    respuesta = new string[] { "El número de cédula debe contener 10 dígitos" };
                }
            }
            catch (Exception error)
            {
                respuesta = new string[] { error.Message };
            }

            return respuesta;
        }

        #endregion
    }
}
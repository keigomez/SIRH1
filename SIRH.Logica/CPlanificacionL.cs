using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPlanificacionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPlanificacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        public List<List<CBaseDTO>> ListarNombramientosActivos(CListaNombramientosActivosDTO datoBusqueda)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            
            CPlanificacionD intermedio = new CPlanificacionD(contexto);
            CSalarioL intermedioSalario = new CSalarioL();

            CNombramientoDTO datoNombramiento = new CNombramientoDTO();

            var nombramientos = intermedio.ListarNombramientosActivos();

            // Cédula
            if (datoBusqueda.Funcionario != null && String.IsNullOrEmpty(datoBusqueda.Funcionario.Cedula) == false)
                nombramientos = nombramientos.Where(Q => Q.Funcionario.IdCedulaFuncionario == datoBusqueda.Funcionario.Cedula).ToList();

            // Nombre
            if (datoBusqueda.Funcionario != null && String.IsNullOrEmpty(datoBusqueda.Funcionario.Nombre) == false)
                nombramientos = nombramientos.Where(Q => Q.Funcionario.NomFuncionario.ToLower().Contains(datoBusqueda.Funcionario.Nombre.ToLower())).ToList();

            // Primer Apellido
            if (datoBusqueda.Funcionario != null && String.IsNullOrEmpty(datoBusqueda.Funcionario.PrimerApellido) == false)
                nombramientos = nombramientos.Where(Q => Q.Funcionario.NomPrimerApellido.ToLower().Contains(datoBusqueda.Funcionario.PrimerApellido.ToLower())).ToList();

            // Segundo Apellido
            if (datoBusqueda.Funcionario != null && String.IsNullOrEmpty(datoBusqueda.Funcionario.SegundoApellido) == false)
                nombramientos = nombramientos.Where(Q => Q.Funcionario.NomSegundoApellido.ToLower().Contains(datoBusqueda.Funcionario.SegundoApellido.ToLower())).ToList();

            // División
            if (datoBusqueda.Division != null && datoBusqueda.Division.IdEntidad > 0)
                nombramientos = nombramientos.Where(Q => Q.Division.PK_Division == datoBusqueda.Division.IdEntidad).ToList();

            // Dirección
            if (datoBusqueda.DireccionGeneral != null && datoBusqueda.DireccionGeneral.IdEntidad > 0)
                nombramientos = nombramientos.Where(Q => Q.PK_Direccion == datoBusqueda.DireccionGeneral.IdEntidad).ToList();

            // Departamento
            if (datoBusqueda.Departamento != null && datoBusqueda.Departamento.IdEntidad > 0)
                nombramientos = nombramientos.Where(Q => Q.Departamento.PK_Departamento == datoBusqueda.Departamento.IdEntidad).ToList();

            // Sección
            if (datoBusqueda.Seccion != null && datoBusqueda.Seccion.IdEntidad > 0)
                nombramientos = nombramientos.Where(Q => Q.Seccion.PK_Seccion == datoBusqueda.Seccion.IdEntidad).ToList();

            // Clase
            if (datoBusqueda.DetallePuesto != null && datoBusqueda.DetallePuesto.Clase != null && datoBusqueda.DetallePuesto.Clase.IdEntidad > 0)
                nombramientos = nombramientos.Where(Q => Q.DetallePuesto.Clase.PK_Clase == datoBusqueda.DetallePuesto.Clase.IdEntidad).ToList();


            if (nombramientos != null)
            {
                foreach (var item in (List<ListaNombramientosActivos>)nombramientos)
                {
                    List<CBaseDTO> lista = new List<CBaseDTO>();

                    //[0] CFuncionarioDTO
                    lista.Add(CFuncionarioL.FuncionarioGeneral(item.Funcionario));

                    //[1] CDetalleContratacionDTO
                    if (item.Funcionario.DetalleContratacion.Count > 0)
                        lista.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(item.Funcionario.DetalleContratacion.FirstOrDefault()));
                    else
                        lista.Add(new CDetalleContratacionDTO { IdEntidad = 0 });

                    //[2] CExpedienteFuncionarioDTO
                    if (item.Funcionario.ExpedienteFuncionario.Count > 0)
                        lista.Add(CExpedienteL.ConvertirExpedienteADTO(item.Funcionario.ExpedienteFuncionario.FirstOrDefault()));
                    else
                        lista.Add(new CExpedienteFuncionarioDTO { IdEntidad = 0, NumeroExpediente = 0});

                    //[3] CHistorialEstadoCivilDTO
                    if (item.Funcionario.HistorialEstadoCivil.Count > 0)
                        lista.Add(CHistorialEstadoCivilL.ConvertirHistorialEstadoCivilADTO(item.Funcionario.HistorialEstadoCivil.FirstOrDefault()));
                    else
                        lista.Add(new CHistorialEstadoCivilDTO { IdEntidad = 0, CatEstadoCivil= new CCatEstadoCivilDTO { IdEntidad = 0, DesEstadoCivil = "SD" } });


                    //[4] CInformacionContactoDTO
                    lista.Add(item.Funcionario.InformacionContacto.Where(Q => Q.TipoContacto.PK_TipoContacto == 3).ToList().Count > 0 ? // Celular
                                CInformacionContactoL.ConvertirInformacionContactoADTO(item.Funcionario.InformacionContacto.Where(Q => Q.TipoContacto.PK_TipoContacto == 3).FirstOrDefault()) :
                                new CInformacionContactoDTO { IdEntidad = 0,  DesContenido ="SD", TipoContacto = new CTipoContactoDTO { IdEntidad = 0, DesTipoContacto = "SD" } });

                    //[5] CNombramientoDTO
                    lista.Add(CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento));

                    //[6] CDetallePuestoDTO
                    lista.Add(CDetallePuestoL.ConstruirDetallePuesto(item.DetallePuesto));

                    //[7] CDivisionDTO
                    lista.Add(item.Division != null ?
                                CDivisionL.ConvertirDivisionADTO(item.Division) :
                                new CDivisionDTO() { IdEntidad = 0, NomDivision = "" });

                    //[8] CDireccionGeneralDTO
                    lista.Add(item.DireccionGeneral != null ?
                                CDireccionGeneralL.ConvertirDireccionGeneralADTO(item.DireccionGeneral) :
                                new CDireccionGeneralDTO() { IdEntidad = 0, NomDireccion = "" });

                    //[9] CDepartamentoDTO
                    lista.Add(item.Departamento != null ?
                                CDepartamentoL.ConstruirDepartamentoDTO(item.Departamento) :
                                new CDepartamentoDTO() { IdEntidad = 0, NomDepartamento = "" });

                    //[10] CSeccionDTO
                    lista.Add(item.Seccion != null ? 
                                CSeccionL.ConvertirSeccionADTO(item.Seccion) : 
                                new CSeccionDTO() { IdEntidad = 0, NomSeccion = "" });

                    //[11] CPresupuestoDTO
                    lista.Add(item.Puesto.UbicacionAdministrativa.Presupuesto != null ?
                                    new CPresupuestoDTO
                                    {
                                        IdEntidad = item.Puesto.UbicacionAdministrativa.Presupuesto.PK_Presupuesto,
                                        CodigoPresupuesto = item.Puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto
                                    }
                                    : new CPresupuestoDTO() { IdEntidad = 0, CodigoPresupuesto = "SD" });



                    // Salarios 
                    var resultadoSalario = ListarPagosFuncionario(item.Funcionario.IdCedulaFuncionario);
                    if (resultadoSalario.GetType() != typeof(CErrorDTO))
                    {
                        //[12] Primer Salario
                        lista.Add((CDesgloseSalarialDTO)resultadoSalario[0]);
                        //[13] Último Salario
                        lista.Add((CDesgloseSalarialDTO)resultadoSalario[1]);
                    }
                    else
                    {
                        //[12] Primer Salario
                        lista.Add(new CDesgloseSalarialDTO { IdEntidad = 0, MontoSalarioBruto = 0 });
                        //[13] Último Salario
                        lista.Add(new CDesgloseSalarialDTO { IdEntidad = 0, MontoSalarioBruto = 0 });
                    }


                    // Ubicación del puesto
                    var ubicacionesPuesto = item.DetallePuesto.Puesto.RelPuestoUbicacion;
                    List<CBaseDTO> ubicaciones = new List<CBaseDTO>();

                    var datoUbicacion = (new CUbicacionPuestoDTO
                    {
                        IdEntidad = 0,
                        TipoUbicacion = new CTipoUbicacionDTO
                        {
                            IdEntidad = 0,
                            DesTipoUbicacion = "SD"
                        },
                        Distrito = new CDistritoDTO
                        {
                            IdEntidad = 0,
                            NomDistrito = "SD",
                            Canton = new CCantonDTO
                            {
                                IdEntidad = 0,
                                NomCanton = "SD",
                                Provincia = new CProvinciaDTO
                                {
                                    IdEntidad = 0,
                                    NomProvincia = "SD"
                                }
                            }
                        }
                    });

                    if (ubicacionesPuesto != null)
                    {
                        // Contrato
                        var ubi = ubicacionesPuesto.Where(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1).FirstOrDefault();
                        if (ubi != null)
                            ubicaciones.Add(ConstruirUbicacionPuesto(ubi, new CUbicacionPuestoDTO()));
                        else
                        {
                            datoUbicacion.TipoUbicacion.IdEntidad = 1; 
                            ubicaciones.Add(datoUbicacion);
                        }

                        // Trabajo
                        ubi = ubicacionesPuesto.Where(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 2).FirstOrDefault();
                        if (ubi != null)
                            ubicaciones.Add(ConstruirUbicacionPuesto(ubi, new CUbicacionPuestoDTO()));
                        else
                        {
                            datoUbicacion.TipoUbicacion.IdEntidad = 2;
                            ubicaciones.Add(datoUbicacion);
                        }
                    }
                    else
                    {
                        datoUbicacion.TipoUbicacion.IdEntidad = 1; // Contrato
                        ubicaciones.Add(datoUbicacion);
                        datoUbicacion.TipoUbicacion.IdEntidad = 2; // Trabajo
                        ubicaciones.Add(datoUbicacion);
                    }

                    //[14] Ubicación Contrato
                    lista.Add(ubicaciones[0]);
                    //[15] Ubicación Trabajo
                    lista.Add(ubicaciones[1]);

                    //[16] Domicilio
                    if (item.Funcionario.Direccion.FirstOrDefault() != null)
                    {
                        lista.Add(CDireccionL.ConvertirDireccionADTO(item.Funcionario.Direccion.FirstOrDefault()));
                    }
                    else
                    {
                        var datoDireccion = new CDireccionDTO();
                        datoDireccion.Distrito = datoUbicacion.Distrito;
                        lista.Add(datoDireccion);
                    }

                    //[17]  Salario Actual
                    var datoSal = intermedioSalario.ObtenerSalario(item.Funcionario.IdCedulaFuncionario);
                    lista.Add((CSalarioDTO)datoSal.ElementAt(1));

                    respuesta.Add(lista);

                    //
                    //var datoDetalle = item.Puesto.DetallePuesto.Where(Q => Q.FK_Nombramiento == item.PK_Nombramiento).FirstOrDefault();
                    //if (datoDetalle == null)
                    //{
                    //    datoDetalle = item.Puesto.DetallePuesto.OrderByDescending(Q => Q.PK_DetallePuesto).FirstOrDefault();
                    //}
                    //datoNombramiento.Puesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(datoDetalle);
                    //respuesta.Add(datoNombramiento);
                }
            }

            if (respuesta.Count == 0)
            {
                List<CBaseDTO> lista = new List<CBaseDTO>();
                lista.Add(new CErrorDTO { IdEntidad = -1, MensajeError = "No se encontraron datos" });
                respuesta.Add(lista);
            }

            return respuesta;
        }

        public List<CBaseDTO> ListarNombramientosFuncionario(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CPlanificacionD intermedio = new CPlanificacionD(contexto);
            CNombramientoDTO datoNombramiento = new CNombramientoDTO();

            var nombramientos = intermedio.ListarNombramientosFuncionario(cedula);

            if (nombramientos != null)
            {
                foreach (var item in (List<Nombramiento>)nombramientos)
                {
                    datoNombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item);
                    var datoDetalle = item.Puesto.DetallePuesto.Where(Q => Q.FK_Nombramiento == item.PK_Nombramiento).FirstOrDefault();
                    if (datoDetalle == null)
                    {
                        datoDetalle = item.Puesto.DetallePuesto.OrderByDescending(Q => Q.PK_DetallePuesto).FirstOrDefault();
                    }
                    datoNombramiento.Puesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(datoDetalle);
                    respuesta.Add(datoNombramiento);
                }
            }

            if(respuesta.Count == 0)
            {
                respuesta.Add( new CErrorDTO { IdEntidad = -1, MensajeError = "No tiene Nombramientos registrados"});
            }

            return respuesta;
        }

        public List<CBaseDTO> CrearSolicitudVacia()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CCatCampoL intermedio = new CCatCampoL();
            CSolicitudCambioDTO datoSolicitud = new CSolicitudCambioDTO();
            CDetalleSolicitudCambioDTO datoDetalle = new CDetalleSolicitudCambioDTO();

            datoSolicitud.Detalle = new List<CDetalleSolicitudCambioDTO>();

            var datos = intermedio.ListarCampos();

            if (datos != null)
            {
                foreach (var item in datos)
                {
                    datoDetalle = new CDetalleSolicitudCambioDTO();
                    datoDetalle.Campo = (CCatCampoDTO)item;
                    datoDetalle.Aplica = true;
                    datoSolicitud.Detalle.Add(datoDetalle);
                }

                respuesta.Add(datoSolicitud);
            }

            if (respuesta.Count == 0)
            {
                respuesta.Add(new CErrorDTO { IdEntidad = -1, MensajeError = "No se pudo cargar la Solicituda de Cambio" });
            }

            return respuesta;
        }

        public List<CBaseDTO> ListarPagosFuncionario(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CDesgloseSalarialDTO datoPrimerSalario = new CDesgloseSalarialDTO();
            CDesgloseSalarialDTO datoUltSalario = new CDesgloseSalarialDTO();
            CPrestacionLegalD intermedio = new CPrestacionLegalD(contexto);
            CHistoricoPlanillaL intermediohistoricoPlanilla = new CHistoricoPlanillaL();

            var datosDesglose = intermedio.BuscarFuncionarioDesgloceSalarial(cedula);
            if (datosDesglose.Codigo > 0)
            {
                var listaDesglose = (List<DesgloseSalarial>)datosDesglose.Contenido;
                var primerSalario = listaDesglose.OrderBy(Q => Q.IndPeriodo).FirstOrDefault();
                var ultSalario = listaDesglose.OrderByDescending(Q => Q.IndPeriodo).FirstOrDefault();
               
                if (primerSalario != null)
                {
                    datoPrimerSalario.Periodo = primerSalario.IndPeriodo.ToShortDateString();
                    datoPrimerSalario.MontoSalarioBruto = Convert.ToDecimal(primerSalario.DetalleDesgloseSalarial.Sum(Q => Q.MtoPagocomponenteSalarial)) * 2;
                }

                if (ultSalario != null)
                {
                    datoUltSalario.Periodo = ultSalario.IndPeriodo.ToShortDateString();
                    datoUltSalario.MontoSalarioBruto = Convert.ToDecimal(ultSalario.DetalleDesgloseSalarial.Sum(Q => Q.MtoPagocomponenteSalarial)) * 2;
                }

                /// DATOS HISTÓRICO
                DateTime fechaInicio = new DateTime();
                DateTime fechaFinal = new DateTime();

                var datohistoricoPlanilla = intermediohistoricoPlanilla.BuscarDatosPlanilla(cedula, fechaInicio, fechaFinal);
                if (datohistoricoPlanilla.ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    var primerSalarioH = (CHistoricoPlanillaDTO)datohistoricoPlanilla.FirstOrDefault();
                    var ultSalarioH = (CHistoricoPlanillaDTO)datohistoricoPlanilla.LastOrDefault();

                    if (primerSalarioH != null)
                    {
                        var a = primerSalarioH.FechaPeriodo.Substring(0, 4);
                        var m = primerSalarioH.FechaPeriodo.Substring(5, 2);
                        var d = primerSalarioH.FechaPeriodo.Substring(8, 2);
                        if (m == "02" && Convert.ToInt32(d) > 28)
                            d = "28";
                        fechaInicio = new DateTime(Convert.ToInt32(a),
                                                   Convert.ToInt32(m),
                                                   Convert.ToInt32(d));
                        if (primerSalario != null && fechaInicio < primerSalario.IndPeriodo ) {
                            datoPrimerSalario.Periodo = fechaInicio.ToShortDateString();
                            decimal salario = 0;
                            try
                            {
                                salario = Convert.ToDecimal(primerSalarioH.SalarioMensual);
                            }
                            catch
                            {
                                try
                                {
                                    salario = Convert.ToDecimal(primerSalarioH.SalarioMensual.Replace(',', '.'));
                                }
                                catch
                                {
                                    salario = Convert.ToDecimal(primerSalarioH.SalarioMensual.Replace('.', ','));
                                }
                            }
                            datoPrimerSalario.MontoSalarioBruto = salario;
                        }
                    }                   
                }

                respuesta.Add(datoPrimerSalario);
                respuesta.Add(datoUltSalario);
            }
           

            if (respuesta.Count == 0)
            {
                respuesta.Add(new CErrorDTO { IdEntidad = -1, MensajeError = "No tiene pagos registrados" });
            }

            return respuesta;
        }
        
        private CUbicacionPuestoDTO ConstruirUbicacionPuesto(RelPuestoUbicacion entrada, CUbicacionPuestoDTO salida)
        {
            salida.IdEntidad = entrada.UbicacionPuesto.PK_UbicacionPuesto;
            salida.ObsUbicacionPuesto = entrada.UbicacionPuesto.ObsUbicacionPuesto;
            salida.TipoUbicacion = new CTipoUbicacionDTO
            {
                IdEntidad = entrada.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion,
                DesTipoUbicacion = entrada.UbicacionPuesto.TipoUbicacion.DesTipoUbicacion
            };
            salida.Distrito = new CDistritoDTO
            {
                IdEntidad = entrada.UbicacionPuesto.Distrito.PK_Distrito,
                NomDistrito = entrada.UbicacionPuesto.Distrito.NomDistrito,
                Canton = new CCantonDTO
                {
                    IdEntidad = entrada.UbicacionPuesto.Distrito.Canton.PK_Canton,
                    NomCanton = entrada.UbicacionPuesto.Distrito.Canton.NomCanton,
                    Provincia = new CProvinciaDTO
                    {
                        IdEntidad = entrada.UbicacionPuesto.Distrito.Canton.Provincia.PK_Provincia,
                        NomProvincia = entrada.UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia
                    }
                }
            };
            return salida;
        }

        public List<CBaseDTO> ListarClases()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CClaseD intermedio = new CClaseD(contexto);
            var clases = intermedio.CargarClases();
            if (clases != null)
            {
                foreach (var item in clases)
                {
                    respuesta.Add(new CClaseDTO
                    {
                        IdEntidad = item.PK_Clase,
                        DesClase = item.DesClase,
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron clases" });
            }
            return respuesta;
        }

        public List<CBaseDTO> ListarEstadoCivil()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CCatEstadoCivilD intermedio = new CCatEstadoCivilD(contexto);
            var estados = intermedio.CargarCatEstadosCiviles();
            if (estados != null)
            {
                foreach (var item in estados)
                {
                    respuesta.Add(new CCatEstadoCivilDTO
                    {
                        IdEntidad = item.PK_CatEstadoCivil,
                        DesEstadoCivil = item.DesEstadoCivil,
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron estados civiles" });
            }
            return respuesta;
        }

        public List<CBaseDTO> ListarEstadoFuncionario()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CPlanificacionD intermedio = new CPlanificacionD(contexto);
            var estados = intermedio.CargarEstadosFuncionario();
            if (estados != null)
            {
                foreach (var item in estados)
                {
                    respuesta.Add(new CEstadoFuncionarioDTO
                    {
                        IdEntidad = item.PK_EstadoFuncionario,
                        DesEstadoFuncionario = item.DesEstadoFuncionario,
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron estados civiles" });
            }
            return respuesta;
        }

        public List<string[]> GenerarDatosReportes(List<CParametrosDTO> parametros)
        {
            int[] valores;

            decimal porcentaje = 0;

            decimal totalFuncionarios = 0;
            decimal totalHombres = 0;
            decimal porHombres = 0;
            decimal totalMujeres = 0;
            decimal porMujeres = 0;

            List<string[]> respuesta = new List<string[]>();
           
            CPlanificacionD intermedio = new CPlanificacionD(contexto);
            CDivisionD intermedioDivision = new CDivisionD(contexto);
            CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
            CDepartamentoD intermedioDepartamento = new CDepartamentoD(contexto);
            CSeccionD intermedioSeccion = new CSeccionD(contexto);
            CEspecialidadD intermedioEspecial = new CEspecialidadD(contexto);

            int[] listOfIds;
            string[] listOfNames;

            var divisiones = intermedioDivision.CargarDivisiones();
            var direcciones = intermedioDireccion.CargarDireccionesGenerales();
            var departamentos = intermedioDepartamento.CargarDepartamentos();
            var secciones = intermedioSeccion.CargarSecciones();

            var especialidades = intermedioEspecial.CargarEspecialidadesParams(0, "");

            var datos = intermedio.ListarNombramientosActivos();

            foreach (var item in parametros)
            {
                valores = new int[0];
                switch (item.nomParametro)
                {
                    case "Division":
                        if (item.valoresBuscar != null)
                        {
                            valores = Array.ConvertAll(item.valoresBuscar, s => int.Parse(s));
                            datos = datos.Where(Q => valores.Contains(Q.Division.PK_Division)).ToList();
                            divisiones = divisiones.Where(Q => valores.Contains(Q.PK_Division)).ToList();
                        }
                        break;

                    case "Direccion":
                        if (item.valoresBuscar != null)
                        {
                            valores = Array.ConvertAll(item.valoresBuscar, s => int.Parse(s));
                            datos = datos.Where(Q => valores.Contains(Q.DireccionGeneral != null ? Q.DireccionGeneral.PK_DireccionGeneral : 0)).ToList();
                            direcciones = direcciones.Where(Q => valores.Contains(Q.PK_DireccionGeneral)).ToList();
                        }
                        break;

                    case "Departamento":
                        if (item.valoresBuscar != null)
                        {
                            valores = Array.ConvertAll(item.valoresBuscar, s => int.Parse(s));
                            datos = datos.Where(Q => valores.Contains(Q.Departamento != null ? Q.Departamento.PK_Departamento : 0)).ToList();
                            departamentos = departamentos.Where(Q => valores.Contains(Q.PK_Departamento)).ToList();
                        }
                        break;

                    case "Seccion":
                        if (item.valoresBuscar != null)
                        {
                            valores = Array.ConvertAll(item.valoresBuscar, s => int.Parse(s));
                            datos = datos.Where(Q => valores.Contains(Q.Seccion != null ? Q.Seccion.PK_Seccion : 0)).ToList();
                            secciones = secciones.Where(Q => valores.Contains(Q.PK_Seccion)).ToList();
                        }
                        break;

                    case "Especialidad":
                        if (item.valoresBuscar != null)
                        {
                            valores = Array.ConvertAll(item.valoresBuscar, s => int.Parse(s));
                            datos = datos.Where(Q => valores.Contains(Q.DetallePuesto.Especialidad != null ? Q.DetallePuesto.Especialidad.PK_Especialidad : 0)).ToList();
                            especialidades = especialidades.Where(Q => valores.Contains(Q.PK_Especialidad)).ToList();
                        }
                        break;

                    default:
                        break;
                }
            }

            //var datosDivisiones = string.Join(",", divisiones.OrderBy(Q=> Q.PK_Division).Select(Q => Q.PK_Division).ToArray());
            //var labelDivisiones = string.Join(",", divisiones.OrderBy(Q => Q.PK_Division).Select(Q => Q.NomDivision.ToUpper().TrimEnd()).ToArray());

            if (datos != null)
            {
                // [0][]  TOTAL FUNCIONARIOS 
                totalFuncionarios = datos.Count();
                totalHombres = datos.Where(Q => Q.Funcionario.IndSexo == "1").Count();
                porHombres = Math.Round(totalHombres * 100 / totalFuncionarios, 2);
                totalMujeres = totalFuncionarios - totalHombres;
                porMujeres = 100 - porHombres;

                // [0][]  Total 
                respuesta.Add(new string[]
                    {
                        totalFuncionarios.ToString(),
                        totalHombres.ToString(),
                        porHombres.ToString(),
                        totalMujeres.ToString(),
                        porMujeres.ToString(),
                    });
            }
            else
            {
                return new List<string[]> { new string[] { "No se encontraron datos" } };
            }


            /// [1][] REPORTE POR RANGO DE EDAD POR GÉNERO
            var edadRangos = new[] { 30, 40, 50, 60, 70, 100 };

            var datosEdad = (from u in datos
                             select new
                             {
                                 Id = u.PK_Funcionario,
                                 Sexo = u.Funcionario.IndSexo,
                                 Edad = CalcularEdad(u.Funcionario.FecNacimiento)
                             }).ToList();

            totalFuncionarios = datosEdad.Count();
            var strGrupoEdad = "";
            var porcGrupoEdad = "";

            //---Hombres
            var grupoEdad = datosEdad.Where(Q => Q.Sexo == "1").OrderBy(Q => Q.Edad).GroupBy(x => edadRangos.FirstOrDefault(R => R > x.Edad)).ToList();

            foreach (var nameGroup in grupoEdad)
            {
                if (strGrupoEdad != "")
                    strGrupoEdad += ",";
                strGrupoEdad += nameGroup.Count();

                if (porcGrupoEdad != "")
                    porcGrupoEdad += ",";

                porcentaje = Math.Round(nameGroup.Count() * 100 / totalFuncionarios, 2);
                porcGrupoEdad += "'" + porcentaje.ToString() + "'";
            }

            var strGrupoEdadH = strGrupoEdad;
            var porcGrupoEdadH = porcGrupoEdad;


            //---Mujeres
            grupoEdad = datosEdad.Where(Q => Q.Sexo == "2").OrderBy(Q => Q.Edad).GroupBy(x => edadRangos.FirstOrDefault(R => R > x.Edad)).ToList();
            strGrupoEdad = "";
            porcGrupoEdad = "";

            foreach (var nameGroup in grupoEdad)
            {
                if (strGrupoEdad != "")
                    strGrupoEdad += ",";
                strGrupoEdad += nameGroup.Count();

                if (porcGrupoEdad != "")
                    porcGrupoEdad += ",";

                porcentaje = Math.Round(nameGroup.Count() * 100 / totalFuncionarios, 2);
                porcGrupoEdad += "'" + porcentaje.ToString() + "'";
            }

            // Sumar Hombres y Mujeres
            grupoEdad = datosEdad.OrderBy(Q => Q.Edad).GroupBy(x => edadRangos.FirstOrDefault(R => R > x.Edad)).ToList();
            var strTotal = "";
            foreach (var nameGroup in grupoEdad)
            {
                if (strTotal != "")
                    strTotal += ",";
                strTotal += nameGroup.Count();
            }

            // [1][] REPORTE POR RANGO DE EDAD POR GÉNERO
            respuesta.Add(new string[]
                    {
                        "20-29,30-39,40-49,50-59,60-69,70+", // Label del Gráfico
                        strGrupoEdadH, // Total Hombres por Edad
                        porcGrupoEdadH, // Porc Hombres por Edad
                        strGrupoEdad, // Total Mujeres por Edad
                        porcGrupoEdad, // Porc Mujeres por Edad
                        strTotal // Total de Funcionarios
                    });


            // [2][] FUNCIONARIOS SEGÚN CONDICIÓN DE NOMBRAMIENTO POR DIVISION
            var labelDivisiones = "";
            var strGrupoPropiedad = "";
            var strGrupoInterino = "";

            listOfIds = datos.Select(x => x.Division.PK_Division).OrderBy(x => x).ToArray();
            var listaDivisiones = divisiones.Where(Q => listOfIds.Contains(Q.PK_Division)).ToList();
            foreach (var item in listaDivisiones.OrderBy(Q => Q.PK_Division).ToList())
            {
                if (labelDivisiones != "")
                    labelDivisiones += ",";
                if (strGrupoPropiedad != "")
                    strGrupoPropiedad += ",";
                if (strGrupoInterino != "")
                    strGrupoInterino += ",";

                labelDivisiones += item.NomDivision.ToUpper().TrimEnd();
                strGrupoPropiedad += datos.Where(Q => Q.IndPropiedad == 1 && Q.Division.PK_Division == item.PK_Division).Count();
                strGrupoInterino += datos.Where(Q => Q.IndPropiedad == 0 && Q.Division.PK_Division == item.PK_Division).Count();
            }

            // [2][] FUNCIONARIOS SEGÚN CONDICIÓN DE NOMBRAMIENTO POR DIVISION
            respuesta.Add(new string[]
                    {
                        labelDivisiones, // Label del Gráfico
                        strGrupoPropiedad, // Total Funcionarios Propiedad por División
                        strGrupoInterino // Total Funcionarios Interinos por División
                    });

            // [3][] FUNCIONARIOS SEGÚN ESPECIALIDAD
            var strEspecialidades = "";
            listOfIds = datos.Where(Q => Q.DetallePuesto.Especialidad != null)
                             .OrderBy(Q => Q.DetallePuesto.Especialidad.PK_Especialidad)
                             .Select(Q => Q.DetallePuesto.Especialidad.PK_Especialidad)
                             .Distinct().ToArray();
            listOfNames = datos.Where(Q => Q.DetallePuesto.Especialidad != null)
                                .OrderBy(Q => Q.DetallePuesto.Especialidad.DesEspecialidad)
                                .Select(Q => Q.DetallePuesto.Especialidad.DesEspecialidad.Replace(",", "").ToUpper().TrimEnd())
                                .Distinct().ToArray();

            foreach (var item in listOfIds)
            {
                if (strEspecialidades != "")
                    strEspecialidades += ",";

                strEspecialidades += datos.Where(Q => Q.DetallePuesto.Especialidad != null && Q.DetallePuesto.Especialidad.PK_Especialidad == item).Count();
            }

            // [3][] FUNCIONARIOS SEGÚN ESPECIALIDAD
            respuesta.Add(new string[]
                    {
                        string.Join(",", listOfNames), // Label del Gráfico
                        strEspecialidades
                    });

            // [4][] GRÁFICO DE GENERO POR FAMILIA DE PUESTO - ESTRATOS
            listOfIds = datos.OrderBy(Q => Q.Puesto.FamiliaPuestos.PK_Familia).Select(Q => Q.Puesto.FamiliaPuestos.PK_Familia).Distinct().ToArray();
            listOfNames = datos.OrderBy(Q => Q.Puesto.FamiliaPuestos.PK_Familia).Select(Q => Q.Puesto.FamiliaPuestos.DesFamilia.Replace(",", "")).Distinct().ToArray();

            strGrupoEdadH = "";
            porcGrupoEdadH = "";
            strGrupoEdad = "";
            porcGrupoEdad = "";
            var totalGrupo = 0;
            foreach (var item in listOfIds)
            {
                // Hombres
                if (strGrupoEdadH != "")
                    strGrupoEdadH += ",";
                totalGrupo = datos.Where(Q => Q.Funcionario.IndSexo == "1" && Q.Puesto.FamiliaPuestos.PK_Familia == item).Count();
                strGrupoEdadH += totalGrupo;

                if (porcGrupoEdadH != "")
                    porcGrupoEdadH += ",";

                porcentaje = Math.Round(totalGrupo * 100 / totalFuncionarios, 2);
                porcGrupoEdadH += "'" + porcentaje.ToString() + "'";

                // Mujeres
                if (strGrupoEdad != "")
                    strGrupoEdad += ",";
                totalGrupo = datos.Where(Q => Q.Funcionario.IndSexo == "2" && Q.Puesto.FamiliaPuestos.PK_Familia == item).Count();
                strGrupoEdad += totalGrupo;

                if (porcGrupoEdad != "")
                    porcGrupoEdad += ",";

                porcentaje = Math.Round(totalGrupo * 100 / totalFuncionarios, 2);
                porcGrupoEdad += "'" + porcentaje.ToString() + "'";
            }

            // [4][] GRÁFICO DE GENERO POR ESTRATO
            respuesta.Add(new string[]
              {
                        string.Join(",", listOfNames), // Label del Gráfico
                        strGrupoEdadH, // Total Hombres por Edad
                        porcGrupoEdadH, // Porc Hombres por Edad
                        strGrupoEdad, // Total Mujeres por Edad
                        porcGrupoEdad // Porc Mujeres por Edad
              });

            return respuesta;
        }

        private int CalcularEdad(DateTime? fechaNacimiento)
        {
            int edad = 0;

            if (fechaNacimiento != null)
            {
                DateTime today = DateTime.Today;
                edad = today.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento > today.AddYears(-edad))
                    edad--;
            }

            return edad;
        }

        #endregion
    }
}

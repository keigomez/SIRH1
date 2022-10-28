using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDesarraigoL
    {

        # region Variables

        // Versión para Orlando

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDesarraigoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CDesarraigoDTO ConvertirDesarraigoDatosaDTO(Desarraigo item)
        {
            List<CDetalleDesarraigoEliminacionDTO> listaDetalle = new List<CDetalleDesarraigoEliminacionDTO>();

            foreach (var detalle in item.DetalleDesarraigoEliminacion)
            {
                listaDetalle.Add(new CDetalleDesarraigoEliminacionDTO
                {
                    IdEntidad = detalle.PK_Detalle,
                    FecEliminacion = detalle.FecEliminacion,
                    FecRegistro = detalle.FecRegistro,
                    ObsEliminacion = detalle.ObsEliminacion,
                    EstadoDesarraigo = ConstruirEstadoDesarraigo(detalle.EstadoDesarraigo),
                });
            }

            return new CDesarraigoDTO
            {
                IdEntidad = item.PK_Desarraigo,
                Nombramiento = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                EstadoDesarraigo = ConstruirEstadoDesarraigo(item.EstadoDesarraigo),
                MontoDesarraigo = Convert.ToDecimal(item.MonDesarraigo),
                MontoSalario = Convert.ToDecimal(item.MonSalario),
                FechaFin = Convert.ToDateTime(item.FecFinDesarraigo),
                FechaInicio = Convert.ToDateTime(item.FecInicDesarraigo),
                FechaRegistro = Convert.ToDateTime(item.FecRegistro),
                ObservacionesDesarraigo = item.ObsDesarraigo == null ? "" : item.ObsDesarraigo,
                ObservacionesEstado = item.ObsEstado == null ? "" : item.ObsEstado,
                DocAdjunto = item.ImgDocumento,
                NumContrato = item.NumContrato,
                NomProyecto = item.DesNomProyecto,
                LugarPernocte = item.DesPernocte,
                Presupuesto = CPresupuestoL.ConvertirPresupuestoDatosaDTO(item.Presupuesto),
                Seccion = item.FK_Seccion != null ?
                                    CSeccionL.ConvertirSeccionADTO(item.Seccion)
                                    : new CSeccionDTO(),
                Departamento = item.FK_Departamento != null ?
                                    CDepartamentoL.ConstruirDepartamentoDTO(item.Departamento)
                                    : new CDepartamentoDTO(),
                DistritoContrato = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito)
                                    : new CDistritoDTO(),
                DistritoPernocte = item.FK_DistritoPernocte != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito1)
                                    : new CDistritoDTO(),
                DistritoTrabajo = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito2)
                                    : new CDistritoDTO(),
                JefeInmediato = item.IdJefe > 0 ?
                            (CFuncionarioDTO)ObtenerFuncionarioID(item.IdJefe)
                            : new CFuncionarioDTO
                            {
                                IdEntidad = Convert.ToInt32(item.IdJefe),
                                Sexo = GeneroEnum.Indefinido
                            },
                RegistradoPor = item.IdRegistradoPor > 0 ?
                            (CFuncionarioDTO)ObtenerFuncionarioID(item.IdRegistradoPor)
                            : new CFuncionarioDTO
                            {
                                IdEntidad = Convert.ToInt32(item.IdRegistradoPor),
                                Sexo = GeneroEnum.Indefinido
                            },
                AnalizadoPor = item.IdAnalizadoPor > 0 ?
                            (CFuncionarioDTO)ObtenerFuncionarioID(item.IdAnalizadoPor)
                            : new CFuncionarioDTO
                            {
                                IdEntidad = Convert.ToInt32(item.IdAnalizadoPor),
                                Sexo = GeneroEnum.Indefinido
                            },
                RevisadoPor = item.IdRevisadoPor > 0 ?
                            (CFuncionarioDTO)ObtenerFuncionarioID(item.IdRevisadoPor)
                            : new CFuncionarioDTO
                            {
                                IdEntidad = Convert.ToInt32(item.IdRevisadoPor),
                                Sexo = GeneroEnum.Indefinido
                            },
                AprobadoPor = item.IdAprobadoPor > 0 ?
                            (CFuncionarioDTO)ObtenerFuncionarioID(item.IdAprobadoPor)
                            : new CFuncionarioDTO
                            {
                                IdEntidad = Convert.ToInt32(item.IdAprobadoPor),
                                Sexo = GeneroEnum.Indefinido
                            },
                DetalleEliminacion = listaDetalle
            };
        }

        //internal static CDesarraigoDTO ConstruirDesarraigoGeneral(Desarraigo item)
        //{
        //    return new CDesarraigoDTO
        //    {
        //        Nombramiento = CNombramientoL.NombramientoGeneral(item.Nombramiento),
        //        MontoDesarraigo = Convert.ToDecimal(item.MonDesarraigo),
        //        FechaFin = Convert.ToDateTime(item.FecFinDesarraigo),
        //        FechaInicio = Convert.ToDateTime(item.FecInicDesarraigo),
        //        ObservacionesDesarraigo = item.ObsDesarraigo == null ? "" : item.ObsDesarraigo,
        //        IdEntidad = item.PK_Desarraigo,
        //        EstadoDesarraigo = ConstruirEstadoDesarraigo(item.EstadoDesarraigo)
        //    };
        //}

        internal static CEstadoDesarraigoDTO ConstruirEstadoDesarraigo(EstadoDesarraigo item)
        {
            return new CEstadoDesarraigoDTO
            {
                IdEntidad = item.PK_EstadoDesarraigo,
                NomEstadoDesarraigo = item.NomEstadoDesarraigo
            };
        }

        private void AgregarFacturas(List<CFacturaDesarraigoDTO> facturas, Desarraigo desarraigoDB, CBaseDTO respuesta)
        {
            FacturaDesarraigo facturaDB;
            CRespuestaDTO agregadoFactura;
            CFacturaDesarraigoD intermedioFactura = new CFacturaDesarraigoD(contexto);
            foreach (var f in facturas)
            {
                facturaDB = new FacturaDesarraigo
                {
                    CodFactura = f.CodigoFactura.ToUpper(),
                    EmisorFactura = f.Emisor.ToUpper(),
                    FecFacturacion = f.FechaFacturacion,
                    MonFactura = f.MontoFactura,
                    ObsConcepto = f.ObsConcepto
                };
                agregadoFactura = intermedioFactura.AgregarFactura(desarraigoDB, facturaDB);
                if (agregadoFactura.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)agregadoFactura.Contenido).MensajeError);
                }
            }
        }

     
        private void AgregarContratos(List<CContratoArrendamientoDTO> contratos, Desarraigo desarraigoDB, CBaseDTO respuesta)
        {
            ContratoArrendamiento contratoDB;
            CRespuestaDTO agregadoContrato;
            CContratoArrendamientoD intermedioContrato = new CContratoArrendamientoD(contexto);
            foreach (var c in contratos)
            {
                contratoDB = new ContratoArrendamiento
                {
                    EmisorContrato = c.EmisorContrato.ToUpper(),
                    MonContrato = c.MontoContrato,
                    FecInicial = c.FechaInicio,
                    FecFinal = c.FechaFin,
                    CodContrato = c.CodigoContratoArrendamiento.ToUpper(),
                };
                agregadoContrato = intermedioContrato.AgregarContrArrendamiento(desarraigoDB, contratoDB);
                if (agregadoContrato.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)agregadoContrato.Contenido).MensajeError);
                }
            }
        }


        public List<List<CBaseDTO>> ActualizarVencimientoDesarraigo(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);

                var desarraigo = intermedio.ActualizarVencimientoDesarraigo(fechaVencimiento);

                if (desarraigo.Codigo > 0)
                {

                    var datosDesarraigo = (List<Desarraigo>)(((List<object>)desarraigo.Contenido)[0]);
                    var codigos = (List<string>)(((List<object>)desarraigo.Contenido)[1]);

                    for (int i = 0; i < datosDesarraigo.Count; i++)
                    {
                        List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                        var desAux = CDesarraigoL.ConvertirDesarraigoDatosaDTO(datosDesarraigo[i]);
                        desAux.CodigoDesarraigo = codigos[i];
                        desarragosList.Add(desAux);
                        desarragosList.Add(CFuncionarioL.FuncionarioGeneral(datosDesarraigo[i].Nombramiento.Funcionario));
                        respuesta.Add(desarragosList);
                    }
                }
                else
                {
                    List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                    desarragosList.Add((CErrorDTO)desarraigo.Contenido);
                    respuesta.Add(desarragosList);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                desarragosList.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(desarragosList);
            }

            return respuesta;
        }

        public CBaseDTO AgregarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo,
                                          List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDesarraigoD intermedioDesarraigo = new CDesarraigoD(contexto);
                CEstadoDesarraigoD intermedioEstado = new CEstadoDesarraigoD(contexto);
                CDistritoD intermedioDistrito = new CDistritoD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);
                CDepartamentoD intermedioDepartamento = new CDepartamentoD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);

                var desarraigoDB = new Desarraigo
                {
                    FK_EstadoDesarraigo = 1,// Espera
                    FK_Nombramiento = desarraigo.Nombramiento.IdEntidad,
                    FecFinDesarraigo = desarraigo.FechaFin,
                    FecInicDesarraigo = desarraigo.FechaInicio,
                    FecRegistro = DateTime.Now,
                    MonDesarraigo = desarraigo.MontoDesarraigo,
                    MonSalario = desarraigo.MontoSalario,
                    ObsDesarraigo = desarraigo.ObservacionesDesarraigo,
                    ObsEstado = "",
                    NumContrato = "",
                    ImgDocumento = desarraigo.DocAdjunto,
                    Distrito = intermedioDistrito.CargarDistritoId(desarraigo.DistritoContrato.IdEntidad),
                    Distrito1 = intermedioDistrito.CargarDistritoId(desarraigo.DistritoPernocte.IdEntidad),
                    Distrito2 = intermedioDistrito.CargarDistritoId(desarraigo.DistritoTrabajo.IdEntidad),
                    Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(desarraigo.Presupuesto.CodigoPresupuesto).Contenido,
                    IdJefe = desarraigo.JefeInmediato.IdEntidad,
                    DesNomProyecto = desarraigo.NomProyecto,
                    DesPernocte = desarraigo.LugarPernocte,
                    IdRegistradoPor = desarraigo.RegistradoPor.IdEntidad,
                    IdAnalizadoPor = 0,
                    IdRevisadoPor = 0,
                    IdAprobadoPor = 0,
                    NumAccionAprobado = "",
                    NumAccionEliminado = ""
                };

                if (desarraigo.Seccion != null && desarraigo.Seccion.IdEntidad > 0)
                    desarraigoDB.Seccion = intermedioSeccion.CargarSeccionPorID(desarraigo.Seccion.IdEntidad);

                if (desarraigo.Departamento != null && desarraigo.Departamento.IdEntidad > 0)
                    desarraigoDB.Departamento = intermedioDepartamento.CargarDepartamentoPorID(desarraigo.Departamento.IdEntidad);

                var funcionarioDB = new Funcionario { IdCedulaFuncionario = funcionario.Cedula };

                var agregadoDesarraigo = intermedioDesarraigo.AgregarDesarraigo(funcionarioDB, desarraigoDB);
                if (agregadoDesarraigo.Codigo > 0)
                {
                    AgregarFacturas(facturas, desarraigoDB, respuesta);
                    AgregarContratos(contratos, desarraigoDB, respuesta);
                    respuesta = agregadoDesarraigo;
                    return respuesta;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoDesarraigo).Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> BuscarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo,
                                    List<DateTime> fechas, CDistritoDTO distrito)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CDesarraigoD intermedio = new CDesarraigoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();
            List<Desarraigo> datosDesarraigo = new List<Desarraigo>();
            List<object> parametros = new List<object>();
            List<string> codigos = new List<string>();
            CRespuestaDTO resultado = new CRespuestaDTO { Codigo = -1 }; ;

            bool buscar = true;

            if (funcionario.Cedula != null)
            {
                resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigo, funcionario.Cedula, "Cedula"));
                if (resultado.Codigo > 0)
                    datosDesarraigo = (List<Desarraigo>)resultado.Contenido;
                else
                    buscar = false;
            }

            if (fechas.Count > 0 && buscar)
            {
                resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigo, fechas, "Fechas"));
                if (resultado.Codigo > 0)
                    datosDesarraigo = (List<Desarraigo>)resultado.Contenido;
                else
                {
                    datosDesarraigo.Clear();
                    buscar = false;
                }
            }


            if (buscar) // Buscar por estado, Si deduccion.Estado == -1, no mostrar las Anuladas (idEstado = 2)
            {
                resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigo, desarraigo.EstadoDesarraigo.IdEntidad, "Estado"));
                if (resultado.Codigo > 0)
                    datosDesarraigo = (List<Desarraigo>)resultado.Contenido;
                else
                {
                    datosDesarraigo.Clear(); 
                    buscar = false;
                }
            }


            if (distrito.IdEntidad > 0 && buscar)
            {
                resultado = ((CRespuestaDTO)intermedio.BuscarDesarraigo(datosDesarraigo, distrito.IdEntidad, "Trabajo"));
                if (resultado.Codigo > 0)
                    datosDesarraigo = (List<Desarraigo>)resultado.Contenido;
                else
                    buscar = false;
            }

            if (datosDesarraigo.Count > 0)
            { 
                for (int i = 0; i < datosDesarraigo.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosDesarraigo[i];
                    var desa = ConvertirDesarraigoDatosaDTO(item);
                    //desa.CodigoDesarraigo = codigos[i];
                    temp.Add(desa);
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));
                    var detallePuesto = intermedioPuesto.DetallePuestoPorCodigo(item.Nombramiento.Puesto.CodPuesto);
                    temp.AddRange(detallePuesto);
                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                //temp.Add((CRespuestaDTO)datos.Contenido);
                respuesta.Add(temp);
            }
            return respuesta;
        }

        public List<List<CBaseDTO>> ObtenerDesarraigo(int codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);
                CPuestoL intermedioPuesto = new CPuestoL();

                var desarraigo = intermedio.ObtenerDesarraigo(codigo);
                List<CBaseDTO> temp = new List<CBaseDTO>();

                if (desarraigo.Codigo > 0)
                {
                    var datoDesarraigo = ConvertirDesarraigoDatosaDTO((Desarraigo)desarraigo.Contenido);
                    datoDesarraigo.CodigoDesarraigo = desarraigo.Mensaje;
                    temp.Add(datoDesarraigo); //0 - 0

                    var funcionario = ((Desarraigo)desarraigo.Contenido).Nombramiento.Funcionario;
                    var datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario);

                    temp.Add(datoFuncionario);//0 - 1

                    var nombramiento = ((Desarraigo)desarraigo.Contenido).Nombramiento;
                    var datosNombramiento = CNombramientoL.NombramientoGeneral(nombramiento);

                    temp.Add(datosNombramiento);//0 - 2

                    //var puesto = ((Desarraigo)desarraigo.Contenido).Nombramiento.Puesto;
                    // var datosPuesto = CPuestoL.ConstruirPuesto(puesto, new CPuestoDTO());

                    // temp.Add(datosPuesto); //0 - 3

                    var cartaPresentacion = ((Desarraigo)desarraigo.Contenido).Nombramiento.CartasPresentacion.FirstOrDefault();
                    var datosCarta = new CCartaPresentacionDTO();
                    if (cartaPresentacion != null)
                        datosCarta = CCartaPresentacionL.ConstruirCartaPresentacion(cartaPresentacion);
                    temp.Add(datosCarta); // 0 - 3

                    // Dirección Exacta     // 0 - 4
                    var direccion = (new CFuncionarioL()).BuscarInformacionBasicaFuncionario(datoFuncionario.Cedula);
                    if (direccion[0].GetType() != typeof(CErrorDTO))
                    {
                        temp.Add(direccion.ElementAtOrDefault(1));
                    }
                    else
                    {
                        temp.Add(new CDireccionDTO
                        {
                            IdEntidad = 0,
                            DirExacta = "",
                            Distrito = new CDistritoDTO { IdEntidad = 0 }
                        });
                    }

                    // CDetalle Contratacion    // 0 - 5
                    var datoContratoFuncionario = new CDetalleContratacionL().DescargarDetalleContratacion(funcionario.IdCedulaFuncionario);
                    if (datoContratoFuncionario.GetType() != typeof(CErrorDTO))
                    {
                        temp.Add(datoContratoFuncionario);
                    }
                    else
                    {
                        temp.Add(new CDetalleContratacionDTO
                        {
                            IdEntidad = 0,
                            NumeroAnualidades = 0
                        });
                    }

                    respuesta.Add(temp);

                    var puesto = ((Desarraigo)desarraigo.Contenido).Nombramiento.Puesto;
                    var detallePuesto = intermedioPuesto.DetallePuestoPorCodigo(puesto.CodPuesto);

                    respuesta.Add(detallePuesto); // 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]

                    var facturas = ((Desarraigo)desarraigo.Contenido).FacturaDesarraigo.ToList();
                    var datosFacturas = CFacturaDesarraigoL.ConstruirFacturaDesarraigo(facturas);

                    respuesta.Add(datosFacturas); // 2

                    var contratos = ((Desarraigo)desarraigo.Contenido).ContratoArrendamiento.ToList();
                    var datosContratos = CContratoArrendamientoL.ConstruirContratoArrendamiento(contratos);

                    respuesta.Add(datosContratos); // 3

                }
                else
                {
                    temp.Add((CErrorDTO)desarraigo.Contenido);
                    respuesta.Add(temp);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                respuesta.Add(temp);
            }
            return respuesta;
        }

        public List<List<CBaseDTO>> DesarraigosPorVencer(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);

                var desarraigo = intermedio.DesarraigoPorVencer(fechaVencimiento);

                if (desarraigo.Codigo > 0)
                {
                    var datosDesarraigo = (List<Desarraigo>)(((List<object>)desarraigo.Contenido)[0]);
                    var codigos = (List<string>)(((List<object>)desarraigo.Contenido)[1]);

                    for (int i = 0; i < datosDesarraigo.Count; i++)
                    {
                        List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                        var desAux = CDesarraigoL.ConvertirDesarraigoDatosaDTO(datosDesarraigo[i]);
                        desAux.CodigoDesarraigo = codigos[i];
                        desarragosList.Add(desAux);
                        desarragosList.Add(CFuncionarioL.FuncionarioGeneral(datosDesarraigo[i].Nombramiento.Funcionario));
                        respuesta.Add(desarragosList);
                    }
                }
                else
                {
                    List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                    desarragosList.Add((CErrorDTO)desarraigo.Contenido);
                    respuesta.Add(desarragosList);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> desarragosList = new List<CBaseDTO>();
                desarragosList.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(desarragosList);
            }

            return respuesta;
        }

        //public List<CBaseDTO> BuscarFuncionarioCedula(string cedula)
        //{
        //    List<CBaseDTO> respuesta = new List<CBaseDTO>();
        //    CPuestoL intermedioPuesto = new CPuestoL();
        //    try
        //    {
        //        respuesta = (new CFuncionarioL()).BuscarFuncionarioDetallePuesto(cedula);
        //        if (respuesta[0].GetType() != typeof(CErrorDTO))
        //        {
        //            var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(((CFuncionarioDTO)respuesta[0]).Cedula);// 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]
        //            if (detallePuesto.Count > 2)
        //            {
        //                if (detallePuesto.ElementAtOrDefault(2) != null && detallePuesto.ElementAtOrDefault(3) != null)
        //                {
        //                    respuesta.Add(detallePuesto.ElementAtOrDefault(2));
        //                    respuesta.Add(detallePuesto.ElementAtOrDefault(3));
        //                    var direccion = (new CFuncionarioL()).BuscarInformacionBasicaFuncionario(cedula);
        //                    if (direccion[0].GetType() != typeof(CErrorDTO))
        //                    {
        //                        respuesta.Add(direccion.ElementAtOrDefault(1));
        //                    }
        //                    else
        //                    {
        //                        respuesta.Add(new CDireccionDTO
        //                        {
        //                            IdEntidad = 0,
        //                            DirExacta = "",
        //                            Distrito = new CDistritoDTO { IdEntidad = 0}
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicación del Contrato");
        //                }

        //            }
        //            else
        //            {
        //                throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicación del Contrato");
        //            }

        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta.Clear();
        //        respuesta.Add(new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = error.Message
        //        });
        //    }

        //    return respuesta;
        //}

        //no es necesario, se puede usar el mismo buscar para este proposito y resulta casi igual en eficiencia
        public List<List<CBaseDTO>> ListarDesarraigo()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CDesarraigoD intermedio = new CDesarraigoD(contexto);
            List<Desarraigo> datosDesarraigo = new List<Desarraigo>();
            List<string> codigos = new List<string>();

            var datos = intermedio.ListarDesarraigo();

            if (datos.Codigo > 0)
            {
                datosDesarraigo = (List<Desarraigo>)(((List<object>)datos.Contenido)[0]);
                codigos = (List<string>)(((List<object>)datos.Contenido)[1]);

                for (int i = 0; i < datosDesarraigo.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosDesarraigo[i];
                    var desarraigo = ConvertirDesarraigoDatosaDTO(item);
                    desarraigo.CodigoDesarraigo = codigos[i];
                    temp.Add(desarraigo);
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));
                    temp.Add(CPuestoL.ConstruirPuesto(item.Nombramiento.Puesto, new CPuestoDTO()));
                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados." });
                respuesta.Add(temp);
            }
            return respuesta;
        }

        public List<CBaseDTO> ListarEstadosDesarraigo()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CEstadoDesarraigoD intermedio = new CEstadoDesarraigoD(contexto);

            var estadosDesarraigo = intermedio.ListarEstadoDesarraigo();

            if (estadosDesarraigo.Codigo != -1)
            {
                foreach (var item in (List<EstadoDesarraigo>)estadosDesarraigo.Contenido)
                {
                    respuesta.Add(new CEstadoDesarraigoDTO
                    {
                        IdEntidad = item.PK_EstadoDesarraigo,
                        NomEstadoDesarraigo = item.NomEstadoDesarraigo
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)estadosDesarraigo.Contenido);
            }

            return respuesta;
        }

        public CBaseDTO AnularDesarraigo(CDesarraigoDTO desarraigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);
                var desarraigoDB = new Desarraigo
                {
                    PK_Desarraigo = desarraigo.IdEntidad,
                    ObsDesarraigo = desarraigo.ObservacionesEstado
                };
                var datosDesarraigo = intermedio.AnularDesarraigo(desarraigoDB);
                if (datosDesarraigo.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = desarraigo.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosDesarraigo.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        //no se va a usar ya que el editar lo hace
        public CBaseDTO RetomarDesarraigo(CDesarraigoDTO desarraigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);
                var desarraigoDB = new Desarraigo { PK_Desarraigo = desarraigo.IdEntidad };
                var datosDesarraigo = intermedio.RetomarDesarraigo(desarraigoDB);
                if (datosDesarraigo.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = desarraigo.IdEntidad,
                        Mensaje = desarraigo.CodigoDesarraigo
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosDesarraigo.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }


        public CBaseDTO ObtenerSalarioBase(int idCategoriaEscala)
        {
            // El Monto del desarraigo se calcula con la Escala Salarial de Julio 2018
            CBaseDTO respuesta = new CBaseDTO();
            CEscalaSalarialL intermedioEscala = new CEscalaSalarialL();
            try
            {
                var salario = intermedioEscala.BuscarEscalaCategoriaPeriodo(idCategoriaEscala, 1);
                if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    respuesta = ((CEscalaSalarialDTO)salario[0]);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ObtenerMontoRetroactivo(string cedula, List<DateTime> fecha)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);

                if (fecha[0].Day != 1 )
                    fecha[0] = new DateTime(fecha[0].Year, fecha[0].Month, 2); // Segunda Quincena

                if (fecha[1].Day <= 15)
                    fecha[1] = new DateTime(fecha[1].Year, fecha[1].Month, 1); // Primera Quincena
                else
                    fecha[1] = new DateTime(fecha[1].Year, fecha[1].Month, 2); // Segunda Quincena


                var datos = intermedio.ObtenerMontoRetroactivo(cedula, fecha);

                decimal total = 0;
                if (datos.Codigo > 0)
                {
                    total = Convert.ToDecimal((Convert.ToDouble(datos.Contenido)) * 0.4);
                    respuesta = new CBaseDTO
                    {
                        Mensaje = (total.ToString().Replace(',','.'))
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datos.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        internal static CBaseDTO ObtenerFuncionarioID(int idFuncionario)
        {
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(new SIRHEntities());               
                var datos = intermedio.ObtenerFuncionarioPorID(idFuncionario);
                return CFuncionarioL.ConvertirDatosFuncionarioADTO(datos);
            }
            catch (Exception error)
            {
                return new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
        }

        public CBaseDTO ModificarDesarraigo(CDesarraigoDTO desarraigo, List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);
                CEstadoDesarraigoD intermedioEstado = new CEstadoDesarraigoD(contexto);

                var Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(desarraigo.Presupuesto.CodigoPresupuesto).Contenido;

                var desarraigoDB = new Desarraigo
                {
                    PK_Desarraigo = desarraigo.IdEntidad,
                    FK_Presupuesto = Presupuesto.PK_Presupuesto,
                    ImgDocumento = desarraigo.DocAdjunto
                };
                //var estado = intermedioEstado.BuscarEstadoCodigo(desarraigo.EstadoDesarraigo.IdEntidad).Contenido;
                //if (estado.GetType() == typeof(CErrorDTO))
                //    return (CBaseDTO)estado;
                var datosDesarraigo = intermedio.ModificarDesarraigo(desarraigoDB);
                if (datosDesarraigo.Codigo > 0)
                {
                    if(facturas != null)
                        AgregarFacturas(facturas, desarraigoDB, respuesta);
                    if (contratos != null)
                        AgregarContratos(contratos, desarraigoDB, respuesta);
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = desarraigo.IdEntidad,
                        Mensaje = desarraigo.CodigoDesarraigo
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosDesarraigo.Contenido;
                }

            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ModificarEstadoDesarraigo(CDesarraigoDTO desarraigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDesarraigoD intermedio = new CDesarraigoD(contexto);
                CEstadoDesarraigoD intermedioEstado = new CEstadoDesarraigoD(contexto);

                var desarraigoDB = new Desarraigo
                {
                    PK_Desarraigo = desarraigo.IdEntidad,
                    ObsEstado = desarraigo.ObservacionesEstado,
                    IdRegistradoPor = desarraigo.RegistradoPor.IdEntidad,
                    IdAnalizadoPor = desarraigo.AnalizadoPor.IdEntidad,
                    IdRevisadoPor = desarraigo.RevisadoPor.IdEntidad,
                    IdAprobadoPor = desarraigo.AprobadoPor.IdEntidad
                };

                var estado = intermedioEstado.BuscarEstadoCodigo(desarraigo.EstadoDesarraigo.IdEntidad).Contenido;
                if (estado.GetType() == typeof(CErrorDTO))
                    return (CBaseDTO)estado;
                var datosDesarriago = intermedio.ModificarEstadoDesarraigo(desarraigoDB, (EstadoDesarraigo)estado);
                if (datosDesarriago.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = datosDesarriago.IdEntidad,
                        Mensaje = datosDesarriago.Codigo.ToString()
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosDesarriago.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }


        ////  Detalle Eliminación Desarraigo

        public CBaseDTO AgregarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle, CDesarraigoDTO desarraigo)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDesarraigoD intermedioDesarraigo = new CDesarraigoD(contexto);
                CDetalleDesarraigoEliminacionD intermedioDetalle = new CDetalleDesarraigoEliminacionD(contexto);
                CEstadoDesarraigoD intermedioEstado = new CEstadoDesarraigoD(contexto);

                var desarraigoDB = new Desarraigo
                {
                    PK_Desarraigo = desarraigo.IdEntidad
                };
                var detalleDB = new DetalleDesarraigoEliminacion
                {
                    FK_Estado = 4,// Espera
                    FecRegistro = DateTime.Now,
                    FecEliminacion = detalle.FecEliminacion,
                    ObsEliminacion = detalle.ObsEliminacion
                };

                var agregadoDesarraigo = intermedioDetalle.AgregarDetalleEliminacion(detalleDB, desarraigoDB);
                if (agregadoDesarraigo.Codigo > 0)
                {
                    respuesta = agregadoDesarraigo;
                    return respuesta;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoDesarraigo).Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }

        public CBaseDTO ModificarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDesarraigoD intermedioDesarraigo = new CDesarraigoD(contexto);
                CDetalleDesarraigoEliminacionD intermedioDetalle = new CDetalleDesarraigoEliminacionD(contexto);
                CEstadoDesarraigoD intermedioEstado = new CEstadoDesarraigoD(contexto);

                var estado = intermedioEstado.BuscarEstadoCodigo(detalle.EstadoDesarraigo.IdEntidad).Contenido;
                if (estado.GetType() == typeof(CErrorDTO))
                    return (CBaseDTO)estado;

                var estadoDB = (EstadoDesarraigo)estado;
                               
                var detalleDB = new DetalleDesarraigoEliminacion
                {
                    EstadoDesarraigo = estadoDB,
                    FecRegistro = DateTime.Now,
                    FecEliminacion = detalle.FecEliminacion,
                    ObsEliminacion = detalle.ObsEliminacion
                };

                var modificarDetalle = intermedioDetalle.ModificarDetalleEliminacion(detalleDB);
                
                if (modificarDetalle.Codigo > 0)
                {
                    if (estadoDB.PK_EstadoDesarraigo == 1)  //Válido
                    {
                        // Debe generar la acción de personal de Eliminación de Desarraigo
                    }
                    respuesta = modificarDetalle;
                    return respuesta;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)modificarDetalle).Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> CerrarDesarraigosVencidos()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            List<Desarraigo> datosDesarraigo = new List<Desarraigo>();

            CDesarraigoD intermedio = new CDesarraigoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();

            var datos = intermedio.ListarDesarraigo();

            if (datos.Codigo > 0)
            {
                CAccionPersonalDTO accion = new CAccionPersonalDTO();
                CDetalleAccionPersonalDTO detalleAccion = new CDetalleAccionPersonalDTO();
                CFuncionarioDTO funcionario;
                CRegistroIncapacidadDTO incapacidad;

                datosDesarraigo = (List<Desarraigo>)(((List<object>)datos.Contenido)[0]);
                //datosDesarraigo = datosDesarraigo.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario.Contains("0062190360")).ToList();
                foreach (var item in datosDesarraigo.Where(Q => Q.EstadoDesarraigo.PK_EstadoDesarraigo == 4).ToList())
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario);
                    incapacidad = new CRegistroIncapacidadDTO { EstadoIncapacidad = 2 };

                    // Verificar fecha vence
                    if (item.FecFinDesarraigo < DateTime.Today)
                    {
                        item.FK_EstadoDesarraigo = 5; // Vencido

                        var datoPuesto = intermedioPuesto.DetallePuestoPorCodigo(item.Nombramiento.Puesto.CodPuesto);

                        accion = new CAccionPersonalDTO
                        {
                            AnioRige = item.FecInicDesarraigo.Year,
                            FecRige = item.FecFinDesarraigo,
                            FecRigeIntegra = item.FecFinDesarraigo,
                            Nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                            TipoAccion = new CTipoAccionPersonalDTO { IdEntidad = 65 },
                            NumAccion = item.NumAccionAprobado,
                            Estado = new CEstadoBorradorDTO { IdEntidad = 7 }, // Aprobado
                            IndDato = 0,
                            CodigoModulo = 5,
                            CodigoObjetoEntidad = item.PK_Desarraigo,
                            Observaciones = "REAJUSTE DE SOBRESUELDOS PARA ELIMINAR EL DESARRAIGO POR VENCIMIENTO",
                        };

                        var puesto = ((CPuestoDTO)datoPuesto.ElementAt(0));
                        var detallePuesto = ((CDetallePuestoDTO)datoPuesto.ElementAt(1));

                        detalleAccion = new CDetalleAccionPersonalDTO
                        {
                            CodClase = detallePuesto.Clase.IdEntidad,
                            CodDetallePuesto = detallePuesto.IdEntidad,
                            CodNombramiento = item.Nombramiento.PK_Nombramiento,
                            CodEspecialidad = detallePuesto.Especialidad.IdEntidad,
                            CodSubespecialidad = detallePuesto.SubEspecialidad.IdEntidad,
                            DetallePuesto = detallePuesto,
                            PorDesarraigoAnterior = 40,
                            PorDesarraigo = 0,
                            MtoOtros = 0
                        };

                        // Funcionario 00
                        temp.Add(funcionario);

                        // Desarraigo 01
                        temp.Add(ConvertirDesarraigoDatosaDTO(item));

                        // Acción  02
                        temp.Add(accion);

                        // Detalle Acción  03
                        temp.Add(detalleAccion);

                        respuesta.Add(temp);
                    }
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados." });
                respuesta.Add(temp);
            }
            return respuesta;
        }

        public List<List<CBaseDTO>> ListarDesarraigosIncapacidades(DateTime fechaInicio, DateTime fechaFinal)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CDesarraigoD intermedio = new CDesarraigoD(contexto);
            CRegistroIncapacidadD intermedioIncap = new CRegistroIncapacidadD(contexto);
            CBitacoraUsuarioD intermedioBitacora = new CBitacoraUsuarioD(contexto);

            CFuncionarioDTO funcionario;
            CRegistroIncapacidadDTO incapacidad;

            CDesarraigoDTO desarraigo = new CDesarraigoDTO
            {
                EstadoDesarraigo = new CEstadoDesarraigoDTO { IdEntidad = 4 } //Aprobado
            };

            CBitacoraUsuarioDTO bitacora = new CBitacoraUsuarioDTO
            {
                CodigoModulo = 13, // Incapacidades
                CodigoAccion = 2 // Editar - Aprobado
            };
            List<DateTime> fechasBitacora = new List<DateTime>();
            fechasBitacora.Add(fechaInicio);
            fechasBitacora.Add(fechaFinal);

            DateTime fecha;
            List<DateTime> fechasRige = new List<DateTime>();
            List<DateTime> listaDias = new List<DateTime>();

            var datosBitacora = intermedioBitacora.ListarBitacora(bitacora, fechasBitacora);
            if (datosBitacora.Codigo > 0)
            {
                var listaBit = (List<BitacoraUsuario>)datosBitacora.Contenido;
                foreach (var item in listaBit.Where(Q => Q.CodEntidad > 0).ToList())
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    //Buscar la incapacidad
                    var datoIncap = intermedioIncap.ObtenerRegistroIncapacidad(Convert.ToInt32(item.CodObjetoEntidad));
                    if (datoIncap.Codigo > 0)
                    {
                        var incapacidadBD = (RegistroIncapacidad)datoIncap.Contenido;
                        incapacidad = CRegistroIncapacidadL.ConvertirDatosRegistroIncapacidadADto(incapacidadBD);
                        if (incapacidad.EstadoIncapacidad == 2) // Aprobada
                        {
                            //Buscar si tiene desarraigo
                            funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(incapacidadBD.Nombramiento.Funcionario);

                            var datos = BuscarDesarraigo(funcionario, desarraigo, new List<DateTime>(), new CDistritoDTO());
                            if (!datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                            {
                                var tempDesarraigo = (CDesarraigoDTO)datos[0].ElementAt(0);
                                // Buscar en incapacidades si tiene incapacidades de más de 1 mes.
                                fechasRige.Clear();
                                fechasRige.Add(tempDesarraigo.FechaInicio);
                                fechasRige.Add(tempDesarraigo.FechaFin);
                                var incapacidadBuscar = new CRegistroIncapacidadDTO { EstadoIncapacidad = 2 };
                                var registroIncap = intermedioIncap.BuscarRegistroIncapacidad(funcionario, incapacidadBuscar, null, fechasRige, new List<DateTime>());
                                if (registroIncap.Codigo > 0)
                                {
                                    var listaIncap = (List<RegistroIncapacidad>)registroIncap.Contenido;

                                    if (listaIncap.Count > 0)
                                    {
                                        foreach (var registro in listaIncap)
                                        {
                                            incapacidad = CRegistroIncapacidadL.ConvertirDatosRegistroIncapacidadADto(registro);

                                            fecha = incapacidad.FecRige;
                                            listaDias.AddRange(Enumerable.Range(0, (incapacidad.FecVence - incapacidad.FecRige).Days + 1)
                                                                            .Select(d => fecha.AddDays(d))
                                                                            .ToArray()
                                                                );
                                        }

                                        listaDias = listaDias.OrderBy(Q => Q).ToList();
                                        var contador = 0;
                                        var contadorMax = 0;
                                        DateTime diaI = listaDias[0];
                                        foreach (var dia in listaDias)
                                        {
                                            if (dia <= diaI.AddDays(2))
                                                contador++;
                                            else
                                            {
                                                if (contador > contadorMax)
                                                    contadorMax = contador;
                                                contador = 0;
                                            }

                                            diaI = diaI.AddDays(1);
                                        }

                                        if (contador > contadorMax)
                                            contadorMax = contador;

                                        if (contadorMax >= 30)
                                        {
                                            // Funcionario 00
                                            temp.Add(funcionario);

                                            // Desarraigo 01
                                            temp.Add(tempDesarraigo);

                                            // Incapacidad 02
                                            temp.Add(incapacidad);

                                            respuesta.Add(temp);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados." });
                respuesta.Add(temp);
            }
            return respuesta;
        }

        #endregion

    }
}
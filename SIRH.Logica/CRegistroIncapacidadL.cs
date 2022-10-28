using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CRegistroIncapacidadL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CRegistroIncapacidadL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CRegistroIncapacidadDTO ConvertirDatosRegistroIncapacidadADto(RegistroIncapacidad item)
        {
            List<CDetalleIncapacidadDTO> listaDetalle = new List<CDetalleIncapacidadDTO>();

            foreach(var detalle in item.DetalleIncapacidad)
            {
                listaDetalle.Add(new CDetalleIncapacidadDTO
                {
                    IdEntidad = detalle.PK_Detalle,
                    FecRige = detalle.FecDia.ToShortDateString(),
                    PorSubsidio = detalle.PorSubsidio,
                    MtoSubsidio = detalle.MtoSubsidio
                });
            }

            return new CRegistroIncapacidadDTO
            {
                IdEntidad = item.PK_RegistroIncapacidad,
                MtoSalario = Convert.ToDecimal(item.MtoSalario),
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                TipoIncapacidad = new CTipoIncapacidadDTO
                {
                    DescripcionTipoIncapacidad = item.TipoIncapacidad.DesIncapacidad,
                    EntidadMedica = new CEntidadMedicaDTO
                    {
                        DescripcionEntidadMedica = item.TipoIncapacidad.EntidadMedica.DesEntidad
                    },
                },
               
                Nombramiento = new CNombramientoDTO
                {
                    IdEntidad = item.Nombramiento.PK_Nombramiento
                },
                ObsIncapacidad = item.ObsIncapacidad,
                CodIncapacidad = item.CodIncapacidad,
                EstadoIncapacidad = Convert.ToInt32(item.IndEstadoIncapacidad),
                DetalleEstadoIncapacidad = DefinirEstadoIncapacidad(Convert.ToInt32(item.IndEstadoIncapacidad)),
                IndProrroga = item.IndProrroga,
                Detalles = listaDetalle
            };
        }


        internal static string DefinirEstadoIncapacidad(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Activa";
                    break;
                case 2:
                    respuesta = "Aprobada";
                    break;
                case 3:
                    respuesta = "Anulada";
                    break;
                default:
                    respuesta = "Indefinido";
                    break;
            }
            return respuesta;
        }


        //Se registró en ICRegistroIncapacidadService y CRegistroIncapacidadService
        public CBaseDTO AgregarRegistroIncapacidad(CFuncionarioDTO funcionario, CTipoIncapacidadDTO tipo, CRegistroIncapacidadDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                CTipoIncapacidadD intermedioTipo = new CTipoIncapacidadD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);

                RegistroIncapacidad datosRegistro = new RegistroIncapacidad
                {
                    MtoSalario = registro.MtoSalario,
                    FecRige = registro.FecRige,
                    FecVence = registro.FecVence,
                    ObsIncapacidad = registro.ObsIncapacidad,
                    CodIncapacidad = registro.CodIncapacidad,
                    IndEstadoIncapacidad = registro.EstadoIncapacidad,
                    IndProrroga = registro.IndProrroga
                };


                var entidadTipo = intermedioTipo.CargarTipoIncapacidadPorID(tipo.IdEntidad);

                if (entidadTipo.Codigo != -1)
                {
                    datosRegistro.TipoIncapacidad = (TipoIncapacidad)entidadTipo.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadTipo).Contenido;
                    throw new Exception();
                }

                var entidadNombramiento = intermedioNombramiento.CargarNombramientoCedula(funcionario.Cedula);

                if (entidadNombramiento.PK_Nombramiento != -1)
                {
                    datosRegistro.Nombramiento = entidadNombramiento;
                }
                else
                {
                    //respuesta = (CErrorDTO)((CRespuestaDTO)entidadNombramiento).Contenido;
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadNombramiento.PK_Nombramiento) };

                    throw new Exception();
                }
                
                double dias = 0;
                dias = registro.NumDiasOrigen;

                respuesta = intermedio.GuardarRegistroIncapacidad(datosRegistro, dias); 

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }


        public CBaseDTO VerificarFechasIncapacidad(CFuncionarioDTO funcionario, CRegistroIncapacidadDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                RegistroIncapacidad datosRegistro = new RegistroIncapacidad
                {
                    MtoSalario = registro.MtoSalario,
                    FecRige = registro.FecRige,
                    FecVence = registro.FecVence,
                    ObsIncapacidad = registro.ObsIncapacidad,
                    CodIncapacidad = registro.CodIncapacidad,
                    IndEstadoIncapacidad = registro.EstadoIncapacidad,
                    IndProrroga = registro.IndProrroga
                };

                datosRegistro.Nombramiento = intermedioNombramiento.CargarNombramiento(registro.Nombramiento.IdEntidad);
                datosRegistro.Nombramiento.Funcionario = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.Cedula);

                //var entidadNombramiento = intermedioNombramiento.CargarNombramientoCedula(funcionario.Cedula);

                //if (entidadNombramiento.PK_Nombramiento != -1)
                //{
                //    datosRegistro.Nombramiento = entidadNombramiento;
                //}
                //else
                //{
                //    //respuesta = (CErrorDTO)((CRespuestaDTO)entidadNombramiento).Contenido;
                //    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadNombramiento.PK_Nombramiento) };

                //    throw new Exception();
                //}

                respuesta = intermedio.VerificarFechasIncapacidad(datosRegistro);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }


        public CBaseDTO EditarRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CBaseDTO respuesta;

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);

                var registroInc = new RegistroIncapacidad
                {
                    PK_RegistroIncapacidad = registro.IdEntidad,
                    MtoSalario = registro.MtoSalario,
                    FecRige = registro.FecRige,
                    FecVence = registro.FecVence,
                    ObsIncapacidad = registro.ObsIncapacidad,
                    CodIncapacidad = registro.CodIncapacidad,
                    IndEstadoIncapacidad = registro.EstadoIncapacidad, 
                    IndProrroga = registro.IndProrroga
                };

                var dato = intermedio.ActualizarRegistroIncapacidad(registroInc);

                if (dato.Codigo > 0)
                {
                    respuesta = new CRegistroIncapacidadDTO { IdEntidad = Convert.ToInt32(dato.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)dato.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }


        //Se registró en ICRegistroIncapacidadService y CRegistroIncapacidadService
        public List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                CEntidadMedicaD intermedioMedica = new CEntidadMedicaD(contexto);
                CTipoIncapacidadD intermedioTipo = new CTipoIncapacidadD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                decimal monRebaja = 0;
                decimal monTotalSubsidio = 0;
                decimal monTotalRebaja = 0;

                var registro = intermedio.ObtenerRegistroIncapacidad(codigo);

                if (registro.Codigo > 0)
                {
                    var datoRegistro = ConvertirDatosRegistroIncapacidadADto((RegistroIncapacidad)registro.Contenido);
                    var detalles =(List<DetalleIncapacidad>) ((RegistroIncapacidad)registro.Contenido).DetalleIncapacidad.ToList();

                    List<CDetalleIncapacidadDTO> listadoDetalles = new List<CDetalleIncapacidadDTO>();
                    foreach (var item in detalles) {

                        monTotalSubsidio += item.MtoSubsidio;
                        monRebaja = Math.Round(Convert.ToDecimal((datoRegistro.MtoSalario / 30) * ((100 - item.PorSubsidio) / 100)), 2);
                        monTotalRebaja += monRebaja;

                        listadoDetalles.Add(new CDetalleIncapacidadDTO
                        {
                            IdEntidad = item.PK_Detalle,
                            FecRige = item.FecDia.ToShortDateString(),
                            PorSubsidio = item.PorSubsidio,
                            MtoSubsidio = item.MtoSubsidio,
                            PorRebaja = 100 - item.PorSubsidio,
                            MtoRebaja = monRebaja
                        });
                    }

                    datoRegistro.Detalles = listadoDetalles;
                    datoRegistro.MontoTotalSubsidio = monTotalSubsidio;
                    datoRegistro.MontoTotalRebaja = monTotalRebaja;
                    //datoRegistro.DetalleEstadoIncapacidad = DefinirEstadoIncapacidad(datoRegistro.EstadoIncapacidad);

                    // [0] Incapacidad
                    respuesta.Add(datoRegistro);

                    var entidadTipo = intermedioTipo.CargarTipoIncapacidadPorID
                       (((RegistroIncapacidad)registro.Contenido).TipoIncapacidad.PK_TipoIncapacidad);
                   
                    // [1] TipoIncapacidad
                    respuesta.Add(CTipoIncapacidadL.ConvertirTipoIncapacidadADto((TipoIncapacidad)entidadTipo.Contenido));

                    var entidadMedica = intermedioMedica.CargarEntidadMedicaPorID
                        (((RegistroIncapacidad)registro.Contenido).TipoIncapacidad.EntidadMedica.PK_EntidadMedica);

                    // [2] Entidad Médica
                    respuesta.Add(CEntidadMedicaL.ConvertirEntidadMedicaADto((EntidadMedica)entidadMedica.Contenido));

                    // [3] Funcionario
                    var funcionario = ((RegistroIncapacidad)registro.Contenido).Nombramiento.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    // [4] Expediente
                    if(funcionario.ExpedienteFuncionario != null && funcionario.ExpedienteFuncionario.Count > 0)
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente
                        });
                    }
                    else
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            NumeroExpediente = 0
                        });
                    }

                    // [5] Puesto
                    var dtoPuesto = new CPuestoDTO();
                    var datosPuesto = new CPuestoL().DetallePuestoPorCodigo(((RegistroIncapacidad)registro.Contenido).Nombramiento.Puesto.CodPuesto);
                    if (datosPuesto.Count() > 1)
                    {
                        dtoPuesto = (CPuestoDTO)datosPuesto.ElementAt(0);
                        dtoPuesto.DetallePuesto = (CDetallePuestoDTO)datosPuesto.ElementAt(1);
                    }
                    respuesta.Add(dtoPuesto);
                }
                else
                {
                    respuesta.Add((CErrorDTO)registro.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        public List<CBaseDTO> ObtenerRegistroIncapacidadCodigo(string numIncapacidad)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                CEntidadMedicaD intermedioMedica = new CEntidadMedicaD(contexto);
                CTipoIncapacidadD intermedioTipo = new CTipoIncapacidadD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                decimal monRebaja = 0;
                decimal monTotalSubsidio = 0;
                decimal monTotalRebaja = 0;

                var registro = intermedio.ObtenerRegistroIncapacidadCodigo(numIncapacidad);

                if (registro.Codigo > 0)
                {
                    var inc = (List<RegistroIncapacidad>)registro.Contenido;
                    var datoRegistro = ConvertirDatosRegistroIncapacidadADto(inc[0]);
                    List<CDetalleIncapacidadDTO> listadoDetalles = new List<CDetalleIncapacidadDTO>();

                    foreach (var listado in inc)
                    {
                        foreach (var item in listado.DetalleIncapacidad)
                        {
                            monTotalSubsidio += item.MtoSubsidio;
                            monRebaja = Math.Round(Convert.ToDecimal((listado.MtoSalario / 30) * ((100 - item.PorSubsidio) / 100)), 2);
                            monTotalRebaja += monRebaja;

                            listadoDetalles.Add(new CDetalleIncapacidadDTO
                            {
                                IdEntidad = item.PK_Detalle,
                                FecRige = item.FecDia.ToShortDateString(),
                                PorSubsidio = item.PorSubsidio,
                                MtoSubsidio = item.MtoSubsidio,
                                PorRebaja = 100 - item.PorSubsidio,
                                MtoRebaja = monRebaja
                            });
                        }
                    }
                    
                    datoRegistro.Detalles = listadoDetalles;
                    datoRegistro.MontoTotalSubsidio = monTotalSubsidio;
                    datoRegistro.MontoTotalRebaja = monTotalRebaja;
                    //datoRegistro.DetalleEstadoIncapacidad = DefinirEstadoIncapacidad(datoRegistro.EstadoIncapacidad);

                    respuesta.Add(datoRegistro);

                    // TipoIncapacidad
                    var entidadTipo = intermedioTipo.CargarTipoIncapacidadPorID(inc[0].TipoIncapacidad.PK_TipoIncapacidad);
                    respuesta.Add(CTipoIncapacidadL.ConvertirTipoIncapacidadADto((TipoIncapacidad)entidadTipo.Contenido));

                    // Entidad Médica
                    var entidadMedica = intermedioMedica.CargarEntidadMedicaPorID(inc[0].TipoIncapacidad.EntidadMedica.PK_EntidadMedica);
                    respuesta.Add(CEntidadMedicaL.ConvertirEntidadMedicaADto((EntidadMedica)entidadMedica.Contenido));

                    // Funcionario
                    var funcionario = inc[0].Nombramiento.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    // Expediente 04
                    if (funcionario.ExpedienteFuncionario != null && funcionario.ExpedienteFuncionario.Count > 0)
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            NumeroExpediente = funcionario.ExpedienteFuncionario.FirstOrDefault().numExpediente
                        });
                    }
                    else
                    {
                        respuesta.Add(new CExpedienteFuncionarioDTO
                        {
                            NumeroExpediente = 0
                        });
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)registro.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        public List<CBaseDTO> ObtenerRegistroIncapacidadProrroga(string numCedula, int idEntidad, int idTipo, DateTime fecha)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                CEntidadMedicaD intermedioMedica = new CEntidadMedicaD(contexto);
                CTipoIncapacidadD intermedioTipo = new CTipoIncapacidadD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                decimal monRebaja = 0;
                decimal monTotalSubsidio = 0;
                decimal monTotalRebaja = 0;
                List<DateTime> fechasRige =  new List<DateTime>();
                List<DateTime> fechasVence = new List<DateTime>();

                CFuncionarioDTO funcionario = new CFuncionarioDTO { Cedula = numCedula };
                CRegistroIncapacidadDTO incap = new CRegistroIncapacidadDTO
                {
                    TipoIncapacidad = new CTipoIncapacidadDTO { IdEntidad = idTipo }
                };
                CEntidadMedicaDTO entidad = new CEntidadMedicaDTO { IdEntidad = idEntidad };

                fechasVence.Add(fecha.AddDays(-30));
                fechasVence.Add(fecha);

                var registro = intermedio.BuscarRegistroIncapacidad(funcionario, incap, entidad, fechasRige, fechasVence);


                if (registro.Codigo > 0)
                {
                    var inc = (List<RegistroIncapacidad>)registro.Contenido;
                    inc = inc.Where(I => I.IndEstadoIncapacidad != 3).ToList();
                    if (inc.Count > 0)
                    {
                        var datoRegistro = ConvertirDatosRegistroIncapacidadADto(inc[0]);
                        List<CDetalleIncapacidadDTO> listadoDetalles = new List<CDetalleIncapacidadDTO>();

                        foreach (var listado in inc)
                        {
                            foreach (var item in listado.DetalleIncapacidad)
                            {
                                monTotalSubsidio += item.MtoSubsidio;
                                monRebaja = Math.Round(Convert.ToDecimal((listado.MtoSalario / 30) * ((100 - item.PorSubsidio) / 100)), 2);
                                monTotalRebaja += monRebaja;

                                listadoDetalles.Add(new CDetalleIncapacidadDTO
                                {
                                    IdEntidad = item.PK_Detalle,
                                    FecRige = item.FecDia.ToShortDateString(),
                                    PorSubsidio = item.PorSubsidio,
                                    MtoSubsidio = item.MtoSubsidio,
                                    PorRebaja = 100 - item.PorSubsidio,
                                    MtoRebaja = monRebaja
                                });
                            }
                        }

                        datoRegistro.Detalles = listadoDetalles;
                        datoRegistro.MontoTotalSubsidio = monTotalSubsidio;
                        datoRegistro.MontoTotalRebaja = monTotalRebaja;
                        //datoRegistro.DetalleEstadoIncapacidad = DefinirEstadoIncapacidad(datoRegistro.EstadoIncapacidad);

                        respuesta.Add(datoRegistro);

                        // TipoIncapacidad
                        var entidadTipo = intermedioTipo.CargarTipoIncapacidadPorID(inc[0].TipoIncapacidad.PK_TipoIncapacidad);
                        respuesta.Add(CTipoIncapacidadL.ConvertirTipoIncapacidadADto((TipoIncapacidad)entidadTipo.Contenido));

                        // Entidad Médica
                        var entidadMedica = intermedioMedica.CargarEntidadMedicaPorID(inc[0].TipoIncapacidad.EntidadMedica.PK_EntidadMedica);
                        respuesta.Add(CEntidadMedicaL.ConvertirEntidadMedicaADto((EntidadMedica)entidadMedica.Contenido));

                        // Funcionario
                        var datoFuncionario = inc[0].Nombramiento.Funcionario;

                        respuesta.Add(new CFuncionarioDTO
                        {
                            Cedula = datoFuncionario.IdCedulaFuncionario,
                            Nombre = datoFuncionario.NomFuncionario,
                            PrimerApellido = datoFuncionario.NomPrimerApellido,
                            SegundoApellido = datoFuncionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        });

                        // Expediente 04
                        if (datoFuncionario.ExpedienteFuncionario != null && datoFuncionario.ExpedienteFuncionario.Count > 0)
                        {
                            respuesta.Add(new CExpedienteFuncionarioDTO
                            {
                                NumeroExpediente = datoFuncionario.ExpedienteFuncionario.FirstOrDefault().numExpediente
                            });
                        }
                        else
                        {
                            respuesta.Add(new CExpedienteFuncionarioDTO
                            {
                                NumeroExpediente = 0
                            });
                        }
                    }
                    else
                    {
                        respuesta.Add(new CErrorDTO
                        {
                            IdEntidad = -1,
                            Mensaje = "No se encontraron datos de Prórroga",
                            MensajeError = "No se encontraron datos de Prórroga"
                        }
                        );
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)registro.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        //Se registró en ICRegistroIncapacidadService y CRegistroIncapacidadService
        public List<List<CBaseDTO>> BuscarRegistroIncapacidades(CFuncionarioDTO funcionario,
                                                                CRegistroIncapacidadDTO registro,
                                                                CEntidadMedicaDTO entidad,
                                                                CBitacoraUsuarioDTO bitacora,
                                                                List<DateTime> fechasRige,
                                                                List<DateTime> fechasVence,
                                                                List<DateTime> fechasBitacora)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
            CBitacoraUsuarioD intermedioBitacora = new CBitacoraUsuarioD(contexto);

            List<RegistroIncapacidad> datosRegistro = new List<RegistroIncapacidad>();

            if (registro != null)
            {
                var resultado = intermedio.BuscarRegistroIncapacidad(funcionario, registro, entidad, fechasRige, fechasVence);

                if (resultado.Codigo > 0)
                {
                    datosRegistro = (List<RegistroIncapacidad>)resultado.Contenido;

                    if (datosRegistro.Count > 0)
                    {
                        var datosBitacora = intermedioBitacora.ListarBitacora(bitacora, fechasBitacora);
                        if (datosBitacora.Codigo > 0)
                        {
                            var dato = from x in datosRegistro
                                       join b in (List<BitacoraUsuario>)datosBitacora.Contenido
                                       on 1 equals 1
                                       where x.PK_RegistroIncapacidad == b.CodObjetoEntidad
                                       select new { Incapacidad = x, Bitacora = b };

                            foreach (var item in dato.OrderBy(Q => Q.Bitacora.FecEjecucion))
                            {
                                List<CBaseDTO> temp = new List<CBaseDTO>();

                                CRegistroIncapacidadDTO tempRegistroIncapacidad = ConvertirDatosRegistroIncapacidadADto(item.Incapacidad);
                                temp.Add(tempRegistroIncapacidad);

                                CTipoIncapacidadDTO tempTipo = CTipoIncapacidadL.ConvertirTipoIncapacidadADto(item.Incapacidad.TipoIncapacidad);
                                temp.Add(tempTipo);

                                CEntidadMedicaDTO tempEntidad = CEntidadMedicaL.ConvertirEntidadMedicaADto(item.Incapacidad.TipoIncapacidad.EntidadMedica);
                                temp.Add(tempEntidad);

                                CFuncionarioDTO tempFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Incapacidad.Nombramiento.Funcionario);
                                temp.Add(tempFuncionario);

                                CBitacoraUsuarioDTO tempBitacora = CBitacoraUsuarioL.ConvertirDatosBitacoraUsuarioADTO(item.Bitacora);
                                temp.Add(tempBitacora);

                                CPuestoDTO tempPuesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(item.Incapacidad.Nombramiento.Puesto);
                                temp.Add(tempPuesto);

                                respuesta.Add(temp);
                            }
                        }
                        else
                        {
                            List<CBaseDTO> temp = new List<CBaseDTO>();
                            temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                            respuesta.Add(temp);
                        }
                    }
                    else
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                        respuesta.Add(temp);
                    }                    
                }
                else
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                    respuesta.Add(temp);
                }
            }

            return respuesta;
        }


        public CBaseDTO ModificarEstadoIncapacidad(CRegistroIncapacidadDTO registro, int indEstado)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);

                RegistroIncapacidad incapacidadBD = new RegistroIncapacidad
                {
                    PK_RegistroIncapacidad = registro.IdEntidad,
                    ObsIncapacidad = registro.ObsIncapacidad
                };

                var datosIncapacidad = intermedio.ModificarEstadoIncapacidad(incapacidadBD, indEstado);

                if (datosIncapacidad.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = registro.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosIncapacidad.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        public List<List<CBaseDTO>> FuncionariosConIncapacidades()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
                var resultado = intermedio.FuncionariosConIncapacidades();
                if (resultado.Codigo > 0)
                {
                    var lista = (List<Funcionario>)resultado.Contenido;

                    foreach (var item in lista)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item));
                        var nombramiento = ((Funcionario)item).Nombramiento
                                            .Where(N => N.FecVence == null).FirstOrDefault();
                        temp.Add(CPuestoL.ConstruirPuesto(nombramiento.Puesto, new CPuestoDTO()));
                        temp.Add(CDetallePuestoL.ConstruirDetallePuesto(nombramiento.Puesto
                                                            .DetallePuesto.FirstOrDefault()));
                        if (nombramiento.RegistroIncapacidad.Count > 0)
                        {
                            temp.Add(CRegistroIncapacidadL.ConvertirDatosRegistroIncapacidadADto(nombramiento.RegistroIncapacidad.FirstOrDefault()));
                            //((CCaucionDTO)temp.Last()).DetalleEstadoPoliza = CCaucionL.DefinirEstadoPoliza(((CCaucionDTO)temp.Last()).EstadoPoliza);        
                        }
                        else
                        {
                            temp.Add(new CBaseDTO { IdEntidad = -1, Mensaje = "El funcionario no tiene incapacidades registradas" });
                        }
                        respuesta.Add(temp);
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { MensajeError = error.Message });
                respuesta.Add(temp);
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<CBaseDTO> ListarRegistroIncapacidad(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroIncapacidadD intermedioRegistroIncapacidad = new CRegistroIncapacidadD(contexto);

                    var registroIncapacidad = intermedioRegistroIncapacidad.ListarRegistroIncapacidad(cedula);
                    if (registroIncapacidad != null)
                    {
                        List<CBaseDTO> registroIncapacidadData = new List<CBaseDTO>();
                        foreach (var item in registroIncapacidad)
                        {
                        registroIncapacidadData.Add(new CRegistroIncapacidadDTO
                        {
                            IdEntidad = item.PK_RegistroIncapacidad,
                            CodIncapacidad = item.CodIncapacidad,
                            EstadoIncapacidad = Convert.ToInt32(item.IndEstadoIncapacidad),
                            FecRige = Convert.ToDateTime(item.FecRige),
                            FecVence = Convert.ToDateTime(item.FecVence),
                            Nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(item.Nombramiento),
                            ObsIncapacidad = item.ObsIncapacidad,
                            MtoSalario = Convert.ToDecimal(item.MtoSalario),
                            TipoIncapacidad = CTipoIncapacidadL.ConvertirTipoIncapacidadADto(item.TipoIncapacidad)
                        });
                        }
                        respuesta = registroIncapacidadData;
                    }
                    else
                    {
                        respuesta=new List<CBaseDTO> { new CRegistroIncapacidadDTO { Mensaje = "No se encontraron datos de incapacidades para este funcionario" } };
                    }

               
            }
            catch (Exception error)
            {
                respuesta=new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }

            return respuesta;
        }
        
        public CBaseDTO ObtenerSalarioBruto(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
   
                respuesta = intermedio.ObtenerSalarioBruto(cedula);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }
        #endregion
    }
}
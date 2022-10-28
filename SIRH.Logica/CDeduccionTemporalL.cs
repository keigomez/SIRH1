using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDeduccionTemporalL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDeduccionTemporalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CDeduccionTemporalDTO ConvertirDeduccionTemporalADTO(DeduccionTemporal item)
        {
            return new CDeduccionTemporalDTO
            {
                FechaActualizacion = Convert.ToDateTime(item.FecActualizacion),
                Dias = Convert.ToDecimal(item.IndDias),
                Estado = Convert.ToInt32(item.IndEstado),
                Explicacion = item.IndExplicacion,
                FechaRige = Convert.ToDateTime(item.FecRige),
                FechaVence = Convert.ToDateTime(item.FecVence),
                Horas = Convert.ToDecimal(item.IndHoras),
                IdEntidad = item.PK_DeduccionTemporal,
                MesProceso = Convert.ToInt32(item.IndMesProceso),
                MontoDeduccion = Convert.ToDecimal(item.MtoDeduccion),
                NumeroDocumento = item.NumDocumento,
                Periodo = item.IndPeriodo,
                DatoTipoDeduccionTemporal = ConvertirTipoDeduccionTemporalADTO(item.TipoDeduccionTemporal)
            };
        }

        internal static CTipoDeduccionTemporalDTO ConvertirTipoDeduccionTemporalADTO(TipoDeduccionTemporal item)
        {
            return new CTipoDeduccionTemporalDTO
            {
                IdEntidad = item.PK_TipoDeduccionTemporal,
                DetalleTipoDeduccionTemporal = item.DesTipoDeduccionTemporal,
                IndEstado = item.IndEstado,
                IndConSalario = item.IndConSalario
            };
        }

        public CRespuestaDTO ObtenerTipoDeduccion(int codigo)
        {
            try
            {
                CRespuestaDTO respuesta = new CRespuestaDTO();
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                var resultado = intermedio.ObtenerTipoDeduccion(codigo);
                if (resultado.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ConvertirTipoDeduccionTemporalADTO((TipoDeduccionTemporal)resultado.Contenido)
                    };   
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = error.Message
                };
            }
        }
        public List<CBaseDTO> ListarTiposDeduccion()
        {
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                var resultado = intermedio.ListarTiposDeduccion();
                if (resultado.Codigo > 0)
                {
                    var lista = ((List<TipoDeduccionTemporal>)resultado.Contenido);
                    foreach (var item in lista)
                    {
                        respuesta.Add(ConvertirTipoDeduccionTemporalADTO(item));
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
                return new List<CBaseDTO>
                {
                    new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public CBaseDTO AgregarDeduccionTemporal(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion)
        {
            try
            {
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                var nombramiento = contexto.Nombramiento.Where(N => (N.FecVence >= deduccion.FechaRige // DateTime.Now
                                                                    || N.FecVence == null)
                                                                    && N.Funcionario.IdCedulaFuncionario == funcionario.Cedula)
                                                                    .OrderByDescending(Q => Q.FecRige)
                                                                    .FirstOrDefault();
                if(nombramiento != null)
                {
                    DeduccionTemporal item = new DeduccionTemporal
                    {
                        FecActualizacion = DateTime.Now,
                        FecRige = deduccion.FechaRige,
                        FecVence = deduccion.FechaVence,
                        FK_Nombramiento = nombramiento.PK_Nombramiento,
                        FK_TipoDeduccionTemporal = deduccion.DatoTipoDeduccionTemporal.IdEntidad,
                        IndDias = deduccion.Dias,
                        IndEstado = 0, // Registrada.,
                        IndExplicacion = deduccion.Explicacion,
                        IndHoras = deduccion.Horas,
                        IndMesProceso = deduccion.MesProceso,
                        IndPeriodo = deduccion.Periodo,
                        MtoDeduccion = deduccion.MontoDeduccion,
                        NumDocumento = deduccion.NumeroDocumento
                    };

                    var resultado = intermedio.AgregarDeduccionTemporal(item);

                    if (resultado.Codigo > 0)
                    {
                        return new CBaseDTO
                        {
                            IdEntidad = Convert.ToInt32(resultado.Contenido)
                        };
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("No se puede registrar la Deducción porque el funcionario no tiene un nombramiento válido en la Fecha Rige");
                }
                
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO AnularDeduccionTemporal(CDeduccionTemporalDTO deduccion)
        {
            try
            {
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                var resultado = intermedio.AnularDeduccionTemporal(deduccion.IdEntidad);

                if (resultado.Codigo > 0)
                {
                    return ConvertirDeduccionTemporalADTO(((DeduccionTemporal)resultado.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> DescargarDetalleDeduccion(CDeduccionTemporalDTO deduccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                var resultado = intermedio.DescargarDetalleDeduccion(deduccion.IdEntidad);

                if (resultado.Codigo > 0)
                {
                    var dato = (DeduccionTemporal)resultado.Contenido;
                    respuesta.Add(ConvertirDeduccionTemporalADTO(dato));
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(dato.Nombramiento.Funcionario));
                    respuesta.Add(CPuestoL.ConstruirPuesto(dato.Nombramiento.Puesto, new CPuestoDTO()));
                    if (dato.Nombramiento.Funcionario.ExpedienteFuncionario.Count > 0)
                        respuesta.Add(CExpedienteL.ConvertirExpedienteADTO(((DeduccionTemporal)resultado.Contenido).Nombramiento.Funcionario.ExpedienteFuncionario.FirstOrDefault()));
                    else
                        respuesta.Add(new CExpedienteFuncionarioDTO { IdEntidad = 0, NumeroExpediente = 0 });

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }

        public List<List<CBaseDTO>> BuscarDeducciones(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion,
                                                CBitacoraUsuarioDTO bitacora,
                                                List<DateTime> fechas,
                                                List<DateTime> fechasBitacora)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                bool buscar = true;
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);
                CBitacoraUsuarioD intermedioBitacora = new CBitacoraUsuarioD(contexto);

                List<DeduccionTemporal> datosDeduccion = new List<DeduccionTemporal>();
                CRespuestaDTO resultado = new CRespuestaDTO { Codigo = -1 }; ;
                if (funcionario.Cedula != null)
                {
                    resultado = ((CRespuestaDTO)intermedio.BuscarDeducciones(datosDeduccion, funcionario.Cedula, "Cedula"));
                    if (resultado.Codigo > 0)
                        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                    else
                        buscar = false;
                }

                if (fechas.Count > 0 && buscar)
                {
                    resultado = ((CRespuestaDTO)intermedio.BuscarDeducciones(datosDeduccion, fechas, "Fechas"));
                    if (resultado.Codigo > 0)
                        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                    else
                        buscar = false;
                }


                if (buscar) // Buscar por estado, Si deduccion.Estado == -1, no mostrar las Anuladas (idEstado = 2)
                {
                    resultado = ((CRespuestaDTO)intermedio.BuscarDeducciones(datosDeduccion, deduccion.Estado, "Estado"));
                    if (resultado.Codigo > 0)
                        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                    else
                        buscar = false;
                }

                //if (deduccion.Estado == 1 && buscar) // Activas
                //{
                //    resultado = ((CRespuestaDTO)intermedio.BuscarDeducciones(datosDeduccion, 1, "Estado"));
                //    if (resultado.Codigo > 0)
                //        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                //    else
                //        buscar = false;
                //}
                //else if(deduccion.Estado == 2 && buscar) // Anuladas
                //{
                //    resultado = ((CRespuestaDTO)intermedio.BuscarDeducciones(datosDeduccion, 2, "Estado"));
                //    if (resultado.Codigo > 0)
                //        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                //    else
                //        buscar = false;
                //}

                if (deduccion.DatoTipoDeduccionTemporal != null && buscar)
                {
                    resultado = ((CRespuestaDTO)intermedio.
                        BuscarDeducciones(datosDeduccion, deduccion.DatoTipoDeduccionTemporal.IdEntidad, "Tipo"));
                    if (resultado.Codigo > 0)
                        datosDeduccion = (List<DeduccionTemporal>)resultado.Contenido;
                    else
                        buscar = false;
                }

                if (datosDeduccion.Count > 0 && buscar)
                {
                    var datosBitacora = intermedioBitacora.ListarBitacora(bitacora, fechasBitacora);
                    if (datosBitacora.Codigo > 0)
                    {
                        var dato = from x in datosDeduccion
                                   join b in (List<BitacoraUsuario>)datosBitacora.Contenido
                                   on 1 equals 1
                                   where x.PK_DeduccionTemporal == b.CodObjetoEntidad
                                   select new { Deduccion = x, Bitacora = b };

                        List<CBaseDTO> temp;
                        CBitacoraUsuarioDTO datoBitacora;

                        foreach (var item in dato.OrderBy(Q => Q.Deduccion.FecRige)) //Q.Bitacora.FecEjecucion
                        {
                            var datosFuncionario = ((DeduccionTemporal)item.Deduccion).Nombramiento.Funcionario;
                            temp = new List<CBaseDTO>();
                            //[0]
                            temp.Add(ConvertirDeduccionTemporalADTO(((DeduccionTemporal)item.Deduccion)));
                            //[1]
                            temp.Add(CFuncionarioL.FuncionarioGeneral(datosFuncionario));
                            //[2]
                            temp.Add(CPuestoL.ConstruirPuesto(((DeduccionTemporal)item.Deduccion).Nombramiento.Puesto, new CPuestoDTO()));

                            datoBitacora = new CBitacoraUsuarioDTO();
                            datoBitacora = CBitacoraUsuarioL.ConvertirDatosBitacoraUsuarioADTO(item.Bitacora);
                            var usuario = item.Bitacora.Usuario.DetalleAcceso.FirstOrDefault().Funcionario;
                            datoBitacora.Usuario.NombreUsuario = datoBitacora.Usuario.NombreUsuario.Replace("MOPT", "").Replace("//", "").Replace("\\", ""); //usuario.NomFuncionario.TrimEnd() + " " + usuario.NomPrimerApellido.TrimEnd() + " " + usuario.NomSegundoApellido.TrimEnd();
                            //[3]
                            temp.Add(datoBitacora);

                            //[4] Expediente
                            if(datosFuncionario.ExpedienteFuncionario.Count > 0)
                                temp.Add(CExpedienteL.ConvertirExpedienteADTO(datosFuncionario.ExpedienteFuncionario.FirstOrDefault()));
                            else
                            {
                                temp.Add(new CExpedienteFuncionarioDTO
                                {
                                    NumeroExpediente = 0
                                });
                            }

                            respuesta.Add(temp);
                        }

                        return respuesta;
                    }
                    else
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                        respuesta.Add(temp);

                        return respuesta;
                    }
                }
                else
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                    respuesta.Add(temp);

                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "Error: " + ex.Message + " " + ex.InnerException != null ? ex.InnerException .Message: "" });
                respuesta.Clear();
                respuesta.Add(temp);

                return respuesta;
            }
        }

        public CBaseDTO ModificarEstadoDeduccion(CDeduccionTemporalDTO registro, int indEstado)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);

                DeduccionTemporal deduccionDB = new DeduccionTemporal
                {
                    PK_DeduccionTemporal = registro.IdEntidad
                };

                var datosDeduccion = intermedio.ModificarEstadoDeduccion(deduccionDB, indEstado);

                if (datosDeduccion.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = registro.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosDeduccion.Contenido);
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

        public CBaseDTO ModificarExplicacion(CDeduccionTemporalDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDeduccionTemporalD intermedio = new CDeduccionTemporalD(contexto);

                DeduccionTemporal deduccionBD = new DeduccionTemporal
                {
                    PK_DeduccionTemporal = registro.IdEntidad,
                    IndExplicacion = registro.Explicacion
                };

                respuesta = intermedio.ModificarExplicacion(deduccionBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta = new CBaseDTO { Mensaje = (((CRespuestaDTO)respuesta).Contenido).ToString() }; ;
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

        #endregion
    }
}

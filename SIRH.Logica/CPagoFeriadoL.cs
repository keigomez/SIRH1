using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPagoFeriadoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPagoFeriadoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CPagoFeriadoDTO ConvertirDatosTramiteADto(PagoFeriado item)
        {

            return new CPagoFeriadoDTO
            {
                IdEntidad = item.PK_PagoFeriado,
                MontoSalaroBruto = item.MtoSalarioBruto,
                MontoSubtotalDia = item.MtoSubtotalDia,
                MontoSalarioEscolar = item.MtoSalarioEscolar,
                MontoDiferenciaLiquida = item.MtoDiferenciaLiquida,
                MontoAguinaldoProporcional = item.MtoAguinaldoProporcional,
                MontoDeduccionPatronal = item.MtoDeduccionPatronal,
                MontoDeduccionObrero = item.MtoDeduccionObrero,
                MontoDeTotal = item.MtoTotal,
                ObsevacionTramite = item.ObsTramite
            };
        }

        /// <summary>
        /// Agrega a la BD un nuevo registro de trámite de feriado
        /// </summary>
        /// <returns>Retorna el nuevo trámite</returns>
        public CBaseDTO AgregarPagoFeriado(CPagoExtraordinarioDTO pagoExtraordinario, CPagoFeriadoDTO pagoFeriado,
                                       CEstadoTramiteDTO estadoTramite, CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPagoFeriadoD intermedio = new CPagoFeriadoD(contexto);

                CPagoExtraordinarioD intermedioPagoExtraordinario = new CPagoExtraordinarioD(contexto);

                CEstadoTramiteD intermedioEstado = new CEstadoTramiteD(contexto);

                Funcionario datosFuncionario = new Funcionario
               {
                   IdCedulaFuncionario = funcionario.Cedula
               };

                PagoFeriado datosTramite = new PagoFeriado
                {
                    MtoSalarioBruto = pagoFeriado.MontoSalaroBruto,
                    MtoSubtotalDia = pagoFeriado.MontoSubtotalDia,
                    MtoDiferenciaLiquida = pagoFeriado.MontoDiferenciaLiquida,
                    MtoAguinaldoProporcional = pagoFeriado.MontoAguinaldoProporcional,
                    MtoDeduccionPatronal = pagoFeriado.MontoDeduccionPatronal,
                    MtoDeduccionObrero = pagoFeriado.MontoDeduccionObrero,
                    MtoSalarioEscolar = pagoFeriado.MontoSalarioEscolar,
                    MtoTotal = pagoFeriado.MontoDeTotal,
                    ObsTramite = pagoFeriado.ObsevacionTramite
                };

                var pagoExtraordinarioT = intermedioPagoExtraordinario.BuscarPagoExtraordinario(pagoExtraordinario.IdEntidad);

                if (pagoExtraordinarioT.Codigo != -1)
                {
                    datosTramite.PagoExtraordinario = (PagoExtraordinario)pagoExtraordinarioT.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoExtraordinarioT).Contenido;
                    throw new Exception();
                }

                PagoExtraordinario datosPagoExtra = new PagoExtraordinario
                {
                    PK_PagoExtraordinario = pagoExtraordinario.IdEntidad
                };

                var estadoTramiteT = intermedioEstado.BuscarEstadoTramite(estadoTramite.IdEntidad);

                if (estadoTramiteT.Codigo != -1)
                {

                    datosTramite.EstadoTramite = (EstadoTramite)estadoTramiteT.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)estadoTramiteT).Contenido;
                    throw new Exception();
                }

                EstadoTramite datosEstadoTramite = new EstadoTramite
                {
                    PK_EstadoTramite = estadoTramite.IdEntidad
                };


                //Guardado
                respuesta = intermedio.AgregarPagoFeriado(datosTramite, datosPagoExtra, datosFuncionario, datosEstadoTramite);

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

        /// <summary>
        /// Busca un trámite de pago
        /// </summary>
        /// <returns>Retorna el trámite de pago</returns>
        public List<CBaseDTO> ObtenerPagoFeriado(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CPagoFeriadoD intermedio = new CPagoFeriadoD(contexto);

                CPagoExtraordinarioD intermedioPagoExtraordinario = new CPagoExtraordinarioD(contexto);

                CEstadoTramiteD intermedioEstado = new CEstadoTramiteD(contexto);

                var tramite = intermedio.BuscarPagoFeriadoCompleto(codigo);

                if (tramite.Codigo > 0)
                {
                    var datoPagoFeriado = ConvertirDatosTramiteADto((PagoFeriado)tramite.Contenido);
                    respuesta.Add(datoPagoFeriado);

                    var pagoExtraordinario = intermedioPagoExtraordinario.BuscarPagoExtraordinario
                       (((PagoFeriado)tramite.Contenido).PagoExtraordinario.PK_PagoExtraordinario);

                    respuesta.Add(CPagoExtraordinarioL.ConvertirDatosPagoExtraordinarioADto((PagoExtraordinario)pagoExtraordinario.Contenido));

                    var funcionario = ((PagoFeriado)tramite.Contenido).PagoExtraordinario.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    var estadoTramite = intermedioEstado.BuscarEstadoTramite
                        (((PagoFeriado)tramite.Contenido).EstadoTramite.PK_EstadoTramite);

                    respuesta.Add(CEstadoTramiteL.ConvertirEstadoTramiteADto((EstadoTramite)estadoTramite.Contenido));
                }
                else
                {
                    respuesta.Add((CErrorDTO)tramite.Contenido);
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

        /// <summary>
        /// Obtiene los registros de pagos de feriado que corresponden con los parámetros de búsqueda
        /// </summary>
        /// <returns>Retorna una lista de registros</returns>
        public List<List<CBaseDTO>> BuscarPagosFeriado(CFuncionarioDTO funcionario, CPagoFeriadoDTO tramite,
                                                       List<DateTime> fechasTramite,CEstadoTramiteDTO estadoTramite, 
                                                       List<string> diasFeriados)
        {
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

                CPagoFeriadoD intermedio = new CPagoFeriadoD(contexto);

                List<PagoFeriado> datosTramites = new List<PagoFeriado>();

                if (funcionario.Cedula != null && !funcionario.Cedula.Equals(""))
                {
                    var resultado = ((CRespuestaDTO)intermedio.ListarPagoFeriado(datosTramites, funcionario.Cedula, "Cedula"));

                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<PagoFeriado>)resultado.Contenido;
                        if (datosTramites.Count < 1)
                        {
                            datosTramites = new List<PagoFeriado>();
                            throw new Exception("No se encontraron resultados para los parámetros especificados.");
                        }
                    }
                    else
                    {
                        datosTramites = new List<PagoFeriado>();
                    }
                }

                if (tramite.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        ListarPagoFeriado(datosTramites,  Convert.ToString(tramite.IdEntidad), "Consecutivo"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<PagoFeriado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<PagoFeriado>();
                    }
                }

                if (estadoTramite.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        ListarPagoFeriado(datosTramites, Convert.ToString(estadoTramite.IdEntidad), "Estado"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<PagoFeriado>)resultado.Contenido;
                    }
                    else {
                        datosTramites = new List<PagoFeriado>();
                    }
                }

                if (diasFeriados.Count()>0)
                {
                    foreach (String id in diasFeriados)
                    {
                        var resultado = ((CRespuestaDTO)intermedio.
                            ListarPagoFeriado(datosTramites, id, "DiaFeriado"));
                        if (resultado.Codigo > 0)
                        {
                            datosTramites = (List<PagoFeriado>)resultado.Contenido;
                        }
                        else
                        {
                            datosTramites = new List<PagoFeriado>();
                        }
                    }
                }
                if (fechasTramite.Count > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        ListarPagoFeriado(datosTramites, fechasTramite, "FechaTramite"));
                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<PagoFeriado>)resultado.Contenido;
                    }
                    else
                    {
                        datosTramites = new List<PagoFeriado>();
                    }
                }

                if (datosTramites.Count > 0)
                {
                    foreach (var item in datosTramites)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();

                        var datoPago = ConvertirDatosTramiteADto(item);

                        CPagoFeriadoDTO tempPago = datoPago;

                        temp.Add(tempPago);

                        CPagoExtraordinarioDTO tempPagoExtraordinario = CPagoExtraordinarioL.ConvertirDatosPagoExtraordinarioADto(item.PagoExtraordinario);

                        temp.Add(tempPagoExtraordinario);

                        CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                        {
                            Cedula = item.PagoExtraordinario.Funcionario.IdCedulaFuncionario,
                            Nombre = item.PagoExtraordinario.Funcionario.NomFuncionario,
                            PrimerApellido = item.PagoExtraordinario.Funcionario.NomPrimerApellido,
                            SegundoApellido = item.PagoExtraordinario.Funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        };

                        temp.Add(tempFuncionario);

                        CEstadoTramiteDTO tempEstadoTramite = CEstadoTramiteL.ConvertirEstadoTramiteADto(item.EstadoTramite);

                        temp.Add(tempEstadoTramite);

                        respuesta.Add(temp);
                    }
                }
                else
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                    respuesta.Add(temp);
                }

                return respuesta;
            }
            catch (Exception e) {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
                return respuesta ;
            }
        }

        /// <summary>
        /// Anula un trámite de pago
        /// </summary>
        /// <returns>Retorna el trámite anulado</returns>
        public CBaseDTO AnularPagoFeriado(CPagoFeriadoDTO tramite)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPagoFeriadoD intermedio = new CPagoFeriadoD(contexto);

                PagoFeriado tramiteBD = new PagoFeriado
                {
                    PK_PagoFeriado = tramite.IdEntidad,
                    ObsTramite = tramite.ObsevacionTramite
                };

                var datosTramite = intermedio.AnularPagoFeriado(tramiteBD);

                if (datosTramite.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = tramite.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosTramite.Contenido);
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

        /// <summary>
        /// Elimina un trámite especifico
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO EliminarPagoFeriado(CPagoFeriadoDTO pagoFeriado)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPagoFeriadoD intermedio = new CPagoFeriadoD(contexto);



                PagoFeriado datosTramite = new PagoFeriado
                {
                    PK_PagoFeriado = pagoFeriado.IdEntidad
                };



                //Guardado
                respuesta = intermedio.EliminarPagoFeriado(datosTramite);

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

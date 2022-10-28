using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDiaPagadoL
    {

        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDiaPagadoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CDiaPagadoDTO ConvertirDiaPagadoADto(DiaPagado item)
        {
            return new CDiaPagadoDTO
            {
                IdEntidad = item.PK_DiaPagado,
                CantidadHoras = item.CntHora,
                MontoSalarioHora = item.MtoSalarioHora,
                MontoTotal = item.MtoTotal,
                Anio = item.AnioPago

            };
        }

        /// <summary>
        /// Agrega un registro de día pagado a la BD
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO AgregarDiaPagado(CFuncionarioDTO funcionario, List<CDiaPagadoDTO> dias, List<CCatalogoDiaDTO> catalogodias,
                                          CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDiaPagadoD intermedio = new CDiaPagadoD(contexto);

                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);

                CCatalogoDiaD intermedioCatalogoDia = new CCatalogoDiaD(contexto);

                CPagoExtraordinarioD intermedioPagoExtraordinario = new CPagoExtraordinarioD(contexto);

                //Funcionario
                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                //PagoFeriado
                var pagoFeriadoBase = intermedioPagoFeriado.BuscarPagoFeriado(pagoFeriado.IdEntidad);

                if (pagoFeriadoBase.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoFeriadoBase).Contenido;
                    throw new Exception();
                }

                PagoFeriado datosFeriado = new PagoFeriado
                {
                    PK_PagoFeriado = pagoFeriado.IdEntidad
                };

                //Pago Extraordinario
                var pagoExtraordinarioBase = intermedioPagoExtraordinario.BuscarPagoExtraordinario(pagoExtraordinario.IdEntidad);

                if (pagoExtraordinarioBase.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoExtraordinarioBase).Contenido;
                    throw new Exception();
                }

                PagoExtraordinario datosExtraordinario = new PagoExtraordinario
                {
                    PK_PagoExtraordinario = pagoExtraordinario.IdEntidad
                };
                int i = 0;
                foreach (var dia in dias) //Todos los días feriados o de asueto a pagar
                {
                    var diaAux = (CatalogoDia)intermedioCatalogoDia.BuscarCatalogoDia(catalogodias[i].IdEntidad).Contenido;
                    DiaPagado datosDia = new DiaPagado
                    {
                        CntHora = dia.CantidadHoras,
                        MtoSalarioHora = dia.MontoSalarioHora,
                        MtoTotal = dia.MontoTotal,
                        AnioPago = dia.Anio
                    };

                    datosDia.PagoFeriado = (PagoFeriado)pagoFeriadoBase.Contenido;

                    datosDia.PagoFeriado.PagoExtraordinario = (PagoExtraordinario)pagoExtraordinarioBase.Contenido;

                    var catalogoDiaBase = intermedioCatalogoDia.BuscarCatalogoDia(diaAux.PK_CatalogoDia); //Catalogo de días

                    if (catalogoDiaBase.Codigo != -1)
                    {
                        datosDia.CatalogoDia = diaAux;
                    }
                    else
                    {
                        respuesta = (CErrorDTO)((CRespuestaDTO)catalogoDiaBase).Contenido;
                        throw new Exception();
                    }

                    //Guardar el día pagado
                    respuesta = intermedio.AgregarDiaPagado(datosDia, datosFuncionario, datosExtraordinario, datosFeriado);

                    if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                    {
                        respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                        throw new Exception();
                    }
                    i++;

                }
                return respuesta;
            }
            catch
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Obtiene un catalogo de día específico
        /// </summary>
        /// <returns>Retorna el catálogo de día</returns>
        public List<CBaseDTO> ObtenerDiaPagado(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDiaPagadoD intermedio = new CDiaPagadoD(contexto);
                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);
                CCatalogoDiaD intermedioDias = new CCatalogoDiaD(contexto);
                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);

                var dia = intermedio.BuscarDiaPagado(codigo);

                if (dia.Codigo > 0)
                {
                    var datoDia = ConvertirDiaPagadoADto((DiaPagado)dia.Contenido);
                    respuesta.Add(datoDia);

                    var funcionario = ((DiaPagado)dia.Contenido).PagoFeriado.PagoExtraordinario.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    var pagoFeriado = intermedioPagoFeriado.BuscarPagoFeriado
                        (((DiaPagado)dia.Contenido).PagoFeriado.PK_PagoFeriado);

                    respuesta.Add(CPagoFeriadoL.ConvertirDatosTramiteADto((PagoFeriado)pagoFeriado.Contenido));

                    var catalogoDia = intermedioDias.BuscarCatalogoDia
                         (((DiaPagado)dia.Contenido).CatalogoDia.PK_CatalogoDia);

                    respuesta.Add(CCatalogoDiaL.ConvertirDatosCatalogoDADto((CatalogoDia)catalogoDia.Contenido));

                    var tipoDia = intermedioTipoDia.BuscarTipoDia
                             (((DiaPagado)dia.Contenido).CatalogoDia.PK_CatalogoDia);

                    respuesta.Add(CTipoDiaL.ConvertirTipoDiaADto((TipoDia)tipoDia.Contenido));
                    
                }
                else
                {
                    respuesta.Add((CErrorDTO)dia.Contenido);
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
        /// Obtiene los días pagados de un trámite en específico
        /// </summary>
        /// <returns>Retorna los días pagados</returns>
        public List<List<CBaseDTO>> RetornarDiasPorTramitePagado(int codigo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CDiaPagadoD intermedio = new CDiaPagadoD(contexto);
                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);
                CCatalogoDiaD intermedioDias = new CCatalogoDiaD(contexto);
                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);

                var diaPagado = intermedio.ListarDiasPagadosPorPago(codigo);

                if (diaPagado.Codigo > 0)
                {
                    foreach (var pago in (List<DiaPagado>)diaPagado.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoPago = ConvertirDiaPagadoADto(pago);
                        temporal.Add(datoPago);

                        var dia = intermedio.BuscarDiaPagado(pago.PK_DiaPagado);

                        var funcionario = ((DiaPagado)dia.Contenido).PagoFeriado.PagoExtraordinario.Funcionario;

                        temporal.Add(new CFuncionarioDTO
                        {
                            Cedula = funcionario.IdCedulaFuncionario,
                            Nombre = funcionario.NomFuncionario,
                            PrimerApellido = funcionario.NomPrimerApellido,
                            SegundoApellido = funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        });

                        var pagoFeriado = intermedioPagoFeriado.BuscarPagoFeriado
                            (((DiaPagado)dia.Contenido).PagoFeriado.PK_PagoFeriado);

                        temporal.Add(CPagoFeriadoL.ConvertirDatosTramiteADto((PagoFeriado)pagoFeriado.Contenido));

                        var catalogoDia = intermedioDias.BuscarCatalogoDia
                             (((DiaPagado)dia.Contenido).CatalogoDia.PK_CatalogoDia);

                        temporal.Add(CCatalogoDiaL.ConvertirDatosCatalogoDADto((CatalogoDia)catalogoDia.Contenido));

                        var tipoDia = intermedioTipoDia.BuscarTipoDia
                             (((DiaPagado)dia.Contenido).CatalogoDia.TipoDia.PK_TipoDia);

                        temporal.Add(CTipoDiaL.ConvertirTipoDiaADto((TipoDia)tipoDia.Contenido));

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)diaPagado.Contenido);
                    resultado.Add(temporal);
                }

            }
            catch (Exception error)
            {
                temporal.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                resultado.Add(temporal);
            }
            return resultado;
        }

        /// <summary>
        /// Obtiene un catalogo de día específico
        /// </summary>
        /// <returns>Retorna el catálogo de día</returns>
        public List<CBaseDTO> RetornarDiaPorTramitePagado(int codigo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CDiaPagadoD intermedio = new CDiaPagadoD(contexto);
                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);
                var diaPagado = intermedio.ListarDiasPagadosPorPago(codigo);

                if (diaPagado.Codigo > 0)
                {
                    foreach (var pago in (List<DiaPagado>)diaPagado.Contenido)
                    {
                       

                        var datoPago = ConvertirDiaPagadoADto(pago);
                        resultado.Add(datoPago);

                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)diaPagado.Contenido);
                   
                }

            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }

        /// <summary>
        /// Elimina los días pagados a un trámite específico
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO EliminarDiaPagado(CPagoFeriadoDTO pagoFeriado)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDiaPagadoD intermedio = new CDiaPagadoD(contexto);


                List<List<CBaseDTO>> diaPagado = this.RetornarDiasPorTramitePagado(pagoFeriado.IdEntidad);



                foreach (var dia in diaPagado)
                {

                    CDiaPagadoDTO diaAux = ((CDiaPagadoDTO)dia.ElementAt(0));
                    DiaPagado diaConvertido = new DiaPagado
                    {
                        PK_DiaPagado = diaAux.IdEntidad
                    };

                    respuesta = intermedio.EliminarDiaPagado(diaConvertido);

                    if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                    {
                        respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                        throw new Exception();
                    }
                }
                return respuesta;
            }
            catch
            {
                return respuesta;
            }
        }

        #endregion
    }
}

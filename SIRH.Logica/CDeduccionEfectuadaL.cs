using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDeduccionEfectuadaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDeduccionEfectuadaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CDeduccionEfectuadaDTO ConvertirDeduccionEfectuadaADto(DeduccionEfectuada item)
        {
            return new CDeduccionEfectuadaDTO
            {
                IdEntidad = item.PK_DeduccionEfectuada,
                PorcentajeEfectuado = item.PorEfectuado,
                MontoDeduccion = item.MtoDeduccion
            };
        }

        /// <summary>
        /// Almacena una deduccion efectuada en la BD
        /// </summary>
        /// <returns>Retorna la deducción almacenada</returns>
        public CBaseDTO AgregarDeduccion(CFuncionarioDTO funcionario, List<CDeduccionEfectuadaDTO> deducciones,List<CCatalogoDeduccionDTO> catalogoDeduccion,
                                           CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDeduccionEfectuadaD intermedio = new CDeduccionEfectuadaD(contexto);

                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);

                CCatalogoDeduccionD intermedioDeducciones = new CCatalogoDeduccionD(contexto);

                CPagoExtraordinarioD intermedioPagoExtraordinario = new CPagoExtraordinarioD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                var pagoFeriadoT = intermedioPagoFeriado.BuscarPagoFeriado(pagoFeriado.IdEntidad);

                if (pagoFeriadoT.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoFeriadoT).Contenido;
                    throw new Exception();
                }

                PagoFeriado datosFeriado = new PagoFeriado
                {
                    PK_PagoFeriado = pagoFeriado.IdEntidad
                };

                var pagoExtraordinarioT = intermedioPagoExtraordinario.BuscarPagoExtraordinario(pagoExtraordinario.IdEntidad);

                if (pagoExtraordinarioT.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoExtraordinarioT).Contenido;
                    throw new Exception();
                }

                PagoExtraordinario datosExtraordinario = new PagoExtraordinario
                {
                    PK_PagoExtraordinario = pagoExtraordinario.IdEntidad
                };

                int i = 0;
                foreach (var deduccion in deducciones)
                {
                   
                    var deduccionAux = (CatalogoDeduccion)intermedioDeducciones.BuscarCatalogoDeduccion(catalogoDeduccion[i].IdEntidad).Contenido;
                    DeduccionEfectuada datosDeduccion = new DeduccionEfectuada
                {
                    PorEfectuado = deduccion.PorcentajeEfectuado,
                    MtoDeduccion = deduccion.MontoDeduccion
                };

                    datosDeduccion.PagoFeriado = (PagoFeriado)pagoFeriadoT.Contenido;

                    datosDeduccion.PagoFeriado.PagoExtraordinario = (PagoExtraordinario)pagoExtraordinarioT.Contenido;

                   // var catDed = CCatalogoDeduccionL.ConvertirDatosCatalogoDeduccionesADto(deduccionAux.CatalogoDeduccion);
              var catalogoDeduccionT = intermedioDeducciones.BuscarCatalogoDeduccion(deduccionAux.PK_CatalogoDeduccion);

                    if (catalogoDeduccionT.Codigo != -1)
                    {
                        datosDeduccion.CatalogoDeduccion = (CatalogoDeduccion)deduccionAux;
                    }
                    else
                    {
                        respuesta = (CErrorDTO)((CRespuestaDTO)catalogoDeduccionT).Contenido;
                        throw new Exception();
                    }

                    respuesta = intermedio.AgregarDeduccionEfectuada(datosDeduccion, datosFuncionario, datosExtraordinario, datosFeriado);

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
        /// Obtiene una deducción efectuada específica
        /// </summary>
        /// <returns>Retorna las deducciones efectuadas</returns>
        public List<CBaseDTO> ObtenerDeduccionEfectuada(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDeduccionEfectuadaD intermedio = new CDeduccionEfectuadaD(contexto);
                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);
                CCatalogoDeduccionD intermedioDeducciones = new CCatalogoDeduccionD(contexto);
                CTipoDeduccionD intermedioTipoDeduccion = new CTipoDeduccionD(contexto);

                var deduccion = intermedio.BuscarDeduccionEfectuada(codigo);

                if (deduccion.Codigo > 0)
                {
                    var datoDeduccion = ConvertirDeduccionEfectuadaADto((DeduccionEfectuada)deduccion.Contenido);
                    respuesta.Add(datoDeduccion);

                    var funcionario = ((DeduccionEfectuada)deduccion.Contenido).PagoFeriado.PagoExtraordinario.Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });

                    var pagoFeriado = intermedioPagoFeriado.BuscarPagoFeriado
                        (((DeduccionEfectuada)deduccion.Contenido).PagoFeriado.PK_PagoFeriado);

                    respuesta.Add(CPagoFeriadoL.ConvertirDatosTramiteADto((PagoFeriado)pagoFeriado.Contenido));

                    var catalogoDeduccion = intermedioDeducciones.BuscarCatalogoDeduccion
                        (((DeduccionEfectuada)deduccion.Contenido).CatalogoDeduccion.PK_CatalogoDeduccion);

                    respuesta.Add(CCatalogoDeduccionL.ConvertirDatosCatalogoDeduccionesADto((CatalogoDeduccion)catalogoDeduccion.Contenido));

                    var tipoDeduccion = intermedioTipoDeduccion.BuscarTipoDeduccion
                            (((DeduccionEfectuada)deduccion.Contenido).CatalogoDeduccion.TipoDeduccion.PK_TipoDeduccion);

                    respuesta.Add(CTipoDeduccionL.ConvertirTipoDiaADto((TipoDeduccion)tipoDeduccion.Contenido));
                }
                else
                {
                    respuesta.Add((CErrorDTO)deduccion.Contenido);
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
        /// Obtiene las deducciones efectuadas aun trámite de pago
        /// </summary>
        /// <returns>Retorna las deucciones efectuadas</returns>
        public List<List<CBaseDTO>> RetornarDeduccionesPorPagoFeriado(int codigo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CDeduccionEfectuadaD intermedio = new CDeduccionEfectuadaD(contexto);
                CPagoFeriadoD intermedioFeriado = new CPagoFeriadoD(contexto);
                CCatalogoDeduccionD intermedioDeducciones = new CCatalogoDeduccionD(contexto);
                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);
                CTipoDeduccionD intermedioTipoDeduccion = new CTipoDeduccionD(contexto);

                var deducciones = intermedio.ListarDeduccionPorPago(codigo);
                if (deducciones.Codigo > 0)
                {
                    foreach (var pago in (List<DeduccionEfectuada>)deducciones.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoDeduccion = ConvertirDeduccionEfectuadaADto(pago);
                        temporal.Add(datoDeduccion);

                        var deduccion = intermedio.BuscarDeduccionEfectuada(pago.PK_DeduccionEfectuada);

                        var funcionario = pago.PagoFeriado.PagoExtraordinario.Funcionario;

                        temporal.Add(new CFuncionarioDTO
                        {
                            Cedula = funcionario.IdCedulaFuncionario,
                            Nombre = funcionario.NomFuncionario,
                            PrimerApellido = funcionario.NomPrimerApellido,
                            SegundoApellido = funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        });

                        var pagoFeriado = intermedioPagoFeriado.BuscarPagoFeriado
                            (pago.PagoFeriado.PK_PagoFeriado);

                        temporal.Add(CPagoFeriadoL.ConvertirDatosTramiteADto((PagoFeriado)pagoFeriado.Contenido));

                        var catalogoDeduccion = intermedioDeducciones.BuscarCatalogoDeduccion
                             (pago.CatalogoDeduccion.PK_CatalogoDeduccion);

                        temporal.Add(CCatalogoDeduccionL.ConvertirDatosCatalogoDeduccionesADto((CatalogoDeduccion)catalogoDeduccion.Contenido));

                        var tipoDeduccion = intermedioTipoDeduccion.BuscarTipoDeduccion
                             (pago.CatalogoDeduccion.TipoDeduccion.PK_TipoDeduccion);

                        temporal.Add(CTipoDeduccionL.ConvertirTipoDiaADto((TipoDeduccion)tipoDeduccion.Contenido));

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)deducciones.Contenido);
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
        /// Elimina las deducciones efectuadas a un trámite de pago
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO EliminarDeduccionEfectuada(CPagoFeriadoDTO pagoFeriado)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDeduccionEfectuadaD intermedio = new CDeduccionEfectuadaD(contexto);


                List<List<CBaseDTO>> deduccionEfectuada = this.RetornarDeduccionesPorPagoFeriado(pagoFeriado.IdEntidad);



                List<CDeduccionEfectuadaDTO> deducciones = new List<CDeduccionEfectuadaDTO>();

                foreach (var deduccion in deduccionEfectuada)
                {

                    CDeduccionEfectuadaDTO deduccionAux = ((CDeduccionEfectuadaDTO)deduccion.ElementAt(0));
                    DeduccionEfectuada deduccionConvertida = new DeduccionEfectuada
                    {
                        PK_DeduccionEfectuada = deduccionAux.IdEntidad
                    };
                    respuesta = intermedio.EliminarDeduccionEfectuada(deduccionConvertida);

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

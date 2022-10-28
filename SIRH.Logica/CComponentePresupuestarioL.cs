using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CComponentePresupuestarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CComponentePresupuestarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        //__________________________________________DECRETOS________________________________________________________________________
        public CBaseDTO AgregarDecretoComponentePresupuestario(CProgramaDTO programa, CObjetoGastoDTO objetoGasto,
                                                    CCatMovimientoPresupuestoDTO tipo, CComponentePresupuestarioDTO componente)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CComponentePresupuestarioD intermedio = new CComponentePresupuestarioD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CCatMovimientoPresupuestoD intermedioCatMovimiento = new CCatMovimientoPresupuestoD(contexto);
                CObjetoGastoD intermedioObjetoGasto = new CObjetoGastoD(contexto);



                ComponentePresupuestario datosComponente = ConvertirDTOComponentePresupuestarioADatos(componente);


                var entidadPrograma = intermedioPrograma.CargarProgramaPorID(programa.IdEntidad);

                if (entidadPrograma.PK_Programa != -1)
                {
                    datosComponente.Programa = entidadPrograma;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadPrograma.PK_Programa) };
                    throw new Exception();
                }


                var entidadObjetoGasto = intermedioObjetoGasto.CargarObjetoGastoId(objetoGasto.IdEntidad);

                if (entidadObjetoGasto.PK_ObjetoGasto != -1)
                {
                    datosComponente.ObjetoGasto = entidadObjetoGasto;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadObjetoGasto.PK_ObjetoGasto) };
                    throw new Exception();
                }


                var entidadCatMovimientoPresupuesto = intermedioCatMovimiento.CargarMovimientoPresupuestoId(tipo.IdEntidad);

                if (entidadCatMovimientoPresupuesto.PK_CatMovimientoPresupuesto != 0)
                {
                    datosComponente.CatMovimientoPresupuesto = entidadCatMovimientoPresupuesto;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadCatMovimientoPresupuesto.PK_CatMovimientoPresupuesto) };
                    throw new Exception();
                }

                //if (entidadCatMovimientoPresupuesto.PK_CatMovimientoPresupuesto == 3)
                //{

                //    datosComponente.MtoComponentePresupuestario = componente.MontoComponente + objetoGasto.Valor;


                //}

                
                respuesta = intermedio.AgregarDecretoComponentePresupuestario(datosComponente);

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

        //__________________________________________DECRETOS________________________________________________________________________


        public CBaseDTO GuardarComponentePresupuestario(CProgramaDTO programa, CObjetoGastoDTO objetoGasto,
                                                   CCatMovimientoPresupuestoDTO tipo, CComponentePresupuestarioDTO componente)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CComponentePresupuestarioD intermedio = new CComponentePresupuestarioD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CCatMovimientoPresupuestoD intermedioCatMovimiento = new CCatMovimientoPresupuestoD(contexto);
                CObjetoGastoD intermedioObjetoGasto = new CObjetoGastoD(contexto);

                ComponentePresupuestario datosComponente = new ComponentePresupuestario
                {
                    AnioPresupuesto = componente.AnioPresupuesto,
                    MtoComponentePresupuestario = componente.MontoComponente,
                    ObsComponentePresupuestario = componente.Detalle
                };


                var entidadPrograma = intermedioPrograma.CargarProgramaPorID(programa.IdEntidad);

                if (entidadPrograma.PK_Programa != -1)
                {
                    datosComponente.Programa = entidadPrograma;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadPrograma.PK_Programa) };
                    throw new Exception();
                }


                var entidadObjetoGasto = intermedioObjetoGasto.CargarObjetoGastoId(objetoGasto.IdEntidad);

                if (entidadObjetoGasto.PK_ObjetoGasto != -1)
                {
                    datosComponente.ObjetoGasto = entidadObjetoGasto;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadObjetoGasto.PK_ObjetoGasto) };
                    throw new Exception();
                }


                var entidadCatMovimientoPresupuesto = intermedioCatMovimiento.CargarMovimientoPresupuestoId(tipo.IdEntidad);

                if (entidadCatMovimientoPresupuesto.PK_CatMovimientoPresupuesto != 0)
                {
                    datosComponente.CatMovimientoPresupuesto = entidadCatMovimientoPresupuesto;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadCatMovimientoPresupuesto.PK_CatMovimientoPresupuesto) };
                    throw new Exception();
                }

                respuesta = intermedio.GuardarComponentePresupuestario(datosComponente);

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


        public CBaseDTO EditarComponentePresupuestario(CComponentePresupuestarioDTO componente)
        {
            CBaseDTO respuesta;

            try
            {
                CComponentePresupuestarioD intermedio = new CComponentePresupuestarioD(contexto);
                var comPresupuestario = new ComponentePresupuestario
                {
                    PK_ComponentePresupuestario = componente.IdEntidad,
                    AnioPresupuesto = componente.AnioPresupuesto,
                    MtoComponentePresupuestario = componente.MontoComponente,
                    FK_ObjetoGasto = componente.ObjetoGasto.IdEntidad,
                    FK_Programa = componente.Programa.IdEntidad,
                    FK_CatMovimientoPresupuesto = componente.TipoMovimiento.IdEntidad,
                    ObsComponentePresupuestario = componente.Detalle
                };
                var dato = intermedio.ActualizarComponentePresupuestario(comPresupuestario);

                if (dato.Codigo > 0)
                {
                    respuesta = new CComponentePresupuestarioDTO { IdEntidad = Convert.ToInt32(dato.Contenido) };
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


        public List<CBaseDTO> DescargarProgramas()
        {
            List<CBaseDTO> resultados = new List<CBaseDTO>();
            try
            {
                CComponentePresupuestarioD intermedioComponentePresupuestario = new CComponentePresupuestarioD(contexto);

                var programas = intermedioComponentePresupuestario.DescargarProgramas();
                if (programas != null)
                {
                    List<CBaseDTO> programasData = new List<CBaseDTO>();
                    foreach (var item in programas)
                    {
                        programasData.Add(new CProgramaDTO
                        {
                            IdEntidad = item.PK_Programa,
                            DesPrograma = item.DesPrograma,
                            IndEstPrograma = Convert.ToInt32(item.IndEstadoPrograma)
                        });
                    }
                    resultados = programasData;
                }
                else
                {
                    resultados = new List<CBaseDTO> { new CComponentePresupuestarioDTO { Mensaje = "No se encontraron datos de programas" } };
                }

            }
            catch (Exception error)
            {
                resultados = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return resultados;
            }

            return resultados;
        }

        public List<CBaseDTO> DescargarObjetosGasto()
        {
            List<CBaseDTO> resultados = new List<CBaseDTO>();
            try
            {
                CComponentePresupuestarioD intermedioComponentePresupuestario = new CComponentePresupuestarioD(contexto);

                var objetosGasto = intermedioComponentePresupuestario.DescargarObjetosGasto();
                if (objetosGasto != null)
                {
                    List<CBaseDTO> objetosGastoData = new List<CBaseDTO>();
                    foreach (var item in objetosGasto)
                    {
                        objetosGastoData.Add(new CObjetoGastoDTO
                        {
                            IdEntidad = item.PK_ObjetoGasto,
                            CodObjGasto = item.CodObjetoGasto,
                            DesObjGasto = item.DesObjetoGasto
                        });
                    }
                    resultados = objetosGastoData;
                }
                else
                {
                    resultados = new List<CBaseDTO> { new CComponentePresupuestarioDTO { Mensaje = "No se encontraron datos de Objetos de Gasto" } };
                }

            }
            catch (Exception error)
            {
                resultados = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return resultados;
            }

            return resultados;
        }

        public List<CBaseDTO> DescargarCatMovimientoPresupuesto()
        {
            List<CBaseDTO> resultados = new List<CBaseDTO>();
            try
            {
                CComponentePresupuestarioD intermedioComponentePresupuestario = new CComponentePresupuestarioD(contexto);

                var tipoMovimientoPresupuesto = intermedioComponentePresupuestario.DescargarCatMovimientoPresupuesto();
                if (tipoMovimientoPresupuesto != null)
                {
                    List<CBaseDTO> tipoMovimientoPresupuestoData = new List<CBaseDTO>();
                    foreach (var item in tipoMovimientoPresupuesto)
                    {
                        tipoMovimientoPresupuestoData.Add(new CCatMovimientoPresupuestoDTO
                        {
                            IdEntidad = item.PK_CatMovimientoPresupuesto,
                            DesMovimientoPresupuesto = item.DesMovimientoPresupuesto
                        });
                    }
                    resultados = tipoMovimientoPresupuestoData;
                }
                else
                {
                    resultados = new List<CBaseDTO> { new CComponentePresupuestarioDTO { Mensaje = "No se encontraron datos de tipos de movimientos" } };
                }

            }
            catch (Exception error)
            {
                resultados = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return resultados;
            }

            return resultados;
        }

        public List<CBaseDTO> ListarMovimientosPresupuesto(string anno)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CComponentePresupuestarioD intermedioComponentePresupuestario = new CComponentePresupuestarioD(contexto);

                var componentePresupuestario = intermedioComponentePresupuestario.ListarComponentePresupuestario(anno);
                if (componentePresupuestario != null)
                {
                    List<CBaseDTO> componentePresupuestarioData = new List<CBaseDTO>();
                    foreach (var item in componentePresupuestario)
                    {
                        componentePresupuestarioData.Add(new CComponentePresupuestarioDTO
                        {
                            IdEntidad = item.PK_ComponentePresupuestario,
                            ObjetoGasto = new CObjetoGastoDTO { IdEntidad = Convert.ToInt32(item.ObjetoGasto.CodObjetoGasto), DesObjGasto = item.ObjetoGasto.DesObjetoGasto },
                            Programa = new CProgramaDTO { IdEntidad = Convert.ToInt32(item.Programa.PK_Programa), DesPrograma = item.Programa.DesPrograma },
                            TipoMovimiento = new CCatMovimientoPresupuestoDTO { IdEntidad = Convert.ToInt32(item.CatMovimientoPresupuesto.PK_CatMovimientoPresupuesto), DesMovimientoPresupuesto = item.CatMovimientoPresupuesto.DesMovimientoPresupuesto },
                            AnioPresupuesto = item.AnioPresupuesto,
                            MontoComponente = Convert.ToDecimal(item.MtoComponentePresupuestario),
                            Detalle = item.ObsComponentePresupuestario
                        });
                    }
                    respuesta = componentePresupuestarioData;
                }
                else
                {
                    respuesta = new List<CBaseDTO> { new CComponentePresupuestarioDTO { Mensaje = "No se encontraron datos de movimientos de presupuesto para el año especificado" } };
                }


            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }

            return respuesta;
        }


        public List<CBaseDTO> ObtenerMovimientoPresupuesto(int idMovimiento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CComponentePresupuestarioD intermedioComponentePresupuestario = new CComponentePresupuestarioD(contexto);

                var componentePresupuestario = intermedioComponentePresupuestario.ObtenerMovimientoPresupuesto(idMovimiento);

                if (componentePresupuestario != null)
                {
                    var datoMovPresupuesto = ConvertirDatosCompPresupuestarioADto((ComponentePresupuestario)componentePresupuestario.Contenido);
                    respuesta.Add(datoMovPresupuesto);
                }
                else
                {
                    respuesta.Add((CErrorDTO)componentePresupuestario.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }

            return respuesta;
        }




        internal static CComponentePresupuestarioDTO ConvertirDatosCompPresupuestarioADto(ComponentePresupuestario item)
        {
            return new CComponentePresupuestarioDTO
            {
                IdEntidad = item.PK_ComponentePresupuestario,

                ObjetoGasto = new CObjetoGastoDTO
                {
                    CodObjGasto = item.ObjetoGasto.CodObjetoGasto,
                    DesObjGasto = item.ObjetoGasto.DesObjetoGasto,
                    SubPartida = new CSubPartidaDTO
                    {
                        CodSubPartida = item.ObjetoGasto.SubPartida.CodSubPartida,
                    }
                },

                Programa = new CProgramaDTO
                {
                    IdEntidad = item.Programa.PK_Programa,
                    DesPrograma = item.Programa.DesPrograma
                },

                TipoMovimiento = new CCatMovimientoPresupuestoDTO
                {
                    DesMovimientoPresupuesto = item.CatMovimientoPresupuesto.DesMovimientoPresupuesto
                },


                AnioPresupuesto = item.AnioPresupuesto,
                MontoComponente = Convert.ToDecimal(item.MtoComponentePresupuestario),
                Detalle = item.ObsComponentePresupuestario,
                TituloComponente = item.TituloComponentePresupuestario,
                NumeroComponentePresupuestario = item.NumComponentePresupuestario,
                FechaDecreto = Convert.ToDateTime(item.FecComponentePresupuestario)
            };
        }


        internal static ComponentePresupuestario ConvertirDTOComponentePresupuestarioADatos(CComponentePresupuestarioDTO item)
        {
            return new ComponentePresupuestario
            {
                PK_ComponentePresupuestario = item.IdEntidad,
                FK_ObjetoGasto = item.ObjetoGasto.IdEntidad,
                FK_Programa = item.Programa.IdEntidad,
                AnioPresupuesto = item.AnioPresupuesto,
                MtoComponentePresupuestario = item.MontoComponente,
                FK_CatMovimientoPresupuesto = item.TipoMovimiento.IdEntidad,
                ObsComponentePresupuestario = item.Detalle,
                TituloComponentePresupuestario = item.TituloComponente,
                NumComponentePresupuestario = item.NumeroComponentePresupuestario,
                FecComponentePresupuestario = item.FechaDecreto

            };
        }

        #endregion
    }
}
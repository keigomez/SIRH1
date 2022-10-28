using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Datos
{
    public class CGastoTrasporteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CGastoTrasporteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos

        //-------------------------------------- CONSTRUIR CÓDIGO ----------------------------------
        /// <summary>
        /// Metodo encargado de generar un codigo de informe para los gastos de transporte
        /// </summary>
        /// <param name="gastoTransporte"></param>
        /// <returns></returns>
        private string GenerarCodigoInforme(GastoTransporte gastoTransporte)
        {
            var annio = gastoTransporte.FecInicGastosTransporte.Value.Year;
            var datos = entidadBase.GastoTransporte.Where(D => D.FecFinGastosTransporte.Value.Year == annio).ToList();
            var secuencia = datos.IndexOf(gastoTransporte) + 1;
            return annio + "-" + secuencia;
        }
        
        /// <summary>
        /// metodo encargado de listar un Gasto Trasnporte
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<object> GastoTransporteCodigo(List<GastoTransporte> datos)
        {
            List<object> respuestaDatos = new List<object>();
            List<string> codigos = new List<string>();
            respuestaDatos.Add(datos);
            foreach (var d in datos)
            {
                codigos.Add(GenerarCodigoInforme(d));
            }
            respuestaDatos.Add(codigos);
            return respuestaDatos;
        }


        //------------------------------------ AGREGAR/MODIFICAR/ANULAR ------------------------------------
        /// <summary>
        /// Método encargado de guardar en base de datos un gasto de transporte.
        /// </summary>
        /// <param name="carta">Objeto 'CartaPresentación' para usar el número de carta.(En realidad no se utiliza dentro del método)</param>
        /// <param name="funcionario">Objeto 'Funcionario' para obtener el nombramiento del funcionario.</param>
        /// <param name="gastoTransporte">Objeto 'GastoTransporte' que se agregará en la BD para el funcionario indicado. </param>
        /// <returns>Un objeto de tipo 'GastoTransporte' como respuesta DTO.</returns>
        public CRespuestaDTO AgregarGastoTransporte(CartasPresentacion carta, Funcionario funcionario, GastoTransporte gastoTransporte)
        {
            CRespuestaDTO respuesta;
            try
            {
                //CNombramientoD intermedioNombramiento = new CNombramientoD(entidadBase);
                //var nombramiento = intermedioNombramiento.CargarNombramientoActualCedula(funcionario.IdCedulaFuncionario);

                var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                var nombramiento = entidadBase.Nombramiento
                                .Where(N => N.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && listaEstados.Contains(N.FK_EstadoNombramiento))
                                .Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now)
                                .OrderBy(N => N.FecRige)
                                .ToList()
                                .LastOrDefault();

                if (nombramiento != null)
                {
                    //var GastoTransporteActivos = nombramiento.GastoTransporte.Count(D => D.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3);

                    bool fechaValida = true;
                    DateTime fecI = Convert.ToDateTime(gastoTransporte.FecInicGastosTransporte);
                    DateTime fecV = Convert.ToDateTime(gastoTransporte.FecFinGastosTransporte);

                    var listaDias = Enumerable.Range(0, (fecV - fecI).Days + 1)
                                              .Select(d => fecI.AddDays(d))
                                              .ToArray();

                    var GastoTransporteActivos = nombramiento.GastoTransporte.Where(D => D.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3). ToList();
                    foreach (var item in GastoTransporteActivos)
                    {
                        listaDias = Enumerable.Range(0, (Convert.ToDateTime(item.FecFinGastosTransporte) - Convert.ToDateTime(item.FecInicGastosTransporte)).Days + 1)
                                              .Select(d => Convert.ToDateTime(item.FecInicGastosTransporte).AddDays(d))
                                              .ToArray();
                        if (listaDias.Contains(fecI) || listaDias.Contains(fecV))
                            fechaValida = false;
                    }

                    //Verificar que no existan ya gastos transporte activos(1:validos) o en espera(3) para poder insertar uno nuevo
                    // if (GastoTransporteActivos == 0)
                    if (fechaValida)
                    {
                        //Verifica que el funcionario tenga estado 1:Servidor Activo 
                        if (nombramiento.Funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1)
                        {
                            //var listaEstados = new List<int> { 1, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                            //listaEstados.Contains(nombramiento.EstadoNombramiento.PK_EstadoNombramiento);
                            //if (listaEstados.Contains(nombramiento.EstadoNombramiento.PK_EstadoNombramiento))
                            //{
                            gastoTransporte.FecRegistro = DateTime.Today;
                            gastoTransporte.FecContrato = DateTime.Today;
                            gastoTransporte.Nombramiento = nombramiento;
                            //nombramiento.GastoTransporte.Add(gastoTransporte);
                            entidadBase.GastoTransporte.Add(gastoTransporte);
                            entidadBase.SaveChanges();
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = GenerarCodigoInforme(gastoTransporte),
                                IdEntidad = gastoTransporte.PK_GastosTransporte
                            };
                            return respuesta;
                            //}
                            //else
                            //{
                            //    throw new Exception("El nombramiento del funcionario no es válido.");
                            //}
                        }
                        else
                        {
                            throw new Exception("El estado del funcionario no es válido. Estado: "+ nombramiento.Funcionario.EstadoFuncionario.DesEstadoFuncionario);
                        }
                    }
                    else
                    {
                        throw new Exception("Ya existe un gasto de transporte registrado");
                    }
                }
                else
                {
                    //NombramientoNull
                    throw new Exception("No se encuentra un nombramiento para el funcionario indicado.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    IdEntidad = -1,
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO AgregarGastoTransporteRuta(GastoTransporteRutas ruta)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.GastoTransporteRutas.Add(ruta);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    IdEntidad = ruta.PK_Ruta
                };
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    IdEntidad = -1,
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ActualizarRutaTarifa(GastoTransporteRutas ruta)
        {
            CRespuestaDTO respuesta;
            try
            {
                var rutaOld = entidadBase.GastoTransporteRutas.Where(Q => Q.PK_Ruta == ruta.PK_Ruta).FirstOrDefault();
                var fechaVence = rutaOld.FecVence;
                rutaOld.FK_Estado = 4; // Vencido
                rutaOld.FecVence = ruta.FecRige.AddDays(-1);
                entidadBase.SaveChanges();

                ruta.GastoTransporte = rutaOld.GastoTransporte;
                ruta.FecVence = fechaVence;
                entidadBase.GastoTransporteRutas.Add(ruta);
                entidadBase.SaveChanges();

                var dato = ModificarMontoGastoTrans(ruta.GastoTransporte.PK_GastosTransporte, ruta.MonGasto * 22);
                if (dato.GetType() == typeof(CErrorDTO))
                {
                    respuesta = dato;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        IdEntidad = ruta.PK_Ruta
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    IdEntidad = -1,
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ModificarEstadoGastoTransporte(GastoTransporte gastoT, EstadoGastoTransporte estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteOld = entidadBase.GastoTransporte.Include("EstadoGastoTransporte").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_GastosTransporte == gastoT.PK_GastosTransporte);
                if (GastoTransporteOld != null)
                {
                    GastoTransporteOld.EstadoGastoTransporte = estado;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(GastoTransporteOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el GastoTransporte requerido");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// metodo encargado de editar un estado de un gasto Transporte
        /// </summary>
        /// <param name="gastoT"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public CRespuestaDTO ModificarGastoTransporte(GastoTransporte gastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteOld = entidadBase.GastoTransporte.Include("EstadoGastoTransporte").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_GastosTransporte == gastoT.PK_GastosTransporte);
                if (GastoTransporteOld != null)
                {
                    if (GastoTransporteOld.EstadoGastoTransporte.PK_EstadoGastosTransporte == 2) // "Espera"
                    {
                        GastoTransporteOld.FK_Presupuesto = gastoT.FK_Presupuesto;
                        if (gastoT.ImgDocumento != null)
                            GastoTransporteOld.ImgDocumento = gastoT.ImgDocumento;

                        entidadBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = GenerarCodigoInforme(GastoTransporteOld)
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("El Gasto Transporte no es modificable");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el Gasto Transporte requerido");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        /// <summary>
        /// Modificar en la bd el monto total de un gasto de transporte cuyas rutas han sido modificadas
        /// </summary>
        /// <param name="idgasto">PK del gasto de transporte</param>
        /// <param name="newMonto">El nuevo valor total del gasto</param>
        /// <returns></returns>
        public CRespuestaDTO ModificarMontoGastoTrans(int idgasto, decimal newMonto)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteOld = entidadBase.GastoTransporte.Include("DetalleAsignacionGastoTransporteModificada")
                                                                    .FirstOrDefault(D => D.PK_GastosTransporte == idgasto);

                //var gastoEnModificadas = entidadBase.DetalleAsignacionGastoTransporteModificada.Where(d => d.FK_GastoTransporte == idgasto);

                if (GastoTransporteOld != null)
                {
                    //if (gastoEnModificadas.Count() >= 1)
                    //{
                    GastoTransporteOld.MonActual = newMonto;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(GastoTransporteOld)
                    };
                    return respuesta;
                    //}
                    //else
                    //{
                    //    throw new Exception("El monto del Gasto de Transporte no es modificable porque sus rutas no han sido cambiadas.");
                    //}
                }
                else
                {
                    throw new Exception("No se encontró el Gasto de Transporte requerido");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        ///<summary>
        /// Método encargado de poner en estado vencido los GT que corresponda
        /// </summary>
        /// <param name="fechaVencimiento"></param>
        /// <returns></returns>
        public CRespuestaDTO ActualizarVencimientoGastoTransporte(DateTime fechaVencimiento)
        {
            CRespuestaDTO respuesta;
            List<GastoTransporte> GastoTransporteVencido = new List<GastoTransporte>();
            try
            {
                var datosGastoTransporte = entidadBase.GastoTransporte.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoGastoTransporte")
                                        .Where(D => D.FecFinGastosTransporte <= fechaVencimiento &&
                                                     D.EstadoGastoTransporte.PK_EstadoGastosTransporte != 4 &&
                                                     D.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3).ToList();
                if (datosGastoTransporte.Count > 0)
                {
                    var estado = entidadBase.EstadoGastoTransporte.FirstOrDefault(E => E.PK_EstadoGastosTransporte == 4);
                    foreach (var item in datosGastoTransporte)
                    {
                        item.EstadoGastoTransporte = estado;
                        entidadBase.SaveChanges();
                        GastoTransporteVencido.Add(item);
                    }
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GastoTransporteCodigo(GastoTransporteVencido)
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay Gasto Transporte cuyo vencimiento se alcance el día de hoy" }
                    };

                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };

                return respuesta;
            }
        }

        /// <summary>
        /// Metodo encargado de anular un gasto de transporte
        /// </summary>
        /// <param name="gastoT">El gasto transporte a anular</param>
        /// <returns></returns>
        public CRespuestaDTO AnularGastoTransporte(GastoTransporte gastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var gastoTransporteOld = entidadBase.GastoTransporte
                                                    .Include("EstadoGastoTransporte")
                                                    .FirstOrDefault(D => D.PK_GastosTransporte == gastoT.PK_GastosTransporte);
                if (gastoTransporteOld != null)
                {
                    gastoTransporteOld.EstadoGastoTransporte.PK_EstadoGastosTransporte = 3;
                    gastoTransporteOld.ObsGastosTransporte = gastoT.ObsGastosTransporte;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = gastoT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el gasto de transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO FinalizarGastoTransporte(GastoTransporte gastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var gastoTransporteOld = entidadBase.GastoTransporte
                                                    .Include("EstadoGastoTransporte")
                                                    .FirstOrDefault(D => D.PK_GastosTransporte == gastoT.PK_GastosTransporte);
                if (gastoTransporteOld != null)
                {
                    gastoTransporteOld.FecFinGastosTransporte = gastoT.FecFinGastosTransporte;
                    gastoTransporteOld.ObsGastosTransporte = gastoT.ObsGastosTransporte;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = gastoT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el gasto de transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Almacena en la base de datos el pago del gasto de transporte que recibe por parámetros
        /// y sus detalles
        /// </summary>
        /// <param name="pago">Objeto pago con todos los atributos de éste</param>
        /// <param name="detalles">Lista de detalles del pago</param>
        /// <param name="funcionario">El funcionario a quien se le hará el pago</param>
        /// <returns></returns>
        public CRespuestaDTO AgregarPagoGastoTransporte(PagoGastoTransporte pago, List<DetallePagoGastoTransporte> detalles, Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {
                funcionario = entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null) //si existe el funcionario
                {
                    var gasto = entidadBase.GastoTransporte.Where(G => G.PK_GastosTransporte == pago.GastoTransporte.PK_GastosTransporte &&
                                                            G.FK_EstadoGastosTransporte == 1).FirstOrDefault();

                    if (gasto != null)
                    {   //si existe el Gasto Transporte

                        if (pago.FecPago.Month >= gasto.FecInicGastosTransporte.Value.Month && pago.FecPago <= gasto.FecFinGastosTransporte)
                        {
                            var pagoGasto = entidadBase.PagoGastoTransporte.Where(PG => PG.IndEstado == 1 &&
                                                                            PG.FecPago.Month == pago.FecPago.Month &&
                                                                            PG.FecPago.Year == pago.FecPago.Year &&
                                                                            PG.FK_GastosTransporte == gasto.PK_GastosTransporte).FirstOrDefault();

                            // No existe el pago del Gasto de Transporte para el mes indicado
                            if (pagoGasto == null)
                            {
                                entidadBase.PagoGastoTransporte.Add(pago);
                                entidadBase.SaveChanges();

                                //// Agregar Detalle de días de Pago;
                                DetallePagoGastoTransporte detalle;

                                foreach (var item in detalles)
                                {
                                    detalle = new DetallePagoGastoTransporte
                                    {
                                        FK_PagoGastoTransporte = pago.PK_PagoGastoTransporte,
                                        FK_TipoDetallePagoGasto = item.TipoDetalleGastoTransporte.PK_TipoDetallePagoGasto,
                                        FecDiaPago = item.FecDiaPago,
                                        MonPago = item.MonPago,
                                        CodEntidad = item.CodEntidad
                                    };
                                    entidadBase.DetallePagoGastoTransporte.Add(detalle);
                                    entidadBase.SaveChanges();
                                }

                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = pago.PK_PagoGastoTransporte
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("Ya existe el pago de gasto transporte para el mes indicado");
                            }
                        }
                        else
                        {
                            throw new Exception("La fecha de pago está fuera del periodo del gasto de transporte");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro del gasto transporte indicado");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message },
                    Mensaje = error.Message
                };
                return respuesta;
            }
        }

        public CRespuestaDTO AgregarPagoGastoTransporte(PagoGastoTransporte pago)
        {
            CRespuestaDTO respuesta;
            try
            {
                var gasto = entidadBase.GastoTransporte.Where(G => G.PK_GastosTransporte == pago.GastoTransporte.PK_GastosTransporte &&
                                                            G.FK_EstadoGastosTransporte == 1).FirstOrDefault();

                if (gasto != null)
                {   //si existe el Gasto Transporte

                    if (pago.FecPago.Month >= gasto.FecInicGastosTransporte.Value.Month && pago.FecPago <= gasto.FecFinGastosTransporte)
                    {
                        var pagoGasto = entidadBase.PagoGastoTransporte.Where(PG => PG.IndEstado == 1 &&
                                                                        PG.FecPago.Month == pago.FecPago.Month &&
                                                                        PG.FecPago.Year == pago.FecPago.Year &&
                                                                        PG.FK_GastosTransporte == gasto.PK_GastosTransporte).FirstOrDefault();

                        // No existe el pago del Gasto de Transporte para el mes indicado
                        if (pagoGasto == null)
                        {
                            var numPago = entidadBase.USP_CALCULAR_GASTO_PAGO_MES(gasto.PK_GastosTransporte, pago.FecPago.Month, pago.FecPago.Year);

                            if (numPago == null)
                                throw new Exception("No se pudo registrar Pago del Gasto Transporte para el mes indicado");

                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = numPago.ToString()
                            };
                            return respuesta;
                        }
                        else
                        {
                            throw new Exception("Ya existe el pago de gasto transporte para el mes indicado");
                        }
                    }
                    else
                    {
                        throw new Exception("La fecha de pago está fuera del periodo del gasto de transporte");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el registro del gasto transporte indicado");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message },
                    Mensaje = error.Message
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Anula un trámite de pago de gasto de transporte
        /// </summary>
        /// <returns>Retorna el trámite anulado</returns>
        public CRespuestaDTO AnularPagoGastoTransporte(PagoGastoTransporte pago)
        {
            CRespuestaDTO respuesta;
            try
            {
                var pagoOld = entidadBase.PagoGastoTransporte.Include("GastoTransporte").Where(PK => PK.PK_PagoGastoTransporte == pago.PK_PagoGastoTransporte).FirstOrDefault();

                if (pagoOld != null)
                {
                    pagoOld.IndEstado = 0;
                    pago = pagoOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = pago.PK_PagoGastoTransporte
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun trámite de pago de gasto de transporte con el código especificado." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }


        //----------------------------------------- BUSCAR/OBTENER/LISTAR/FILTRAR -------------------------------------------
        /// <summary>
        /// Metodo encargado de buscar segun el parametro 
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public CRespuestaDTO BuscarGastoTransporte(object[] parametros)
        {
            CRespuestaDTO respuesta;
            try
            {

                var datos = FiltrarViatiCorrido(parametros);
                if (datos.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GastoTransporteCodigo(datos)
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Método encargado de obtener el gasto transporte que coincida con el código
        /// enviado por parámetro(PK).
        /// </summary>
        /// <param name="codGastoT">Llave primaria o código identificador del gasto de transporte.</param>
        /// <returns>Respuesta conteniendo el gasto de transporte o un error con mensaje</returns>
        //Codigo tipo: yyyy-secuencial
        public CRespuestaDTO ObtenerGastoTransporte(string codGastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(codGastoT);
                var gastoTransporte = entidadBase.GastoTransporte
                                                 .Include("Nombramiento")
                                                 .Include("Nombramiento.CartasPresentacion")
                                                 .Include("Nombramiento.Funcionario")
                                                 .Include("Nombramiento.Puesto")
                                                 .Include("Nombramiento.Puesto.RelPuestoUbicacion")
                                                 .Include("EstadoGastoTransporte")
                                                 .Include("MovimientoGastoTransporte")
                                                 .Include("MovimientoGastoTransporte.DetalleDeduccionGastoTransporte")
                                                 .Include("Presupuesto")
                                                 .Include("PagoGastoTransporte")
                                                 .Include("PagoGastoTransporte.DetallePagoGastoTransporte")
                                                 .Include("PagoGastoTransporte.DetallePagoGastoTransporte.TipoDetalleGastoTransporte")
                                                 .Include("GastoTransporteReintegroDias")
                                                 .FirstOrDefault(D => D.PK_GastosTransporte == cod);
                if (gastoTransporte != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = gastoTransporte,
                        Mensaje = GenerarCodigoInforme(gastoTransporte)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Gasto de Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// metodo encargado de optener un solo GastoTransporte ///Obtiene el gasto de transporte para un funcionario específico
        /// </summary>
        /// <param name="codViaticoC"></param>
        /// <returns></returns>
        //Codigo tipo: yyyy-secuencial
        public CRespuestaDTO ObtenerGastoTransporteActual(string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(cedula);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var gastoTransporte = entidadBase.GastoTransporte
                                                .Include("EstadoGastoTransporte")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.CartasPresentacion")
                                                .Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Include("MovimientoGastoTransporte")
                                                .Include("MovimientoGastoTransporte.DetalleDeduccionGastoTransporte")
                                                .Include("Presupuesto")
                                                .Include("PagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte.TipoDetalleGastoTransporte")
                                                .Include("GastoTransporteReintegroDias")
                                                .Where(G => G.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && G.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3)
                                                .OrderByDescending(G => G.FecFinGastosTransporte)
                                                .FirstOrDefault();
                //.FirstOrDefault(D => D.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && D.EstadoGastoTransporte.PK_EstadoGastosTransporte == 1);
                //.Where(D => D.FecInicDesarraigo.Value.Year == annio)
                //.OrderBy(D => D.FecInicDesarraigo).ToList().ElementAtOrDefault(consecutivo);
                if (gastoTransporte != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = gastoTransporte,
                        Mensaje = GenerarCodigoInforme(gastoTransporte)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Gasto de Transporte actual");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Obtener el pago y sus detalles de acuerdo al código(PK) indicado.
        /// </summary>
        /// <param name="codPago">Primary Key del pago de gasto transporte </param>
        /// <returns>RespuestaDTO con el registro de pago del gasto</returns>
        public CRespuestaDTO ObtenerPagoGastoTransporte(int codPago)
        {
            CRespuestaDTO respuesta;
            try
            {
                //var datos = entidadBase.PagoGastoTransporte.ToList();
                var dato = entidadBase.PagoGastoTransporte
                                                .Include("GastoTransporte")
                                                .Include("DetallePagoGastoTransporte")
                                                .Include("DetallePagoGastoTransporte.TipoDetalleGastoTransporte")
                                                .Where(PK => PK.PK_PagoGastoTransporte == codPago).FirstOrDefault();

                //if (dato != null)
                //{
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato
                    };
                    return respuesta;
                //}
                //else
                //{
                //    throw new Exception("No se encontró el pago del gasto de transporte");
                //}
            }
            catch (Exception error)
            {
                var h = error;
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerMotivoDetalleDeduccion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.DetalleDeduccionGastoTransporte
                                    .Where(Q => Q.PK_DetalleDeduccionGastoTransporte == codigo).FirstOrDefault();

                if (dato != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato.DesMotivo
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ""
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = "err"
                };
            }

            return respuesta;
        }
        /// <summary>
        /// metodo encargado de listar un Gasto transporte
        /// </summary>
        /// <returns></returns>
        public CRespuestaDTO ListarGastoTransporte()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.GastoTransporte
                                                .Include("EstadoGastoTransporte")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")                                                
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.RelPuestoUbicacion").ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GastoTransporteCodigo(datosEntidad)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ListarGastoTransportePago(int mes, int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.GastoTransporte
                                                .Include("EstadoGastoTransporte")
                                                .Include("PagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte.TipoDetalleGastoTransporte")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Where(P => P.FecFinGastosTransporte.Value.Month >= mes &&
                                                            P.FecFinGastosTransporte.Value.Year == anio &&
                                                            P.FK_EstadoGastosTransporte == 1) // 1-Aprobado  3 - Anulado
                                                .ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ListarGastoPagosPendientes(int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.GastoTransporte
                                                .Include("EstadoGastoTransporte")
                                                .Include("PagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte")
                                                .Include("PagoGastoTransporte.DetallePagoGastoTransporte.TipoDetalleGastoTransporte")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Where(P => P.FecFinGastosTransporte.Value.Year == anio &&
                                                            P.FK_EstadoGastosTransporte != 3) // 1- Aprobado  3 - Anulado
                                                .ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron contratos de Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public List<GastoTransporte> ListarPagoMesesAnteriores(string cedula)
        {
            List<GastoTransporte> resultado = new List<GastoTransporte>();

            try
            {
                var datospregunta = entidadBase.GastoTransporte.Where(
                                    C => C.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && 
                                    C.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3).ToList();
                if (datospregunta != null)
                {
                    resultado = datospregunta;
                    return resultado.OrderByDescending(Q => Q.PK_GastosTransporte).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron Preguntas asociadas al tipoFormulario.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public CRespuestaDTO ListarGastoTransporteServicio(int mes, int anio, string cedula) // Lo usa el departamento Financiero
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoGastoTransporte
                                                .Include("GastoTransporte")
                                                .Include("DetallePagoGastoTransporte")
                                                .Include("GastoTransporte.EstadoGastoTransporte")
                                                .Include("GastoTransporte.Nombramiento")
                                                .Include("GastoTransporte.Nombramiento.Funcionario")
                                                .Include("GastoTransporte.Nombramiento.Puesto")
                                                .Include("GastoTransporte.Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.GastoTransporte.FK_EstadoGastosTransporte == 1)  // Válido
                                                .ToList();

                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    if (cedula != null)
                        if (cedula != "")
                            datosEntidad = datosEntidad.Where(D => D.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }


        public CRespuestaDTO ListarPagosGastoTransporte(int mes, int anio) 
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoGastoTransporte
                                                .Include("GastoTransporte")
                                                .Include("DetallePagoGastoTransporte")
                                                .Include("GastoTransporte.Presupuesto")
                                                .Include("GastoTransporte.EstadoGastoTransporte")
                                                .Include("GastoTransporte.Nombramiento")
                                                .Include("GastoTransporte.Nombramiento.Funcionario")
                                                .Include("GastoTransporte.Nombramiento.Puesto")
                                                .Include("GastoTransporte.Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.GastoTransporte.FK_EstadoGastosTransporte == 1)  // Válido
                                                .ToList();

                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO AsignarReservaRecurso(int idPago, string reserva)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoGastoTransporte
                                                .Include("DetallePagoGastoTransporte")
                                                .Include("GastoTransporte")
                                                .Include("GastoTransporte.Nombramiento")
                                                .Include("GastoTransporte.Nombramiento.Funcionario")
                                                .Where(P => P.PK_PagoGastoTransporte == idPago &&
                                                            P.IndEstado == 1 &&
                                                            P.NumBoleta.Length == 0 &&
                                                            P.GastoTransporte.FK_EstadoGastosTransporte != 3) // 3 - Anulado
                                               .FirstOrDefault();

                if (datosEntidad != null)
                {
                    datosEntidad.ReservaRecurso = reserva;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad.PK_PagoGastoTransporte
                    };
                }
                else
                {
                    throw new Exception("No se encontraron datos");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        public CRespuestaDTO ActualizarGastoTransporteServicio(int mes, int anio, string cedula, string reserva, string numBoleta)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoGastoTransporte
                                                .Include("DetallePagoGastoTransporte")
                                                .Include("GastoTransporte")
                                                .Include("GastoTransporte.Nombramiento")
                                                .Include("GastoTransporte.Nombramiento.Funcionario")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                                                            P.GastoTransporte.FK_EstadoGastosTransporte != 3) // 3 - Anulado
                                               .ToList();

                if (datosEntidad.Count > 0)
                {
                    foreach (var item in datosEntidad)
                    {
                        item.NumBoleta = numBoleta;
                        item.ReservaRecurso = reserva;
                        entidadBase.SaveChanges();
                    }
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Metodo encargado de filtrar un gasto de transporte
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private List<GastoTransporte> FiltrarViatiCorrido(object[] parametros)
        {
            var datos = entidadBase.GastoTransporte.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoGastoTransporte")
                                                .Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.RelPuestoUbicacion")
                                                .AsQueryable();
            string elemento;

            for (int i = 0; i < parametros.Length && datos.Count() != 0; i = i + 2)
            {
                elemento = parametros[i].ToString();
                switch (elemento)
                {
                    case "NumFuncionario":
                        var idF = parametros[i + 1].ToString();
                        datos = datos.Where(D => D.Nombramiento.Funcionario.IdCedulaFuncionario == idF);
                        break;
                    case "NumGastoTransporte":
                        var datosBusqueda = parametros[i + 1].ToString().Split('-'); // [año,consecutivo]
                        var annio = int.Parse(datosBusqueda[0]);
                        var consecutivo = int.Parse(datosBusqueda[1]) - 1;
                        var GastoTransporte = datos.Where(D => D.FecInicGastosTransporte.Value.Year == annio).ToList()
                                              .ElementAtOrDefault(consecutivo);
                        if (GastoTransporte != null)
                            datos = datos.Where(D => D.PK_GastosTransporte == GastoTransporte.PK_GastosTransporte);
                        break;
                    case "Estado":
                        var estado = parametros[i + 1].ToString();
                        if (estado == "Todas las Notificaciones")
                            datos = datos.Where(D => D.EstadoGastoTransporte.NomEstado != "Valido"
                                                     && D.EstadoGastoTransporte.NomEstado != "Espera"
                                                     && D.EstadoGastoTransporte.NomEstado != "Anulado");
                        else
                            datos = datos.Where(D => D.EstadoGastoTransporte.NomEstado == estado);
                        break;
                    case "LugarContrato":
                        var distrito = ((List<string>)parametros[i + 1]).ElementAt(0);
                        var canton = ((List<string>)parametros[i + 1]).ElementAt(1);
                        var provincia = ((List<string>)parametros[i + 1]).ElementAt(2);
                        datos = datos.Where(D => D.Nombramiento.Puesto.RelPuestoUbicacion.
                                            Where(R => (R.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1) &&
                                                  (distrito == "null" ? true : (R.UbicacionPuesto.Distrito.NomDistrito == distrito)) &&
                                                  (canton == "null" ? true : (R.UbicacionPuesto.Distrito.Canton.NomCanton == canton)) &&
                                                  (provincia == "null" ? true : (R.UbicacionPuesto.Distrito.Canton.Provincia.NomProvincia == provincia))).Count() > 0);
                        break;
                    case "FechaInicio":
                        var fechaInicioI = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinI = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(D => D.FecInicGastosTransporte >= fechaInicioI && D.FecInicGastosTransporte <= fechaFinI);
                        break;

                    case "FechaFinal":
                        var fechaInicioF = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(D => D.FecFinGastosTransporte >= fechaInicioF && D.FecFinGastosTransporte <= fechaFinF);
                        break;
                    default: throw new Exception("Busqueda no definida");
                }
            }
            return datos.ToList();
        }



        /// <summary>
        /// Metodo encargado de obtener el monto retroactivo
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        //public CRespuestaDTO ObtenerMontoRetroactivo(CartasPresentacion carta, List<DateTime> fecha)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        var nombramiento = entidadBase.Nombramiento
        //                                        .Include("DesgloseSalarial").FirstOrDefault(N => N.CartasPresentacion.FirstOrDefault(C => C.NumCarta == carta.NumCarta) != null);

        //        if (nombramiento != null)
        //        {
        //            var fechaIni = fecha[0];
        //            var fechaFin = fecha[1];

        //            var desglose = nombramiento.DesgloseSalarial.Where(D => D.IndPeriodo >= fechaIni && D.IndPeriodo <= fechaFin);

        //            var detalleDesglose = entidadBase.DetalleDesgloseSalarial // se puede sacar de "var nombramiento"?
        //                                    .Include("DesgloseSalarial")
        //                                    .Where(D => D.DesgloseSalarial.Nombramiento.PK_Nombramiento == nombramiento.PK_Nombramiento).ToList()
        //                                            .Where(D => D.DesgloseSalarial.IndPeriodo >= fechaIni && D.DesgloseSalarial.IndPeriodo <= fechaFin);

        //            if (desglose.Count() != 1)
        //            {
        //                throw new Exception("Fecha en conflicto. Es posible que el rango de fechas incluya dos salarios distintos.");
        //            }

        //            respuesta = new CRespuestaDTO
        //            {
        //                Codigo = 1,
        //                Contenido = detalleDesglose.Sum(D => D.MtoPagocomponenteSalarial) //desglose.FirstOrDefault().MtoTotal
        //            };
        //            return respuesta;
        //        }
        //        else
        //        {
        //            throw new Exception("No se encontró el nombramiento");
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
        //        };
        //        return respuesta;
        //    }
        //}

        public CRespuestaDTO AgregarReintegro(List<GastoTransporteReintegroDias> lista)
        {
            CRespuestaDTO respuesta;
            try
            {
                // Agregar Detalle de días de Pago;
                GastoTransporteReintegroDias reintegro;
                int total = 0;

                foreach (var item in lista)
                {
                    reintegro = new GastoTransporteReintegroDias
                    {
                        FK_GastoTransporte = item.FK_GastoTransporte,
                        FecDia = item.FecDia,
                        MonReintegro = item.MonReintegro,
                        ObsMotivo = item.ObsMotivo,
                        IndEstado = item.IndEstado
                    };
                    entidadBase.GastoTransporteReintegroDias.Add(reintegro);
                    entidadBase.SaveChanges();
                    total++;
                }

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = total
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }


        /// Modificar en la bd la fecha de contrato y el numero de documento de un gasto de transporte
        /// </summary>
        /// <param name="idgasto">PK del gasto de transporte</param>
        /// <param name="feccontrato">El nuevo valor de la fecha contrato del gasto</param>
        /// <param name="numdocumento">El nuevo valor del numero de documento del gasto</param>
        /// <returns></returns>
        public CRespuestaDTO ModificarFecContratoNumDocGT(int idgasto, DateTime feccontrato, string numdocumento)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteOld = entidadBase.GastoTransporte.Where(D => D.PK_GastosTransporte == idgasto).FirstOrDefault();

                if (GastoTransporteOld != null)
                {
                    GastoTransporteOld.FecContrato = feccontrato;
                    GastoTransporteOld.NumDocumento = numdocumento;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(GastoTransporteOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Gasto de Transporte requerido");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ModificarPagoReservaRecursoGT(int idPago, string reserva)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteOld = entidadBase.PagoGastoTransporte.Where(D => D.PK_PagoGastoTransporte == idPago).FirstOrDefault();

                if (GastoTransporteOld != null)
                {
                    GastoTransporteOld.ReservaRecurso = reserva;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GastoTransporteOld.PK_PagoGastoTransporte
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Pago");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        #endregion
    }
}

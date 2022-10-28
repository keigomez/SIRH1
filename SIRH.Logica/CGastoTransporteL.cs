using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Logica
{//
    public class CGastoTransporteL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        //----------------------------------------- CONVERTIR DTO <-> DATOS -----------------------------------------
        /// <summary>
        /// Método encargado de convertir un gasto transporte de Datos a DTO
        /// </summary>
        /// <param name="item">Objeto tipo Gasto Transporte (Datos)</param>
        /// <returns>Retorna el gasto de transporte como la clase DTO</returns>
        internal static CGastoTransporteDTO ConvertirGastoTransporteDatosaDTO(GastoTransporte item) //ACM: Cambiar los nombres de métodos convertir porque están al revés
        {
            return new CGastoTransporteDTO
            {
                NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                MontGastoTransporteDTO = item.MonOriginal.ToString().Replace(",", "."),
                MonActual = item.MonActual,
                FecFinDTO = Convert.ToDateTime(item.FecFinGastosTransporte),
                EstadoGastoTransporteDTO = CEstadoGastoTransporteL.ConvertirEstadoGastoTransporteDatosaDTO(item.EstadoGastoTransporte),
                FecInicioDTO = Convert.ToDateTime(item.FecInicGastosTransporte),
                ObsGastoTransporteDTO = item.ObsGastosTransporte == null ? "" : item.ObsGastosTransporte,
                IdEntidad = item.PK_GastosTransporte,
                PresupuestoDTO = CPresupuestoL.ConvertirPresupuestoDatosaDTO(item.Presupuesto),
                DocAdjunto = item.ImgDocumento,
                DistritoContrato = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito)
                                    : new CDistritoDTO(),
                DistritoTrabajo = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito1)
                                    : new CDistritoDTO()
            };
        }
        
        /// <summary>
        /// Método encargado de convertir un gasto transporte DTO a Datos
        /// </summary>
        /// <param name="item">Objeto tipo CGastoTransporteDTO</param>
        /// <returns>Un objeto tipo Gasto de Transporte</returns>
        internal static GastoTransporte ConvertirGastoTransporteDTOaDatos(CGastoTransporteDTO item)
        {
            return new GastoTransporte
            {
                Nombramiento = CNombramientoL.ConvertirDatosNombramientoADATOS(item.NombramientoDTO),
                MonOriginal = Convert.ToDecimal(item.MontGastoTransporteDTO),
                MonActual = item.MonActual,
                FecFinGastosTransporte = Convert.ToDateTime(item.FecFinDTO),
                FecInicGastosTransporte = Convert.ToDateTime(item.FecInicioDTO),
                ObsGastosTransporte = item.ObsGastoTransporteDTO == null ? "" : item.ObsGastoTransporteDTO,
                EstadoGastoTransporte = CEstadoGastoTransporteL.ConvertirEstadoGastoTransporteDTOaDatos(item.EstadoGastoTransporteDTO),
                PK_GastosTransporte = item.IdEntidad,
                ImgDocumento = item.DocAdjunto
            };
        }

        internal static CGastoTransporteRutasDTO ConvertirRutaDatosaDTO(GastoTransporteRutas item) //ACM: Cambiar los nombres de métodos convertir porque están al revés
        {
            return new CGastoTransporteRutasDTO
            {
                IdEntidad = item.PK_Ruta,
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                Estado = CEstadoGastoTransporteL.ConvertirEstadoGastoTransporteDatosaDTO(item.EstadoGastoTransporte),
                MonDiario = item.MonGasto
            };
        }

        internal static GastoTransporteRutas ConvertirRutaDTOaDatos(CGastoTransporteRutasDTO item) //ACM: Cambiar los nombres de métodos convertir porque están al revés
        {
            return new GastoTransporteRutas
            {
                PK_Ruta = item.IdEntidad,
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecRige),
                EstadoGastoTransporte = CEstadoGastoTransporteL.ConvertirEstadoGastoTransporteDTOaDatos(item.Estado),
                MonGasto = item.MonDiario
            };
        }

        //----------------------------------------------- CONSTRUIR -------------------------------------------------
        /// <summary>
        /// metodo encargado de contruir un gasto transport general
        /// </summary>
        /// <param name="item">tipo: Gasto Transporte Datos</param>
        /// <returns></returns>
        internal static CGastoTransporteDTO ConstruirGastoTransporteGeneral(GastoTransporte item)
        {
            List<CPagoGastoTransporteDTO> pagosList = new List<CPagoGastoTransporteDTO>();
            List<CMovimientoGastoTransporteDTO> movimList = new List<CMovimientoGastoTransporteDTO>();
            List<CGastoTransporteReintegroDTO> listaReintegro = new List<CGastoTransporteReintegroDTO>();

            List<CGastoTransporteRutasDTO> listaRutas = new List<CGastoTransporteRutasDTO>();
            List<CDetalleAsignacionGastoTransporteDTO> detalleRutas = new List<CDetalleAsignacionGastoTransporteDTO>();

            // RUTAS
            foreach (var ruta in item.GastoTransporteRutas)
            {
                detalleRutas = new List<CDetalleAsignacionGastoTransporteDTO>();

                var dato = ConvertirRutaDatosaDTO(ruta);
                foreach (var detalle in ruta.DetalleAsignacionGastoTransporte)
                {
                    detalleRutas.Add(CDetalleAsignacionGastoTransporteL.ConvertirDetalleAsignacionGastoTransporteDatosaDTO(detalle));
                }

                dato.Detalle = detalleRutas;
                listaRutas.Add(dato);
            }

            // PAGOS
            foreach (var pago in item.PagoGastoTransporte)
            {
                pagosList.Add(ConstruirPagoGastoTransporteGeneral(pago));
            }

            // REINTEGROS
            foreach (var reintegro in item.GastoTransporteReintegroDias)
            {
                listaReintegro.Add(new CGastoTransporteReintegroDTO
                {
                    IdEntidad = reintegro.PK_Reintegro,
                    FecDiaDTO = reintegro.FecDia,
                    MonReintegroDTO = reintegro.MonReintegro,
                    ObsMotivoDTO = reintegro.ObsMotivo,
                    EstadoDTO = reintegro.IndEstado
                });
            }

            // MOVIMIENTOS
            foreach (var mov in item.MovimientoGastoTransporte)
            {
                movimList.Add(CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos(mov));
            }

            return new CGastoTransporteDTO
            {
                NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                MontGastoTransporteDTO = item.MonOriginal.ToString().Replace(",", "."),
                MonActual = item.MonActual,
                FecFinDTO = Convert.ToDateTime(item.FecFinGastosTransporte),
                FecInicioDTO = Convert.ToDateTime(item.FecInicGastosTransporte),
                ObsGastoTransporteDTO = item.ObsGastosTransporte == null ? "" : item.ObsGastosTransporte,                
                IdEntidad = item.PK_GastosTransporte,
                DocAdjunto = item.ImgDocumento,
                EstadoGastoTransporteDTO = ConstruirEstadoGastoTransporte(item.EstadoGastoTransporte),
                PresupuestoDTO = CPresupuestoL.ConvertirPresupuestoDatosaDTO(item.Presupuesto),
                Pagos = pagosList.OrderByDescending(Q => Q.IdEntidad).ToList(),
                Movimientos = movimList,
                FecRegistroDTO = item.FecRegistro,
                DistritoContrato = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito)
                                    : new CDistritoDTO(),
                DistritoTrabajo = item.FK_DistritoContrato != null ?
                                    CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito1)
                                    : new CDistritoDTO(),
                Rutas = listaRutas
            };
        }
        
        /// <summary>
        ///  metodo encargado de contruir un estado gasto transporte Datos
        /// </summary>
        /// <param name="item">tipo: Estado Gasto Transporte Datos</param>
        /// <returns></returns>
        internal static CEstadoGastoTransporteDTO ConstruirEstadoGastoTransporte(EstadoGastoTransporte item)
        {
            return new CEstadoGastoTransporteDTO
            {
                IdEntidad = item.PK_EstadoGastosTransporte,
                NomEstadoDTO = item.NomEstado
            };
        }
        
        /// <summary>
        /// Encargado de reunir todos los datos de pagos y detalles de pago de gasto de transporte y 
        /// los retorna como objetos DTO
        /// </summary>
        /// <param name="item">Pago de gasto de transporte</param>
        /// <returns>Objeto CPagoGastoTransporteDTO con todos los datos del pago</returns>
        internal static CPagoGastoTransporteDTO ConstruirPagoGastoTransporteGeneral(PagoGastoTransporte item)
        {
            List<CDetallePagoGastoTrasporteDTO> lista = new List<CDetallePagoGastoTrasporteDTO>();
            CViaticoCorridoD intermedio = new CViaticoCorridoD(new SIRHEntities());
            string desTipo = "";

            foreach (var detalle in item.DetallePagoGastoTransporte.OrderBy(Q => Q.FecDiaPago).ToList())
            {
                desTipo = detalle.TipoDetalleGastoTransporte.DesTipoDetalle;
                // Si es tipo 4 Deducción, se adjunta el motivo,
                if (detalle.TipoDetalleGastoTransporte.PK_TipoDetallePagoGasto == 4)
                {
                    desTipo += " " + intermedio.ObtenerMotivoDetalleDeduccion(detalle.CodEntidad).Contenido;
                }
                lista.Add(new CDetallePagoGastoTrasporteDTO
                {
                    IdEntidad = detalle.PK_DetallePago,
                    FecDiaPago = detalle.FecDiaPago.ToShortDateString(),
                    MonPago = detalle.MonPago,
                    CodEntidad = detalle.CodEntidad,
                    TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                    {
                        IdEntidad = detalle.TipoDetalleGastoTransporte.PK_TipoDetallePagoGasto,
                        DescripcionTipo = desTipo
                    }
                });
            }

            return new CPagoGastoTransporteDTO
            {
                IdEntidad = item.PK_PagoGastoTransporte,
                FecPago = item.FecPago,
                MonPago = item.MonPago,
                MonContrato = item.MonContrato,
                HojaIndividualizada = item.HojaIndividualizada,
                NumBoleta = item.NumBoleta,
                ReservaRecurso = item.ReservaRecurso,
                IndEstado = item.IndEstado,
                FecRegistro = item.FecRegistro,
                Detalles = lista,                
                GastoTransporteDTO = new CGastoTransporteDTO { IdEntidad = item.FK_GastosTransporte }
            };
        }


        //-----------------------------------------AGREGAR/MODIFICAR/ANULAR -----------------------------------------
        /// <summary>
        /// Método encargado de agregar un gasto transporte y su detalle (las rutas) a la base de datos.
        /// </summary>
        /// <param name="carta">Número de carta de presentación del funcionario</param>
        /// <param name="funcionario">El funcionario a quien se le agrega un GT</param>
        /// <param name="gastoTransporte">Los datos del GT ingresados en la vista</param>
        /// <param name="DetalleAGT">La lista de Rutas que realizó el funcionario</param>
        /// <returns>Retorna la respuesta BaseDTO en caso de ser correcto o ErrorDTO si hubo un error en el proceso</returns>
        public List<CBaseDTO> AgregarGastoTransporte(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CGastoTransporteDTO gastoTransporte, List<CDetalleAsignacionGastoTransporteDTO> DetalleAGT)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CGastoTrasporteD intermedioGastoTransporte = new CGastoTrasporteD(contexto);
                CEstadoGastoTransporteD intermedioEstado = new CEstadoGastoTransporteD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);
                CDistritoD intermedioDistrito = new CDistritoD(contexto);

                var cartaDB = new CartasPresentacion { NumCarta = carta.NumeroCarta };
                var funcionarioDB = new Funcionario { IdCedulaFuncionario = funcionario.Cedula };
                var GastoTransporteDB = new GastoTransporte
                {
                    MonOriginal = Convert.ToDecimal(gastoTransporte.MontGastoTransporteDTO),
                    MonActual = Convert.ToDecimal(gastoTransporte.MontGastoTransporteDTO),
                    FecFinGastosTransporte = gastoTransporte.FecFinDTO,
                    FecInicGastosTransporte = gastoTransporte.FecInicioDTO,
                    ObsGastosTransporte = gastoTransporte.ObsGastoTransporteDTO == null ? "" : gastoTransporte.ObsGastoTransporteDTO,
                    EstadoGastoTransporte = (EstadoGastoTransporte)intermedioEstado.BuscarEstadoGastoTransporteNombre(gastoTransporte.EstadoGastoTransporteDTO.NomEstadoDTO).Contenido,
                    Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(gastoTransporte.PresupuestoDTO.CodigoPresupuesto).Contenido,
                    ImgDocumento = gastoTransporte.DocAdjunto,
                    Distrito = intermedioDistrito.CargarDistritoId(gastoTransporte.DistritoContrato.IdEntidad),
                    Distrito1 = intermedioDistrito.CargarDistritoId(gastoTransporte.DistritoTrabajo.IdEntidad)
                    //Nombramiento = CNombramientoL.ConvertirDatosNombramientoADATOSBasicos(gastoTransporte.NombramientoDTO)
                };

                var insertarGT = intermedioGastoTransporte.AgregarGastoTransporte(cartaDB, funcionarioDB, GastoTransporteDB);
                //Si consigue insertar
                if (insertarGT.Codigo > 0)
                {
                    //AgregarFacturas(facturas, desarraigoDB, respuesta);
                    //AgregarContratos(contratos, desarraigoDB, respuesta);

                    respuesta.Add(insertarGT);

                    //Crear la Ruta
                    GastoTransporteRutas rutaBD = new GastoTransporteRutas
                    {
                        FK_GastoTransporte = GastoTransporteDB.PK_GastosTransporte,
                        FecRige = Convert.ToDateTime(GastoTransporteDB.FecInicGastosTransporte),
                        FecVence = Convert.ToDateTime(GastoTransporteDB.FecFinGastosTransporte),
                        EstadoGastoTransporte = (EstadoGastoTransporte)intermedioEstado.BuscarEstadoGastoTransporteId(1).Contenido,// Estado Válido
                        MonGasto = DetalleAGT.Sum(Q => Q.MontTarifa) * 2
                    };
                    var insertarRuta = intermedioGastoTransporte.AgregarGastoTransporteRuta(rutaBD);
                    if (insertarRuta.Codigo > 0)
                    {
                        CDetalleAsignacionGastoTrasporteD intermedioDE = new CDetalleAsignacionGastoTrasporteD(contexto);
                        foreach (var item in DetalleAGT)
                        {
                            //Crear el detalle (las rutas del GT)
                            DetalleAsignacionGastoTransporte detalleRutasBD = new DetalleAsignacionGastoTransporte
                            {
                                FK_Ruta = insertarRuta.IdEntidad,
                                MontTarifa = item.MontTarifa,
                                NomFraccionamiento = item.NomFraccionamientoDTO,
                                NomRuta = item.NomRutaDTO,
                                NumGaceta = item.NumGaceta
                            };
                            //Insertar detalle de ruta(detalleAsignacion) a la BD y respuesta
                            var insertarDetalleRutas = intermedioDE.AgregarDetalleAsignacionGastoTransporte(detalleRutasBD, insertarRuta.IdEntidad);
                            if (insertarDetalleRutas.Codigo > 0)
                                respuesta.Add(CDetalleAsignacionGastoTransporteL.ConvertirDetalleAsignacionGastoTransporteDatosaDTO((DetalleAsignacionGastoTransporte)insertarDetalleRutas.Contenido));
                            else
                                throw new Exception(((CErrorDTO)respuesta[2]).MensajeError);
                        }
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)insertarRuta.Contenido).MensajeError);
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)insertarGT.Contenido);
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return respuesta;
        }   
        ///// <summary>
        ///// Agregar en la BD la nueva lista de rutas modificadas
        ///// </summary>
        ///// <param name="asigModificadas">Lista de rutas del gasto que se van a modificar</param>
        ///// <param name="idgasto">PK del gasto de transporte al que se le modifican las rutas</param>
        ///// <returns></returns>
        public List<List<CBaseDTO>> AgregarDetalleAsignModificada(List<CDetalleAsignacionGastoTransporteModificadaDTO> asigModificadas, int idgasto )
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            //try
            //{
            //    CDetalleAsignacionGastoTransporteL asignLogica = new CDetalleAsignacionGastoTransporteL();
            //    CDetalleAsignacionGastoTransporteModificadaL asignModifLogica = new CDetalleAsignacionGastoTransporteModificadaL();
            //    List<CDetalleAsignacionGastoTransporteModificadaDTO> onlyModified = new List<CDetalleAsignacionGastoTransporteModificadaDTO>();

            //    //Traer lista de asignaciones normales para ese gasto,
            //    var asignOriginales = asignLogica.ListarAsignacion(idgasto);
            //    if (asignOriginales != null)
            //    {
            //        if(asignOriginales.Count == asigModificadas.Count)
            //        {
            //            for (int i = 0; i < asignOriginales.Count; i++)
            //            {
            //                //Sacar los valores de las rutas originales
            //                var oldruta = ((CDetalleAsignacionGastoTransporteDTO)asignOriginales[i]).NomRutaDTO;
            //                var oldFrac = ((CDetalleAsignacionGastoTransporteDTO)asignOriginales[i]).NomFraccionamientoDTO;
            //                var oldTarif = ((CDetalleAsignacionGastoTransporteDTO)asignOriginales[i]).MontTarifa;

            //                var result = asigModificadas.Where(w => !(((CDetalleAsignacionGastoTransporteDTO)asignOriginales[i]).NomRutaDTO.Equals(w.NomRutaDTO)));//.Contains(w));

            //                //Comparar las rutas originales con las que vienen a modificar para ver dónde sí hay cambios
            //                if (oldruta != asigModificadas[i].NomRutaDTO ||
            //                     oldFrac != asigModificadas[i].NomFraccionamientoDTO ||
            //                     oldTarif != asigModificadas[i].MontTarifa)                            
            //                {
            //                    //Algo cambió. Guardar en una lista temporal SOLO las que tienen cambios y se van a modificar.
            //                    onlyModified.Add(asigModificadas[i]);
            //                }                            
            //            }
            //        }
            //        else
            //        {
            //            CErrorDTO error = new CErrorDTO
            //            {
            //                IdEntidad = -1,
            //                MensajeError = "Las listas no coinciden"
            //            };
            //            throw new Exception(error.MensajeError);
            //        }
            //        respuesta.Add(asignModifLogica.AgregarDetalleAsignacionGTModificada(onlyModified, idgasto));

            //        //PENDIENTE: verificar en una lista de registros de la tabla modificada si la ruta (para ese gasto) ya se ha agregado antes...
            //        //..si sí, hacerle update
            //    }
            //    return respuesta;
            //}
            //catch
            //{
            //    return respuesta;
            //}

            return respuesta;
        }

        public CBaseDTO ModificarEstadoGastoTransporte(CGastoTransporteDTO gastoT)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                CEstadoGastoTransporteD intermedioEstado = new CEstadoGastoTransporteD(contexto);
                var gastoTransporteDB = new GastoTransporte
                {
                    PK_GastosTransporte = gastoT.IdEntidad
                };
                var estado = intermedioEstado.BuscarEstadoGastoTransporteNombre(gastoT.EstadoGastoTransporteDTO.NomEstadoDTO).Contenido;
                if (estado.GetType() == typeof(CErrorDTO))
                    return (CBaseDTO)estado;
                var datosGastoT = intermedio.ModificarEstadoGastoTransporte(gastoTransporteDB, (EstadoGastoTransporte)estado);
                if (datosGastoT.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = gastoT.IdEntidad,
                        Mensaje = gastoT.CodigoGastoTransporte
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoT.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        /// <summary>
        /// metodo encargado de modificar un GastoTransporte
        /// </summary>
        /// <param name="viaticoC"></param>
        /// <param name="contratos"></param>
        /// <returns></returns>
        public CBaseDTO ModificarGastoTransporte(CGastoTransporteDTO gastoT)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);

                var Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(gastoT.PresupuestoDTO.CodigoPresupuesto).Contenido;

                var gastoTransporteDB = new GastoTransporte
                {
                    PK_GastosTransporte = gastoT.IdEntidad,
                    FK_Presupuesto = Presupuesto.PK_Presupuesto,
                    ImgDocumento = gastoT.DocAdjunto
                };
               
                var datosGastoT = intermedio.ModificarGastoTransporte(gastoTransporteDB);
                if (datosGastoT.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = gastoT.IdEntidad,
                        Mensaje = gastoT.CodigoGastoTransporte
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoT.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        /// <summary>
        /// Método encargado de llamar la capa de datos para modificar el monto de un gasto de transporte
        /// al que se le hayan modificado una o más rutas.
        /// </summary>
        /// <param name="idgasto">PK del gasto</param>
        /// <param name="newMonto">Nuevo valor del total del gasto</param>
        /// <returns></returns>
        public CBaseDTO ModificarMontoGastoTrans(int idgasto, decimal newMonto)
        {
            CBaseDTO respuesta;
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                var datosGastoT = intermedio.ModificarMontoGastoTrans(idgasto, newMonto);
                if (datosGastoT.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = idgasto,
                        Mensaje = "Monto modificado con éxito"
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoT.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        /// <summary>
        /// encargado de Actualizar Vencimiento Gasto Trasnporte
        /// </summary>
        /// <param name="fechaVencimiento"></param>
        public List<List<CBaseDTO>> ActualizarVencimientoGastoTransporte(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                var gastoT = intermedio.ActualizarVencimientoGastoTransporte(fechaVencimiento);

                if (gastoT.Codigo > 0)
                {

                    var datosGastoT = (List<GastoTransporte>)(((List<object>)gastoT.Contenido)[0]);
                    var codigos = (List<string>)(((List<object>)gastoT.Contenido)[1]);

                    for (int i = 0; i < datosGastoT.Count; i++)
                    {
                        List<CBaseDTO> GastoTList = new List<CBaseDTO>();
                        var desAux = CGastoTransporteL.ConstruirGastoTransporteGeneral(datosGastoT[i]);
                        desAux.CodigoGastoTransporte = codigos[i];
                        GastoTList.Add(desAux);
                        GastoTList.Add(CFuncionarioL.FuncionarioGeneral(datosGastoT[i].Nombramiento.Funcionario));
                        respuesta.Add(GastoTList);
                    }
                }
                else
                {
                    List<CBaseDTO> GastoTList = new List<CBaseDTO>();
                    GastoTList.Add((CErrorDTO)gastoT.Contenido);
                    respuesta.Add(GastoTList);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> GastoTList = new List<CBaseDTO>();
                GastoTList.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(GastoTList);
            }

            return respuesta;
        }
        
        /// <summary>
        /// Metodo encargado de anular GastoTransporte
        /// </summary>
        /// <param name="gastoT"></param>
        /// <returns></returns>
        public CBaseDTO AnularGastoTransporte(CGastoTransporteDTO gastoT)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                var gastoTransporteDB = new GastoTransporte
                {
                    PK_GastosTransporte = gastoT.IdEntidad,
                    ObsGastosTransporte = gastoT.ObsGastoTransporteDTO
                };
                var datosGastoTransporte = intermedio.AnularGastoTransporte(gastoTransporteDB);
                if (datosGastoTransporte.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = gastoT.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoTransporte.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO FinalizarGastoTransporte(CGastoTransporteDTO gastoT)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                var gastoTransporteDB = new GastoTransporte
                {
                    PK_GastosTransporte = gastoT.IdEntidad,
                    FecFinGastosTransporte = gastoT.FecFinDTO,
                    ObsGastosTransporte = gastoT.ObsGastoTransporteDTO
                };
                var datosGastoTransporte = intermedio.FinalizarGastoTransporte(gastoTransporteDB);
                if (datosGastoTransporte.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = gastoT.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoTransporte.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        /// <summary>
        /// Encargado de enviar los datos del pago de gasto de transporte para almacenarlos en la BD
        /// </summary>
        /// <param name="pago">El pago a agregar</param>
        /// <param name="detalles">La lista de detalles del pago</param>
        /// <param name="funcionario">El funcionario a quien se le hará el pago</param>
        /// <returns></returns>
        public CBaseDTO AgregarPagoGastoTransporte(CPagoGastoTransporteDTO pago, List<CDetallePagoGastoTrasporteDTO> detalles, CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedioGT = new CGastoTrasporteD(contexto);
                //Tomar los datos del pago en un objeto PagoGastoTransporte
                var datoPago = new PagoGastoTransporte
                {
                    IndEstado = pago.IndEstado,
                    FecPago = pago.FecPago,
                    MonPago = pago.MonPago,
                    HojaIndividualizada = pago.HojaIndividualizada,
                    NumBoleta = pago.NumBoleta,
                    ReservaRecurso = pago.ReservaRecurso,
                    FecRegistro = DateTime.Now
                };
                var dato = intermedioGT.ObtenerGastoTransporte(pago.GastoTransporteDTO.IdEntidad.ToString());
                datoPago.GastoTransporte = (GastoTransporte)dato.Contenido;
                
                //Obtener la lista de detalles del pago
                List<DetallePagoGastoTransporte> datoDetalle = new List<DetallePagoGastoTransporte>();
                foreach (var item in detalles)
                {
                    datoDetalle.Add(new DetallePagoGastoTransporte
                    {
                        TipoDetalleGastoTransporte = new TipoDetalleGastoTransporte
                        {
                            PK_TipoDetallePagoGasto = item.TipoDetalleDTO.IdEntidad
                        },
                        FecDiaPago = Convert.ToDateTime(item.FecDiaPago),
                        MonPago = item.MonPago,
                        CodEntidad = item.CodEntidad,
                    });
                }
                var datoFuncionario = new Funcionario { IdCedulaFuncionario = funcionario.Cedula };
                respuesta = intermedioGT.AgregarPagoGastoTransporte(datoPago, datoDetalle, datoFuncionario);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    //respuesta = ((CRespuestaDTO)respuesta).Contenido;
                    //respuesta = ((CErrorDTO)((CRespuestaDTO)respuesta).Contenido);

                    throw new Exception(((CErrorDTO)respuesta).Mensaje);

                    //respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    //throw new Exception();
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

        public CBaseDTO AgregarPagoGastoTransporte(CPagoGastoTransporteDTO pago)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedioGT = new CGastoTrasporteD(contexto);
                //Tomar los datos del pago en un objeto PagoGastoTransporte
                var datoPago = new PagoGastoTransporte
                {
                    IndEstado = pago.IndEstado,
                    FecPago = pago.FecPago,
                    FecRegistro = DateTime.Now
                };
                var dato = intermedioGT.ObtenerGastoTransporte(pago.GastoTransporteDTO.IdEntidad.ToString());

                datoPago.GastoTransporte = (GastoTransporte)dato.Contenido;
                datoPago.MonContrato = datoPago.GastoTransporte.MonActual;

                respuesta = intermedioGT.AgregarPagoGastoTransporte(datoPago);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    //respuesta = ((CRespuestaDTO)respuesta).Contenido;
                    //respuesta = ((CErrorDTO)((CRespuestaDTO)respuesta).Contenido);

                    throw new Exception(((CErrorDTO)respuesta).Mensaje);

                    //respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    //throw new Exception();
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
        /// Encargado de anular un pago de gasto de transporte.
        /// </summary>
        /// <param name="pago"></param>
        /// <returns></returns>
        public CBaseDTO AnularPagoGastoTransporte(CPagoGastoTransporteDTO pago)
        {
            CBaseDTO respuesta;
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                var datoPago = new PagoGastoTransporte
                {
                    PK_PagoGastoTransporte = pago.IdEntidad
                };

                var datos = intermedio.AnularPagoGastoTransporte(datoPago);
                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = Convert.ToInt16(datos.Contenido)
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

        //--------------------------------------------- BUSCAR/OBTENER/LISTAR-------------------------------------------
        /// <summary>
        /// Método encargado de buscar gasto transporte
        /// </summary>
        /// <param name="funcionario"></param>
        /// <param name="gastoTransporte"></param>
        /// <param name="fechasEmision"></param>
        /// <param name="fechasVencimiento"></param>
        /// <param name="lugarContrato"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> BuscarGastoTransporte(CFuncionarioDTO funcionario, CGastoTransporteDTO gastoTransporte,
                                    List<DateTime> fechasEmision, List<DateTime> fechasVencimiento, List<string> lugarContrato)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();
            List<GastoTransporte> datosGastoTransporte = new List<GastoTransporte>();
            List<object> parametros = new List<object>();
            List<string> codigos = new List<string>();

            if (funcionario.Cedula != null)
            {
                parametros.Add("NumFuncionario");
                parametros.Add(funcionario.Cedula);
            }
            if (fechasEmision.Count == 2)
            {
                parametros.Add("FechaInicio");
                parametros.Add(fechasEmision);
            }
            if (fechasVencimiento.Count == 2)
            {
                parametros.Add("FechaFinal");
                parametros.Add(fechasVencimiento);
            }
            if (lugarContrato.Count > 0)
            {
                parametros.Add("LugarContrato");
                parametros.Add(lugarContrato);
            }
            var datos = intermedio.BuscarGastoTransporte(parametros.ToArray());

            if (datos.Codigo > 0)
            {
                datosGastoTransporte = (List<GastoTransporte>)(((List<object>)datos.Contenido)[0]);
                codigos = (List<string>)(((List<object>)datos.Contenido)[1]);

                for (int i = 0; i < datosGastoTransporte.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosGastoTransporte[i];
                    var desa = ConstruirGastoTransporteGeneral(item);
                    desa.CodigoGastoTransporte = codigos[i];
                    temp.Add(desa);
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));
                    //temp.Add(CPuestoL.ConstruirPuesto(item.Nombramiento.Puesto, new CPuestoDTO()));
                    //var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(item.Nombramiento.Funcionario.IdCedulaFuncionario);
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

        /// <summary>
        /// Busca un funcionario por cedula
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<CBaseDTO> BuscarFuncionarioCedulaGT(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CPuestoL intermedioPuesto = new CPuestoL();
            CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
            try
            {
                respuesta = (new CAccionPersonalL()).BuscarFuncionarioDetallePuesto(cedula);
                if (respuesta[0].GetType() != typeof(CErrorDTO))
                {
                    //var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(((CFuncionarioDTO)respuesta[0]).Cedula);// 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]
                    var detallePuesto = intermedioPuesto.DetallePuestoPorCodigo(((CPuestoDTO)respuesta[1]).CodPuesto);// 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]
                    if (detallePuesto.Count > 2)
                    {
                        if (detallePuesto.ElementAtOrDefault(2) != null && detallePuesto.ElementAtOrDefault(3) != null)
                        {
                            var idNombramiento = ((CNombramientoDTO)respuesta.ElementAtOrDefault(4)).IdEntidad;
                            var nombramiento = intermedioNombramiento.CargarNombramiento(idNombramiento);
                            respuesta.Add(detallePuesto.ElementAtOrDefault(2));
                            respuesta.Add(detallePuesto.ElementAtOrDefault(3));
                            respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento));
                        }
                        else
                        {
                            throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicacion del Contrato");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicacion del Contrato");
                    }
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
        /// Método encargado de obtener el gasto transporte que coincida con el código(PK)
        /// enviado por parámetro.
        /// </summary>
        /// <param name="codigo">Codigo identificador(PK) del gasto de transporte</param>
        /// <returns>Respuesta con el gasto de transporte o con un error</returns>
        public List<List<CBaseDTO>> ObtenerGastosTransporte(string codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {                
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                CPuestoL intermedioPuesto = new CPuestoL();

                var gastoT = intermedio.ObtenerGastoTransporte(codigo);
                List<CBaseDTO> temp = new List<CBaseDTO>();

                if (gastoT.Codigo > 0)
                {
                    var datoviaticoCorrido = ConstruirGastoTransporteGeneral((GastoTransporte)gastoT.Contenido);
                    datoviaticoCorrido.CodigoGastoTransporte = gastoT.Mensaje;
                    temp.Add(datoviaticoCorrido); //0 - 0

                    var funcionario = ((GastoTransporte)gastoT.Contenido).Nombramiento.Funcionario;
                    var datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario);

                    temp.Add(datoFuncionario);//0 - 1

                    var nombramiento = ((GastoTransporte)gastoT.Contenido).Nombramiento;
                    var datosNombramiento = CNombramientoL.NombramientoGeneral(nombramiento);

                    temp.Add(datosNombramiento);//0 - 2

                    //var puesto = ((Desarraigo)desarraigo.Contenido).Nombramiento.Puesto;
                    // var datosPuesto = CPuestoL.ConstruirPuesto(puesto, new CPuestoDTO());

                    // temp.Add(datosPuesto); //0 - 3

                    CCartaPresentacionDTO datosCarta = new CCartaPresentacionDTO();
                    var cartaPresentacion = nombramiento.CartasPresentacion.FirstOrDefault();
                    if(cartaPresentacion != null)
                    {
                        datosCarta = CCartaPresentacionL.ConstruirCartaPresentacion(cartaPresentacion);
                    }
                    temp.Add(datosCarta); // 0 - 4

                    respuesta.Add(temp);

                    //var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(funcionario.IdCedulaFuncionario);
                    var detallePuesto = intermedioPuesto.DetallePuestoPorCodigo(nombramiento.Puesto.CodPuesto);
                    respuesta.Add(detallePuesto); // 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]

                   // var facturas = ((GastoTransporte)gastoT.Contenido).FacturaDesarraigo.ToList();
                  //  var datosFacturas = CFacturaDesarraigoL.ConstruirFacturaGastoTransporte(facturas);

                 //   respuesta.Add(datosFacturas); // 2

                   // var contratos = ((GastoTransporte)gastoT.Contenido).ContratoArrendamiento.ToList();
                  //  var datosContratos = CContratoArrendamientoL.ConstruirContratoArrendamientoGastoTransporte(contratos);

                   // respuesta.Add(datosContratos); // 3
                }
                else
                {
                    temp.Add((CErrorDTO)gastoT.Contenido);
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
        
        /// <summary>
        /// Obtiene el gasto de transporte actual correspondiente al funcionario con la cédula indicada
        /// </summary>
        /// <param name="cedula">Número de cédula del funcionario</param>
        /// <returns>Registro de gasto de transporte actual</returns>
        public CBaseDTO ObtenerGastoTransporteActual(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                var datos = intermedio.ObtenerGastoTransporteActual(cedula);
                if (datos.Codigo > 0)
                {
                    respuesta = ConstruirGastoTransporteGeneral((GastoTransporte)datos.Contenido);
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

        /// <summary>
        /// Encargado de obtener los días que se deben de rebajar del pago del Gasto de Transporte, 
        /// correspondientes a un mes
        /// </summary>
        /// <returns></returns>
        //public List<CBaseDTO> ObtenerDiasRebajarGT(CFuncionarioDTO funcionario, int mes, int anio, decimal montoDia)
        //{
        //    List<CBaseDTO> respuesta = new List<CBaseDTO>();
        //    List<DateTime> fechas = new List<DateTime>();
        //    List<DateTime> fechaInicio = new List<DateTime>();
        //    List<DateTime> fechaFin = new List<DateTime>();
        //    List<CDetallePagoGastoTrasporteDTO> detalles = new List<CDetallePagoGastoTrasporteDTO>();

        //    try
        //    {
        //        var fecPrimerDia = new DateTime(anio, mes, 1);
        //        var fecUltDia = fecPrimerDia.AddMonths(1).AddDays(-1);

        //        var listaDias = Enumerable.Range(0, (fecUltDia - fecPrimerDia).Days + 1)
        //                                      .Select(d => fecPrimerDia.AddDays(d))
        //                                      .ToArray();

        //        //
        //        // Buscar Incapacidades
        //        CRegistroIncapacidadD intermedioIncap = new CRegistroIncapacidadD(contexto);
        //        CRegistroIncapacidadDTO registroIncap = new CRegistroIncapacidadDTO
        //        {
        //            EstadoIncapacidad = 2 // Aprobada
        //        };

        //        var datosIncap = intermedioIncap.BuscarRegistroIncapacidad(funcionario, registroIncap, null, fechaInicio, fechaFin);

        //        if (datosIncap.Codigo > 0)
        //        {
        //            // Lista de Incapacidades
        //            foreach (var item in (List<RegistroIncapacidad>)datosIncap.Contenido)
        //            {
        //                // La lista de los días de Incapacidad
        //                foreach (var detalleI in item.DetalleIncapacidad)
        //                {
        //                    var fecha = Convert.ToDateTime(detalleI.FecDia);
        //                    if (fecha >= fecPrimerDia && fecha <= fecUltDia)
        //                    {
        //                        fechas.Add(fecha);
        //                        respuesta.Add(new CDetallePagoGastoTrasporteDTO
        //                        {
        //                            FecDiaPago = fecha.ToShortDateString(),
        //                            MonPago = montoDia,
        //                            CodEntidad = item.PK_RegistroIncapacidad,
        //                            TipoDetalleDTO = new CTipoDetallePagoGastoDTO
        //                            {
        //                                IdEntidad = 1,
        //                                DescripcionTipo = "Incapacidad"
        //                            }
        //                        });
        //                    }
        //                }
        //            }
        //        }


        //        //
        //        // Buscar Vacaciones


        //        //
        //        // Buscar Permisos con / sin goce salarial
        //        CAccionPersonalL intermedioAP = new CAccionPersonalL();
        //        CAccionPersonalDTO accion = new CAccionPersonalDTO();

        //        CEstadoBorradorDTO tipoEst = new CEstadoBorradorDTO
        //        {
        //            IdEntidad = 7
        //        };
        //        accion.Estado = tipoEst;

        //        var datosAP = intermedioAP.BuscarAccion(funcionario, null, accion, fechaInicio);

        //        if (!datosAP.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
        //        {
        //            var listaEstados = new List<int> { 6, 7, 9, 10 };

        //            // Lista de Acciones de Personal
        //            foreach (var item in datosAP)
        //            {
        //                var temp = (CAccionPersonalDTO)item.ElementAt(0);

        //                if (listaEstados.Contains(temp.TipoAccion.IdEntidad))
        //                {
        //                    // La lista de los días de Incapacidad
        //                    foreach (var dia in listaDias)
        //                    {
        //                        if (dia >= temp.FecRige && dia <= (temp.FecVence.HasValue ? temp.FecVence : fecUltDia))
        //                        {
        //                            // Verifica que no se repita el día en la lista
        //                            if (!fechas.Contains(dia))
        //                            {
        //                                fechas.Add(dia);
        //                                respuesta.Add(new CDetallePagoGastoTrasporteDTO
        //                                {
        //                                    FecDiaPago = dia.ToShortDateString(),
        //                                    MonPago = montoDia,
        //                                    CodEntidad = temp.IdEntidad,
        //                                    TipoDetalleDTO = new CTipoDetallePagoGastoDTO
        //                                    {
        //                                        IdEntidad = 3,
        //                                        DescripcionTipo = "Permiso con / sin goce salarial"
        //                                    }
        //                                });
        //                            }
        //                        }
        //                    }
        //                };
        //            }
        //        }

        //        //
        //        // Buscar Formulario de Deducción.
        //        CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
        //        var datosG = intermedio.ObtenerGastoTransporteActual(funcionario.Cedula);
        //        if (datosG.Codigo > 0)
        //        {
        //            foreach (var item in ((GastoTransporte)datosG.Contenido).MovimientoGastoTransporte.Where(Q => Q.FecMovimiento.Month == mes && Q.FecMovimiento.Year == anio).ToList())
        //            {
        //                foreach (var dato in item.DetalleDeduccionGastoTransporte)
        //                {
        //                    DateTime dia = Convert.ToDateTime(dato.FecRige);

        //                    //
        //                    while (dia <= Convert.ToDateTime(dato.FecVence))
        //                    {
        //                        // Verifica que no se repita el día en la lista
        //                        if (!fechas.Contains(dia))
        //                        {
        //                            fechas.Add(dia);
        //                            respuesta.Add(new CDetallePagoGastoTrasporteDTO
        //                            {
        //                                FecDiaPago = dia.ToShortDateString(),
        //                                MonPago = montoDia,
        //                                CodEntidad = dato.PK_DetalleDeduccionGastoTransporte,
        //                                TipoDetalleDTO = new CTipoDetallePagoGastoDTO
        //                                {
        //                                    IdEntidad = 4,
        //                                    DescripcionTipo = dato.DesMotivo
        //                                }
        //                            });
        //                        }
        //                        dia = dia.AddDays(1);
        //                    }
        //                }
        //            }
        //        }


        //        //respuesta.Add(new CRespuestaDTO
        //        //{
        //        //    Codigo = 1,
        //        //    Contenido = detalles
        //        //});
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta.Add(new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = error.Message
        //        });
        //    }

        //    return respuesta;
        //}

        public List<CBaseDTO> ObtenerDiasRebajarGT(CFuncionarioDTO funcionario, CGastoTransporteDTO gasto, int mes, int anio, decimal montoDia)
        {
            CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
            CViaticoCorridoD intermedioV = new CViaticoCorridoD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            List<DateTime> fechas = new List<DateTime>();
            List<DateTime> fechaInicio = new List<DateTime>();
            List<DateTime> fechaFin = new List<DateTime>();

            List<DateTime> listaDiasRebajados = new List<DateTime>();
            List<DateTime> listaDiasReintegrados = new List<DateTime>();

            GastoTransporte datoGasto = new GastoTransporte();
            int maxDias = 21; //22;

            try
            {
                // Buscar Datos de Gasto
                var resultado = intermedio.ObtenerGastoTransporte(gasto.IdEntidad.ToString());
                if (resultado.Codigo > 0)
                    datoGasto = (GastoTransporte)resultado.Contenido;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);


                foreach (var pago in datoGasto.PagoGastoTransporte.Where(Q => Q.IndEstado == 1 && (Q.NumBoleta != "" || Q.ReservaRecurso != "S/R")).ToList())
                {
                    foreach (var item in pago.DetallePagoGastoTransporte) 
                        if (item.FK_TipoDetallePagoGasto == 5) // Tipo 5 - Reintegro de Días
                            listaDiasReintegrados.Add(item.FecDiaPago);
                        else
                            listaDiasRebajados.Add(item.FecDiaPago);
                }

                var fecPrimerDia = new DateTime(anio, mes, 1);
                var fecUltDia = fecPrimerDia.AddMonths(1).AddDays(-1);

                if (Convert.ToDateTime(datoGasto.FecFinGastosTransporte).CompareTo(fecUltDia) == -1)
                {
                    fecUltDia = Convert.ToDateTime(datoGasto.FecFinGastosTransporte);
                    maxDias = Convert.ToInt16((fecUltDia - fecPrimerDia).TotalDays) + 1;
                }

                var listaDias = Enumerable.Range(0, (fecUltDia - fecPrimerDia).Days + 1)
                                              .Select(d => fecPrimerDia.AddDays(d))
                                              .ToArray();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Incapacidades

                CRegistroIncapacidadD intermedioIncap = new CRegistroIncapacidadD(contexto);
                CRegistroIncapacidadDTO registroIncap = new CRegistroIncapacidadDTO
                {
                    EstadoIncapacidad = 2 // Aprobada
                };

                fechaInicio.Add(Convert.ToDateTime(datoGasto.FecInicGastosTransporte));
                fechaInicio.Add(fecUltDia);

                var datosIncap = intermedioIncap.BuscarRegistroIncapacidad(funcionario, registroIncap, null, fechaInicio, fechaFin);

                if (datosIncap.Codigo > 0)
                {
                    // Lista de Incapacidades
                    foreach (var item in (List<RegistroIncapacidad>)datosIncap.Contenido)
                    {
                        // La lista de los días de Incapacidad
                        foreach (var detalleI in item.DetalleIncapacidad)
                        {
                            var dia = Convert.ToDateTime(detalleI.FecDia);
                            // Buscar que no esté registrado en la tabla de Detalle Pago
                            // Verificar que el día de rebajo no sea ni Sábado ni Domingo
                            if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias 
                                && dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                            {
                                fechas.Add(dia);
                                respuesta.Add(new CDetallePagoGastoTrasporteDTO
                                {
                                    FecDiaPago = dia.ToShortDateString(),
                                    MonPago = montoDia,
                                    CodEntidad = item.PK_RegistroIncapacidad,
                                    TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                    {
                                        IdEntidad = 1,
                                        DescripcionTipo = "Incapacidad"
                                    }
                                });
                            }
                        }
                    }
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Vacaciones

                CRegistroVacacionesD intermedioVacaciones = new CRegistroVacacionesD(contexto);
                var datosVacaciones = intermedioVacaciones.ConsultaVacaciones(funcionario.Cedula);

                if (datosVacaciones.Codigo > 0)
                {
                    var listaVacaciones = ((List<RegistroVacaciones>)datosVacaciones.Contenido).Where(Q => Q.FecInicio.Value >= fechaInicio[0] && Q.FecInicio.Value <= fechaInicio[1]).OrderBy(Q => Q.FecInicio).ToList();
                    // Lista de Incapacidades
                    foreach (var item in listaVacaciones)
                    {
                        if (item.IndEstado == 1)
                        {
                            listaDias = Enumerable.Range(0, (Convert.ToDateTime(item.FecFin) - Convert.ToDateTime(item.FecInicio)).Days + 1)
                                           .Select(d => Convert.ToDateTime(item.FecInicio).AddDays(d))
                                           .ToArray();

                            foreach (var diaVacaciones in listaDias)
                            {
                                DateTime dia = Convert.ToDateTime(diaVacaciones.ToShortDateString());
                                // Verifica que no se repita el día en la lista
                                if (!fechas.Contains(dia))
                                {
                                    // Buscar que no esté registrado en la tabla de Detalle Pago
                                    // Verificar que el día de rebajo no sea ni Sábado ni Domingo
                                    if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias
                                        && dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                                    {
                                        fechas.Add(dia);
                                        respuesta.Add(new CDetallePagoGastoTrasporteDTO
                                        {
                                            FecDiaPago = dia.ToShortDateString(),
                                            MonPago = montoDia,
                                            CodEntidad = item.PK_RegistroVacaciones,
                                            TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                            {
                                                IdEntidad = 2,
                                                DescripcionTipo = "Vacaciones"
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                }


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Permisos con / sin goce salarial

                CAccionPersonalL intermedioAP = new CAccionPersonalL();
                CAccionPersonalDTO accion = new CAccionPersonalDTO();

                CEstadoBorradorDTO tipoEst = new CEstadoBorradorDTO
                {
                    IdEntidad = 7
                };
                accion.Estado = tipoEst;

                var datosAP = intermedioAP.BuscarAccion(funcionario, null, accion, fechaInicio);

                if (!datosAP.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                {
                    var listaEstados = new List<int> { 6, 7, 9, 10 };

                    // Lista de Acciones de Personal
                    foreach (var item in datosAP)
                    {
                        var temp = (CAccionPersonalDTO)item.ElementAt(0);

                        if (listaEstados.Contains(temp.TipoAccion.IdEntidad))
                        {
                            var diaVence = temp.FecVence.HasValue ? Convert.ToDateTime(temp.FecVence) : fecUltDia;
                            listaDias = Enumerable.Range(0, (diaVence - Convert.ToDateTime(temp.FecRige)).Days + 1)
                                          .Select(d => Convert.ToDateTime(temp.FecRige).AddDays(d))
                                          .ToArray();

                            foreach (var dia in listaDias)
                            {
                                // Verifica que no se repita el día en la lista
                                if (!fechas.Contains(dia))
                                {
                                    // Buscar que no esté registrado en la tabla de Detalle Pago
                                    // Verificar que el día de rebajo no sea ni Sábado ni Domingo
                                    if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias
                                        && dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                                    {
                                        fechas.Add(dia);
                                        respuesta.Add(new CDetallePagoGastoTrasporteDTO
                                        {
                                            FecDiaPago = dia.ToShortDateString(),
                                            MonPago = montoDia,
                                            CodEntidad = temp.IdEntidad,
                                            TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                            {
                                                IdEntidad = 3,
                                                DescripcionTipo = "Permiso con / sin goce salarial"
                                            }
                                        });
                                    }
                                }
                            }
                        };
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar en Catálogo de días de Rebajo / Reintegro Masivo.
                var catalogoDia = intermedioV.ListarCatalogoDia(mes, anio);
                if (catalogoDia.Codigo > 0)
                {
                    var listadoCatalogo = (List<CatDiaViaticoGasto>)catalogoDia.Contenido;
                    listadoCatalogo = listadoCatalogo.Where(Q => Q.IndRebajo && Q.IndGasto).ToList();
                    foreach (var item in listadoCatalogo)
                    {
                        DateTime dia = Convert.ToDateTime(item.FecDia);

                        // Verifica que no se repita el día en la lista
                        if (!fechas.Contains(dia))
                        {
                            // Buscar que no esté registrado en la tabla de Detalle Pago
                            if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias)
                            {
                                // Verificar que el día de rebajo no sea ni Sábado ni Domingo
                                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    fechas.Add(dia);
                                    respuesta.Add(new CDetallePagoGastoTrasporteDTO
                                    {
                                        FecDiaPago = dia.ToShortDateString(),
                                        MonPago = montoDia,
                                        CodEntidad = item.PK_CatalogoDia,
                                        TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                        {
                                            IdEntidad = 4,
                                            DescripcionTipo = item.DesDia
                                        }
                                    });
                                }
                            }
                        }
                    }
                }



                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Formulario de Deducción.

                //var datosG = intermedio.ObtenerGastoTransporteActual(funcionario.Cedula);
                if (resultado.Codigo > 0)
                {
                    foreach (var item in datoGasto.MovimientoGastoTransporte.Where(Q => Q.IndEstado == 1).ToList())
                    {
                        foreach (var dato in item.DetalleDeduccionGastoTransporte)
                        {
                            DateTime dia = Convert.ToDateTime(dato.FecRige);

                            while (dia <= Convert.ToDateTime(dato.FecVence))
                            {
                                // Verifica que no se repita el día en la lista
                                if (!fechas.Contains(dia))
                                {
                                    // Buscar que no esté registrado en la tabla de Detalle Pago
                                    if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias)
                                    {
                                        // Verificar que el día de rebajo no sea ni Sábado ni Domingo
                                        if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            fechas.Add(dia);
                                            respuesta.Add(new CDetallePagoGastoTrasporteDTO
                                            {
                                                FecDiaPago = dia.ToShortDateString(),
                                                MonPago = montoDia,
                                                CodEntidad = dato.PK_DetalleDeduccionGastoTransporte,
                                                TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                                {
                                                    IdEntidad = 4,
                                                    DescripcionTipo = dato.DesMotivo
                                                }
                                            });
                                        }
                                    }
                                }
                                dia = dia.AddDays(1);
                            }
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  REINTEGROS
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
               
                
                // Buscar los Pagos que no se le han hecho desde el inicio del Contrato.
                if (resultado.Codigo > 0)
                {
                    var fechaInicioContrato = Convert.ToDateTime(datoGasto.FecInicGastosTransporte);
                    //if (fechaInicioContrato < new DateTime(2020, 6, 1)) // Fecha que se hizo el primer pago en el SIRH
                    //    fecPrimerDia = new DateTime(2020, 6, 1);
                    //else
                    //    fecPrimerDia = new DateTime(fechaInicioContrato.Year, fechaInicioContrato.Month, 1);

                    //fecUltDia = new DateTime(anio, mes - 1, 1);

                    //var fecha = fecPrimerDia;

                    fecUltDia = new DateTime(anio, mes - 1, 1);

                    if (fechaInicioContrato.Day > 1)
                    {
                        //var finMes = new DateTime(anio, mes, 1).AddDays(-1);
                        var finMes = new DateTime(anio, fechaInicioContrato.Month + 1, 1).AddDays(-1);
                        maxDias = Convert.ToInt16((finMes - fechaInicioContrato).TotalDays) + 1;
                        var diasMesProp = 22 / Convert.ToDouble(finMes.Day);
                        maxDias = Convert.ToInt16(maxDias * diasMesProp);
                    }
                    else
                        maxDias = 22;

                    var fecha = new DateTime(fechaInicioContrato.Year, fechaInicioContrato.Month, 1);

                    while (fecha <= fecUltDia)
                    {
                        var pago = datoGasto.PagoGastoTransporte.Where(Q => Q.IndEstado == 1
                                                                     && Q.FecPago.Month == fecha.Month
                                                                     && Q.FecPago.Year == fecha.Year
                                                                     && (Q.NumBoleta != "" || Q.ReservaRecurso != "S/R"))
                                                                     .FirstOrDefault();
                        if (pago == null && !listaDiasReintegrados.Contains(fecha))  // No se existe registro de pago de ese mes
                        {
                            var strMes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                            if (maxDias > 22)
                                maxDias = 22;

                            respuesta.Add(new CDetallePagoGastoTrasporteDTO
                            {
                                IdEntidad = maxDias,
                                FecDiaPago = fecha.ToShortDateString(),
                                MonPago = Decimal.Round((montoDia * maxDias), 2), //Convert.ToDecimal(datoGasto.MontGastosTransporte),
                                CodEntidad = 0,
                                TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                {
                                    IdEntidad = 5,
                                    DescripcionTipo = "PAGO RETROACTIVO " + strMes.ToUpper()
                                }
                            });
                        }
                        fecha = fecha.AddMonths(1);
                        maxDias = 22;
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Formulario de Reintegro.
                if (resultado.Codigo > 0)
                {
                    foreach (var item in datoGasto.GastoTransporteReintegroDias.Where(Q => Q.IndEstado == 1).ToList())
                    {
                        DateTime dia = Convert.ToDateTime(item.FecDia);
                                                
                        // Verifica que no se repita el día en la lista
                        // Verifica que no se haya reintegrado ese día en un pago anterior
                        if (!fechas.Contains(dia) && !listaDiasReintegrados.Contains(dia))
                        {
                            fechas.Add(dia);
                            respuesta.Add(new CDetallePagoGastoTrasporteDTO
                            {
                                FecDiaPago = dia.ToShortDateString(),
                                MonPago = item.MonReintegro,
                                CodEntidad = 0,
                                TipoDetalleDTO = new CTipoDetallePagoGastoDTO
                                {
                                    IdEntidad = 5,
                                    DescripcionTipo = item.ObsMotivo
                                }
                            });
                        }
                    }
                }
                respuesta.OrderBy(Q => ((CDetallePagoGastoTrasporteDTO)Q).FecDiaPago).ToList();
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
        
        public List<CBaseDTO> ObtenerDiasPagar(CGastoTransporteDTO gasto, int mes, int anio)
        {
            CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            List<DateTime> fechas = new List<DateTime>();
            List<DateTime> fechaInicio = new List<DateTime>();
            List<DateTime> fechaFin = new List<DateTime>();

            GastoTransporte datoGasto = new GastoTransporte();

            int maxDias = 22;
            try
            {
                // Buscar Datos de Gasto
                var resultado = intermedio.ObtenerGastoTransporte(gasto.IdEntidad.ToString());
                if (resultado.Codigo > 0)
                    datoGasto = (GastoTransporte)resultado.Contenido;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);

                bool calcularDias = false;
                var fecPrimerDia = new DateTime(anio, mes, 1);
                var fecUltDia = fecPrimerDia.AddMonths(1).AddDays(-1);
                var diasMesProp = Convert.ToDouble(maxDias) / Convert.ToDouble(fecUltDia.Day);

                if (Convert.ToDateTime(datoGasto.FecInicGastosTransporte).CompareTo(fecPrimerDia) > 0)
                {
                    fecPrimerDia = Convert.ToDateTime(datoGasto.FecInicGastosTransporte);
                    calcularDias = true;
                }

                if (Convert.ToDateTime(datoGasto.FecFinGastosTransporte).CompareTo(fecUltDia) < 0)
                {
                    fecUltDia = Convert.ToDateTime(datoGasto.FecFinGastosTransporte);
                    calcularDias = true;
                }

                if (calcularDias)
                    maxDias = (Convert.ToInt16((((fecUltDia - fecPrimerDia).TotalDays) + 1) * diasMesProp));

                respuesta.Add(new CDetallePagoGastoTrasporteDTO
                {
                    IdEntidad = maxDias
                });
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
        /// Obtener el pago y sus detalles de acuerdo al código(PK) indicado.
        /// </summary>
        /// <param name="codigo">PrimaryKey del pago de gasto de transporte</param>
        /// <returns></returns>
        public List<List<CBaseDTO>> ObtenerPagoGastoTransporte(int codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> temp = new List<CBaseDTO>();

            try
            {
                CGastoTrasporteD intermedioGT = new CGastoTrasporteD(contexto);
                CPuestoL intermedioPuesto = new CPuestoL();

                var pagoGT = intermedioGT.ObtenerPagoGastoTransporte(codigo);
                if (pagoGT.Codigo > 0)
                {
                    //[0] PagoViaticoCorrido
                    var dato = ConstruirPagoGastoTransporteGeneral((PagoGastoTransporte)pagoGT.Contenido);
                    temp.Add(dato);

                    var gastoT = ObtenerGastosTransporte(dato.GastoTransporteDTO.IdEntidad.ToString());

                    if (gastoT.Count() > 1)
                    {
                        // [0]
                        temp.Add(gastoT[0][0]); // [1] Viático Corrido
                        temp.Add(gastoT[0][1]); // [2] Funcionario
                        temp.Add(gastoT[0][2]); // [3] Nombramiento
                        //temp.Add(gastoT[0][3]); // [4] Puesto
                        temp.Add(gastoT[0][3]); // [5] Carta Presentación
                        temp.Add(gastoT[1][1]); // [6] Detalle Puesto
                        temp.Add(gastoT[1][2]); // [7] Ubicación Puesto
                        temp.Add(gastoT[1][3]); // [8] Ubicación Puesto
                        respuesta.Add(temp);

                        // [1] Factura Desarraigo
                        //respuesta.Add(gastoT[2]);
                        //if (viaticoC[2].Count() > 0)
                        //    respuesta.Add(viaticoC[2]); 
                        //else
                        //    respuesta.Add(new CBaseDTO());

                        // [2] Contrato Arrendamiento
                        //respuesta.Add(viaticoC[3]);
                        //if (viaticoC[3].Count() > 0)
                        //    respuesta.Add(viaticoC[3][0]);
                        //else
                        //    respuesta.Add(new CBaseDTO());
                    }
                    else
                    {
                        temp.Add((CErrorDTO)gastoT[0][0]);
                        respuesta.Add(temp);
                    }
                }
                else
                {
                    temp.Add((CErrorDTO)pagoGT.Contenido);
                    respuesta.Add(temp);
                }
            }
            catch (Exception error)
            {
                temp.Clear();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                respuesta.Add(temp);
            }

            return respuesta;
        }

        //no es necesario, se puede usar el mismo buscar para este proposito y resulta casi igual en eficiencia
        public List<List<CBaseDTO>> ListarGastoTransporte()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
            List<GastoTransporte> datosGastoTransporte = new List<GastoTransporte>();
            List<string> codigos = new List<string>();

            var datos = intermedio.ListarGastoTransporte();

            if (datos.Codigo > 0)
            {
                datosGastoTransporte = (List<GastoTransporte>)(((List<object>)datos.Contenido)[0]);
                codigos = (List<string>)(((List<object>)datos.Contenido)[1]);

                for (int i = 0; i < datosGastoTransporte.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosGastoTransporte[i];
                    var gastoT = ConstruirGastoTransporteGeneral(item);
                    gastoT.CodigoGastoTransporte = codigos[i];
                    temp.Add(gastoT);
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
        
        /// <summary>
        /// Metodo encargado de obtener la lista de estados que puede tener un gasto de transporte
        /// </summary>
        /// <returns>Lista de estados</returns>
        public List<CBaseDTO> ListarEstadosGastoTransporte()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CEstadoGastoTransporteD intermedio = new CEstadoGastoTransporteD(contexto);

            var estadosGastoTransporte = intermedio.ListarEstadoGastoTransporte();

            if (estadosGastoTransporte.Codigo != -1)
            {
                foreach (var item in (List<EstadoGastoTransporte>)estadosGastoTransporte.Contenido)
                {
                    respuesta.Add(new CEstadoGastoTransporteDTO
                    {
                        IdEntidad = item.PK_EstadoGastosTransporte,
                        NomEstadoDTO = item.NomEstado
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)estadosGastoTransporte.Contenido);
            }

            return respuesta;
        }

        /// <summary>
        /// Encargado de listar los gastos de transporte tramitados en meses anteriores para el funcionario indicado
        /// </summary>
        /// <param name="cedula">Número de cédula del funcionario</param>
        /// <returns>Lista de gastos de transporte de meses anteriores</returns>
        public List<List<CBaseDTO>> ListarPagoMesesAnterioresGastoTransporte(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultadoFuncionario = intermedio.PruebaBuscarFuncionarioCedula(cedula);
                if (resultadoFuncionario.Codigo != -1)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral((Funcionario)resultadoFuncionario.Contenido) });
                    CGastoTrasporteD intermedioGastoTransporte = new CGastoTrasporteD(contexto);

                    var mesesanteriores = intermedioGastoTransporte.ListarPagoMesesAnteriores(cedula);
                    if (mesesanteriores != null)
                    {
                        List<CBaseDTO> mesesanterioresdata = new List<CBaseDTO>();
                        foreach (var item in mesesanteriores)
                        {
                            var datogasto = ConstruirGastoTransporteGeneral(item);
                            mesesanterioresdata.Add(datogasto);
                            //mesesanterioresdata.Add(new CGastoTransporteDTO
                            //{
                            //    NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                            //    MontGastoTransporteDTO = item.MontGastosTransporte,
                            //    FecFinDTO = Convert.ToDateTime(item.FecFinGastosTransporte),
                            //    FecInicioDTO = Convert.ToDateTime(item.FecInicGastosTransporte),
                            //    EstadoGastoTransporteDTO = CEstadoGastoTransporteL.ConvertirEstadoGastoTransporteDatosaDTO(item.EstadoGastoTransporte),
                            //    ObsGastoTransporteDTO = item.ObsGastosTransporte == null ? "" : item.ObsGastosTransporte,

                            //    IdEntidad = item.PK_GastosTransporte

                            //});
                        }
                        Console.WriteLine("meses anteriores:" + mesesanteriores.Count());
                        respuesta.Add(mesesanterioresdata);
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                    }

                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }

        public List<List<CBaseDTO>> ListarGastoTransportePago(int mes, int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioGasto.ListarGastoTransportePago(mes, anio);

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<GastoTransporte>)resultado.Contenido)
                    {
                        List<CBaseDTO> lista = new List<CBaseDTO>();

                        // [0] GastoTransporte
                        var datoGasto = ConvertirGastoTransporteDatosaDTO(item);
                        lista.Add(datoGasto);

                        // [1] PagoGastoTransporte
                        if (item.PagoGastoTransporte.Count > 0)
                        {
                            var datoPago = item.PagoGastoTransporte
                                                    .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1)
                                                    .FirstOrDefault();
                            if (datoPago != null)
                            {
                                lista.Add(new CPagoGastoTransporteDTO
                                {
                                    IdEntidad = datoPago.PK_PagoGastoTransporte,
                                    FecPago = datoPago.FecPago,
                                    MonPago = datoPago.MonPago,
                                    HojaIndividualizada = datoPago.HojaIndividualizada,
                                    NumBoleta = datoPago.NumBoleta,
                                    ReservaRecurso = datoPago.ReservaRecurso,
                                    IndEstado = datoPago.IndEstado,
                                    FecRegistro = datoPago.FecRegistro,
                                    GastoTransporteDTO = datoGasto
                                });
                            }
                            else
                            {
                                lista.Add(new CPagoGastoTransporteDTO
                                {
                                    IdEntidad = -1,
                                    MonPago = 0,
                                    HojaIndividualizada = "",
                                    NumBoleta = "",
                                    ReservaRecurso = "S/R",
                                    GastoTransporteDTO = datoGasto
                                });
                            }
                        }
                        else
                        {
                            lista.Add(new CPagoGastoTransporteDTO());
                        }

                        // [2] Funcionario
                        lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.Nombramiento.Funcionario));

                        respuesta.Add(lista);
                    }
                }
                else
                {
                    List<CBaseDTO> lista = new List<CBaseDTO>();
                    lista.Add((CErrorDTO)resultado.Contenido);
                    respuesta.Add(lista);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }

        public List<List<CBaseDTO>> ListarGastoPagosPendientes(int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> lista;
            CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
            int mesVence = 0;
            try
            {
                var resultado = intermedioGasto.ListarGastoPagosPendientes(anio);

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<GastoTransporte>)resultado.Contenido)
                    {
                        lista = new List<CBaseDTO>();

                        if (item.PagoGastoTransporte.Count > 0)
                        {
                            mesVence = Convert.ToDateTime(item.FecFinGastosTransporte).Month;
                            var datoPago = item.PagoGastoTransporte
                                                    .Where(P => P.FecPago.Month == mesVence &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.NumBoleta == "")
                                                    .OrderByDescending(P => P.FecPago)
                                                    .FirstOrDefault();
                            if (datoPago != null)
                            {
                                // [0] GastoTransporte
                                var datoGasto = ConvertirGastoTransporteDatosaDTO(item);
                                lista.Add(datoGasto);

                                // [1] PagoGastoTransporte
                                lista.Add(new CPagoGastoTransporteDTO
                                {
                                    IdEntidad = datoPago.PK_PagoGastoTransporte,
                                    FecPago = datoPago.FecPago,
                                    MonPago = datoPago.MonPago,
                                    HojaIndividualizada = datoPago.HojaIndividualizada,
                                    NumBoleta = datoPago.NumBoleta,
                                    ReservaRecurso = datoPago.ReservaRecurso,
                                    IndEstado = datoPago.IndEstado,
                                    FecRegistro = datoPago.FecRegistro,
                                    GastoTransporteDTO = datoGasto
                                });

                                // [2] Funcionario
                                lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.Nombramiento.Funcionario));

                                respuesta.Add(lista);
                            }
                        }
                    }
                }
                else
                {
                    lista = new List<CBaseDTO>();
                    lista.Add((CErrorDTO)resultado.Contenido);
                    respuesta.Add(lista);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            if (respuesta.Count == 0)
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = "No hay pagos pendientes para el año: " + anio.ToString() } });
            return respuesta;
        }

        public List<List<CBaseDTO>> ListarPagosGastoTransporte(int mes, int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioGasto.ListarPagosGastoTransporte(mes, anio);

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<PagoGastoTransporte>)resultado.Contenido)
                    {
                        List<CBaseDTO> lista = new List<CBaseDTO>();

                        // [0] PagoViaticoCorrido
                        lista.Add(new CPagoGastoTransporteDTO
                        {
                            IdEntidad = item.PK_PagoGastoTransporte,
                            FecPago = item.FecPago,
                            MonPago = item.MonPago,
                            HojaIndividualizada = item.HojaIndividualizada,
                            NumBoleta = item.NumBoleta,
                            ReservaRecurso = item.ReservaRecurso,
                            IndEstado = item.IndEstado,
                            FecRegistro = item.FecRegistro
                        });

                        // [1] ViaticoCorrido
                        lista.Add(ConvertirGastoTransporteDatosaDTO(item.GastoTransporte));

                        // [2] Funcionario
                        lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.GastoTransporte.Nombramiento.Funcionario));

                        respuesta.Add(lista);
                    }
                }
                else
                {
                    List<CBaseDTO> lista = new List<CBaseDTO>();
                    lista.Add((CErrorDTO)resultado.Contenido);
                    respuesta.Add(lista);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }
        
        public List<List<CBaseDTO>> ListarGastos(CGastoTransporteDTO datoBusqueda)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CGastoTrasporteD intermedioGasto = new CGastoTrasporteD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioGasto.ListarGastoTransporte();
                
                if (resultado.Codigo != -1)
                {
                    var datos = (List<GastoTransporte>)(((List<object>)resultado.Contenido)[0]);
                    //datos = datos.Where(Q => Q.FecInicGastosTransporte.Value.Year == DateTime.Today.Year).ToList();

                    // Cédula
                    if (datoBusqueda.NombramientoDTO.Funcionario.Cedula != null)
                        datos = datos.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario.Contains(datoBusqueda.NombramientoDTO.Funcionario.Cedula)).ToList();

                    // Estado
                    if (datoBusqueda.EstadoGastoTransporteDTO.IdEntidad != -1)
                        datos = datos.Where(Q => Q.EstadoGastoTransporte.PK_EstadoGastosTransporte == datoBusqueda.EstadoGastoTransporteDTO.IdEntidad).ToList();
                    else
                        datos = datos.Where(Q => Q.EstadoGastoTransporte.PK_EstadoGastosTransporte != 3 ).ToList(); // Anulados

                    // Fecha Inicio
                    if (datoBusqueda.FecInicioDTO != null && datoBusqueda.FecInicioDTO.Year != 1)
                        datos = datos.Where(Q => Q.FecInicGastosTransporte >= datoBusqueda.FecInicioDTO).ToList();

                    // Fecha Vence
                    if (datoBusqueda.FecFinDTO != null && datoBusqueda.FecFinDTO.Year != 1) 
                        datos = datos.Where(Q => Q.FecFinGastosTransporte <= datoBusqueda.FecFinDTO).ToList();

                    if (datos.Count > 0)
                    {
                        foreach (var item in datos)
                        {
                            List<CBaseDTO> lista = new List<CBaseDTO>();

                            // [0] Gasto Transporte
                            lista.Add(ConstruirGastoTransporteGeneral(item));

                            // [1] Funcionario
                            lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.Nombramiento.Funcionario));

                            // [2] Expediente
                            if (((Funcionario)item.Nombramiento.Funcionario).ExpedienteFuncionario.Count() > 0)
                                lista.Add(CExpedienteL.ConvertirExpedienteADTO(((Funcionario)item.Nombramiento.Funcionario).ExpedienteFuncionario.FirstOrDefault()));
                            else
                                lista.Add(new CExpedienteFuncionarioDTO { NumeroExpediente = 0, Mensaje = "No tiene un número de Expediente registrado" });

                            //var datosFuncionario = BuscarFuncionarioCedulaGT(((Funcionario)item.Nombramiento.Funcionario).IdCedulaFuncionario);
                            //if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            //{
                            //    // [3] Puesto
                            //    lista.Add((CPuestoDTO)datosFuncionario[1]);
                            //    // [4] DetallePuesto
                            //    lista.Add((CDetallePuestoDTO)datosFuncionario[2]);
                            //    var ubicacion1 = (CUbicacionPuestoDTO)datosFuncionario[6];
                            //    var ubicacion2 = (CUbicacionPuestoDTO)datosFuncionario[7];

                            //    if (ubicacion1.TipoUbicacion.IdEntidad == 1) // Contrato
                            //    {
                            //        // [5] Ubicación Contrato
                            //        lista.Add(ubicacion1);
                            //        // [6] Ubicación Trabajo
                            //        lista.Add(ubicacion2);
                            //    }
                            //    else
                            //    {
                            //        // [5] Ubicación Contrato
                            //        lista.Add(ubicacion2);
                            //        // [6] Ubicación Trabajo
                            //        lista.Add(ubicacion1);
                            //    }
                            //}
                            //else
                            //{
                            //    throw new Exception(((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                            //}

                            respuesta.Add(lista);
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontraron datos de Gasto Transporte de acuerdo a los parámetros de búsqueda");
                    }
                }
                else
                {
                    List<CBaseDTO> lista = new List<CBaseDTO>();
                    lista.Add((CErrorDTO)resultado.Contenido);
                    respuesta.Add(lista);
                }
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }

        public CBaseDTO AsignarReservaRecurso(CPagoGastoTransporteDTO pago)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                var datos = intermedio.AsignarReservaRecurso(pago.IdEntidad, pago.ReservaRecurso);
                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = Convert.ToInt16(datos.Contenido)
                    };
                }
                else
                {
                    return (CErrorDTO)datos.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO AgregarReintegro(List<CGastoTransporteReintegroDTO> lista)
        {
            CBaseDTO respuesta = new CBaseDTO();
            List<GastoTransporteReintegroDias> listaDias = new List<GastoTransporteReintegroDias>();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                foreach (var item in lista)
                {
                    listaDias.Add(new GastoTransporteReintegroDias
                    {
                        FK_GastoTransporte = item.GastoTransporteDTO.IdEntidad,
                        FecDia = item.FecDiaDTO,
                        MonReintegro = item.MonReintegroDTO,
                        ObsMotivo = item.ObsMotivoDTO,
                        IndEstado = item.EstadoDTO
                    });
                }

                respuesta = intermedio.AgregarReintegro(listaDias);
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta = (((CErrorDTO)((CRespuestaDTO)respuesta).Contenido));
            }

            return respuesta;
        }


        /// <summary>
        /// Método encargado de modificar los atributos FecContrato y NumDocumento
        /// </summary>
        /// <param name="idgasto">PK del gasto</param>
        /// <param name="feccontrato">Fecha de contrato del gasto</param>
        /// <param name="numdocumento">Numero de documento</param>
        /// <returns></returns>
        public CBaseDTO ModificarFecContratoNumDocGT(int idgasto, DateTime feccontrato, string numdocumento)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                var datosGastoT = intermedio.ModificarFecContratoNumDocGT(idgasto, feccontrato, numdocumento);
                if (datosGastoT.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = datosGastoT.IdEntidad,
                        Mensaje = "Fecha de contrato y/o número de documento modificados con éxito"
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoT.Contenido;
                }

            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ModificarPagoReservaRecursoGT(int idPago, string reserva)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);

                var datosGastoT = intermedio.ModificarPagoReservaRecursoGT(idPago, reserva);
                if (datosGastoT.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = datosGastoT.IdEntidad,
                        Mensaje = "Reserva Recurso asignada con éxito"
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosGastoT.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ActualizarRutaTarifa(List<CGastoTransporteRutasDTO> rutas)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
                CEstadoGastoTransporteD intermedioEstado = new CEstadoGastoTransporteD(contexto);

                foreach (var item in rutas)
                {
                    //Crear la Ruta
                    GastoTransporteRutas rutaBD = new GastoTransporteRutas
                    {
                        PK_Ruta = item.IdEntidad,
                        FK_GastoTransporte = item.Gasto.IdEntidad,
                        FecRige = item.FecRige,
                        FecVence = item.FecVence,
                        EstadoGastoTransporte = (EstadoGastoTransporte)intermedioEstado.BuscarEstadoGastoTransporteId(1).Contenido,// Estado Válido
                        MonGasto = item.Detalle.Sum(Q => Q.MontTarifa) * 2,
                        DetalleAsignacionGastoTransporte = new List<DetalleAsignacionGastoTransporte>()
                    };

                    foreach (var detalle in item.Detalle)
                    {
                        rutaBD.DetalleAsignacionGastoTransporte.Add(new DetalleAsignacionGastoTransporte
                        {
                            FK_Ruta = 0,
                            MontTarifa = detalle.MontTarifa,
                            NomFraccionamiento = detalle.NomFraccionamientoDTO,
                            NomRuta = detalle.NomRutaDTO,
                            NumGaceta = detalle.NumGaceta
                        });
                    }

                    var datosGastoT = intermedio.ActualizarRutaTarifa(rutaBD);

                    if (datosGastoT.Codigo > 0)
                    {
                        respuesta = new CBaseDTO
                        {
                            IdEntidad = datosGastoT.IdEntidad,
                            Mensaje = "Actualización de Tarifas con éxito"
                        };
                    }
                    else
                    {
                        respuesta = (CErrorDTO)datosGastoT.Contenido;
                    }
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }
        /// <summary>
        /// Metodo encargado de obtener el monto retro activo
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        //public CBaseDTO ObtenerMontoRetroactivoGastoTransporte(CCartaPresentacionDTO carta, List<DateTime> fecha)
        //{
        //    CBaseDTO respuesta = new CBaseDTO();
        //    try
        //    {
        //        CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
        //        var cartaDB = new CartasPresentacion { NumCarta = carta.NumeroCarta };
        //        var datos = intermedio.ObtenerMontoRetroactivo(cartaDB, fecha);
        //        if (datos.Codigo > 0)
        //        {
        //            respuesta = new CBaseDTO
        //            {
        //                Mensaje = datos.Contenido.ToString()
        //            };
        //        }
        //        else
        //        {
        //            respuesta = (CErrorDTO)datos.Contenido;
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
        //    }
        //    return respuesta;
        //}
        /// <summary>
        /// encargado de obtener un GastoTransporte en especifico 
        /// [SE USA MÁS ObtenerGastosTransporte]
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        //public List<List<CBaseDTO>> ObtenerGastoTransporte(string codigo)
        //{
        //    List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
        //    try
        //    {
        //        CGastoTrasporteD intermedio = new CGastoTrasporteD(contexto);
        //        CPuestoL intermedioPuesto = new CPuestoL();

        //        var gastoT = intermedio.ObtenerGastoTransporte(codigo);
        //        List<CBaseDTO> temp = new List<CBaseDTO>();

        //        if (gastoT.Codigo > 0)
        //        {
        //            var datogastoTranspore = ConstruirGastoTransporteGeneral((GastoTransporte)gastoT.Contenido);
        //            datogastoTranspore.CodigoGastoTransporte = gastoT.Mensaje;
        //            temp.Add(datogastoTranspore); //0 - 0

        //            var funcionario = ((GastoTransporte)gastoT.Contenido).Nombramiento.Funcionario;
        //            var datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario);

        //            temp.Add(datoFuncionario);//0 - 1

        //            var nombramiento = ((GastoTransporte)gastoT.Contenido).Nombramiento;
        //            var datosNombramiento = CNombramientoL.NombramientoGeneral(nombramiento);

        //            temp.Add(datosNombramiento);//0 - 2

        //            //Esta comentado esto en el otro
        //            var puesto = ((GastoTransporte)gastoT.Contenido).Nombramiento.Puesto;
        //            var datosPuesto = CPuestoL.ConstruirPuesto(puesto, new CPuestoDTO());
        //            temp.Add(datosPuesto); //0 - 3

        //            //Debe actualizarse esta parte
        //            var cartaPresentacion = ((GastoTransporte)gastoT.Contenido).Nombramiento.CartasPresentacion.FirstOrDefault();
        //            var datosCarta = CCartaPresentacionL.ConstruirCartaPresentacion(cartaPresentacion);
        //            temp.Add(datosCarta); // 0 - 3

        //            respuesta.Add(temp);

        //            var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(funcionario.IdCedulaFuncionario);

        //            respuesta.Add(detallePuesto); // 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]

        //            //var facturas = ((GastoTransporte)gastoT.Contenido).FacturaDesarraigo.ToList();
        //           // var datosFacturas = CFacturaDesarraigoL.ConstruirFacturaGastoTransporte(facturas);

        //            //respuesta.Add(datosFacturas); // 2

        //           // var contratos = ((GastoTransporte)gastoT.Contenido).ContratoArrendamiento.ToList();
        //            //var datosContratos = CContratoArrendamientoL.ConstruirContratoArrendamientoGastoTransporte(contratos);

        //           // respuesta.Add(datosContratos); // 3

        //        }
        //        else
        //        {
        //            temp.Add((CErrorDTO)gastoT.Contenido);
        //            respuesta.Add(temp);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        List<CBaseDTO> temp = new List<CBaseDTO>();
        //        temp.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
        //        respuesta.Add(temp);
        //    }
        //    return respuesta;
        //}

        #endregion
    }
}

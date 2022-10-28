using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Logica
{
    public class CViaticoCorridoL
    {
        # region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CViaticoCorridoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo Encargado de convertir un viatico corrido DTO a Datos
        /// </summary>
        /// <param name="item"> tipo: Viatico Corrido Datos</param>
        /// <returns></returns>
        internal static ViaticoCorrido ConvertirViaticoCorridoDTOaDatos(CViaticoCorridoDTO item)
        {
            return new ViaticoCorrido
            {
                Nombramiento = CNombramientoL.ConvertirDatosNombramientoADATOS(item.NombramientoDTO),
                MontViaticoCorrido = item.MontViaticoCorridoDTO,
                FecFinViaticoCorrido = Convert.ToDateTime(item.FecFinDTO),
                FecInicViaticoCorrido = Convert.ToDateTime(item.FecInicioDTO),
                ObsViaticoCorrido = item.ObsViaticoCorridoDTO == null ? "" : item.ObsViaticoCorridoDTO,
                EstadoViaticoCorrido = CEstadoViaticoCorridoL.ConvertirEstadoViaticoCorridoDatosaDTO(item.EstadoViaticoCorridoDTO),
                DesSeñas = item.DesSenasDTO,
                NumTelDomicilio = item.NumTelDomicilioDTO,
                Distrito = CDistritoL.ConvertirDistritoDatosaDTO(item.NomDistritoDTO),
                ObsLugarPernocte = item.PernocteDTO,
                ObsLugarHospedaje = item.HospedajeDTO,
                FecContrato = item.FecContratoDTO,
                FecRegistro = item.FecRegistroDTO,
                NumDocumento = item.NumDocumentoDTO,
                Presupuesto = new Presupuesto
                {
                    IdPresupuesto = item.PresupuestoDTO.IdEntidad.ToString()
                },
                ImgDocumento = item.DocAdjunto,
                IndCabinas = item.IndCabinas
                //HojaIndividualizada = item.HojaIndividualizadaDTO
            };
        }

        /// <summary>
        ///  Metodo Encargado de convertir un viatico corrido Datos a DTO
        /// </summary>
        /// <param name="item">tipo: Viatico Corrido DTO</param>
        /// <returns></returns>
        internal static CViaticoCorridoDTO ConvertirViaticoCorridoDatosaDTO(ViaticoCorrido item)
        {
            return new CViaticoCorridoDTO
            {
                NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                PresupuestoDTO = CPresupuestoL.ConvertirPresupuestoDatosaDTO(item.Presupuesto),
                MontViaticoCorridoDTO = item.MontViaticoCorrido,
                FecFinDTO = Convert.ToDateTime(item.FecFinViaticoCorrido),
                FecInicioDTO = Convert.ToDateTime(item.FecInicViaticoCorrido),
                EstadoViaticoCorridoDTO = CEstadoViaticoCorridoL.ConvertirEstadoViaticoCorridoDTOaDatos(item.EstadoViaticoCorrido),
                ObsViaticoCorridoDTO = item.ObsViaticoCorrido == null ? "" : item.ObsViaticoCorrido,
                DesSenasDTO = item.DesSeñas,
                NumTelDomicilioDTO = item.NumTelDomicilio,
                NomDistritoDTO = CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito),
                PernocteDTO = item.ObsLugarPernocte,
                HospedajeDTO = item.ObsLugarHospedaje,
                FecContratoDTO = Convert.ToDateTime(item.FecContrato),
                FecRegistroDTO = Convert.ToDateTime(item.FecRegistro),
                NumDocumentoDTO = item.NumDocumento,
                //HojaIndividualizadaDTO = item.PagoViaticoCorrido.FirstOrDefault().HojaIndividualizada,
                IdEntidad = item.PK_ViaticoCorrido,
                IndCabinas = Convert.ToInt16(item.IndCabinas),
                DocAdjunto = item.ImgDocumento
            };
        }

        /// <summary>
        /// metodo encargado de contruir un viatico corrido general
        /// </summary>
        /// <param name="item">tipo: Viatico Corrido Datos</param>
        /// <returns></returns>
        internal static CViaticoCorridoDTO ConstruirViaticoCorridoGeneral(ViaticoCorrido item)
        {
            List<CPagoViaticoCorridoDTO> lista = new List<CPagoViaticoCorridoDTO>();
            List<CMovimientoViaticoCorridoDTO> listaMovimiento = new List<CMovimientoViaticoCorridoDTO>();
            List<CViaticoCorridoReintegroDTO> listaReintegro = new List<CViaticoCorridoReintegroDTO>();

            foreach (var pago in item.PagoViaticoCorrido.OrderBy(Q => Q.FecPago))
            {
                lista.Add(ConstruirPagoViaticoCorridoGeneral(pago));
            }

            foreach (var mov in item.MovimientoViaticoCorrido.OrderBy(Q => Q.FecMovimiento))
            {
                listaMovimiento.Add(CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO(mov));
            }

            foreach (var reintegro in item.ViaticoCorridoReintegroDias.OrderBy(Q => Q.FecDia))
            {
                listaReintegro.Add(new CViaticoCorridoReintegroDTO
                {
                    IdEntidad = reintegro.PK_Reintegro,
                    FecDiaDTO = reintegro.FecDia,
                    MonReintegroDTO = reintegro.MonReintegro,
                    ObsMotivoDTO = reintegro.ObsMotivo,
                    EstadoDTO = reintegro.IndEstado
                });
            }


            return new CViaticoCorridoDTO
            {
                NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                PresupuestoDTO = CPresupuestoL.ConvertirPresupuestoDatosaDTO(item.Presupuesto),
                MontViaticoCorridoDTO = item.MontViaticoCorrido,
                FecFinDTO = Convert.ToDateTime(item.FecFinViaticoCorrido),
                FecInicioDTO = Convert.ToDateTime(item.FecInicViaticoCorrido),
                ObsViaticoCorridoDTO = item.ObsViaticoCorrido == null ? "" : item.ObsViaticoCorrido,
                DesSenasDTO = item.DesSeñas,
                NomDistritoDTO = CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito),
                PernocteDTO = item.ObsLugarPernocte,
                HospedajeDTO = item.ObsLugarHospedaje,
                //HojaIndividualizadaDTO = item.PagoViaticoCorrido.FirstOrDefault().HojaIndividualizada,
                IdEntidad = item.PK_ViaticoCorrido,
                EstadoViaticoCorridoDTO = ConstruirEstadoViaticoCorrido(item.EstadoViaticoCorrido),
                FecContratoDTO = Convert.ToDateTime(item.FecContrato),
                FecRegistroDTO = Convert.ToDateTime(item.FecRegistro),
                NumDocumentoDTO  = item.NumDocumento,
                IndCabinas=  Convert.ToInt32(item.IndCabinas),
                DocAdjunto = item.ImgDocumento,
                Pagos = lista,
                Movimientos = listaMovimiento,
                Reintegros = listaReintegro
            };
        }

        internal static CPagoViaticoCorridoDTO ConstruirPagoViaticoCorridoGeneral(PagoViaticoCorrido item)
        {
            List<CDetallePagoViaticoCorridoDTO> lista = new List<CDetallePagoViaticoCorridoDTO>();
            List<CPagoViaticoRetroactivoDTO> listaPagosRetroactivos = new List<CPagoViaticoRetroactivoDTO>();
            List<CDetallePagoViaticoCorridoDTO> listaDetalleRetroactivos = new List<CDetallePagoViaticoCorridoDTO>();
            CViaticoCorridoD intermedio = new CViaticoCorridoD(new SIRHEntities());
            string desTipo = "";

            // Listado de días
            foreach (var detalle in item.DetallePagoViaticoCorrido.OrderBy(Q => Q.FecDiaPago).ToList())
            {
                desTipo = detalle.TipoDetallePagoViatico.DesTipoDetalle;
                // Si es tipo 4 Deducción, se adjunta el motivo,
                if (detalle.TipoDetallePagoViatico.PK_TipoDetallePagoViatico == 4)
                {
                    desTipo += " " + intermedio.ObtenerMotivoDetalleDeduccion(detalle.CodEntidad).Contenido;
                }
                lista.Add(new CDetallePagoViaticoCorridoDTO
                {
                    IdEntidad = detalle.PK_DetallePago,
                    FecDiaPago = detalle.FecDiaPago.ToShortDateString(),
                    MonPago = detalle.MonPago,
                    CodEntidad = detalle.CodEntidad,
                    TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                    {
                        IdEntidad = detalle.TipoDetallePagoViatico.PK_TipoDetallePagoViatico,
                        DescripcionTipo = desTipo
                    }
                });
            }

            // Lista de Pagos Retroactivos
            foreach (var pagos in item.PagoViaticoRetroactivo)
            {
                // Listado de días
                foreach (var detalle in pagos.DetallePagoViaticoRetroactivo)
                {
                    listaDetalleRetroactivos.Add(new CDetallePagoViaticoCorridoDTO
                    {
                        IdEntidad = detalle.PK_DetallePago,
                        FecDiaPago = detalle.FecDiaPago.ToShortDateString(),
                        MonPago = detalle.MonPago,
                        CodEntidad = detalle.CodEntidad,
                        TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                        {
                            IdEntidad = detalle.TipoDetallePagoViatico.PK_TipoDetallePagoViatico,
                            DescripcionTipo = detalle.TipoDetallePagoViatico.DesTipoDetalle
                        }
                    });
                }

                listaPagosRetroactivos.Add(new CPagoViaticoRetroactivoDTO
                {
                    IdEntidad = item.PK_PagoViaticoCorrido,
                    FecPago = item.FecPago,
                    MonPago = item.MonPago,
                    Detalles = listaDetalleRetroactivos
                });
            }

            return new CPagoViaticoCorridoDTO
            {
                IdEntidad = item.PK_PagoViaticoCorrido,
                FecPago = item.FecPago,
                MonPago = item.MonPago,
                HojaIndividualizada = item.HojaIndividualizada,
                NumBoleta = item.NumBoleta,
                ReservaRecurso = item.ReservaRecurso,
                IndEstado = item.IndEstado,
                FecRegistro = item.FecRegistro,
                Detalles = lista,
                PagosRetroactivos = listaPagosRetroactivos,
                ViaticoCorridoDTO = new CViaticoCorridoDTO { IdEntidad = item.FK_ViaticoCorrido}
            };
        }

        /// <summary>
        /// metodo encargado de contruir un estado viatico corrido 
        /// </summary>
        /// <param name="item">tipo: Estado Viatico Corrido Datos</param>
        /// <returns></returns>
        internal static CEstadoViaticoCorridoDTO ConstruirEstadoViaticoCorrido(EstadoViaticoCorrido item)
        {
            return new CEstadoViaticoCorridoDTO
            {
                IdEntidad = item.PK_EstadoViaticoCorrido,
                NomEstadoDTO = item.NomEstado
            };
        }

        /// <summary>
        /// Metodo encargado de guardarle una factura a un viatico corrido
        /// </summary>
        /// <param name="facturas"></param>
        /// <param name="viaticoCDB"></param>
        /// <param name="respuesta"></param>
        private void AgregarFacturas(List<CFacturaDesarraigoDTO> facturas, ViaticoCorrido viaticoCDB, CBaseDTO respuesta)
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
                agregadoFactura = intermedioFactura.AgregarFacturaViaticoCorrido(viaticoCDB, facturaDB);
                if (agregadoFactura.Codigo < 0)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoFactura).Contenido;
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// encargado de agregar contratos a un viatico Corrido
        /// </summary>
        /// <param name="contratos"></param>
        /// <param name="visticoCorridoDB"></param>
        /// <param name="respuesta"></param>
        private void AgregarContratos(List<CContratoArrendamientoDTO> contratos, ViaticoCorrido viaticoCorridoDB, CBaseDTO respuesta)
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
                    CodContrato = c.CodigoContratoArrendamiento.ToUpper()
                };
                agregadoContrato = intermedioContrato.AgregarContrArrendamientoViaticoCorrido(viaticoCorridoDB, contratoDB);
                if (agregadoContrato.Codigo < 0)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoContrato).Contenido;
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// encargado de Actualizar Vencimiento Viatico Corrido
        /// </summary>
        /// <param name="fechaVencimiento"></param>
        public List<List<CBaseDTO>> ActualizarVencimientoViaticoCorrido(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

                var viaticoC = intermedio.ActualizarVencimientoViaticoCorrido(fechaVencimiento);

                if (viaticoC.Codigo > 0)
                {

                    var datosViaticoC = (List<ViaticoCorrido>)(((List<object>)viaticoC.Contenido)[0]);
                    var codigos = (List<string>)(((List<object>)viaticoC.Contenido)[1]);

                    for (int i = 0; i < datosViaticoC.Count; i++)
                    {
                        List<CBaseDTO> ViaticosCList = new List<CBaseDTO>();
                        var desAux = CViaticoCorridoL.ConstruirViaticoCorridoGeneral(datosViaticoC[i]);
                        desAux.CodigoViaticoCorrido = codigos[i];
                        ViaticosCList.Add(desAux);
                        ViaticosCList.Add(CFuncionarioL.FuncionarioGeneral(datosViaticoC[i].Nombramiento.Funcionario));
                        respuesta.Add(ViaticosCList);
                    }
                }
                else
                {
                    List<CBaseDTO> ViaticosCList = new List<CBaseDTO>();
                    ViaticosCList.Add((CErrorDTO)viaticoC.Contenido);
                    respuesta.Add(ViaticosCList);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> ViaticosCList = new List<CBaseDTO>();
                ViaticosCList.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(ViaticosCList);
            }

            return respuesta;
        }

        /// <summary>
        /// metodo encargado de agragar un viatico corrido
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="funcionario"></param>
        /// <param name="viaticoCorrido"></param>
        /// <returns></returns>
        public CBaseDTO AgregarViaticoCorrido(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoCorrido, 
                                                List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contrato)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CViaticoCorridoD intermedioViaticoCorrido = new CViaticoCorridoD(contexto);
                CEstadoViaticoCorridoD intermedioEstado = new CEstadoViaticoCorridoD(contexto);
                CDistritoD intermedioDistrito = new CDistritoD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);

                var cartaDB = new CartasPresentacion { NumCarta = carta.NumeroCarta };
                var ViaticoCorridoDB = new ViaticoCorrido
                {
                  //  Nombramiento = CNombramientoL.ConvertirDatosNombramientoADATOSBasicos(viaticoCorrido.NombramientoDTO),
                    MontViaticoCorrido = viaticoCorrido.MontViaticoCorridoDTO,
                    FecFinViaticoCorrido = Convert.ToDateTime(viaticoCorrido.FecFinDTO),
                    FecInicViaticoCorrido = Convert.ToDateTime(viaticoCorrido.FecInicioDTO),
                    ObsViaticoCorrido = viaticoCorrido.ObsViaticoCorridoDTO == null ? "" : viaticoCorrido.ObsViaticoCorridoDTO,
                    EstadoViaticoCorrido =(EstadoViaticoCorrido) intermedioEstado.BuscarEstadoViaticoCorridoNombre(viaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO).Contenido,
                    DesSeñas = viaticoCorrido.DesSenasDTO,
                    NumTelDomicilio = viaticoCorrido.NumTelDomicilioDTO,
                    //Distrito = CDistritoL.ConvertirDistritoDatosaDTO(viaticoCorrido.NomDistritoDTO),
                    Distrito = intermedioDistrito.CargarDistritoId(viaticoCorrido.NomDistritoDTO.IdEntidad),
                    ObsLugarPernocte = viaticoCorrido.PernocteDTO,
                    ObsLugarHospedaje = viaticoCorrido.HospedajeDTO,
                    FecContrato = viaticoCorrido.FecContratoDTO,
                    FecRegistro = viaticoCorrido.FecRegistroDTO,
                    NumDocumento = viaticoCorrido.NumDocumentoDTO,
                    Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(viaticoCorrido.PresupuestoDTO.CodigoPresupuesto).Contenido,
                    IndCabinas = viaticoCorrido.IndCabinas,
                    ImgDocumento = viaticoCorrido.DocAdjunto
                    //HojaIndividualizada = viaticoCorrido.HojaIndividualizadaDTO
                };
                
                var funcionarioDB = new Funcionario { IdCedulaFuncionario = funcionario.Cedula };

                var agregadoViaticoCorrido = intermedioViaticoCorrido.AgregarViaticoCorrido(cartaDB, funcionarioDB, ViaticoCorridoDB);
                if (agregadoViaticoCorrido.Codigo > 0)
                {
                    AgregarFacturas(facturas, ViaticoCorridoDB, respuesta);
                    AgregarContratos(contrato, ViaticoCorridoDB, respuesta);
                    respuesta = agregadoViaticoCorrido;
                    return respuesta;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoViaticoCorrido).Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }

        /// <summary>
        /// encargado de realizar una busqueda de viatico corrido
        /// </summary>
        /// <param name="funcionario"></param>
        /// <param name="viaticoCorrido"></param>
        /// <param name="fechasEmision"></param>
        /// <param name="fechasVencimiento"></param>
        /// <param name="lugarContrato"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> BuscarViaticoCorrido(CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoCorrido,
                                    List<DateTime> fechasEmision, List<DateTime> fechasVencimiento, List<string> lugarContrato)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();
            List<ViaticoCorrido> datosViaticoCorrido = new List<ViaticoCorrido>();
            List<object> parametros = new List<object>();
            List<string> codigos = new List<string>();

            if (funcionario.Cedula != null)
            {
                parametros.Add("NumFuncionario");
                parametros.Add(funcionario.Cedula);
            }
            //if (viaticoCorrido.CodigoViaticoCorrido != null)
            //{
            //    parametros.Add("NumViaticoCorrido");
            //    parametros.Add(viaticoCorrido.CodigoViaticoCorrido);
            //}
            //if (viaticoCorrido.EstadoViaticoCorridoDTO != null)
            //{
            //    parametros.Add("Estado");
            //    parametros.Add(viaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO);
            //}
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
            var datos = intermedio.BuscarViaticoCorrido(parametros.ToArray());

            if (datos.Codigo > 0)
            {
                datosViaticoCorrido = (List<ViaticoCorrido>)(((List<object>)datos.Contenido)[0]);
                codigos = (List<string>)(((List<object>)datos.Contenido)[1]);

                for (int i = 0; i < datosViaticoCorrido.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosViaticoCorrido[i];
                    var desa = ConstruirViaticoCorridoGeneral(item);
                    desa.CodigoViaticoCorrido = codigos[i];
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
        /// encargado de obtener un viatico corrido en especifico
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> ObtenerViaticoCorrido(string codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                CPuestoL intermedioPuesto = new CPuestoL();

                var viaticoC = intermedio.ObtenerViaticoCorrido(codigo);
                List<CBaseDTO> temp = new List<CBaseDTO>();

                if (viaticoC.Codigo > 0)
                {
                    var datoviaticoCorrido = ConstruirViaticoCorridoGeneral((ViaticoCorrido)viaticoC.Contenido);
                    datoviaticoCorrido.CodigoViaticoCorrido = viaticoC.Mensaje;
                    temp.Add(datoviaticoCorrido); //0 - 0

                    var funcionario = ((ViaticoCorrido)viaticoC.Contenido).Nombramiento.Funcionario;
                    var datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario);

                    temp.Add(datoFuncionario); //0 - 1

                    var nombramiento = ((ViaticoCorrido)viaticoC.Contenido).Nombramiento;
                    var datosNombramiento = CNombramientoL.NombramientoGeneral(nombramiento);

                    temp.Add(datosNombramiento); //0 - 2

                    var puesto = ((ViaticoCorrido)viaticoC.Contenido).Nombramiento.Puesto;
                    var datosPuesto = CPuestoL.ConstruirPuesto(puesto, new CPuestoDTO());

                    temp.Add(datosPuesto); //0 - 3

                    CCartaPresentacionDTO datosCarta = new CCartaPresentacionDTO();
                    var cartaPresentacion = nombramiento.CartasPresentacion.FirstOrDefault();
                    if (cartaPresentacion != null)
                    {
                        datosCarta = CCartaPresentacionL.ConstruirCartaPresentacion(cartaPresentacion);
                    }
                    temp.Add(datosCarta); //0 - 4

                    respuesta.Add(temp);

                    //var detallePuesto = intermedioPuesto.DetallePuestoPorCedula(funcionario.IdCedulaFuncionario);
                    var detallePuesto = intermedioPuesto.DetallePuestoPorCodigo(datosPuesto.CodPuesto);

                    respuesta.Add(detallePuesto); // 1- [1:CPuestoDTO,CDetallePuestoDTO,CUbicacionPuestoDTO,CUbicacionPuestoDTO]

                    var facturas = ((ViaticoCorrido)viaticoC.Contenido).FacturaDesarraigo.ToList();
                    var datosFacturas = CFacturaDesarraigoL.ConstruirFacturaViaticoCorrido(facturas);

                    respuesta.Add(datosFacturas); // 2

                    var contratos = ((ViaticoCorrido)viaticoC.Contenido).ContratoArrendamiento.ToList();
                    var datosContratos = CContratoArrendamientoL.ConstruirContratoArrendamientoViaticoCorrido(contratos);

                    respuesta.Add(datosContratos); // 3
                }
                else
                {
                    temp.Add((CErrorDTO)viaticoC.Contenido);
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
        /// Encargado de obtener los días que se deben de rebajar del pago del Viático Corrido, correspondientes a un mes
        /// </summary>
        /// <returns></returns>
        //public List<CBaseDTO> ObtenerDiasRebajar(CFuncionarioDTO funcionario, int mes, int anio, decimal montoDia) 
        //{
        //    List<CBaseDTO> respuesta = new List<CBaseDTO>();
        //    List<DateTime> fechas =  new List<DateTime>();
        //    List<DateTime> fechaInicio = new List<DateTime>();
        //    List<DateTime> fechaFin = new List<DateTime>();
        //    List<CDetallePagoViaticoCorridoDTO> detalles = new List<CDetallePagoViaticoCorridoDTO>();

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
        //                        respuesta.Add(new CDetallePagoViaticoCorridoDTO
        //                        {
        //                            FecDiaPago = fecha.ToShortDateString(),
        //                            MonPago = montoDia,
        //                            CodEntidad = item.PK_RegistroIncapacidad,
        //                            TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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

        //        var datosAP = intermedioAP.BuscarAccion(funcionario,null, accion, fechaInicio);

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
        //                            if(!fechas.Contains(dia))
        //                            {
        //                                fechas.Add(dia);
        //                                respuesta.Add(new CDetallePagoViaticoCorridoDTO
        //                                {
        //                                    FecDiaPago = dia.ToShortDateString(),
        //                                    MonPago = montoDia,
        //                                    CodEntidad = temp.IdEntidad,
        //                                    TipoDetalleDTO = new CTipoDetallePagoViaticoDTO {
        //                                        IdEntidad = 3,
        //                                        DescripcionTipo = "Permiso con / sin goce salarial"}
        //                                });
        //                            }
        //                        }
        //                    }
        //                };
        //            }
        //        }

        //        //
        //        // Buscar Formulario de Deducción.
        //        CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
        //        var datosV = intermedio.ObtenerViaticoCorridoActual(funcionario.Cedula);
        //        if (datosV.Codigo > 0)
        //        {
        //            foreach(var item in ((ViaticoCorrido)datosV.Contenido).MovimientoViaticoCorrido.Where(Q => Q.FecMovimiento.Month == mes && Q.FecMovimiento.Year == anio).ToList())
        //            {
        //                foreach(var dato in item.DetalleDeduccionViaticoCorrido)
        //                {
        //                    DateTime dia = Convert.ToDateTime(dato.FecRige);

        //                    //
        //                    while(dia <= Convert.ToDateTime(dato.FecVence))
        //                    {
        //                        // Verifica que no se repita el día en la lista
        //                        if (!fechas.Contains(dia))
        //                        {
        //                            fechas.Add(dia);
        //                            respuesta.Add(new CDetallePagoViaticoCorridoDTO
        //                            {
        //                                FecDiaPago = dia.ToShortDateString(),
        //                                MonPago = montoDia,
        //                                CodEntidad = dato.PK_DetalleDeduccionViaticoCorrido,
        //                                TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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

        public List<CBaseDTO> ObtenerDiasRebajar(CFuncionarioDTO funcionario, CViaticoCorridoDTO viatico, int mes, int anio, decimal montoDia) //int mes, int anio,
        {
            CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            List<DateTime> fechas = new List<DateTime>();
            List<DateTime> fechaInicio = new List<DateTime>();
            List<DateTime> fechaFin = new List<DateTime>();

            List<DateTime> listaDiasRebajados = new List<DateTime>();
            List<DateTime> listaDiasReintegrados = new List<DateTime>();

            ViaticoCorrido datoViatico = new ViaticoCorrido();
            int maxDias = 25; // 26;

            try
            {
                // Buscar Datos de Viático
                var resultado = intermedio.ObtenerViaticoCorrido(viatico.IdEntidad.ToString());
                if(resultado.Codigo > 0)
                    datoViatico = (ViaticoCorrido)resultado.Contenido;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);


                foreach (var pago in datoViatico.PagoViaticoCorrido.Where(Q => Q.IndEstado == 1 && (Q.NumBoleta != "" || Q.ReservaRecurso != "S/R")).ToList())
                {
                    // Listado de días del Pago
                    foreach (var item in pago.DetallePagoViaticoCorrido)
                        if (item.FK_TipoDetallePagoViatico == 5) // Tipo 5 - Reintegro de Días
                            listaDiasReintegrados.Add(item.FecDiaPago);
                        else
                            listaDiasRebajados.Add(item.FecDiaPago);

                    // Listado de Pagos Retroactivos
                    foreach (var retroactivo in pago.PagoViaticoRetroactivo)
                        // Listado de días del Pago
                        foreach (var item in retroactivo.DetallePagoViaticoRetroactivo)
                            if (item.FK_TipoDetallePagoViatico == 5) // Tipo 5 - Reintegro de Días
                                listaDiasReintegrados.Add(item.FecDiaPago);
                            else
                                listaDiasRebajados.Add(item.FecDiaPago);
                }

                var fecPrimerDia = new DateTime(anio, mes, 1);
                var fecUltDia = fecPrimerDia.AddMonths(1).AddDays(-1);

                if (Convert.ToDateTime(datoViatico.FecFinViaticoCorrido).CompareTo(fecUltDia) == -1 )
                {
                    fecUltDia = Convert.ToDateTime(datoViatico.FecFinViaticoCorrido);
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

                fechaInicio.Add(Convert.ToDateTime(datoViatico.FecInicViaticoCorrido));
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
                            // Verificar que el día de rebajo no sea Domingo
                            if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias
                                && dia.DayOfWeek != DayOfWeek.Sunday)
                            {
                                fechas.Add(dia);
                                respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                {
                                    IdEntidad = maxDias,
                                    FecDiaPago = dia.ToShortDateString(),
                                    MonPago = montoDia,
                                    CodEntidad = item.PK_RegistroIncapacidad,
                                    TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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
                    //var listaVacaciones = ((List<RegistroVacaciones>)datosVacaciones.Contenido).Where(Q => Q.FecInicio.Value >= fechaInicio[0] && Q.FecInicio.Value <= fechaInicio[1] ).OrderBy(Q=> Q.FecInicio).ToList();
                    var listaVacaciones = ((List<RegistroVacaciones>)datosVacaciones.Contenido).Where(Q => Q.FecFin.Value <= fechaInicio[1] && Q.FecFin.Value.Year == fechaInicio[0].Year).OrderBy(Q => Q.FecInicio).ToList();
                    // Lista de Vacaciones
                    foreach (var item in listaVacaciones)
                    {
                        if(item.IndEstado == 1)
                        {
                            var diaInicioVacaciones = item.FecInicio;
                            //Si el día de inicio de las vacaciones fué en una fecha antes del inicio del contrato, se toma la fecha del Inicio de Contrato
                            if (item.FecInicio <= fechaInicio[0])
                                diaInicioVacaciones = fechaInicio[0];

                            if (item.FecFin >= diaInicioVacaciones)
                            {
                                listaDias = Enumerable.Range(0, (Convert.ToDateTime(item.FecFin) - Convert.ToDateTime(diaInicioVacaciones)).Days + 1)
                                         .Select(d => Convert.ToDateTime(diaInicioVacaciones).AddDays(d))
                                         .ToArray();

                                foreach (var diaVacaciones in listaDias)
                                {
                                    DateTime dia = Convert.ToDateTime(diaVacaciones.ToShortDateString());
                                    // Verifica que no se repita el día en la lista
                                    if (!fechas.Contains(dia))
                                    {
                                        // Buscar que no esté registrado en la tabla de Detalle Pago
                                        // Verificar que el día de rebajo no sea Domingo
                                        if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias
                                            && dia.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            fechas.Add(dia);
                                            respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                            {
                                                IdEntidad = maxDias,
                                                FecDiaPago = dia.ToShortDateString(),
                                                MonPago = montoDia,
                                                CodEntidad = item.PK_RegistroVacaciones,
                                                TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                                                {
                                                    IdEntidad = 2,
                                                    DescripcionTipo = "Vacaciones"
                                                }
                                            });

                                            //Si el día de Vacaciones es Viernes, también se rebaja el Sábado
                                            if (dia.DayOfWeek == DayOfWeek.Friday)
                                            {
                                                var diaSabado = dia.AddDays(1);

                                                if (diaSabado <= fecUltDia && fechas.Count < maxDias)
                                                {
                                                    fechas.Add(diaSabado);
                                                    respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                                    {
                                                        FecDiaPago = diaSabado.ToShortDateString(),
                                                        MonPago = montoDia,
                                                        CodEntidad = item.PK_RegistroVacaciones,
                                                        TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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
                                    // Verificar que el día de rebajo no sea Domingo
                                    if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias
                                        && dia.DayOfWeek != DayOfWeek.Sunday)
                                    {
                                        fechas.Add(dia);
                                        respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                        {
                                            IdEntidad = maxDias,
                                            FecDiaPago = dia.ToShortDateString(),
                                            MonPago = montoDia,
                                            CodEntidad = temp.IdEntidad,
                                            TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                                            {
                                                IdEntidad = 3,
                                                DescripcionTipo = "Permiso con / sin goce salarial"
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar en Catálogo de días de Rebajo / Reintegro Masivo.
                var catalogoDia = intermedio.ListarCatalogoDia(mes, anio);
                if (catalogoDia.Codigo > 0)
                {
                    var listadoCatalogo = (List<CatDiaViaticoGasto>)catalogoDia.Contenido;
                    listadoCatalogo = listadoCatalogo.Where(Q => Q.IndRebajo && Q.IndViatico).ToList();
                    foreach (var item in listadoCatalogo)
                    {
                        DateTime dia = Convert.ToDateTime(item.FecDia);

                        // Verifica que no se repita el día en la lista
                        if (!fechas.Contains(dia))
                        {
                            // Buscar que no esté registrado en la tabla de Detalle Pago
                            if (listaDiasRebajados.Contains(dia) == false && dia <= fecUltDia && fechas.Count < maxDias)
                            {
                                // Verificar que el día de rebajo no sea Domingo
                                if (dia.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    fechas.Add(dia);
                                    respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                    {
                                        FecDiaPago = dia.ToShortDateString(),
                                        MonPago = montoDia,
                                        CodEntidad = item.PK_CatalogoDia,
                                        TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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

                if (resultado.Codigo > 0)
                {
                    foreach (var item in datoViatico.MovimientoViaticoCorrido.Where(Q => Q.IndEstado == 1).ToList())
                    {
                        foreach (var dato in item.DetalleDeduccionViaticoCorrido)
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
                                        if (dia.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            fechas.Add(dia);
                                            respuesta.Add(new CDetallePagoViaticoCorridoDTO
                                            {
                                                IdEntidad = maxDias,
                                                FecDiaPago = dia.ToShortDateString(),
                                                MonPago = montoDia,
                                                CodEntidad = dato.PK_DetalleDeduccionViaticoCorrido,
                                                TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
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
                // Buscar los Pagos que no se le han hecho desde el inicio del Contrato.
                if (resultado.Codigo > 0)
                {
                    var fechaInicioContrato = Convert.ToDateTime(datoViatico.FecInicViaticoCorrido);
                    //if (fechaInicioContrato < new DateTime(2020, 6, 1)) // Fecha que se hizo el primer pago en el SIRH
                    //    fecPrimerDia = new DateTime(2020, 6, 1);
                    //else
                    //    fecPrimerDia = fechaInicioContrato;

                    fecUltDia = new DateTime(anio, mes - 1, 1);

                    if (fechaInicioContrato.Day > 1)
                    {
                        //var finMes = new DateTime(anio, mes, 1).AddDays(-1);
                        var finMes = new DateTime(anio, fechaInicioContrato.Month + 1, 1).AddDays(-1);
                        maxDias = Convert.ToInt16((finMes - fechaInicioContrato).TotalDays) + 1;
                        var diasMesProp = 26 / Convert.ToDouble(finMes.Day);
                        maxDias = Convert.ToInt16(maxDias * diasMesProp);
                    }
                    else
                        maxDias = 26;

                    var fecha = new DateTime(fechaInicioContrato.Year, fechaInicioContrato.Month, 1);
                    while (fecha <= fecUltDia)
                    {
                        var pago = datoViatico.PagoViaticoCorrido.Where(Q => Q.IndEstado == 1
                                                                     && Q.FecPago.Month == fecha.Month
                                                                     && Q.FecPago.Year == fecha.Year
                                                                     && (Q.NumBoleta != "" || Q.ReservaRecurso != "S/R"))
                                                                     .FirstOrDefault();
                        if (pago == null && !listaDiasReintegrados.Contains(fecha))  // No se existe registro de pago de ese mes
                        {
                            var strMes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();

                            if (maxDias > 26)
                                maxDias = 26;

                            respuesta.Add(new CDetallePagoViaticoCorridoDTO
                            {
                                IdEntidad = maxDias,
                                FecDiaPago = fecha.ToShortDateString(),
                                MonPago = Decimal.Round((montoDia * maxDias), 2), // datoViatico.MontViaticoCorrido,
                                CodEntidad = 0,
                                TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                                {
                                    IdEntidad = 5,
                                    DescripcionTipo = "PAGO RETROACTIVO " + strMes.ToUpper()
                                }
                            });
                        }
                        fecha = fecha.AddMonths(1);
                        maxDias = 26;
                    }
                }
              
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Buscar Formulario de Reintegro.
                if (resultado.Codigo > 0)
                {
                    foreach (var item in datoViatico.ViaticoCorridoReintegroDias.Where(Q => Q.IndEstado == 1).ToList())
                    {
                        DateTime dia = Convert.ToDateTime(item.FecDia);

                        // Verifica que no se repita el día en la lista
                        // Verifica que no se haya reintegrado ese día en un pago anterior
                        if (!fechas.Contains(dia) && !listaDiasReintegrados.Contains(dia))
                        {
                            fechas.Add(dia);
                            respuesta.Add(new CDetallePagoViaticoCorridoDTO
                            {
                                IdEntidad = maxDias,
                                FecDiaPago = dia.ToShortDateString(),
                                MonPago = item.MonReintegro,
                                CodEntidad = 0,
                                TipoDetalleDTO = new CTipoDetallePagoViaticoDTO
                                {
                                    IdEntidad = 5,
                                    DescripcionTipo = item.ObsMotivo
                                }
                            });
                        }
                    }
                }

                respuesta = respuesta.OrderBy(Q => DateTime.Parse(((CDetallePagoViaticoCorridoDTO)Q).FecDiaPago)).ToList();
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

        public List<CBaseDTO> ObtenerDiasPagar(CViaticoCorridoDTO viatico, int mes, int anio) 
        {
            CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            List<DateTime> fechas = new List<DateTime>();
            List<DateTime> fechaInicio = new List<DateTime>();
            List<DateTime> fechaFin = new List<DateTime>();
                        
            ViaticoCorrido datoViatico = new ViaticoCorrido();

            int maxDias = 26;
            try
            {
                // Buscar Datos de Viático
                var resultado = intermedio.ObtenerViaticoCorrido(viatico.IdEntidad.ToString());
                if (resultado.Codigo > 0)
                    datoViatico = (ViaticoCorrido)resultado.Contenido;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);

                bool calcularDias = false;
                var fecPrimerDia = new DateTime(anio, mes, 1);
                var fecUltDia = fecPrimerDia.AddMonths(1).AddDays(-1);
                var diasMesProp = Convert.ToDouble(maxDias) / Convert.ToDouble(fecUltDia.Day);

                if (Convert.ToDateTime(datoViatico.FecInicViaticoCorrido).CompareTo(fecPrimerDia) > 0)
                {
                    fecPrimerDia = Convert.ToDateTime(datoViatico.FecInicViaticoCorrido);
                    calcularDias = true;
                }

                if (Convert.ToDateTime(datoViatico.FecFinViaticoCorrido).CompareTo(fecUltDia) < 0)
                {
                    fecUltDia = Convert.ToDateTime(datoViatico.FecFinViaticoCorrido);
                    calcularDias = true;    
                }

                if(calcularDias)
                    maxDias = (Convert.ToInt16( (((fecUltDia - fecPrimerDia).TotalDays) + 1) * diasMesProp));

                respuesta.Add(new CDetallePagoViaticoCorridoDTO
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
        /// busca un funcionario por cedula
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<CBaseDTO> BuscarFuncionarioCedula(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CPuestoL intermedioPuesto = new CPuestoL();
            CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
            try
            {
                //var x = (new CAccionPersonalL()).BuscarFuncionarioDetallePuesto(cedula);
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
                            respuesta.RemoveAt(4); // Elimina los elementos de más
                            respuesta.Add(detallePuesto.ElementAtOrDefault(2));
                            respuesta.Add(detallePuesto.ElementAtOrDefault(3));
                            respuesta.Add(CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento));
                        }
                        else
                        {
                            throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicación del Contrato");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentran resultados para la Ubicación del puesto y la Ubicación del Contrato");
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

        //no es necesario, se puede usar el mismo buscar para este proposito y resulta casi igual en eficiencia
        public List<List<CBaseDTO>> ListarViaticoCorrido()
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
            List<ViaticoCorrido> datosViaticoCorrido = new List<ViaticoCorrido>();
            List<string> codigos = new List<string>();

            var datos = intermedio.ListarViaticoCorrido();

            if (datos.Codigo > 0)
            {
                datosViaticoCorrido = (List<ViaticoCorrido>)(((List<object>)datos.Contenido)[0]);
                codigos = (List<string>)(((List<object>)datos.Contenido)[1]);

                for (int i = 0; i < datosViaticoCorrido.Count; i++)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    var item = datosViaticoCorrido[i];
                    var viaticoC = ConstruirViaticoCorridoGeneral(item);
                    viaticoC.CodigoViaticoCorrido = codigos[i];
                    temp.Add(viaticoC);
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
        /// Metodo encargado de listar estados viatico corrido
        /// </summary>
        /// <returns></returns>
        public List<CBaseDTO> ListarEstadosViaticoCorrido()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CEstadoViaticoCorridoD intermedio = new CEstadoViaticoCorridoD(contexto);

            var estadosViaticoCorrido = intermedio.ListarEstadoViaticoCorrido();

            if (estadosViaticoCorrido.Codigo != -1)
            {
                foreach (var item in (List<EstadoViaticoCorrido>)estadosViaticoCorrido.Contenido)
                {
                    respuesta.Add(new CEstadoViaticoCorridoDTO
                    {
                        IdEntidad = item.PK_EstadoViaticoCorrido,
                        NomEstadoDTO = item.NomEstado
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)estadosViaticoCorrido.Contenido);
            }

            return respuesta;
        }

        /// <summary>
        /// Metodo encargado de anular viatico corrido
        /// </summary>
        /// <param name="viaticoC"></param>
        /// <returns></returns>
        public CBaseDTO AnularViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                var viaticoCorridoDB = new ViaticoCorrido
                {
                    PK_ViaticoCorrido = viaticoC.IdEntidad,
                    ObsViaticoCorrido = viaticoC.ObsViaticoCorridoDTO
                };
                var datosViaticoCorrido = intermedio.AnularViaticoCorrido(viaticoCorridoDB);
                if (datosViaticoCorrido.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = viaticoC.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViaticoCorrido.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO FinalizarViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                var viaticoCorridoDB = new ViaticoCorrido
                {
                    PK_ViaticoCorrido = viaticoC.IdEntidad,
                    FecFinViaticoCorrido = viaticoC.FecFinDTO,
                    ObsViaticoCorrido = viaticoC.ObsViaticoCorridoDTO
                };
                var datosViaticoCorrido = intermedio.FinalizarViaticoCorrido(viaticoCorridoDB);
                if (datosViaticoCorrido.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = viaticoC.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViaticoCorrido.Contenido;
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
        public CBaseDTO ObtenerMontoRetroactivo(CCartaPresentacionDTO carta, List<DateTime> fecha)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                var cartaDB = new CartasPresentacion { NumCarta = carta.NumeroCarta };
                var datos = intermedio.ObtenerMontoRetroactivo(cartaDB, fecha);
                if (datos.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        Mensaje = datos.Contenido.ToString()
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

        public CBaseDTO ModificarEstadoViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                CEstadoViaticoCorridoD intermedioEstado = new CEstadoViaticoCorridoD(contexto);
                var viaticoCorridoDB = new ViaticoCorrido
                {
                    PK_ViaticoCorrido = viaticoC.IdEntidad
                };
                var estado = intermedioEstado.BuscarEstadoViaticoCorridoNombre(viaticoC.EstadoViaticoCorridoDTO.NomEstadoDTO).Contenido;
                if (estado.GetType() == typeof(CErrorDTO))
                    return (CBaseDTO)estado;
                var datosViaticoC = intermedio.ModificarEstadoViaticoCorrido(viaticoCorridoDB, (EstadoViaticoCorrido)estado);
                if (datosViaticoC.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = viaticoC.IdEntidad,
                        Mensaje = viaticoC.CodigoViaticoCorrido
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViaticoC.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ModificarViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);

                var Presupuesto = (Presupuesto)intermedioPresupuesto.BuscarPresupXCodPresupuestario(viaticoC.PresupuestoDTO.CodigoPresupuesto).Contenido;

                var viaticoCorridoDB = new ViaticoCorrido
                {
                    PK_ViaticoCorrido = viaticoC.IdEntidad,
                    FK_Presupuesto = Presupuesto.PK_Presupuesto,
                    ImgDocumento = viaticoC.DocAdjunto,
                };
               
                var datosViaticoC = intermedio.ModificarViaticoCorrido(viaticoCorridoDB);
                if (datosViaticoC.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = viaticoC.IdEntidad,
                        Mensaje = viaticoC.CodigoViaticoCorrido
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViaticoC.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public List<List<CBaseDTO>> ListarViaticoCorridoPago(int mes, int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioViatico.ListarViaticoCorridoPago(mes, anio);

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<ViaticoCorrido>)resultado.Contenido)
                    {
                        List<CBaseDTO> lista = new List<CBaseDTO>();

                        // [0] ViaticoCorrido
                        var datoViatico = ConvertirViaticoCorridoDatosaDTO(item);
                        lista.Add(datoViatico);


                        // [1] PagoViaticoCorrido
                        if(item.PagoViaticoCorrido.Count > 0)
                        {
                            var datoPago = item.PagoViaticoCorrido
                                                    .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1)
                                                    .FirstOrDefault();
                            if(datoPago != null)
                            {
                                lista.Add(new CPagoViaticoCorridoDTO
                                {
                                    IdEntidad = datoPago.PK_PagoViaticoCorrido,
                                    FecPago = datoPago.FecPago,
                                    MonPago = datoPago.MonPago,
                                    HojaIndividualizada = datoPago.HojaIndividualizada,
                                    NumBoleta = datoPago.NumBoleta,
                                    ReservaRecurso = datoPago.ReservaRecurso,
                                    IndEstado = datoPago.IndEstado,
                                    FecRegistro = datoPago.FecRegistro,
                                    ViaticoCorridoDTO = datoViatico
                                });
                            }
                            else
                            {
                                lista.Add(new CPagoViaticoCorridoDTO {
                                    IdEntidad = -1,
                                    MonPago = 0,
                                    HojaIndividualizada = "",
                                    NumBoleta = "",
                                    ReservaRecurso = "S/R",
                                    ViaticoCorridoDTO = datoViatico
                                });
                            }
                        }
                        else
                        {
                            lista.Add(new CPagoViaticoCorridoDTO());
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

        public List<List<CBaseDTO>> ListarViaticoPagosPendientes(int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> lista;
            CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
            int mesVence = 0;

            try
            {
                var resultado = intermedioViatico.ListarViaticoPagosPendientes(anio);

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<ViaticoCorrido>)resultado.Contenido)
                    {
                        lista = new List<CBaseDTO>();

                        if (item.PagoViaticoCorrido.Count > 0)
                        {
                            mesVence = Convert.ToDateTime(item.FecFinViaticoCorrido).Month;
                            var datoPago = item.PagoViaticoCorrido
                                                    .Where(P => P.FecPago.Month == mesVence &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.NumBoleta == "")
                                                    .OrderByDescending(P => P.FecPago)
                                                    .FirstOrDefault();

                            if (datoPago != null)
                            {
                                // [0] ViaticoCorrido
                                var datoViatico = ConvertirViaticoCorridoDatosaDTO(item);
                                lista.Add(datoViatico);

                                // [1] PagoViaticoCorrido
                                lista.Add(new CPagoViaticoCorridoDTO
                                {
                                    IdEntidad = datoPago.PK_PagoViaticoCorrido,
                                    FecPago = datoPago.FecPago,
                                    MonPago = datoPago.MonPago,
                                    HojaIndividualizada = datoPago.HojaIndividualizada,
                                    NumBoleta = datoPago.NumBoleta,
                                    ReservaRecurso = datoPago.ReservaRecurso,
                                    IndEstado = datoPago.IndEstado,
                                    FecRegistro = datoPago.FecRegistro,
                                    ViaticoCorridoDTO = datoViatico
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
        public List<List<CBaseDTO>> ListarPagosViaticoCorrido(int mes, int anio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            
            CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioViatico.ListarPagosViaticoCorrido(mes, anio);
               
                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<PagoViaticoCorrido>)resultado.Contenido)
                    {
                        List<CBaseDTO> lista = new List<CBaseDTO>();

                        // [0] PagoViaticoCorrido
                        lista.Add(new CPagoViaticoCorridoDTO
                        {
                            IdEntidad = item.PK_PagoViaticoCorrido,
                            FecPago = item.FecPago,
                            MonPago = item.MonPago,
                            HojaIndividualizada = item.HojaIndividualizada,
                            NumBoleta = item.NumBoleta,
                            ReservaRecurso = item.ReservaRecurso,
                            IndEstado = item.IndEstado,
                            FecRegistro = item.FecRegistro
                        });

                        // [1] ViaticoCorrido
                        lista.Add(ConvertirViaticoCorridoDatosaDTO(item.ViaticoCorrido));

                        // [2] Funcionario
                        lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.ViaticoCorrido.Nombramiento.Funcionario));

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

        /// <summary>
        /// Metodo encargador de ahcer logica con el metodo lista meses anteriores en datos de Viatico corrido
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> ListarPagoMesesAnteriores(string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultadoFuncionario = intermedio.PruebaBuscarFuncionarioCedula(cedula);
                if (resultadoFuncionario.Codigo != -1)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral((Funcionario)resultadoFuncionario.Contenido) });
                    CViaticoCorridoD intermedioCalificaciones = new CViaticoCorridoD(contexto);

                    var mesesanteriores = intermedioCalificaciones.ListarPagoMesesAnteriores(cedula);
                    if (mesesanteriores != null)
                    {
                        List<CBaseDTO> mesesanterioresdata = new List<CBaseDTO>();
                        foreach (var item in mesesanteriores)
                        {
                            mesesanterioresdata.Add(new CViaticoCorridoDTO
                            {
                                NombramientoDTO = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                                MontViaticoCorridoDTO = item.MontViaticoCorrido,
                                FecFinDTO = Convert.ToDateTime(item.FecFinViaticoCorrido),
                                FecInicioDTO = Convert.ToDateTime(item.FecInicViaticoCorrido),
                                EstadoViaticoCorridoDTO = CEstadoViaticoCorridoL.ConvertirEstadoViaticoCorridoDTOaDatos(item.EstadoViaticoCorrido),
                                ObsViaticoCorridoDTO = item.ObsViaticoCorrido == null ? "" : item.ObsViaticoCorrido,
                                DesSenasDTO = item.DesSeñas,
                                NumTelDomicilioDTO = item.NumTelDomicilio,
                                NomDistritoDTO = CDistritoL.ConvertirViaticoCorridoDTOaDatos(item.Distrito),
                                PernocteDTO = item.ObsLugarPernocte,
                                HospedajeDTO = item.ObsLugarHospedaje,
                                IdEntidad = item.PK_ViaticoCorrido

                            });
                        }
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

        public List<List<CBaseDTO>> ListarViaticos(CViaticoCorridoDTO datoBusqueda)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            try
            {
                var resultado = intermedioViatico.ListarViaticoCorrido();

                if (resultado.Codigo != -1)
                {
                    var datos = (List<ViaticoCorrido>)(((List<object>)resultado.Contenido)[0]);
                    //datos = datos.Where(Q => Q.FecInicViaticoCorrido.Year == DateTime.Today.Year).ToList();

                    // Cédula
                    if (datoBusqueda.NombramientoDTO.Funcionario.Cedula != null)
                        datos = datos.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario.Contains(datoBusqueda.NombramientoDTO.Funcionario.Cedula)).ToList();

                    // Estado
                    if (datoBusqueda.EstadoViaticoCorridoDTO.IdEntidad != -1)
                        datos = datos.Where(Q => Q.EstadoViaticoCorrido.PK_EstadoViaticoCorrido == datoBusqueda.EstadoViaticoCorridoDTO.IdEntidad).ToList();
                    else
                        datos = datos.Where(Q => Q.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3).ToList(); // Anulados
                    
                    // Fecha Inicio
                    if (datoBusqueda.FecInicioDTO != null && datoBusqueda.FecInicioDTO.Year != 1)
                        datos = datos.Where(Q => Q.FecInicViaticoCorrido >= datoBusqueda.FecInicioDTO).ToList();
                    
                    // Fecha Vence
                    if (datoBusqueda.FecFinDTO != null && datoBusqueda.FecFinDTO.Year != 1)
                        datos = datos.Where(Q => Q.FecFinViaticoCorrido <= datoBusqueda.FecFinDTO).ToList();

                    if (datos.Count < 1)
                        throw new Exception("No se encontraron datos de Viático Corrido de acuerdo a los parámetros de búsqueda");

                    foreach (var item in datos)
                    {
                        List<CBaseDTO> lista = new List<CBaseDTO>();

                        // [0] ViaticoCorrido
                        lista.Add(ConvertirViaticoCorridoDatosaDTO(item));

                        // [1] Funcionario
                        lista.Add(CFuncionarioL.FuncionarioGeneral((Funcionario)item.Nombramiento.Funcionario));

                        // [2] Expediente
                        if( ((Funcionario)item.Nombramiento.Funcionario).ExpedienteFuncionario.Count() > 0)
                            lista.Add(CExpedienteL.ConvertirExpedienteADTO( ((Funcionario)item.Nombramiento.Funcionario).ExpedienteFuncionario.FirstOrDefault()));
                        else
                            lista.Add(new CExpedienteFuncionarioDTO { NumeroExpediente = 0, Mensaje = "No tiene un número de Expediente registrado" });

                        var datosFuncionario = BuscarFuncionarioCedula(((Funcionario)item.Nombramiento.Funcionario).IdCedulaFuncionario);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            // [3] Puesto
                            lista.Add((CPuestoDTO)datosFuncionario[1]);
                            // [4] DetallePuesto
                            lista.Add((CDetallePuestoDTO)datosFuncionario[2]);
                            if (datosFuncionario[6].GetType() != typeof(CErrorDTO))
                            {
                                var ubicacion1 = (CUbicacionPuestoDTO)datosFuncionario[5];
                                var ubicacion2 = (CUbicacionPuestoDTO)datosFuncionario[6];

                                if (ubicacion1.TipoUbicacion.IdEntidad == 1) // Contrato
                                {
                                    // [5] Ubicación Contrato
                                    lista.Add(ubicacion1);
                                    // [6] Ubicación Trabajo
                                    lista.Add(ubicacion2);
                                }
                                else
                                {
                                    // [5] Ubicación Contrato
                                    lista.Add(ubicacion2);
                                    // [6] Ubicación Trabajo
                                    lista.Add(ubicacion1);
                                }
                            }
                            else
                            {
                                // [5] Ubicación Contrato
                                lista.Add(new CUbicacionPuestoDTO
                                {
                                    Distrito = new CDistritoDTO
                                    {
                                        IdEntidad = 0,
                                        NomDistrito = "",
                                        Canton = new CCantonDTO { IdEntidad = 0, NomCanton = "", Provincia = new CProvinciaDTO { IdEntidad = 0, NomProvincia = "" } }
                                    }
                                });
                                // [6] Ubicación Trabajo
                                lista.Add(new CUbicacionPuestoDTO
                                {
                                    Distrito = new CDistritoDTO
                                    {
                                        IdEntidad = 0,
                                        NomDistrito = "",
                                        Canton = new CCantonDTO { IdEntidad = 0, NomCanton = "", Provincia = new CProvinciaDTO { IdEntidad = 0, NomProvincia = "" } }
                                    }
                                });
                            }
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                        }

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
                respuesta.Clear();
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }

        /// <summary>
        /// Metodo encargado de obtener el monto retro activo
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public CBaseDTO ObtenerViaticoCorridoActual(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                var datos = intermedio.ObtenerViaticoCorridoActual(cedula);
                if (datos.Codigo > 0)
                {
                    respuesta = ConstruirViaticoCorridoGeneral((ViaticoCorrido)datos.Contenido);
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
        
        public CBaseDTO AgregarPagoViaticoCorrido(CPagoViaticoCorridoDTO pago, List<CDetallePagoViaticoCorridoDTO> detalles, CFuncionarioDTO funcionario)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CViaticoCorridoD intermedioViaticoCorrido = new CViaticoCorridoD(contexto);

                var datoPago = new PagoViaticoCorrido
                {
                    IndEstado = pago.IndEstado,
                    FecPago = pago.FecPago,
                    MonPago = pago.MonPago,
                    HojaIndividualizada = pago.HojaIndividualizada,
                    NumBoleta = pago.NumBoleta,
                    ReservaRecurso = pago.ReservaRecurso,
                    FecRegistro = DateTime.Now
                };

                var dato = intermedioViaticoCorrido.ObtenerViaticoCorrido(pago.ViaticoCorridoDTO.IdEntidad.ToString());
                datoPago.ViaticoCorrido = (ViaticoCorrido)dato.Contenido;

                List<DetallePagoViaticoCorrido> datoDetalle = new List<DetallePagoViaticoCorrido>();

                foreach (var item in detalles)
                {
                    datoDetalle.Add(new DetallePagoViaticoCorrido
                    {
                        TipoDetallePagoViatico = new TipoDetallePagoViatico
                        {
                            PK_TipoDetallePagoViatico = item.TipoDetalleDTO.IdEntidad
                        },
                        FecDiaPago = Convert.ToDateTime(item.FecDiaPago),
                        MonPago = item.MonPago,
                        CodEntidad = item.CodEntidad,
                    });
                }

                var datoFuncionario = new Funcionario { IdCedulaFuncionario = funcionario.Cedula };

                respuesta =  intermedioViaticoCorrido.AgregarPagoViaticoCorrido(datoPago, datoDetalle, datoFuncionario);

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

        public CBaseDTO AsignarReservaRecurso(CPagoViaticoCorridoDTO pago)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

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
        
        public CBaseDTO AnularPagoViaticoCorrido(CPagoViaticoCorridoDTO pago)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                var datoPago = new PagoViaticoCorrido
                {
                    PK_PagoViaticoCorrido = pago.IdEntidad
                };

                var datos = intermedio.AnularPagoViaticoCorrido(datoPago);
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
        
        public List<List<CBaseDTO>> ObtenerPagoViaticoCorrido(int codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> temp = new List<CBaseDTO>();

            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);
                CPuestoL intermedioPuesto = new CPuestoL();

                var pagoVC = intermedio.ObtenerPagoViaticoCorrido(codigo);
                if (pagoVC.Codigo > 0)
                {
                    //[0] PagoViaticoCorrido
                    var dato = ConstruirPagoViaticoCorridoGeneral((PagoViaticoCorrido)pagoVC.Contenido);
                    temp.Add(dato);

                    var viaticoC = ObtenerViaticoCorrido(dato.ViaticoCorridoDTO.IdEntidad.ToString());

                    if (viaticoC.Count() > 1)
                    {
                        // [0]
                        temp.Add(viaticoC[0][0]); // [1] Viático Corrido
                        temp.Add(viaticoC[0][1]); // [2] Funcionario
                        temp.Add(viaticoC[0][2]); // [3] Nombramiento
                        temp.Add(viaticoC[0][3]); // [4] Puesto
                        temp.Add(viaticoC[0][4]); // [5] Carta Presentación
                        temp.Add(viaticoC[1][1]); // [6] Detalle Puesto
                        temp.Add(viaticoC[1][2]); // [7] Ubicación Puesto
                        temp.Add(viaticoC[1][3]); // [8] Ubicación Puesto
                        respuesta.Add(temp);

                        // [1] Factura Desarraigo
                        respuesta.Add(viaticoC[2]);
                        //if (viaticoC[2].Count() > 0)
                        //    respuesta.Add(viaticoC[2]); 
                        //else
                        //    respuesta.Add(new CBaseDTO());

                        // [2] Contrato Arrendamiento
                        respuesta.Add(viaticoC[3]);
                        //if (viaticoC[3].Count() > 0)
                        //    respuesta.Add(viaticoC[3][0]);
                        //else
                        //    respuesta.Add(new CBaseDTO());
                    }
                    else
                    {
                        temp.Add((CErrorDTO)viaticoC[0][0]);
                        respuesta.Add(temp);
                    }
                }
                else
                {
                    temp.Add((CErrorDTO)pagoVC.Contenido);
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


        public CBaseDTO AgregarReintegro (List<CViaticoCorridoReintegroDTO> lista)
        {
            CBaseDTO respuesta = new CBaseDTO();
            List<ViaticoCorridoReintegroDias> listaDias = new List<ViaticoCorridoReintegroDias>();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

                foreach (var item in lista)
                {
                    listaDias.Add(new ViaticoCorridoReintegroDias
                    {
                        FK_ViaticoCorrido = item.ViaticoCorridoDTO.IdEntidad,
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
        /// <param name="idviatico">PK del viatico</param>
        /// <param name="feccontrato">Fecha de contrato del viatico</param>
        /// <param name="numdocumento">Número de documento del viatico</param>
        /// <returns></returns>
        public CBaseDTO ModificarFecContratoNumDocVC(int idviatico, DateTime feccontrato, string numdocumento)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

                var datosViatico = intermedio.ModificarFecContratoNumDocVC(idviatico, feccontrato, numdocumento);
                if (datosViatico.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = datosViatico.IdEntidad,
                        Mensaje = "Fecha de contrato y/o numero de documento modificados con éxito"
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViatico.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        public CBaseDTO ModificarPagoReservaRecursoVC(int idPago, string reserva)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CViaticoCorridoD intermedio = new CViaticoCorridoD(contexto);

                var datosViatico = intermedio.ModificarPagoReservaRecursoVC(idPago, reserva);
                if (datosViatico.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = datosViatico.IdEntidad,
                        Mensaje = "Reserva Recurso asignada con éxito"
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosViatico.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }


        public CBaseDTO AgregarCatalogoDia(CCatDiaViaticoGastoDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CViaticoCorridoD intermedioViaticoCorrido = new CViaticoCorridoD(contexto);
                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);

                var dato = new CatDiaViaticoGasto
                {
                    DesDia = registro.DescripcionDia,
                    FecDia = registro.Dia,
                    IndRebajo = registro.IndRebajo,
                    IndGasto = registro.IndGasto,
                    IndViatico = registro.IndViatico,
                    FK_TipoDia = registro.TipoDia.IdEntidad,
                    TipoDia = (TipoDia)intermedioTipoDia.BuscarTipoDia(registro.TipoDia.IdEntidad).Contenido
                };
                
                respuesta = intermedioViaticoCorrido.AgregarCatalogoDia(dato);

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


        public List<CBaseDTO> ListarCatalogoDia()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CViaticoCorridoD intermedioViatico = new CViaticoCorridoD(contexto);
            
            try
            {
                var resultado = intermedioViatico.ListarCatalogoDia();

                if (resultado.Codigo != -1)
                {
                    foreach (var item in (List<CatDiaViaticoGasto>)resultado.Contenido)
                    {
                        respuesta.Add(new CCatDiaViaticoGastoDTO
                        {
                            IdEntidad = item.PK_CatalogoDia,
                            DescripcionDia = item.DesDia,
                            Dia = item.FecDia,
                            IndRebajo = item.IndRebajo,
                            IndViatico = item.IndViatico,
                            IndGasto = item.IndGasto,
                            TipoDia = CTipoDiaL.ConvertirTipoDiaADto(item.TipoDia)
                        });
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)resultado.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message } );
                return respuesta;
            }

            return respuesta;
        }

        #endregion
    }
}

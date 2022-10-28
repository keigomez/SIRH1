using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Datos
{
    public class CViaticoCorridoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CViaticoCorridoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargado de generar un codigo de informe para el viatico corrido
        /// </summary>
        /// <param name="viaticoCorrido"></param>
        /// <returns></returns>
        private string GenerarCodigoInforme(ViaticoCorrido viaticoCorrido)
        {
            var annio = viaticoCorrido.FecFinViaticoCorrido.Year;
            var datos = entidadBase.ViaticoCorrido.Where(D => D.FecFinViaticoCorrido.Year == annio).ToList();
            var secuencia = datos.IndexOf(viaticoCorrido) + 1;
            return annio + "-" + secuencia;
        }
       
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo viatico como respuesta DTO
        /// </summary>
        /// <param name="viaticoCorrido"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarViaticoCorrido(CartasPresentacion carta, Funcionario funcionario, ViaticoCorrido viaticoCorrido)
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
                    //var ViaticoCorridoActivos = nombramiento.ViaticoCorrido.Count(D => D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3);

                    bool fechaValida = true;
                    var listaDias = Enumerable.Range(0, (viaticoCorrido.FecFinViaticoCorrido - viaticoCorrido.FecInicViaticoCorrido).Days + 1)
                                              .Select(d => viaticoCorrido.FecInicViaticoCorrido.AddDays(d))
                                              .ToArray();

                    var ViaticoCorridoActivos = nombramiento.ViaticoCorrido.Where(D => D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3).ToList();
                    foreach (var item in ViaticoCorridoActivos)
                    {
                        listaDias = Enumerable.Range(0, (item.FecFinViaticoCorrido - item.FecInicViaticoCorrido).Days + 1)
                                              .Select(d => item.FecInicViaticoCorrido.AddDays(d))
                                              .ToArray();
                        if (listaDias.Contains(viaticoCorrido.FecInicViaticoCorrido) || listaDias.Contains(viaticoCorrido.FecFinViaticoCorrido))
                            fechaValida = false;
                    }


                    //si no hay Viáticos activos
                    //if (ViaticoCorridoActivos == 0)

                    if (fechaValida)
                    {
                        //Servidor Activo 
                        if (nombramiento.Funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1)
                        {
                            //var estadoNombramiento = nombramiento.EstadoNombramiento.PK_EstadoNombramiento;
                            //El estado del nombramiento es: Propiedad,Nombramiento interino,Ascenso interino
                            // if (estadoNombramiento == 1 || estadoNombramiento == 2 || estadoNombramiento == 9)

                            //var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                            listaEstados.Contains(nombramiento.EstadoNombramiento.PK_EstadoNombramiento);

                            if (listaEstados.Contains(nombramiento.EstadoNombramiento.PK_EstadoNombramiento))
                            {
                                viaticoCorrido.FecRegistro = DateTime.Today;
                                viaticoCorrido.FecContrato = DateTime.Today;
                                if (viaticoCorrido.FecFinViaticoCorrido.Year == 1)
                                    viaticoCorrido.FecFinViaticoCorrido = Convert.ToDateTime(nombramiento.FecVence);

                                viaticoCorrido.Nombramiento = nombramiento;
                                //nombramiento.ViaticoCorrido.Add(viaticoCorrido);
                                entidadBase.ViaticoCorrido.Add(viaticoCorrido);
                                entidadBase.SaveChanges();
                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = GenerarCodigoInforme(viaticoCorrido),
                                    IdEntidad = viaticoCorrido.PK_ViaticoCorrido
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("El nombramiento no es válido.");
                            }
                        }
                        else
                        {
                            throw new Exception("El funcionario no tiene el estado válido.");
                        }
                    }
                    else
                    {
                        throw new Exception("Ya existe un viático registrado");
                    }
                }
                else
                {
                    throw new Exception("El funcionario no tiene un nombramiento válido");
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
        /// Almacena el pago de Viático Corrido que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el pk del registro almacenado</returns>
        public CRespuestaDTO AgregarPagoViaticoCorrido(PagoViaticoCorrido pago, List<DetallePagoViaticoCorrido> detalles, Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {

                funcionario = entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null) //si existe el funcionario
                {
                    var viatico = entidadBase.ViaticoCorrido.Where(V => V.PK_ViaticoCorrido == pago.ViaticoCorrido.PK_ViaticoCorrido &&
                                                                    V.FK_EstadoViaticoCorrido == 1).FirstOrDefault();

                    if (viatico != null)
                    {   //si existe el Viático Corrido

                        if(pago.FecPago.Month >= viatico.FecInicViaticoCorrido.Month && pago.FecPago <= viatico.FecFinViaticoCorrido)
                        {
                            // Buscar si existe el pago como Retroactivo para el mes indicado
                            var pagoViatico = entidadBase.PagoViaticoCorrido.Where(PV => PV.IndEstado == 1 &&
                                                                                    PV.FK_ViaticoCorrido == viatico.PK_ViaticoCorrido &&
                                                                                    PV.PagoViaticoRetroactivo.Where(Q=> Q.FecPago.Month == pago.FecPago.Month &&
                                                                                                                PV.FecPago.Year == pago.FecPago.Year).Count() > 0)
                                                                                    .FirstOrDefault();

                            // No existe el pago del Viático Corrido para el mes indicado
                            if (pagoViatico == null)
                            {
                                pagoViatico = entidadBase.PagoViaticoCorrido.Where(PV => PV.IndEstado == 1 &&
                                                                            PV.FecPago.Month == pago.FecPago.Month &&
                                                                            PV.FecPago.Year == pago.FecPago.Year &&
                                                                            PV.FK_ViaticoCorrido == viatico.PK_ViaticoCorrido).FirstOrDefault();
                            }

                            // No existe el pago del Viático Corrido para el mes indicado
                            if (pagoViatico == null)
                            {
                                entidadBase.PagoViaticoCorrido.Add(pago);
                                entidadBase.SaveChanges();

                                // Agregar Detalle de días de Pago;
                                DetallePagoViaticoCorrido detalle;

                                foreach (var item in detalles)
                                {
                                    detalle = new DetallePagoViaticoCorrido
                                    {
                                        FK_PagoViaticoCorrido = pago.PK_PagoViaticoCorrido,
                                        FK_TipoDetallePagoViatico = item.TipoDetallePagoViatico.PK_TipoDetallePagoViatico,
                                        FecDiaPago = item.FecDiaPago,
                                        MonPago = item.MonPago,
                                        CodEntidad = item.CodEntidad
                                    };
                                    entidadBase.DetallePagoViaticoCorrido.Add(detalle);
                                    entidadBase.SaveChanges();
                                }

                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = pago.PK_PagoViaticoCorrido
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("Ya existe el Pago del Viático Corrido para el mes indicado");
                            }
                        }
                        else
                        {
                            throw new Exception("La Fecha de Pago está fuera del periodo del Viático Corrido");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro del Viático Corrido indicado");
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
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerPagoViaticoCorrido(int codPago)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.PagoViaticoCorrido
                                        .Include("ViaticoCorrido")
                                        .Include("DetallePagoViaticoCorrido")
                                        .Include("DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                        .Where(PK => PK.PK_PagoViaticoCorrido == codPago).FirstOrDefault();

                if (dato != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Pago del Viático Corrido");
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

        public CRespuestaDTO ObtenerMotivoDetalleDeduccion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.DetalleDeduccionViaticoCorrido
                                    .Where(Q => Q.PK_DetalleDeduccionViaticoCorrido == codigo).FirstOrDefault();

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

        public CRespuestaDTO AgregarReintegro(List<ViaticoCorridoReintegroDias> lista)
        {
            CRespuestaDTO respuesta;
            try
            {
                // Agregar Detalle de días de Pago;
                ViaticoCorridoReintegroDias reintegro;
                int total = 0;

                foreach (var item in lista)
                {
                    reintegro = new ViaticoCorridoReintegroDias
                    {
                        FK_ViaticoCorrido = item.FK_ViaticoCorrido,
                        FecDia = item.FecDia,
                        MonReintegro = item.MonReintegro,
                        ObsMotivo = item.ObsMotivo,
                        IndEstado = item.IndEstado
                    };
                    entidadBase.ViaticoCorridoReintegroDias.Add(reintegro);
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


        /// <summary>
        /// metodo encargado de editar un estado de un viatico corrido
        /// </summary>
        /// <param name="viaticoC"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        /// 

        public CRespuestaDTO ModificarEstadoViaticoCorrido(ViaticoCorrido viaticoC, EstadoViaticoCorrido estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var ViaticoCorridoOld = entidadBase.ViaticoCorrido.Include("EstadoViaticoCorrido").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido);
                if (ViaticoCorridoOld != null)
                {
                    ViaticoCorridoOld.EstadoViaticoCorrido = estado;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(ViaticoCorridoOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el ViaticoCorrido requerido");
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
        public CRespuestaDTO ModificarViaticoCorrido(ViaticoCorrido viaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var ViaticoCorridoOld = entidadBase.ViaticoCorrido.Include("EstadoViaticoCorrido").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido);
                if (ViaticoCorridoOld != null)
                {
                    if (ViaticoCorridoOld.EstadoViaticoCorrido.PK_EstadoViaticoCorrido == 2) // "Espera"
                    {
                        ViaticoCorridoOld.FK_Presupuesto = viaticoC.FK_Presupuesto;
                        if(viaticoC.ImgDocumento != null)
                            ViaticoCorridoOld.ImgDocumento = viaticoC.ImgDocumento;

                        entidadBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = GenerarCodigoInforme(ViaticoCorridoOld)
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("El Viatico Corrido no es modificable");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el Viatico Corrido requerido");
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
        /// metodo encargado de optener un solo viatico corrido
        /// </summary>
        /// <param name="codViaticoC"></param>
        /// <returns></returns>
        //Codigo tipo: yyyy-secuencial
        public CRespuestaDTO ObtenerViaticoCorrido(string codViaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(codViaticoC);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var viaticoCorrido = entidadBase.ViaticoCorrido
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.CartasPresentacion")
                                                .Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Include("EstadoViaticoCorrido")
                                                .Include("MovimientoViaticoCorrido")
                                                .Include("MovimientoViaticoCorrido.DetalleDeduccionViaticoCorrido")
                                                .Include("PagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo.DetallePagoViaticoRetroactivo")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo.DetallePagoViaticoRetroactivo.TipoDetallePagoViatico")
                                                .Include("ViaticoCorridoReintegroDias")
                                                .FirstOrDefault(D => D.PK_ViaticoCorrido == cod);
                //.Where(D => D.FecInicDesarraigo.Value.Year == annio)
                //.OrderBy(D => D.FecInicDesarraigo).ToList().ElementAtOrDefault(consecutivo);
                if (viaticoCorrido != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoCorrido,
                        Mensaje = GenerarCodigoInforme(viaticoCorrido)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Viático Corrido");
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
        /// Metodo encargador de filtrar viatico corrido
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private List<ViaticoCorrido> FiltrarViatiCorrido(object[] parametros)
        {
            var datos = entidadBase.ViaticoCorrido.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoViaticoCorrido")
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
                    case "NumViaticoCorrido":
                        var datosBusqueda = parametros[i + 1].ToString().Split('-'); // [año,consecutivo]
                        var annio = int.Parse(datosBusqueda[0]);
                        var consecutivo = int.Parse(datosBusqueda[1]) - 1;
                        var viaticoCorrido = datos.Where(D => D.FecInicViaticoCorrido.Year == annio).ToList()
                                              .ElementAtOrDefault(consecutivo);
                        if (viaticoCorrido != null)
                            datos = datos.Where(D => D.PK_ViaticoCorrido == viaticoCorrido.PK_ViaticoCorrido);
                        break;
                    case "Estado":
                        var estado = parametros[i + 1].ToString();
                        if (estado == "Todas las Notificaciones")
                            datos = datos.Where(D => D.EstadoViaticoCorrido.NomEstado != "Valido"
                                                     && D.EstadoViaticoCorrido.NomEstado != "Espera"
                                                     && D.EstadoViaticoCorrido.NomEstado != "Anulado");
                        else
                            datos = datos.Where(D => D.EstadoViaticoCorrido.NomEstado == estado);
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
                        datos = datos.Where(D => D.FecInicViaticoCorrido >= fechaInicioI && D.FecFinViaticoCorrido <= fechaFinI);
                        break;

                    case "FechaFinal":
                        var fechaInicioF = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(D => D.FecFinViaticoCorrido >= fechaInicioF && D.FecFinViaticoCorrido <= fechaFinF);
                        break;
                    default: throw new Exception("Busqueda no definida");
                }
            }
            return datos.ToList();
        }


        /// <summary>
        /// metodo encargado de listar un viatico corrido con codigos
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private List<object> ViaticoCorridoCodigo(List<ViaticoCorrido> datos)
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

        /// <summary>
        /// Metodo encargado de actualizar un viatico corrido
        /// </summary>
        /// <param name="fechaVencimiento"></param>
        /// <returns></returns>
        public CRespuestaDTO ActualizarVencimientoViaticoCorrido(DateTime fechaVencimiento)
        {
            CRespuestaDTO respuesta;
            List<ViaticoCorrido> ViaticoCorridoVencido = new List<ViaticoCorrido>();
            try
            {
                var datosViaticoCorrido = entidadBase.ViaticoCorrido.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoViaticoCorrido")
                                        .Where(D => D.FecFinViaticoCorrido <= fechaVencimiento &&
                                                     D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 4 &&
                                                     D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3).ToList();
                if (datosViaticoCorrido.Count > 0)
                {
                    var estado = entidadBase.EstadoViaticoCorrido.FirstOrDefault(E => E.PK_EstadoViaticoCorrido == 4);
                    foreach (var item in datosViaticoCorrido)
                    {
                        item.EstadoViaticoCorrido = estado;
                        entidadBase.SaveChanges();
                        ViaticoCorridoVencido.Add(item);
                    }
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ViaticoCorridoCodigo(ViaticoCorridoVencido)
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay Viatico Corrido cuyo vencimiento se alcance el día de hoy" }
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


        public CRespuestaDTO AsignarReservaRecurso(int idPago, string reserva)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoViaticoCorrido
                                                .Include("DetallePagoViaticoCorrido")
                                                .Include("ViaticoCorrido")
                                                .Include("ViaticoCorrido.Nombramiento")
                                                .Include("ViaticoCorrido.Nombramiento.Funcionario")
                                                .Where(P => P.PK_PagoViaticoCorrido == idPago &&
                                                            P.IndEstado == 1 &&
                                                            P.NumBoleta.Length == 0 &&
                                                            P.ViaticoCorrido.FK_EstadoViaticoCorrido != 3) // 3 - Anulado
                                               .FirstOrDefault();

                if (datosEntidad != null)
                {
                    datosEntidad.ReservaRecurso = reserva;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad.PK_PagoViaticoCorrido
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

        /// <summary>
        /// Metodo encargado de anular un viatico corrido
        /// </summary>
        /// <param name="viaticoC"></param>
        /// <returns></returns>
        public CRespuestaDTO AnularViaticoCorrido(ViaticoCorrido viaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var viaticoCorridoOld = entidadBase.ViaticoCorrido.Include("EstadoViaticoCorrido")
                                                 .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido);
                if (viaticoCorridoOld != null)
                {
                    viaticoCorridoOld.EstadoViaticoCorrido.PK_EstadoViaticoCorrido = 3;
                    viaticoCorridoOld.ObsViaticoCorrido = viaticoC.ObsViaticoCorrido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoC
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Viático Corrido");
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
        /// Anula un trámite de pago
        /// </summary>
        /// <returns>Retorna el trámite anulado</returns>
        public CRespuestaDTO AnularPagoViaticoCorrido(PagoViaticoCorrido pago)
        {
            CRespuestaDTO respuesta;

            try
            {
                var pagoOld = entidadBase.PagoViaticoCorrido.Include("ViaticoCorrido")
                                                    .Where(PK => PK.PK_PagoViaticoCorrido == pago.PK_PagoViaticoCorrido).FirstOrDefault();

                if (pagoOld != null)
                {
                    pagoOld.IndEstado = 0;
                    pago = pagoOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = pago.PK_PagoViaticoCorrido
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun trámite de pago de Viático Corrido con el código especificado." }
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


        public CRespuestaDTO FinalizarViaticoCorrido(ViaticoCorrido viaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var viaticoCorridoOld = entidadBase.ViaticoCorrido.Include("EstadoViaticoCorrido")
                                                 .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido);
                if (viaticoCorridoOld != null)
                {
                    viaticoCorridoOld.FecFinViaticoCorrido = viaticoC.FecFinViaticoCorrido;
                    viaticoCorridoOld.ObsViaticoCorrido = viaticoC.ObsViaticoCorrido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoC
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Viático Corrido");
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
        /// metodo encargado de listar un viatico corrido
        /// </summary>
        /// <returns></returns>
        public CRespuestaDTO ListarViaticoCorrido()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.ViaticoCorrido
                                                .Include("EstadoViaticoCorrido")
                                                .Include("PagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                                .Include("ViaticoCorridoReintegroDias")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.RelPuestoUbicacion").ToList();

                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ViaticoCorridoCodigo(datosEntidad)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron registros");
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

        public CRespuestaDTO ListarViaticoCorridoPago(int mes, int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.ViaticoCorrido
                                                .Include("EstadoViaticoCorrido")
                                                .Include("PagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                                .Include("ViaticoCorridoReintegroDias")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Where(P => P.FecFinViaticoCorrido.Month >= mes &&
                                                            P.FecFinViaticoCorrido.Year == anio &&
                                                            P.FK_EstadoViaticoCorrido == 1) // 1 - Aprobado // 3 - Anulado
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
                    throw new Exception("No se encontraron registros");
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

        public CRespuestaDTO ListarViaticoPagosPendientes(int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.ViaticoCorrido
                                                .Include("EstadoViaticoCorrido")
                                                .Include("PagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                                .Include("ViaticoCorridoReintegroDias")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Where(P => P.FecFinViaticoCorrido.Year == anio &&
                                                            P.FK_EstadoViaticoCorrido != 3) //1- Aprobado // 3 - Anulado
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
                    throw new Exception("No se encontraron registros");
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

        public CRespuestaDTO ListarViaticoCorridoServicio(int mes, int anio, string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoViaticoCorrido
                                                .Include("DetallePagoViaticoCorrido")
                                                .Include("ViaticoCorrido")
                                                .Include("ViaticoCorrido.Presupuesto")
                                                .Include("ViaticoCorrido.Nombramiento")
                                                .Include("ViaticoCorrido.Nombramiento.Funcionario")
                                                .Include("ViaticoCorrido.Nombramiento.Puesto")
                                                .Include("ViaticoCorrido.Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.ViaticoCorrido.FK_EstadoViaticoCorrido != 3) // 3 - Anulado
                                                .ToList();

                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    if (cedula != null)
                        if (cedula != "")
                            datosEntidad = datosEntidad.Where(D => D.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();

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

        public CRespuestaDTO ListarPagosViaticoCorrido(int mes, int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoViaticoCorrido
                                                .Include("DetallePagoViaticoCorrido")
                                                .Include("ViaticoCorrido")
                                                .Include("ViaticoCorrido.Presupuesto")
                                                .Include("ViaticoCorrido.Nombramiento")
                                                .Include("ViaticoCorrido.Nombramiento.Funcionario")
                                                .Include("ViaticoCorrido.Nombramiento.Puesto")
                                                .Include("ViaticoCorrido.Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.ViaticoCorrido.FK_EstadoViaticoCorrido != 3) // 3 - Anulado
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

        public List<C_ViaticoCorrido> ListarViaticoHistorico(int mes, int anio, string cedula)
        {
            List<C_ViaticoCorrido> resultado = new List<C_ViaticoCorrido>();

            resultado = entidadBase.C_ViaticoCorrido.Where(P => P.FecVence.Month == mes &&
                                                            P.FecVence.Year == anio).ToList();

            if (cedula != null)
                if (cedula != "")
                    resultado = resultado.Where(P => P.Cedula == cedula).ToList();


            return resultado;
        }

        public CRespuestaDTO ActualizarViaticoCorridoServicio(int mes, int anio, string cedula, string reserva, string numBoleta)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.PagoViaticoCorrido
                                                .Include("DetallePagoViaticoCorrido")
                                                .Include("ViaticoCorrido")
                                                .Include("ViaticoCorrido.Nombramiento")
                                                .Include("ViaticoCorrido.Nombramiento.Funcionario")
                                                .Where(P => P.FecPago.Month == mes &&
                                                            P.FecPago.Year == anio &&
                                                            P.IndEstado == 1 &&
                                                            P.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                                                            P.ViaticoCorrido.FK_EstadoViaticoCorrido != 3) // 3 - Anulado
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
                }
                else
                {
                    var datosHistorico = entidadBase.C_ViaticoCorrido.Where(P => P.FecVence.Month == mes && P.FecVence.Year == anio && P.Cedula.Contains(cedula)).ToList();

                    if (datosHistorico.Count > 0)
                    {
                        foreach (var item in datosHistorico)
                        {
                            item.NumFactura = numBoleta;
                            item.DetalleViatico = reserva;
                            entidadBase.SaveChanges();
                        }

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = datosHistorico
                        };
                    }
                    else
                    {
                       throw new Exception("No se encontraron datos");
                    }
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


        /// <summary>
        /// Metodo encargado de buscar segun el parametro el viatico corrido
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public CRespuestaDTO BuscarViaticoCorrido(object[] parametros)
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
                        Contenido = ViaticoCorridoCodigo(datos)
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
        /// Metodo encargado de obtener el monto retroactivo
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public CRespuestaDTO ObtenerMontoRetroactivo(CartasPresentacion carta, List<DateTime> fecha)
        {
            CRespuestaDTO respuesta;
            try
            {
                var nombramiento = entidadBase.Nombramiento
                                                .Include("DesgloseSalarial").FirstOrDefault(N => N.CartasPresentacion.FirstOrDefault(C => C.NumCarta == carta.NumCarta) != null);

                if (nombramiento != null)
                {
                    var fechaIni = fecha[0];
                    var fechaFin = fecha[1];
                    //var desglose = nombramiento.DesgloseSalarial.Where(D =>
                    //{
                    //    var d = DateTime.ParseExact(D.IndPeriodo, "ddMMyyyy", CultureInfo.InvariantCulture);
                    //    return d >= fechaIni && d <= fechaFin;
                    //});
                    var desglose = nombramiento.DesgloseSalarial.Where(D => D.IndPeriodo >= fechaIni && D.IndPeriodo <= fechaFin);


                    var detalleDesglose = entidadBase.DetalleDesgloseSalarial
                                           .Include("DesgloseSalarial")
                                           .Where(D => D.DesgloseSalarial.Nombramiento.PK_Nombramiento == nombramiento.PK_Nombramiento).ToList()
                                                   .Where(D => D.DesgloseSalarial.IndPeriodo >= fechaIni && D.DesgloseSalarial.IndPeriodo <= fechaFin);

                    if (desglose.Count() != 1)
                    {
                        throw new Exception("Fecha en conflicto. Es posible que el rango de fechas incluya dos salarios distintos.");
                    }

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleDesglose.Sum(D => D.MtoPagocomponenteSalarial) //desglose.FirstOrDefault().MtoTotal
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el nombramiento");
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
        /// Metodo encargado de listar pagos anteriores mediante cedula
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<ViaticoCorrido> ListarPagoMesesAnteriores(string cedula)
        {
            List<ViaticoCorrido> resultado = new List<ViaticoCorrido>();

            try
            {
                var datosViatico = entidadBase.ViaticoCorrido
                                                .Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && C.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3)
                                                .ToList();
                if (datosViatico != null)
                {
                    resultado = datosViatico;
                    return resultado.OrderByDescending(Q => Q.PK_ViaticoCorrido).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron Pagos anteriores.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        /// <summary>
        /// metodo encargado de optener un solo viatico corrido
        /// </summary>
        /// <param name="codViaticoC"></param>
        /// <returns></returns>
        //Codigo tipo: yyyy-secuencial
        public CRespuestaDTO ObtenerViaticoCorridoActual(string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(cedula);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var viaticoCorrido = entidadBase.ViaticoCorrido
                                                .Include("EstadoViaticoCorrido")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.CartasPresentacion")
                                                .Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.RelPuestoUbicacion")
                                                .Include("MovimientoViaticoCorrido")
                                                .Include("MovimientoViaticoCorrido.DetalleDeduccionViaticoCorrido")
                                                .Include("PagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido")
                                                .Include("PagoViaticoCorrido.DetallePagoViaticoCorrido.TipoDetallePagoViatico")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo.DetallePagoViaticoRetroactivo")
                                                .Include("PagoViaticoCorrido.PagoViaticoRetroactivo.DetallePagoViaticoRetroactivo.TipoDetallePagoViatico")
                                                .Include("ViaticoCorridoReintegroDias")
                                                .Where(D => D.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3)
                                                .OrderByDescending(D=> D.FecFinViaticoCorrido)
                                                .FirstOrDefault();

                if (viaticoCorrido != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoCorrido,
                        Mensaje = GenerarCodigoInforme(viaticoCorrido)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Viático Corrido");
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
        /// Modificar en la bd la fecha de contrato y el numero de documento de un viatico corrido
        /// </summary>
        /// <param name="idviatico">PK del viatico corrido</param>
        /// <param name="feccontrato">El nuevo valor de la fecha contrato del viatico</param>
        /// <param name="numdocumento">El nuevo valor del numero de documento del viatico</param>
        /// <returns></returns>
        public CRespuestaDTO ModificarFecContratoNumDocVC(int idviatico, DateTime feccontrato, string numdocumento)
        {
            CRespuestaDTO respuesta;
            try
            {
                var ViaticoCorridoOld = entidadBase.ViaticoCorrido.Where(V => V.PK_ViaticoCorrido == idviatico).FirstOrDefault();
                if (ViaticoCorridoOld != null)
                {
                    ViaticoCorridoOld.FecContrato = feccontrato;
                    ViaticoCorridoOld.NumDocumento = numdocumento;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(ViaticoCorridoOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Viatico Corrido requerido");
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

        public CRespuestaDTO ModificarPagoReservaRecursoVC(int idPago, string reserva)
        {
            CRespuestaDTO respuesta;
            try
            {
                var ViaticoCorridoOld = entidadBase.PagoViaticoCorrido.Where(V => V.PK_PagoViaticoCorrido == idPago).FirstOrDefault();
                if (ViaticoCorridoOld != null)
                {
                    ViaticoCorridoOld.ReservaRecurso = reserva;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ViaticoCorridoOld.PK_PagoViaticoCorrido
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

        public CRespuestaDTO AgregarCatalogoDia(CatDiaViaticoGasto registro)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.CatDiaViaticoGasto.Where(F => F.FecDia.Day == registro.FecDia.Day && F.FecDia.Month == registro.FecDia.Month && F.FecDia.Year == registro.FecDia.Year).FirstOrDefault();

                if (dato == null) //si no existe el día registrado
                {
                    entidadBase.CatDiaViaticoGasto.Add(registro);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_CatalogoDia
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Ya está registrada la Fecha");
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

        public CRespuestaDTO ListarCatalogoDia()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.CatDiaViaticoGasto
                                                .Include("TipoDia")
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
                    throw new Exception("No se encontraron registros");
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

        public CRespuestaDTO ListarCatalogoDia(int mes, int anio)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.CatDiaViaticoGasto
                                                .Include("TipoDia")
                                                .Where(P => P.FecDia.Month >= mes &&
                                                            P.FecDia.Year == anio)
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
                    throw new Exception("No se encontraron registros");
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

        #endregion
    }
}

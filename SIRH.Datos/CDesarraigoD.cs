using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Datos
{
    public class CDesarraigoD
    {
        #region Variables

        //versión para Orlando

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDesarraigoD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        
        private bool FormacionAcademicaValida(Funcionario fun)
        {
            return true; // mientras ingresamos datos
        }

        private string GenerarCodigoInforme(Desarraigo desarraigo)
        {

            var annio = desarraigo.FecInicDesarraigo.Year;
            var datos = entidadesBase.Desarraigo.Where(D => D.FecInicDesarraigo.Year == annio).ToList();
            var secuencia = datos.IndexOf(desarraigo) + 1;
            return annio + "-" + secuencia;
        }

        private List<object> DesarraigosCodigo(List<Desarraigo> datos) {
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

        private List<Desarraigo> FiltrarDesarraigo(object[] parametros)
        {
            var datos = entidadesBase.Desarraigo.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoDesarraigo")
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
                    case "NumDesarraigo":
                        var datosBusqueda = parametros[i + 1].ToString().Split('-'); // [año,consecutivo]
                        var annio = int.Parse(datosBusqueda[0]);
                        var consecutivo = int.Parse(datosBusqueda[1]);
                        var desarraigo = datos.Where(D => D.FecInicDesarraigo.Year == annio).ToList().ElementAt(consecutivo - 1);
                        if (desarraigo == null)
                        {
                            throw new Exception("Busqueda no definida");
                        }
                        else
                            datos = datos.Where(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo);
                        break;
                    case "Estado":
                        var estado = parametros[i + 1].ToString();
                        if (estado == "Todas las Notificaciones")
                            datos = datos.Where(D => D.EstadoDesarraigo.NomEstadoDesarraigo != "Valido"
                                                     && D.EstadoDesarraigo.NomEstadoDesarraigo != "Espera"
                                                     && D.EstadoDesarraigo.NomEstadoDesarraigo != "Anulado");
                        else
                            datos = datos.Where(D => D.EstadoDesarraigo.NomEstadoDesarraigo == estado);
                        break;
                    //case "LugarContrato":
                    //    var distrito = ((List<string>)parametros[i + 1]).ElementAt(0);
                    //    var canton = ((List<string>)parametros[i + 1]).ElementAt(1);
                    //    var provincia = ((List<string>)parametros[i + 1]).ElementAt(2);
                    //    datos = datos.Where(D => (distrito == "null" ? true : (D.Distrito.NomDistrito == distrito)) &&
                    //                              (canton == "null" ? true : (D.Distrito.Canton.NomCanton == canton)) &&
                    //                              (provincia == "null" ? true : (D.Distrito.Canton.Provincia.NomProvincia == provincia))).ToList();
                    //    break;
                    case "FechaInicio":
                        var fechaInicioI = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinI = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(D => D.FecInicDesarraigo >= fechaInicioI && D.FecInicDesarraigo <= fechaFinI);
                        break;

                    case "FechaFinal":
                        var fechaInicioF = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(D => D.FecFinDesarraigo >= fechaInicioF && D.FecFinDesarraigo <= fechaFinF);
                        break;
                    default: throw new Exception("Busqueda no definida");
                }
            }
            return datos.ToList();
        }

        public CRespuestaDTO ActualizarVencimientoDesarraigo(DateTime fechaVencimiento)
        {
            CRespuestaDTO respuesta;
            List<Desarraigo> DesarraigosVencido = new List<Desarraigo>();
            try
            {
                var datosDesarraigo = entidadesBase.Desarraigo.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoDesarraigo")
                                        .Where(D => D.FecFinDesarraigo <= fechaVencimiento &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 3 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 4 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 5 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 6 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 7).ToList();
                var datosDesarraigoEspecial = entidadesBase.Desarraigo.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoDesarraigo")
                    .Where(D => D.EstadoDesarraigo.PK_EstadoDesarraigo != 3 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 4 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 5 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 6 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 7
                                                     && D.FecFinDesarraigo > fechaVencimiento).ToList();

                if (datosDesarraigo.Count > 0 || datosDesarraigoEspecial.Count > 0)
                {
                    var estadoVencido = entidadesBase.EstadoDesarraigo.FirstOrDefault(E => E.PK_EstadoDesarraigo == 4);
                    var estadoPps = entidadesBase.EstadoDesarraigo.FirstOrDefault(E => E.PK_EstadoDesarraigo == 5);
                    var estadoVac = entidadesBase.EstadoDesarraigo.FirstOrDefault(E => E.PK_EstadoDesarraigo == 6);
                    var estadoInc = entidadesBase.EstadoDesarraigo.FirstOrDefault(E => E.PK_EstadoDesarraigo == 7);
                    if (datosDesarraigo.Count > 0)
                    {
                        foreach (var item in datosDesarraigo)
                        {
                            item.EstadoDesarraigo = estadoVencido;
                            entidadesBase.SaveChanges();
                            DesarraigosVencido.Add(item);
                        }
                    }
                    if (datosDesarraigoEspecial.Count > 0)
                    {
                        foreach (var item in datosDesarraigoEspecial)
                        {
                            var datosDesarraigoIncapacidad = entidadesBase.RegistroIncapacidad.Where(I => I.FK_Nombramiento == item.FK_Nombramiento && I.FecRige >= item.FecInicDesarraigo).FirstOrDefault();
                            var datosDesarraigoPps = entidadesBase.AccionPersonal.Where(P => P.FK_Nombramiento == item.FK_Nombramiento && P.FK_TipoAccionPersonal == 10 && P.FecRige >= item.FecInicDesarraigo).OrderByDescending(P => P.FecRige).FirstOrDefault();
                            var periodoVacaciones = entidadesBase.PeriodoVacaciones.Where(V => V.FK_Nombramiento == item.FK_Nombramiento).FirstOrDefault();
    
                            if (datosDesarraigoIncapacidad != null &&
                                datosDesarraigoIncapacidad.FecRige.Value.Date.AddDays(30) <= datosDesarraigoIncapacidad.FecVence)
                            {
                                item.EstadoDesarraigo = estadoInc;
                                entidadesBase.SaveChanges();
                                DesarraigosVencido.Add(item);

                            }
                            if (datosDesarraigoPps != null &&
                                datosDesarraigoPps.FecRige.Value.Date.AddDays(30) <= datosDesarraigoPps.FecVence)
                            {
                                item.EstadoDesarraigo = estadoPps;
                                entidadesBase.SaveChanges();
                                DesarraigosVencido.Add(item);
                            }
                            if (periodoVacaciones != null)
                            {
                                var datosVacaciones = entidadesBase.RegistroVacaciones.Where(V => V.FK_PeriodoVacaciones == periodoVacaciones.PK_PeriodoVacaciones).OrderBy(V => V.FecInicio).ToList();
                                var vacaciones = 0.0m;
                                for (int i = 0; i < datosVacaciones.Count; i++)
                                {
                                    if (datosVacaciones[i].IndEstado == 2 && datosVacaciones[i + 1].IndEstado == 2)
                                    {
                                        vacaciones += datosVacaciones[i].CntDias.Value;

                                    }
                                    if (vacaciones >= 30)
                                    {
                                        item.EstadoDesarraigo = estadoVac;
                                        entidadesBase.SaveChanges();
                                        DesarraigosVencido.Add(item);
                                    }
                                    else
                                    {
                                        vacaciones = 0;
                                    }
                                }
                            }
                        }

                    }
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = DesarraigosCodigo(DesarraigosVencido)
                    };
                    return respuesta;
                }

                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay desarraigos cuyo vencimiento se alcance el día de hoy" }
                    };

                    return respuesta;
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO AgregarDesarraigo(Funcionario funcionario, Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                var nombramiento = entidadesBase.Nombramiento.Include("Funcionario").Include("Desarraigo").Include("Desarraigo.EstadoDesarraigo")
                                            .Include("Funcionario.EstadoFuncionario").Include("EstadoNombramiento")
                                            .Where(N=> N.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario
                                                        && listaEstados.Contains(N.FK_EstadoNombramiento) && N.PK_Nombramiento == desarraigo.FK_Nombramiento)
                                            .FirstOrDefault();
                if (nombramiento != null)
                {
                    var desarraigoActivos = nombramiento.Desarraigo.Count(D => D.EstadoDesarraigo.PK_EstadoDesarraigo == 1);
                    var desarraigoEspera = nombramiento.Desarraigo.Count(D => D.EstadoDesarraigo.PK_EstadoDesarraigo == 2);
                    //si no hay desarraigos activos
                    if (desarraigoActivos == 0 && desarraigoEspera==0) 
                    {
                        //si el funcionario cumple con la escolaridad necesaria
                        if (FormacionAcademicaValida(nombramiento.Funcionario))
                        {
                            //Servidor Activo 
                            if (nombramiento.Funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1)
                            {
                                entidadesBase.Desarraigo.Add(desarraigo);
                                entidadesBase.SaveChanges();
                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = GenerarCodigoInforme(desarraigo),
                                    IdEntidad = desarraigo.PK_Desarraigo
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("El funcionario no tiene el estado válido.");
                            }
                        }
                        else
                        {
                            throw new Exception("No se cuenta con Licenciatura como mínimo");
                        }
                    }
                    else
                    {
                        throw new Exception("Ya existe un desarraigo registrado");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el nombramiento válido para el funcionario");
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

        public CRespuestaDTO ModificarDesarraigo(Desarraigo desarraigo) {
            CRespuestaDTO respuesta;
            try {
                var desarraigoOld = entidadesBase.Desarraigo.Include("EstadoDesarraigo").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo);
                if (desarraigoOld != null)
                {
                    if (desarraigoOld.EstadoDesarraigo.PK_EstadoDesarraigo == 2) // "Espera"
                    {
                        desarraigoOld.FK_Presupuesto = desarraigo.FK_Presupuesto;
                        if (desarraigo.ImgDocumento != null)
                            desarraigoOld.ImgDocumento = desarraigo.ImgDocumento;
                        entidadesBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = GenerarCodigoInforme(desarraigoOld)
                        };
                        return respuesta;
                    }
                    else {
                        throw new Exception("El desarraigo no es modificable");
                    }
                }
                else {
                    throw new Exception("No se encontró el desarraigo requerido");
                }
            }catch(Exception error){
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ModificarEstadoDesarraigo(Desarraigo desarraigo, EstadoDesarraigo estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var DesarraigoOld = entidadesBase.Desarraigo.Include("EstadoDesarraigo").Include("Nombramiento")
                                                 .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo);
                if (DesarraigoOld != null)
                {
                    DesarraigoOld.FK_EstadoDesarraigo = estado.PK_EstadoDesarraigo;
                    DesarraigoOld.ObsEstado = desarraigo.ObsEstado;
                    DesarraigoOld.IdRegistradoPor = desarraigo.IdRegistradoPor;
                    DesarraigoOld.IdAnalizadoPor = desarraigo.IdAnalizadoPor;
                    DesarraigoOld.IdRevisadoPor = desarraigo.IdRevisadoPor;
                    DesarraigoOld.IdAprobadoPor = desarraigo.IdAprobadoPor;
                    entidadesBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(DesarraigoOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Desarraigo requerido");
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

        public CRespuestaDTO DesarraigoPorVencer(DateTime fechaVencimiento) {
            CRespuestaDTO respuesta;
            try {
                DateTime fechaQuince = fechaVencimiento.Date.AddDays(15);
                DateTime fechaTres = fechaVencimiento.Date.AddDays(30);

                var datosDesarraigo = entidadesBase.Desarraigo.Include("Nombramiento").Include("Nombramiento.Funcionario").Include("EstadoDesarraigo")
                                                   .Where(D => D.FecFinDesarraigo <= fechaVencimiento &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 3 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 4 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 5 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 6 &&
                                                     D.EstadoDesarraigo.PK_EstadoDesarraigo != 7).Where(D => D.FecFinDesarraigo == fechaQuince || D.FecFinDesarraigo == fechaTres).ToList();

                if (datosDesarraigo.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = DesarraigosCodigo(datosDesarraigo)
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay desarraigos cuyo vencimiento este entre 15 o 30 días" }
                    };

                    return respuesta;
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1,MensajeError= error.Message}
                };
                return respuesta;
            }
        }

        //Codigo tipo: yyyy-secuencial
        public CRespuestaDTO ObtenerDesarraigo(int idDesarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var desarraigo = entidadesBase.Desarraigo
                                                .Where(D => D.PK_Desarraigo == idDesarraigo)
                                                .FirstOrDefault();
                                           
                if (desarraigo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = desarraigo,
                        Mensaje = GenerarCodigoInforme(desarraigo)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el desarraigo");
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

        public CRespuestaDTO ListarDesarraigo()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.Desarraigo.ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = DesarraigosCodigo(datosEntidad)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró desarraigos");
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

        public CRespuestaDTO BuscarDesarraigo(List<Desarraigo> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;
            try
            {
                
                var datos = CargarDatos(elemento, datosPrevios, parametro);
                if (datos.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos //DesarraigosCodigo(datos)
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

        public CRespuestaDTO AnularDesarraigo(Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var desarraigoOld = entidadesBase.Desarraigo.Include("EstadoDesarraigo")
                                                 .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo);
                if (desarraigoOld != null)
                {
                    desarraigoOld.EstadoDesarraigo = entidadesBase.EstadoDesarraigo
                                                                  .FirstOrDefault(E => E.NomEstadoDesarraigo == "Anulado");
                    desarraigoOld.ObsEstado = desarraigo.ObsEstado;
                    entidadesBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = desarraigo.PK_Desarraigo
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el desarraigo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1,MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        //No es necesario, se puede usar el modificar
        public CRespuestaDTO RetomarDesarraigo(Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var desarraigoOld = entidadesBase.Desarraigo.Include("EstadoDesarraigo")
                                                 .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo 
                                                                      && D.EstadoDesarraigo.PK_EstadoDesarraigo == 2);
                if (desarraigoOld != null)
                {
                    desarraigoOld.EstadoDesarraigo = entidadesBase.EstadoDesarraigo
                                                                  .FirstOrDefault(E => E.NomEstadoDesarraigo == "Valido");
                    entidadesBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = GenerarCodigoInforme(desarraigoOld)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el desarraigo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo=-1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerMontoRetroactivo(string cedula, List<DateTime> fecha)
        {
            CRespuestaDTO respuesta;
            try
            {

                var fechaIni = fecha[0];
                var fechaFin = fecha[1];
                var desglose = entidadesBase.DesgloseSalarial.Where(D => D.IndPeriodo >= fechaIni && D.IndPeriodo <= fechaFin && D.Nombramiento.Funcionario.IdCedulaFuncionario.Contains(cedula)).ToList();

                //var detalleDesglose = entidadesBase.DetalleDesgloseSalarial
                //                       .Include("DesgloseSalarial")
                //                       .Where(D => D.DesgloseSalarial.Nombramiento.PK_Nombramiento == nombramiento.PK_Nombramiento).ToList()
                //                               .Where(D => D.DesgloseSalarial.IndPeriodo >= fechaIni && D.DesgloseSalarial.IndPeriodo <= fechaFin);
                if (desglose.Count() == 0)
                {
                    throw new Exception("No existe desglose de salario en el rango de fechas.");
                }
                else
                    if (desglose.Count() < 1)
                {
                    throw new Exception("Fecha en conflicto. Es posible que el rango de fechas incluya dos salarios distintos.");
                }

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = desglose.Sum(Q => Q.DetalleDesgloseSalarial.Sum(D => D.MtoPagocomponenteSalarial)) // desglose.FirstOrDefault().MtoTotal
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo=-1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        private List<Desarraigo> CargarDatos(string elemento, List<Desarraigo> datosPrevios, object parametro)
        {
            string param = "";
            int iParam = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString();
            }
            else
            {
                if (parametro.GetType() == typeof(int))
                {
                    iParam = Convert.ToInt32(parametro);
                }
                else
                {
                    paramFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                    paramFechaFinal = ((List<DateTime>)parametro).ElementAt(1);
                }
            }

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = entidadesBase.Desarraigo
                                                    .Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario
                                                        == param).ToList();
                        break;
                    case "Fechas":
                        datosPrevios = entidadesBase.Desarraigo
                                                    .Where(C => C.FecInicDesarraigo >= paramFechaInicio &&
                                                        C.FecFinDesarraigo <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "Estado":
                        if (iParam == -1) // No mostrar las Anuladas
                            datosPrevios = entidadesBase.Desarraigo
                                                    .Where(C => C.EstadoDesarraigo.PK_EstadoDesarraigo != 6).ToList();
                        else
                            datosPrevios = entidadesBase.Desarraigo
                                                   .Where(C => C.EstadoDesarraigo.PK_EstadoDesarraigo == iParam).ToList();

                        break;
                    case "Trabajo":
                        datosPrevios = entidadesBase.Desarraigo
                                                    .Where(C => C.FK_DistritoTrabajo == iParam).ToList();
                        break;
                    default:
                        datosPrevios = new List<Desarraigo>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario
                                                                == param).ToList();
                        break;
                    case "Fechas":
                        datosPrevios = datosPrevios.Where(C => C.FecInicDesarraigo >= paramFechaInicio &&
                                                               C.FecFinDesarraigo <= paramFechaFinal).ToList();
                        break;
                    case "Estado":
                        if (iParam == -1) // No mostrar las Anuladas
                            datosPrevios = datosPrevios.Where(C => C.EstadoDesarraigo.PK_EstadoDesarraigo != 6).ToList();
                        else
                            datosPrevios = datosPrevios.Where(C => C.EstadoDesarraigo.PK_EstadoDesarraigo == iParam).ToList();
                        break;
                    case "Trabajo":
                        datosPrevios = datosPrevios.Where(C => C.FK_DistritoTrabajo == iParam).ToList();
                        break;
                    default:
                        datosPrevios = new List<Desarraigo>();
                        break;
                }
            }

            return datosPrevios;
        }


        public Funcionario ObtenerFuncionarioPorID(int idFuncionario)
        {
            return entidadesBase.Funcionario.Where(D => D.PK_Funcionario == idFuncionario).FirstOrDefault();
        }

        //public CRespuestaDTO ObtenerMontoRetroactivo(string cedula, List<DateTime> fecha)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        var fechaIni = fecha[0];
        //        var fechaFin = fecha[1];

        //        var desglose = entidadesBase.DetalleDesgloseSalarial
        //                                        .Include("DesgloseSalarial")
        //                                        .Include("DesgloseSalarial.Nombramiento")   
        //                                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
        //                                        .Where(Q => Q.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula 
        //                                                && Q.DesgloseSalarial.IndPeriodo >= fechaIni && Q.DesgloseSalarial.IndPeriodo <= fechaFin)
        //                                        .ToList();
        //        if (desglose.Count() == 0)
        //        {
        //            throw new Exception("No existe desglose de salario en el rango de fechas.");
        //        }
        //        else
        //            if (desglose.Count() < 1)
        //        {
        //            throw new Exception("Fecha en conflicto. Es posible que el rango de fechas incluya dos salarios distintos.");
        //        }

        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = desglose.Sum(Q => Q.MtoPagocomponenteSalarial)
        //        };
        //        return respuesta;

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

        #endregion
    }
}

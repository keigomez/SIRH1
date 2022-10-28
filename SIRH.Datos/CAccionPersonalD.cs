using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;
using System.Globalization;
using System.Data.Odbc;
using System.Data.Objects.DataClasses;

namespace SIRH.Datos
{
    public class CAccionPersonalD
    {

        #region Variables

        /// <summary>
        /// Contexto de la entidad funcionario
        /// </summary>
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase Borrador Acción de Personal
        /// </summary>
        /// <param name="entidadGlobal"></param>
        public CAccionPersonalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        
        public CRespuestaDTO GuardarAccion(AccionPersonal accion, DetalleAccionPersonal detalle, int indClase, int indEscala)
        {
            CRespuestaDTO respuesta = null;

           try
           {
                var numAccion = entidadBase.USP_INSERTAR_ACCION_PERSONAL(accion.AnioRige, accion.FecRige, accion.FecVence, accion.FecRigeIntegra, accion.FecVenceIntegra, 
                                            accion.TipoAccionPersonal.PK_TipoAccionPersonal, accion.Nombramiento.PK_Nombramiento, 
                                            accion.Observaciones, accion.CodModulo, accion.CodObjetoEntidad, accion.IndDato).ToList();

                if (detalle != null)
                {
                    //string numA = numAccion[0].ToString();
                    //detalle.AccionPersonal = entidadBase.AccionPersonal.Where(Q => Q.NumAccion == numA).FirstOrDefault();
                    //entidadBase.DetalleAccionPersonal.Add(detalle);
                    //entidadBase.SaveChanges();

                    entidadBase.USP_INSERTAR_ACCION_PERSONAL_DETALLE(numAccion[0].ToString(), accion.Nombramiento.PK_Nombramiento, indClase, indEscala, detalle.CodPrograma, detalle.CodSeccion,
                                    detalle.CodEspecialidad, detalle.CodSubespecialidad, detalle.NumHoras, detalle.NumAnualidad, detalle.MtoRecargo, detalle.MtoGradoGrupo, detalle.PorProhibicion, detalle.MtoOtros);
                }


                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = numAccion[0].ToString()
                };
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message + " " + (error.InnerException != null ? error.InnerException.Message : "") + " Datos"
                    }
                };
            }
            return respuesta;
        }


        //public CRespuestaDTO GuardarDetalleRubro(int indDetallePuesto, int indComponente, decimal Valor)
        //{
        //    CRespuestaDTO respuesta = null;

        //    try
        //    {
        //        var detalle = entidadBase.DetallePuestoRubro.Where(Q => Q.FK_DetallePuesto == indDetallePuesto && Q.FK_ComponenteSalarial == indComponente).FirstOrDefault();

        //        if (detalle == null)
        //        {
        //            // Agregar
        //            DetallePuestoRubro registro = new DetallePuestoRubro
        //            {
        //                FK_DetallePuesto = indDetallePuesto,
        //                FK_ComponenteSalarial = indComponente,
        //                PorValor = Valor
        //            };
        //            entidadBase.DetallePuestoRubro.Add(registro);
        //            entidadBase.SaveChanges();
        //        }
        //        else
        //        {
        //            // Actualizar
        //            detalle.FK_DetallePuesto = indDetallePuesto;
        //            detalle.FK_ComponenteSalarial = indComponente;
        //            detalle.PorValor = Valor;
        //        }


        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = indDetallePuesto
        //        };
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO
        //            {
        //                Mensaje = error.Message
        //            }
        //        };
        //    }
        //    return respuesta;
        //}
        public CRespuestaDTO GuardarDetalleRubro(DetallePuestoRubro rubro)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                entidadBase.DetallePuestoRubro.Add(rubro);
                entidadBase.SaveChanges();

                //var detalle = entidadBase.DetallePuestoRubro.Where(Q => Q.FK_DetallePuesto == indDetallePuesto && Q.FK_ComponenteSalarial == rubro.FK_ComponenteSalarial).FirstOrDefault();

                //if (detalle == null)
                //{
                //    // Agregar
                //    DetallePuestoRubro registro = new DetallePuestoRubro
                //    {
                //        FK_DetallePuesto = indDetallePuesto,
                //        FK_ComponenteSalarial = indComponente,
                //        PorValor = Valor
                //    };
                //    entidadBase.DetallePuestoRubro.Add(registro);
                //    entidadBase.SaveChanges();
                //}
                //else
                //{
                //    // Actualizar
                //    detalle.FK_DetallePuesto = indDetallePuesto;
                //    detalle.FK_ComponenteSalarial = indComponente;
                //    detalle.PorValor = Valor;
                //}


                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = rubro.PK_DetallePuestoRubro
                };
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ObtenerAccion(string numAccion)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.AccionPersonal
                                            .Include("EstadoBorrador")
                                            .Include("DetalleAccionPersonal")
                                            .Include("DetalleAccionPersonalAnterior")
                                            .Include("TipoAccionPersonal")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.DetalleContratacion")
                                            .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                            .Include("Nombramiento.Puesto")
                                            .Where(R => R.NumAccion == numAccion).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontró la Acción de Personal"
                    };
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

        public CRespuestaDTO ObtenerDetalle(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetalleAccionPersonal
                                            .Include("AccionPersonal")
                                            .Include("DetallePuesto")
                                            .Include("DetallePuesto.Clase")
                                            .Include("DetallePuesto.EscalaSalarial")
                                            .Include("DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                            .Where(R => R.AccionPersonal.PK_AccionPersonal == codigo).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Detalle de la Acción de Personal"
                    };
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

        public CRespuestaDTO ObtenerDetalleAnterior(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetalleAccionPersonalAnterior
                                            .Include("AccionPersonal")
                                            .Where(R => R.AccionPersonal.PK_AccionPersonal == codigo).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Detalle de la Acción de Personal"
                    };
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

        public CRespuestaDTO ObtenerAccionProrroga(int idTipo, string numCedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.USP_OBTENER_ACCION_PERSONAL_PRORROGA(idTipo, numCedula).ToList();

                if (registro != null)
                {
                    CAccionPersonalDTO ap = new CAccionPersonalDTO
                    {
                        IdEntidad = Convert.ToInt32(registro[0].PK_AccionPersonal),
                        NumAccion = registro[0].NumAccion,
                        FecRige = Convert.ToDateTime(registro[0].FecRige),
                        FecVence = Convert.ToDateTime(registro[0].FecVence),
                        CodigoObjetoEntidad = Convert.ToInt32(registro[0].CodObjetoEntidad)
                    };

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ap
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                    };
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

        public CRespuestaDTO AnularAccion(AccionPersonal accion)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                var numAccion = entidadBase.USP_ANULAR_ACCION_PERSONAL(accion.PK_AccionPersonal, accion.Observaciones).ToList();

                if (numAccion[0] != "-1")
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = numAccion[0].ToString()
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO
                        {
                            Mensaje = "No se puede anular la acción de Personal, porque no es la Acción más reciente para el mismo tipo"
                        }
                    };
                }    
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO AnularAccionModulo(AccionPersonal accion)
        {
            CRespuestaDTO respuesta;

            try
            {
                var accionOld = entidadBase.AccionPersonal
                                            //.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                            .Where(I => I.CodModulo == accion.CodModulo && I.CodObjetoEntidad == accion.CodObjetoEntidad && 
                                                   I.TipoAccionPersonal.PK_TipoAccionPersonal == accion.TipoAccionPersonal.PK_TipoAccionPersonal)
                                            .FirstOrDefault();

                if (accionOld != null)
                {
                    accionOld.EstadoBorrador = entidadBase.EstadoBorrador.Where(q => q.PK_EstadoBorrador == 8).FirstOrDefault(); // Anulada
                    accionOld.Observaciones = accion.Observaciones;

                    respuesta = AnularAccion(accionOld);
                    //accion = accionOld;
                    //entidadBase.SaveChanges();

                    //respuesta = new CRespuestaDTO
                    //{
                    //    Codigo = 1,
                    //    Contenido = accion.PK_AccionPersonal
                    //};

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna acción de personal con el código especificado." }
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

        public CRespuestaDTO ModificarObservaciones(AccionPersonal accion)
        {
            CRespuestaDTO respuesta;

            try
            {
                var accionOld = entidadBase.AccionPersonal
                                            .Where(I => I.NumAccion == accion.NumAccion)
                                            .FirstOrDefault();

                if (accionOld != null)
                {
                    accionOld.Observaciones = accion.Observaciones;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = accionOld.NumAccion
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna acción de personal con el código especificado." }
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

        public CRespuestaDTO AprobarAccion(AccionPersonal accion)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                var accionOld = entidadBase.AccionPersonal.Where(Q => Q.PK_AccionPersonal == accion.PK_AccionPersonal).FirstOrDefault();

                if (accionOld != null)
                {
                    accionOld.FK_Estado = 8; // Aprobado
                    accion = accionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = accion.PK_AccionPersonal
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO
                        {
                            Mensaje = "No se puede aprobar la acción de Personal"
                        }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO BuscarAccion(CFuncionarioDTO funcionario, CPuestoDTO puesto, CAccionPersonalDTO accion, List<DateTime> fechas)
        {
            CRespuestaDTO respuesta;

            try
            {
                DateTime paramFechaInicioR = new DateTime();
                DateTime paramFechaFinalR = new DateTime();
                DateTime paramFechaInicioV = new DateTime();
                DateTime paramFechaFinalV = new DateTime();

                bool condicionFechaRige = false;
                bool condicionFechaVence = false;
                bool condicionTipo = false;
                bool condicionNumAccion = false;
                bool condicionPuesto = false;
                bool condicionFuncionario = false;
                bool condicionEstado = false;

                List<AccionPersonal> resultado = new List<AccionPersonal>();

                //if (fechas.Count > 0)
                //{
                //    paramFechaInicio = fechas.ElementAt(0);
                //    paramFechaFinal = fechas.ElementAt(1);
                //    condicionFecha = true;
                //}

                // Fecha Rige
                if (fechas[0].Year > 1)
                {
                    paramFechaInicioR = fechas.ElementAt(0);
                    paramFechaFinalR = fechas.ElementAt(1);
                    condicionFechaRige = true;
                }

                // Fecha Vence
                if (fechas[2].Year > 1)
                {
                    paramFechaInicioV = fechas.ElementAt(2);
                    paramFechaFinalV = fechas.ElementAt(3);
                    condicionFechaVence = true;
                }

                

                if (accion.TipoAccion != null)
                    condicionTipo = accion.TipoAccion.IdEntidad > 0;

                if (funcionario.Cedula != null)
                    condicionFuncionario = funcionario.Cedula != "";

                if (accion.NumAccion != null)
                    condicionNumAccion = accion.NumAccion != "";

                if (puesto != null)
                    if (puesto.CodPuesto != null)
                            condicionPuesto = puesto.CodPuesto != "";

                if (accion.Estado != null)
                    condicionEstado = true;


                // Filtrar
                if (condicionFuncionario)
                    resultado = entidadBase.AccionPersonal.Where(q => q.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula).ToList();

                if (condicionTipo) { 
                    if(resultado.Count > 0)
                        resultado = resultado.Where(q => q.TipoAccionPersonal.PK_TipoAccionPersonal == accion.TipoAccion.IdEntidad).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.TipoAccionPersonal.PK_TipoAccionPersonal == accion.TipoAccion.IdEntidad).ToList();
                }

                if (condicionNumAccion) { 
                    if (resultado.Count > 0)
                        resultado = resultado.Where(q => q.NumAccion == accion.NumAccion).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.NumAccion == accion.NumAccion).ToList();
                }

                if (condicionFechaRige) { 
                    if (resultado.Count > 0)
                        resultado = resultado.Where(q => q.FecRige >= paramFechaInicioR && q.FecRige <= paramFechaFinalR).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.FecRige >= paramFechaInicioR && q.FecRige <= paramFechaFinalR).ToList();
                }

                if (condicionFechaVence) { 
                    if (resultado.Count > 0)
                        resultado = resultado.Where(q => q.FecVence >= paramFechaInicioV && q.FecVence <= paramFechaFinalV).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.FecVence >= paramFechaInicioV && q.FecVence <= paramFechaFinalV).ToList();
                }

                if (condicionPuesto) { 
                    if (resultado.Count > 0)
                        resultado = resultado.Where(q => q.Nombramiento.Puesto.CodPuesto.TrimEnd() == puesto.CodPuesto.TrimEnd()).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.Nombramiento.Puesto.CodPuesto.TrimEnd() == puesto.CodPuesto.TrimEnd()).ToList();
                }

                if (condicionEstado) {
                    if (resultado.Count > 0)
                        resultado = resultado.Where(q => q.FK_Estado == accion.Estado.IdEntidad).ToList();
                    else
                        resultado = entidadBase.AccionPersonal.Where(q => q.FK_Estado == accion.Estado.IdEntidad).ToList();
                }
                else if (resultado.Count > 0) { 
                    resultado = resultado.Where(q => q.FK_Estado != 8).ToList();  // 8- Anulado
                }


                if (resultado != null)
                {
                    if (resultado.Count() > 0)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                    }
                    else
                    {
                        throw new Exception("No se encontraron resultados");
                    }
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                    };
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

        public CRespuestaDTO BuscarAccionProrroga(string tipoAccion, List<DateTime> fechas)
        {
            CRespuestaDTO respuesta;

            try
            {
                var listado = new List<int> {0};
                //2   Cese de funciones
                //3   Cese de interinidad
                //4   Despido por causa
                //12  Renuncia.
                //25  Regreso al trabajo.
                //80  Regreso al puesto en propiedad
                var listadoNoProrrogar = new List<int> { 2, 3, 4, 12, 25, 80 };
                var listaAcciones = new List<AccionPersonal>();

                switch (tipoAccion)
                {
                    case "6":
                        // 6	Permiso con sueldo total
                        // 9	Prórroga de permiso con salario.
                        listado = new List<int> { 6, 9 };
                        break;

                    case "7":
                        // 7	Permiso sin salario
                        // 10   Prórroga de permiso sin salario.
                        listado = new List<int> { 7, 10 };
                        break;

                    case "22":
                        // 22  Nomb.Interino.
                        // 23  Prórroga de nombramiento.
                        listado = new List<int> { 22, 23 };
                        break;

                    case "31":
                        // 31  Ascenso interino.
                        // 48  Prórroga de ascen. Interino.
                        listado = new List<int> { 31, 48 };
                        break;

                    case "57":
                        // 57  Traslado.
                        listado = new List<int> { 57 };
                        break;

                    case "58":
                        // 52  Prórroga de traslado interino.
                        // 58  Traslado interino.
                        listado = new List<int> { 52, 58 };
                        break;
                    case "78":  //Dedicación Exclusiva
                        // 62  Reajuste aprob. Dedic.Exclusiva.
                        // 78  Prórroga Dedic. Exclusiva.
                        // 94  Addendum Dedicación Exclusiva
                        listado = new List<int> { 62, 78, 94 };
                        break;

                    default:
                        break;
                }

                DateTime paramFechaInicioV = fechas.ElementAt(0);
                DateTime paramFechaFinalV = fechas.ElementAt(1);

                var registro = entidadBase.AccionPersonal
                                            .Include("EstadoBorrador")
                                            .Include("DetalleAccionPersonal")
                                            .Include("TipoAccionPersonal")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.EstadoFuncionario")
                                            .Where(R => R.FK_Estado == 7 && listado.Contains(R.FK_TipoAccionPersonal) &&
                                                        R.Nombramiento.Funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1 &&
                                                        R.FecVence >= paramFechaInicioV  && R.FecVence <= paramFechaFinalV)
                                            .ToList();
                
                if (registro != null)
                {
                    foreach (var item in registro)
                    {
                        // Buscar si existe una acción posterior para la misma persona
                        var dato = entidadBase.AccionPersonal
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Where(R => R.FK_Estado == 7 &&
                                                        (listado.Contains(R.FK_TipoAccionPersonal) || listadoNoProrrogar.Contains(R.FK_TipoAccionPersonal)) &&
                                                        R.PK_AccionPersonal != item.PK_AccionPersonal &&
                                                        R.Nombramiento.Funcionario.PK_Funcionario == item.Nombramiento.Funcionario.PK_Funcionario &&
                                                        R.FecRige > item.FecRige)
                                            .FirstOrDefault();
            
                        // Si no existe, se agrega
                        if (dato == null)
                            listaAcciones.Add(item);
                    }
                }

                // HISTÓRICO

                var resultadoHistorico = entidadBase.C_EMU_AccionPersonal
                                                        .Where(q => listado.Contains(q.CodAccion.Value) && 
                                                               q.FecVence >= paramFechaInicioV && 
                                                               q.FecVence <= paramFechaFinalV)
                                                        .ToList();

                if (resultadoHistorico != null)
                {
                    foreach (var item in resultadoHistorico)
                    {
                        // Buscar si existe una acción posterior para la misma persona
                        var dato = entidadBase.AccionPersonal
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Where(R => R.FK_Estado == 7 &&
                                                        (listado.Contains(R.FK_TipoAccionPersonal) || listadoNoProrrogar.Contains(R.FK_TipoAccionPersonal)) &&
                                                        R.Nombramiento.Funcionario.IdCedulaFuncionario == item. Cedula &&
                                                        R.FecRige >= item.FecRige)
                                            .FirstOrDefault();

                        // Si no existe, se agrega
                        if (dato == null)
                        {
                            var itemAccion = new AccionPersonal
                            {
                                NumAccion = item.NumAccion,
                                FecRige = item.FecRige,
                                FecVence = item.FecVence,
                                Observaciones = item.Explicacion,
                                TipoAccionPersonal = new TipoAccionPersonal { PK_TipoAccionPersonal = Convert.ToInt16(item.CodAccion)},
                                FK_TipoAccionPersonal = Convert.ToInt16(item.CodAccion) ,
                                EstadoBorrador = new EstadoBorrador { PK_EstadoBorrador = 7 },
                                Nombramiento = new Nombramiento
                                {
                                    PK_Nombramiento = 0,
                                    Funcionario = new Funcionario { IdCedulaFuncionario = item.Cedula}
                                }
                            };

                            listaAcciones.Add(itemAccion);
                        }
                    }
                }


                if (listaAcciones.Count > 0)
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = listaAcciones.OrderBy(Q => Q.Nombramiento.Funcionario.NomPrimerApellido)
                                                 .ThenBy(Q => Q.Nombramiento.Funcionario.NomSegundoApellido)
                                                 .ThenBy(Q => Q.Nombramiento.Funcionario.NomFuncionario)
                                                 .ToList()
                    };
                else
                    throw new Exception("No se encontraron registros para esas fecha");

                //else
                //{
                //    respuesta = new CRespuestaDTO
                //    {
                //        Codigo = -1,
                //        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                //    };
                //}
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


        public CRespuestaDTO BuscarHistorial(CAccionPersonalHistoricoDTO accion, List<DateTime> fechas)
        {
            CRespuestaDTO respuesta;

            try
            {
                DateTime paramFechaInicio = new DateTime();
                DateTime paramFechaFinal = new DateTime();
                string strFechaInicio = "";
                string strFechaFinal = "";

                bool condicionFecha = false;
                bool condicionTipo = false;
                bool condicionNumAccion = false;
                bool condicionPuesto = false;
                bool condicionFuncionario = false;
                bool condicionClase = false;
                //bool condicionEspecialidad = false;

                if (fechas.Count > 0)
                {
                    paramFechaInicio = fechas.ElementAt(0);
                    paramFechaFinal = fechas.ElementAt(1);
                    strFechaInicio = paramFechaInicio.Day.ToString().PadLeft(2, '0') + paramFechaInicio.Month.ToString().PadLeft(2, '0') + paramFechaInicio.Year;
                    strFechaFinal = paramFechaFinal.Day.ToString().PadLeft(2, '0') + paramFechaFinal.Month.ToString().PadLeft(2, '0') + paramFechaFinal.Year;
                    condicionFecha = true;
                }

                List<C_EMU_AccionPersonal> resultado = null;  // = entidadBase.C_EMU_AccionPersonal.ToList();

                if (accion.CodAccion != 0)
                    condicionTipo = accion.CodAccion != 0;

                if (accion.Cedula != null)
                    condicionFuncionario = accion.Cedula != "";

                if (accion.NumAccion != null)
                    condicionNumAccion = accion.NumAccion != "";

                if (accion.CodPuesto != null)
                    condicionPuesto = accion.CodPuesto != "";

                if (accion.CodClase != null)
                    condicionClase = accion.CodClase != "";

                // Filtrar


                if (condicionFecha)
                {
                    //var dato = entidadBase.C_EMU_AccionPersonal.ToList().Where(q => DateTime.ParseExact(q.FecRige, "dd-MM-yyyy", CultureInfo.InvariantCulture) >= paramFechaInicio && DateTime.ParseExact(q.FecRige, "dd-MM-yyyy", CultureInfo.InvariantCulture) <= paramFechaFinal);
                    var dato = entidadBase.C_EMU_AccionPersonal.ToList().Where(q => Convert.ToDateTime(q.FecRige) >= paramFechaInicio);
                    resultado = dato.ToList();
                }


                if (condicionFuncionario)
                {
                    if(resultado == null)
                        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => q.Cedula == accion.Cedula).ToList();
                    else
                        resultado = resultado.Where(q => q.Cedula == accion.Cedula).ToList();
                }
                    

                if (condicionTipo)
                {
                    //resultado = entidadBase.C_EMU_AccionPersonal.Where(q => Convert.ToInt16(q.CodAccion) == Convert.ToInt16(accion.CodAccion)).ToList();
                    //string codigo = accion.CodAccion.ToString().PadLeft(2, '0');
                   
                    if (resultado == null)
                        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => q.CodAccion == accion.CodAccion).ToList();
                    else
                        resultado = resultado.Where(q => q.CodAccion == accion.CodAccion).ToList();
                }

                if (condicionNumAccion)
                {
                    if (resultado == null)
                        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => q.NumAccion == accion.NumAccion).ToList();
                    else
                        resultado = resultado.Where(q => q.NumAccion == accion.NumAccion).ToList();
                }

                if (condicionPuesto)
                {
                    if (resultado == null)
                        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => q.NumPuesto1 == accion.CodPuesto).ToList();
                    else
                        resultado = resultado.Where(q => q.NumPuesto1 == accion.CodPuesto).ToList();
                }


                if (condicionClase)
                {
                    string codigo = accion.CodClase.ToString().PadLeft(4, '0');

                    if (resultado == null)
                        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => q.Clase2 == codigo).ToList();
                    else
                        resultado = resultado.Where(q => q.Clase2 == codigo).ToList();
                }

                //if (condicionEspecialidad)
                //{
                //    var listaDetalle = entidadBase.DetallePuesto.Where(Q => Q.FK_Especialidad == 22).Select(Q => Q.FK_Puesto).ToList();
                //    var listaPuesto = entidadBase.Puesto.Where(P => listaDetalle.Contains(P.PK_Puesto)).Select(P => P.CodPuesto).ToList();

                //    if (resultado == null)
                //        resultado = entidadBase.C_EMU_AccionPersonal.Where(q => listaPuesto.Contains(q.NumPuesto1)).ToList();
                //    else
                //        resultado = resultado.Where(q => listaPuesto.Contains(q.NumPuesto1)).ToList();
                //}
                                
            
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado.OrderByDescending(q => q.NumAccion).ToList()
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de las Acciones de Personal"
                    };
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

        public CRespuestaDTO BuscarFuncionarioDetallePuesto(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var listaEstados = new List<int> { 1, 2, 5, 6, 9, 10, 13, 18, 19, 20, 21, 22, 23, 24, 25, 26,27, 28, 30, 33, 35, 36, 37, 38, 39 };

                respuesta.Contenido = entidadBase.Funcionario
                                                   .Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.DetallePuestoRubro")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(F => F.IdCedulaFuncionario == cedula
                                                            && F.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).Count() > 0)
                                                   .FirstOrDefault();
                if ((Funcionario)respuesta.Contenido != null)
                {
                    ((Funcionario)respuesta.Contenido).Nombramiento = ((Funcionario)respuesta.Contenido).Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).ToList();
                    //if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    //{
                    //    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse en el módulo.");
                    //}
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }

        public CRespuestaDTO ObtenerAccionHistorico(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.C_EMU_AccionPersonal.Where(R => R.ID == codigo).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                    };
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

        public CRespuestaDTO ObtenerDetallePuestoNombramiento(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetallePuesto
                                .Where(R => R.FK_Nombramiento == codigo)
                                //.FirstOrDefault();
                                .OrderByDescending(Q=> Q.PK_DetallePuesto).ToList().FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    var nombramiento = entidadBase.Nombramiento.Where(N => N.PK_Nombramiento == codigo).FirstOrDefault();
                    registro = entidadBase.DetallePuesto.Include("Puesto").Include("Puesto.Nombramiento")
                                .Where(R => R.Puesto.PK_Puesto == nombramiento.FK_Puesto).FirstOrDefault();

                    if (registro != null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = registro
                        };
                    }
                    else
                    {

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = "0"
                        };
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

        public CRespuestaDTO ObtenerDetallePuestoCodigo(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetallePuesto
                                .Where(R => R.PK_DetallePuesto == codigo)
                                .FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    throw new Exception("No se encontraron registros del Detalle del Puesto");
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

        public CRespuestaDTO ObtenerPorcentajeComponenteSalarial(CDetallePuestoDTO detalle, int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                //var registro = entidadBase.CambioPorAccionPersonal
                //                .Include("Nombramiento")
                //                //.Include("Nombramiento.Funcionario")
                //                .Include("ComponenteSalarial")
                //                .Where(R => R.Nombramiento.PK_Nombramiento == nombramiento.IdEntidad && R.ComponenteSalarial.PK_ComponenteSalarial == codigo).FirstOrDefault();
                //                //.Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula && R.ComponenteSalarial.PK_ComponenteSalarial == codigo).FirstOrDefault();

                //var registro = entidadBase.DetallePuestoRubro
                //                .Include("DetallePuesto")
                //                .Include("DetallePuesto.Puesto")
                //                .Include("DetallePuesto.Puesto.Nombramiento")
                //                .Where(R => R.DetallePuesto.Puesto.Nombramiento.Where(N => N.PK_Nombramiento == nombramiento.IdEntidad).Count() > 0
                //                            && (R.DetallePuesto.IndEstadoDetallePuesto ?? 1) == 1
                //                            && R.ComponenteSalarial.PK_ComponenteSalarial == codigo).FirstOrDefault();

                var registro = entidadBase.DetallePuestoRubro
                                .Where(R => R.FK_DetallePuesto == detalle.IdEntidad
                                            && R.FK_ComponenteSalarial == codigo).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PorValor
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "0"
                    };
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

        public CRespuestaDTO ObtenerPorcentajeComponenteSalarial(CFuncionarioDTO funcionario, int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                //var registro = entidadBase.CambioPorAccionPersonal
                //                .Include("Nombramiento")
                //                .Include("Nombramiento.Funcionario")
                //                .Include("ComponenteSalarial")
                //                .Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula && R.ComponenteSalarial.PK_ComponenteSalarial == codigo).FirstOrDefault();

                var registro = entidadBase.DetallePuestoRubro
                                .Include("DetallePuesto")
                                .Include("DetallePuesto.Puesto")
                                .Include("DetallePuesto.Puesto.Nombramiento")
                                .Include("DetallePuesto.Puesto.Nombramiento.Funcionario")
                                .Where(R => R.DetallePuesto.Puesto.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == funcionario.Cedula).Count() > 0
                                        &&  (R.DetallePuesto.IndEstadoDetallePuesto ?? 1) == 1
                                        &&  R.ComponenteSalarial.PK_ComponenteSalarial == codigo).FirstOrDefault();


                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PorValor
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "0"
                    };
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

        public CRespuestaDTO ObtenerCategoriaClase(int codClase)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.Clase.Where(C => C.PK_Clase == codClase).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.IndCategoria
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "0"
                    };
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

        public CRespuestaDTO ObtenerSalario(string numCedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registroNombramiento = entidadBase.Nombramiento.Where(N => N.FecVence == null && N.Funcionario.IdCedulaFuncionario == numCedula).ToList().LastOrDefault();
                var registroDetallePuesto = entidadBase.DetallePuesto.Where(D => D.IndEstadoDetallePuesto.HasValue == false || D.IndEstadoDetallePuesto == 1 && 
                                                                            D.Puesto.PK_Puesto == registroNombramiento.FK_Puesto)
                                                                    .FirstOrDefault();

                var registro = entidadBase.DetallePuestoRubro.Where(R => R.DetallePuesto.PK_DetallePuesto == registroDetallePuesto.PK_DetallePuesto).ToList();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                    };
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

        public CRespuestaDTO ObtenerCategoriaReasignacion(int codDetallePuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetalleAccionPersonal
                                        .Where(Q => Q.FK_DetallePuesto == codDetallePuesto 
                                                && Q.AccionPersonal.TipoAccionPersonal.PK_TipoAccionPersonal == 54
                                                && Q.AccionPersonal.FK_Estado == 7)
                                        .FirstOrDefault();

                if (registro != null)
                {
                    // Buscar la Clase
                    var registroDetallePuesto = entidadBase.DetallePuesto
                                                .Where(Q => Q.PK_DetallePuesto == registro.CodDetallePuesto)
                                                .FirstOrDefault();

                    if (registroDetallePuesto != null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = registroDetallePuesto.Clase.IndCategoria
                        };
                    }
                    else
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = "0"
                        };
                    }
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "0"
                    };
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

        public CRespuestaDTO CargarAccionHistorico()
        {
            CRespuestaDTO respuesta;
            try
            {
                List<C_EMU_AccionPersonal> listado = new List<C_EMU_AccionPersonal>();
                C_EMU_AccionPersonal temp;

                int numero = 0;

                var num_accion = "";
                string clase1 = "", clase2 = "";
                string numPuesto2 = "";
                int codigo = 0;
                int categoria1 = 0, categoria2 = 0;
                decimal salario1 = 0, salario2 = 0;
                decimal aumentos1 = 0, aumentos2 = 0; 
                decimal recargo1 = 0, recargo2 = 0;
                decimal grupo1 = 0, grupo2 = 0;
                decimal proh1 = 0, proh2 = 0;
                decimal otros1 = 0, otros2 = 0;
                int disfrutado1 = 0, disfrutado2 = 0;
                int autorizado1 = 0, autorizado2 = 0;

                DateTime? fechaR, fechaV;

               
                var lista = entidadBase.C_EMU_AccionPersonal.Select(Q => Q.NumAccion).ToList();
                
                OdbcConnection DbConnection = new OdbcConnection("Dsn=connx40g;uid=sag;pwd=123");
                DbConnection.Open();
                    
                var strQuery = "SELECT * FROM AASU001M WHERE CEDULA IS NOT NULL";
                strQuery += "  AND NUMERO_PUESTO1 IS NOT NULL";
                strQuery += " AND NUMERO_ACCION IS NOT NULL";
                strQuery += " AND SUBSTRING(NUMERO_ACCION, 1, 4) IN ('2019')"; //('2017', '2018','2019','2020')";
                //strQuery += " AND NUMERO_ACCION IN ('202000374')";

                OdbcDataAdapter adapter = new OdbcDataAdapter(strQuery,DbConnection);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "Acciones");

                foreach (DataRow x in ds.Tables["Acciones"].Rows)
                {
                    // Si no está en la tabla Histórica
                    if (! lista.Contains(x.Field<string>("NUMERO_ACCION").TrimEnd()))
                    {
                        numero += 1;
                        num_accion = x.Field<string>("NUMERO_ACCION"); 
                        try
                        {
                            //var a = entidadBase.USP_VALIDAR_FECHA_RIGE(x.Field<string>("FECHA_RIGE").TrimEnd()).ToList();
                            fechaR = DateTime.Now;//Convert.ToDateTime(a[0]);
                        }
                        catch (Exception X) { fechaR = null; }

                        try {
                            //var a = entidadBase.USP_VALIDAR_FECHA_RIGE(x.Field<string>("FECHA_VENCE").TrimEnd()).ToList();
                            fechaV = DateTime.Now;//Convert.ToDateTime(a[0]);
                        }
                        catch (Exception X) { fechaV = null; }
                        

                        try { codigo = int.Parse(x.Field<string>("CODIGO_ACCION").TrimEnd()); }
                        catch { codigo = 0; }

                        numPuesto2 = x.Field<string>("NUMERO_PUESTO2") != null ? x.Field<string>("NUMERO_PUESTO2").TrimEnd() : ""; 

                        clase1 = x.Field<string>("CLASE_PUESTO1") != null ? x.Field<string>("CLASE_PUESTO1").TrimEnd() : "";
                        clase2 = x.Field<string>("CLASE_PUESTO2") != null ? x.Field<string>("CLASE_PUESTO2").TrimEnd() : "";

                        try { categoria1 = int.Parse(x.Field<string>("CATEGORIA1").TrimEnd()); }
                        catch { categoria1 = 0; }
                        try { categoria2 = int.Parse(x.Field<string>("CATEGORIA2").TrimEnd()); }
                        catch { categoria2 = 0; }

                        try { salario1 = x.Field<decimal>("SALARIO_BASE1"); }
                        catch { salario1 = 0; }
                        try { salario2 = x.Field<decimal>("SALARIO_BASE2"); }
                        catch { salario2 = 0; }

                        try { aumentos1 = x.Field<decimal>("AUMENTOS1"); }
                        catch { aumentos1 = 0; }
                        try { aumentos2 = x.Field<decimal>("AUMENTOS2"); }
                        catch { aumentos2 = 0; }
                        
                        try { recargo1 = x.Field<decimal>("RECARGO_FUNCIONES1"); }
                        catch { recargo1 = 0; }
                        try { recargo2 = x.Field<decimal>("RECARGO_FUNCIONES2"); }
                        catch { recargo2 = 0; }
                        
                        try { grupo1 = x.Field<decimal>("GRUPO_PROFESIONAL1"); }
                        catch { grupo1 = 0; }
                        try { grupo2 = x.Field<decimal>("GRUPO_PROFESIONAL2"); }
                        catch { grupo2 = 0; }
                        
                        try { proh1 = x.Field<decimal>("PROHIBICION1"); }
                        catch { proh1 = 0; }
                        try { proh2 = x.Field<decimal>("PROHIBICION2"); }
                        catch { proh2 = 0; }
                        
                        try { otros1 = x.Field<decimal>("OTROS_SOBRESUELDOS1"); }
                        catch { otros1 = 0; }
                        try { otros2 = x.Field<decimal>("OTROS_SOBRESUELDOS2"); }
                        catch { otros2 = 0; }

                        try { disfrutado1 = int.Parse(x.Field<string>("DISFRUTADO1").TrimEnd()); }
                        catch { disfrutado1 = 0; }
                        try { disfrutado2 = int.Parse(x.Field<string>("DISFRUTADO2").TrimEnd()); }
                        catch { disfrutado2 = 0; }

                        try { autorizado1 = int.Parse(x.Field<string>("AUTORIZADO1").TrimEnd()); }
                        catch { autorizado1 = 0; }
                        try { autorizado2 = int.Parse(x.Field<string>("AUTORIZADO2").TrimEnd()); }
                        catch { autorizado2 = 0; }

                        try
                        {

                            temp = new C_EMU_AccionPersonal
                                {
                                    NumAccion = x.Field<string>("NUMERO_ACCION"),
                                    FecRige = DateTime.Compare(Convert.ToDateTime(fechaR), Convert.ToDateTime("1/1/0001")) != 0 ? fechaR : null,
                                    FecVence = DateTime.Compare(Convert.ToDateTime(fechaV), Convert.ToDateTime("1/1/0001")) != 0 ? fechaV : null,
                                    CodAccion = codigo,
                                    Explicacion = x.Field<string>("EXPLICACION2").TrimEnd(),
                                    Cedula = x.Field<string>("CEDULA"),
                                    NumPuesto1 = x.Field<string>("NUMERO_PUESTO1").TrimEnd(),
                                    NumPuesto2 = numPuesto2,
                                    Categoria1 = categoria1.ToString(),
                                    Categoria2 = categoria2.ToString(),
                                    Clase1 = clase1, // x.Field<string>("CLASE_PUESTO1").TrimEnd(),
                                    Clase2 = clase2,
                                    Salario1 = salario1,
                                    Salario2 = salario2,
                                    Aumentos1 = aumentos1,
                                    Aumentos2 = aumentos2,
                                    Recargo1 = recargo1,
                                    Recargo2 = recargo2,
                                    Grupo1 = grupo1,
                                    Grupo2 = grupo2,
                                    Prohibicion1 = proh1,
                                    Prohibicion2 = proh2,
                                    Otros1 = otros1,
                                    Otros2 = otros2,
                                    Disfrutado1 = disfrutado1,
                                    Disfrutado2 = disfrutado2,
                                    Autorizado1 = autorizado1,
                                    Autorizado2 = autorizado2,
                                    FK_Nombramiento = 0
                                };

                            entidadBase.C_EMU_AccionPersonal.Add(temp);
                            entidadBase.SaveChanges();
                        }
                        catch(Exception error)
                        {
                            var err = error.InnerException;
                        }                     
                    }
                }

                //var query = (from DataRow x in 
                //            //.Where(Q => !listado.Any(C => C.NumAccion == Q.Field<string>("NUMERO_ACCION"))).ToList()
                //             select new 
                //             {
                //                ID = 0,
                //                FK_Nombramiento = 0,
                //                NumAccion = x.Field<string>("NUMERO_ACCION").TrimEnd(),
                //                 FecRige = DateTime.TryParse(x.Field<string>("FECHA_RIGE"), out fecha) ? fecha : fecha,
                //                 FecVence = DateTime.TryParse(x.Field<string>("FECHA_VENCE"), out fecha) ? fecha : fecha,
                //                 CodAccion = int.TryParse(x.Field<string>("CODIGO_ACCION"), out numero) ? numero : 0,
                //                 Explicacion = x.Field<string>("EXPLICACION2").TrimEnd(),
                //                 Cedula = x.Field<string>("CEDULA"),
                //                 NumPuesto1 = x.Field<string>("NUMERO_PUESTO1").TrimEnd(),
                //                 NumPuesto2 = x.Field<string>("NUMERO_PUESTO2").TrimEnd(),
                //                 Categoria1 = (int.TryParse(x.Field<string>("CATEGORIA2").TrimEnd(), out numero) ? numero : 0).ToString(),
                //                 Categoria2 = (int.TryParse(x.Field<string>("CATEGORIA1").TrimEnd(), out numero) ? numero : 0).ToString(),
                //                 Clase1 = x.Field<string>("CLASE_PUESTO1").TrimEnd(),
                //                 Clase2 = x.Field<string>("CLASE_PUESTO2").TrimEnd(),
                //                 Salario1 = decimal.TryParse(x.Field<string>("SALARIO_BASE1"), out monto) ? monto : 0,
                //                 Aumentos1 = decimal.TryParse(x.Field<string>("AUMENTOS1"), out monto) ? monto : 0,
                //                 Recargo1 = decimal.TryParse(x.Field<string>("RECARGO_FUNCIONES1"), out monto) ? monto : 0,
                //                 Grupo1 = decimal.TryParse(x.Field<string>("GRUPO_PROFESIONAL1"), out monto) ? monto : 0,
                //                 Prohibicion1 = decimal.TryParse(x.Field<string>("PROHIBICION1"), out monto) ? monto : 0,
                //                 Otros1 = decimal.TryParse(x.Field<string>("OTROS_SOBRESUELDOS1"), out monto) ? monto : 0,
                //                 Salario2 = decimal.TryParse(x.Field<string>("SALARIO_BASE2"), out monto) ? monto : 0,
                //                 Aumentos2 = decimal.TryParse(x.Field<string>("AUMENTOS2"), out monto) ? monto : 0,
                //                 Recargo2 = decimal.TryParse(x.Field<string>("RECARGO_FUNCIONES2"), out monto) ? monto : 0,
                //                 Grupo2 = decimal.TryParse(x.Field<string>("GRUPO_PROFESIONAL2"), out monto) ? monto : 0,
                //                 Prohibicion2 = decimal.TryParse(x.Field<string>("PROHIBICION2"), out monto) ? monto : 0,
                //                 Otros2 = decimal.TryParse(x.Field<string>("OTROS_SOBRESUELDOS2"), out monto) ? monto : 0,
                //                 Disfrutado1 = int.TryParse(x.Field<string>("DISFRUTADO1"), out numero) ? numero : 0,
                //                 Autorizado1 = int.TryParse(x.Field<string>("AUTORIZADO1"), out numero) ? numero : 0,
                //                 Disfrutado2 = int.TryParse(x.Field<string>("DISFRUTADO2"), out numero) ? numero : 0,
                //                 Autorizado2 = int.TryParse(x.Field<string>("AUTORIZADO2"), out numero) ? numero : 0
                //             });

                DbConnection.Close();

               
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = numero
                };


                //var registro = numero;

                //if (registro != null)
                //{
                //    respuesta = new CRespuestaDTO
                //    {
                //        Codigo = 1,
                //        Contenido = registro
                //    };
                //}
                //else
                //{
                //    respuesta = new CRespuestaDTO
                //    {
                //        Codigo = -1,
                //        Contenido = "Ocurrió un error al leer los datos de la Acción de Personal"
                //    };
                //}
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


        [EdmFunction("ModelSIRH.Store", "UDF_VALIDAR_FECHA_RIGE")]
        public static DateTime? ValidarFecha(string FECHARIGE)
        {
            throw new NotSupportedException("Error.");
        }


        public DetallePuesto CargarDetallePuesto(int indDetalle)
        {
            DetallePuesto resultado = new DetallePuesto();

            resultado = entidadBase.DetallePuesto.Include("Clase")
                                                    .Include("Puesto")
                                                    .Include("Especialidad")
                                                    .Include("SubEspecialidad")
                                                    .Include("OcupacionReal")
                                                    .Include("EscalaSalarial")
                                                    .Where(Q => Q.PK_DetallePuesto == indDetalle).FirstOrDefault();

            return resultado;
        }

        public Nombramiento CargarNombramientoCedula(string cedula)
        {
            Nombramiento resultado = new Nombramiento();

            resultado = entidadBase.Nombramiento.Include("EstadoNombramiento").Where(R => R.Funcionario.IdCedulaFuncionario == cedula).OrderBy(Q => Q.FecRige).ToList().LastOrDefault();

            return resultado;
        }

        public CRespuestaDTO ActualizarEstadoDetallePuesto(int indDetalle, int indEstado)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                var detalle = entidadBase.DetallePuesto.Where(Q => Q.PK_DetallePuesto == indDetalle).FirstOrDefault(); ;

                if (detalle != null)
                {
                    detalle.IndEstadoDetallePuesto = indEstado;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = indDetalle
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO
                        {
                            Mensaje = "No existe el Detalle Puesto " + indDetalle.ToString()
                        }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;
        }

        #endregion
    }
}
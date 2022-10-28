using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CBorradorAccionPersonalD
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
        public CBorradorAccionPersonalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Almacena datos del Borrador de Acción de Personal en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Devuelve la llave primaria del registro insertado</returns>
        public CRespuestaDTO GuardarSolicitud(BorradorAccionPersonal registro, string codUsuarioEnvia, string codUsuarioRecibe)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.BorradorAccionPersonal
                                            .Include("EstadoBorrador")
                                            .Where(B => B.PK_Borrador == registro.PK_Borrador)
                                            .ToList();


                if (resultado.Where(R => R.PK_Borrador == registro.PK_Borrador).Count() < 1)
                {
                        // Movimiento 
                        MovimientoBorradorAccionPersonal movimientoBD = new MovimientoBorradorAccionPersonal
                        {
                            CodMovimiento = 1,
                            FecMovimiento = DateTime.Now,
                            UsuarioEnvia = codUsuarioEnvia,
                            UsuarioRecibe = codUsuarioRecibe,
                            ObsMovimiento = "Registro"
                        };

                        registro.MovimientoBorradorAccionPersonal.Add(movimientoBD);

                        entidadBase.BorradorAccionPersonal.Add(registro);
                        entidadBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = registro.PK_Borrador
                        };
                 }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ya existen los datos del Borrador"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
            return respuesta;
        }


        public CRespuestaDTO GuardarBorrador(int idBorrador, DetalleBorradorAccionPersonal detalle)
        {
            CRespuestaDTO respuesta;

            try
            {           
                var resultado = entidadBase.DetalleBorradorAccionPersonal
                                            .Include("BorradorAccionPersonal")
                                            .Where(B => B.BorradorAccionPersonal.PK_Borrador == idBorrador)
                                            .ToList();


                if (resultado.Count() < 1)
                {
                    detalle.BorradorAccionPersonal = entidadBase.BorradorAccionPersonal.Where(Q => Q.PK_Borrador == idBorrador).FirstOrDefault(); 

                    entidadBase.DetalleBorradorAccionPersonal.Add(detalle);

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle.BorradorAccionPersonal.PK_Borrador
                    };            
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "Ya existen los datos del Detalle del Borrador" }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
            return respuesta;
        }

        /// <summary>
        /// Actualiza los datos del Borrador de Acción de Personal en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Número de filas afectadas</returns>
        public CRespuestaDTO ActualizarDetalleBorrador(int idBorrador, DetalleBorradorAccionPersonal detalle, MovimientoBorradorAccionPersonal movimiento)
        {
            CRespuestaDTO respuesta;

            try
            {
                var detalleOld = entidadBase.DetalleBorradorAccionPersonal
                                            .Include("BorradorAccionPersonal")
                                            .Include("BorradorAccionPersonal.MovimientoBorradorAccionPersonal")
                                            .Where(B => B.BorradorAccionPersonal.PK_Borrador == idBorrador)
                                            .FirstOrDefault();

                if (detalleOld != null)
                {
                    detalleOld.FecRige = detalle.FecRige;
                    detalleOld.FecVence = detalle.FecVence;
                    detalleOld.FecRigeIntegra = detalle.FecRigeIntegra;
                    detalleOld.FecVenceIntegra = detalle.FecVenceIntegra;
                    detalleOld.CodClase = detalle.CodClase;
                    detalleOld.CodPuesto = detalle.CodPuesto;
                    detalleOld.NumHoras = detalle.NumHoras;
                    detalleOld.Disfrutado = detalle.Disfrutado;
                    detalleOld.Autorizado = detalle.Autorizado;
                    detalleOld.IndCategoria = detalle.IndCategoria;
                    detalleOld.MtoSalarioBase = detalle.MtoSalarioBase;
                    detalleOld.MtoAumentosAnuales = detalle.MtoAumentosAnuales;
                    detalleOld.MtoRecargo = detalle.MtoRecargo;
                    detalleOld.MtoGradoGrupo = detalle.MtoGradoGrupo;
                    detalleOld.PorProhibicion = detalle.PorProhibicion;
                    detalleOld.MtoOtros = detalle.MtoOtros;

                    detalleOld.BorradorAccionPersonal = entidadBase.BorradorAccionPersonal.Where(Q => Q.PK_Borrador == idBorrador).FirstOrDefault();
                    detalleOld.TipoAccionPersonal = detalle.TipoAccionPersonal;
                    detalleOld.Nombramiento = detalle.Nombramiento;
                    detalleOld.Programa = detalle.Programa;
                    detalleOld.Seccion = detalle.Seccion;

                    var borrador = entidadBase.BorradorAccionPersonal.Where(Q => Q.PK_Borrador == idBorrador).FirstOrDefault();
                    
                    // Movimiento 
                    MovimientoBorradorAccionPersonal movimientoBD = new MovimientoBorradorAccionPersonal
                    {
                        BorradorAccionPersonal = new BorradorAccionPersonal
                        {
                            PK_Borrador = idBorrador
                        },
                        CodMovimiento = movimiento.CodMovimiento,
                        FecMovimiento = DateTime.Now,
                        UsuarioEnvia = movimiento.UsuarioEnvia,
                        UsuarioRecibe = movimiento.UsuarioRecibe,
                        ObsMovimiento = movimiento.ObsMovimiento
                    };

                    borrador.MovimientoBorradorAccionPersonal.Add(movimientoBD);

                    detalle = detalleOld;
    
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle.PK_Detalle
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún borrador con el código especificado." }
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


        public CRespuestaDTO ActualizarEstado(BorradorAccionPersonal registro, MovimientoBorradorAccionPersonal movimiento)
        {
            CRespuestaDTO respuesta;

            int idEstado = registro.EstadoBorrador.PK_EstadoBorrador;

            try
            {
                var borradordOld = entidadBase.BorradorAccionPersonal
                                              .Include("EstadoBorrador")
                                              .Include("MovimientoBorradorAccionPersonal")
                                              .Where(B => B.PK_Borrador == registro.PK_Borrador).FirstOrDefault();

                if (borradordOld != null)
                {
                    borradordOld.UsuarioAsignado = registro.UsuarioAsignado;
                    borradordOld.EstadoBorrador = entidadBase.EstadoBorrador.Where(q => q.PK_EstadoBorrador == idEstado).FirstOrDefault();

                    


                    // Movimiento 
                    MovimientoBorradorAccionPersonal movimientoBD = new MovimientoBorradorAccionPersonal
                    {
                        BorradorAccionPersonal = entidadBase.BorradorAccionPersonal.Where(B => B.PK_Borrador == registro.PK_Borrador).FirstOrDefault(),
                        CodMovimiento = movimiento.CodMovimiento,
                        FecMovimiento = DateTime.Now,
                        UsuarioEnvia = movimiento.UsuarioEnvia,
                        UsuarioRecibe = movimiento.UsuarioRecibe,
                        ObsMovimiento = movimiento.ObsMovimiento
                    };

                    borradordOld.MovimientoBorradorAccionPersonal.Add(movimientoBD);

                    registro = borradordOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_Borrador
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún borrador con el código especificado." }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }

            return respuesta;
        }



        public CRespuestaDTO ObtenerBorrador(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.BorradorAccionPersonal
                                            .Include("EstadoBorrador")
                                            .Include("DetalleBorradorAccionPersonal")
                                            .Include("DetalleBorradorAccionPersonal.TipoAccionPersonal")
                                            .Include("DetalleBorradorAccionPersonal.Nombramiento")
                                            .Include("DetalleBorradorAccionPersonal.Nombramiento.Funcionario")
                                            .Include("MovimientoBorradorAccionPersonal")
                                            .Where(R => R.PK_Borrador == codigo).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Borrador"
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


        public CRespuestaDTO RetornarBorradorNombramiento(int idNombramiento)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.BorradorAccionPersonal.Where(N => N.DetalleBorradorAccionPersonal.FirstOrDefault().Nombramiento.PK_Nombramiento == idNombramiento).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Borrador"
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

        public CRespuestaDTO BuscarSolicitud(CBorradorAccionPersonalDTO registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                bool condicionNumOficio = false;
                bool condicionEstado = false;
              
                List<BorradorAccionPersonal> resultado = entidadBase.BorradorAccionPersonal
                                                                .Include("EstadoBorrador")
                                                                .ToList();

                if (registro.NumOficio != null)
                    condicionNumOficio = registro.NumOficio != "";

                if (registro.EstadoBorrador != null)
                    condicionEstado = registro.EstadoBorrador.IdEntidad != 0;

                if (condicionNumOficio)
                    resultado = resultado.Where(q => q.NumOficio.ToLower().Contains(registro.NumOficio.ToLower())).ToList();

                if (condicionEstado)
                    resultado = resultado.Where(q => q.EstadoBorrador.PK_EstadoBorrador == registro.EstadoBorrador.IdEntidad).ToList();

                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Borrador"
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


        public CRespuestaDTO BuscarBorrador(CFuncionarioDTO funcionario,
                                            CBorradorAccionPersonalDTO registro,
                                            CDetalleBorradorAccionPersonalDTO detalle,
                                            List<DateTime> fechas,
                                            List<DateTime> fechasIntegra)
        {
            CRespuestaDTO respuesta;

            try
            {
                DateTime paramFechaInicio = new DateTime();
                DateTime paramFechaFinal = new DateTime();
                DateTime paramFechaIntegraInicio = new DateTime();
                DateTime paramFechaIntegraFinal = new DateTime();

                bool condicionFecha = false;
                bool condicionFechaIntegra = false;

                bool condicionTipo = false;
                bool condicionNumOficio = false;
                bool condicionEstado = false;
                bool condicionFuncionario = false;

                if (fechas.Count > 0)
                {
                    paramFechaInicio = fechas.ElementAt(0);
                    paramFechaFinal = fechas.ElementAt(1);
                    condicionFecha = true;
                }

                if (fechasIntegra.Count > 0)
                {
                    paramFechaIntegraInicio = fechasIntegra.ElementAt(0);
                    paramFechaIntegraFinal = fechasIntegra.ElementAt(1);
                    condicionFechaIntegra = true;
                }

                List<BorradorAccionPersonal> resultado = entidadBase.BorradorAccionPersonal
                                                                .Include("EstadoBorrador")
                                                                .Include("DetalleBorradorAccionPersonal")
                                                                .Include("DetalleBorradorAccionPersonal.TipoAccionPersonal")
                                                                .Include("DetalleBorradorAccionPersonal.Programa")
                                                                .Include("DetalleBorradorAccionPersonal.Seccion")
                                                                .Include("DetalleBorradorAccionPersonal.Nombramiento")
                                                                .Include("DetalleBorradorAccionPersonal.Nombramiento.Funcionario")
                                                                .Include("DetalleBorradorAccionPersonal.Nombramiento.Puesto")
                                                                .Where(q => q.DetalleBorradorAccionPersonal.Count() > 0)
                                                                .ToList();

                if (detalle != null && detalle.TipoAccion != null)
                    condicionTipo = detalle.TipoAccion.IdEntidad != 0;

                if (funcionario != null && funcionario.Cedula != null)
                    condicionFuncionario = funcionario.Cedula != "";

                if (registro != null && registro.NumOficio != null)
                    condicionNumOficio = registro.NumOficio != "";
                
                if (registro != null && registro.EstadoBorrador != null)
                    condicionEstado = registro.EstadoBorrador.IdEntidad != 0;

                // Filtrar
                if (condicionFuncionario)
                    resultado = resultado.Where(q => q.DetalleBorradorAccionPersonal.FirstOrDefault().Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula).ToList();

                if (condicionTipo)
                    resultado = resultado.Where(q => q.DetalleBorradorAccionPersonal.FirstOrDefault().TipoAccionPersonal.PK_TipoAccionPersonal == detalle.TipoAccion.IdEntidad).ToList();

                if (condicionNumOficio)
                    resultado = resultado.Where(q => q.NumOficio.ToLower().Contains(registro.NumOficio.ToLower())).ToList();
                
                if (condicionFecha)
                    //resultado = resultado.Where(q => q.DetalleBorradorAccionPersonal.FirstOrDefault().FecRige >= paramFechaRigeInicio && q.DetalleBorradorAccionPersonal.FirstOrDefault().FecRige <= paramFechaRigeFinal).ToList();
                    resultado = resultado.Where(q => q.DetalleBorradorAccionPersonal.FirstOrDefault().FecRige <= paramFechaInicio && q.DetalleBorradorAccionPersonal.FirstOrDefault().FecVence >= paramFechaFinal).ToList();

                if (condicionFechaIntegra)
                    resultado = resultado.Where(q => q.DetalleBorradorAccionPersonal.FirstOrDefault().FecRigeIntegra <= paramFechaIntegraInicio && q.DetalleBorradorAccionPersonal.FirstOrDefault().FecVenceIntegra <= paramFechaIntegraFinal).ToList();

                if (condicionEstado)
                    resultado = resultado.Where(q => q.EstadoBorrador.PK_EstadoBorrador == registro.EstadoBorrador.IdEntidad).ToList();

                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Borrador"
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


  
        public CRespuestaDTO RegistrarMovimiento(MovimientoBorradorAccionPersonal registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                entidadBase.MovimientoBorradorAccionPersonal.Add(registro);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = registro.PK_MovimientoBorrador
                };
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }

            return respuesta;
        }


        public CRespuestaDTO FuncionariosConBorradores()
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.DetalleNombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.DetalleBorradorAccionPersonal")
                                                   .Include("Nombramiento.DetalleBorradorAccionPersonal.BorradorAccionPersonal")
                                                   .Include("Nombramiento.DetalleBorradorAccionPersonal.BorradorAccionPersonal.EstadoBorrador")
                                                   .Include("Nombramiento.DetalleBorradorAccionPersonal.TipoAccionPersonal")
                                                   .Where(Q => Q.Nombramiento.Where(N => N.DetalleBorradorAccionPersonal.Count > 0)
                                                       .Count() > 0).OrderBy(Q => Q.NomPrimerApellido).ToList();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron funcionarios que tengan borradores de acción de personal registradas.");
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
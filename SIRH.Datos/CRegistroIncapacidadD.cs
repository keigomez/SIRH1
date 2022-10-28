using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;
using System.Data.Objects;


namespace SIRH.Datos
{
    public class CRegistroIncapacidadD
    {

        #region Variables

        /// <summary>
        /// Contexto de la entidad funcionario
        /// </summary>
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase Registro Incapacidad
        /// </summary>
        /// <param name="entidadGlobal"></param>
        public CRegistroIncapacidadD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Almacena datos del Registro de Incapacidad en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Devuelve la llave primaria del registro insertado</returns>
        public CRespuestaDTO GuardarRegistroIncapacidad(RegistroIncapacidad registro, double dias)
        {
            bool boolFecha = false;

            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.RegistroIncapacidad.Include("Nombramiento")
                                            .Include("Nombramiento.Puesto")
                                            .Include("Nombramiento.Funcionario")
                                            .Where(F => F.Nombramiento.PK_Nombramiento == registro.Nombramiento.PK_Nombramiento && F.IndEstadoIncapacidad != 3) // Estado 3 = Anulado
                                            .ToList();

                //var resultado = entidadBase.Funcionario.Include("Nombramiento")
                //                                   .Include("Nombramiento.DetalleNombramiento")
                //                                   .Include("Nombramiento.Puesto")
                //                                   .Include("Nombramiento.RegistroIncapacidad")
                //                                   .Where(F => F.IdCedulaFuncionario == registro.Nombramiento.Funcionario.IdCedulaFuncionario)
                //                                   .ToList();


                if (resultado.Where(R => R.PK_RegistroIncapacidad == registro.PK_RegistroIncapacidad).Count() < 1)
                {
                    DateTime fecIni = DateTime.Parse(registro.FecRige.ToString());
                    DateTime fecFin = DateTime.Parse(registro.FecVence.ToString());

                    var lista = Enumerable.Range(0, (fecFin - fecIni).Days + 1)
                                          .Select(d => fecIni.AddDays(d))
                                          .ToArray();


                    foreach (DateTime fecha in lista)
                    {
                        if (resultado.Count(Q => Q.FecRige <= fecha && Q.FecVence >= fecha) > 0)
                        {
                            boolFecha = true;
                            break;
                        }
                    }

                    //if (resultado.Count(Q => Q.FecRige <= registro.FecRige && Q.FecVence >= registro.FecVence) < 1)
                    if (boolFecha == false)
                    {
                        registro.IndEstadoIncapacidad = 1; // Activa
                        entidadBase.RegistroIncapacidad.Add(registro);
                        entidadBase.SaveChanges();

                        DetalleIncapacidad detalle;
                        Decimal porcentaje = 0;
                        Decimal monto = 0;

                        // Se debe sumar un día más
                        dias += 1;

                        foreach (DateTime fecha in lista)
                        {
                            porcentaje = GetPorcentaje(registro.TipoIncapacidad.PK_TipoIncapacidad, registro.TipoIncapacidad.FK_EntidadMedica, dias);
                            monto = Math.Round(Convert.ToDecimal((registro.MtoSalario / 30) * (porcentaje / 100)), 2);
                            detalle = new DetalleIncapacidad
                            {
                                FK_RegistroIncapacidad = registro.PK_RegistroIncapacidad,
                                FecDia = fecha,
                                PorSubsidio = porcentaje,
                                MtoSubsidio = monto
                            };

                            entidadBase.DetalleIncapacidad.Add(detalle);
                            entidadBase.SaveChanges();

                            dias++;
                        }

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = registro.PK_RegistroIncapacidad
                        };
                    }
                    else
                    {
                        throw new Exception("Ya existe una incapacidad dentro de las mismas fechas suministradas, por favor revise los datos ingresados.");
                    }
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ya existen los datos de la Incapacidad"
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

        public CRespuestaDTO VerificarFechasIncapacidad(RegistroIncapacidad registro)
        {
            bool boolFecha = false;

            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.RegistroIncapacidad.Include("Nombramiento")
                                            .Include("Nombramiento.Puesto")
                                            .Include("Nombramiento.Funcionario")
                                            //.Where(F => F.Nombramiento.PK_Nombramiento == registro.Nombramiento.PK_Nombramiento && F.IndEstadoIncapacidad != 3) // Estado 3 = Anulado
                                            .Where(F => F.Nombramiento.Funcionario.IdCedulaFuncionario == registro.Nombramiento.Funcionario.IdCedulaFuncionario && F.IndEstadoIncapacidad != 3) // Estado 3 = Anulado
                                            .ToList();

                if (resultado.Where(R => R.PK_RegistroIncapacidad == registro.PK_RegistroIncapacidad).Count() < 1)
                {
                    DateTime fecIni = DateTime.Parse(registro.FecRige.ToString());
                    DateTime fecFin = DateTime.Parse(registro.FecVence.ToString());

                    var lista = Enumerable.Range(0, (fecFin - fecIni).Days + 1)
                                          .Select(d => fecIni.AddDays(d))
                                          .ToArray();


                    foreach (DateTime fecha in lista)
                    {
                        if (resultado.Count(Q => Q.FecRige <= fecha && Q.FecVence >= fecha) > 0)
                        {
                            boolFecha = true;
                            break;
                        }
                    }

                    if (boolFecha == false)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = 1
                        };
                    }
                    else
                    {
                        throw new Exception("Ya existe una incapacidad dentro de las mismas fechas suministradas, por favor revise los datos ingresados.");
                    }
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ya existen los datos de la Incapacidad"
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
        /// Actualiza los datos del Registro de Incapacidad en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Número de filas afectadas</returns>
        public CRespuestaDTO ActualizarRegistroIncapacidad(RegistroIncapacidad registro)
        {
            CRespuestaDTO respuesta;
            int dato;

            try
            {
                EntityKey llave;
                object objetoOriginal;

                using (entidadBase)
                {
                    //llave = entidadBase.CreateEntityKey("RegistroIncapacidad", registro);
                    //if (entidadBase.TryGetObjectByKey(llave, out objetoOriginal))
                    //{
                    //    entidadBase.ApplyPropertyChanges(llave.EntitySetName, registro);
                    //}
                    entidadBase.Entry(registro).State = EntityState.Modified;

                    //var original = entidadBase.RegistroIncapacidad.Find(registro.CodIncapacidad);

                    //if (original != null)
                    //{
                    //    entidadBase.Entry(original).CurrentValues.SetValues(registro);
                    //}


                    dato = entidadBase.SaveChanges();
                }

                if (dato > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = true
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = false
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


        public CRespuestaDTO ObtenerRegistroIncapacidad(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.RegistroIncapacidad
                                            .Include("DetalleIncapacidad")
                                            .Include("TipoIncapacidad")
                                            .Include("TipoIncapacidad.EntidadMedica")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Puesto")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                            .Where(R => R.PK_RegistroIncapacidad == codigo).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos de la Incapacidad"
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


        public CRespuestaDTO ObtenerRegistroIncapacidadCodigo(string numIncapacidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.RegistroIncapacidad
                                            .Include("DetalleIncapacidad")
                                            .Include("TipoIncapacidad")
                                            .Include("TipoIncapacidad.EntidadMedica")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                            .Where(R => R.CodIncapacidad == numIncapacidad && R.IndEstadoIncapacidad != 3)
                                            //.OrderByDescending(R => R.PK_RegistroIncapacidad)
                                            .ToList();

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
                        Contenido = "Ocurrió un error al leer los datos de la Incapacidad"
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<RegistroIncapacidad> ListarRegistroIncapacidad(string cedula)
        {
            List<RegistroIncapacidad> resultado = new List<RegistroIncapacidad>();

            try
            {
                var datosRegistroIncapacidad = entidadBase.RegistroIncapacidad
                                            .Include("TipoIncapacidad")
                                            .Include("TipoIncapacidad.EntidadMedica")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                if (datosRegistroIncapacidad != null)
                {
                    resultado = datosRegistroIncapacidad;
                    return resultado.OrderByDescending(Q => Q.PK_RegistroIncapacidad).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron registros de Incapacidades para ese funcionario.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public CRespuestaDTO RetornarRegistroIncapacidadNombramiento(int idNombramiento)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.RegistroIncapacidad.Where(N => N.Nombramiento.PK_Nombramiento == idNombramiento).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos de la Incapacidad"
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


        public CRespuestaDTO BuscarRegistroIncapacidad(CFuncionarioDTO funcionario,
                                                 CRegistroIncapacidadDTO registro,
                                                 CEntidadMedicaDTO entidadMedica,
                                                 List<DateTime> fechasRige,
                                                 List<DateTime> fechasVence)
        {
            CRespuestaDTO respuesta;

            try
            {
                List<RegistroIncapacidad> resultado = new List<RegistroIncapacidad>();

                DateTime paramFechaRigeInicio = new DateTime();
                DateTime paramFechaRigeFinal = new DateTime();
                DateTime paramFechaVenceInicio = new DateTime();
                DateTime paramFechaVenceFinal = new DateTime();

                bool buscar = true;
                bool condicionFechaRige = false;
                bool condicionFechaVence = false;

                bool condicionTipo = false;
                bool condicionEntidad = false;
                bool condicionFuncionario = false;
                bool condicionObservacion = false;
                bool condicionCodigo = false;
                bool condicionEstado = false;

                if (fechasRige.Count > 0)
                {
                    paramFechaRigeInicio = fechasRige.ElementAt(0);
                    paramFechaRigeFinal = fechasRige.ElementAt(1);
                    condicionFechaRige = true;
                }


                if (fechasVence.Count > 0)
                {
                    paramFechaVenceInicio = fechasVence.ElementAt(0);
                    paramFechaVenceFinal = fechasVence.ElementAt(1);
                    condicionFechaVence = true;
                }



                if (registro.TipoIncapacidad != null)
                    condicionTipo = registro.TipoIncapacidad.IdEntidad != 0;

                if (registro.EstadoIncapacidad > 0)
                    condicionEstado = true;

                if (entidadMedica != null)
                    condicionEntidad = entidadMedica.IdEntidad != 0;
                //bool condicionNombramiento = registro.Nombramiento.IdEntidad != 0 && registro.Nombramiento.IdEntidad != null;

                if (funcionario.Cedula != null)
                    condicionFuncionario = funcionario.Cedula != "";
                //bool condicionPuesto = registro.Nombramiento.Puesto.CodPuesto != "" && registro.Nombramiento.Puesto.CodPuesto != null;

                if (registro.ObsIncapacidad != null)
                    condicionObservacion = registro.ObsIncapacidad != "";

                if (registro.CodIncapacidad != null)
                    condicionCodigo = registro.CodIncapacidad != "";


                // Filtrar

                if (condicionFuncionario)
                {
                    resultado = entidadBase.RegistroIncapacidad
                                                .Include("DetalleIncapacidad")
                                                .Include("TipoIncapacidad")
                                                .Include("TipoIncapacidad.EntidadMedica")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto")
                                                .Where(q => q.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula)
                                                .ToList();
                    if (resultado.Count() < 1)
                        buscar = false;
                }



                //if (condicionNombramiento)
                //    resultado = resultado.Where(q => q.Nombramiento.PK_Nombramiento == registro.Nombramiento.IdEntidad).ToList();

                //if (condicionPuesto)
                //    resultado = resultado.Where(q => q.Nombramiento.Puesto.CodPuesto == registro.Nombramiento.Puesto.CodPuesto).ToList();

                if (condicionTipo && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.TipoIncapacidad.PK_TipoIncapacidad == registro.TipoIncapacidad.IdEntidad).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.TipoIncapacidad.PK_TipoIncapacidad == registro.TipoIncapacidad.IdEntidad)
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionEntidad && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.TipoIncapacidad.FK_EntidadMedica == entidadMedica.IdEntidad).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                                    .Include("DetalleIncapacidad")
                                                    .Include("TipoIncapacidad")
                                                    .Include("TipoIncapacidad.EntidadMedica")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(q => q.TipoIncapacidad.FK_EntidadMedica == entidadMedica.IdEntidad)
                                                    .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionFechaRige && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.FecRige >= paramFechaRigeInicio && q.FecRige <= paramFechaRigeFinal).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.FecRige >= paramFechaRigeInicio && q.FecRige <= paramFechaRigeFinal)
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionFechaVence && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.FecVence >= paramFechaVenceInicio && q.FecVence <= paramFechaVenceFinal).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.FecVence >= paramFechaVenceInicio && q.FecVence <= paramFechaVenceFinal)
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionObservacion && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.ObsIncapacidad.ToLower().Contains(registro.ObsIncapacidad.ToLower())).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.ObsIncapacidad.ToLower().Contains(registro.ObsIncapacidad.ToLower()))
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionCodigo && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.CodIncapacidad == registro.CodIncapacidad).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.CodIncapacidad == registro.CodIncapacidad)
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


                if (condicionEstado && buscar)
                {
                    if (resultado.Count() > 0)
                        resultado = resultado.Where(q => q.IndEstadoIncapacidad == registro.EstadoIncapacidad).ToList();
                    else
                        resultado = entidadBase.RegistroIncapacidad
                                        .Include("DetalleIncapacidad")
                                        .Include("TipoIncapacidad")
                                        .Include("TipoIncapacidad.EntidadMedica")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Puesto")
                                        .Where(q => q.IndEstadoIncapacidad == registro.EstadoIncapacidad)
                                        .ToList();

                    if (resultado.Count() < 1)
                        buscar = false;
                }


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
                        Contenido = "Ocurrió un error al leer los datos de la Incapacidad"
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


        public CRespuestaDTO ModificarEstadoIncapacidad(RegistroIncapacidad registro, int indEstado)
        {
            CRespuestaDTO respuesta;

            try
            {
                var incapacidadOld = entidadBase.RegistroIncapacidad
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Where(I => I.PK_RegistroIncapacidad == registro.PK_RegistroIncapacidad).FirstOrDefault();

                if (incapacidadOld != null)
                {
                    incapacidadOld.IndEstadoIncapacidad = indEstado;
                    incapacidadOld.ObsIncapacidad = registro.ObsIncapacidad;
                    registro = incapacidadOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_RegistroIncapacidad
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna incapacidad con el código especificado." }
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


        public CRespuestaDTO FuncionariosConIncapacidades()
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
                                                   .Include("Nombramiento.RegistroIncapacidad")
                                                   .Include("Nombramiento.RegistroIncapacidad.TipoIncapacidad")
                                                   .Include("Nombramiento.RegistroIncapacidad.TipoIncapacidad.EntidadMedica")
                                                   .Where(Q => Q.Nombramiento.Where(N => N.RegistroIncapacidad.Count > 0)
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
                    throw new Exception("No se encontraron funcionarios que tengan Incapacidades registradas.");
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


        private decimal GetPorcentaje(int? idTipo, int? idEntidad, double dias)
        {
            decimal porcentaje = 0;

            try
            {
                switch (idEntidad)
                {
                    case 1: // CCSS

                        switch (idTipo)
                        {
                            case 1: // // Maternidad
                                porcentaje = 50;
                                break;

                            case 4:  // Fase Terminal
                            case 5:  // Licencia
                                porcentaje = 0;
                                break;

                            default:
                                if (dias >= 1 && dias <= 3)
                                    porcentaje = 80;

                                if (dias >= 4 && dias <= 30)
                                    porcentaje = 20;

                                if (dias > 30)
                                    porcentaje = 40;
                                break;
                        }
                        break;

                    case 2: // INS

                        if (idTipo == 3) // SOA
                        {
                            porcentaje = 0;
                        }
                        else
                        {
                            if (dias >= 1 && dias <= 30)
                                porcentaje = 20;

                            if (dias >= 31 && dias <= 45)
                                porcentaje = 40;

                            if (dias >= 46 && dias <= 760)
                                porcentaje = 33;
                        }

                        break;
                }

                return porcentaje;

            }
            catch (Exception error)
            {
                return -1;
            } 
        }


        public CRespuestaDTO ObtenerSalarioBruto(string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DesgloseSalarial
                               .Include("DetalleDesgloseSalarial")
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList().LastOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.DetalleDesgloseSalarial.Sum(D => D.MtoPagocomponenteSalarial) * 2
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Incapacidad"
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

        public CRespuestaDTO ConsultaIncapacidades(string cedula)
        {
            try
            {
                var datos = entidadBase.RegistroIncapacidad.Include("Nombrammiento")
                    .Include("Funcionario")
                    .Where(I => I.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                if (datos == null)
                {
                    throw new Exception("No se encontraron datos de incapacidades para la cédula indicada");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = datos
                };

            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        #endregion
    }
}
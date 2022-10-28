using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Datos
{
    public class CPrestacionLegalD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPrestacionLegalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Métodos

        public CRespuestaDTO ObtenerPrestacion(CPrestacionLegalDTO prestacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.PrestacionLegal
                                        .Include("TipoPrestacion")
                                        .Include("DetallePrestacion")
                                        .Include("DetallePrestacionAfiliacion")
                                        .Include("DetallePrestacionCuadro")
                                        .Include("DetallePrestacionCuadro.TipoPrestacionCuadro")
                                        .Include("Nombramiento")
                                        .Include("Nombramiento.Funcionario")
                                        .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                        .Include("Nombramiento.Puesto")
                                        .Include("Nombramiento.Puesto.DetallePuesto")
                                        .Where(Q => Q.IndEstado == 1) // Válido
                                        .ToList();
                if(prestacion.IdEntidad > 0)
                    dato = dato.Where(Q => Q.PK_PrestacionLegal == prestacion.IdEntidad).ToList();

                if (prestacion.Nombramiento != null && prestacion.Nombramiento.Funcionario != null && prestacion.Nombramiento.Funcionario.Cedula != "")
                    dato = dato.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario == prestacion.Nombramiento.Funcionario.Cedula).ToList();

                if (dato != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato.FirstOrDefault()
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna prestacion con el código especificado." }
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

        public CRespuestaDTO AgregarPrestacionLegal(PrestacionLegal prestacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.PrestacionLegal
                                .Where(Q => Q.Nombramiento.Funcionario.PK_Funcionario == prestacion.Nombramiento.Funcionario.PK_Funcionario 
                                        && Q.IndEstado == 2) // Anulada
                                .FirstOrDefault();

                if (dato == null)
                {
                    prestacion.IndEstado = 1;
                    entidadBase.PrestacionLegal.Add(prestacion);
                    //entidadBase.SaveChanges();

                    // Detalle
                    foreach (var item in prestacion.DetallePrestacion)
                    {
                        item.FK_PrestacionLegal = prestacion.PK_PrestacionLegal;
                        entidadBase.DetallePrestacion.Add(item);
                    }

                    // Detalle Asignacion
                    if(prestacion.DetallePrestacionAfiliacion != null)
                        foreach (var item in prestacion.DetallePrestacionAfiliacion)
                        {
                            item.FK_Prestacion = prestacion.PK_PrestacionLegal;
                            entidadBase.DetallePrestacionAfiliacion.Add(item);
                        }

                    // Detalle Cuadros
                    foreach (var item in prestacion.DetallePrestacionCuadro)
                    {
                        item.FK_Prestacion = prestacion.PK_PrestacionLegal;
                        entidadBase.DetallePrestacionCuadro.Add(item);
                    }

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = prestacion.PK_PrestacionLegal
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("El funcionario ya tiene una Prestación Registrada");
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
                
        public CRespuestaDTO AnularPrestacion(PrestacionLegal prestacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                var prestacionOld = entidadBase.PrestacionLegal.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Where(C => C.PK_PrestacionLegal == prestacion.PK_PrestacionLegal).FirstOrDefault();
                if (prestacionOld != null)
                {
                    prestacionOld.IndEstado = 2;

                    prestacion = prestacionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = prestacion.PK_PrestacionLegal
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna prestacion con el código especificado." }
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
        
        public CRespuestaDTO GuardarPrestacionLegal(PrestacionLegal prestacion)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                entidadBase.PrestacionLegal.Add(prestacion);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = prestacion.PK_PrestacionLegal
                };
                return respuesta;
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

        public CRespuestaDTO BuscarFuncionarioDesgloceSalarial(string cedula)
        {
            try
            {
                var resultado = entidadBase.DesgloseSalarial
                                            .Include("DetalleDesgloseSalarial")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Where(F => F.Nombramiento.Funcionario.IdCedulaFuncionario == cedula)
                                            .ToList();
                if (resultado == null)
                    throw new Exception("No se encontraron Pagos asociados a la cédula indicada.");
               
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado
                };

            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public Usuario CargarUsuarioPorID(int idUsuario)
        {
            Usuario resultado = new Usuario();
            resultado = entidadBase.Usuario.Where(R => R.PK_Usuario == idUsuario).FirstOrDefault();
            return resultado;
        }

        public AccionPersonal CargarAccionPorID(int idAccion)
        {
            AccionPersonal resultado = new AccionPersonal();
            resultado = entidadBase.AccionPersonal.Where(R => R.PK_AccionPersonal == idAccion).FirstOrDefault();
            return resultado;
        }

        #endregion
    }
}


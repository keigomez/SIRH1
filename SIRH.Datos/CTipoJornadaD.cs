using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTipoJornadaD
   {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoJornadaD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene los tipos de Jornada de la BD
        /// </summary>
        /// <returns>Retorna Tipos de Jornada</returns>
        public List<TipoJornada> RetornarTipoJornada()
        {
            return contexto.TipoJornada.ToList();
        }

        public TipoJornada CargarTipoJornadaPorID(int idTipoJornada)
        {
            TipoJornada resultado = new TipoJornada();

            resultado = contexto.TipoJornada.Where(R => R.PK_TipoJornada == idTipoJornada).FirstOrDefault();

            return resultado;
        }

        public int GuardarTipoJornada(TipoJornada tipoJornada)
        {
            contexto.TipoJornada.Add(tipoJornada);
            return tipoJornada.PK_TipoJornada;
        }

        public CRespuestaDTO RegistrarJornadaFuncionario(int idNombramiento, TipoJornada item)
        {
            return new CRespuestaDTO();
            //CRespuestaDTO respuesta;

            //try
            //{
            //    var nombramiento = contexto.Nombramiento
            //                        .Include("DetalleNombramiento")
            //                        .Include("DetalleNombramiento.TipoJornada")
            //                        .Where(N => N.PK_Nombramiento == idNombramiento)
            //                        .FirstOrDefault();

            //    if (nombramiento != null)
            //    {
            //        if (nombramiento.DetalleNombramiento.Count < 1)
            //        {
            //            nombramiento.DetalleNombramiento.Add(
            //                new DetalleNombramiento
            //                {
            //                    FecCreacion = DateTime.Now,
            //                    TipoJornada = item
            //                });
            //            contexto.SaveChanges();
            //            respuesta = new CRespuestaDTO
            //            {
            //                Codigo = 1,
            //                Contenido = nombramiento.DetalleNombramiento.FirstOrDefault().TipoJornada
            //            };
            //            return respuesta;
            //        }
            //        else
            //        {
            //            nombramiento.DetalleNombramiento.FirstOrDefault().TipoJornada = item;
            //            contexto.SaveChanges();
            //            respuesta = new CRespuestaDTO
            //            {
            //                Codigo = 1,
            //                Contenido = nombramiento.DetalleNombramiento.FirstOrDefault().TipoJornada
            //            };
            //            return respuesta;
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("Ocurrió un error al cargar los datos del nombramiento");
            //    }
            //}
            //catch (Exception error)
            //{
            //    respuesta = new CRespuestaDTO
            //    {
            //        Codigo = -1,
            //        Contenido = new CErrorDTO { MensajeError = error.Message }
            //    };
            //    return respuesta;
            //}
        }

        public CRespuestaDTO CargarTipoJornadaPorFuncionarioDTO(string cedula)
        {
            return new CRespuestaDTO();
            //CRespuestaDTO respuesta;
            //try
            //{
            //    var dato = contexto.TipoJornada
            //                       .Include("DetalleNombramiento")
            //                       .Include("DetalleNombramiento.Nombramiento")
            //                       .Include("DetalleNombramiento.Nombramiento.Funcionario")
            //                       .Where(T => T.DetalleNombramiento.Where(D => D.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).Count()>0)
            //                       .FirstOrDefault();

            //    if (dato != null)
            //    {
            //        respuesta = new CRespuestaDTO
            //        {
            //            Codigo = 1,
            //            Contenido = dato
            //        };
            //    }
            //    else throw new Exception("No se encontró la Jornada para la cédula: " + cedula);
            //}
            //catch (Exception error)
            //{
            //    respuesta = new CRespuestaDTO
            //    {
            //        Codigo = -1,
            //        Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
            //    };
            //}
            //return respuesta;
        }

        public CRespuestaDTO GuardarTipoJornadaPorPuesto(string codPuesto, TipoJornada tipoJornada)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.TipoJornada.Add(tipoJornada);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tipoJornada
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

        public CRespuestaDTO EditarJornadaFuncionario(TipoJornada item)
        {
            CRespuestaDTO respuesta;
            try
            {
                var jornada = contexto.TipoJornada
                                .Include("DetalleNombramiento")
                                .Include("DetalleNombramiento.Nombramiento")
                                .Include("DetalleNombramiento.Nombramiento.Funcionario")
                                .Where(J => J.PK_TipoJornada == item.PK_TipoJornada)
                                .FirstOrDefault();

                if (jornada != null)
                {
                    jornada.IndInicio = item.IndInicio;
                    jornada.IndFin = item.IndFin;
                    jornada.IndAcumulativa = item.IndAcumulativa;
                    jornada.IndDiaLibre = item.IndDiaLibre;
                    jornada.DesTipoJornada = item.DesTipoJornada;
                    item = null;
                    contexto.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = jornada
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Ocurrió un error al cargar los datos del nombramiento");
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
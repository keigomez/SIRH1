using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    //
    public class CMetaIndividualEvidenciaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CMetaIndividualEvidenciaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public CRespuestaDTO InsertarEvidencia(MetaIndividualEvidencia evidencia)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (evidencia != null)
                {
                    entidadBase.MetaIndividualEvidencia.Add(evidencia);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = evidencia
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Los parámetros son inválidos");
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

        public CRespuestaDTO ActualizarEvidencia(MetaIndividualEvidencia evidencia)
        {

            CRespuestaDTO respuesta;
            var evidenciaActualizar = entidadBase.MetaIndividualEvidencia.
                                       Where(e => e.PK_Evidencia == evidencia.PK_Evidencia).FirstOrDefault();

            evidenciaActualizar.DesObservaciones = evidencia.DesObservaciones != null ? evidencia.DesObservaciones : "";
            evidenciaActualizar.IndEstado = evidencia.IndEstado;
            if (evidencia.IndEstado == 0)
            {
                evidenciaActualizar.DesEnlace = "";
                evidenciaActualizar.DocAdjunto = new byte[0];
            }
              

            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = evidenciaActualizar.PK_Evidencia
                };
            }
            else
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = "No se encontró la evidencia a modificar." }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ConsultarEvidencia(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var evidencia = entidadBase.MetaIndividualEvidencia.Where(E => E.PK_Evidencia == codigo).FirstOrDefault();

                if (evidencia != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = evidencia
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna evidencia" }
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

        public CRespuestaDTO BuscarEvidencia(List<MetaIndividualEvidencia> datosPrevios, object parametro, string elemento, bool busquedaAnterior)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro, busquedaAnterior);
                if (datosPrevios.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPrevios
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
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

        private List<MetaIndividualEvidencia> CargarDatos(string elemento, List<MetaIndividualEvidencia> datosPrevios, object parametro, bool busquedaAnterior)
        {
            string param = "";
            decimal dparam = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString().ToUpper();
            }
            else
            {
                if (parametro.GetType().Name.Equals("Decimal"))
                {
                    dparam = Convert.ToDecimal(parametro);
                }
                else
                {
                    paramFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                    paramFechaFinal = ((List<DateTime>)parametro).ElementAt(1);
                }
            }

            if (datosPrevios.Count < 1 && ! busquedaAnterior)
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = entidadBase.MetaIndividualEvidencia
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Periodo":
                        datosPrevios = entidadBase.MetaIndividualEvidencia
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.FK_PeriodoCalificacion == dparam).ToList();
                        break;

                    case "Meta":
                        datosPrevios = entidadBase.MetaIndividualEvidencia
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.PK_Meta == dparam).ToList();
                        break;

                    case "Descripcion":
                        datosPrevios = entidadBase.MetaIndividualEvidencia
                                                    .Where(C => C.DesEvidencia.Contains(param)).ToList();
                        break;

                    case "Fecha":
                        datosPrevios = entidadBase.MetaIndividualEvidencia
                                                    .Where(C => C.FecRegistro >= paramFechaInicio &&
                                                        C.FecRegistro <= paramFechaFinal).ToList();
                        break;

                    default:
                        datosPrevios = new List<MetaIndividualEvidencia>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Periodo":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.FK_PeriodoCalificacion == dparam).ToList();
                        break;

                    case "Meta":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.PK_Meta == dparam).ToList();
                        break;

                    case "Descripcion":
                        datosPrevios = datosPrevios.Where(C => C.DesEvidencia.Contains(param)).ToList();
                        break;

                    case "Fecha":
                        datosPrevios = datosPrevios.Where(C => C.FecRegistro >= paramFechaInicio &&
                                                        C.FecRegistro <= paramFechaFinal).ToList();
                        break;
            
                    default:
                        datosPrevios = new List<MetaIndividualEvidencia>();
                        break;
                }
            }

            datosPrevios = datosPrevios.Where(C => C.IndEstado == 1).ToList();
            return datosPrevios;
        }

        public CRespuestaDTO InsertarPermiso(CatPermisoEvidencia permiso)
        {
            CRespuestaDTO respuesta;
            try
            {
                permiso.IndArchivo = 1;
                var permisoBD = entidadBase.CatPermisoEvidencia.Where(Q => Q.FK_Funcionario == permiso.FK_Funcionario).FirstOrDefault();
                if (permisoBD == null)
                    entidadBase.CatPermisoEvidencia.Add(permiso);
                else
                    permisoBD.IndArchivo = 1;

                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = permisoBD
                };

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

        public CRespuestaDTO ModificarPermiso(CatPermisoEvidencia permiso)
        {
            CRespuestaDTO respuesta;
            try
            {
                var permisoBD = entidadBase.CatPermisoEvidencia.Where(Q => Q.FK_Funcionario == permiso.FK_Funcionario).FirstOrDefault();
                permisoBD.IndArchivo = permiso.IndArchivo;

                int resultado = entidadBase.SaveChanges();
                if (resultado > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = permisoBD.PK_Permiso
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró la evidencia a modificar." }
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

        public CRespuestaDTO ConsultarPermiso(int codFuncionario)
        {
            CRespuestaDTO respuesta;
            try
            {
                var permisoBD = entidadBase.CatPermisoEvidencia.Where(Q => Q.FK_Funcionario == codFuncionario).FirstOrDefault();
                if (permisoBD != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = permisoBD.IndArchivo
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 0,
                        Contenido = 0
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
            }

            return respuesta;
        }

        #endregion
    }
}
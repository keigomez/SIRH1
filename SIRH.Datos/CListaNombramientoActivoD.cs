using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CListaNombramientoActivoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CListaNombramientoActivoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Métodos

        public CRespuestaDTO ActualizarLista()
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.USP_LISTAR_NOMBRAMIENTOS_ACTIVOS();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = dato
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

        public CRespuestaDTO BuscarNombramiento(List<ListaNombramientosActivos> datosPrevios, object parametro, string elemento, bool busquedaAnterior)
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

        private List<ListaNombramientosActivos> CargarDatos(string elemento, List<ListaNombramientosActivos> datosPrevios, object parametro, bool busquedaAnterior)
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
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                        .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Division":
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                                     .Where(C => C.Division.PK_Division == dparam).ToList();
                        break;

                    case "Direccion":
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                                     .Where(C => C.DireccionGeneral.PK_DireccionGeneral == dparam).ToList();
                        break;

                    case "Seccion":
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                                     .Where(C => C.Seccion.PK_Seccion == dparam).ToList();
                        break;

                    case "Departamento":
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                                     .Where(C => C.Departamento.PK_Departamento == dparam).ToList();
                        break;

                    case "Propiedad":
                        datosPrevios = entidadBase.ListaNombramientosActivos
                                                     .Where(C => C.IndPropiedad == dparam).ToList();
                        break;

                    default:
                        datosPrevios = new List<ListaNombramientosActivos>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Division":
                        datosPrevios = datosPrevios.Where(C => C.Division.PK_Division == dparam).ToList();
                        break;

                    case "Direccion":
                        datosPrevios = datosPrevios.Where(C => C.DireccionGeneral.PK_DireccionGeneral == dparam).ToList();
                        break;

                    case "Seccion":
                        datosPrevios = datosPrevios.Where(C => C.Seccion.PK_Seccion == dparam).ToList();
                        break;

                    case "Departamento":
                        datosPrevios = datosPrevios.Where(C => C.Departamento.PK_Departamento == dparam).ToList();
                        break;

                    case "Propiedad":
                        datosPrevios = datosPrevios.Where(C => C.IndPropiedad == dparam).ToList();
                        break;

                    default:
                        datosPrevios = new List<ListaNombramientosActivos>();
                        break;
                }
            }
            return datosPrevios;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDeduccionTemporalD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDeduccionTemporalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO ObtenerTipoDeduccion(int codigo)
        {
            try
            {
                var tipos = entidadBase.TipoDeduccionTemporal.Where(Q => Q.PK_TipoDeduccionTemporal == codigo).FirstOrDefault();
                if (tipos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipos
                    };
                }
                else
                {
                    throw new Exception("No se encontró el Tipo de deducción asociado a la consulta");
                }
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

        public CRespuestaDTO ListarTiposDeduccion()
        {
            try
            {
                var tipos = entidadBase.TipoDeduccionTemporal.Where(Q=> Q.IndEstado ==1).ToList();
                if (tipos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron tipos de deducción asociados a la consulta");
                }
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

        public CRespuestaDTO AgregarDeduccionTemporal(DeduccionTemporal item)
        {
            try
            {

                var deduccionOld = entidadBase.DeduccionTemporal
                                                .Where(Q => Q.FK_Nombramiento == item.FK_Nombramiento
                                                && Q.FecRige == item.FecRige
                                                && Q.FecVence == item.FecVence
                                                && Q.NumDocumento == item.NumDocumento
                                                && Q.FK_TipoDeduccionTemporal == item.FK_TipoDeduccionTemporal
                                                && Q.IndEstado != 2) // 2 - Anulado
                                                .FirstOrDefault();

                if(deduccionOld  != null)
                    throw new Exception("Ya existe una deducción para las mismas fechas. Favor verificar");


                entidadBase.DeduccionTemporal.Add(item);
                entidadBase.SaveChanges();
                var resultado = item.PK_DeduccionTemporal;
                if (resultado > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se pudo insertar la deducción temporal indicada");
                }
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

        public CRespuestaDTO AnularDeduccionTemporal(int id)
        {
            try
            {
                var deduccion = entidadBase.DeduccionTemporal.Include("TipoDeduccionTemporal")
                                                .Where(Q => Q.PK_DeduccionTemporal == id).FirstOrDefault();
                if (deduccion != null)
                {
                    deduccion.IndEstado = 2;
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = deduccion
                    };
                }
                else
                {
                    throw new Exception("No se encontró la deducción que se intentó anular");
                }
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

        public CRespuestaDTO DescargarDetalleDeduccion(int id)
        {
            try
            {
                var deduccion = entidadBase.DeduccionTemporal
                                           .Include("TipoDeduccionTemporal")
                                           .Include("Nombramiento")
                                           .Include("Nombramiento.Funcionario")
                                           .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                           .Include("Nombramiento.Puesto")
                                           .Where(Q => Q.PK_DeduccionTemporal == id).FirstOrDefault();
                if (deduccion != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = deduccion
                    };
                }
                else
                {
                    throw new Exception("No se encontró la deducción indicada");
                }
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

        public CRespuestaDTO BuscarDeducciones(List<DeduccionTemporal> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro);
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

        private List<DeduccionTemporal> CargarDatos(string elemento, List<DeduccionTemporal> datosPrevios, object parametro)
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
                if (parametro.GetType() ==  typeof(int))
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
                        datosPrevios = entidadBase.DeduccionTemporal
                                                    .Include("TipoDeduccionTemporal")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario
                                                        == param).ToList();
                        break;
                    case "Fechas":
                        datosPrevios = entidadBase.DeduccionTemporal
                                                    .Include("TipoDeduccionTemporal")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.FecRige >= paramFechaInicio &&
                                                        C.FecRige <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "Estado":
                        if(iParam == -1) // No mostrar las Anuladas
                            datosPrevios = entidadBase.DeduccionTemporal
                                                    .Include("TipoDeduccionTemporal")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.IndEstado != 2).ToList();
                        else
                            datosPrevios = entidadBase.DeduccionTemporal
                                                   .Include("TipoDeduccionTemporal")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Funcionario")
                                                   .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                   .Include("Nombramiento.Puesto")
                                                   .Where(C => C.IndEstado == iParam).ToList();

                        break;
                    case "Tipo":
                        datosPrevios = entidadBase.DeduccionTemporal
                                                    .Include("TipoDeduccionTemporal")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.TipoDeduccionTemporal.PK_TipoDeduccionTemporal == iParam).ToList();
                        break;
                    default:
                        datosPrevios = new List<DeduccionTemporal>();
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
                        datosPrevios = datosPrevios.Where(C => C.FecRige >= paramFechaInicio &&
                                                               C.FecRige <= paramFechaFinal).ToList();
                        break;
                    case "Estado":
                        if (iParam == -1) // No mostrar las Anuladas
                            datosPrevios = datosPrevios.Where(C => C.IndEstado != 2).ToList();
                        else
                            datosPrevios = datosPrevios.Where(C => C.IndEstado == iParam).ToList();
                        break;
                    case "Tipo":
                        datosPrevios = datosPrevios.Where(C => C.TipoDeduccionTemporal.PK_TipoDeduccionTemporal 
                                                                == iParam).ToList();
                        break;
                    default:
                        datosPrevios = new List<DeduccionTemporal>();
                        break;
                }
            }

            return datosPrevios;
        }

        public CRespuestaDTO ModificarEstadoDeduccion(DeduccionTemporal registro, int indEstado)
        {
            CRespuestaDTO respuesta;

            try
            {
                var deduccionOld = entidadBase.DeduccionTemporal.Where(I => I.PK_DeduccionTemporal == registro.PK_DeduccionTemporal).FirstOrDefault();

                if (deduccionOld != null)
                {
                    deduccionOld.IndEstado = indEstado;
                    registro = deduccionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_DeduccionTemporal
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna deducción con el código especificado." }
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

        public CRespuestaDTO ModificarExplicacion(DeduccionTemporal registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var deduccionOld = entidadBase.DeduccionTemporal.Where(I => I.PK_DeduccionTemporal == registro.PK_DeduccionTemporal).FirstOrDefault();

                if (deduccionOld != null)
                {
                    deduccionOld.IndExplicacion = registro.IndExplicacion;
                    //registro = deduccionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_DeduccionTemporal
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna deducción con el código especificado." }
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
        #endregion
    }
}

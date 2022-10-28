using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Datos
{
    public class CNotificacionUsuarioD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CNotificacionUsuarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO EnviarNotificacion(NotificacionUsuario notificacion)
        {
            try
            {
                entidadBase.NotificacionUsuario.Add(notificacion);
                entidadBase.SaveChanges();
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = notificacion.PK_NotificacionUsuario
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public CRespuestaDTO ObtenerNotificacionesCedula(string cedula, int modulo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var notificaciones = entidadBase.NotificacionUsuario.Include("Usuario")
                                                    .Include("Usuario.DetalleAcceso")
                                                    .Include("Usuario.DetalleAcceso.Funcionario")
                                                    .Where(K => K.IndFuncionario == cedula && K.CodModulo == modulo).ToList();

                if (notificaciones != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = notificaciones
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna notificación." }
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

        public CRespuestaDTO ObtenerNotificacion(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var notificacion = entidadBase.NotificacionUsuario.Include("Usuario")
                                                    .Include("Usuario.DetalleAcceso")
                                                    .Include("Usuario.DetalleAcceso.Funcionario")
                                                    .FirstOrDefault(K => K.PK_NotificacionUsuario == codigo);

                if (notificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = notificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna notificación." }
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

        public CRespuestaDTO BuscarNotificaciones(List<NotificacionUsuario> datosPrevios, object parametro, string elemento)
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

        private List<NotificacionUsuario> CargarDatos(string elemento, List<NotificacionUsuario> datosPrevios, object parametro)
        {
            string param = "";
            int iparam = 0;
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
                    iparam = Convert.ToInt32(parametro);
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
                        datosPrevios = entidadBase.NotificacionUsuario
                                                    .Include("Usuario")
                                                    .Include("Usuario.DetalleAcceso")
                                                    .Include("Usuario.DetalleAcceso.Funcionario")
                                                    .Where(C => C.IndFuncionario
                                                        == param).ToList();
                        break;
                    case "FechaEnvio":
                        datosPrevios = entidadBase.NotificacionUsuario
                                                    .Include("Usuario")
                                                    .Include("Usuario.DetalleAcceso")
                                                    .Include("Usuario.DetalleAcceso.Funcionario")
                                                    .Where(C => C.FecEnvio >= paramFechaInicio &&
                                                        C.FecEnvio <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "Modulo":
                        datosPrevios = entidadBase.NotificacionUsuario
                                                    .Include("Usuario")
                                                    .Include("Usuario.DetalleAcceso")
                                                    .Include("Usuario.DetalleAcceso.Funcionario")
                                                    .Where(C => C.CodModulo == iparam).ToList();
                        break;
                    default:
                        datosPrevios = new List<NotificacionUsuario>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.IndFuncionario == param).ToList();
                        break;
                    case "FechaEnvio":
                        datosPrevios = datosPrevios.Where(C => C.FecEnvio >= paramFechaInicio &&
                                                        C.FecEnvio <= paramFechaFinal).ToList();
                        break;
                    case "Modulo":
                        datosPrevios = datosPrevios.Where(C => C.CodModulo == iparam).ToList();
                        break;
                    default:
                        datosPrevios = new List<NotificacionUsuario>();
                        break;
                }
            }

            return datosPrevios;
        }

        public CRespuestaDTO GuardarEnvioCorreos(List<TEMP_EnviarCorreo> correos)
        {
            try
            {
                foreach (var item in correos)
                {
                    entidadBase.TEMP_EnviarCorreo.Add(item);
                }

                entidadBase.SaveChanges();

                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = "Correos enviados de forma exitosa"
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
        }

        public CRespuestaDTO VerificarCorreosEnviados(DateTime fecha)
        {
            try
            {
                var dato = entidadBase.TEMP_EnviarCorreo.Count(P => P.FechaEnvio == fecha);

                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = dato
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
        }

        #endregion
    }
}

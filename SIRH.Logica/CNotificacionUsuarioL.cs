using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CNotificacionUsuarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CNotificacionUsuarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CNotificacionUsuarioDTO ConvertirDatosNotificacionADto(NotificacionUsuario item)
        {
            return new CNotificacionUsuarioDTO
            {
                IdEntidad = item.PK_NotificacionUsuario,
                Usuario = CUsuarioL.UsuarioADto(item.Usuario),
                FechaEnvio = Convert.ToDateTime(item.FecEnvio),
                Contenido = item.DesContenido,
                Destinatario = item.EmlDestino,
                Modulo = Convert.ToInt32(item.CodModulo),
                CedulaReferencia = item.IndFuncionario,
                UsuarioDestino = CUsuarioL.UsuarioADto(item.Usuario1),
                Asunto = item.IndAsunto
            };

        }

        public CBaseDTO EnviarNotificacion(CNotificacionUsuarioDTO notificacion)
        {
            try
            {
                CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

                int usuarioDestino = contexto.Usuario.FirstOrDefault(U => U.NomUsuario.Contains(notificacion.UsuarioDestino.NombreUsuario)).PK_Usuario;
                int usuarioEnvio = contexto.Usuario.FirstOrDefault(U => U.NomUsuario.Contains(notificacion.Usuario.NombreUsuario)).PK_Usuario;
                NotificacionUsuario dato = new NotificacionUsuario
                {
                    Usuario = contexto.Usuario.FirstOrDefault(Q => Q.PK_Usuario == usuarioEnvio),
                    FecEnvio = DateTime.Now,
                    DesContenido = notificacion.Contenido,
                    EmlDestino = notificacion.Destinatario,
                    CodModulo = notificacion.Modulo,
                    IndFuncionario = notificacion.CedulaReferencia,
                    Usuario1 = contexto.Usuario.FirstOrDefault(Q => Q.PK_Usuario == usuarioDestino),
                    IndAsunto = notificacion.Asunto
                };

                var respuesta = intermedio.EnviarNotificacion(dato);

                if (respuesta.Codigo > 0)
                {
                    return new CBaseDTO { IdEntidad = Convert.ToInt32(respuesta.Contenido) };
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ObtenerNotificacionCedula(string cedula, int modulo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var notificacion = intermedio.ObtenerNotificacionesCedula(cedula, modulo);

                if (notificacion.Codigo > 0)
                {
                    var datoNotificacion = ConvertirDatosNotificacionADto(((List<NotificacionUsuario>)notificacion.Contenido).OrderByDescending(F => F.FecEnvio).FirstOrDefault());

                    respuesta = datoNotificacion;
                }
                else
                {
                    respuesta = (CErrorDTO)notificacion.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerNotificacion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var notificacion = intermedio.ObtenerNotificacion(codigo);

                if (notificacion.Codigo > 0)
                {
                    var datoNotificacion = ConvertirDatosNotificacionADto((NotificacionUsuario)notificacion.Contenido);

                    respuesta.Add(datoNotificacion);

                    var funcionario = contexto.Funcionario.FirstOrDefault(Q => Q.IdCedulaFuncionario == datoNotificacion.CedulaReferencia);

                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario));
                }
                else
                {
                    respuesta.Add((CErrorDTO)notificacion.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarNotificaciones(CFuncionarioDTO funcionario, CNotificacionUsuarioDTO notificacion,
                                                List<DateTime> fechasEnvio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

            List<NotificacionUsuario> datosNotificaciones = new List<NotificacionUsuario>();

            if (funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarNotificaciones(datosNotificaciones, funcionario.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosNotificaciones = (List<NotificacionUsuario>)resultado.Contenido;
                }
            }

            if (notificacion.Modulo != 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarNotificaciones(datosNotificaciones, notificacion.Modulo, "Modulo"));
                if (resultado.Codigo > 0)
                {
                    datosNotificaciones = (List<NotificacionUsuario>)resultado.Contenido;
                }
            }

            if (fechasEnvio.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarNotificaciones(datosNotificaciones, fechasEnvio, "FechaEnvio"));
                if (resultado.Codigo > 0)
                {
                    datosNotificaciones = (List<NotificacionUsuario>)resultado.Contenido;
                }
            }

            if (datosNotificaciones.Count > 0)
            {
                foreach (var item in datosNotificaciones)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    var datoNotificacion = ConvertirDatosNotificacionADto(item);

                    CNotificacionUsuarioDTO tempNotificacion = datoNotificacion;

                    temp.Add(datoNotificacion);

                    CFuncionarioDTO tempFuncionario = CFuncionarioL.FuncionarioGeneral(contexto.Funcionario.FirstOrDefault(Q => Q.IdCedulaFuncionario == datoNotificacion.CedulaReferencia));

                    temp.Add(tempFuncionario);

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }

        public CBaseDTO GuardarEnvioCorreos(List<CTemp_EnviarCorreoDTO> correos)
        {
            CBaseDTO respuesta;
            try
            {
                CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

                List<TEMP_EnviarCorreo> listaCorreos = new List<TEMP_EnviarCorreo>();

                foreach (var item in correos)
                {
                    var correo = new TEMP_EnviarCorreo
                    {
                        Cedula = item.Cedula,
                        Correo = item.Correo,
                        FechaEnvio = item.FechaEnvio,
                        IdMotivo = item.IdMotivo,
                        Nombre = item.Nombre
                    };

                    listaCorreos.Add(correo);
                }

                var resultado = intermedio.GuardarEnvioCorreos(listaCorreos);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        Mensaje = resultado.Contenido.ToString()
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };

                return respuesta;
            }
        }

        public CBaseDTO VerificarCorreosEnviados(DateTime fecha)
        {
            CBaseDTO respuesta;
            try
            {
                CNotificacionUsuarioD intermedio = new CNotificacionUsuarioD(contexto);

                var resultado = intermedio.VerificarCorreosEnviados(fecha);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        Mensaje = resultado.Contenido.ToString()
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };

                return respuesta;
            }
        }

        #endregion
    }
}

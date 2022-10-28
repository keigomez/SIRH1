using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CUsuarioL
    {
        #region Variables

        SIRHEntities contexto;
        CUsuarioD usuarioDescarga;

        #endregion

        #region Constructor

        public CUsuarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        //Se insertó en ICFuncionarioService y CFuncionarioService 31/01/2017
        public List<List<CBaseDTO>> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();
            List<CBaseDTO> listaError = new List<CBaseDTO>();
            try
            {
                usuarioDescarga = new CUsuarioD(contexto);

                CRespuestaDTO usuarioBD = usuarioDescarga.CargarPerfilUsuario(nombreUsuario);
                List<CBaseDTO> listaUsuario = new List<CBaseDTO>();

                if (usuarioBD.Contenido.GetType() != typeof(CErrorDTO))
                {
                    if (((Usuario)usuarioBD.Contenido).DetalleAcceso.Count > 0)
                    {
                        listaUsuario.Add(UsuarioADto(((Usuario)usuarioBD.Contenido)));
                        resultado.Add(listaUsuario);
                        List<CBaseDTO> listaFuncionario = new List<CBaseDTO>();
                        listaFuncionario.Add(CFuncionarioL.FuncionarioGeneral(((Usuario)usuarioBD.Contenido).DetalleAcceso.FirstOrDefault().Funcionario));
                        resultado.Add(listaFuncionario);

                        foreach (var item in ((Usuario)usuarioBD.Contenido).DetalleAcceso)
                        {
                            List<CBaseDTO> listaPerfiles = new List<CBaseDTO>();

                            var datos = item.PerfilAcceso.OrderBy(Q => Q.CatPermiso.Perfil.PK_Perfil);

                            foreach (var ordenado in datos)
                            {
                                if (listaPerfiles.Count > 0)
                                {
                                    if (listaPerfiles.FirstOrDefault().IdEntidad == ordenado.CatPermiso.Perfil.PK_Perfil)
                                    {
                                        listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                                    }
                                    else
                                    {
                                        resultado.Add(listaPerfiles);
                                        listaPerfiles = new List<CBaseDTO>();
                                        listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                                        listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                                    }
                                }
                                else
                                {
                                    listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                                    listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                                }
                            }

                            if (listaPerfiles.Count > 0)
                            {
                                resultado.Add(listaPerfiles);
                            }
                        }

                        return resultado;
                    }
                    else
                    {
                        listaUsuario.Add(UsuarioADto(((Usuario)usuarioBD.Contenido)));
                        resultado.Add(listaUsuario);
                        listaError.Add(new CErrorDTO { MensajeError = "El usuario está registrado en el sistema, pero no se le ha asignado ningún perfil para el uso del mismo" });
                        resultado.Add(listaError);
                        return resultado;
                    }
                }
                else
                {
                    listaError.Add(((CErrorDTO)usuarioBD.Contenido));
                    resultado.Add(listaError);
                    return resultado;
                }
            }
            catch (Exception error)
            {
                listaError.Add(new CErrorDTO { MensajeError = error.Message });
                resultado.Add(listaError);
                return resultado;
            }
        }        

        //Se insertó en ICFuncionarioService y CFuncionarioService el 31/01/2017
        public List<List<CBaseDTO>> CargarPerfilCompletoUsuario(string nombreUsuario, int catPermiso,
                                                int perfil)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> listaError = new List<CBaseDTO>();
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);
                var dato = intermedio.CargarPerfilEspecificoUsuario(nombreUsuario, catPermiso,
                                                                    perfil);
                List<CBaseDTO> listaUsuario = new List<CBaseDTO>();

                if (dato.Contenido.GetType() != typeof(CErrorDTO))
                {
                    List<CBaseDTO> listaFuncionario = new List<CBaseDTO>();
                    listaFuncionario.Add(CFuncionarioL.FuncionarioGeneral(((DetalleAcceso)dato.Contenido).Funcionario));
                    respuesta.Add(listaFuncionario);

                    List<CBaseDTO> listaPerfiles = new List<CBaseDTO>();

                    var datos = ((DetalleAcceso)dato.Contenido).PerfilAcceso.OrderBy(Q => Q.CatPermiso.Perfil.PK_Perfil);

                    foreach (var ordenado in datos)
                    {
                        if (ordenado.CatPermiso.Perfil.PK_Perfil == perfil ||
                            ordenado.CatPermiso.Perfil.PK_Perfil == 1)
                        {
                            if (listaPerfiles.Count > 0)
                            {
                                if (listaPerfiles.FirstOrDefault().IdEntidad == ordenado.CatPermiso.Perfil.PK_Perfil)
                                {
                                    listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                                }
                                else
                                {
                                    respuesta.Add(listaPerfiles);
                                    listaPerfiles = new List<CBaseDTO>();
                                    listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                                    listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                                }
                            }
                            else
                            {
                                listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                                listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                            }
                        }
                    }

                    if (listaPerfiles.Count > 0)
                    {
                        respuesta.Add(listaPerfiles);
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)dato.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                listaError.Add(new CErrorDTO { MensajeError = error.Message });
                respuesta.Add(listaError);
                return respuesta;
            }
        }        

        //Se insertó en ICFuncionarioService y CFuncionarioService el 31/01/2017
        public List<CBaseDTO> AsignarAccesosUsuario(string nombreUsuario, 
                                                    List<CCatPermisoDTO> permisos)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);

                List<CatPermiso> listaPermisos = new List<CatPermiso>();

                foreach (var item in permisos)
                {
                    listaPermisos.Add(contexto.CatPermiso.Where(Q => Q.PK_CatPermiso == item.IdEntidad).FirstOrDefault());
                }

                var resultado = intermedio.AsignarAccesosUsuario(nombreUsuario, listaPermisos);

                if(resultado.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta.Add(UsuarioADto(((DetalleAcceso)resultado.Contenido).Usuario));
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(((DetalleAcceso)resultado.Contenido).Funcionario));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch(Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        //Se insertó en ICFuncionarioService y CFuncionarioService el 31/01/2017
        public List<List<CBaseDTO>> DescargarUsuario(string nombreUsuario, string cedulaFunc)
        {
            try
            {
                List<List<CBaseDTO>> resultadoTotal = new List<List<CBaseDTO>>();
                usuarioDescarga = new CUsuarioD(contexto);
                var item = usuarioDescarga.CargarUsuariosParam(nombreUsuario, cedulaFunc);
                if (item != null) {
                    foreach (var aux in item)
                    {
                        List<CBaseDTO> resultado = new List<CBaseDTO>();
                        if (aux.DetalleAcceso.Count > 0)
                        {
                            resultado.Add(new CFuncionarioDTO
                            {
                                Cedula = aux.DetalleAcceso.FirstOrDefault().Funcionario.IdCedulaFuncionario,
                                Nombre = aux.DetalleAcceso.FirstOrDefault().Funcionario.NomFuncionario,
                                PrimerApellido = aux.DetalleAcceso.FirstOrDefault().Funcionario.NomPrimerApellido,
                                SegundoApellido = aux.DetalleAcceso.FirstOrDefault().Funcionario.NomSegundoApellido,
                                Sexo = GeneroEnum.Indefinido
                            });
                            resultado.Add(new CUsuarioDTO
                            {
                                NombreUsuario = aux.DetalleAcceso.FirstOrDefault().Usuario.NomUsuario,
                                EmailOficial = aux.DetalleAcceso.FirstOrDefault().Usuario.EmlOficial
                            });
                        }
                        resultadoTotal.Add(resultado);
                    }

                    return resultadoTotal;
                }else
                {
                    throw new Exception("Ocurrió un error al cargar el usuario, contactar al personal autorizado.");
                }
                
            }
            catch (Exception error)
            {
                List<List<CBaseDTO>> resultadoTotal = new List<List<CBaseDTO>>();
                List<CBaseDTO> resultado = new List<CBaseDTO>();
                resultado.Add(new CErrorDTO {
                   MensajeError = error.Message 
                });
                resultadoTotal.Add(resultado);
                return resultadoTotal;
            }
        }

        public CBaseDTO CargarPerfilEspecificoUsuario(string nombreUsuario, int catPermiso, int perfil)
        {
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);
                var dato = intermedio.CargarPerfilEspecificoUsuario(nombreUsuario, catPermiso,
                                                                    perfil);
                if (dato.GetType() != typeof(CErrorDTO))
                {
                    return new CBaseDTO { Mensaje = "true" };
                }
                else
                {
                    throw new Exception(((CErrorDTO)dato.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistrarUsuario(string nombreUsuario, string cedula)
        {
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);

                var resultado = intermedio.RegistrarUsuario(nombreUsuario, cedula);

                if (resultado.Contenido.GetType() != typeof(CErrorDTO))
                {
                    return UsuarioADto(((Usuario)resultado.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        internal static CUsuarioDTO UsuarioADto(Usuario item)
        {
            return new CUsuarioDTO 
            {
                NombreUsuario = item.NomUsuario,
                IdEntidad = item.PK_Usuario,
                TelefonoOficial = item.TelOficial,
                EmailOficial = item.EmlOficial
            };
        }


        public List<List<CBaseDTO>> CargarUsuariosPerfil(int idPerfil, int catPermiso)
        {    
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> listaError = new List<CBaseDTO>();
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);
                var dato = intermedio.CargarUsuariosPerfil(idPerfil, catPermiso);

                if (dato.Contenido.GetType() != typeof(CErrorDTO))
                {
                    var lista = (List<DetalleAcceso>)dato.Contenido;

                    foreach (var item in lista)
                    {
                        List<CBaseDTO> listaFuncionario = new List<CBaseDTO>();

                        listaFuncionario.Add(CFuncionarioL.FuncionarioGeneral(((DetalleAcceso)item).Funcionario));
                        listaFuncionario.Add(CUsuarioL.UsuarioADto(((DetalleAcceso)item).Usuario));

                        respuesta.Add(listaFuncionario);
                        
                        //List<CBaseDTO> listaPerfiles = new List<CBaseDTO>();

                        //var datos = ((DetalleAcceso)item).PerfilAcceso
                        //    //.Where(Q => Q.CatPermiso.Perfil.PK_Perfil == idPerfil)
                        //                                .OrderBy(Q => Q.CatPermiso.Perfil.PK_Perfil);

                        //foreach (var ordenado in datos)
                        //{
                        //    listaFuncionario.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));

                        //    //if (listaPerfiles.Count > 0)
                        //    //{
                        //    //    if (listaPerfiles.FirstOrDefault().IdEntidad == ordenado.CatPermiso.Perfil.PK_Perfil)
                        //    //    {
                                    
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        respuesta.Add(listaPerfiles);
                        //    //        listaPerfiles = new List<CBaseDTO>();
                        //    //        listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                        //    //        listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                        //    //    }
                        //    //}
                        //    //else
                        //    //{
                        //    //    listaPerfiles.Add(CPerfilL.PerfilADto(ordenado.CatPermiso.Perfil));
                        //    //    listaPerfiles.Add(CCatPermisoL.PermisoADto(ordenado.CatPermiso));
                        //    //}    
                            
                        //}

                        //if (listaPerfiles.Count > 0)
                        //{
                        //    respuesta.Add(listaPerfiles);
                        //}

                        //respuesta.Add(listaFuncionario);
                    }
                }
                else
                {
                    throw new Exception(((CErrorDTO)dato.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                listaError.Add(new CErrorDTO { MensajeError = error.Message });
                respuesta.Add(listaError);
            }

            return respuesta;
        }  

        public CBaseDTO DeshabilitarUsuario(string nombreUsuario,string observacion)
        {
            try
            {
                CUsuarioD intermedio = new CUsuarioD(contexto);
                var resultado = intermedio.DeshabilitarUsuario(nombreUsuario,observacion);
                if (resultado.Contenido.GetType() != typeof(CErrorDTO))
                {
                    return UsuarioADto(((Usuario)resultado.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
                
            }
            catch(Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
            
        }

        #endregion
    }
}

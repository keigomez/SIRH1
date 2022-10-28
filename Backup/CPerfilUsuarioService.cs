using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Servicios
{
    // NOTE: If you change the class name "CPerfilUsuarioService" here, you must also update the reference to "CPerfilUsuarioService" in App.config.
    public class CPerfilUsuarioService : ICPerfilUsuarioService
    {
        public List<List<CBaseDTO>> DescargarUsuario(string nombreUsuario, string cedulaFunc)
        {
            CUsuarioL logica = new CUsuarioL();
            return logica.DescargarUsuario(nombreUsuario, cedulaFunc);
        }

        public List<CBaseDTO> AsignarAccesosUsuario(string nombreUsuario, List<CCatPermisoDTO> permisos)
        {
            CUsuarioL logica = new CUsuarioL();
            return logica.AsignarAccesosUsuario(nombreUsuario, permisos);
        } 

        public List<List<CBaseDTO>> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            CUsuarioL logica = new CUsuarioL();
            return logica.ObtenerUsuarioPorNombre(nombreUsuario);
        }

        public List<List<CBaseDTO>> RetornarPerfilUsuario(string nombreUsuario)
        {
            CUsuarioL usuarioLogica = new CUsuarioL();
            return usuarioLogica.ObtenerUsuarioPorNombre(nombreUsuario);
        }
        
        public CBaseDTO CargarPerfilUsuarioEspecifico(string nombreUsuario, int catPermiso, 
                                                        int perfil)
        {
            CUsuarioL usuarioLogica = new CUsuarioL();
            return usuarioLogica.CargarPerfilEspecificoUsuario(nombreUsuario, catPermiso, perfil);
        }

        public CBaseDTO RegistrarUsuario(string nombreUsuario, string cedula)
        {
            CUsuarioL usuarioLogica = new CUsuarioL();
            return usuarioLogica.RegistrarUsuario(nombreUsuario, cedula);
        }
        
        public List<CBaseDTO> DescargarPerfiles()
        {
            CPerfilL perfilLogica = new CPerfilL();
            return perfilLogica.DescargarPerfiles();
        }

        public CBaseDTO AgregarPerfil(CPerfilDTO perfil)
        {
            CPerfilL perfilLogica = new CPerfilL();
            return perfilLogica.AgregarPerfil(perfil);
        }

        public List<CBaseDTO> DescargarPermisosPerfil(CPerfilDTO perfil)
        {
            CCatPermisoL permisoLogica = new CCatPermisoL();
            return permisoLogica.DescargarPermisosPerfil(perfil);
        }

        public CBaseDTO AgregarPermiso(CCatPermisoDTO permiso, CPerfilDTO perfil)
        {
            CCatPermisoL permisoLogica = new CCatPermisoL();
            return permisoLogica.AgregarPermiso(permiso, perfil);
        }
        
        public List<List<CBaseDTO>> CargarPerfilUsuarioCompleto(string nombreUsuario, int catPermiso, int perfil)
        {
            CUsuarioL usuarioLogica = new CUsuarioL();
            return usuarioLogica.CargarPerfilCompletoUsuario(nombreUsuario, catPermiso, perfil);
        }

        public List<List<CBaseDTO>> CargarUsuariosPerfil(int idPerfil)
        {
            CUsuarioL usuarioLogica = new CUsuarioL();
            return usuarioLogica.CargarUsuariosPerfil(idPerfil);
        }
    }
}

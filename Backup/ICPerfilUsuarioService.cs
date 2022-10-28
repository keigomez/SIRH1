using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICPerfilUsuarioService" here, you must also update the reference to "ICPerfilUsuarioService" in App.config.
    [ServiceContract]
    public interface ICPerfilUsuarioService
    {                      

        [OperationContract]
        List<List<CBaseDTO>> DescargarUsuario(string nombreUsuario, string cedulaFunc);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerUsuarioPorNombre(string nombreUsuario);

        [OperationContract]
        List<List<CBaseDTO>> RetornarPerfilUsuario(string nombreUsuario);

        [OperationContract]
        CBaseDTO CargarPerfilUsuarioEspecifico(string nombreUsuario, int catPermiso, int perfil);

        [OperationContract]
        List<List<CBaseDTO>> CargarPerfilUsuarioCompleto(string nombreUsuario, int catPermiso, int perfil);

        [OperationContract]
        CBaseDTO RegistrarUsuario(string nombreUsuario, string cedula);

        [OperationContract]
        List<CBaseDTO> AsignarAccesosUsuario(string nombreUsuario, List<CCatPermisoDTO> permisos);

        [OperationContract]
        List<CBaseDTO> DescargarPerfiles();

        [OperationContract]
        CBaseDTO AgregarPerfil(CPerfilDTO perfil);

        [OperationContract]
        List<CBaseDTO> DescargarPermisosPerfil(CPerfilDTO perfil);

        [OperationContract]
        CBaseDTO AgregarPermiso(CCatPermisoDTO permiso, CPerfilDTO perfil);

        [OperationContract]
        List<List<CBaseDTO>> CargarUsuariosPerfil(int idPerfil);
    }
}

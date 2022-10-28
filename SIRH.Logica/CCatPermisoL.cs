using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCatPermisoL
    {
        #region Variables

        SIRHEntities contexto;
        CCatPermisoD permisoDescarga;

        #endregion

        #region Constructor

        public CCatPermisoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        public List<CBaseDTO> DescargarPermisosPerfil(CPerfilDTO perfil)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            
            try
            {
                permisoDescarga = new CCatPermisoD(contexto);

                var permisos = permisoDescarga.DescargarPermisosPerfil(perfil.IdEntidad);

                if (permisos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<CatPermiso>)permisos.Contenido))
                    {
                        respuesta.Add(PermisoADto(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)permisos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO AgregarPermiso(CCatPermisoDTO permiso, CPerfilDTO perfil)
        {
            try
            {
                permisoDescarga = new CCatPermisoD(contexto);

                CatPermiso permisoAgregar = new CatPermiso
                {
                    NomPermiso = permiso.NomPermiso,
                    DesPermiso = permiso.DesPermiso,
                    IndEstadoCatPermiso = Convert.ToInt32(permiso.IndEstCatPermiso)
                };

                var datos = permisoDescarga.AgregarPermiso(permisoAgregar, perfil.IdEntidad);

                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    return PermisoADto(((CatPermiso)datos.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        internal static CCatPermisoDTO PermisoADto(CatPermiso item)
        {
            return new CCatPermisoDTO 
            {
                IdEntidad = item.PK_CatPermiso,
                NomPermiso = item.NomPermiso,
                DesPermiso = item.DesPermiso,
                IndEstCatPermiso = Convert.ToInt32(item.IndEstadoCatPermiso),
                Perfil = item.Perfil.NomPerfil
            };
        }

        public List<CBaseDTO> ListarPermisos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                permisoDescarga = new CCatPermisoD(contexto);

                var permisos = permisoDescarga.ListarPermisos();

                if (permisos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<CatPermiso>)permisos.Contenido))
                    {
                        respuesta.Add(PermisoADto(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)permisos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
    }
}
                
                      
            
           

            
    


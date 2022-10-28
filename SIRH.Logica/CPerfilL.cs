using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPerfilL
    {
        #region Variables

        SIRHEntities contexto;
        CPerfilD perfilDescarga;

        #endregion

        #region Constructor

        public CPerfilL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public List<CBaseDTO> DescargarPerfiles()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                perfilDescarga = new CPerfilD(contexto);

                var perfiles = perfilDescarga.DescargarPerfiles();

                if (perfiles.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<Perfil>)perfiles.Contenido))
                    {
                        respuesta.Add(PerfilADto(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)perfiles.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO AgregarPerfil(CPerfilDTO perfil)
        {
            try
            {
                perfilDescarga = new CPerfilD(contexto);

                Perfil perfilBD = new Perfil 
                {
                    NomPerfil = perfil.NomPerfil,
                    DesPerfil = perfil.DesPerfil
                };

                var datos = perfilDescarga.AgregarPerfil(perfilBD);

                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    return PerfilADto(((Perfil)datos.Contenido));
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

        public List<List<CBaseDTO>> DescargarPerfilesUsuario(string nombreUsuario)
        {
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            perfilDescarga = new CPerfilD(contexto);

            var perfiles = perfilDescarga.DescargarPerfilesUsuario(nombreUsuario);

            if (perfiles != null)
            {
                foreach (var item in perfiles)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CPerfilDTO { IdEntidad = item.PK_Perfil, NomPerfil = item.NomPerfil, DesPerfil = item.DesPerfil });
                    foreach (var aux in item.CatPermiso)
                    {
                        temp.Add(new CCatPermisoDTO { IdEntidad = aux.PK_CatPermiso, NomPermiso = aux.NomPermiso, DesPermiso = aux.DesPermiso });
                    }
                    resultado.Add(temp);
                }
            }

            return resultado;
        }

        internal static CPerfilDTO PerfilADto(Perfil item)
        {
            return new CPerfilDTO 
            {
                IdEntidad = item.PK_Perfil,
                NomPerfil = item.NomPerfil,
                DesPerfil = item.DesPerfil
            };
        }


        #endregion
    }
}



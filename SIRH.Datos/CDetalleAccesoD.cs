using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CDetalleAccesoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDetalleAccesoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Guarda los detalles del acceso
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="detalle"></param>
        /// <param name="perfiles"></param>
        /// <returns>Retorna Detalles de Acceso</returns>
        public int GuardarDetalleAcceso(Usuario usuario, DetalleAcceso detalle, List<PerfilAcceso> perfiles)
        {
            entidadBase.Usuario.Add(usuario);
            entidadBase.DetalleAcceso.Add(detalle);
            foreach (var item in perfiles)
            {
                entidadBase.PerfilAcceso.Add(item);
            }

            return entidadBase.SaveChanges();
        }

        /// <summary>
        ///  Actualiza Detalle del Acceso
        /// </summary>
        /// <param name="detalle"></param>
        /// <param name="perfiles"></param>
        /// <returns>Retorna Detalles de Acceso</returns>
        public int ActualizarDetalleAcceso(DetalleAcceso detalle, List<PerfilAcceso> perfiles)
        {
            List<PerfilAcceso> temp = new List<PerfilAcceso>();
            foreach (var item in perfiles)
            {
                if (detalle.PerfilAcceso.Where(Q => Q.CatPermiso.PK_CatPermiso.Equals(item.CatPermiso.PK_CatPermiso)).Count() < 1)
                {
                    detalle.PerfilAcceso.Add(item);
                    entidadBase.PerfilAcceso.Add(item);
                }
            }

            return entidadBase.SaveChanges();
        }

        /// <summary>
        /// Carga el Perfil del Usuario accesado de la BD
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        public DetalleAcceso CargarPerfilUsuario(string nombreUsuario)
        {
            var resultado = entidadBase.DetalleAcceso.Include("Usuario").
                                                        Include("Funcionario").
                                                        Include("PerfilAcceso").
                                                        Include("PerfilAcceso.CatPermiso").
                                                        Include("PerfilAcceso.CatPermiso.Perfil").Where(Q => Q.Usuario.NomUsuario.ToLower().Equals(nombreUsuario.ToLower())).FirstOrDefault();

            resultado = CargarPerfilFaltanteUsuario(nombreUsuario);
            if (resultado == null)
            {
               resultado = new DetalleAcceso();
            }

            return resultado;
        }

        //Comprueba que no esté vacío
        public DetalleAcceso CargarPerfilFaltanteUsuario(string nombreUsuario)
        {
            var resultado = entidadBase.DetalleAcceso.Include("Usuario").
                                                        Include("Funcionario").
                                                        Include("PerfilAcceso").
                                                        Include("PerfilAcceso.CatPermiso").
                                                        Include("PerfilAcceso.CatPermiso.Perfil").Where(Q => Q.Usuario.NomUsuario.ToLower().Equals(nombreUsuario.ToLower())).FirstOrDefault();
            if (resultado == null)
            {
                resultado = new DetalleAcceso();
            }

            return resultado;
        }
        #endregion
    }
}

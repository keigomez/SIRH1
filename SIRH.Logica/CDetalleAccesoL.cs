using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleAccesoL
    {
        #region Variables

        SIRHEntities contexto;
        CDetalleAccesoD accesoDescarga;

        #endregion

        #region Constructor

        public CDetalleAccesoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public List<List<ObjetosEntidad>> DescargarPerfilUsuario(string nombreUsuario)
        {
            List<List<ObjetosEntidad>> resultado = new List<List<ObjetosEntidad>>();
            accesoDescarga = new CDetalleAccesoD(contexto);
            var item = accesoDescarga.CargarPerfilUsuario(nombreUsuario);

            if (item != null && item.Usuario != null)
            {
                var idPerfil = item.PerfilAcceso.Select(Q => Q.CatPermiso.Perfil.PK_Perfil);
                var nomPerfil = item.PerfilAcceso.Select(Q => Q.CatPermiso.Perfil.NomPerfil);
                var idsDistintos = idPerfil.Distinct();
                var nombresDistintos = nomPerfil.Distinct();

                for (int i = 0; i < idsDistintos.Count(); i++)
                {
                    List<ObjetosEntidad> temp = new List<ObjetosEntidad>();
                    temp.Add(new ObjetosEntidad { Encabezado = "Perfil", Valor = nombresDistintos.ElementAt(i) });
                    var permisos = item.PerfilAcceso.Where(Q => Q.CatPermiso.Perfil.PK_Perfil.Equals(idsDistintos.ElementAt(i)));
                    foreach (var permiso in permisos)
                    {
                        temp.Add(new ObjetosEntidad { Encabezado = "Permiso", Valor = permiso.CatPermiso.PK_CatPermiso + "-" + permiso.CatPermiso.NomPermiso  });
                    }
                    //resultado.Add(aux);
                }
            }

            return resultado;
        }

        public int AlmacenarPerfilesAcceso(List<ObjetosEntidad> usuario, List<ObjetosEntidad> detalleAcceso, List<List<ObjetosEntidad>> perfilesAcceso)
        {
            accesoDescarga = new CDetalleAccesoD(contexto);

            Usuario usuarioDatos = new Usuario
            {
                NomUsuario = usuario.Where(Q => Q.Encabezado.Equals("NomUsuario")).FirstOrDefault().Valor.ToString(),
                TelOficial = usuario.Where(Q => Q.Encabezado.Equals("TelOficial")).FirstOrDefault().Valor.ToString(),
                EmlOficial = usuario.Where(Q => Q.Encabezado.Equals("EmlOficial")).FirstOrDefault().Valor.ToString()
            };                          

            string cedula = detalleAcceso.Where(R => R.Encabezado.Equals("Funcionario")).FirstOrDefault().Valor.ToString();
            DetalleAcceso detalleDatos = new DetalleAcceso
            {
                Funcionario = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario.Equals(cedula)).FirstOrDefault(),
                Usuario = usuarioDatos,
                FecCreacion = DateTime.Now
            };

            List<PerfilAcceso> perfilesDatos = new List<PerfilAcceso>();
            foreach (var item in perfilesAcceso)
            {
                int idPerfil = Convert.ToInt32(item.Where(R => R.Encabezado.Equals("Permiso")).FirstOrDefault().Valor);
                PerfilAcceso temp = new PerfilAcceso
                {
                    DetalleAcceso = detalleDatos,
                    CatPermiso = contexto.CatPermiso.Where(Q => Q.PK_CatPermiso.Equals(idPerfil)).FirstOrDefault(),
                    FecAsignacion = DateTime.Now
                };
                perfilesDatos.Add(temp);
            }

            return accesoDescarga.GuardarDetalleAcceso(usuarioDatos, detalleDatos, perfilesDatos);
        }

        public int ActualizarPerfilesAcceso(List<ObjetosEntidad> detalleAcceso, List<List<ObjetosEntidad>> perfilesAcceso)
        {
            accesoDescarga = new CDetalleAccesoD(contexto);

            //BUSCAR EL DETALLE DE ACCESO EN EL CONTEXTO
            string cedula = detalleAcceso.Where(R => R.Encabezado.Equals("Funcionario")).FirstOrDefault().Valor.ToString();
            string nomUsuario = detalleAcceso.Where(R => R.Encabezado.Equals("Usuario")).FirstOrDefault().Valor.ToString();
            DetalleAcceso detalleDatos = contexto.DetalleAcceso.Include("PerfilAcceso").Include("PerfilAcceso.CatPermiso").Where(Q => Q.Usuario.NomUsuario.Equals(nomUsuario) 
                                                        && Q.Funcionario.IdCedulaFuncionario.Equals(cedula)).FirstOrDefault();

            List<PerfilAcceso> perfilesDatos = new List<PerfilAcceso>();
            foreach (var item in perfilesAcceso)
            {
                int idPerfil = Convert.ToInt32(item.Where(R => R.Encabezado.Equals("Permiso")).FirstOrDefault().Valor);
                int pkdetalle = detalleDatos.PK_DetalleAcceso;
                PerfilAcceso aux = contexto.PerfilAcceso.Where(Q => Q.CatPermiso.PK_CatPermiso.Equals(idPerfil) && Q.DetalleAcceso.PK_DetalleAcceso.Equals(pkdetalle)).FirstOrDefault();

                if (!(detalleDatos.PerfilAcceso.Contains(aux)))
                {
                    PerfilAcceso temp = new PerfilAcceso
                    {
                        DetalleAcceso = detalleDatos,
                        CatPermiso = contexto.CatPermiso.Where(Q => Q.PK_CatPermiso.Equals(idPerfil)).FirstOrDefault(),
                        FecAsignacion = DateTime.Now
                    };
                    perfilesDatos.Add(temp);
                }
            }

            List<PerfilAcceso> perfilTemp = detalleDatos.PerfilAcceso.ToList();
            if (perfilTemp.Count > perfilesAcceso.Count)
            {
                foreach (var item in perfilTemp)
                {
                    string catPermisoString = item.CatPermiso.PK_CatPermiso.ToString();
                    var algo = perfilesAcceso.Where(Q => Q.Where(R => R.Valor.Equals(catPermisoString)).Count() > 0).FirstOrDefault();
                    if (algo == null)
                    {
                        contexto.PerfilAcceso.Remove(item);
                    }
                }
            }

            return accesoDescarga.ActualizarDetalleAcceso(detalleDatos, perfilesDatos);
        }

        #endregion
    }
}

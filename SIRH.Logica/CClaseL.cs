using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CClaseL
    {
        #region Variables

        SIRHEntities contexto;
        CClaseD claseDescarga;

        #endregion

        #region Constructor

        public CClaseL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //registrar en PuestoService el 30/01/2017
        public List<CClaseDTO> DescargarClases(int codigo, string nombre)
        {
            List<CClaseDTO> resultado = new List<CClaseDTO>();
            claseDescarga = new CClaseD(contexto);
            var item = claseDescarga.CargarClasesParams(codigo, nombre);

            foreach (var aux in item)
            {
                CClaseDTO temp = new CClaseDTO();
                temp.IdEntidad = aux.PK_Clase;
                temp.DesClase = aux.DesClase;
                temp.IndEstClase = Convert.ToInt32(aux.IndEstadoClase);
                temp.IndCategoria = Convert.ToInt32(aux.IndCategoria);
                resultado.Add(temp);
            }
            return resultado;
        }

        internal static CClaseDTO ConstruirClase(Clase entrada)
        {
            return new CClaseDTO
            {
                IdEntidad = entrada.PK_Clase,
                DesClase = entrada.DesClase,
                IndEstClase = (entrada.IndEstadoClase != null) ? Convert.ToInt32(entrada.IndEstadoClase) : Convert.ToInt32("0"),
                IndCategoria = (entrada.IndCategoria != null) ? Convert.ToInt32(entrada.IndCategoria) : Convert.ToInt32("0")
            };
        }

        public List<CBaseDTO> ListarClasesConFormato(string formato)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CClaseD intermedio = new CClaseD(contexto);

            var clases = intermedio.ListarClasesConFormato(formato);

            if (clases.Codigo != -1)
            {
                foreach (var item in (List<Clase>)clases.Contenido)
                {
                    respuesta.Add(new CClaseDTO
                    {
                        IdEntidad = item.PK_Clase,
                        DesClase = item.DesClase
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)clases.Contenido);
            }

            return respuesta;
        }

        #endregion
    }
}

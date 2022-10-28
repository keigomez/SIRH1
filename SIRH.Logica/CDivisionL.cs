using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDivisionL
    {
        #region Variables

        SIRHEntities contexto;
        CDivisionD DivisionDescarga;

        #endregion

        #region Constructor

        public CDivisionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //Se insertó en PuestoService el 30/01/2017
        public List<CDivisionDTO> DescargarDivisions(int codigo, string nombre)
        {
            List<CDivisionDTO> resultado = new List<CDivisionDTO>();
            DivisionDescarga = new CDivisionD(contexto);
            var item = DivisionDescarga.CargarDivisionesParam(codigo, nombre);

            foreach (var aux in item)
            {
                CDivisionDTO temp = new CDivisionDTO();

                temp.IdEntidad = aux.PK_Division;
                temp.NomDivision = aux.NomDivision;

                resultado.Add(temp);
            }

            return resultado;
        }

        public List<CBaseDTO> ListarDivisiones()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CDivisionD intermedio = new CDivisionD(contexto);
            var divisiones = intermedio.CargarDivisiones();
            if (divisiones != null)
            {
                foreach (var item in divisiones)
                {
                    respuesta.Add(new CDivisionDTO
                    {
                        NomDivision = item.NomDivision,
                        IdEntidad = item.PK_Division

                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron divisiones" });
            }
            return respuesta;
        }

        internal static CDivisionDTO ConvertirDivisionADTO(Division item)
        {
            CDivisionDTO respuesta = new CDivisionDTO
            {
                IdEntidad = item.PK_Division,
                NomDivision = item.NomDivision,


            };
            return respuesta;
        }

        internal static Division ConvertirCDivisionDTOaDatos(CDivisionDTO item)
        {
            Division respuesta = new Division
            {
                PK_Division = item.IdEntidad,
                NomDivision = item.NomDivision,


            };
            return respuesta;
        }

        #endregion
    }
}

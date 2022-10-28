using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class COcupacionRealL
    {
        #region Variables

        SIRHEntities contexto;
        COcupacionRealD OcupacionRealDescarga;

        #endregion

        #region Constructor

        public COcupacionRealL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //Se insertó en ICPuestoService y CPuestoService el 30/01/2017
        public List<COcupacionRealDTO> DescargarOcupacionReals(int codigo, string nombre)
        {
            List<COcupacionRealDTO> resultado = new List<COcupacionRealDTO>();
            OcupacionRealDescarga = new COcupacionRealD(contexto);
            var item = OcupacionRealDescarga.CargarOcupacionRealesParams(codigo, nombre);

            foreach (var aux in item)
            {
                COcupacionRealDTO ocupacionReal = new COcupacionRealDTO();
                ocupacionReal.IdEntidad = aux.PK_OcupacionReal;
                ocupacionReal.DesOcupacionReal = aux.DesOcupacionReal;
                ocupacionReal.IndEstOcupacion = Convert.ToInt32(aux.IndEstadoOcupacionReal);
                resultado.Add(ocupacionReal);
            }

            return resultado;
        }

        internal static COcupacionRealDTO ConstruirOcupacionReal(OcupacionReal entrada)
        {
            return new COcupacionRealDTO 
            {
                IdEntidad = entrada.PK_OcupacionReal,
                DesOcupacionReal = entrada.DesOcupacionReal
            };
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
   public class CTipoPuestoL
    {
        #region Variables

        private SIRHEntities contexto;
        private string desTipoPuesto;
        
                             
        public string DescripcionTipoPuesto
        {
            get
            {
                return desTipoPuesto;
            }
            set
            {
                desTipoPuesto = value;
            }
        }
                
        #endregion

        #region Constructor

        public CTipoPuestoL()
        {
             contexto = new SIRHEntities();
        }
         
        #endregion

       #region Metodos

        //Se insertó en ICPuestoService y CPuestoService el 27/01/2017                          
        public List<CTipoPuestoDTO> RetornarTiposPuesto()
         {
            List<CTipoPuestoDTO> resultado = new List<CTipoPuestoDTO>();

            List<TipoPuesto> temp = new List<TipoPuesto>();

            CTipoPuestoD intermedio = new CTipoPuestoD(contexto);

            temp = intermedio.RetornarTiposPuesto();

            foreach (var item in temp)
            {
                CTipoPuestoDTO aux = new CTipoPuestoDTO();
                aux.IdEntidad = item.PK_TipoPuesto;
                aux.DescripcionTipoPuesto= item.DesTipoPuesto;
                resultado.Add(aux);
            }

            return resultado;
         }
       //Se insertó en ICPuestoService y CPuestoService el 27/01/2017
        public CTipoPuestoDTO RetornarTipoPuestoEspecifico(string codPuesto)
        {
            CTipoPuestoDTO respuesta = new CTipoPuestoDTO();
            TipoPuesto temp = new TipoPuesto();
            CTipoPuestoD intermedio = new CTipoPuestoD(contexto);
            temp = intermedio.RetornarTipoPuestoEspecifico(codPuesto);

            respuesta.IdEntidad = temp.PK_TipoPuesto;
            respuesta.DescripcionTipoPuesto = temp.DesTipoPuesto;

            return respuesta;
        }

        #endregion
    }
}
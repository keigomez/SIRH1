using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
 public class CRelPresupuestoExtraL
    
    {
        #region Variables

        private SIRHEntities contexto;
                
        #endregion

        #region Constructor

        public CRelPresupuestoExtraL()
        {
             contexto = new SIRHEntities();
        }
         
        #endregion

        #region Metodos

        //TipoContacto de la clase LOGICA
     //Se insertó en CRegistroTESService el 30/01/2017
        public List<CRelPresupuestoExtraDTO> RetornarRelPresupuestoExtras()
        {
            List<CRelPresupuestoExtraDTO> resultado = new List<CRelPresupuestoExtraDTO>();

            List<RelPresupuestoExtra> temp = new List<RelPresupuestoExtra>();

            CRelPresupuestoExtraD intermedio = new CRelPresupuestoExtraD(contexto);

            temp = intermedio.RetornarRelPresupuestoExtra();

            foreach (var item in temp)
            {
                CRelPresupuestoExtraDTO aux = new CRelPresupuestoExtraDTO();
                aux.IdEntidad = item.PK_PresupuestoExtra;
                aux.RegTipExtra = new CTipoExtraDTO { IdEntidad = Convert.ToInt32(item.PK_PresupuestoExtra) }; 
                resultado.Add(aux);
            }

            return resultado;  
        }
        //Se insertó en CRegistroTESService el 30/01/2017
        public CRelPresupuestoExtraDTO RetornarRelPresupuestoExtra(string Cedula)
        {
            CRelPresupuestoExtraDTO respuesta = new CRelPresupuestoExtraDTO();
            RelPresupuestoExtra temp = new RelPresupuestoExtra();
            CRelPresupuestoExtraD intermedio = new CRelPresupuestoExtraD(contexto);
            temp = intermedio.RetornarRelPresupuestoExtra(Cedula);

            respuesta.IdEntidad = temp.PK_PresupuestoExtra;
            respuesta.RegTipExtra = new CTipoExtraDTO { IdEntidad = Convert.ToInt32(temp.PK_PresupuestoExtra) }; 

            return respuesta;
        }

        //Se insertó en CRegistroTESService el 30/01/2017
        public CRelPresupuestoExtraDTO CargarRelPresupuestoExtraPorID(string IdPresupuesto)
        {
            CRelPresupuestoExtraDTO respuesta = new CRelPresupuestoExtraDTO();
            RelPresupuestoExtra temp = new RelPresupuestoExtra();
            CRelPresupuestoExtraD intermedio = new CRelPresupuestoExtraD(contexto);
            temp = intermedio.CargarRelPresupuestoExtraPorID(IdPresupuesto);
            respuesta.IdEntidad = temp.PK_PresupuestoExtra;
            respuesta.RegTipExtra = new CTipoExtraDTO { IdEntidad = Convert.ToInt32(temp.PK_PresupuestoExtra) }; 
            return respuesta;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CCatEstadoCivilL
    {
        #region Variables

        SIRHEntities contexto;
        CHistorialEstadoCivilD estadoDescarga;

        #endregion

        #region constructor

        public CCatEstadoCivilL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CCatEstadoCivilDTO ConvertirCatEstadoCivilADTO(CatEstadoCivil item)
        {
            return new CCatEstadoCivilDTO
            {
                IdEntidad = item.PK_CatEstadoCivil,
                DesEstadoCivil = item.DesEstadoCivil
            };
        }
        //Se insertó el 30/01/2017
        public List<CBaseDTO> DescargarCatEstadoCivil(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            CCatEstadoCivilD datos = new CCatEstadoCivilD(contexto);
            estadoDescarga = new CHistorialEstadoCivilD(contexto);

            var estado = estadoDescarga.CargarCatEstadoCivilCedula(cedula);
            
                foreach (var item in estado)
                {
                    CHistorialEstadoCivilDTO temp = new CHistorialEstadoCivilDTO();
                    
                    temp.IdEntidad = item.PK_HistorialEstadoCivil;
                    temp.FecIncio = Convert.ToDateTime(item.FecInicio);
                    temp.FecFin = Convert.ToDateTime(item.FecFin);
                    temp.CatEstadoCivil = new CCatEstadoCivilDTO
                    {
                        IdEntidad = item.CatEstadoCivil.PK_CatEstadoCivil,
                        DesEstadoCivil = item.CatEstadoCivil.DesEstadoCivil
                    };

                    resultado.Add(temp);
                }
            
            return resultado;
        
        }

        #endregion
    }
}

        
       

         
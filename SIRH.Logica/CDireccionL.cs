using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
   public class CDireccionL
   {
       #region Variables

       SIRHEntities contexto;
       CDireccionD direccionDescarga;
       CProvinciaD provinciaDescarga;
       CCantonD cantonDescarga;
       CDistritoD distritoDescarga;

       #endregion

       #region Constructor

       public CDireccionL()
       {
           contexto = new SIRHEntities();
       }

       #endregion

       #region Metodos
       //Se insertó en FuncionarioService el 31/01/2017
       public List<CBaseDTO> DescargarDireccion(string cedula)
       {
           List<CBaseDTO> resultado = new List<CBaseDTO>();
           
            direccionDescarga = new CDireccionD(contexto);
            distritoDescarga = new CDistritoD(contexto);
            cantonDescarga = new CCantonD(contexto);
            provinciaDescarga = new CProvinciaD(contexto);
            var item = direccionDescarga.DireccionPorCedula(cedula);

            if(item != null)
            {
                CDireccionDTO dirDTO = new CDireccionDTO();
                dirDTO.IdEntidad = item.PK_Direccion;
                dirDTO.DirExacta = item.DirExacta;
                resultado.Add(dirDTO);

                var temp = direccionDescarga.DireccionPorCedula(cedula);

                var tempDistrito = distritoDescarga.CargarDistritos().Where(Q => Q.CodPostalDistrito == temp.Distrito.CodPostalDistrito && Q.PK_Distrito == temp.Distrito.PK_Distrito).FirstOrDefault();
                var tempCanton = cantonDescarga.CargarCantones().Where(Q => Q.CodPostalCanton == tempDistrito.Canton.CodPostalCanton && Q.PK_Canton == tempDistrito.Canton.PK_Canton).FirstOrDefault();
                var tempProvincia = provinciaDescarga.CargarProvincias(provinciaDescarga).Where(Q => Q.PK_Provincia == tempCanton.Provincia.PK_Provincia).FirstOrDefault();

                CProvinciaDTO prov = new CProvinciaDTO { IdEntidad = tempProvincia.PK_Provincia, NomProvincia = tempProvincia.NomProvincia };
                resultado.Add(prov);
                CCantonDTO can = new CCantonDTO { IdEntidad = tempCanton.PK_Canton, NomCanton = tempCanton.NomCanton };
                resultado.Add(can);
                CDistritoDTO dis = new CDistritoDTO { IdEntidad = tempDistrito.PK_Distrito, NomDistrito = tempDistrito.NomDistrito };
                resultado.Add(dis);
            }
            
            
            
            return resultado;
       }

       internal static CBaseDTO ConvertirDireccionADTO(Direccion direccion)
       {
           return new CDireccionDTO
           {
               DirExacta = direccion.DirExacta,
               Distrito = new CDistritoDTO
               {
                   NomDistrito = direccion.Distrito.NomDistrito,
                   Canton = new CCantonDTO
                   {
                       NomCanton = direccion.Distrito.Canton.NomCanton,
                       Provincia = new CProvinciaDTO
                       {
                           NomProvincia = direccion.Distrito.Canton.Provincia.NomProvincia
                       }
                   }
               }
           };
       }

       public CBaseDTO GuardarDireccionFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion)
       {
           CBaseDTO respuesta = new CBaseDTO();
           try
           {
               CDireccionD intermedio = new CDireccionD(contexto);

               Direccion domicilio = new Direccion
               {
                   DirExacta = direccion.DirExacta,
                   Distrito = contexto.Distrito.FirstOrDefault(Q => Q.PK_Distrito == direccion.Distrito.IdEntidad)
               };

               var resultado = intermedio.GuardarDireccionFuncionario(funcionario.Cedula, domicilio);
               if (resultado.Codigo > 0)
               {
                   respuesta = resultado;
                   return respuesta;
               }
               else
               {
                   throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
               }
           }
           catch (Exception error)
           {
               respuesta = new CErrorDTO { MensajeError = error.Message };
               return respuesta;
           }
       }

       #endregion

   }
}
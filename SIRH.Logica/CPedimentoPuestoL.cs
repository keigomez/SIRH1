using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
   public class CPedimentoPuestoL
   {
       #region Variables

       SIRHEntities contexto;       

       #endregion


       #region Constructor

       public CPedimentoPuestoL()
       {
           contexto = new SIRHEntities();
       }   
       #endregion
       
       #region Metodos

       internal static CPedimentoPuestoDTO ConvertirDatosPedimentoPuestoADTO(PedimentoPuesto item)
       {
           return new CPedimentoPuestoDTO
           {
               IdEntidad = item.PK_PedimentoPuesto,
               FechaPedimento = Convert.ToDateTime(item.FecPedimento),
               NumeroPedimento = item.NumPedimento,
               ObservacionesPedimento = item.ObsPedimento                                           
           };
       }

       //se insertó en ICPuestoService y en CPuestoService el 27/01/2017....
       public List<CBaseDTO> BuscarPedimentosPorPuesto(CPuestoDTO puesto)
       {
           List<CBaseDTO> respuesta = new List<CBaseDTO>();
           try
           {
               CPedimentoPuestoD intermedio = new CPedimentoPuestoD(contexto);
               var resultado = intermedio.BuscarPedimentosPorPuesto(puesto.CodPuesto);

               if (resultado.Contenido.GetType() != typeof(CErrorDTO))
               {
                   respuesta.Add(CPuestoL.ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                   respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                   respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault().UbicacionPuesto));

                   foreach (var item in ((Puesto)resultado.Contenido).PedimentoPuesto)
                   {
                       respuesta.Add(ConvertirDatosPedimentoPuestoADTO(item));
                   }

                   return respuesta;
               }
               else
               {
                   throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
               }
           }
           catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;                
            }
        }


        public List<CBaseDTO> BuscarPedimentosCodigo(int pedimento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPedimentoPuestoD intermedio = new CPedimentoPuestoD(contexto);
                var resultado = intermedio.BuscarPedimentoCodigo(pedimento);

                if (resultado.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta.Add(CPuestoL.ConstruirPuesto(((PedimentoPuesto)resultado.Contenido).Puesto, new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((PedimentoPuesto)resultado.Contenido).Puesto.DetallePuesto.FirstOrDefault()));
                    //respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido).RelPuestoUbicacion.FirstOrDefault().UbicacionPuesto));

                    //foreach (var item in ((PedimentoPuesto)resultado.Contenido))
                    //{
                        respuesta.Add(ConvertirDatosPedimentoPuestoADTO((PedimentoPuesto)resultado.Contenido));
                    //}

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        //lo inserté en ICPuestoService y en CPuestoService el 25/01/2017....
        public CBaseDTO GuardarPedimentoPuesto(CPuestoDTO puesto, CPedimentoPuestoDTO pedimento)
       {
           CBaseDTO respuesta = new CBaseDTO();
           try
           {
               CPedimentoPuestoD intermedio = new CPedimentoPuestoD(contexto);
               CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
               PedimentoPuesto pedimentoP = new PedimentoPuesto
               {
                   //LOS CAMPOS DE PEDIMENTO DE PUESTO.
                   //PK_PedimentoPuesto = pedimento.IdEntidad,
                   FecPedimento = Convert.ToDateTime(pedimento.FechaPedimento),
                   NumPedimento = pedimento.NumeroPedimento,
                   ObsPedimento = pedimento.ObservacionesPedimento,
                   FK_Motivo = 1                 
               };

               var datos = intermedio.GuardarPedimentoPuesto(puesto.CodPuesto, pedimentoP);
               if (datos.Contenido.GetType() != typeof(CErrorDTO))
               {
                    respuesta = datos;
                    return respuesta;
               }
               else
               {
                   throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
               }
           }
           catch (Exception error)
           {
               respuesta = new CErrorDTO { MensajeError = error.Message };
               return respuesta;
           }
       }

        public List<List<CBaseDTO>> BuscarPedimentos(CPuestoDTO puesto, CPedimentoPuestoDTO pedimento,
                                                 List<DateTime> fechasEnvio)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CPedimentoPuestoD intermedio = new CPedimentoPuestoD(contexto);

            List<PedimentoPuesto> datosPedimentos = new List<PedimentoPuesto>();

            if (puesto.CodPuesto != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarPedimentos(datosPedimentos, puesto.CodPuesto, "Puesto"));

                if (resultado.Codigo > 0)
                {
                    datosPedimentos = (List<PedimentoPuesto>)resultado.Contenido;
                }
            }

            if (pedimento.NumeroPedimento != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarPedimentos(datosPedimentos, pedimento.NumeroPedimento, "NumeroPedimento"));
                if (resultado.Codigo > 0)
                {
                    datosPedimentos = (List<PedimentoPuesto>)resultado.Contenido;
                }
            }

            if (fechasEnvio.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarPedimentos(datosPedimentos, fechasEnvio, "FechaPedimento"));
                if (resultado.Codigo > 0)
                {
                    datosPedimentos = (List<PedimentoPuesto>)resultado.Contenido;
                }
            }

            if (datosPedimentos.Count > 0)
            {
                foreach (var item in datosPedimentos)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    var datoPedimento = ConvertirDatosPedimentoPuestoADTO(item);

                    CPedimentoPuestoDTO tempNotificacion = datoPedimento;

                    temp.Add(datoPedimento);

                    CPuestoDTO tempPuesto = CPuestoL.ConstruirPuesto(contexto.Puesto.FirstOrDefault(Q => Q.CodPuesto == item.Puesto.CodPuesto), new CPuestoDTO());

                    temp.Add(tempPuesto);

                    CDetallePuestoDTO tempDetalle = CDetallePuestoL.ConstruirDetallePuesto(contexto.Puesto.FirstOrDefault(Q => Q.CodPuesto == item.Puesto.CodPuesto).DetallePuesto
                                                                    .Where(D => (D.IndEstadoDetallePuesto == null || D.IndEstadoDetallePuesto == 1)).OrderBy(D => D.FecRige).FirstOrDefault());

                    temp.Add(tempDetalle);

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }

        #endregion
    }
}
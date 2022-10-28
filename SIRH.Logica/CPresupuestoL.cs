using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPresupuestoL
    {
        #region Variables

        SIRHEntities contexto;
        CPresupuestoD PresupuestoDescarga;

        #endregion

        #region Constructor

        public CPresupuestoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public List<List<CPresupuestoDTO>> DescargarPresupuestos(string codigo)
        {

            List<List<CPresupuestoDTO>> container = new List<List<CPresupuestoDTO>>();

            List<CPresupuestoDTO> resultado = new List<CPresupuestoDTO>();
            List<CPresupuestoDTO> unique = new List<CPresupuestoDTO>();
            unique.Add(new CPresupuestoDTO { CodigoPresupuesto = codigo, text = "Todos" });

            PresupuestoDescarga = new CPresupuestoD(contexto);
            var presupuesto = PresupuestoDescarga.CargarPresupuestosParam(codigo);
            {
                foreach (var item in presupuesto)
                {
                    CPresupuestoDTO list_item = new CPresupuestoDTO();
                    list_item.CodigoPresupuesto = item.IdPresupuesto;
                    resultado.Add(list_item);

                    CPresupuestoDTO Selectlist_item = new CPresupuestoDTO();
                    Selectlist_item.CodigoPresupuesto = item.IdPresupuesto;
                    Selectlist_item.text = item.IdPresupuesto;
                    unique.Add(Selectlist_item);
                }
            }

            container.Add(resultado);
            container.Add(unique);

            return container;

            //List<List<CPresupuestoDTO>> container = new List<List<CPresupuestoDTO>>();

            //List<CPresupuestoDTO> resultado = new List<CPresupuestoDTO>();
            //HashSet<string> unique_IdPresupuesto = new HashSet<string>();

            //PresupuestoDescarga = new CPresupuestoD(contexto);
            //var presupuesto = PresupuestoDescarga.CargarPresupuestosParam(codigo);
            //{
            //    foreach (var item in presupuesto)
            //    {
            //        CPresupuestoDTO temp = new CPresupuestoDTO();
            //        temp.CodigoPresupuesto = item.IdPresupuesto;
            //        resultado.Add(temp);
            //        unique_IdPresupuesto.Add(item.IdPresupuesto);
            //    }
            //}

            //List<CPresupuestoDTO> unique = new List<CPresupuestoDTO>();
            //unique.Add(new CPresupuestoDTO { CodigoPresupuesto = codigo, text = "Todos" });
            //foreach (var item in unique_IdPresupuesto)
            //{
            //    CPresupuestoDTO temp = new CPresupuestoDTO();
            //    temp.CodigoPresupuesto = item;
            //    temp.text = item;
            //    unique.Add(temp);
            //}

            //container.Add(resultado);
            //container.Add(unique);

            //return container;
        }

        public List<CProgramaDTO> DescargarCodigosProgramas() {
            List<CProgramaDTO> resultado = new List<CProgramaDTO>();
            PresupuestoDescarga = new CPresupuestoD(contexto);
            var presupuesto = PresupuestoDescarga.CargarCodigoProgramas();
            {
                foreach (var item in presupuesto)
                {
                    CProgramaDTO temp = new CProgramaDTO();
                    temp.IdEntidad = item.PK_Programa;
                    resultado.Add(temp);
                }
            }
            return resultado;
        }

        public List<CBaseDTO> ListarPresupuestos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CPresupuestoD intermedio = new CPresupuestoD(contexto);

            var presupuestos = intermedio.ListarPresupuestos();

            if (presupuestos.Codigo != -1)
            {
                foreach (var item in (List<Presupuesto>)presupuestos.Contenido)
                {
                    respuesta.Add(new CPresupuestoDTO
                    {
                        IdEntidad = item.PK_Presupuesto,
                        CodigoPresupuesto = item.IdPresupuesto
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)presupuestos.Contenido);
            }

            return respuesta;
        }

        /// <summary>
        /// Retornar objeto tipo CPresupuestoDTO con la información del presupuesto que se obtiene desde la BD
        /// </summary>
        /// <param name="presupuesto"></param>
        /// <returns></returns>
        internal static CPresupuestoDTO ConvertirPresupuestoDatosaDTO(Presupuesto presupuesto)
        {
            return new CPresupuestoDTO
            {
                Programa = new CProgramaDTO
                {
                    DesPrograma = presupuesto.Programa.DesPrograma,
                    IndEstPrograma = Convert.ToInt32(presupuesto.Programa.IndEstadoPrograma)
                },
                Area = presupuesto.CodArea,
                Actividad = presupuesto.CodActividad,
                IdUnidadPresupuestaria = presupuesto.IdUnidadPresupuestaria,
                IdDireccionPresupuestaria = presupuesto.IdDireccionPresupuestaria,
                CodigoPresupuesto = presupuesto.IdPresupuesto
            };
        }

        public List<CBaseDTO> BuscarCodigoPresupuestarioList(string codigopresupuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPresupuestoD intermedio = new CPresupuestoD(contexto);
                var datos = intermedio.BuscarCodigoPresupuestarioList(codigopresupuesto);
                if (datos != null)
                {
                    foreach (var item in datos)
                    {
                        respuesta.Add(new CBaseDTO
                        {
                            Mensaje = item.IdPresupuesto
                        });
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        #endregion
    }
}

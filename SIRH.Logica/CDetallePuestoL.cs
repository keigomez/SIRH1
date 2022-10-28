using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetallePuestoL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDetallePuestoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CDetallePuestoDTO ConstruirDetallePuesto(DetallePuesto entrada)
        {
            try
            {
                CDetallePuestoDTO salida = new CDetallePuestoDTO();

                salida.IdEntidad = entrada.PK_DetallePuesto;
                if (entrada.Clase != null)
                {
                    salida.Clase = CClaseL.ConstruirClase(entrada.Clase);
                }
                else
                {
                    salida.Clase = new CClaseDTO();
                }
                if (entrada.Especialidad != null)
                {
                    salida.Especialidad = CEspecialidadL.ConstruirEspecialidad(entrada.Especialidad);
                }
                else
                {
                    salida.Especialidad = new CEspecialidadDTO();
                }
                if (entrada.EscalaSalarial != null)
                {
                    salida.EscalaSalarial = new CEscalaSalarialDTO
                    {
                        IdEntidad = entrada.EscalaSalarial.PK_Escala,
                        CategoriaEscala = Convert.ToInt32(entrada.EscalaSalarial.IndCategoria),
                        SalarioBase = Convert.ToDecimal(entrada.EscalaSalarial.MtoSalarioBase),
                        MontoAumentoAnual = Convert.ToDecimal(entrada.EscalaSalarial.MtoAumento)
                    };
                }
                else
                {
                    salida.EscalaSalarial = new CEscalaSalarialDTO();
                }
                salida.PorProhibicion = (entrada.PorProhibicion != null) ? Convert.ToDecimal(entrada.PorProhibicion) : 0;
                salida.PorDedicacion = (entrada.PorDedicacion != null) ? Convert.ToDecimal(entrada.PorDedicacion) : 0;
                if (entrada.OcupacionReal != null)
                {
                    salida.OcupacionReal = COcupacionRealL.ConstruirOcupacionReal(entrada.OcupacionReal);
                }
                else
                {
                    salida.OcupacionReal = new COcupacionRealDTO();
                }
                if (entrada.SubEspecialidad != null)
                {
                    salida.SubEspecialidad = CSubEspecialidadL.ConvertirSubEspecialidadADTO(entrada.SubEspecialidad);
                }
                else
                {
                    salida.SubEspecialidad = new CSubEspecialidadDTO();
                }

                return salida;
            }
            catch (Exception error) {
                return null;
            }
        }

        public CBaseDTO GuardarDetalleEscala(int clase, int categoria, int periodo)
        {
            try
            {
                var resultado = new CDetallePuestoD(contexto).ActualizarEscalaSalarial(clase, categoria, periodo);

                if (resultado.Codigo > 0)
                {
                    return ConstruirDetallePuesto(((DetallePuesto)resultado.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    MensajeError = error.Message
                };
            }
        }
      
        #endregion
    }
}

        
    

    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEscalaSalarialL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEscalaSalarialL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Metodos

        public CBaseDTO GuardarPeriodoEscalaSalarial(CEscalaSalarialDTO escala)
        {
            try
            {
                var escalaDatos = ConvertirEscalaSalarialADatos(escala);

                var resultado = new CEscalaSalarialD(contexto).GuardarEscalaSalarial(escalaDatos);

                if (resultado.Codigo > 0)
                {
                    return ConvertirEscalaSalarialADTO(((EscalaSalarial)resultado.Contenido));
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
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }
        }

        internal CEscalaSalarialDTO ConvertirEscalaSalarialADTO(EscalaSalarial escala)
        {
            return new CEscalaSalarialDTO 
            {
                IdEntidad = escala.PK_Escala,
                CategoriaEscala = Convert.ToInt32(escala.IndCategoria),
                SalarioBase = Convert.ToDecimal(escala.MtoSalarioBase),
                MontoAumentoAnual = Convert.ToDecimal(escala.MtoAumento),
                Periodo = CPeriodoEscalaSalarialL.ConvertirPeriodoEscalaSalarialADTO(escala.PeriodoEscalaSalarial)
            };
        }

        internal EscalaSalarial ConvertirEscalaSalarialADatos(CEscalaSalarialDTO escala)
        {
            return new EscalaSalarial
            {
                IndCategoria = escala.CategoriaEscala,
                MtoSalarioBase = escala.SalarioBase,
                MtoAumento = escala.MontoAumentoAnual,
                PeriodoEscalaSalarial = contexto.PeriodoEscalaSalarial.Where(Q => Q.PK_Periodo == escala.Periodo.IdEntidad).FirstOrDefault()
            };
        }

        public List<CBaseDTO> RetornarCategorias()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CEscalaSalarialD intermedio = new CEscalaSalarialD(contexto);

                var tipoEsc = intermedio.RetornarCategorias();

                if (tipoEsc.Codigo > 0)
                {
                    foreach (int tipo in (List<int?>)tipoEsc.Contenido)
                    {
                        resultado.Add(new CBaseDTO
                        {
                            IdEntidad = tipo,
                            Mensaje = tipo.ToString()
                        });
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)tipoEsc.Contenido);
                }

            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }


        public List<CBaseDTO> BuscarEscalaCategoria(int indCategoria)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEscalaSalarialD intermedio = new CEscalaSalarialD(contexto);

                var resultado = intermedio.BuscarEscalaCategoria(indCategoria);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(ConvertirEscalaSalarialADTO((EscalaSalarial)resultado.Contenido));
                    return respuesta;
                }
                else
                {
                    respuesta.Add((CErrorDTO)resultado.Contenido);
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta.Add( new CErrorDTO
                            {
                                Codigo = -1,
                                MensajeError = error.Message
                            });
            }
            return respuesta;
        }


        public List<CBaseDTO> BuscarEscalaCategoriaPeriodo(int indCategoria, int indPeriodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEscalaSalarialD intermedio = new CEscalaSalarialD(contexto);

                var resultado = intermedio.BuscarEscalaCategoriaPeriodo(indCategoria, indPeriodo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(ConvertirEscalaSalarialADTO((EscalaSalarial)resultado.Contenido));
                    return respuesta;
                }
                else
                {
                    respuesta.Add( new CErrorDTO {
                        IdEntidad = -1,
                        Mensaje = resultado.Contenido.ToString(),
                        MensajeError = resultado.Contenido.ToString()
                    });
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return respuesta;
        }

        #endregion
    }
}

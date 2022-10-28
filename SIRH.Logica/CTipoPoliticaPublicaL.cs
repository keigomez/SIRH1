using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CTipoPoliticaPublicaL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoPoliticaPublicaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CTipoPoliticaPublicaDTO ConvertirTipoPoliticaADto(TipoPoliticaPublica item)
        {
            return new CTipoPoliticaPublicaDTO
            {
                IdEntidad = item.PK_TipoPP,
                DescripcionTipoPP = item.DesTipoPP,
                SiglaTipoPP = item.DesSiglas
            };
        }


        /// <summary>
        /// Busca todos los Tipos de Políticas Públicas registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<CBaseDTO> RetornarTipos()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CTipoPoliticaPublicaD intermedio = new CTipoPoliticaPublicaD(contexto);

                var tipoInc = intermedio.RetornarTipoPolitica();

                if (tipoInc.Codigo > 0)
                {
                    foreach (var tipo in (List<TipoPoliticaPublica>)tipoInc.Contenido)
                    {
                        var datoTipoPP = ConvertirTipoPoliticaADto((TipoPoliticaPublica)tipo);
                        resultado.Add(datoTipoPP);
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)tipoInc.Contenido);
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


        /// <summary>
        /// Obtiene un Tipo de Política Pública registrado en la BD
        /// </summary>
        /// <returns>Retorna el Tipo de Incapacidad</returns>
        public List<CBaseDTO> ObtenerTipo(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CTipoPoliticaPublicaD intermedio = new CTipoPoliticaPublicaD(contexto);

                var tipo = intermedio.CargarTipoPorID(codigo);

                if (tipo.Codigo > 0)
                {
                    var dato = ConvertirTipoPoliticaADto((TipoPoliticaPublica)tipo.Contenido);
                    respuesta.Add(dato);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipo.Contenido);
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


        public CBaseDTO AgregarTipo(CTipoPoliticaPublicaDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoPoliticaPublicaD intermedio = new CTipoPoliticaPublicaD(contexto);

                var dato = new TipoPoliticaPublica
                {
                    PK_TipoPP = tipo.IdEntidad,
                    DesTipoPP = tipo.DescripcionTipoPP,
                    DesSiglas = tipo.SiglaTipoPP
                };

                var tipoInc = intermedio.GuardarTipo(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CTipoPoliticaPublicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)tipoInc.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }



        public CBaseDTO EditarTipo(CTipoPoliticaPublicaDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoPoliticaPublicaD intermedio = new CTipoPoliticaPublicaD(contexto);

                var dato = new TipoPoliticaPublica
                {
                    PK_TipoPP = tipo.IdEntidad,
                    DesTipoPP = tipo.DescripcionTipoPP
                };

                var tipoInc = intermedio.EditarTipo(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CTipoPoliticaPublicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)tipoInc.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        #endregion

    }
}
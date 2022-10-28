using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CTipoIncapacidadL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoIncapacidadL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CTipoIncapacidadDTO ConvertirTipoIncapacidadADto(TipoIncapacidad item)
        {
            return new CTipoIncapacidadDTO
            {
                IdEntidad = item.PK_TipoIncapacidad,
                DescripcionTipoIncapacidad = item.DesIncapacidad,
                EntidadMedica = new CEntidadMedicaDTO
                {
                   IdEntidad = item.EntidadMedica.PK_EntidadMedica,
                   DescripcionEntidadMedica = item.EntidadMedica.DesEntidad
                }
                
            };
        }


        /// <summary>
        /// Busca todos los Tipo de Incapacidad registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<CBaseDTO> RetornarTiposIncapacidad()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CTipoIncapacidadD intermedio = new CTipoIncapacidadD(contexto);

                var tipoInc = intermedio.RetornarTipoIncapacidad();

                if (tipoInc.Codigo > 0)
                {
                    foreach (var tipo in (List<TipoIncapacidad>)tipoInc.Contenido)
                    {
                        var datoTipoInc = ConvertirTipoIncapacidadADto((TipoIncapacidad)tipo);
                        resultado.Add(datoTipoInc);
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
        /// Obtiene un Tipo de Incapacidad registrado en la BD
        /// </summary>
        /// <returns>Retorna el Tipo de Incapacidad</returns>
        public List<CBaseDTO> ObtenerTipoIncapacidad(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CTipoIncapacidadD intermedio = new CTipoIncapacidadD(contexto);

                var tipoInc = intermedio.CargarTipoIncapacidadPorID(codigo);

                if (tipoInc.Codigo > 0)
                {
                    var dato = ConvertirTipoIncapacidadADto((TipoIncapacidad)tipoInc.Contenido);
                    respuesta.Add(dato);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipoInc.Contenido);
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


        public CBaseDTO AgregarTipoIncapacidad(CTipoIncapacidadDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoIncapacidadD intermedio = new CTipoIncapacidadD(contexto);

                var dato = new TipoIncapacidad
                {
                    PK_TipoIncapacidad = tipo.IdEntidad,
                    DesIncapacidad = tipo.DescripcionTipoIncapacidad
                };

                var tipoInc = intermedio.GuardarTipoIncapacidad(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
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



        public CBaseDTO EditarTipoIncapacidad(CTipoIncapacidadDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoIncapacidadD intermedio = new CTipoIncapacidadD(contexto);

                var dato = new TipoIncapacidad
                {
                    PK_TipoIncapacidad = tipo.IdEntidad,
                    DesIncapacidad = tipo.DescripcionTipoIncapacidad
                };

                var tipoInc = intermedio.EditarTipoIncapacidad(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CEntidadMedicaL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEntidadMedicaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CEntidadMedicaDTO ConvertirEntidadMedicaADto(EntidadMedica item)
        {
            return new CEntidadMedicaDTO
            {
                IdEntidad = item.PK_EntidadMedica,
                DescripcionEntidadMedica = item.DesEntidad
            };
        }


        /// <summary>
        /// Busca todas las Entidades Médicas registradas en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<CBaseDTO> RetornarEntidadesMedicas()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CEntidadMedicaD intermedio = new CEntidadMedicaD(contexto);

                var tipoEnt = intermedio.RetornarEntidadMedica();

                if (tipoEnt.Codigo > 0)
                {
                    foreach (var tipo in (List<EntidadMedica>)tipoEnt.Contenido)
                    {
                        var datotipoEnt = ConvertirEntidadMedicaADto((EntidadMedica)tipo);
                        resultado.Add(datotipoEnt);
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)tipoEnt.Contenido);
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
        /// Obtiene una Entidad Médica registrada en la BD
        /// </summary>
        /// <returns>Retorna la Entidad Médica</returns>
        public List<CBaseDTO> ObtenerEntidadMedica(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEntidadMedicaD intermedio = new CEntidadMedicaD(contexto);

                var tipoEnt = intermedio.CargarEntidadMedicaPorID(codigo);

                if (tipoEnt.Codigo > 0)
                {
                    var dato = ConvertirEntidadMedicaADto((EntidadMedica)tipoEnt.Contenido);
                    respuesta.Add(dato);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipoEnt.Contenido);
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


        public CBaseDTO AgregarEntidadMedica(CEntidadMedicaDTO entidad)
        {
            CBaseDTO respuesta;

            try
            {
                CEntidadMedicaD intermedio = new CEntidadMedicaD(contexto);

                var entidadMed = new EntidadMedica
                {
                    PK_EntidadMedica = entidad.IdEntidad,
                    DesEntidad = entidad.DescripcionEntidadMedica
                };

                var tipoEnt = intermedio.GuardarEntidadMedica(entidadMed);

                if (tipoEnt.Codigo > 0)
                {
                    respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoEnt.Contenido) };
                }
                else
                {
                    respuesta =((CErrorDTO)tipoEnt.Contenido);
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


        // Editar
        public CBaseDTO EditarEntidadMedica(CEntidadMedicaDTO entidad)
        {
            CBaseDTO respuesta;

            try
            {
                CEntidadMedicaD intermedio = new CEntidadMedicaD(contexto);

                var entidadMed = new EntidadMedica
                {
                    PK_EntidadMedica = entidad.IdEntidad,
                    DesEntidad = entidad.DescripcionEntidadMedica
                };

                var tipoEnt = intermedio.EditarEntidadMedica(entidadMed);

                if (tipoEnt.Codigo > 0)
                {
                    respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoEnt.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)tipoEnt.Contenido);
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
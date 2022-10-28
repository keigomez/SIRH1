using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEstadoTramiteL
        {
            #region Variables

            SIRHEntities contexto;

            #endregion

            #region Constructor

            public CEstadoTramiteL()
            {
                contexto = new SIRHEntities();
            }

            #endregion

            #region Métodos

            internal static CEstadoTramiteDTO ConvertirEstadoTramiteADto(EstadoTramite item)
            {
                return new CEstadoTramiteDTO
                {
                    IdEntidad = item.PK_EstadoTramite,
                    DescripcionEstado = item.DesEstado
                };
            }

            public List<List<CBaseDTO>> RetornarEstadosTramite()
            {
                List<CBaseDTO> temporal = new List<CBaseDTO>();
                List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

                try
                {
                    CEstadoTramiteD intermedio = new CEstadoTramiteD(contexto);

                    var estadoTramite = intermedio.ListarEstado();

                    if (estadoTramite.Codigo > 0)
                    {
                        foreach (var estado in (List<EstadoTramite>)estadoTramite.Contenido)
                        {
                            temporal = new List<CBaseDTO>();

                            var datoEstado = ConvertirEstadoTramiteADto(estado);
                            temporal.Add(datoEstado);

                            resultado.Add(temporal);
                        }
                    }
                    else
                    {
                        temporal.Add((CErrorDTO)estadoTramite.Contenido);
                        resultado.Add(temporal);
                    }

                }
                catch (Exception error)
                {
                    temporal.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    });
                    resultado.Add(temporal);
                }
                return resultado;
            }

            #endregion
        }
    
}

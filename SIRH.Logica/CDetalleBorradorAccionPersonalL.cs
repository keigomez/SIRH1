using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CDetalleBorradorAccionPersonalL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDetalleBorradorAccionPersonalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CDetalleBorradorAccionPersonalDTO ConvertirDatosDetalleADto(DetalleBorradorAccionPersonal item)
        {
            return new CDetalleBorradorAccionPersonalDTO
            {
                IdEntidad = item.PK_Detalle,
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = Convert.ToDateTime(item.FecVence),
                FecRigeIntegra = Convert.ToDateTime(item.FecRigeIntegra),
                FecVenceIntegra = Convert.ToDateTime(item.FecVenceIntegra),
                CodClase = item.CodClase ,
                CodPuesto = item.CodPuesto.ToString(),
                NumHoras = item.NumHoras ,
                Disfrutado = Convert.ToInt32(item.Disfrutado) ,
                Autorizado = Convert.ToInt32(item.Autorizado) ,
                Categoria = item.IndCategoria,
                MtoSalarioBase = item.MtoSalarioBase,
                MtoAumentosAnuales = item.MtoAumentosAnuales,
                MtoRecargo = item.MtoRecargo,
                NumGradoGrupo = item.MtoGradoGrupo,
                PorProhibicion = item.PorProhibicion,
                MtoProhibicion = item.MtoSalarioBase * item.PorProhibicion / 100,
                MtoOtros = item.MtoOtros,
                Borrador = new CBorradorAccionPersonalDTO
                {
                    IdEntidad = item.BorradorAccionPersonal.PK_Borrador
                },
                TipoAccion = new CTipoAccionPersonalDTO
                {
                    IdEntidad = item.TipoAccionPersonal.PK_TipoAccionPersonal,
                    DesTipoAccion = item.TipoAccionPersonal.DesTipoAccion
                },
                Nombramiento = new CNombramientoDTO
                {
                    IdEntidad = item.Nombramiento.PK_Nombramiento
                },
                Programa = item.Programa != null ? new CProgramaDTO
                {
                    IdEntidad = item.Programa.PK_Programa,
                    DesPrograma = item.Programa.DesPrograma
                } : new CProgramaDTO (),
                Seccion = item.Seccion != null ? new CSeccionDTO
                {
                    IdEntidad = item.Seccion.PK_Seccion,
                    NomSeccion = item.Seccion.NomSeccion
                } : new CSeccionDTO()

            };
        }

        //Se registró en ICDetalleBorradorAccionPersonalService y CDetalleBorradorAccionPersonalService
        public CBaseDTO AgregarDetalle(CProgramaDTO programa, CSeccionDTO seccion,
                                       CBorradorAccionPersonalDTO borrador, CDetalleBorradorAccionPersonalDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDetalleBorradorAccionPersonalD intermedio = new CDetalleBorradorAccionPersonalD(contexto);
                CBorradorAccionPersonalD intermedioBorrador = new CBorradorAccionPersonalD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);

                DetalleBorradorAccionPersonal datosRegistro = new DetalleBorradorAccionPersonal
                {
                    FecRige = registro.FecRige,
                    FecVence = registro.FecVence,
                    FecRigeIntegra = registro.FecRigeIntegra,
                    FecVenceIntegra = registro.FecVenceIntegra,
                    CodClase = registro.CodClase,
                    CodPuesto = registro.CodPuesto,
                    NumHoras = registro.NumHoras,
                    Disfrutado = registro.Disfrutado.ToString(),
                    Autorizado = registro.Autorizado.ToString(),
                    IndCategoria = registro.Categoria,
                    MtoSalarioBase = registro.MtoSalarioBase,
                    MtoAumentosAnuales = registro.MtoAumentosAnuales,
                    MtoRecargo = registro.MtoRecargo,
                    MtoGradoGrupo = registro.NumGradoGrupo,
                    PorProhibicion = registro.PorProhibicion,
                    MtoOtros = registro.MtoOtros
                };

                var estadoBorrador = intermedioBorrador.ObtenerBorrador(borrador.IdEntidad);

                if (estadoBorrador.Codigo != -1)
                {
                    datosRegistro.BorradorAccionPersonal = (BorradorAccionPersonal)estadoBorrador.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)estadoBorrador).Contenido;
                    throw new Exception();
                }

                //var estadoNombramiento = intermedio.
                //     if (estadoNombramiento.PK_Programa != -1)
                //{
                //    datosRegistro.Nombramiento = estadoNombramiento;
                //}
                //else
                //{
                //    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoNombramiento.PK_Programa) };
                //    throw new Exception();
                //}


                var estadoPrograma = intermedioPrograma.CargarProgramaPorID(programa.IdEntidad);

                if (estadoPrograma.PK_Programa != -1)
                {
                    datosRegistro.Programa = estadoPrograma;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoPrograma.PK_Programa) };
                    throw new Exception();
                }

                var estadoSeccion = intermedioSeccion.CargarSeccionPorID(seccion.IdEntidad);

                if (estadoSeccion.PK_Seccion != -1)
                {
                    datosRegistro.Seccion = estadoSeccion;
                }
                else
                {
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(estadoSeccion.PK_Seccion) };

                    throw new Exception();
                }

                respuesta = intermedio.GuardarDetalle(datosRegistro);
                        
                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }


        public CBaseDTO EditarBorrador(CDetalleBorradorAccionPersonalDTO registro)
        {
            CBaseDTO respuesta;

            try
            {
                CDetalleBorradorAccionPersonalD intermedio = new CDetalleBorradorAccionPersonalD(contexto);

                var registroInc = new DetalleBorradorAccionPersonal
                {
                    CodClase = registro.CodClase,
                    CodPuesto = registro.CodPuesto,
                    NumHoras = registro.NumHoras,
                    Disfrutado = registro.Disfrutado.ToString(),
                    Autorizado = registro.Autorizado.ToString(),
                    IndCategoria = registro.Categoria,
                    MtoSalarioBase = registro.MtoSalarioBase,
                    MtoAumentosAnuales = registro.MtoAumentosAnuales,
                    MtoRecargo = registro.MtoRecargo,
                    MtoGradoGrupo = registro.NumGradoGrupo,
                    PorProhibicion = registro.PorProhibicion,
                    MtoOtros = registro.MtoOtros
                };

                var dato = intermedio.ActualizarDetalle(registroInc);

                if (dato.Codigo > 0)
                {
                    respuesta = new CDetalleBorradorAccionPersonalDTO { IdEntidad = Convert.ToInt32(dato.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)dato.Contenido);
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


        //Se registró en ICDetalleBorradorAccionPersonalService y CDetalleBorradorAccionPersonalService
        public List<CBaseDTO> ObtenerBorrador(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDetalleBorradorAccionPersonalD intermedio = new CDetalleBorradorAccionPersonalD(contexto);
                CBorradorAccionPersonalD intermedioBorrador = new CBorradorAccionPersonalD(contexto);
                CProgramaD intermedioPrograma = new CProgramaD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);

                var registro = intermedio.ObtenerDetalle(codigo);

                if (registro.Codigo > 0)
                {
                    var datoRegistro = ConvertirDatosDetalleADto((DetalleBorradorAccionPersonal)registro.Contenido);
                    respuesta.Add(datoRegistro);

                    // SolicitudBorrador
                    var datoBorrador = intermedioBorrador.ObtenerBorrador
                       (((DetalleBorradorAccionPersonal)registro.Contenido).BorradorAccionPersonal.PK_Borrador);

                    respuesta.Add(CBorradorAccionPersonalL.ConvertirDatosBorradorADto((BorradorAccionPersonal)datoBorrador.Contenido));

                    //// Programa
                    //var datoPrograma = intermedioPrograma.CargarProgramaPorID
                    //    (((DetalleBorradorAccionPersonal)registro.Contenido).Programa.PK_Programa);

                    //respuesta.Add(CProgramaL.ConvertirEstadoBorradorADto((Programa)datoPrograma.Contenido));


                    //// Seccion
                    //var datoSeccion = intermedioSeccion.CargarSeccionPorID
                    //    (((DetalleBorradorAccionPersonal)registro.Contenido).Seccion.PK_Seccion);

                    //respuesta.Add(CSeccionL) (Seccion) datoSeccion.Contenido;
                }
                else
                {
                    respuesta.Add((CErrorDTO)registro.Contenido);
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
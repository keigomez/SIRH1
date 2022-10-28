using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CContratoArrendamientoD
    {

        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CContratoArrendamientoD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO AgregarContrArrendamiento(Desarraigo desarraigo, ContratoArrendamiento contrato)
        {
            CRespuestaDTO respuesta;
            try
            {
                desarraigo = entidadesBase.Desarraigo
                             .Include("Nombramiento").Include("EstadoDesarraigo")
                             .Include("ContratoArrendamiento")
                             .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo
                                                  && D.EstadoDesarraigo.PK_EstadoDesarraigo != 3);

                var datosContrato = desarraigo.ContratoArrendamiento
                                    .Where(C => C.EmisorContrato == contrato.EmisorContrato
                                             && C.CodContrato == contrato.CodContrato);

                if (desarraigo != null)
                {
                    //El contrato no se encuentra repetida en código y emisor
                    if (datosContrato.Count() == 0)
                    {
                        desarraigo.ContratoArrendamiento.Add(contrato);
                        entidadesBase.SaveChanges();
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = contrato.PK_ContratoArrendamiento
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Este contrato ya esta ingresada.");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el desarraigo indicado.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO AgregarContrArrendamientoViaticoCorrido(ViaticoCorrido viaticoC, ContratoArrendamiento contrato)
        {
            CRespuestaDTO respuesta;
            try
            {
                viaticoC = entidadesBase.ViaticoCorrido
                             .Include("Nombramiento").Include("EstadoViaticoCorrido")
                             .Include("ContratoArrendamiento")
                             .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido
                                                  && D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3);

                var datosContrato = viaticoC.ContratoArrendamiento
                                    .Where(C => C.EmisorContrato == contrato.EmisorContrato
                                             && C.CodContrato == contrato.CodContrato);

                if (viaticoC != null)
                {
                    //El contrato no se encuentra repetida en código y emisor
                    if (datosContrato.Count() == 0)
                    {
                        viaticoC.ContratoArrendamiento.Add(contrato);
                        entidadesBase.SaveChanges();
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = contrato.PK_ContratoArrendamiento
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Este contrato ya esta ingresada.");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el desarraigo indicado.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }
      
        public CRespuestaDTO ObtenerContrArrendamientoDesarraigo(Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.ContratoArrendamiento.Include("Desarraigo").Include("Desarraigo.Nombramiento")
                                                .Where(C => C.Desarraigo.PK_Desarraigo == desarraigo.PK_Desarraigo).ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró contratos de arrendamiento");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo=-1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO ObtenerContrArrendamientoViaticoCorrido(ViaticoCorrido viaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.ContratoArrendamiento.Include("VaiticoCorrido").Include("VaiticoCorrido.Nombramiento")
                                                .Where(C => C.ViaticoCorrido.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido).ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró contratos de arrendamiento");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
       

        #endregion
    }
}
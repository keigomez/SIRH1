using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CFacturaDesarraigoD
    {

        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CFacturaDesarraigoD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO AgregarFactura(Desarraigo desarraigo, FacturaDesarraigo factura)
        {
            CRespuestaDTO respuesta;
            try
            {
                desarraigo = entidadesBase.Desarraigo
                             .Include("Nombramiento").Include("EstadoDesarraigo")
                             .Include("FacturaDesarraigo")
                             .FirstOrDefault(D => D.PK_Desarraigo == desarraigo.PK_Desarraigo 
                                               && D.EstadoDesarraigo.PK_EstadoDesarraigo != 3);

                if (desarraigo != null)
                {
                    var datosFacturas = desarraigo.FacturaDesarraigo
                                        .Where(F => F.EmisorFactura == factura.EmisorFactura 
                                                 && F.CodFactura == factura.CodFactura);

                    //La factura no se encuentra repetida en código y emisor
                    if (datosFacturas.Count() == 0)
                    {
                        desarraigo.FacturaDesarraigo.Add(factura);
                        entidadesBase.SaveChanges();
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = factura.PK_FacturaDesarraigo
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Esta factura ya esta ingresada.");
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
                    Contenido = new CErrorDTO { Codigo=-1,MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO AgregarFacturaViaticoCorrido(ViaticoCorrido viaticoC, FacturaDesarraigo factura)
        {
            CRespuestaDTO respuesta;
            try
            {
                viaticoC = entidadesBase.ViaticoCorrido
                             .Include("Nombramiento").Include("EstadoViaticoCorrido")
                             .Include("FacturaDesarraigo")
                             .FirstOrDefault(D => D.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido
                                               && D.EstadoViaticoCorrido.PK_EstadoViaticoCorrido != 3);

                if (viaticoC != null)
                {
                    var datosFacturas = viaticoC.FacturaDesarraigo
                                        .Where(F => F.EmisorFactura == factura.EmisorFactura
                                                 && F.CodFactura == factura.CodFactura);

                    //La factura no se encuentra repetida en código y emisor
                    if (datosFacturas.Count() == 0)
                    {
                        viaticoC.FacturaDesarraigo.Add(factura);
                        entidadesBase.SaveChanges();
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = factura.PK_FacturaDesarraigo
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Esta factura ya esta ingresada.");
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
        
        //no me parece necesario
        public CRespuestaDTO ObtenerFacturaCodFactura(string codFactura)
        {
            CRespuestaDTO respuesta;
            try
            {
                var factura = entidadesBase.FacturaDesarraigo
                                           .FirstOrDefault(F => F.CodFactura == codFactura);
                if (factura != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = factura
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ninguna factura con codigo: " + codFactura);
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

        public CRespuestaDTO ObtenerFacturasDesarraigo(Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.FacturaDesarraigo.Include("Desarraigo").Include("Desarraigo.Nombramiento")
                                                .Where(F => F.Desarraigo.PK_Desarraigo == desarraigo.PK_Desarraigo).ToList();
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
                    throw new Exception("No se encontró facturas de desarraigo");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1,MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO ObtenerFacturasViaticoCorrido(ViaticoCorrido viaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.FacturaDesarraigo.Include("ViaticoCorrido").Include("ViaticoCorrido.Nombramiento")
                                                .Where(F => F.ViaticoCorrido.PK_ViaticoCorrido == viaticoC.PK_ViaticoCorrido).ToList();
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
                    throw new Exception("No se encontró facturas de desarraigo");
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

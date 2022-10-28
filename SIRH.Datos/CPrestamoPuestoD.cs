using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPrestamoPuestoD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CPrestamoPuestoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        #endregion
        
        #region Metodos

        public CRespuestaDTO BuscarPrestamoPuestoPorPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var prestamo = contexto.Puesto.Include("PrestamoPuesto").Where(P => P.CodPuesto == codPuesto).ToList();
                if (prestamo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = prestamo
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron préstamos del puesto consultado");
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

        public CRespuestaDTO BuscarEstudioPuestoPorPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta;

            try
            {
                var puesto = contexto.Puesto.Include("PedimentoPuesto")
                                                        .Include("EstudioPuesto")
                                                        .Where(P => P.CodPuesto == codPuesto).FirstOrDefault();
                //if (puesto.PedimentoPuesto.Where(P => P.EstadoPedimento.PK_EstadoPedimento == 1).Count() < 1)
                //{
                //    if (puesto.EstudioPuesto.Where(E => E.NumResolucion == null).Count() < 1)
                //    {
                //        respuesta = new CRespuestaDTO
                //        {
                //            Codigo = 1,
                //            Contenido = puesto
                //        };
                //        return respuesta;
                //    }
                //    else
                //    {
                        throw new Exception("Este puesto actualmente está bajo estudio");
                    //}
                //}
                //else
                //{
                //    throw new Exception("Este puesto actualmente está bajo pedimento");
                //}
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

        public CRespuestaDTO GuardarPrestamoPuesto(string codPuesto, PrestamoPuesto prestamoPuesto)
        {
            CRespuestaDTO respuesta;

            try
            {
                //en el contexto, en puesto me incluya a prestamoPuesto, donde el codPuesto sea igual al que se solicitó
                var puesto = contexto.Puesto.Include("PrestamoPuesto").Where(P => P.CodPuesto == codPuesto).FirstOrDefault();

                //si el puesto no es null
                if (puesto != null)
                {
                    //si en puesto, en prestamopuesto, el addendum Prestamo es null o menor a uno
                    if (puesto.PrestamoPuesto.Where(P => P.AddendumPrestamoPuesto == null).Count() < 1)
                    {
                        //de puesto pasa a la tabla PrestamoPuesto y agregue el prestamo puesto
                        puesto.PrestamoPuesto.Add(prestamoPuesto);
                        contexto.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = puesto
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Este puesto esta actualmente en préstamo");
                    }
                }
                else
                {
                    throw new Exception("No se encontró en préstamo el siguiente puesto: " + puesto);
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
        
        #endregion
    }
}

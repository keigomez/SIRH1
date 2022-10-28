using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstudioPuestoD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstudioPuestoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO BuscarEstudiosPorPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var estudioP = contexto.Puesto.Include("EstudioPuesto").Where(P => P.CodPuesto == codPuesto).ToList();
                if (estudioP != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = estudioP
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron estudios de puesto para el código de puesto consultado");
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
                //var puesto = contexto.Puesto.Include("PedimentoPuesto")
                //                                        .Include("EstudioPuesto")
                //                                        .Where(P => P.CodPuesto == codPuesto).FirstOrDefault();
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
                //        throw new Exception("Este puesto actualmente está bajo estudio");
                //    }
                //}
                //else
                //{
                    throw new Exception("Este puesto actualmente está bajo pedimento");
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

        public CRespuestaDTO GuardarEstudioPuesto(string codPuesto, EstudioPuesto estudioPuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                var puesto = contexto.Puesto.Include("EstudioPuesto").Where(P => P.CodPuesto == codPuesto).FirstOrDefault();
                //si el puesto es diferente de nulo, o sea que no se encuentra registrado
                if (puesto != null)
                {
                    //en el puesto, si el numero de resolución del estudio de puesto es null o menor que uno.
                    if (puesto.EstudioPuesto.Where(E => E.NumResolucion == null).Count() < 1)
                    {
                        //en la tabla puesto pasa a la tabla estudio de puesto y agregue el estudio de puesto
                        puesto.EstudioPuesto.Add(estudioPuesto);
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
                        throw new Exception("Este puesto actualmente está bajo estudio");
                    }
                }
                else 
                {
                    throw new Exception("No se encontró ningún estudio asociado al puesto: " + puesto);
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

        public CRespuestaDTO ModificiarEstudioPuesto(EstudioPuesto estudioPuesto)
        {
            CRespuestaDTO respuesta;

            try
            {
                //estudio de puesto anterior será igual a la llave de estudioPuesto, el primer id del puesto que se necesita cambiar o modificar 
                var EstPuestoAnt = contexto.EstudioPuesto.Where(E => E.PK_EstudioPuesto == estudioPuesto.PK_EstudioPuesto).FirstOrDefault();
                //si estado de puesto anterior no es nulo
                if (EstPuestoAnt != null)
                {
                    //se van a cambiar los siguientes datos de estudio de puesto, como el num de resolucion, las observaciones en estudio de puesto.
                    //estudio de puesto anterior, será reemplazado por el nuevo.
                    EstPuestoAnt.ObsEstudioPuesto = estudioPuesto.ObsEstudioPuesto;
                    EstPuestoAnt.NumResolucion = estudioPuesto.NumResolucion;
                    estudioPuesto = EstPuestoAnt;
                    contexto.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = EstPuestoAnt.PK_EstudioPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Este puesto actualmente está bajo estudio");                    
                }

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        #endregion
    }
}

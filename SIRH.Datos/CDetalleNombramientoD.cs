using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleNombramientoD
    {
        #region Variables

        private SIRHEntities contexto = new SIRHEntities();

        #endregion       

        #region Constructor

        public CDetalleNombramientoD(SIRHEntities entidadGlobal)
        {
            contexto = entidadGlobal;
        }       

        #endregion

        #region Metodos

        //public int GuardarDetalleNombramiento(DetalleNombramiento detalleNombramiento)
        //{
        //    contexto.DetalleNombramiento.Add(detalleNombramiento);
        //    return detalleNombramiento.PK_DetalleNombramiento;
        //}

        //public CRespuestaDTO GuardarDetalleNombramiento(string cedula, DetalleNombramiento detalleNombramiento)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        contexto.DetalleNombramiento.Add(detalleNombramiento);
        //        contexto.SaveChanges();
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = detalleNombramiento
        //        };
        //        return respuesta;
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };
        //        return respuesta;
        //    }
        //}

        public CRespuestaDTO ObtenerDetalleNombramientoFuncionario (Funcionario funcionario){

            CRespuestaDTO respuesta;
            try
            {
                var funcionarioEntidad = contexto.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionarioEntidad != null)
                {
                    var nombramientoEntidad = contexto.Nombramiento.Where(N => N.Funcionario.PK_Funcionario == funcionarioEntidad.PK_Funcionario).FirstOrDefault();
                    if (nombramientoEntidad != null)
                    {
                        //var detalleNombramientoEntidad = contexto.DetalleNombramiento.Where(DN => DN.Nombramiento.PK_Nombramiento == nombramientoEntidad.PK_Nombramiento).FirstOrDefault();
                        //if (detalleNombramientoEntidad != null)
                        //{
                        //    respuesta = new CRespuestaDTO
                        //    {
                        //        Codigo = 1,
                        //        Contenido = detalleNombramientoEntidad
                        //    };
                            return new CRespuestaDTO();
                        //}
                        //else
                        //{
                        //    throw new Exception("No se encontró el detalle de nombramiento para el funcionario indicado");
                        //}
                    }
                    else
                    {
                        throw new Exception("El funcionario indicado no tiene un nombramiento asociado");
                    }
                }
                else {
                    throw new Exception("El funcionario indicado no está registrado en la base de datos de recursos humanos");
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

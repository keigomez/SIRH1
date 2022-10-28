using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMotivoMovimientoD
   {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CMotivoMovimientoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

         //<summary>
         //Guarda los Motivos de Movimientos en la BD
         //</summary>
         //<returns>Retorna Los Motivos de movimientos</returns>
        public int GuardarMotivoMovimiento(MotivoMovimiento MotivoMovimiento)
        {
            contexto.MotivoMovimiento.Add(MotivoMovimiento);
            return MotivoMovimiento.PK_MotivoMovimiento;
        }

        public CRespuestaDTO GuardarMotivoMovimiento(string codPuesto, MotivoMovimiento motivo)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.MotivoMovimiento.Add(motivo);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = motivo
                };
                return respuesta;
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
        //<summary>
        //Obtiene la carga de los Motivos de Movimientos de la BD
        //</summary>
        //<returns>Retorna la carga de los Motivos de los Movimientos</returns>    
        public MotivoMovimiento CargarMotivoMovimientoPorPuesto(string NumeroPuesto)
        {
            MotivoMovimiento resultado = new MotivoMovimiento();

            resultado = contexto.MotivoMovimiento.Where(R => R.MovimientoPuesto.Where(P => P.Puesto.CodPuesto == NumeroPuesto).Count() > 0).FirstOrDefault();

            return resultado;
        }

        public MotivoMovimiento CargarMotivoMovimientoPorCedula(string Cedula)
        {
            MotivoMovimiento resultado = new MotivoMovimiento();

            resultado = contexto.MotivoMovimiento.Where(Q =>
                                                   Q.MovimientoPuesto.Where(R =>
                                                                             R.Puesto.RelPuestoUbicacion.Where(T =>
                                                                                                                T.UbicacionPuesto.Distrito.Direccion.Where(A =>
                                                                                                                                                           A.Funcionario.IdCedulaFuncionario == Cedula).Count() > 0).Count() > 0).Count() > 0).FirstOrDefault();

            return resultado;
        }

        public MotivoMovimiento CargarMotivoMovimientoCodigo(int codigoMotivo)
        {
            MotivoMovimiento resultado = new MotivoMovimiento();

            resultado = contexto.MotivoMovimiento.Where(Q => Q.PK_MotivoMovimiento == codigoMotivo).FirstOrDefault();
        
            return resultado;
        }

        public List<MotivoMovimiento> DescargarMotivoMovimientoCompleto()
        {
            List<MotivoMovimiento> resultado = new List<MotivoMovimiento>();

            resultado = contexto.MotivoMovimiento.ToList();

            return resultado;
        }

        public CRespuestaDTO ListarMotivosMovimiento()
        {
            try
            {
                var motivosMovimiento = contexto.MotivoMovimiento.ToList();
                if (motivosMovimiento != null)
                {
                    return new CRespuestaDTO 
                    {
                        Codigo = 1,
                        Contenido = motivosMovimiento
                    };
                }
                else
                {
                    throw new Exception("No se encontraron motivos de movimiento");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        #endregion
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;
using SIRH.Datos.Helpers;

namespace SIRH.Datos
{
    public class CDesgloseSalarialD
   {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CDesgloseSalarialD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        /// Regresa la lista del Desglose Salarial        
        public List<DesgloseSalarial> RetornarDesgloseSalarial()
        {
            return contexto.DesgloseSalarial.ToList();
        }

        public CRespuestaDTO GuardarDesgloseSalarial(string cedula, DesgloseSalarial desglose)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.DesgloseSalarial.Add(desglose);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = desglose
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
        //Lista por Cédula
        public DesgloseSalarial RetornarDesgloseSalarialEspecifico(string Cedula)
        {
            return contexto.DesgloseSalarial.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario == Cedula).FirstOrDefault();
        }

        public int CrearDesgloceSalarial(string cedula, DateTime periodo)
        {
            var funcionario = contexto.Funcionario.Include("Nombramiento")
                                .Include("Nombramiento.EstadoNombramiento")
                                .Include("Nombramiento.DesgloseSalarial")
                                .Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();

            int estado = Convert.ToInt32(EstadosFuncionario.Activo);

            var nombramiento = funcionario.Nombramiento.Where(Q => Q.EstadoNombramiento.PK_EstadoNombramiento == estado).FirstOrDefault();

            return 0;
        }

        #endregion
    }
}

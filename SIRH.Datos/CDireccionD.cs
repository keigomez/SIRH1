using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using SIRH.DTO;
using System.Data.Entity.Infrastructure;

namespace SIRH.Datos
{
    public class CDireccionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDireccionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Guarda la Dirección en la BD
        /// </summary>
        /// <param name="direccionLocal"></param>
        /// <returns>Retorna la Dirección</returns>
        public int GuardarDireccion(Direccion direccionLocal)
        {
            entidadBase.Direccion.Add(direccionLocal);
            entidadBase.SaveChanges();
            return direccionLocal.PK_Direccion;
        }

        /// <summary>
        /// Obtiene el Distrito al que corresponde la dirección
        /// </summary>
        /// <returns>Retorna el Distrito</returns>
        private DbQuery<Direccion> RetornarDistritos()
        {
            var item = entidadBase.Direccion.Include("Distrito");
            return entidadBase.Direccion.Include("Distrito");
        }

        /// <summary>
        /// POR CÉDULA
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns>El Funcionario y su Dirección</returns>
        public Direccion DireccionPorCedula(string cedula)
        {
            var item = RetornarDistritos().Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula).FirstOrDefault();
            return RetornarDistritos().Include("Funcionario").Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula).FirstOrDefault();
        }

        public CRespuestaDTO GuardarDireccionFuncionario(string cedula, Direccion domicilio)
        {
            try
            {
                //AGREGAR VALIDACIÓN PARA QUE SOLO INCLUYA FUNCIONARIOS QUE ESTÉN ACTIVOS
                var resultado = entidadBase.Funcionario.Include("EstadoFuncionario")
                                                       .Include("Direccion")
                                                       .FirstOrDefault(Q => Q.IdCedulaFuncionario == cedula);

                if (resultado != null)
                {
                    if (resultado.Direccion.Count > 0)
                    {
                        throw new Exception("El funcionario ya tiene una dirección registrada, debe editar dicha dirección antes de guardar una nueva.");
                    }
                    else
                    {
                        if (resultado.EstadoFuncionario.PK_EstadoFuncionario == 20)
                        {
                            resultado.EstadoFuncionario = entidadBase.EstadoFuncionario.FirstOrDefault(Q => Q.PK_EstadoFuncionario == 21);
                        }
                        resultado.Direccion.Add(domicilio);
                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = domicilio.PK_Direccion
                        };
                    }
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario activo registrado con la cédula suministrada.");
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

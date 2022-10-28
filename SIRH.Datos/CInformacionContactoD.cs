using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.DTO;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;

namespace SIRH.Datos
{
    public class CInformacionContactoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CInformacionContactoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Guarda la Información del Contacto en la BD
        /// </summary>
        /// <param name="contactoLocal"></param>
        /// <returns>Retorna la información del Contacto</returns>
        public int GuardarInformacionContacto(InformacionContacto contactoLocal)
        {
            entidadBase.InformacionContacto.Add(contactoLocal);
            return contactoLocal.PK_InformacionContacto;
        }


        //22/11/2016, "GUARDAR INFORMACION DEL CONTACTO DEL FUNCIONARIO"
        public CRespuestaDTO GuardarInformacionContacto(string cedula, InformacionContacto informacionContacto)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.InformacionContacto.Add(informacionContacto);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = informacionContacto
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

        public CRespuestaDTO FuncionarioInformacionContacto()
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = new List<InformacionContacto> ();
                //var resultado = entidadBase.Funcionario.Include("InformacionContacto")
                //                                   .Include("TipoContacto")                                                                                                   
                //                                   .Where(Q => Q.InformacionContacto.Where(I=> I.TipoContacto
                //                                       N.Puesto.IndPuestoConfianza == true)
                //                                       .Count() > 0).OrderBy(Q => Q.NomPrimerApellido).ToList();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron funcionarios que estén actualmente en puestos de confianza o suscritos a pólizas de caución.");
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

        private DbQuery<InformacionContacto> DescargarConTipoContacto()
        {
            return entidadBase.InformacionContacto.Include("TipoContacto");
        }
                
        // Carga la Información del Contacto correspondiente a la cédula     
        //param name="cedula"></param>
        //Retorna la información del Contacto</returns>
        public List<InformacionContacto> DescargarInformacionContactoCedula(string cedula)
        {
            return DescargarConTipoContacto().Include("Funcionario").Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula).ToList();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CExpedienteD
    {

        #region Variables
        private SIRHEntities entidadBase = new SIRHEntities();
        #endregion

        #region Coonstructor
        public CExpedienteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Métodos
        public CRespuestaDTO BuscarExpedienteUsuarioPorCedula(string cedula) {

            CRespuestaDTO respuesta;

            try {

                var fun = entidadBase.Funcionario.Where(f => f.IdCedulaFuncionario == cedula).FirstOrDefault();

                var temp = entidadBase.ExpedienteFuncionario.Include("Funcionario").Where(f => f.FK_Funcionario == fun.PK_Funcionario).FirstOrDefault();

                if (temp != null) {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = temp
                    };
                    return respuesta;

                }
                else {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "El Usuario buscado no posee un expediente registrado en el MOPT." }
                    };
                    return respuesta;

                }
            }
            catch (Exception error ){
                    respuesta = new CRespuestaDTO
                    {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                    };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerExpedientePorNumeroExpediente(int numero_expediente) {

            CRespuestaDTO respuesta;
            try
            {
                var expediente = entidadBase.ExpedienteFuncionario.Include("Funcionario").Where(e => e.numExpediente == numero_expediente).FirstOrDefault();
                if (expediente != null)
                {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = expediente
                    };
                }
                else {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "El número de expediente no corresponde a ningún expediente existente." }
                    };
                }
            }
            catch (Exception error) {

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }



        public CRespuestaDTO ObtenerExpedientePorCedulaFuncionario(string cedula) {

            CRespuestaDTO respuesta;
            try
            {
                var func = entidadBase.Funcionario.Where(f => f.IdCedulaFuncionario == cedula).FirstOrDefault();

                if (func != null)
                {

                    var expediente = entidadBase.ExpedienteFuncionario.Where(e => e.FK_Funcionario == func.PK_Funcionario).FirstOrDefault();

                    if (expediente != null)
                    {

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = expediente
                        };
                    }
                    else
                    {

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = new CErrorDTO { MensajeError = "El Funcionario no posee ningún expediente por el momento." }
                        };
                    }
                }
                else {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "La cédula solicitada no pertenece a ningún funcionario del MOPT." }
                    };
                }
            }
            catch (Exception error)
            {

                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ActualizarFechaTrasladoArchivoCentralExpediente(ExpedienteFuncionario expediente) {

            CRespuestaDTO respuesta;

            try
            {
                ExpedienteFuncionario Expediente = entidadBase.ExpedienteFuncionario.Where(e => e.PK_IdExpedienteFuncionario == expediente.PK_IdExpedienteFuncionario).FirstOrDefault();

                Expediente.FecTrasladoArchivoCentral = expediente.FecTrasladoArchivoCentral;
                Expediente.Estado = 2;
                Expediente.numExpedienteEnArchivo = expediente.numExpedienteEnArchivo;
                Expediente.numCaja = expediente.numCaja;

                int resultado = entidadBase.SaveChanges();

                if (resultado > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Expediente.numExpediente
                    };
                }
                else { // si no sufrió cambios.

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1
                    };
                }
            }
            catch (Exception error) {

                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message}
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ActualizarEstadoExpedienteEnPrestamo(ExpedienteFuncionario expediente) {
            CRespuestaDTO respuesta;
            try
            {
                ExpedienteFuncionario Expediente = entidadBase.ExpedienteFuncionario.Where(e => e.numExpediente == expediente.numExpediente).FirstOrDefault();
                Expediente.Estado = 0;
                int resultado = entidadBase.SaveChanges();

                if (resultado > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Expediente.PK_IdExpedienteFuncionario
                    };
                }
                else
                { // si no sufrió cambios.

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1
                    };
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }


        public bool ActualizarExpedienteFuncionario(Funcionario func) {

            bool respuesta = false;
            ExpedienteFuncionario expedienteActualizar = entidadBase.ExpedienteFuncionario.
                Where(e => e.Funcionario.IdCedulaFuncionario == func.IdCedulaFuncionario).FirstOrDefault();

            DetalleContratacion detalle = entidadBase.DetalleContratacion.Where(d => d.FK_Funcionario == func.PK_Funcionario).FirstOrDefault();

            expedienteActualizar.FecCreacion = detalle != null ? Convert.ToDateTime(detalle.FecIngreso) : DateTime.Today;
            expedienteActualizar.FecTrasladoArchivoCentral = null;
            expedienteActualizar.Estado = 1;

            int resultado = entidadBase.SaveChanges();
            if (resultado > 0) {
                respuesta = true;
            }
            return respuesta;
        }


        public bool ActualizarExpedienteRecienCreado(Funcionario func) {

            bool respuesta = false;
            ExpedienteFuncionario expedienteActualizar = entidadBase.ExpedienteFuncionario.
            Where(e => e.Funcionario.IdCedulaFuncionario == func.IdCedulaFuncionario).FirstOrDefault();

            expedienteActualizar.FecTrasladoArchivoCentral = null;
            expedienteActualizar.Estado = 1;
            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public CRespuestaDTO AgregarExpedienteFuncionario(ExpedienteFuncionario expediente)
        {

            CRespuestaDTO respuesta;
            try
            {
                entidadBase.ExpedienteFuncionario.Add(expediente);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = expediente.PK_IdExpedienteFuncionario
                };

                return respuesta;
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

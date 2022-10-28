using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CExpedienteL
    {
        #region Variables
        SIRHEntities contexto;
        #endregion

        #region Constructor
        public CExpedienteL() {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Métodos

        public List<CBaseDTO> ObtenerExpedientePorCedula(string cedula) {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CExpedienteD intermedio = new CExpedienteD(contexto);

            try
            {

                ExpedienteFuncionario expediente = (ExpedienteFuncionario)intermedio.BuscarExpedienteUsuarioPorCedula(cedula).Contenido;
                respuesta.Add(ConvertirExpedienteADTO(expediente));
                //new CUsuarioDTO { IdEntidad = 1  }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }
            return respuesta;
        }




        public List<CBaseDTO> ObtObtenerExpedientePorNumeroExpediente(int numero_expediente) {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);
                var expediente = mediadorExpediente.ObtenerExpedientePorNumeroExpediente(numero_expediente);

                if (expediente.Codigo > 0)
                {

                    respuesta.Add(ConvertirExpedienteADTO((ExpedienteFuncionario)expediente.Contenido));
                }
                else {
                    respuesta.Add((CErrorDTO)expediente.Contenido);
                }
            }
            catch (Exception error) {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerExpedientePorCedulaFuncionario(string cedula) {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);

                var expediente = mediadorExpediente.ObtenerExpedientePorCedulaFuncionario(cedula);

                if (expediente.Codigo > 0)
                {
                    respuesta.Add(ConvertirExpedienteADTO((ExpedienteFuncionario)expediente.Contenido));
                }
                else
                {
                    respuesta.Add((CErrorDTO)expediente.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return respuesta;

        }


        public CBaseDTO ActualizarFechaTrasladoArchivoCentralExpediente(CExpedienteFuncionarioDTO expediente) {

            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                ExpedienteFuncionario exp = ConvertirDTOAExpediente(expediente);
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);

                var boletaActualizada = mediadorExpediente.ActualizarFechaTrasladoArchivoCentralExpediente(exp);
                if (boletaActualizada.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = boletaActualizada.Contenido //contiene el PK del expediente.
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = (CErrorDTO)boletaActualizada.Contenido
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

        public CBaseDTO ActualizarEstadoExpedienteEnPrestamo(CExpedienteFuncionarioDTO expediente) {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                ExpedienteFuncionario exp = ConvertirDTOAExpediente(expediente);
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);

                var expedienteActualizado = mediadorExpediente.ActualizarEstadoExpedienteEnPrestamo(exp);
                if (expedienteActualizado.Codigo > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = expedienteActualizado.Contenido //contiene el PK del expediente.
                    };
                }
                else {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = (CErrorDTO)expedienteActualizado.Contenido
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
            bool respuesta;
            CExpedienteD intermedio = new CExpedienteD(contexto);
            respuesta = intermedio.ActualizarExpedienteFuncionario(func);
            if (respuesta)
            {
                return respuesta;
            }
            else { return false; }
        }

        public bool ActualizarExpedienteRecienCreado(Funcionario func) {
            bool respuesta;
            CExpedienteD intermedio = new CExpedienteD(contexto);
            respuesta = intermedio.ActualizarExpedienteRecienCreado(func);
            if (respuesta)
            {
                return respuesta;
            }
            else { return false; }
        }

        internal static CExpedienteFuncionarioDTO ConvertirExpedienteADTO(ExpedienteFuncionario exp) {
            if (exp != null)
            {
                return new CExpedienteFuncionarioDTO
                {
                    IdEntidad = exp.PK_IdExpedienteFuncionario,
                    Funcionario = ConvertirFuncionarioADTO(exp.Funcionario),
                    FechaCreacion = exp.FecCreacion != null ? Convert.ToDateTime(exp.FecCreacion) : DateTime.Now,
                    Estado = castearEstadoExpediente(Convert.ToInt32(exp.Estado)),
                    FechaTrasladoArchivoCentral = Convert.ToDateTime(exp.FecTrasladoArchivoCentral),
                    NumeroExpediente = exp.numExpediente,
                    NumeroExpedienteEnArchivo = exp.numExpedienteEnArchivo,
                    NumeroCaja = exp.numCaja
                };
            }
            else
            {
                return null;
            }
        }

        internal static CFuncionarioDTO ConvertirFuncionarioADTO(Funcionario item)
        {

            return new CFuncionarioDTO
            {
                IdEntidad = item.PK_Funcionario,
                Cedula = item.IdCedulaFuncionario,
                Nombre = item.NomFuncionario,
                PrimerApellido = item.NomPrimerApellido,
                SegundoApellido = item.NomSegundoApellido,
                Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino,
                FechaNacimiento = Convert.ToDateTime(item.FecNacimiento),
                EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                }
            };
        }

        internal static ExpedienteFuncionario ConvertirDTOAExpediente(CExpedienteFuncionarioDTO exp) {
            return new ExpedienteFuncionario
            {
                PK_IdExpedienteFuncionario = exp.IdEntidad,
                Estado = Convert.ToInt32(exp.Estado),
                FecCreacion = Convert.ToDateTime(exp.FechaCreacion),
                FecTrasladoArchivoCentral = Convert.ToDateTime(exp.FechaTrasladoArchivoCentral),
                numExpediente = exp.NumeroExpediente,
                numExpedienteEnArchivo = exp.NumeroExpedienteEnArchivo,
                numCaja = exp.NumeroCaja
            };
        }

        internal static EstadoEnum castearEstadoExpediente(int estado)
        {
            switch (estado)
            {
                case 2:
                    return EstadoEnum.TrasladadoArchivoCentral;
                case 1:
                    return EstadoEnum.Prestado;
                case 0:
                    return EstadoEnum.NoPrestado;

                default:
                    return EstadoEnum.NoDefinido;
            }
        }
        #endregion
    }
}

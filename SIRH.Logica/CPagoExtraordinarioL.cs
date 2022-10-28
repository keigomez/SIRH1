using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPagoExtraordinarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPagoExtraordinarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CPagoExtraordinarioDTO ConvertirDatosPagoExtraordinarioADto(PagoExtraordinario item)
        {
            return new CPagoExtraordinarioDTO
            {
                FechaTramite = Convert.ToDateTime(item.FecTramite),
                IdEntidad = item.PK_PagoExtraordinario
            };
        }

        /// <summary>
        /// Almacena en la base de datos un pago extraordinario
        /// </summary>
        /// <returns>Retorna el registro almacenado</returns>
        public CBaseDTO AgregarPagoExtraordinario(CFuncionarioDTO funcionario, CPagoExtraordinarioDTO pagoExtraordinario)
        {

            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CPagoExtraordinarioD intermedio = new CPagoExtraordinarioD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                PagoExtraordinario datosPagoExt = new PagoExtraordinario
                {
                    FecTramite = pagoExtraordinario.FechaTramite
                };

                respuesta = intermedio.AgregarPagoExtraordinario(datosPagoExt, datosFuncionario);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Busca un registro específico de un pago extraordinario 
        /// </summary>
        /// <returns>Retorna el pago extraordinario</returns>
        public List<CBaseDTO> ObtenerPagoExtraordinario(int codigo)
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CPagoExtraordinarioD intermedio = new CPagoExtraordinarioD(contexto);

                var pagoExtraordinario = intermedio.BuscarPagoExtraordinario(codigo);//Buscar pago extraordinario


                if (pagoExtraordinario.Codigo > 0)
                {
                    var datoPagoExtraordinario = ConvertirDatosPagoExtraordinarioADto((PagoExtraordinario)pagoExtraordinario.Contenido);
                    respuesta.Add(datoPagoExtraordinario);
                    var funcionario = ((PagoExtraordinario)pagoExtraordinario.Contenido).Funcionario;

                    respuesta.Add(new CFuncionarioDTO
                    {
                        Cedula = funcionario.IdCedulaFuncionario,
                        Nombre = funcionario.NomFuncionario,
                        PrimerApellido = funcionario.NomPrimerApellido,
                        SegundoApellido = funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    });


                }
                else
                {
                    respuesta.Add((CErrorDTO)pagoExtraordinario.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        /// <summary>
        /// Obtiene todos los pagos extraordinarios almacenados en la BD
        /// </summary>
        /// <returns>Retorna todos los pagos extraordinarios registrados</returns>
        public List<List<CBaseDTO>> ObtenerListaPagoExtraordinario()
        {

            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CPagoExtraordinarioD intermedio = new CPagoExtraordinarioD(contexto);

                var pagoExtraordinario = intermedio.ListarPagoExtraordinario();//Buscar pago extraordinario

                if (pagoExtraordinario.Codigo > 0)
                {
                    foreach (var pago in (List<PagoExtraordinario>)pagoExtraordinario.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoPagoExtraordinario = ConvertirDatosPagoExtraordinarioADto(pago);
                        temporal.Add(datoPagoExtraordinario);

                        var funcionario = ((PagoExtraordinario)pagoExtraordinario.Contenido).Funcionario;

                        temporal.Add(new CFuncionarioDTO
                        {
                            Cedula = funcionario.IdCedulaFuncionario,
                            Nombre = funcionario.NomFuncionario,
                            PrimerApellido = funcionario.NomPrimerApellido,
                            SegundoApellido = funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        });
                        respuesta.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)pagoExtraordinario.Contenido);
                    respuesta.Add(temporal);
                }
            }
            catch (Exception error)
            {
                temporal.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(temporal);
            }

            return respuesta;
        }

        /// <summary>
        /// Elimina un pago extraordinario en la BD
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO EliminarPagoExtraordinario(CPagoExtraordinarioDTO pagoExtraordinario)
        {

            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CPagoExtraordinarioD intermedio = new CPagoExtraordinarioD(contexto);

                PagoExtraordinario datosPagoExt = new PagoExtraordinario
                {
                    PK_PagoExtraordinario = pagoExtraordinario.IdEntidad
                };

                respuesta = intermedio.EliminarPagoExtraordinario(datosPagoExt);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CAguinadldoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CAguinadldoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CAguinaldoDTO ConvertirDatosTramiteADto(Aguinaldo item)
        {

            return new CAguinaldoDTO
            {
                IdEntidad = item.PK_Aguinaldo,
                MtoAguinaldo = Convert.ToInt32(item.MtoAguinaldo),
                Periodo = Convert.ToDateTime(item.Periodo)

            };
        }

        /// <summary>
        /// Agrega a la BD un nuevo registro de trámite de aguinaldo
        /// </summary>
        /// <returns>Retorna el nuevo monto</returns>
        public CBaseDTO AgregarAguinaldo(CAguinaldoDTO aguinaldo, CDesgloseSalarialDTO desgloseSalarial,
                                       CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAguinaldoD intermedio = new CAguinaldoD(contexto);

                CDesgloseSalarialD intermedioDesgloseSalarial = new CDesgloseSalarialD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                Aguinaldo datosTramite = new Aguinaldo
                {
                    PK_Aguinaldo = aguinaldo.IdEntidad,
                    MtoAguinaldo = Convert.ToInt32(aguinaldo.MtoAguinaldo),
                    Periodo = Convert.ToDateTime(aguinaldo.Periodo)
                };

                DesgloseSalarial datosDesgloseSalarial = new DesgloseSalarial
                {
                    PK_DesgloseSalarial = desgloseSalarial.IdEntidad
                };


                //Guardado
                respuesta = intermedio.AgregarAguinaldo(datosTramite, datosDesgloseSalarial, datosFuncionario);

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
        /// Obtiene los registros de pagos de feriado que corresponden con los parámetros de búsqueda
        /// </summary>
        /// <returns>Retorna una lista de registros</returns>
        public List<List<CBaseDTO>> BuscarAguinaldo(CFuncionarioDTO funcionario,
                                                       DateTime periodo)
        {
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

                CAguinaldoD intermedio = new CAguinaldoD(contexto);

                List<Aguinaldo> datosTramites = new List<Aguinaldo>();
                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };


                if (funcionario.Cedula != null && !funcionario.Cedula.Equals(""))
                {
                    var resultado = ((CRespuestaDTO)intermedio.ObtenerAguinaldoPorFuncionarioPeriodo(datosFuncionario, periodo));

                    if (resultado.Codigo > 0)
                    {
                        datosTramites = (List<Aguinaldo>)resultado.Contenido;
                        if (datosTramites.Count < 1)
                        {
                            datosTramites = new List<Aguinaldo>();
                            throw new Exception("No se encontraron resultados para los parámetros especificados.");
                        }
                    }
                    else
                    {
                        datosTramites = new List<Aguinaldo>();
                    }
                }
                if (datosTramites.Count > 0)
                {
                    foreach (var item in datosTramites)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();

                        var datoPago = ConvertirDatosTramiteADto(item);

                        CAguinaldoDTO tempPago = datoPago;

                        temp.Add(tempPago);



                        CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                        {
                            Cedula = item.Funcionario.IdCedulaFuncionario,
                            Nombre = item.Funcionario.NomFuncionario,
                            PrimerApellido = item.Funcionario.NomPrimerApellido,
                            SegundoApellido = item.Funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        };

                        temp.Add(tempFuncionario);


                        respuesta.Add(temp);
                    }
                }
                else
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                    respuesta.Add(temp);
                }

                return respuesta;
            }
            catch (Exception e)
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
                return respuesta;
            }
        }


        /// <summary>
        /// Elimina un trámite especifico
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CBaseDTO EliminarAguinaldo(CAguinaldoDTO aguinaldo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CAguinaldoD intermedio = new CAguinaldoD(contexto);



                Aguinaldo datosTramite = new Aguinaldo
                {
                    PK_Aguinaldo = aguinaldo.IdEntidad
                };



                //Guardado
                respuesta = intermedio.EliminarAguinaldo(datosTramite);

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


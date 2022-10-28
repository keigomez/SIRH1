using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTipoJornadaL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoJornadaL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Metodos
        
        internal static CTipoJornadaDTO TipoJornadaGeneral(TipoJornada item)
        {
            return new CTipoJornadaDTO 
            {
                IdEntidad = item.PK_TipoJornada,
                DescripcionJornada = item.DesTipoJornada,
                DiaLibre = item.IndDiaLibre,
                InicioJornada = item.IndInicio,
                FinJornada = item.IndFin,
                JornadaAcumulativa = Convert.ToBoolean(item.IndAcumulativa)
            };
        }
        
        //Se insertó en ICFuncionarioService y CFuncionarioService el 30/01/2017
        public List<CBaseDTO> RegistrarJornadaFuncionario(CFuncionarioDTO funcionario,
                                                            CNombramientoDTO nombramiento,
                                                            CTipoJornadaDTO jornada)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CTipoJornadaD intermedio = new CTipoJornadaD(contexto);

                TipoJornada item = new TipoJornada
                {
                    DesTipoJornada = jornada.DescripcionJornada,
                    IndAcumulativa = jornada.JornadaAcumulativa,
                    IndInicio = jornada.InicioJornada,
                    IndFin = jornada.FinJornada,
                    IndDiaLibre = jornada.DiaLibre
                };

                var resultado = intermedio.RegistrarJornadaFuncionario(nombramiento.IdEntidad,
                                                                       item);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(funcionario);
                    respuesta.Add(TipoJornadaGeneral(((TipoJornada)resultado.Contenido)));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        //Se insertó en ICFuncionarioService y CFuncionarioService el 30/01/2017
        public List<CBaseDTO> EditarJornadaFuncionario(CTipoJornadaDTO jornada)
        {
            return new List<CBaseDTO>();
            //List<CBaseDTO> respuesta = new List<CBaseDTO>();
            //try
            //{
            //    CTipoJornadaD intermedio = new CTipoJornadaD(contexto);

            //    TipoJornada item = new TipoJornada
            //    {
            //        PK_TipoJornada = jornada.IdEntidad,
            //        DesTipoJornada = jornada.DescripcionJornada,
            //        IndAcumulativa = jornada.JornadaAcumulativa,
            //        IndInicio = jornada.InicioJornada,
            //        IndFin = jornada.FinJornada,
            //        IndDiaLibre = jornada.DiaLibre
            //    };

            //    var resultado = intermedio.EditarJornadaFuncionario(item);

            //    if (resultado.Codigo > 0)
            //    {
            //        respuesta.Add(CFuncionarioL.FuncionarioGeneral(
            //                                    ((TipoJornada)resultado.Contenido)
            //                                    .DetalleNombramiento.FirstOrDefault()
            //                                    .Nombramiento.Funcionario));
            //        respuesta.Add(TipoJornadaGeneral(((TipoJornada)resultado.Contenido)));
            //        return respuesta;
            //    }
            //    else
            //    {
            //        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
            //    }
            //}
            //catch (Exception error)
            //{
            //    if (respuesta.Count > 0)
            //    {
            //        respuesta.Clear();
            //    }
            //    respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            //    return respuesta;
            //}
        }


        public List<CBaseDTO> ObtenerJornadaFuncionario(CFuncionarioDTO funcionario)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CTipoJornadaD intermedio = new CTipoJornadaD(contexto);

                Funcionario funcionarioEntidad = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                    
                };

                var resultado = intermedio.CargarTipoJornadaPorFuncionarioDTO(funcionarioEntidad.IdCedulaFuncionario);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(TipoJornadaGeneral((TipoJornada)resultado.Contenido));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        #endregion
    }
}

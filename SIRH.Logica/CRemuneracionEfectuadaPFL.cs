using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CRemuneracionEfectuadaPFL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CRemuneracionEfectuadaPFL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCatalogoRemuneracionDTO ConvertirDatosCatalogoBeneficioADto(CatalogoRemuneracion item)
        {

            return new CCatalogoRemuneracionDTO
            {
                IdEntidad = item.PK_CatalogoRemuneracion,
                DescripcionRemuneracion = item.DesRemuneracion,
                PorcentajeRemuneracion = item.PorRemuneracion
            };
        }

        internal static CRemuneracionEfectuadaPFDTO ConvertirDatosRemuneracionEfectuadaADto(RemuneracionEfectuadaPF item)
        {

            return new CRemuneracionEfectuadaPFDTO
            {
                IdEntidad = item.PK_RemuneracionEfectuadaPF,
                PorcentajeEfectuado =item.PorEfectuado
            };
        }

        public CBaseDTO AgregarBeneficio(CFuncionarioDTO funcionario, CRemuneracionEfectuadaPFDTO beneficio,
                                          CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRemuneracionEfectuadaPFD intermedio = new CRemuneracionEfectuadaPFD(contexto);

                CPagoFeriadoD intermedioPagoFeriado = new CPagoFeriadoD(contexto);

                CPagoExtraordinarioD intermedioPagoExtraordinario = new CPagoExtraordinarioD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                var pagoFeriadoT = intermedioPagoFeriado.BuscarPagoFeriado(pagoFeriado.IdEntidad);

                if (pagoFeriadoT.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoFeriadoT).Contenido;
                    throw new Exception();
                }

                PagoFeriado datosFeriado = new PagoFeriado
                {
                    PK_PagoFeriado = pagoFeriado.IdEntidad
                };

                var pagoExtraordinarioT = intermedioPagoExtraordinario.BuscarPagoExtraordinario(pagoExtraordinario.IdEntidad);

                if (pagoExtraordinarioT.Codigo == -1)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)pagoExtraordinarioT).Contenido;
                    throw new Exception();
                }

                PagoExtraordinario datosExtraordinario = new PagoExtraordinario
                {
                    PK_PagoExtraordinario = pagoExtraordinario.IdEntidad
                };
                //BeneficioEfectuado datosBeneficio = new BeneficioEfectuado();

                ///var catalogoBeneficio = (CatalogoBeneficio)intermedio.BuscarCatalogoBeneficio(1).Contenido;
                RemuneracionEfectuadaPF datosBeneficio = new RemuneracionEfectuadaPF
                {
                    PorEfectuado = beneficio.PorcentajeEfectuado
                };

                //datosBeneficio.PagoFeriado = (PagoFeriado)pagoFeriadoT.Contenido;

                //datosBeneficio.PagoFeriado.PagoExtraordinario = (PagoExtraordinario)pagoExtraordinarioT.Contenido;


                respuesta = intermedio.AgregarRemuneracionEfectuada(datosBeneficio, datosFuncionario, datosExtraordinario, datosFeriado);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }

                return respuesta;
            }
            catch
            {
                return respuesta;
            }
        }


        public List<CBaseDTO> ObtenerSalarioEscolar()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CRemuneracionEfectuadaPFD intermedio = new CRemuneracionEfectuadaPFD(contexto);




                var catalogoRemuneracion = intermedio.obtenerSalarioEscolar();

                if (catalogoRemuneracion.Codigo > 0)
                {

                    var datoCatalogo = ConvertirDatosCatalogoBeneficioADto((CatalogoRemuneracion)catalogoRemuneracion.Contenido);

                    respuesta.Add(datoCatalogo);


                }
                else
                {
                    respuesta.Add((CErrorDTO)catalogoRemuneracion.Contenido);
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


        public CBaseDTO ActualizarSalarioEscolar(CCatalogoRemuneracionDTO remuneracionEfectuada)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRemuneracionEfectuadaPFD intermedio = new CRemuneracionEfectuadaPFD(contexto);


                CatalogoRemuneracion aux = new CatalogoRemuneracion
                {
                    PorRemuneracion = remuneracionEfectuada.PorcentajeRemuneracion
                };

                var catalogoRemuneracion = intermedio.actualizarSalarioEscolar(aux);

                if (catalogoRemuneracion.Codigo > 0)
                {

                    var datoCatalogo = ConvertirDatosCatalogoBeneficioADto((CatalogoRemuneracion)catalogoRemuneracion.Contenido);

                    respuesta = datoCatalogo;


                }
                else
                {
                    respuesta = (CErrorDTO)catalogoRemuneracion.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public CBaseDTO ObtenerSalarioEscolarEfectuado(int codigoTramite)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CRemuneracionEfectuadaPFD intermedio = new CRemuneracionEfectuadaPFD(contexto);

                var salarioEscolar = intermedio.obtenerSalarioEscolarEfectuado(codigoTramite);

                if (salarioEscolar.Codigo > 0)
                {
                    respuesta = ConvertirDatosRemuneracionEfectuadaADto((RemuneracionEfectuadaPF)salarioEscolar.Contenido);
                }
                else
                {
                    respuesta=((CErrorDTO)salarioEscolar.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        #endregion

    }
}

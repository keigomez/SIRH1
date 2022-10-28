using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CRemuneracionEfectuadaPFD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CRemuneracionEfectuadaPFD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO AgregarRemuneracionEfectuada(RemuneracionEfectuadaPF beneficioEfectuado, Funcionario funcionario, PagoExtraordinario pagoExtraordinario, PagoFeriado pagoFeriado)
        {
            CRespuestaDTO respuesta;
            try
            {
                funcionario = entidadBase.Funcionario
                                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null)
                {  //existe el funcionario
                    pagoExtraordinario = entidadBase.PagoExtraordinario.
                        Where(PE => PE.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario &&
                            PE.Funcionario.PK_Funcionario == funcionario.PK_Funcionario).FirstOrDefault();
                    if (pagoExtraordinario != null)
                    {//existe un pago extraordinario
                        pagoFeriado = entidadBase.PagoFeriado.Where(PF => PF.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado
                            && PF.PagoExtraordinario.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario && PF.EstadoTramite.PK_EstadoTramite == 1).FirstOrDefault();
                        if (pagoFeriado != null)
                        {//existe un pago de feriado
                            // var deduccionEfctda = entidadBase.PagoFeriado.Where(PF => PF.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado && PF.DeduccionEfectuada.Where(DE => DE.CatalogoDeduccion.PK_CatalogoDeduccion == deduccionEfectuada.CatalogoDeduccion.PK_CatalogoDeduccion).Count() > 0);
                            //if (deduccionEfctda.Count() == 0)
                            // {
                            //no se ingreso la deduccion respectiva
                            CatalogoRemuneracion catalogoBeneficio = entidadBase.CatalogoRemuneracion.Where(CD => CD.PK_CatalogoRemuneracion == 1).FirstOrDefault();

                            pagoFeriado.RemuneracionEfectuadaPF.Add(beneficioEfectuado);
                            catalogoBeneficio.RemuneracionEfectuadaPF.Add(beneficioEfectuado);

                            entidadBase.RemuneracionEfectuadaPF.Add(beneficioEfectuado);

                            entidadBase.SaveChanges();
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = "Guardado"
                            };
                            return respuesta;
                            // }
                            //else
                            // {
                            //  throw new Exception("Ya efectuó la deducción respectiva");
                            //}
                        }

                        else
                        {
                            throw new Exception("No existe el registro de pago de feriado indicado");
                        }

                    }
                    else
                    {
                        throw new Exception("No existe el registro de pago extraordinario indicado");
                    }
                }
                else
                {
                    throw new Exception("No existe el funcionario indicado");
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


        public CRespuestaDTO obtenerSalarioEscolar()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoRemuneracion.Where(C => C.PK_CatalogoRemuneracion == 1).FirstOrDefault();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                }
                else
                {
                    throw new Exception("No se encontró el salario escolar");
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
            return respuesta;
        }

        public CRespuestaDTO actualizarSalarioEscolar(CatalogoRemuneracion salarioEscolar)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                CatalogoRemuneracion datosEntidad = entidadBase.CatalogoRemuneracion.Where(C => C.PK_CatalogoRemuneracion == 1).FirstOrDefault();

                if (datosEntidad != null)
                {
                    datosEntidad.PorRemuneracion = salarioEscolar.PorRemuneracion;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                }
                else
                {
                    throw new Exception("No se encontró el salario escolar");
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
            return respuesta;
        }

        public CRespuestaDTO obtenerSalarioEscolarEfectuado(int codigoTramite)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.RemuneracionEfectuadaPF.Where(C => C.PagoFeriado.PK_PagoFeriado == codigoTramite).FirstOrDefault();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                }
                else
                {
                    throw new Exception("No se encontró el salario escolar efectuado para el trámite de pago indicado");
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
            return respuesta;
        }

        #endregion
    }
}





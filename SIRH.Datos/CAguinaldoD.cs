using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Datos
{
    public class CAguinaldoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CAguinaldoD(SIRHEntities contextoGlobal)
        {
            entidadBase = contextoGlobal;
        }

        #endregion

        #region Métodos
        /// Regresa la lista de los aguinaldos
        public List<Aguinaldo> RetornarAguinaldo()
        {
            return entidadBase.Aguinaldo.ToList();
        }

        public CRespuestaDTO AgregarAguinaldo(Aguinaldo monto, DesgloseSalarial pago, Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {

                funcionario = entidadBase.Funcionario
                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null) //si existe el funcionario
                {
                    pago = entidadBase.DesgloseSalarial.Where(DE => DE.PK_DesgloseSalarial == pago.PK_DesgloseSalarial).FirstOrDefault();

                    if (pago != null)
                    { //si existe el pago del salario

                        entidadBase.Aguinaldo.Add(monto);
                        entidadBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = monto.PK_Aguinaldo
                        };
                        return respuesta;


                    }
                    else
                    {
                        throw new Exception("No se encontró el registro del pago indicado");
                    }
                }

                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
                }
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

        /// <summary>
        /// Calcula el monto del aguinaldo
        /// </summary>
        /// <returns>Guarda el monto del aguinaldo</returns>
        public CRespuestaDTO RegistrarAguinaldoPorSalario(Funcionario funcionario, CDesgloseSalarialDTO desgloseS)
        {

            CRespuestaDTO respuesta;

            try
            {
                funcionario = entidadBase.Funcionario
                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                var desgloseDatos1 = entidadBase.DesgloseSalarial.Include("DesgloseSalarial")
                                                                .Include("DesgloseSalarial.DetalleDesgloseSalarial")
                                                                .Include("Aguinaldo")
                                                            .Include("Aguinaldo.MtoAguinaldo")
                                                            .Include("Aguinaldo.Periodo")
                                                            .Include("Nombramiento")
                                                            .Include("Nombramiento.Funcionario")
                                                            .FirstOrDefault(D => D.PK_DesgloseSalarial == desgloseS.IdEntidad);


                var periodo = desgloseDatos1.IndPeriodo;

                var temp = desgloseDatos1.DetalleDesgloseSalarial.Where(D => D.FK_DesgloseSalarial == desgloseDatos1.PK_DesgloseSalarial);



                if (desgloseDatos1.IndPeriodo.Month == 12)
                {
                    DateTime Diciembre = desgloseDatos1.IndPeriodo.AddYears(1);
                    periodo = Diciembre;
                }
                else
                {
                    periodo = desgloseDatos1.IndPeriodo;
                }



                if (desgloseDatos1 != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = desgloseDatos1,
                        Mensaje = "Aguinaldo"
                    };
                }
                else
                {
                    throw new Exception("No se encontraron datos asociados al número de cédula ingresado");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        /// <summary>
        /// Busca el monto de aguinaldo a pagar del funcionario
        /// </summary>
        /// <returns>Retorna el el monto de aguinaldo a pagar</returns>
        public CRespuestaDTO ObtenerAguinaldoPorFuncionarioPeriodo(Funcionario funcionario, DateTime periodo)
        {

            CRespuestaDTO respuesta;
            try
            {

                funcionario = entidadBase.Funcionario
                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                var aguinaldo = entidadBase.Aguinaldo.Include("DesgloseSalarial")
                                                                .Include("DesgloseSalarial.DetalleDesgloseSalarial")
                                                                .Include("DesgloseSalarial.Nombramiento")
                                                                .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                                                .Include("MtoAguinaldo")
                                                                .Include("Periodo")
                                                                .Where(A => A.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && A.Periodo == periodo);
                if (aguinaldo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = aguinaldo,
                        Mensaje = "Aguinaldo"
                    };
                }
                else
                {
                    throw new Exception("No se encontraron datos asociados al número de cédula ingresado");
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        private string DefinirPeriodoPago(DateTime fechapago)
        {
            string respuesta = "";




            if (fechapago.Month == 12)
            {
                DateTime Diciembre = fechapago.AddYears(1);
                respuesta += Diciembre.Year.ToString();
            }
            else
            {
                respuesta += fechapago.Year.ToString();


            }


            return respuesta;
        }

        public CRespuestaDTO EliminarAguinaldo(Aguinaldo aguinaldo)
        {
            CRespuestaDTO respuesta;
            try
            {
                aguinaldo = entidadBase.Aguinaldo
                    .Where(F => F.PK_Aguinaldo == aguinaldo.PK_Aguinaldo).FirstOrDefault();

                if (aguinaldo != null)
                {
                    entidadBase.Aguinaldo.Remove(aguinaldo);
                    entidadBase.SaveChanges();


                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "Eliminado"
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el aguinaldo indicado");
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
        #endregion
    }
}


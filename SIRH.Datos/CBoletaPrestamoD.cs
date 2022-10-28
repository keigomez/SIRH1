using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CBoletaPrestamoD {

        #region Variables
        private SIRHEntities entidadBase = new SIRHEntities();
        #endregion
        #region Coonstructor
        public CBoletaPrestamoD(SIRHEntities entidadGlobal) {
            entidadBase = entidadGlobal;
        }
        #endregion
        #region Métodos

        public CRespuestaDTO GuardarBoletaPrestamo(BoletaPrestamo boleta)
        {
            CRespuestaDTO respuesta;

            string fmt = "00000";
            string numBoleta = "";
                    
            try
            {
                var annio = DateTime.Today.Year.ToString();
                var consecutivo = entidadBase.CatAnioBoletaPrestamo.Where(Q => Q.AnioRige.Contains(annio)).FirstOrDefault();

                if (consecutivo != null)
                {
                    numBoleta = annio + "-" + consecutivo.NumConsecutivo.ToString(fmt);
                }
                else
                {
                    consecutivo = new CatAnioBoletaPrestamo
                    {
                        AnioRige = annio,
                        NumConsecutivo = 1
                    };

                    entidadBase.CatAnioBoletaPrestamo.Add(consecutivo);
                    entidadBase.SaveChanges();
                    numBoleta = annio + "-" + consecutivo.NumConsecutivo.ToString(fmt);
                }

                boleta.NumBoleta = numBoleta;
                entidadBase.BoletaPrestamo.Add(boleta);
                entidadBase.SaveChanges();

                // Actualizar el consecutivo
                var datoConsecutivo = entidadBase.CatAnioBoletaPrestamo.Where(Q => Q.AnioRige.Contains(annio)).FirstOrDefault();
                datoConsecutivo.NumConsecutivo += 1;
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = boleta.PK_IdBoletaPrestamo
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

        public CRespuestaDTO GuardarDatosBoleta(DetalleBoleta datos)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.DetalleBoleta.Add(datos);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = datos.PK_IdDetalleBoleta
                };
                return respuesta;
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerBoletaPrestamo(int codigo) {
            CRespuestaDTO respuesta;
            try
            {
                var boleta = entidadBase.BoletaPrestamo.Where(b => b.PK_IdBoletaPrestamo == codigo).FirstOrDefault();
                if (boleta != null) {

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = boleta
                    };
                }
                else {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la boleta"
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

        public CRespuestaDTO ObtenerDetalleBoleta(BoletaPrestamo boleta) {
            CRespuestaDTO respuesta;
            try
            {
                var detalle = entidadBase.DetalleBoleta.Where(d => d.FK_IdBoletaPrestamo == boleta.PK_IdBoletaPrestamo).FirstOrDefault();
                if (detalle != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle
                    };
                }
                else {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de detalle."
                    };
                }
                return respuesta;
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


        public CRespuestaDTO ObtenerUsuario(BoletaPrestamo boleta) {
            CRespuestaDTO respuesta;
            try
            {
                var usuario = entidadBase.Usuario.Where(u => u.PK_Usuario == boleta.FK_Usuario).FirstOrDefault();
                if (usuario != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = usuario
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de usuario."
                    };
                }
                return respuesta;
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

        public CRespuestaDTO ObtenerExpedienteFuncionario(BoletaPrestamo boleta) {
            CRespuestaDTO respuesta;
            try
            {
                var expediente = entidadBase.ExpedienteFuncionario.Include("Funcionario").Where(u => u.PK_IdExpedienteFuncionario == boleta.FK_ExpedienteFuncionario).FirstOrDefault();
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
                        Contenido = "Ocurrió un error al leer los datos de usuario."
                    };
                }
                return respuesta;
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

        public CRespuestaDTO ObtenerFuncionario(ExpedienteFuncionario expediente) {
            CRespuestaDTO respuesta;
            try
            {
                var funcionario = entidadBase.Funcionario.Where(u => u.PK_Funcionario == expediente.FK_Funcionario).FirstOrDefault();
                if (funcionario != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = funcionario
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del funcionario."
                    };
                }
                return respuesta;
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

        public CRespuestaDTO VerificarExistenciaBoleta(string cedula) {
            CRespuestaDTO respuesta;
            try
            {
                Funcionario fun = entidadBase.Funcionario.Where(f => f.IdCedulaFuncionario == cedula).FirstOrDefault();
                BoletaPrestamo temp = entidadBase.BoletaPrestamo.Where(f => f.ExpedienteFuncionario.FK_Funcionario == fun.PK_Funcionario).FirstOrDefault();

                if (temp != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = temp
                    };
                }
                else {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al obtener la boleta."
                    };
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = "Ocurrió un error al leer los datos de la boleta."
                };
            }
            return respuesta;
        }

        public CRespuestaDTO VerificarExistenciaBoletaSegunParametros(string parametro, string dato, string fecha_inicio, string fecha_fin) {

            CRespuestaDTO respuesta = new CRespuestaDTO();
            List<BoletaPrestamo> listaBoletas = new List<BoletaPrestamo>();
            try
            {
                BoletaPrestamo boleta;
                DateTime fechaInicio;
                DateTime fechaFin;

                switch (parametro) {

                    case "Cédula Funcionario":

                        Funcionario fun = entidadBase.Funcionario.Where(f => f.IdCedulaFuncionario == dato).FirstOrDefault();
                        if (fun != null)
                        {
                            listaBoletas = entidadBase.BoletaPrestamo.Where(f => f.ExpedienteFuncionario.FK_Funcionario == fun.PK_Funcionario).ToList();
                            if (listaBoletas.Count() > 0)
                            {
                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = listaBoletas
                                };
                            }
                            else
                            {
                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = -1,
                                    Contenido = new CErrorDTO { MensajeError = "No existen préstamos de expediente bajo la cédula de funcionario recibida." }
                                };
                            }
                        }
                        else {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = -1,
                                Contenido = new CErrorDTO { MensajeError = "La cédula ingresada no corresponde a ningún funcionario del MOPT." }
                            };
                        }
                        break;

                    case "Cédula Solicitante":

                         listaBoletas = entidadBase.BoletaPrestamo.Where(f => f.IdCedulaSolicitante == dato).ToList();

                        if (listaBoletas.Count() > 0)
                        {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = listaBoletas
                            };
                        }
                        else
                        {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = -1,
                                Contenido = new CErrorDTO { MensajeError = "No existen préstamos de expediente bajo la cédula de solicitante recibida." }
                            };
                        }
                        break;

                    case "Número Expediente":
                        int valor = Convert.ToInt32(dato);
                        listaBoletas = entidadBase.BoletaPrestamo.Where(f => f.ExpedienteFuncionario.numExpediente == valor).ToList();
                        if (listaBoletas.Count() > 0)
                        {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = listaBoletas
                            };
                        }
                        else
                        {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = -1,
                                Contenido = new CErrorDTO { MensajeError = "No existen préstamos de expediente bajo el número de expediente indicado." }
                            };
                        }
                        break;

                    case "Fecha Préstamo":
                        fechaInicio = Convert.ToDateTime(fecha_inicio);
                        fechaFin = Convert.ToDateTime(fecha_fin);

                        var listaDetalles = entidadBase.DetalleBoleta.Where(d => d.FecPrestamo >= fechaInicio && d.FecPrestamo <= fechaFin).ToList();
                        if (listaDetalles.Count() > 0)
                        {
                            foreach (DetalleBoleta detalle in listaDetalles)
                            {
                                int fk_boleta = Convert.ToInt32(detalle.FK_IdBoletaPrestamo);
                                BoletaPrestamo item = entidadBase.BoletaPrestamo.Where(b => b.PK_IdBoletaPrestamo == fk_boleta).FirstOrDefault();
                                listaBoletas.Add(item);
                            }

                            respuesta = new CRespuestaDTO {
                                Codigo = 1,
                                Contenido = listaBoletas
                            };
                        }
                        else {
                            respuesta = new CRespuestaDTO {
                                Codigo = -1,
                                Contenido = new CErrorDTO { MensajeError = "No existen préstamos de expediente bajo el rango de fechas seleccionada." }
                            };
                        }
                        break;

                    case "Fecha Devolución":              
                        fechaInicio = Convert.ToDateTime(fecha_inicio);
                        fechaFin = Convert.ToDateTime(fecha_fin);

                        var listaDevolucion = entidadBase.DetalleBoleta.Where(d => d.FecCaducidad >= fechaInicio && d.FecCaducidad <= fechaFin).ToList();
                        if (listaDevolucion.Count() > 0)
                        {
                            foreach (DetalleBoleta detalle in listaDevolucion)
                            {
                                int fk_boleta = Convert.ToInt32(detalle.FK_IdBoletaPrestamo);
                                BoletaPrestamo item = entidadBase.BoletaPrestamo.Where(b => b.PK_IdBoletaPrestamo == fk_boleta).FirstOrDefault();
                                listaBoletas.Add(item);
                            }

                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = listaBoletas
                            };
                        }
                        else
                        {
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = -1,
                                Contenido = new CErrorDTO { MensajeError = "No existen préstamos de expediente bajo el rango de fechas seleccionada." }
                            };
                        }
                        break;

                    default:
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = new CErrorDTO { MensajeError = "Ocurrió un error inesperado." }
                        };
                        break;
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = "Ocurrió un error al consultar disponibilidad del expediente." }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO VerificarFechaVencimientoPrestamo(DateTime fecha) {

            List<BoletaPrestamo> listaBoletas = new List<BoletaPrestamo>();
            CRespuestaDTO respuesta;
            //temp.AddDays(3);
            try
            {
                List<BoletaPrestamo> temp = new List<BoletaPrestamo>(); // lista temporal.
                temp = entidadBase.BoletaPrestamo.Include("DetalleBoleta").Include("ExpedienteFuncionario").ToList(); // obtnemos todas las boletas existentes.
                if (temp.Count() > 0)
                {

                    foreach (BoletaPrestamo item in temp) {

                        DateTime fecha_previa_devolucion = item.DetalleBoleta.FirstOrDefault().FecCaducidad.Value.AddDays(-3).Date; //obtenemos la fecha, 3 días antes de que se venza el plazo.

                        if ( (fecha.Date >= fecha_previa_devolucion) &&
                             (fecha.Date <= item.DetalleBoleta.FirstOrDefault().FecCaducidad.Value.Date) &&
                             (item.ExpedienteFuncionario.Estado == 1)) // 1 = Prestado, 2 = NoPrestado
                        {
                            listaBoletas.Add(item);
                        }
                    }

                    temp.RemoveAll(t => t.FK_Usuario > 0);
                    temp = null;

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = listaBoletas
                    };
                }
                else {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No hay préstamos proóximos a vencer."}
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

        #endregion
    }
}

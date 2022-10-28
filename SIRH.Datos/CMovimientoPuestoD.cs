using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
   public class CMovimientoPuestoD
   {
       #region Variables

       SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CMovimientoPuestoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        public CRespuestaDTO HistorialMovimientos(string codPuesto)
        {
            CRespuestaDTO respuesta;

            try
            {
                var movimientos = contexto.MovimientoPuesto.Include("Puesto").Include("MotivoMovimiento").Include("EstadoMovimientoPuesto")
                                                .Where(Q => Q.Puesto.CodPuesto == codPuesto).ToList();

                if (movimientos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = movimientos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron movimientos relacionados al puesto solicitado, por favor verificar los parámetros indicados.");
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

        public CRespuestaDTO DetalleMovimiento(int codMovimiento)
        {
            CRespuestaDTO respuesta;

            try
            {
                var movimiento = contexto.MovimientoPuesto.Include("Puesto")
                                                          .Include("Puesto.Nombramiento")
                                                          .Include("Puesto.DetallePuesto")
                                                          .Include("Puesto.DetallePuesto.Especialidad")
                                                          .Include("Puesto.DetallePuesto.OcupacionReal")
                                                          .Include("Puesto.DetallePuesto.Clase")
                                                          .Include("Puesto.UbicacionAdministrativa")
                                                          .Include("Puesto.UbicacionAdministrativa.Presupuesto")
                                                          .Include("Puesto.UbicacionAdministrativa.Division")
                                                          .Include("Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                          .Include("Puesto.UbicacionAdministrativa.Departamento")
                                                          .Include("Puesto.UbicacionAdministrativa.Seccion")
                                                          .Include("MotivoMovimiento")
                                                          .Include("EstadoMovimientoPuesto")
                                                .Where(Q => Q.PK_MovimientoPuesto == codMovimiento 
                                                        && Q.Puesto.Nombramiento.Where(P => P.FecVence == Q.FecMovimiento).Count() > 0).FirstOrDefault();

                if (movimiento != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = movimiento
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el movimiento de puesto solicitado, por favor verificar los parámetros indicados.");
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
                       
        public MovimientoPuesto RetornarMovimientoPuestoEspecificoOficio(string CodOficio)
        {
            return contexto.MovimientoPuesto.Where(R => R.CodOficio == CodOficio).FirstOrDefault();
        }

        public MovimientoPuesto RetornarMovimientoPuestoEspecifico(string NumeroPuesto)
        {
            return contexto.MovimientoPuesto.Where(R => R.Puesto.CodPuesto == NumeroPuesto).FirstOrDefault();
        }
          //GUARDAR EL MOVIMIENTO DE PUESTO EN PUESTO
        public CRespuestaDTO InsertarMovimientoPuesto(string puesto, MovimientoPuesto movimiento)
        {
            CRespuestaDTO respuesta;
            try
            {
                movimiento.FecMovimiento = Convert.ToDateTime(movimiento.FecMovimiento).AddDays(1);
                contexto.MovimientoPuesto.Add(movimiento);
                int a = contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = movimiento
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

        public CRespuestaDTO GuardarMovimientoPuesto(MovimientoPuesto movimiento, 
                                                     MotivoMovimiento motivo, Nombramiento nombramiento)
        {
            try
            {
                int estado = DeterminarEstadoFuncionario(motivo.PK_MotivoMovimiento);
                if (estado > 0)
                {
                    //Altera el estado del funcionario asociado al nombramiento
                    nombramiento.Funcionario.EstadoFuncionario
                        = contexto.EstadoFuncionario.Where(E =>
                        E.PK_EstadoFuncionario == estado).FirstOrDefault();
                }

                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = null
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }
       
         //Guarda los movimientos de puestos en la BD        
         //<returns>Retorna los movimientos de puestos</returns>
        public int GuardarMovimientoPuesto(MovimientoPuesto movimientoPuestoRef)
        {
            int pkMotivo = movimientoPuestoRef.MotivoMovimiento.PK_MotivoMovimiento;
            movimientoPuestoRef.MotivoMovimiento = null;
            movimientoPuestoRef.Puesto = contexto.Puesto
                                                        .Include("Nombramiento")
                                                        .Include("Nombramiento.Funcionario")
                                                        .Where(Q => Q.CodPuesto == movimientoPuestoRef.Puesto.CodPuesto)
                                                        .FirstOrDefault();

            DateTime fechaMaxima = DateTime.Now.AddDays(-5);
            Nombramiento temp = contexto.Nombramiento.Include("Funcionario").Include("Funcionario.EstadoFuncionario")
                                                .Where(N => (N.FecVence >= fechaMaxima || N.FecVence == null)
                                                && N.Puesto.CodPuesto == movimientoPuestoRef.Puesto.CodPuesto && N.FK_EstadoNombramiento != 6 && N.FK_EstadoNombramiento != 7 
                                                && N.FK_EstadoNombramiento != 8 && N.FK_EstadoNombramiento != 10 && N.FK_EstadoNombramiento != 15)
                                                .OrderByDescending(Q => Q.FecRige)
                                                .FirstOrDefault();

            if (pkMotivo == 46 || pkMotivo == 49 || pkMotivo == 50)
            {
                if (temp.FecVence != null)
                {
                    temp.FecVence = Convert.ToDateTime(movimientoPuestoRef.FecMovimiento).AddDays(-1);
                }
                if (pkMotivo == 46)
                {
                    contexto.Nombramiento.Add(new Nombramiento
                    {
                        FecNombramiento = DateTime.Now,
                        FK_Puesto = movimientoPuestoRef.Puesto.PK_Puesto,
                        FK_Funcionario = temp.FK_Funcionario,
                        FecRige = movimientoPuestoRef.FecMovimiento,
                        FecVence = movimientoPuestoRef.FecVence,
                        FK_EstadoNombramiento = 7
                    });
                }
                if (pkMotivo == 49)
                {
                    contexto.Nombramiento.Add(new Nombramiento
                    {
                        FecNombramiento = DateTime.Now,
                        FK_Puesto = movimientoPuestoRef.Puesto.PK_Puesto,
                        FK_Funcionario = temp.FK_Funcionario,
                        FecRige = movimientoPuestoRef.FecMovimiento,
                        FecVence = movimientoPuestoRef.FecVence,
                        FK_EstadoNombramiento = 8
                    });
                }
                if (pkMotivo == 50)
                {
                    contexto.Nombramiento.Add(new Nombramiento
                    {
                        FecNombramiento = DateTime.Now,
                        FK_Puesto = movimientoPuestoRef.Puesto.PK_Puesto,
                        FK_Funcionario = temp.FK_Funcionario,
                        FecRige = movimientoPuestoRef.FecMovimiento,
                        FecVence = movimientoPuestoRef.FecVence,
                        FK_EstadoNombramiento = 10
                    });
                }
            }
            else
            {
                temp.FecVence = movimientoPuestoRef.FecMovimiento;
                List<int> motivosMovimiento = new List<int> { 7, 9, 10, 11, 13, 14, 15, 16, 19, 20, 35, 45, 47, 48 };
                if (motivosMovimiento.Contains(pkMotivo))
                {
                    var nombramientosAdicionales = contexto.Nombramiento.Include("Funcionario").Include("Funcionario.EstadoFuncionario")
                                               .Where(N => (N.FecVence >= fechaMaxima || N.FecVence == null)
                                               && N.FK_EstadoNombramiento != 15
                                               && N.PK_Nombramiento != temp.PK_Nombramiento
                                               && N.FK_Funcionario == temp.FK_Funcionario)
                                               .OrderByDescending(Q => Q.FecRige).ToList();
                    if (nombramientosAdicionales.Count() > 0)
                    {
                        foreach (var item in nombramientosAdicionales)
                        {
                            item.FecVence = movimientoPuestoRef.FecMovimiento;
                        }
                    }
                }
            }
            
            var nombramientoCierre = contexto.Nombramiento.Where(N => N.FK_Puesto == movimientoPuestoRef.Puesto.PK_Puesto && N.FK_Funcionario != temp.Funcionario.PK_Funcionario && N.FecVence >= DateTime.Now);

            if (!(temp.Funcionario.FK_EstadoFuncionario == 2 && pkMotivo == 15 && nombramientoCierre.Count() > 0))
            {
                if (pkMotivo != 21 && pkMotivo != 66)
                {
                    int estadoPuesto = DeterminarEstadoPuesto(pkMotivo);

                    movimientoPuestoRef.Puesto.EstadoPuesto = contexto.EstadoPuesto.Where(Q => Q.PK_EstadoPuesto == estadoPuesto).FirstOrDefault();
                }
            }


            int estado = DeterminarEstadoFuncionario(pkMotivo);

            if (estado > 0)
            {
                temp.Funcionario.EstadoFuncionario
                                        = contexto.EstadoFuncionario.Where(E =>
                                        E.PK_EstadoFuncionario == estado).FirstOrDefault();
                if (estado < 45)
                {
                    var contrato = contexto.DetalleContratacion.Where(C => C.FK_Funcionario == temp.Funcionario.PK_Funcionario).FirstOrDefault();

                    contrato.FecCese = movimientoPuestoRef.FecMovimiento;
                }
            }
            movimientoPuestoRef.MotivoMovimiento = contexto.MotivoMovimiento.
                                                    Where(Q => Q.PK_MotivoMovimiento == pkMotivo).
                                                    FirstOrDefault();
            movimientoPuestoRef.FecMovimiento = Convert.ToDateTime(movimientoPuestoRef.FecMovimiento).AddDays(1);
            
            contexto.MovimientoPuesto.Add(movimientoPuestoRef);            
            contexto.SaveChanges();
            return movimientoPuestoRef.PK_MovimientoPuesto;
        }

        private int DeterminarEstadoPuesto(int motivoMovimiento)
        {
            int respuesta = 0;

            switch (motivoMovimiento)
            {
                case 7:
                case 9:
                case 10:
                case 11:
                case 13:
                case 14:
                case 15:
                case 16:
                case 19:
                case 20:
                case 35:
                case 51:
                case 52:
                    respuesta = 24;
                    break;
                case 12:
                    respuesta = 6;
                    break;
                //case 17:
                //case 65:
                //    respuesta = 5;
                //    break;
                case 45:
                case 47:
                case 48:
                    respuesta = 25;
                    break;
                case 46:
                case 49:
                case 50:
                    respuesta = 26;
                    break;
                default:
                    break;
            }

            return respuesta;
        }

        private int DeterminarEstadoFuncionario(int motivoMovimiento)
        {
            int respuesta = 0;

            switch (motivoMovimiento)
            {
                case 7: // Despido causa
                    respuesta = 7;
                    break;
                case 9: // Pensión CCSS
                    respuesta = 10;
                    break;
                case 10: // Cese nombramiento plazo fijo
                    respuesta = 14;
                    break;
                case 11: // Cese de funciones
                    respuesta = 8;
                    break;
                case 12: // PSS
                    respuesta = 2;
                    break;
                case 13:
                case 20:// Pensión Estado
                    respuesta = 9;
                    break;
                case 14: // Pensión CCSS
                    respuesta = 10;
                    break;
                case 15: // Renuncia
                    respuesta = 12;
                    break;
                case 16: // Renuncia
                    respuesta = 12;
                    break;
                case 17: // Suspensión indefinida
                case 65:
                    respuesta = 3;
                    break;
                case 18: // Agotó subsidio
                    respuesta = 5;
                    break;
                case 19:
                    respuesta = 11;
                    break;
                case 21: // Suspensión tiempo definido
                case 66:
                    respuesta = 24;
                    break;
                case 22: // Permiso con salario
                    respuesta = 6;
                    break;
                case 35: // Cese interinidad
                    respuesta = 13;
                    break;
                case 45: //Traslados en propiedad
                case 47:
                case 48:
                    respuesta = 4;
                    break;
                case 46: //Traslados interinos
                case 49:
                case 50:
                    respuesta = 25;
                    break;
                default:
                    break;
            }
            return respuesta;
        }

        public List<MovimientoPuesto> BuscarMovimientoPuestosFiltros(string codPuesto, string numCedula, int motivoM, DateTime? fechaHasta, DateTime? fechaDesde)
        {
            try
            {
                List<MovimientoPuesto> resultadoTotal = new List<MovimientoPuesto>();
                List<MovimientoPuesto> movimiento = contexto.MovimientoPuesto.Include("Puesto")
                                                          .Include("Puesto.Nombramiento")
                                                          .Include("Puesto.EstadoPuesto")
                                                          .Include("Puesto.Nombramiento.Funcionario")
                                                          .Include("Puesto.DetallePuesto.Especialidad")
                                                          .Include("Puesto.DetallePuesto.Clase")
                                                          .Include("MotivoMovimiento")
                                                          .Include("EstadoMovimientoPuesto")
                                                          .Where(Q => Q.FK_EstadoMovimientoPuesto == 1 && Q.FK_MotivoMovimiento != null) // Activo
                                                          .OrderByDescending(Q => Q.PK_MovimientoPuesto).ToList();

                resultadoTotal = movimiento;

                if (movimiento != null)
                {
                    if (codPuesto != null && codPuesto != "")
                    {
                        movimiento = movimiento.Where(M => M.Puesto.CodPuesto.Contains(codPuesto)).ToList();
                    }
                    if (numCedula != null && numCedula != "")
                    {
                        movimiento = movimiento.Where(M => M.Puesto.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == numCedula).Count() > 0).ToList();
                    }
                    if (motivoM != 0)
                    {
                        switch (motivoM)
                        {
                            case -1:
                                movimiento = movimiento.Where(M => M.MotivoMovimiento.PK_MotivoMovimiento == 7 ||
                                (M.MotivoMovimiento.PK_MotivoMovimiento >= 9 && M.MotivoMovimiento.PK_MotivoMovimiento <= 11) ||
                                (M.MotivoMovimiento.PK_MotivoMovimiento >= 13 && M.MotivoMovimiento.PK_MotivoMovimiento <= 16) ||
                                (M.MotivoMovimiento.PK_MotivoMovimiento >= 18 && M.MotivoMovimiento.PK_MotivoMovimiento <= 20)).ToList();
                                break;
                            case -2:
                                movimiento = movimiento.Where(M => (M.MotivoMovimiento.PK_MotivoMovimiento >= 2 && M.MotivoMovimiento.PK_MotivoMovimiento <= 6) ||
                                M.MotivoMovimiento.PK_MotivoMovimiento == 8 || M.MotivoMovimiento.PK_MotivoMovimiento == 23).ToList();
                                break;
                            case -3:
                                movimiento = movimiento.Where(M => M.MotivoMovimiento.PK_MotivoMovimiento == 12 ||
                                M.MotivoMovimiento.PK_MotivoMovimiento == 17 || M.MotivoMovimiento.PK_MotivoMovimiento == 21 ||
                                M.MotivoMovimiento.PK_MotivoMovimiento == 22).ToList();
                                break;
                            case -4:
                                movimiento = movimiento.Where(M => M.MotivoMovimiento.PK_MotivoMovimiento != 1).ToList();
                                break;
                            case -5:
                                movimiento = movimiento.Where(M => (M.FK_MotivoMovimiento == 5 || M.FK_MotivoMovimiento == 6 || M.FK_MotivoMovimiento == 7
                                                              || M.FK_MotivoMovimiento == 9 || M.FK_MotivoMovimiento == 10 || M.FK_MotivoMovimiento == 11
                                                              || M.FK_MotivoMovimiento == 12 || M.FK_MotivoMovimiento == 13 || M.FK_MotivoMovimiento == 14
                                                              || M.FK_MotivoMovimiento == 15 || M.FK_MotivoMovimiento == 16 || M.FK_MotivoMovimiento == 19
                                                              || M.FK_MotivoMovimiento == 20 || M.FK_MotivoMovimiento == 35 || M.FK_MotivoMovimiento == 45
                                                              || M.FK_MotivoMovimiento == 46 || M.FK_MotivoMovimiento == 47 || M.FK_MotivoMovimiento == 48
                                                              || M.FK_MotivoMovimiento == 49 || M.FK_MotivoMovimiento == 50 || M.FK_MotivoMovimiento == 51
                                                              || M.FK_MotivoMovimiento == 52) || (M.ObsMovimientoPuesto != null && M.ObsMovimientoPuesto.StartsWith("Vacante por"))).ToList();
                                //movimiento = movimiento.Where(M => (M.MotivoMovimiento.PK_MotivoMovimiento >= 3 && M.MotivoMovimiento.PK_MotivoMovimiento <= 11) ||
                                //                                (M.MotivoMovimiento.PK_MotivoMovimiento == 12 && ((DateTime)M.FecVence).Subtract((DateTime)M.FecMovimiento).TotalDays >= 30)
                                //                                || (M.MotivoMovimiento.PK_MotivoMovimiento >= 13 && M.MotivoMovimiento.PK_MotivoMovimiento <= 16)
                                //                                || M.MotivoMovimiento.PK_MotivoMovimiento == 19
                                //                                || M.MotivoMovimiento.PK_MotivoMovimiento == 20 || M.MotivoMovimiento.PK_MotivoMovimiento == 35 ||
                                //                                (M.MotivoMovimiento.PK_MotivoMovimiento >= 45 && M.MotivoMovimiento.PK_MotivoMovimiento <= 52) ||
                                //                                M.MotivoMovimiento.PK_MotivoMovimiento == 63 || M.ObsMovimientoPuesto.StartsWith("Vacante por")).ToList();
                                break;
                            default:
                                movimiento = movimiento.Where(M => M.MotivoMovimiento.PK_MotivoMovimiento == motivoM).ToList();
                                break;
                        }
                    }

                    if (fechaDesde != null && fechaHasta != null)
                    {
                        movimiento = movimiento.Where(M => M.FecMovimiento >= fechaDesde && M.FecMovimiento <= fechaHasta).ToList();
                    }
                    //if (fechaHasta != null)
                    //{
                    //    //movimiento = movimiento.Where(M => (Convert.ToDateTime(M.FecMovimiento).Year <= Convert.ToDateTime(fechaDesde).Year &&
                    //    //(Convert.ToDateTime(M.FecMovimiento).Month > Convert.ToDateTime(fechaHasta).Month ||
                    //    //Convert.ToDateTime(M.FecMovimiento).Month == Convert.ToDateTime(fechaDesde).Month) &&
                    //    //(Convert.ToDateTime(M.FecMovimiento).Day > Convert.ToDateTime(fechaDesde).Day ||
                    //    //Convert.ToDateTime(M.FecMovimiento).Day == Convert.ToDateTime(fechaDesde).Day)).ToList();

                    //    movimiento = movimiento.Where(M => Convert.ToDateTime(M.FecMovimiento) <= Convert.ToDateTime(fechaDesde)).ToList();
                    //}

                    if (movimiento.Count < 1)
                    {
                        throw new Exception("No se encontraron movimientos, por favor verificar los parámetros indicados.");
                    }
                    else
                    {
                        resultadoTotal = movimiento;
                    }

                    return resultadoTotal;
                }
                else
                {
                    throw new Exception("No se encontró el movimiento de puesto solicitado, por favor verificar los parámetros indicados.");
                }
            }
            catch (Exception error)
            {
                throw new Exception("Ha ocurrido un error al realizar la busqueda, contacte al administrador");
            }
        }
        #endregion
    }
}

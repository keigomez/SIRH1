using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMovimientoPuestoL
    {
        #region Variables

        private SIRHEntities contexto;        
   
        #endregion

        #region Constructor

        public CMovimientoPuestoL()
        {
            contexto = new SIRHEntities();
        }
         
        #endregion

        #region Metodos
        
        internal static CMovimientoPuestoDTO ConvertirDatosMovimientoPuestoADTO(MovimientoPuesto item)
        {
            return new CMovimientoPuestoDTO
            {
                CodOficio = item.CodOficio,
                FecMovimiento = Convert.ToDateTime(item.FecMovimiento),
                FechaVencimiento = Convert.ToDateTime(item.FecVence),
                MotivoMovimiento = new CMotivoMovimientoDTO
                {
                    IdEntidad = item.MotivoMovimiento.PK_MotivoMovimiento,
                    DesMotivo = item.MotivoMovimiento.DesMotivo
                }
                //Agregar la explicación del movimiento
            };
        }

        public List<CBaseDTO> DetalleMovimiento(int codMovimiento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);

                var movimiento = intermedio.DetalleMovimiento(codMovimiento);

                if (movimiento.Codigo > 0)
                {
                    var datoMovimiento = ConvertirDatosMovimientoPuestoADTO((MovimientoPuesto)movimiento.Contenido);
                    respuesta.Add(datoMovimiento);

                    var datosPuesto = CPuestoL.ConstruirPuesto(((MovimientoPuesto)movimiento.Contenido).Puesto, new CPuestoDTO());
                    respuesta.Add(datosPuesto);

                    var detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((MovimientoPuesto)movimiento.Contenido).Puesto.DetallePuesto.FirstOrDefault());
                    respuesta.Add(detallePuesto);

                    var nombramiento = ((MovimientoPuesto)movimiento.Contenido).Puesto.Nombramiento.FirstOrDefault();

                    var datoFuncionario = CFuncionarioL.FuncionarioGeneral(((Funcionario)new CNombramientoD(contexto).FuncionarioEnNombramiento(nombramiento.PK_Nombramiento).Contenido));
                    respuesta.Add(datoFuncionario);

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)movimiento.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });

                return respuesta;
            }
        }

        public List<List<CBaseDTO>> HistorialMovimientos(string codPuesto)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CMovimientoPuestoD resultado = new CMovimientoPuestoD(contexto);

                var datosHistorial = resultado.HistorialMovimientos(codPuesto);

                if (datosHistorial.Codigo > 0)
                {
                    List<CBaseDTO> itemHistorial = new List<CBaseDTO>();

                    var historial = ((List<MovimientoPuesto>)datosHistorial.Contenido);

                    var datosPuesto = CPuestoL.PuestoGeneral(((MovimientoPuesto)historial.FirstOrDefault()).Puesto);
                    itemHistorial.Add(datosPuesto);

                    foreach (var item in historial)
                    {
                        var datoMovimiento = ConvertirDatosMovimientoPuestoADTO((MovimientoPuesto)item);
                        itemHistorial.Add(datoMovimiento);

                        respuesta.Add(itemHistorial);
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datosHistorial.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<List<CBaseDTO>>();

                respuesta.Add(new List<CBaseDTO> 
                { 
                    new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    }
                });

                return respuesta;
            }
        }

        //Busca un movimiento de puesto, por medio de un numero de oficio especifico
        //Se insertó en ICPuesto y CPuestoService el 27/01/2017
        public CMovimientoPuestoDTO RetornarMovimientoPuestoEspecificoOficio(string CodOficio)
        {
            CMovimientoPuestoDTO resultado = new CMovimientoPuestoDTO();       
            //En esta variable vamos a almacenar los datos que provienen de la BD
            MovimientoPuesto temp = new MovimientoPuesto();

            //Esta variable, va a servir de intermedio entre la lógica y la BD, que es la clase almacenada en SIRH.Datos
            CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);
            temp = intermedio.RetornarMovimientoPuestoEspecificoOficio(CodOficio);

            //Devuelve un objeto con los datos que vienen desde la base de datos, en un objeto de la misma clase donde se esta trabajando
            //this.NombreJornada = temp.DesTipoJornada;
            resultado.IdEntidad = temp.PK_MovimientoPuesto;
            resultado.Puesto = new CPuestoDTO { IdEntidad = temp.Puesto.PK_Puesto };
            resultado.MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = temp.MotivoMovimiento.PK_MotivoMovimiento };
            resultado.CodOficio = temp.CodOficio;
            resultado.FecMovimiento = Convert.ToDateTime(temp.FecMovimiento);

            return resultado;
        }

        //Se insertó en ICPuesto y CPuestoService el 27/01/2017
        public CMovimientoPuestoDTO RetornarMovimientoPuestoEspecifico(string numeroPuesto)
        {
            CMovimientoPuestoDTO resultado = new CMovimientoPuestoDTO();  
            //En esta variable vamos a almacenar los datos que provienen de la BD
            MovimientoPuesto temp = new MovimientoPuesto();

            //Esta variable, va a servir de intermedio entre la lógica y la BD, que es la clase almacenada en SIRH.Datos
            CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);
            temp = intermedio.RetornarMovimientoPuestoEspecifico(numeroPuesto);

            //Devuelve un objeto con los datos que vienen desde la base de datos, en un objeto de la misma clase donde se esta trabajando
            //this.NombreJornada = temp.DesTipoJornada;
            resultado.IdEntidad = temp.PK_MovimientoPuesto;
            resultado.Puesto = new CPuestoDTO { IdEntidad = temp.Puesto.PK_Puesto };
            resultado.MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = temp.MotivoMovimiento.PK_MotivoMovimiento };
            resultado.CodOficio = temp.CodOficio;
            resultado.FecMovimiento = Convert.ToDateTime(temp.FecMovimiento);
            
            return resultado;
        }

        //Se insertó en ICPuesto y CPuestoService (Deivert)
        public CBaseDTO GuardarMovimientoPuesto(CMovimientoPuestoDTO MovimientoPuesto)
        {
            CBaseDTO resultado = new CBaseDTO();
            //En esta variable vamos a almacenar los datos que provienen de la BD
            MovimientoPuesto temp = new MovimientoPuesto();

            //Esta variable, va a servir de intermedio entre la lógica y la BD, que es la clase almacenada en SIRH.Datos
            CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);


            //Devuelve un objeto con los datos que vienen desde la base de datos, en un objeto de la misma clase donde se esta trabajando

            temp.CodOficio = MovimientoPuesto.CodOficio;
            temp.FecMovimiento = MovimientoPuesto.FecMovimiento;
            if (MovimientoPuesto.FechaVencimiento != null && MovimientoPuesto.FechaVencimiento.Year != 1)
            { 
                temp.FecVence = MovimientoPuesto.FechaVencimiento;
            }
            temp.ObsMovimientoPuesto = MovimientoPuesto.Explicacion;
            //Agregar aquí la explicacion del movimiento
            
            temp.MotivoMovimiento = new MotivoMovimiento { PK_MotivoMovimiento = MovimientoPuesto.MotivoMovimiento.IdEntidad };

            temp.Puesto = new Puesto { CodPuesto = MovimientoPuesto.Puesto.CodPuesto };
            temp.FK_EstadoMovimientoPuesto = 1;
            
            int respuesta = intermedio.GuardarMovimientoPuesto(temp);

            if (respuesta > 0)
            {
                resultado.Mensaje = "El movimiento del puesto N° " + MovimientoPuesto.Puesto.CodPuesto + " se ha almacenado de forma correcta";
                //aquí hay que insertar la acción de personal
            }
            else
            {
                resultado.Mensaje = "Ocurrió un error mientras los cambios eran almacenados, " +                
                                    "revise la información enviada o contacte al administrador del sistema";
            }

            return resultado;
        }
        
        //06/01/17...REVISAR CON DEIVERT.
        //Se insertó en ICPuestoService y CPuestoService el 25/01/2017
        public CBaseDTO InsertarMovimientoPuesto(CMovimientoPuestoDTO movimiento)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);
                CMotivoMovimientoD intermedioMotivo = new CMotivoMovimientoD(contexto);                
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CEstadoMovimientoPuestoD intermedioEstado = new CEstadoMovimientoPuestoD(contexto);

                if (movimiento.Explicacion.StartsWith("Vacante por"))
                {
                    movimiento.Explicacion = movimiento.Explicacion + contexto.MotivoMovimiento.FirstOrDefault(M => M.PK_MotivoMovimiento == movimiento.MotivoMovimiento.IdEntidad).DesMotivo;
                }

                MovimientoPuesto datos = new MovimientoPuesto
                {
                    CodOficio = movimiento.CodOficio,
                    FecMovimiento = Convert.ToDateTime(movimiento.FecMovimiento),
                    MotivoMovimiento = contexto.MotivoMovimiento.FirstOrDefault(M => M.PK_MotivoMovimiento == movimiento.MotivoMovimiento.IdEntidad),
                    //intermedioMotivo.CargarMotivoMovimientoPorPuesto(movimiento.Puesto.CodPuesto),
                    Puesto = intermedioPuesto.DescargarPuestoCodigo(movimiento.Puesto.CodPuesto),
                    EstadoMovimientoPuesto = contexto.EstadoMovimientoPuesto.Where(E => E.PK_EstadoMovimientoPuesto == 1).FirstOrDefault(),
                    
                    ObsMovimientoPuesto = movimiento.Explicacion
                };

                if (movimiento.FechaVencimiento.Year != 1)
                {
                    datos.FecVence = Convert.ToDateTime(movimiento.FechaVencimiento);
                }

                intermedio.InsertarMovimientoPuesto(movimiento.Puesto.CodPuesto,datos);

                return respuesta;
            }
            catch (Exception error)
            {
                //en respuesta del catch, se digita el error 
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        public List<CMovimientoPuestoDTO> BuscarMovimientoPuestosFiltros(string codPuesto, string numCedula, int motivoM, DateTime? fechaHasta, DateTime? fechaDesde)
        {
            try
            {
                CMovimientoPuestoD intermedio = new CMovimientoPuestoD(contexto);
                List<CMovimientoPuestoDTO> movimientos = new List<CMovimientoPuestoDTO>();

                List<MovimientoPuesto> result = intermedio.BuscarMovimientoPuestosFiltros(codPuesto, numCedula, motivoM, fechaHasta, fechaDesde);

                foreach (var item in result)
                {
                    var motivoMovimiento = VerificarMotivo(item.MotivoMovimiento.PK_MotivoMovimiento);
                    var fechaVerificacion = Convert.ToDateTime(item.FecMovimiento).AddDays(-1);
                    var nombramientoMovimiento = contexto.AccionPersonal.FirstOrDefault(A => (A.FecRige >= fechaVerificacion && A.FecRige <= item.FecMovimiento)
                                                                            && A.TipoAccionPersonal.CodMotivoMovimientoPuesto != 0
                                                                            && A.TipoAccionPersonal.CodMotivoMovimientoPuesto == motivoMovimiento
                                                                            && A.Nombramiento.Puesto.PK_Puesto == item.FK_Puesto);

                    var funcionarioMovimiento = new Funcionario();
                    if (nombramientoMovimiento != null)
                    {
                        funcionarioMovimiento = nombramientoMovimiento.Nombramiento.Funcionario;
                    }
                    else
                    {
                        continue;
                    }

                    CMovimientoPuestoDTO movimientoP = new CMovimientoPuestoDTO
                    {
                        IdEntidad = item.PK_MovimientoPuesto,
                        FecMovimiento = Convert.ToDateTime(item.FecMovimiento),
                        FechaVencimiento = Convert.ToDateTime(item.FecVence),
                        MotivoMovimiento = new CMotivoMovimientoDTO
                        {
                            DesMotivo = item.MotivoMovimiento.DesMotivo,
                            IdEntidad = item.MotivoMovimiento.PK_MotivoMovimiento
                        },
                        Mensaje = funcionarioMovimiento != null ? funcionarioMovimiento.IdCedulaFuncionario + "-" + funcionarioMovimiento.NomFuncionario + " " + funcionarioMovimiento.NomPrimerApellido + " " + funcionarioMovimiento.NomSegundoApellido : " - "

                    };
                    if (item.Puesto != null)
                    {
                        movimientoP.Puesto = new CPuestoDTO
                        {
                            CodPuesto = item.Puesto.CodPuesto,
                            IdEntidad = item.Puesto.PK_Puesto,
                            EstadoPuesto = new CEstadoPuestoDTO
                            {
                                IdEntidad = item.Puesto.EstadoPuesto.PK_EstadoPuesto,
                                DesEstadoPuesto = item.Puesto.EstadoPuesto.DesEstadoPuesto
                            },
                            UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                            {
                                Seccion = new CSeccionDTO
                                {
                                    NomSeccion = item.Puesto.UbicacionAdministrativa.Seccion.NomSeccion
                                },
                                Presupuesto = new CPresupuestoDTO
                                {
                                    CodigoPresupuesto = item.Puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto
                                }
                            }
                        };
                        if (item.Puesto.DetallePuesto != null)
                        {
                            movimientoP.Puesto.DetallePuesto = new CDetallePuestoDTO();

                            if (item.Puesto.DetallePuesto.ElementAt(0).Clase != null)
                            {
                                movimientoP.Puesto.DetallePuesto.Clase = new CClaseDTO
                                {
                                    DesClase = item.Puesto.DetallePuesto.ElementAt(0).Clase.DesClase,
                                    IdEntidad = item.Puesto.DetallePuesto.ElementAt(0).Clase.PK_Clase
                                };
                            }
                            if (item.Puesto.DetallePuesto.ElementAt(0).Especialidad != null)
                            {
                                movimientoP.Puesto.DetallePuesto.Especialidad = new CEspecialidadDTO
                                {
                                    DesEspecialidad = item.Puesto.DetallePuesto.ElementAt(0).Especialidad.DesEspecialidad,
                                    IdEntidad = item.Puesto.DetallePuesto.ElementAt(0).Especialidad.PK_Especialidad
                                };
                            }
                            if (item.Puesto.DetallePuesto.ElementAt(0).SubEspecialidad != null)
                            {
                                movimientoP.Puesto.DetallePuesto.SubEspecialidad = new CSubEspecialidadDTO
                                {
                                    DesSubEspecialidad = item.Puesto.DetallePuesto.ElementAt(0).SubEspecialidad.DesSubEspecialidad,
                                    IdEntidad = item.Puesto.DetallePuesto.ElementAt(0).SubEspecialidad.PK_SubEspecialidad
                                };
                            }
                        }
                    }

                    if (motivoM == -5)
                    {
                        if (VacanteyNombramiento(Convert.ToInt32(item.FK_Puesto), Convert.ToDateTime(item.FecMovimiento), Convert.ToDateTime(fechaHasta), item.PK_MovimientoPuesto))
                        {
                            movimientos.Add(movimientoP);
                        }
                    }
                    else
                    {
                        movimientos.Add(movimientoP);
                    }
                }
                return movimientos;
            }
            catch (Exception error)
            {
                throw new Exception("Ha ocurrido un error al realizar la busqueda, contacte al administrador");
            }
        }

        private bool VacanteyNombramiento(int codigoPuesto, DateTime desde, DateTime hasta, int movimiento)
        {
            desde = desde.AddDays(-1);
            var listaMovimientos = new List<int?> { 2, 3, 4, 5, 6, 23, 36, 37, 38, 41, 42, 43, 44, 53, 54, 55, 56, 57, 58, 63, 64 };
            if (codigoPuesto == 1243)
            {
                string a = "";
            }
            var existePuesto = contexto.MovimientoPuesto.Where(Q => Q.FK_Puesto == codigoPuesto && (Q.FecMovimiento >= desde && Q.FecMovimiento <= hasta) && listaMovimientos.Contains(Q.FK_MotivoMovimiento) && Q.PK_MovimientoPuesto != movimiento).Count();
            if (existePuesto > 0)
            {
                return false;
            }
            return true;
        }

        private int VerificarMotivo(int motivo)
        {
            switch (motivo)
            {
                case 10:
                case 19:
                    return 11;
                case 15:
                case 16:
                case 9:
                case 13:
                case 14:
                case 20:
                case 51:
                    return 15;
                case 21:
                case 66:
                    return 21;
                case 17:
                case 65:
                    return 17;
                case 5:
                case 38:
                case 43:
                case 44:
                case 46:
                case 49:
                case 50:
                case 53:
                    return 5;
                case 6:
                case 37:
                case 41:
                case 42:
                case 45:
                case 47:
                case 48:
                case 54:
                    return 6;
                case 52:
                    return 35;
                default:
                    return motivo;
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DatosMarcasReloj;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Logica
{
    public class CMarcacionesL
    {
        # region Variables

        MasterTASEntities contexto;

        #endregion

        #region Constructor

        public CMarcacionesL()
        {
            contexto = new MasterTASEntities();
        }

        #endregion

        #region Métodos

        internal static CMarcacionesDTO ConstruirMarcacion(Marcaciones item)
        {
            return new CMarcacionesDTO
            {
                DispositivoMarcas = item.IdDispositivo,
                CodigoEmpleado =  item.CodigoEmpleado,
                FechaHoraMarca = new DateTime(item.Fecha.Year, item.Fecha.Month, item.Fecha.Day, item.Hora.Hour, item.Hora.Minute, item.Hora.Second),
                ProcesadoMarcas = item.Procesado
            };
        }

        /// <summary>
        /// Cambia la cedula de SIRH a EmpresaDB
        /// </summary>
        /// <param name="cedula">cedula a cambiar</param>
        /// <returns>cedula version EmpresaDB</returns>
        private string ObtenerCedulaEmpresaDB(string cedula){
            if(cedula != null)
            {
                if (cedula[1] == '0' && cedula[0] == '0')
                {
                    cedula = cedula.Remove(0, 2);
                    cedula = cedula.Insert(1, "0");
                }
                else
                {
                    if (cedula[0] == '0')
                    {
                        cedula = cedula.Remove(0, 1);
                    }
                }
                return cedula;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Arregla la hora de las fechas para que pueda tomar todas las marcaciones en cuenta
        /// </summary>
        /// <param name="fechas">Las fechas que se van a arreglar</param>
        private void ArreglarFechas(List<DateTime> fechas)
        {
            fechas[0] = new DateTime(fechas[0].Year, fechas[0].Month, fechas[0].Day, 0, 0, 0);
            if (fechas.Count == 1)
            {
                fechas.Add(new DateTime(fechas[0].Year, fechas[0].Month, fechas[0].Day, 23, 59, 59));
            }
            else
            {
                fechas[1] = new DateTime(fechas[1].Year, fechas[1].Month, fechas[1].Day, 23, 59, 59);
            }
        }

        /// <summary>
        /// Obtener la jornada de un funcionario segun su cedula
        /// </summary>
        /// <param name="cedula">cedula del funcionario</param>
        /// <returns>CTipoJornadaDTO o CErrorDTO</returns>
        private object ObtenertipoJornada(string cedula) {
            try
            {
                object respuesta = null;
                using (var context = new SIRHEntities())
                {
                    CTipoJornadaD intermedioJornada = new CTipoJornadaD(context);
                    var datosJornada = intermedioJornada.CargarTipoJornadaPorFuncionarioDTO(cedula);
                    if (datosJornada != null) {
                        if (datosJornada.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            respuesta = CTipoJornadaL.TipoJornadaGeneral((TipoJornada)datosJornada.Contenido);
                        }
                        else
                        {
                            respuesta = datosJornada.Contenido;
                        }
                    }
                   
                    return respuesta;
                }
            }
            catch (Exception error) {
                return new CErrorDTO{
                    MensajeError = error.Message
                };        
            }
        }

        /// <summary>
        /// Obtine todos los datos del encabezado de los reportes
        /// </summary>
        /// <param name="funcionario">El funcionario del que se quiere los datos</param>
        /// <returns>Una lista de la siguiente forma [CfuncionarioDTO?,CDetalleContratacionDTO?,CTipoJornadaDTO?,CUbicacionAdministrativaDTO] (? = Puede ser CErrorDTO)</returns>
        private List<CBaseDTO> ObtenerDatosEncabezadoReportes(CFuncionarioDTO funcionario){
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>(); //[CfuncionarioDTO?,CDetalleContratacionDTO?,CTipoJornadaDTO?,CUbicacionAdministrativaDTO]
                using (var context = new SIRHEntities())
                {
                    var intermedioFuncionario = new CFuncionarioD(new SIRHEntities());
                    CPuestoL intermedioPuesto = new CPuestoL();
                    var funcionarioDatos = intermedioFuncionario.BuscarFuncionarioCedulaBase(funcionario.Cedula);
                    if(funcionarioDatos != null)
                    {
                        if (funcionarioDatos.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            var fuDB = (Funcionario)funcionarioDatos.Contenido;
                            var fuDTO = CFuncionarioL.FuncionarioGeneral(fuDB);
                            var DetCont = new CDetalleContratacionDTO { EstadoMarcacion = (fuDB.DetalleContratacion == null || fuDB.DetalleContratacion.Count == 0) ? null : fuDB.DetalleContratacion.FirstOrDefault().MarcaAsistencia };
                            respuesta.Add(fuDTO);
                            respuesta.Add(DetCont);
                            var tipoJornada = ObtenertipoJornada(funcionario.Cedula);
                            if(tipoJornada != null)
                            {
                                if (tipoJornada.GetType() != typeof(CErrorDTO)) {
                                    respuesta.Add((CBaseDTO)tipoJornada);
                                }
                                else
                                {
                                    respuesta.Add(new CTipoJornadaDTO());
                                }
                            }
                            else
                            {
                                respuesta.Add(new CTipoJornadaDTO());
                            }
                                                    
                            var puestoDatos = intermedioPuesto.DetallePuestoPorCedula(funcionario.Cedula).FirstOrDefault();

                            if (puestoDatos != null)
                            {
                                if (puestoDatos.GetType() != typeof(CErrorDTO))
                                {
                                    respuesta.Add( ((CPuestoDTO)puestoDatos).UbicacionAdministrativa);

                                }
                                else
                                {
                                    respuesta.Add(new CUbicacionAdministrativaDTO());
                                }
                            }
                            else
                            {
                                respuesta.Add(new CUbicacionAdministrativaDTO());
                            }

                        }
                        else respuesta.Add((CErrorDTO)funcionarioDatos.Contenido);
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error a la hora de Buscar el funcionario por la cedula, favor contactar al personal autorizado.");
                    }
                    
                }
                return respuesta;
            }
            catch (Exception error)
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO {
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> ReporteMarcacionesPorDia(List<DateTime> fechas, CFuncionarioDTO funcionario){
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { ObtenerDatosEncabezadoReportes(funcionario) };
                if (respuesta.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    CMarcacionesD intermedio = new CMarcacionesD(contexto);
                    ArreglarFechas(fechas);
                    var empl = new Empleado { CodigoEmpleado = ObtenerCedulaEmpresaDB(funcionario.Cedula) };
                    var datos = intermedio.ReporteMarcacionesPorDia(fechas[0], fechas[1], empl);
                    if(datos != null)
                    {
                        if (respuesta.Count > 1)
                            ((CFuncionarioDTO)respuesta.First().First()).Mensaje = empl.CodigoAcceso;
                        var aux = new List<CBaseDTO>();
                        if (datos.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            DateTime fechaAux = new DateTime(1, 1, 1, 0, 0, 0);
                            foreach (var item in (List<Marcaciones>)datos.Contenido)
                            {
                                if (fechaAux.Date != item.Fecha.Date)
                                {
                                    if (aux.Count > 0)
                                    {
                                        respuesta.Add(aux);
                                        aux = new List<CBaseDTO>();
                                    }
                                    fechaAux = item.Fecha;
                                }
                                aux.Add(CMarcacionesL.ConstruirMarcacion(item));
                            }
                            respuesta.Add(aux);
                        }
                        else
                        {
                            aux.Add((CErrorDTO)datos.Contenido);
                            respuesta.Add(aux);
                        }
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error realizando el reporte de marcaciones por día, favor contactar al personal autorizado.");
                    }
                    
                }
                
                return respuesta;
            }
            catch (Exception error)
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { new List<CBaseDTO>() };
                respuesta.FirstOrDefault().Add(new CErrorDTO {
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> ReporteConsolidadoPorFuncionario(List<DateTime> fechas,CFuncionarioDTO funcionario){
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { ObtenerDatosEncabezadoReportes(funcionario) };
                CMarcacionesD intermedio = new CMarcacionesD(contexto);

                if (respuesta.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    ArreglarFechas(fechas);
                    var empl = new Empleado { CodigoEmpleado = ObtenerCedulaEmpresaDB(funcionario.Cedula) };
                    var datos = intermedio.ReporteMarcacionesPorDia(fechas[0], fechas[1], empl);
                    if(datos != null)
                    {
                        ((CFuncionarioDTO)respuesta.First().First()).Mensaje = empl.CodigoAcceso;
                        var aux = new List<CBaseDTO>();

                        if (datos.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            var listDatos = (List<Marcaciones>)datos.Contenido;
                            DateTime fechaAux = new DateTime(1, 1, 1, 0, 0, 0);
                            int ultimoInsertado = -1;
                            for (var i = 0; i < listDatos.Count; i++)
                            {
                                if (fechaAux.Date != listDatos[i].Fecha.Date)
                                {
                                    if (aux.Count == 1)
                                    {
                                        if (ultimoInsertado != i - 1)
                                            respuesta.Last().Add(CMarcacionesL.ConstruirMarcacion(listDatos[i - 1]));
                                        aux = new List<CBaseDTO>();
                                    }
                                    aux.Add(CMarcacionesL.ConstruirMarcacion(listDatos[i]));
                                    respuesta.Add(aux);
                                    ultimoInsertado = i;
                                    fechaAux = listDatos[i].Fecha;
                                }
                            }
                            if (ultimoInsertado != listDatos.Count - 1)
                            {
                                respuesta.Last().Add(CMarcacionesL.ConstruirMarcacion(listDatos.Last()));
                            }
                        }
                        else
                        {
                            aux.Add((CErrorDTO)datos.Contenido);
                            respuesta.Add(aux);
                        }
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error realizando el reporte de marcaciones por día consolidado por funcionario, favor contactar al personal autorizado.");
                    }
                    
                }
                
                return respuesta;
            }
            catch (Exception error)
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { new List<CBaseDTO>() };
                respuesta.FirstOrDefault().Add(new CErrorDTO
                {
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> ReporteConsolidadoPorDepartamento(CDepartamentoDTO departamento, List<DateTime> fechas){
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
                using (var context = new SIRHEntities())
                {
                    CMarcacionesD intermedio = new CMarcacionesD(contexto);
                    var funcionarios = new CFuncionarioD(context).ObtenerFuncionariosPorDepartamento(departamento);
                    if (funcionarios != null) {
                        if (funcionarios.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            foreach (var item in (List<Funcionario>)funcionarios.Contenido)
                            {
                                respuesta.Add(new List<CBaseDTO>());
                                var datos = this.ReporteConsolidadoPorFuncionario(fechas, new CFuncionarioDTO { Cedula = item.IdCedulaFuncionario });
                                respuesta.Last().AddRange(datos[0]);
                                respuesta.AddRange(datos.Skip(1));
                            }
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO>());
                            respuesta.Last().Add((CErrorDTO)funcionarios.Contenido);
                        }
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error al obtener los funcionarios consolidados por departamento, favor contactar al personal autorizado.");
                    }
                    
                }
            }
            catch (Exception error)
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { new List<CBaseDTO>() };
                respuesta.FirstOrDefault().Add(new CErrorDTO
                {
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> ReporteFuncionariosPorDepartamento(CDepartamentoDTO departamento){
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
                using (var context = new SIRHEntities())
                {
                    CMarcacionesD intermedio = new CMarcacionesD(contexto);
                    var funcionarios = new CFuncionarioD(context).ObtenerFuncionariosPorDepartamento(departamento);
                    if (funcionarios != null) {
                        if (funcionarios.Contenido.GetType() != typeof(CErrorDTO))
                        {
                            foreach (var item in (List<Funcionario>)funcionarios.Contenido)
                            {
                                respuesta.Add(new List<CBaseDTO>());
                                respuesta.Last().AddRange(ObtenerDatosEncabezadoReportes(new CFuncionarioDTO { Cedula = item.IdCedulaFuncionario }));
                                var empl = new Empleado { CodigoEmpleado = this.ObtenerCedulaEmpresaDB(item.IdCedulaFuncionario) };
                                intermedio.ObtenerCodigoAcceso(empl);
                                respuesta.Last()[0].Mensaje = empl.CodigoAcceso;
                            }
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO>());
                            respuesta.Last().Add((CErrorDTO)funcionarios.Contenido);
                        }
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error al obtener los funcionarios por departamento, favor comunicarse con el personal autorizado.");
                    }
                    
                }
            }
            catch (Exception error)
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>() { new List<CBaseDTO>() };
                respuesta.FirstOrDefault().Add(new CErrorDTO
                {
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        #endregion
    }
}

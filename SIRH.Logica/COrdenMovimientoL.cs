using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class COrdenMovimientoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public COrdenMovimientoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static COrdenMovimientoDTO ConvertirOrdenADTO(OrdenMovimiento item)
        {
            return new COrdenMovimientoDTO
            {
                IdEntidad = item.PK_OrdenMovimiento,
                NumOrden = item.NumOrden,
                FuncionarioOrden = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario),
                Estado = ConvertirEstadoADTO(item.EstadoOrdenMovimiento),
                //Pedimento = CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(item.PedimentoPuesto),
                CuentaCliente = item.CuentaCliente,
                FecRige = Convert.ToDateTime(item.FecRige),
                FecVence = DateTime.Compare(Convert.ToDateTime(item.FecVence), Convert.ToDateTime("1/1/0001")) != 0 ? item.FecVence : null,
                DesObservaciones  = item.DesObservaciones,
                DesEstadoObservaciones = item.DesObservacionesEstado,
                DesPartidaPresupuestaria = item.DesPartidaPresupuestaria,
                TipoMovimiento = CMotivoMovimientoL.ConvertirMotivoMovimientoADTO(item.MotivoMovimiento),
                MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = Convert.ToInt32(item.IdMotivoMovimiento) },
                FuncionarioSustituido = new CFuncionarioDTO { IdEntidad = item.IdFuncionarioSustituido },
                FuncionarioResponsable = new CFuncionarioDTO { IdEntidad = item.IdFuncionarioResponsable },
                FuncionarioRevision = new CFuncionarioDTO { IdEntidad = item.IdFuncionarioRevision},
                FuncionarioJefatura = new CFuncionarioDTO { IdEntidad = item.IdFuncionarioJefatura }
            };
        }

        internal static OrdenMovimiento ConvertirDTOaOrden(COrdenMovimientoDTO orden)
        {
            return new OrdenMovimiento
            {
                PK_OrdenMovimiento = orden.IdEntidad,
                NumOrden = orden.NumOrden != null ? orden.NumOrden : "",
                FecRige = orden.FecRige,
                FecVence = orden.FecVence,
                FK_Funcionario = orden.FuncionarioOrden.IdEntidad,
                CuentaCliente = orden.CuentaCliente,
                FK_Pedimento = orden.Pedimento.IdEntidad > 0 ? orden.Pedimento.IdEntidad : (int?)null,
                DesObservaciones = orden.DesObservaciones,
                DesObservacionesEstado = orden.DesEstadoObservaciones,
                DesPartidaPresupuestaria = orden.DesPartidaPresupuestaria,
                FK_DetallePuesto = orden.DetallePuesto.IdEntidad,
                FK_TipoMovimiento = orden.TipoMovimiento.IdEntidad,
                IdMotivoMovimiento = orden.MotivoMovimiento.IdEntidad,
                FK_Estado = orden.Estado.IdEntidad,
                IdFuncionarioSustituido = orden.FuncionarioSustituido.IdEntidad,
                IdFuncionarioResponsable = orden.FuncionarioResponsable.IdEntidad,
                IdFuncionarioRevision = orden.FuncionarioRevision.IdEntidad,
                IdFuncionarioJefatura = orden.FuncionarioJefatura.IdEntidad
            };
        }

        internal static COrdenMovimientoDeclaracionDTO ConvertirOrdenDeclaracionADTO(OrdenMovimientoDeclaracion item)
        {
            return new COrdenMovimientoDeclaracionDTO
            {
               IdEntidad = item.PK_Declaracion,
               FechaCertificacion = item.FecDeclaracion,
               Academica= item.DesCondicionAcademica,
               Capacitacion = item.DesCapacitacion,
               Colegiaturas = item.DesColegiatura,
               Experiencia = item.DesExperiencia,
               Licencias = item.DesLicencia
            };
        }

        internal static CEstadoOrdenDTO ConvertirEstadoADTO(EstadoOrdenMovimiento item)
        {
            return new CEstadoOrdenDTO
            {
                IdEntidad = item.PK_Estado,
                DesEstado = item.DesEstado
            };
        }

        public List<CBaseDTO> DatosOrdenMovimiento(string cedula, string codpuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                string nombre, cedulaf, datonombramiento;

                var funcionario = intermedioFuncionario.BuscarFuncionarioCedula(cedula);
                var puesto = intermedioPuesto.DescargarPuestoCompleto(codpuesto);

                if (funcionario != null)
                {
                    //[0] CFuncionarioDTO
                    respuesta.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario));
                    cedulaf = funcionario.IdCedulaFuncionario;
                    nombre = funcionario.NomPrimerApellido.TrimEnd() + " " + funcionario.NomSegundoApellido.TrimEnd() + " " + funcionario.NomFuncionario.TrimEnd();
                }
                else
                {
                    ////[0] CFuncionarioDTO
                    //respuesta.Add(new CFuncionarioDTO { Cedula = "0", Nombre = "", PrimerApellido = "", SegundoApellido = "" });
                    throw new Exception("No existe ningún funcionario ni oferente con ese número de cédula. Favor registrar la información primero antes de hacer la Orden de Movimiento");
                }

                if (puesto.Codigo > 0)
                {
                    var datosPuesto = (Puesto)puesto.Contenido;
                    var estadopuesto = contexto.EstadoPuesto.Where(E => E.PK_EstadoPuesto == datosPuesto.FK_EstadoPuesto).FirstOrDefault();
                    var nombramiento = contexto.Nombramiento.Where(N => N.FK_Puesto == datosPuesto.PK_Puesto && (N.FecVence == null || N.FecVence >= DateTime.Now)).OrderByDescending(O => O.FecRige).FirstOrDefault();
                    DetallePuesto datoDetalle = datosPuesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).OrderBy(O => O.FecRige).FirstOrDefault();

                    if (nombramiento != null)
                    {
                        var estadonombramiento = contexto.EstadoNombramiento.Where(E => E.PK_EstadoNombramiento == nombramiento.FK_EstadoNombramiento).FirstOrDefault();
                        var funcionarionombramiento = contexto.Funcionario.Where(Q => Q.PK_Funcionario == nombramiento.FK_Funcionario).FirstOrDefault();
                        datonombramiento = "Este puesto tiene un nombramiento activo de tipo: " + estadonombramiento.DesEstado + ". Asociado al funcionario " + funcionarionombramiento.NomFuncionario.TrimEnd() + " " + funcionarionombramiento.NomPrimerApellido.TrimEnd() + " " + funcionarionombramiento.NomSegundoApellido.TrimEnd();
                    }
                    else
                    {
                        datonombramiento = "Este puesto no tiene ningún nombramiento activo. Su estado es: " + estadopuesto.DesEstadoPuesto;
                    }

                    if (estadopuesto.PK_EstadoPuesto == 18 || estadopuesto.PK_EstadoPuesto == 23) // ELIMINADO
                        throw new Exception("El puesto indicado se encuentra eliminado, por favor revise los datos");

                    //[1] CPuestoDTO
                    var dtoPuesto = new CPuestoDTO();
                    CPuestoL.ConstruirPuesto(datosPuesto, dtoPuesto);
                    dtoPuesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(datoDetalle);
                    respuesta.Add(dtoPuesto);

                    //[2] CNombramientoDTO
                    var dtoNombramiento = new CNombramientoDTO { IdEntidad = 0 };
                    if (nombramiento != null)
                        dtoNombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento);

                    dtoNombramiento.Mensaje = datonombramiento;
                    respuesta.Add(dtoNombramiento);
                }
                else
                {
                    throw new Exception("No se encontró el puesto indicado con el número suministrado, por favor revise los datos");
                }
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public CBaseDTO AgregarOrden(COrdenMovimientoDTO orden, COrdenMovimientoDeclaracionDTO declaracion)
        {
            try
            {
                COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);
                orden.Estado = new CEstadoOrdenDTO { IdEntidad = 1 }; //
                OrdenMovimiento itemOrden = ConvertirDTOaOrden(orden);

                OrdenMovimientoDeclaracion itemDeclaracion = new OrdenMovimientoDeclaracion
                {
                    FecDeclaracion = declaracion.FechaCertificacion,
                    DesCondicionAcademica = declaracion.Academica,
                    DesCapacitacion = declaracion.Capacitacion,
                    DesColegiatura = declaracion.Colegiaturas,
                    DesExperiencia = declaracion.Experiencia,
                    DesLicencia = declaracion.Licencias
                };

                var resultado = intermedio.InsertarOrdenMovimiento(itemOrden, itemDeclaracion);

                if (resultado.Codigo > 0)
                {
                    return resultado;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO AnularOrden(COrdenMovimientoDTO orden)
        {
            try
            {
                COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);

                OrdenMovimiento itemOrden = ConvertirDTOaOrden(orden);
                                
                itemOrden.EstadoOrdenMovimiento.PK_Estado = 3; // Anulado
                var resultado = intermedio.ActualizarOrden(itemOrden);

                if (resultado.Codigo > 0)
                {
                    return resultado;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }
        
        public CBaseDTO ActualizarOrden(COrdenMovimientoDTO orden)
        {
            try
            {
                COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);

                OrdenMovimiento itemOrden = new OrdenMovimiento
                {
                    PK_OrdenMovimiento = orden.IdEntidad,
                    IdFuncionarioRevision = orden.FuncionarioRevision.IdEntidad,
                    IdFuncionarioJefatura = orden.FuncionarioJefatura.IdEntidad
                };

                var estado = intermedio.BuscarEstado(orden.Estado.IdEntidad).Contenido;
                if (estado.GetType() == typeof(CErrorDTO))
                    return (CBaseDTO)estado;
                itemOrden.EstadoOrdenMovimiento = (EstadoOrdenMovimiento)estado;

                var resultado = intermedio.ActualizarOrden(itemOrden);

                if (resultado.Codigo > 0)
                {
                    return resultado;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> ObtenerOrden(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);
                CMotivoMovimientoD intermedioMotivo = new CMotivoMovimientoD(contexto);

                var dato = intermedio.ConsultarOrdenMovimiento(codigo);
                if (dato.Codigo != -1)
                {
                    var datosOrden =(OrdenMovimiento)dato.Contenido;
                    var dtoOrden = ConvertirOrdenADTO(datosOrden);
                    if (datosOrden.PedimentoPuesto != null)
                        dtoOrden.Pedimento = CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(datosOrden.PedimentoPuesto);
                    dtoOrden.FuncionarioSustituido = (CFuncionarioDTO)BuscarFuncionarioCodigo(dtoOrden.FuncionarioSustituido.IdEntidad);
                    dtoOrden.FuncionarioResponsable = (CFuncionarioDTO)BuscarFuncionarioCodigo(dtoOrden.FuncionarioResponsable.IdEntidad);
                    dtoOrden.FuncionarioRevision = (CFuncionarioDTO)BuscarFuncionarioCodigo(dtoOrden.FuncionarioRevision.IdEntidad);
                    dtoOrden.FuncionarioJefatura = (CFuncionarioDTO)BuscarFuncionarioCodigo(dtoOrden.FuncionarioJefatura.IdEntidad);

                    var datoMotivo = intermedioMotivo.CargarMotivoMovimientoCodigo(dtoOrden.MotivoMovimiento.IdEntidad);
                    dtoOrden.MotivoMovimiento = CMotivoMovimientoL.ConvertirMotivoMovimientoADTO(datoMotivo);
                    
                    var dtoDetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(datosOrden.DetallePuesto);
                    var dtoPuesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(datosOrden.DetallePuesto.Puesto);

                    // [0] Orden
                    respuesta.Add(dtoOrden);
                    
                    // [1] COrdenMovimientoDeclaracionDTO
                    var dtoDeclaracion = ConvertirOrdenDeclaracionADTO(datosOrden.OrdenMovimientoDeclaracion.FirstOrDefault());
                    respuesta.Add(dtoDeclaracion);

                    // [2] CPuestoDTO
                    respuesta.Add(dtoPuesto);

                    // [3] CDetallePuestoDTO
                    respuesta.Add(dtoDetallePuesto);
                }
                else
                {
                    respuesta.Add((CErrorDTO)dato.Contenido);
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

        public List<CBaseDTO> BuscarOrdenes(COrdenMovimientoDTO orden, List<DateTime> fechas)
        {
            COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<OrdenMovimiento> datosOrden = new List<OrdenMovimiento>();

            // Cedula
            if (orden != null && orden.FuncionarioOrden != null && orden.FuncionarioOrden.Cedula != null && orden.FuncionarioOrden.Cedula != "")
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, orden.FuncionarioOrden.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }


            // NumOrden
            if (orden != null && orden.NumOrden != null && orden.NumOrden != "")
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, orden.NumOrden, "NumOrden"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }

            // Puesto
            if (orden != null && orden.DetallePuesto != null && orden.DetallePuesto.Puesto != null && orden.DetallePuesto.Puesto.CodPuesto != "")
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, orden.DetallePuesto.Puesto.CodPuesto, "NumOrden"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }

            // Estado
            if (orden != null && orden.Estado != null && orden.Estado.IdEntidad > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, orden.Estado.IdEntidad, "Estado"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }

            // Tipo
            if (orden != null && orden.TipoMovimiento != null && orden.TipoMovimiento.IdEntidad > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, orden.TipoMovimiento.IdEntidad, "Tipo"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }

            // Fecha Inicio
            if (fechas!= null && fechas.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarOrdenes(datosOrden, fechas, "FechaInicio"));

                if (resultado.Codigo > 0)
                {
                    datosOrden = (List<OrdenMovimiento>)resultado.Contenido;
                }
            }

            if (datosOrden.Count > 0)
            {
                foreach (var item in datosOrden)
                {
                    var dtoOrden = ConvertirOrdenADTO(item);
                    if (item.PedimentoPuesto != null)
                        dtoOrden.Pedimento = CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(item.PedimentoPuesto);
                    dtoOrden.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(item.DetallePuesto);
                    dtoOrden.DetallePuesto.Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(item.DetallePuesto.Puesto);

                    dtoOrden.FuncionarioResponsable.Sexo = GeneroEnum.Indefinido;
                    dtoOrden.FuncionarioRevision.Sexo = GeneroEnum.Indefinido;
                    dtoOrden.FuncionarioSustituido.Sexo = GeneroEnum.Indefinido;
                    dtoOrden.FuncionarioJefatura.Sexo = GeneroEnum.Indefinido;

                    // [0] Orden
                    respuesta.Add(dtoOrden);
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        public CBaseDTO BuscarFuncionarioCodigo(int codigo)
        {
            // Funcionario
            COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);
            var entidadFunc = intermedio.BuscarFuncionarioCodigo(codigo);
            if (entidadFunc.Codigo != -1)
            {
                var datoFunc = (Funcionario)entidadFunc.Contenido;
                return CFuncionarioL.ConvertirDatosFuncionarioADTO(datoFunc);
            }
            else
                return new CFuncionarioDTO { IdEntidad = 0, Cedula = "", Nombre = "", PrimerApellido = "", SegundoApellido = "", Sexo = GeneroEnum.Indefinido };
        }


        public List<CBaseDTO> ListarOrdenEstados()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                COrdenMovimientoD intermedio = new COrdenMovimientoD(contexto);
                var resultado = intermedio.ListarOrdenEstados();

                if (resultado.Codigo > 0)
                {
                    var estados = ((List<EstadoOrdenMovimiento>)resultado.Contenido);
                    foreach (var item in estados)
                    {
                        respuesta.Add(ConvertirEstadoADTO(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
        #endregion
    }
}

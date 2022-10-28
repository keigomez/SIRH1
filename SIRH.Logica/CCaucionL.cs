using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCaucionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCaucionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        //public -> Son métodos o variables que son visibles a toda la aplicación
        //private -> Son métodos o variables que son visibiles solo dentro la propia clase que los define
        //protected -> Que no sirven para nada, pero que la función es proteger variables o metodos a nivel de la clase y compartirlas con otras de forma anónima
        //internal -> Son métodos o variables que son visibles sobre una misma capa

        internal static CCaucionDTO ConvertirDatosCaucionADto(Caucion item)
        {
            return new CCaucionDTO
            {
                IdEntidad = item.PK_Caucion,
                NumeroPoliza = item.NumPoliza,
                FechaEmision = Convert.ToDateTime(item.FecEmision),
                FechaVence = Convert.ToDateTime(item.FecVence),
                EstadoPoliza = Convert.ToInt32(item.IndEstadoPoliza),
                ObservacionesPoliza = item.ObsPoliza,
                CopiaCertificada = Convert.ToBoolean(item.IndCopiaCertificada),
                NumeroOficioEntrega = item.NumOficioEntrega
            };  
            
        }

        internal static string DefinirEstadoPoliza(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Activa";
                    break;
                case 2:
                    respuesta = "Por Activar";
                    break;
                case 3:
                    respuesta = "Anulada";
                    break;
                case 4:
                    respuesta = "Vencida";
                    break;
                default:
                    respuesta = "Indefinido";
                    break;
            }
            return respuesta;
        }

        //Se registró en ICCaucionesService y CCaucionesService
        public CBaseDTO AgregarCaucion(CFuncionarioDTO funcionario, CCaucionDTO caucion, 
                                            CEntidadSegurosDTO aseguradora, CMontoCaucionDTO montoCaucion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCaucionD intermedio = new CCaucionD(contexto);

                CEntidadSegurosD intermedioSeguros = new CEntidadSegurosD(contexto);

                CMontoCaucionD intermedioMontos = new CMontoCaucionD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                Caucion datosCaucion = new Caucion
                {
                    FecEmision = caucion.FechaEmision,
                    FecVence = caucion.FechaVence,
                    IndEstadoPoliza = caucion.EstadoPoliza,
                    NumPoliza = caucion.NumeroPoliza,
                    IndCopiaCertificada = caucion.CopiaCertificada,
                    NumOficioEntrega = caucion.NumeroOficioEntrega
                };

                var entidadSeguros = intermedioSeguros.ObtenerEntidadSeguros(aseguradora.IdEntidad);

                if (entidadSeguros.Codigo != -1)
                {
                    datosCaucion.EntidadSeguros = (EntidadSeguros)entidadSeguros.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadSeguros).Contenido;
                    throw new Exception();
                }

                var entidadMontos = intermedioMontos.ObtenerMontoCaucion(montoCaucion.IdEntidad);

                if (entidadMontos.Codigo != -1)
                {
                    datosCaucion.MontoCaucion = (MontoCaucion)entidadMontos.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)entidadMontos).Contenido;
                    throw new Exception();
                }

                respuesta = intermedio.AgregarCaucion(datosFuncionario, datosCaucion);

                if (((CRespuestaDTO)respuesta).Codigo == -1)
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

        //Se registró en ICCaucionesService y CCaucionesService (DEIVERT)
        public List<CBaseDTO> ObtenerCaucion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCaucionD intermedio = new CCaucionD(contexto);

                CEntidadSegurosD intermedioSeguros = new CEntidadSegurosD(contexto);

                CMontoCaucionD intermedioMontos = new CMontoCaucionD(contexto);

                var caucion = intermedio.ObtenerCaucion(codigo);

                if (caucion.Codigo > 0)
                {
                    var datoCaucion = ConvertirDatosCaucionADto((Caucion)caucion.Contenido);

                    datoCaucion.DetalleEstadoPoliza = DefinirEstadoPoliza(datoCaucion.EstadoPoliza);

                    respuesta.Add(datoCaucion); // [0] CaucionDTO

                    var funcionario = ((Caucion)caucion.Contenido).Nombramiento.Funcionario;

                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario)); //[1] FuncionarioDTO

                    var entidadSeguros = intermedioSeguros.ObtenerEntidadSeguros
                        (((Caucion)caucion.Contenido).EntidadSeguros.PK_EntidadSeguros);


                    respuesta.Add(CEntidadSegurosL.ConvertirEntidadSegurosADto((EntidadSeguros)entidadSeguros.Contenido)); //[2] Entidad Seguros DTO

                    var montosCaucion = intermedioMontos.ObtenerMontoCaucion
                        (((Caucion)caucion.Contenido).MontoCaucion.PK_MontoCaucion);

                    respuesta.Add(CMontoCaucionL.ConvertirMontoCaucionADTO((MontoCaucion)montosCaucion.Contenido)); //[3] MontoCaucion DTO
                }
                else
                {
                    respuesta.Add((CErrorDTO)caucion.Contenido); //[0] ErrorDTO
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO 
                {
                    Codigo = -1,
                    MensajeError = error.Message
                }); //[0] ErrorDTO
            }

            return respuesta;
        }

        //Se registró en ICCaucionesService y CCaucionesService (DEIVERT)
        public List<List<CBaseDTO>> BuscarCauciones(CFuncionarioDTO funcionario, CCaucionDTO caucion,
                                                        List<DateTime> fechasEmision,
                                                        List<DateTime> fechasVencimiento, CPuestoDTO puesto, CMontoCaucionDTO nivel)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CCaucionD intermedio = new CCaucionD(contexto);

            List<Caucion> datosCauciones = new List<Caucion>();

            if (funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, funcionario.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (caucion.NumeroPoliza != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, caucion.NumeroPoliza, "Numero"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (fechasEmision.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, fechasEmision, "FechaEmision"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (fechasVencimiento.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, fechasVencimiento, "FechaVence"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (caucion.DetalleEstadoPoliza != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, caucion.DetalleEstadoPoliza, "Estado"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (puesto.CodPuesto != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, puesto.CodPuesto, "Puesto"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (nivel.Descripcion != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCauciones(datosCauciones, nivel.Descripcion, "Nivel"));
                if (resultado.Codigo > 0)
                {
                    datosCauciones = (List<Caucion>)resultado.Contenido;
                }
            }

            if (datosCauciones.Count > 0)
            {
                foreach (var item in datosCauciones)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    var datoCaucion = ConvertirDatosCaucionADto(item);
                    datoCaucion.DetalleEstadoPoliza = DefinirEstadoPoliza(datoCaucion.EstadoPoliza);

                    CCaucionDTO tempCaucion = datoCaucion;

                    temp.Add(tempCaucion);

                    CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                    {
                        Cedula = item.Nombramiento.Funcionario.IdCedulaFuncionario,
                        Nombre = item.Nombramiento.Funcionario.NomFuncionario,
                        PrimerApellido = item.Nombramiento.Funcionario.NomPrimerApellido,
                        SegundoApellido = item.Nombramiento.Funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    };

                    temp.Add(tempFuncionario);

                    CPuestoDTO tempPuesto = new CPuestoDTO
                    {
                        CodPuesto = item.Nombramiento.Puesto.CodPuesto
                    };

                    temp.Add(tempPuesto);

                    CEntidadSegurosDTO tempEntidad = CEntidadSegurosL.ConvertirEntidadSegurosADto(item.EntidadSeguros);

                    temp.Add(tempEntidad);

                    CMontoCaucionDTO tempMonto = CMontoCaucionL.ConvertirMontoCaucionADTO(item.MontoCaucion);

                    temp.Add(tempMonto);

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

        //Se registró en ICCaucionesService y CCaucionesService (DEIVERT)
        public CBaseDTO AnularCaucion(CCaucionDTO caucion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCaucionD intermedio = new CCaucionD(contexto);

                Caucion caucionBD = new Caucion 
                {
                    PK_Caucion = caucion.IdEntidad,
                    ObsPoliza = caucion.ObservacionesPoliza
                };

                var datosCaucion = intermedio.AnularCaucion(caucionBD);

                if (datosCaucion.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = caucion.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosCaucion.Contenido);
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

        //Se registró en ICCaucionesService y CCaucionesService (DEIVERT)
        public List<List<CBaseDTO>> ActualizarVencimientoPolizas(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CCaucionD intermedio = new CCaucionD(contexto);

                var caucion = intermedio.ActualizarVencimientoPolizas(fechaVencimiento);

                if (caucion.Codigo > 0)
                {
                    
                    foreach (var item in ((List<Caucion>)caucion.Contenido))
                    {
                        List<CBaseDTO> polizas = new List<CBaseDTO>();
                        var poliza = CCaucionL.ConvertirDatosCaucionADto(item);
                        polizas.Add(poliza);
                        var funcionario = CFuncionarioL.FuncionarioGeneral(item.Nombramiento.Funcionario);
                        polizas.Add(funcionario);
                        respuesta.Add(polizas);
                    }
                }
                else
                {
                    List<CBaseDTO> polizas = new List<CBaseDTO>();
                    polizas.Add((CErrorDTO)caucion.Contenido);
                    respuesta.Add(polizas);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> polizas = new List<CBaseDTO>();
                polizas.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(polizas);
            }

            return respuesta;
        }

        //Se registró en ICCaucionesService y CCaucionesService (DEIVERT)
        public List<List<CBaseDTO>> PolizasPorVencer(DateTime fechaVencimiento)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CCaucionD intermedio = new CCaucionD(contexto);

                var caucion = intermedio.PolizasPorVencer(fechaVencimiento);

                if (caucion.Codigo > 0)
                {

                    foreach (var item in ((List<Caucion>)caucion.Contenido))
                    {
                        List<CBaseDTO> polizas = new List<CBaseDTO>();
                        var poliza = CCaucionL.ConvertirDatosCaucionADto(item);
                        polizas.Add(poliza);
                        var funcionario = CFuncionarioL.FuncionarioGeneral(item.Nombramiento.Funcionario);
                        polizas.Add(funcionario);
                        respuesta.Add(polizas);
                    }
                }
                else
                {
                    List<CBaseDTO> polizas = new List<CBaseDTO>();
                    polizas.Add((CErrorDTO)caucion.Contenido);
                    respuesta.Add(polizas);
                }
            }
            catch (Exception error)
            {
                List<CBaseDTO> polizas = new List<CBaseDTO>();
                polizas.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(polizas);
            }

            return respuesta;
        }

        #endregion
    }
}

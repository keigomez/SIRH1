using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleContratacionL
    {
        #region Variables

        SIRHEntities contexto;
        CDetalleContratacionD detalleDescarga;

        #endregion

        #region Constructor

        public CDetalleContratacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public static CDetalleContratacionDTO DetalleContratacionGeneral(DetalleContratacion item)
        {
            CDetalleContratacionDTO respuesta = new CDetalleContratacionDTO();

            respuesta.IdEntidad = item.PK_DetalleContratacion;
            if (item.FecCese != null)
            {
                respuesta.FechaCese = Convert.ToDateTime(item.FecCese);
            }
            respuesta.FechaIngreso = Convert.ToDateTime(item.FecIngreso);
            respuesta.FechaMesAumento = item.FecMesAumento;
            respuesta.NumeroAnniosServicioPublico = Convert.ToInt32(item.NumAnniosServicioPublico);
            respuesta.NumeroAnualidades = Convert.ToInt32(item.NumAnualidades);
            respuesta.EstadoMarcacion = item.MarcaAsistencia;
            respuesta.FechaVacaciones = Convert.ToDateTime(item.FecVacaciones);
            
            return respuesta;
        }

        public CBaseDTO DetalleContratacion(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                var detalle = DescargarDetalleContratacion(cedula);
                respuesta = new CDetalleContratacionDTO
                { FechaIngreso = detalle.FechaIngreso, FechaVacaciones = detalle.FechaVacaciones };
                return respuesta;
            }
            catch (Exception error)
            {
                //es una nueva lista con un solo elemento en este caso es el Error
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        public CDetalleContratacionDTO DescargarDetalleContratacion(string cedula)
        {
            CDetalleContratacionDTO resultado = new CDetalleContratacionDTO();
            detalleDescarga = new CDetalleContratacionD(contexto);
            var item = detalleDescarga.CargarDetalleContratacionCedula(cedula);
            if (item != null)
            {
                resultado.IdEntidad = item.PK_DetalleContratacion;
                resultado.FechaCese = Convert.ToDateTime(item.FecCese);
                resultado.FechaIngreso = Convert.ToDateTime(item.FecIngreso);
                resultado.FechaMesAumento = item.FecMesAumento;
                resultado.NumeroAnualidades = Convert.ToInt32(item.NumAnualidades);
                resultado.NumeroAnniosServicioPublico = Convert.ToInt32(item.NumAnniosServicioPublico);
                resultado.CodigoPolicial = Convert.ToInt32(item.CodPolicial);
                resultado.FechaVacaciones = Convert.ToDateTime(item.FecVacaciones);
            }
            return resultado;
        }

        public CDetalleContratacionDTO DescargarDetalleContratacionOriginal(string cedula)
        {
            CDetalleContratacionDTO resultado = new CDetalleContratacionDTO();
            detalleDescarga = new CDetalleContratacionD(contexto);
            var item = detalleDescarga.CargarDetalleContratacionCedula(cedula);

            if (item != null)
            {
                resultado.IdEntidad = item.PK_DetalleContratacion;
                resultado.FechaCese = Convert.ToDateTime(item.FecCese);
                resultado.FechaIngreso = Convert.ToDateTime(item.FecIngreso);
                resultado.FechaMesAumento = item.FecMesAumento;
                resultado.NumeroAnualidades = Convert.ToInt32(item.NumAnualidades);
                resultado.NumeroAnniosServicioPublico = Convert.ToInt32(item.NumAnniosServicioPublico);
                resultado.FechaVacaciones = Convert.ToDateTime(item.FecVacaciones);
            }

            return resultado;
        }

        internal static CDetalleContratacionDTO ConvertirDetalleContratacionADTO(DetalleContratacion item)
        {
            return new CDetalleContratacionDTO
            {
                FechaCese = Convert.ToDateTime(item.FecCese),
                FechaIngreso = Convert.ToDateTime(item.FecIngreso),
                FechaMesAumento = item.FecMesAumento,
                IdEntidad = item.PK_DetalleContratacion,
                CodigoPolicial = Convert.ToInt32(item.CodPolicial),
                NumeroAnniosServicioPublico = Convert.ToInt32(item.NumAnniosServicioPublico),
                NumeroAnualidades = Convert.ToInt32(item.NumAnualidades),
                FechaVacaciones = Convert.ToDateTime(item.FecVacaciones),
                FechaRegimenPolicial = Convert.ToDateTime(item.FecRegimenPolicial)                  //quitar si da problemas
            };
        }

        internal static CDetalleContratacionDTO ConvertirDetalleContratacionADTOOriginal(DetalleContratacion item)
        {
            return new CDetalleContratacionDTO 
            {
                FechaCese = Convert.ToDateTime(item.FecCese),
                FechaIngreso = Convert.ToDateTime(item.FecIngreso),
                FechaMesAumento = item.FecMesAumento,
                IdEntidad = item.PK_DetalleContratacion,
                CodigoPolicial = Convert.ToInt32(item.CodPolicial),
                NumeroAnniosServicioPublico = Convert.ToInt32(item.NumAnniosServicioPublico),
                NumeroAnualidades = Convert.ToInt32(item.NumAnualidades),
                FechaVacaciones = Convert.ToDateTime(item.FecVacaciones)
            };
        }
       
        //06/01/17...REVISAR CON DEIVERT...
        //INSERTADO EN ICFuncinarioService y CFuncionarioService el 25/01/2017
        public List<CBaseDTO>GuardarDetalleContratacion(CDetalleContratacionDTO detalle, CCuentaBancariaDTO cuenta)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDetalleContratacionD intermedioDetalle = new CDetalleContratacionD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                CCuentaBancariaD intermedioCuenta = new CCuentaBancariaD(contexto);
                CEntidadFinancieraD intermedioEntidad = new CEntidadFinancieraD(contexto);
                
                DetalleContratacion datos = new DetalleContratacion               
                {
                    //Atributos
                    FecIngreso = Convert.ToDateTime(detalle.FechaIngreso),
                    FecCese = Convert.ToDateTime(detalle.FechaCese), 
                    NumAnniosServicioPublico = detalle.NumeroAnniosServicioPublico,
                    NumAnualidades = detalle.NumeroAnualidades,
                    FecMesAumento = detalle.FechaMesAumento,
                    CodPolicial = detalle.CodigoPolicial,
                    FecVacaciones = detalle.FechaVacaciones,
                    //con ID
                    Funcionario = intermedioFuncionario.BuscarFuncionarioCedula(detalle.Funcionario.Cedula)                    
                };    
                //esta variable puede tener cualquier nombre
                //  resultado                                      variable "datos" de DetalleContratacion   
                var resultado = intermedioDetalle.GuardarDetalleContratacion(datos);
                if (resultado.Contenido.GetType() != typeof(CErrorDTO))                    
                {
                    //la variable "resultado." es el nombre que se le dé arriba en (Var resultado = etc....)
                    respuesta.Add((CDetalleContratacionDTO)resultado.Contenido);
                    //respuesta.Add(detalle);

                    CuentaBancaria datosCuenta = new CuentaBancaria
                    {
                        DetalleContratacion = intermedioDetalle.CargarDetalleContratacionPorID(datos.PK_DetalleContratacion),
                        CtaCliente = cuenta.CtaCliente,
                        EntidadFinanciera = (EntidadFinanciera)intermedioEntidad.BuscarEntidadFinanciera(int.Parse(cuenta.EntidadFinanciera.CodEntidad)).Contenido
                    };

                    var resultadoCuenta = intermedioCuenta.GuardarCuentaBancaria(datosCuenta);
                    if (resultadoCuenta.Contenido.GetType() != typeof(CErrorDTO))
                    {
                        //la variable "resultado." es el nombre que se le dé arriba en (Var resultado = etc....)
                        respuesta.Add((CCuentaBancariaDTO)resultado.Contenido);                        
                    }
                    else
                    {                        
                        respuesta.Add((CErrorDTO)resultado.Contenido);
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
                //es una nueva lista con un solo elemento en este caso es el Error
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }
        }

        public CBaseDTO GuardarDetalleContratacion(CDetalleContratacionDTO datos)
        {
            try
            {
                CDetalleContratacionD intermedio = new CDetalleContratacionD(contexto);

                DetalleContratacion datoGuardar = new DetalleContratacion
                {
                    CodPolicial = datos.CodigoPolicial,
                    FecMesAumento = datos.FechaMesAumento,
                    MarcaAsistencia = datos.EstadoMarcacion,
                    NumAnualidades = datos.NumeroAnualidades,
                    FecIngreso = datos.FechaIngreso,
                    IndUbicacionReal = datos.UbicacionReal,
                    FecRegimenPolicial = datos.FechaRegimenPolicial,
                    FecVacaciones = datos.FechaVacaciones,
                    Funcionario = contexto.Funcionario.FirstOrDefault(F => F.IdCedulaFuncionario == datos.Funcionario.Cedula)
                };

                var resultado = new CRespuestaDTO();

                if (datos.Mensaje == "Guardar")
                {
                    resultado = intermedio.GuardarDetalleContratacion(datoGuardar);
                }
                else
                {
                    resultado = intermedio.ActualizarDetalleContratacion(datoGuardar);
                }

                if (resultado.Codigo > 0)
                {
                    return new CBaseDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }
        }

        public List<CBaseDTO> CargarDetalleContratacionFuncionario(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDetalleContratacionD intermedio = new CDetalleContratacionD(contexto);

                var resultado = intermedio.CargarDetalleContratacionFuncionario(cedula);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(((DetalleContratacion)(resultado.Contenido)).Funcionario));
                    respuesta.Add(ConvertirDetalleContratacionADTO(((DetalleContratacion)(resultado.Contenido))));
                    respuesta.Add(CExpedienteL.ConvertirExpedienteADTO(((DetalleContratacion)(resultado.Contenido)).Funcionario.ExpedienteFuncionario.FirstOrDefault()));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO>
                {
                    new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    }
                };
            }
        }

        public List<List<CBaseDTO>> BuscarFuncionarioPolicial(CDetalleContratacionDTO detalle, List<DateTime> fechas)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CDetalleContratacionD intermedio = new CDetalleContratacionD(contexto);
            List<Funcionario> datos = new List<Funcionario>();

            if (detalle.Funcionario != null)
                if (detalle.Funcionario.Cedula != null)
                {
                    CRespuestaDTO resultado = intermedio.BuscarFuncionarioPolicial(datos, detalle.Funcionario.Cedula, "Cedula");
                    if (resultado.Codigo > 0)
                        datos = (List<Funcionario>)resultado.Contenido;
                }

            if (detalle.CodigoPolicial > 0)
            {
                CRespuestaDTO resultado = intermedio.BuscarFuncionarioPolicial(datos, detalle.CodigoPolicial, "CodigoPolicial");
                if (resultado.Codigo > 0)
                    datos = (List<Funcionario>)resultado.Contenido;
            }

            if (fechas.Count > 0)
            {
                CRespuestaDTO resultado = intermedio.BuscarFuncionarioPolicial(datos, fechas, "Fechas");
                if (resultado.Codigo > 0)
                {
                    datos = (List<Funcionario>)resultado.Contenido;
                }
            }




            if (datos.Count > 0)
            {

                foreach (var item in datos)
                {

                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    CFuncionarioDTO tempFun = CFuncionarioL.FuncionarioGeneral(item);
                    temp.Add(tempFun);

                    Nombramiento tempNom = item.Nombramiento.Where(N => N.FecVence > DateTime.Now || N.FecVence == null).FirstOrDefault();

                    CPuestoDTO tempPuesto = new CPuestoDTO();
                    tempPuesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(tempNom.Puesto);
                    temp.Add(tempPuesto);

                    CDetallePuestoDTO tempDetPuesto = new CDetallePuestoDTO();
                    if (tempNom.Puesto.DetallePuesto != null)
                    {
                        if (tempNom.Puesto.DetallePuesto.FirstOrDefault().Clase != null)
                        {
                            tempDetPuesto.Clase = new CClaseDTO
                            {
                                IdEntidad = tempNom.Puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                                DesClase = tempNom.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase,
                            };
                        }
                        else
                        {
                            tempDetPuesto.Clase = new CClaseDTO();
                        }
                        if (tempNom.Puesto.DetallePuesto.FirstOrDefault().Especialidad != null)
                        {
                            tempDetPuesto.Especialidad = new CEspecialidadDTO
                            {
                                IdEntidad = tempNom.Puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                                DesEspecialidad = tempNom.Puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                            };
                        }
                        else
                        {
                            tempDetPuesto.Especialidad = new CEspecialidadDTO();
                        }

                    }
                    temp.Add(tempDetPuesto);

                    CDetalleContratacionDTO tempDetContratacion = new CDetalleContratacionDTO();
                    tempDetContratacion = ConvertirDetalleContratacionADTO(item.DetalleContratacion.FirstOrDefault());
                    temp.Add(tempDetContratacion);

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

        internal static CFuncionarioDTO ConvertirDatosFuncionarioADTO(Funcionario item)
        {
            return new CFuncionarioDTO
            {
                IdEntidad = item.PK_Funcionario,
                Cedula = item.IdCedulaFuncionario,
                Nombre = item.NomFuncionario,
                PrimerApellido = item.NomPrimerApellido,
                SegundoApellido = item.NomSegundoApellido,
                Sexo = item.IndSexo != "" ? GeneroEnum.Indefinido : (GeneroEnum)Convert.ToInt32(item.IndSexo)

            };
        }

        #endregion
    }
}

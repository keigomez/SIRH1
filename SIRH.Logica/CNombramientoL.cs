using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
   public class CNombramientoL
    {
       #region Variables

        SIRHEntities contexto;
        CNombramientoD nombramientoDescarga;

        #endregion

       #region Constructor

        public CNombramientoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

       #region Metodos

        internal static CNombramientoDTO ConvertirDatosNombramientoADTO(Nombramiento nombramiento)
        {
            return new CNombramientoDTO
            {
                FecRige = Convert.ToDateTime(nombramiento.FecRige),
                FecVence = Convert.ToDateTime(nombramiento.FecVence),
                FecNombramiento = Convert.ToDateTime(nombramiento.FecNombramiento),
                IdEntidad = nombramiento.PK_Nombramiento,
                Puesto = CPuestoL.PuestoGeneral(nombramiento.Puesto),
                EstadoNombramiento = new CEstadoNombramientoDTO
                {
                    IdEntidad = nombramiento.EstadoNombramiento.PK_EstadoNombramiento,
                    DesEstado = nombramiento.EstadoNombramiento.DesEstado
                }
            };
        }
        internal static Nombramiento ConvertirDatosNombramientoADATOS(CNombramientoDTO nombramiento)
        {
            return new Nombramiento
            {
                FecRige = Convert.ToDateTime(nombramiento.FecRige),
                FecVence = Convert.ToDateTime(nombramiento.FecVence),
                FecNombramiento = Convert.ToDateTime(nombramiento.FecNombramiento),
                PK_Nombramiento = nombramiento.IdEntidad,
                Puesto = CPuestoL.ConvertirCPuestoGeneralDTOaDatos(nombramiento.Puesto),
                EstadoNombramiento = new EstadoNombramiento
                {
                    DesEstado = nombramiento.EstadoNombramiento.DesEstado
                }

            };
        }
        internal static Nombramiento ConvertirDatosNombramientoADATOSBasicos(CNombramientoDTO nombramiento)
        {
            return new Nombramiento
            {
                FecRige = Convert.ToDateTime(nombramiento.FecRige),
                FecVence = Convert.ToDateTime(nombramiento.FecVence),
                FecNombramiento = Convert.ToDateTime(nombramiento.FecNombramiento),
                PK_Nombramiento = nombramiento.IdEntidad
            };
        }
        internal static CNombramientoDTO NombramientoGeneral(Nombramiento item)
        {
            CNombramientoDTO respuesta = new CNombramientoDTO();

            respuesta.IdEntidad = item.PK_Nombramiento;
            respuesta.FecNombramiento = Convert.ToDateTime(item.FecNombramiento);
            respuesta.FecRige = Convert.ToDateTime(item.FecRige);
            if (item.FecVence != null)
            {
                respuesta.FecVence = Convert.ToDateTime(item.FecVence);
            }
            if (item.EstadoNombramiento != null)
            {
                respuesta.EstadoNombramiento = new CEstadoNombramientoDTO
                {
                    IdEntidad = item.EstadoNombramiento.PK_EstadoNombramiento,
                    DesEstado = item.EstadoNombramiento.DesEstado
                };
            }

            return respuesta;
        }

        //Esto tiene que ser una lista de listas
        public List<List<CBaseDTO>> BuscarDatosParaNombramiento(string cedula, string codPuesto)
        {
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var puesto = intermedioPuesto.DescargarPuestoCompleto(codPuesto);
                List<CBaseDTO> listaPuesto = new List<CBaseDTO>();
                var funcionario = intermedioFuncionario.BuscarFuncionarioBase(cedula);
                List<CBaseDTO> listaFuncionario = new List<CBaseDTO>();
                var nombramientoFuncionarioActual = intermedioPuesto.DescargarPerfilPuestoAccionesFuncionario(cedula);
                List<CBaseDTO> listaNombramientoFuncionario = new List<CBaseDTO>();
                var nombramientoPuestoActual = intermedioPuesto.DescargarPerfilPuestoAcciones(codPuesto);
                List<CBaseDTO> listaPuestoNombramiento = new List<CBaseDTO>();

                if (puesto.Codigo > 0)
                {
                    listaPuesto.Add(CPuestoL.ConstruirPuesto((Puesto)puesto.Contenido, new CPuestoDTO()));
                    listaPuesto.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)puesto.Contenido).DetallePuesto.FirstOrDefault()));
                }
                else
                {
                    listaPuesto.Add((CErrorDTO)puesto.Contenido);
                }

                respuesta.Add(listaPuesto);

                if (funcionario.Codigo > 0)
                {
                    listaFuncionario.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO((Funcionario)funcionario.Contenido));
                }
                else
                {
                    listaFuncionario.Add((CErrorDTO)funcionario.Contenido);
                }

                respuesta.Add(listaFuncionario);

                if (nombramientoFuncionarioActual.Codigo > 0)
                {
                    listaNombramientoFuncionario.Add(CPuestoL.ConstruirPuesto(((Puesto)nombramientoFuncionarioActual.Contenido), new CPuestoDTO()));
                    listaNombramientoFuncionario.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)nombramientoFuncionarioActual.Contenido).DetallePuesto.FirstOrDefault()));
                    listaNombramientoFuncionario.Add(CNombramientoL.ConvertirDatosNombramientoADTO(((Puesto)nombramientoFuncionarioActual.Contenido).Nombramiento.FirstOrDefault()));
                }
                else
                {
                    listaNombramientoFuncionario.Add((CErrorDTO)nombramientoFuncionarioActual.Contenido);
                }

                respuesta.Add(listaNombramientoFuncionario);

                if (nombramientoPuestoActual.Codigo > 0)
                {
                    listaPuestoNombramiento.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(((Funcionario)nombramientoPuestoActual.Contenido).Nombramiento.FirstOrDefault().Funcionario));
                    listaPuestoNombramiento.Add(CNombramientoL.ConvertirDatosNombramientoADTO(((Puesto)nombramientoPuestoActual.Contenido).Nombramiento.FirstOrDefault()));
                }
                else
                {
                    listaPuestoNombramiento.Add((CErrorDTO)nombramientoFuncionarioActual.Contenido);
                }

                respuesta.Add(listaPuestoNombramiento);

                return respuesta;
            }
            catch (Exception error)
            {
                return new List<List<CBaseDTO>> { new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } } };
            } 
        }

        private Nombramiento ConstruirNombramiento(SIRHEntities contexto, CNombramientoDTO nombramientoInicial)
        {
            CEstadoNombramientoD estado = new CEstadoNombramientoD(contexto);
            CPuestoD puesto = new CPuestoD(contexto);
            CFuncionarioD funcionario = new CFuncionarioD(contexto);

            Nombramiento respuesta = new Nombramiento
            {
                //ATRIBUTO
                FecRige = nombramientoInicial.FecRige,
                //ATRIBUTO      //dentro del paréntesis
                FecNombramiento = nombramientoInicial.FecNombramiento,
                //FK               //estado. es la variable de CEstadoNombramientoD   
                EstadoNombramiento = estado.CargarEstadoNombramientoPorID(nombramientoInicial.EstadoNombramiento.IdEntidad),
                //FK   //puesto. es la variable de CPuestoD
                Puesto = puesto.DescargarPuestoCodigo(nombramientoInicial.Puesto.CodPuesto),
                //FK        //traer la información que trae el método BuscarFuncionarioCedula y lo guarde en nombramientoInicial
                Funcionario = funcionario.BuscarFuncionarioCedula(nombramientoInicial.Funcionario.Cedula)
            };

            return respuesta;
        }

        //Se insertó ICFuncionarioService y CFuncionarioService el 27/01/2017....
        public CNombramientoDTO DescargarNombramiento(string cedula)
        {
            CNombramientoDTO res = new CNombramientoDTO();

            nombramientoDescarga = new CNombramientoD(contexto);
            var item = nombramientoDescarga.CargarNombramientoCedula(cedula);
            if (item != null)
            {
                res.IdEntidad = item.PK_Nombramiento;
                res.EstadoNombramiento = new CEstadoNombramientoDTO
                {
                    IdEntidad = item.EstadoNombramiento.PK_EstadoNombramiento,
                    DesEstado = item.EstadoNombramiento.DesEstado
                };
                res.FecRige = Convert.ToDateTime(item.FecRige);
                res.FecVence = Convert.ToDateTime(item.FecVence);
            }

            return res;
        }


        public List<CBaseDTO> DescargarNombramientoActualCedula(string cedula)
        {
            try
            {
                List<CBaseDTO> res = new List<CBaseDTO>();

                nombramientoDescarga = new CNombramientoD(contexto);
                var item = nombramientoDescarga.CargarNombramientoActualCedula(cedula);

                if (item != null)
                {
                    var nombramientoActual = ConvertirDatosNombramientoADTO(item);
                    var funcionario = CFuncionarioL.FuncionarioGeneral(item.Funcionario);
                    var puesto = CPuestoL.ConstruirPuesto(item.Puesto, new CPuestoDTO());
                    var direccion = CDireccionL.ConvertirDireccionADTO(item.Funcionario.Direccion.FirstOrDefault());
                    var ubicacionA = CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(item.Puesto.RelPuestoUbicacion.ElementAt(0).UbicacionPuesto);
                    var ubicacionB = CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(item.Puesto.RelPuestoUbicacion.ElementAt(1).UbicacionPuesto);
                    var ubicacionAdministrativa = CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO(item.Puesto.UbicacionAdministrativa);
                    return res;
                }
                else
                {
                    throw new Exception("El funcionario no cuenta con ningún nombramiento activo.");
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }


        public CNombramientoDTO DescargarNombramientoActualPuesto(string codPuesto)
        {
            CNombramientoDTO res = new CNombramientoDTO();

            nombramientoDescarga = new CNombramientoD(contexto);
            var item = nombramientoDescarga.CargarNombramientoActualPuesto(codPuesto);

            res.IdEntidad = item.PK_Nombramiento;
            res.EstadoNombramiento = new CEstadoNombramientoDTO
            {
                IdEntidad = item.EstadoNombramiento.PK_EstadoNombramiento,
                DesEstado = item.EstadoNombramiento.DesEstado
            };
            res.FecRige = Convert.ToDateTime(item.FecRige);
            res.FecVence = Convert.ToDateTime(item.FecVence);

            return res;
        }          
        
        //Se insertó ICNombramiento y CNombramientoService el 30/01/2017....
        public CBaseDTO CrearNombramientoInicial(CNombramientoDTO nombramientoInicial)
        {
            CBaseDTO resultado = new CBaseDTO();

            CNombramientoD intermedio = new CNombramientoD(contexto);

            try
            {                      //Metodo que está anteriormente
                Nombramiento datos = ConstruirNombramiento(contexto, nombramientoInicial);
                int respuesta = intermedio.GuardarNombramiento(datos);
                if (respuesta > 0)
                {
                    resultado.Mensaje = "Exito";
                }
                else
                {
                    resultado.Mensaje = "Error desconocido";
                }
            }
            catch (Exception ex)
            {
                resultado.Mensaje = "Error: " + ex.Message;
            }
            return resultado;
        }

       //Se insertó ICNombramiento y CNombramientoService el 25/01/2017....
       public CBaseDTO GuardarNombramiento(CNombramientoDTO nombramiento, CPuestoDTO puesto)        
        {         
            CBaseDTO respuesta = new CBaseDTO();
            try
            {                
                CNombramientoD intermedio = new CNombramientoD(contexto);
                CEstadoNombramientoD intermedioEstado = new CEstadoNombramientoD(contexto);
                CPuestoD intermedioPuesto = new CPuestoD(contexto);
                CFuncionarioD intermediarioFuncionario = new CFuncionarioD(contexto);
                CPeriodoVacacionesD intermedioVacaciones = new CPeriodoVacacionesD(contexto);              
                
                Nombramiento datos = new Nombramiento
                {
                    FecRige = Convert.ToDateTime(nombramiento.FecRige),
                    FecNombramiento = Convert.ToDateTime(nombramiento.FecNombramiento),
                    FecVence = nombramiento.FecVence.Year == 1 ? (DateTime?)null : nombramiento.FecVence,
                    FK_EstadoNombramiento =  intermedioEstado.CargarEstadoNombramientoPorID(nombramiento.EstadoNombramiento.IdEntidad).PK_EstadoNombramiento,
                    Puesto = intermedioPuesto.DescargarPuestoCodigo(nombramiento.Puesto.CodPuesto),
                    Funcionario = intermediarioFuncionario.BuscarFuncionarioCedula(nombramiento.Funcionario.Cedula)
                };

                datos.Puesto.EstadoPuesto = contexto.EstadoPuesto.Where(Q => Q.PK_EstadoPuesto == nombramiento.Puesto.EstadoPuesto.IdEntidad).FirstOrDefault();
                datos.Funcionario.EstadoFuncionario = contexto.EstadoFuncionario.Where(Q => Q.PK_EstadoFuncionario == nombramiento.Funcionario.EstadoFuncionario.IdEntidad).FirstOrDefault();

                if (puesto != null && nombramiento.Puesto.CodPuesto != puesto.CodPuesto)
                {
                    var guardarPuesto = false;
                    //if (datos.Puesto.CodPuesto != puesto.CodPuesto)
                    if (intermedioPuesto.DescargarPuestoCodigo(nombramiento.Puesto.CodPuesto).CodPuesto != puesto.CodPuesto)
                    {
                        var puestoBusca = intermedioPuesto.DescargarPuestoCodigo(puesto.CodPuesto);
                        puestoBusca.EstadoPuesto = contexto.EstadoPuesto.Where(Q => Q.PK_EstadoPuesto == puesto.EstadoPuesto.IdEntidad).FirstOrDefault();
                        guardarPuesto = intermedioPuesto.ActualizarEstadoPuesto(puestoBusca);
                    }
                    else
                    {
                        guardarPuesto = true;
                    }
                    if (guardarPuesto)
                    {
                        respuesta.IdEntidad = intermedio.GuardarNombramiento(datos);

                        if (nombramiento.Funcionario.Mensaje != null)
                        {
                            intermedioVacaciones.TrasladarPeriodosVacacionesNombramiento(nombramiento.Funcionario.Cedula,
                                                                                         respuesta.IdEntidad);
                        }
                    }
                    else
                    {
                        throw new Exception("No se pudo guardar el nombramiento");
                    }
                }
                else
                {
                    respuesta.IdEntidad = intermedio.GuardarNombramiento(datos);
                    if (nombramiento.Funcionario.Mensaje != null)
                    {
                        intermedioVacaciones.TrasladarPeriodosVacacionesNombramiento(nombramiento.Funcionario.Cedula,
                                                                                     respuesta.IdEntidad);
                    }
                }

                return respuesta;
            }
            catch (Exception error)
            {
                //en respuesta del catch, se digita el error 
                respuesta = new CErrorDTO { IdEntidad = -1, MensajeError = error.Message };
                return respuesta;
            }         
        }


        public List<CBaseDTO> DescargarNombramientoDetallePuesto(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            decimal monto = 0;


            CNombramientoD intermedio = new CNombramientoD(contexto);
            CFuncionarioD intermediarioFuncionario = new CFuncionarioD(contexto);

            try
            {

                Nombramiento datoNombramiento = intermedio.CargarNombramiento(codigo);
                //Nombramiento datoNombramiento = dato.Nombramiento.OrderBy(N => N.FecRige).LastOrDefault();

                if (datoNombramiento == null)
                {
                    throw new Exception("No existe el Nombramiento");
                }

                CFuncionarioDTO funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(intermediarioFuncionario.BuscarFuncionarioCedula(datoNombramiento.Funcionario.IdCedulaFuncionario));
                respuesta.Add(funcionario);

                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                respuesta.Add(puesto);

                DetallePuesto datoDetalle = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault();
                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();

                detallePuesto.IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PK_DetallePuesto;
                detallePuesto.PorProhibicion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorProhibicion);
                detallePuesto.PorDedicacion = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().PorDedicacion);

                detallePuesto.Clase = new CClaseDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.PK_Clase,
                    DesClase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Clase.DesClase
                };


                if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad != null)
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().Especialidad.DesEspecialidad
                    };
                }
                else
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO { IdEntidad = -1 };
                }


                detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PK_Escala,
                    CategoriaEscala = Convert.ToInt16(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.IndCategoria),
                    SalarioBase = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                    MontoAumentoAnual = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.MtoAumento),

                    Periodo = new CPeriodoEscalaSalarialDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.PK_Periodo,
                        MontoPuntoCarrera = Convert.ToDecimal(datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().EscalaSalarial.PeriodoEscalaSalarial.MtoPuntoCarrera)
                    }
                };

                detallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                foreach (var item in datoDetalle.DetallePuestoRubro)
                {
                    if (item.ComponenteSalarial.TipComponenteSalarial == 1)  // Tipo Nominal
                        monto = item.PorValor;
                    else   // Tipo Porcentual
                        monto = (item.PorValor * detallePuesto.EscalaSalarial.SalarioBase) / 100;

                    detallePuesto.DetalleRubros.Add(new CDetallePuestoRubroDTO
                    {
                        IdEntidad = item.PK_DetallePuestoRubro,
                        Componente = new CComponenteSalarialDTO
                        {
                            IdEntidad = item.ComponenteSalarial.PK_ComponenteSalarial,
                            DesComponenteSalarial = item.ComponenteSalarial.DesComponenteSalarial
                        },
                        PorValor = item.PorValor,
                        MtoValor = monto
                    });
                }

                if (datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = datoNombramiento.Puesto.DetallePuesto.Where(DP => DP.IndEstadoDetallePuesto.HasValue == false || DP.IndEstadoDetallePuesto == 1).FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }

                respuesta.Add(detallePuesto);

                // Con esta instrucción se omiten los Exempleados
                //var datoDetalleContrato = dato.DetalleContratacion.Where(Q => Q.FecCese == null).FirstOrDefault();
                var datoDetalleContrato = datoNombramiento.Funcionario.DetalleContratacion.FirstOrDefault();

                if (datoDetalleContrato != null)
                {
                    respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(datoDetalleContrato));
                }
                else
                {
                    respuesta.Add(new CDetalleContratacionDTO
                    {
                        NumeroAnualidades = 0,
                        FechaMesAumento = "1"
                    });
                }
            }
            catch (Exception error)
            {
                if (respuesta.Count > 0)
                {
                    respuesta.Clear();
                }
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }


            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarDatosRegistroNombramiento(string codpuesto, string cedula)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CNombramientoD intermedio = new CNombramientoD(contexto);

                var puesto = intermedio.BuscarNombramientoCodigoPuesto(codpuesto);
                var funcionario = intermedio.BuscarNombramientoCedulaFuncionario(cedula);

                if (puesto.Codigo == -1)
                {
                    throw new Exception(((CErrorDTO)puesto.Contenido).MensajeError);
                }
                else
                {
                    if (puesto.Mensaje == "Nombramiento")
                    {
                        List<CBaseDTO> temp;
                        var datosNombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(((Nombramiento)puesto.Contenido));
                        var datosPuesto = CPuestoL.ConstruirPuesto(((Nombramiento)puesto.Contenido).Puesto, new CPuestoDTO());
                        var detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Nombramiento)puesto.Contenido).Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1).OrderByDescending(O => O.FecRige).FirstOrDefault());
                        if (detallePuesto == null)
                        {
                            detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Nombramiento)puesto.Contenido).Puesto.DetallePuesto.FirstOrDefault());
                        }
                        temp = new List<CBaseDTO>
                        {
                            datosPuesto, detallePuesto
                        };
                        var datosFuncionario = CFuncionarioL.FuncionarioGeneral(((Nombramiento)puesto.Contenido).Funcionario);
                        var detalleContratacion = CDetalleContratacionL.ConvertirDetalleContratacionADTO(((Nombramiento)puesto.Contenido).Funcionario.DetalleContratacion.FirstOrDefault());
                        temp = new List<CBaseDTO>
                        {
                            datosNombramiento, datosPuesto, detallePuesto, datosFuncionario, detalleContratacion
                        };
                        respuesta.Add(temp);
                    }
                    else
                    {
                        List<CBaseDTO> temp;
                        var datosPuesto = CPuestoL.ConstruirPuesto(((Puesto)puesto.Contenido), new CPuestoDTO());
                        var detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Puesto)puesto.Contenido).DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault());
                        if (detallePuesto == null)
                        {
                            detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Puesto)puesto.Contenido).DetallePuesto.FirstOrDefault());
                        }
                        temp = new List<CBaseDTO>
                        {
                            datosPuesto, detallePuesto
                        };
                        respuesta.Add(temp);
                    }
                }

                if (funcionario.Codigo == -1)
                {
                    throw new Exception(((CErrorDTO)funcionario.Contenido).MensajeError);
                }
                else
                {
                    if (funcionario.Mensaje == "Nombramiento")
                    {
                        List<CBaseDTO> temp;
                        var datosNombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(((Nombramiento)funcionario.Contenido));
                        var datosPuesto = CPuestoL.ConstruirPuesto(((Nombramiento)funcionario.Contenido).Puesto, new CPuestoDTO());
                        var detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Nombramiento)funcionario.Contenido).Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1).OrderByDescending(O => O.FecRige).FirstOrDefault());
                        if (detallePuesto == null)
                        {
                            detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Nombramiento)funcionario.Contenido).Puesto.DetallePuesto.FirstOrDefault());
                        }
                        var datosFuncionario = CFuncionarioL.FuncionarioGeneral(((Nombramiento)funcionario.Contenido).Funcionario);
                        var detalleContratacion = CDetalleContratacionL.ConvertirDetalleContratacionADTO(((Nombramiento)funcionario.Contenido).Funcionario.DetalleContratacion.FirstOrDefault());
                        temp = new List<CBaseDTO>
                        {
                            datosNombramiento, datosPuesto, detallePuesto, datosFuncionario, detalleContratacion
                        };
                        respuesta.Add(temp);
                    }
                    else
                    {
                        List<CBaseDTO> temp;
                        var datosFuncionario = CFuncionarioL.FuncionarioGeneral(((Funcionario)funcionario.Contenido));
                        if (datosFuncionario.Sexo == 0)
                        {
                            datosFuncionario.Sexo = GeneroEnum.Indefinido;
                        }
                        var detalleContratacion = CDetalleContratacionL.ConvertirDetalleContratacionADTO(((Funcionario)funcionario.Contenido).DetalleContratacion.FirstOrDefault());
                        temp = new List<CBaseDTO>
                        {
                            datosFuncionario, detalleContratacion
                        };
                        respuesta.Add(temp);
                    }
                }

                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new List<List<CBaseDTO>>();
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message + "logica" } });
                return respuesta;
            }
        }

        public List<CBaseDTO> ListarEstadosNombramiento()
        {
            List<CBaseDTO> respuesta;
            try
            {
                CNombramientoD intermedio = new CNombramientoD(contexto);
                var estados = intermedio.ListarEstadosNombramiento();
                if (estados.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();
                    foreach (var item in (List<EstadoNombramiento>)estados.Contenido)
                    {
                        var estado = new CEstadoNombramientoDTO { IdEntidad = item.PK_EstadoNombramiento, DesEstado = item.DesEstado };
                        respuesta.Add(estado);
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)estados.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
        
        public List<List<CBaseDTO>> BuscarHistorialNombramiento(CFuncionarioDTO funcionario,
                                                List<DateTime> fechasEmision, CPuestoDTO puesto)
        {
            try
            {
                List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

                CNombramientoD intermedio = new CNombramientoD(contexto);

                List<Nombramiento> datosNombramiento = new List<Nombramiento>();

                if (funcionario.Cedula != null)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        BuscarHistorialNombramiento(datosNombramiento, funcionario.Cedula, "cedula"));

                    if (resultado.Codigo > 0)
                    {
                        datosNombramiento = (List<Nombramiento>)resultado.Contenido;
                    }
                }

                if (fechasEmision.Count > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        BuscarHistorialNombramiento(datosNombramiento, fechasEmision, "fechas"));
                    if (resultado.Codigo > 0)
                    {
                        datosNombramiento = (List<Nombramiento>)resultado.Contenido;
                    }
                }

                if (puesto.CodPuesto != null)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        BuscarHistorialNombramiento(datosNombramiento, puesto.CodPuesto, "numeroPuesto"));
                    if (resultado.Codigo > 0)
                    {
                        datosNombramiento = (List<Nombramiento>)resultado.Contenido;
                    }
                }

                if (datosNombramiento.Count > 0)
                {
                    foreach (var item in datosNombramiento)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();

                        var datoNombramiento = ConvertirDatosNombramientoADTO(item);

                        temp.Add(datoNombramiento);

                        CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                        {
                            Cedula = item.Funcionario.IdCedulaFuncionario,
                            Nombre = item.Funcionario.NomFuncionario,
                            PrimerApellido = item.Funcionario.NomPrimerApellido,
                            SegundoApellido = item.Funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        };

                        temp.Add(tempFuncionario);

                        CPuestoDTO tempPuesto = new CPuestoDTO
                        {
                            CodPuesto = item.Puesto.CodPuesto
                        };

                        temp.Add(tempPuesto);

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
            catch (Exception error)
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                resultado.Add(temp);
                return resultado;
            }
        }

        public List<CBaseDTO> NombramientoPorCodigo(int codNombramiento)
        {
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();

                CNombramientoD intermedio = new CNombramientoD(contexto);

                var resultado = intermedio.NombramientoPorCodigo(codNombramiento);

                if (resultado.Codigo > 0)
                {
                    var nombramiento = ConvertirDatosNombramientoADTO((Nombramiento)resultado.Contenido);

                    respuesta.Add(nombramiento);

                    var funcionario = CFuncionarioL.FuncionarioGeneral(((Nombramiento)resultado.Contenido).Funcionario);

                    respuesta.Add(funcionario);

                    var puesto = CPuestoL.ConstruirPuesto(((Nombramiento)resultado.Contenido).Puesto, new CPuestoDTO());

                    respuesta.Add(puesto);

                    var detallePuesto = CDetallePuestoL.ConstruirDetallePuesto(((Nombramiento)resultado.Contenido).Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderBy(O => O.FecRige).FirstOrDefault());

                    respuesta.Add(detallePuesto);

                    var datoAccionPersonal = contexto.AccionPersonal.Where(Q => Q.FK_Nombramiento == nombramiento.IdEntidad).FirstOrDefault();

                    if (datoAccionPersonal != null)
                    {
                        var accionPersonalSIRH = CAccionPersonalL.ConvertirDatosADto(datoAccionPersonal);
                        accionPersonalSIRH.Mensaje = "SIRH";
                        respuesta.Add(accionPersonalSIRH);
                    }
                    else
                    {
                        List<int?> listaEstados = new List<int?> { 20, 21, 22, 23, 24, 25, 30, 31, 38, 39, 48, 49, 54, 57, 58, 80, 90 };
                        var datoRespaldo = contexto.C_EMU_AccionPersonal.
                                                    Where(Q => Q.Cedula == funcionario.Cedula
                                                    && Q.NumPuesto2 == puesto.CodPuesto
                                                    && Q.FecRige == nombramiento.FecRige
                                                    && listaEstados.Contains(Q.CodAccion))
                                                    .FirstOrDefault();
                        if (datoRespaldo != null)
                        {
                            CAccionPersonalHistoricoDTO accionRespuesta = new CAccionPersonalHistoricoDTO
                            {
                                IdEntidad = datoRespaldo.ID,
                                NumAccion = datoRespaldo.NumAccion,
                                Mensaje = "Emulacion"
                            };
                            respuesta.Add(accionRespuesta);
                        }
                        else
                        {
                            respuesta.Add(new CAccionPersonalDTO());
                        }
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
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }

        public List<List<CBaseDTO>> ListarNombramientosVence(DateTime fecha)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CNombramientoD intermedio = new CNombramientoD(contexto);

                var resultado = intermedio.ListarNombramientosVence(fecha);

                if (resultado.Codigo > 0)
                {
                    foreach (var item in (List<Nombramiento>)resultado.Contenido)
                    {
                        List<CBaseDTO> temp = new List<CBaseDTO>();
                        temp.Add(ConvertirDatosNombramientoADTO(item));
                        temp.Add(CFuncionarioL.FuncionarioGeneral(item.Funcionario));
                        temp.Add(CPuestoL.ConstruirPuesto(item.Puesto, new CPuestoDTO()));
                        respuesta.Add(temp);
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
                respuesta.Clear();
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.InnerException != null ? error.InnerException.Message : error.Message } });
                return respuesta;
            }
        }

        #endregion
    }
}

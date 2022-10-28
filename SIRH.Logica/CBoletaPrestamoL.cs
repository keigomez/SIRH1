using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CBoletaPrestamoL
    {
        #region Variables
        SIRHEntities contexto;
        #endregion

        #region Constructor
        public CBoletaPrestamoL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Métodos


        public CBaseDTO AgregarBoletaPrestamo(CFuncionarioDTO funcionario, CUsuarioDTO usuario, CBoletaPrestamoDTO boleta)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CUsuarioD mediadorUsuario = new CUsuarioD(contexto);
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);
                CBoletaPrestamoD mediadorBoleta = new CBoletaPrestamoD(contexto);
                CDepartamentoD mediadorDepartmaneto = new CDepartamentoD(contexto);
                CTomoD mediadorTomo = new CTomoD(contexto);

                Funcionario FuncionarioConvertido = ConvertirDTOAFuncionario(funcionario);
                // Departamento DepartamentoConvertido = mediadorDepartmaneto.CargarDepartamenotParam(boleta.DepartamentoFuncionario);
                Usuario UsuarioConvertido = ConvertirDTOAUsuario(usuario);

                if (Convert.ToInt32(boleta.TipoUsuario) == 1)
                { // usuario Interno

                    var FuncionarioExpediente = mediadorExpediente.BuscarExpedienteUsuarioPorCedula(FuncionarioConvertido.IdCedulaFuncionario);
                    if (FuncionarioExpediente.Codigo != -1)
                    { // Si existe expediente.

                        ExpedienteFuncionario expedienteRecuperado = (ExpedienteFuncionario)FuncionarioExpediente.Contenido;


                        Tomo nuevoTomo;

                        var Actualizado = mediadorExpediente.ActualizarExpedienteFuncionario(FuncionarioConvertido);
                        int cantidadTomos = mediadorTomo.ObtenerTomosPorExpediente(FuncionarioConvertido, expedienteRecuperado);


                        if (cantidadTomos == 0)
                        { // si el expediente no tiene Tomos.
                            nuevoTomo = CrearTomo(DateTime.Today, expedienteRecuperado, 1);
                            var TomoAgregado = mediadorTomo.AgregarTomo(nuevoTomo);

                            if (TomoAgregado.Codigo > 0)
                            { // si el tomo se agregó exitosamente
                                BoletaPrestamo nuevaBoleta = NuevaBoletaInterna(FuncionarioConvertido, UsuarioConvertido, boleta, expedienteRecuperado);
                                respuesta = mediadorBoleta.GuardarBoletaPrestamo(nuevaBoleta);

                                DetalleBoleta nuevosDatos = NuevosDatosBoleta(boleta, Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido));
                                var datosGuardados = mediadorBoleta.GuardarDatosBoleta(nuevosDatos);

                                if (datosGuardados.Codigo > 0)
                                { //Si guardo los datos de la boleta exitosamente, significa que la boleta si se guardó correctamente.


                                    if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                                    { // si hubo algún error al agregar 
                                        respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                                        throw new Exception();
                                    }
                                    else
                                    { // si todo salió bien.
                                        return respuesta;
                                    }
                                }
                                else
                                { //sino se guardó la boleta exitosamente
                                    respuesta = (CErrorDTO)((CRespuestaDTO)datosGuardados).Contenido;
                                    throw new Exception();
                                }

                            }
                            else
                            { // si el tomo no se agregó exitosamente
                                //respuesta = (CErrorDTO)(CRespuestaDTO)TomoAgregado.Contenido;
                                respuesta = (CErrorDTO)TomoAgregado.Contenido;
                                throw new Exception();
                            }
                        }
                        else
                        { // si el expediente  tiene almenos 1 Tomo, agregue la boleta.
                            BoletaPrestamo nuevaBoleta = NuevaBoletaInterna(FuncionarioConvertido, UsuarioConvertido, boleta, expedienteRecuperado);
                            respuesta = mediadorBoleta.GuardarBoletaPrestamo(nuevaBoleta);

                            DetalleBoleta nuevosDatos = NuevosDatosBoleta(boleta, Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido));
                            var datosGuardados = mediadorBoleta.GuardarDatosBoleta(nuevosDatos);

                            if (datosGuardados.Codigo > 0)
                            { //Si guardo los datos de la boleta exitosamente, significa que la boleta si se guardó correctamente.

                                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                                { // si hubo algún error al agregar 
                                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                                    throw new Exception();
                                }
                                else
                                { // si todo salió bien.
                                    return respuesta;
                                }
                            }
                            else
                            { //sino se guardó la boleta exitosamente
                                //respuesta = (CErrorDTO)(CRespuestaDTO)datosGuardados.Contenido;
                                respuesta = (CErrorDTO)datosGuardados.Contenido;
                                throw new Exception();
                            }
                        }
                    }
                    else
                    { // si el usuario interno no posee expediente
                        ExpedienteFuncionario nuevoExpediente = NuevoExpediente(FuncionarioConvertido, boleta, DateTime.Today);

                        var expedienteAgregado = mediadorExpediente.AgregarExpedienteFuncionario(nuevoExpediente);
                        if (expedienteAgregado.Codigo > 0)
                        { // si se agregó un expediente satisfatoriamente

                            // Obtenemos el expediente recien creado.
                            var expedienteNuevoFuncionario = mediadorExpediente.BuscarExpedienteUsuarioPorCedula(FuncionarioConvertido.IdCedulaFuncionario);
                            if (expedienteNuevoFuncionario.Codigo != -1)
                            {

                                ExpedienteFuncionario expedienteRecuperado = (ExpedienteFuncionario)expedienteNuevoFuncionario.Contenido;

                                //Actualizamos la info del tomo recien creado.
                                var Actualizado = mediadorExpediente.ActualizarExpedienteFuncionario(FuncionarioConvertido);
                                int cantidadTomos = mediadorTomo.ObtenerTomosPorExpediente(FuncionarioConvertido, expedienteRecuperado);

                                Tomo nuevoTomo = CrearTomoDeExpedienteNuevo(DateTime.Today, nuevoExpediente, Convert.ToInt32(expedienteAgregado.Contenido), 1);
                                var TomoAgregado = mediadorTomo.AgregarTomo(nuevoTomo);

                                if (TomoAgregado.Codigo > 0)
                                { // si el tomo se agregó exitosamente
                                    BoletaPrestamo nuevaBoleta = NuevaBoletaInterna(FuncionarioConvertido, UsuarioConvertido, boleta, expedienteRecuperado);
                                    respuesta = mediadorBoleta.GuardarBoletaPrestamo(nuevaBoleta);

                                    DetalleBoleta nuevosDatos = NuevosDatosBoleta(boleta, Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido));
                                    var datosGuardados = mediadorBoleta.GuardarDatosBoleta(nuevosDatos);

                                    if (datosGuardados.Codigo > 0)
                                    { //Si guardo los datos de la boleta exitosamente, significa que la boleta si se guardó correctamente.

                                        if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                                        { // si hubo algún error al agregar 
                                            respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                                            throw new Exception();
                                        }
                                        else
                                        { // si todo salió bien.
                                            return respuesta;
                                        }
                                    }
                                    else
                                    { //sino se guardó la boleta exitosamente
                                        //respuesta = (CErrorDTO)(CRespuestaDTO)datosGuardados.Contenido;
                                        respuesta = (CErrorDTO)datosGuardados.Contenido;
                                        throw new Exception();
                                    }

                                }
                                else
                                { // si el tomo no se agregó exitosamente
                                    respuesta = (CErrorDTO)((CRespuestaDTO)TomoAgregado).Contenido;
                                    throw new Exception();
                                }
                            }
                        }
                        return respuesta;
                    }
                }
                else
                { // usuario Externo
                    var FuncionarioExpediente = mediadorExpediente.BuscarExpedienteUsuarioPorCedula(FuncionarioConvertido.IdCedulaFuncionario);
                    if (FuncionarioExpediente.Codigo != -1)
                    { // Si existe expediente.

                        ExpedienteFuncionario expedienteRecuperado = (ExpedienteFuncionario)FuncionarioExpediente.Contenido;
                        Tomo nuevoTomo;

                        var Actualizado = mediadorExpediente.ActualizarExpedienteFuncionario(FuncionarioConvertido);
                        int cantidadTomos = mediadorTomo.ObtenerTomosPorExpediente(FuncionarioConvertido, expedienteRecuperado);

                        if (cantidadTomos == 0)
                        { // si el expediente no tiene Tomos.
                            nuevoTomo = CrearTomo(DateTime.Today, expedienteRecuperado, 1);
                            var TomoAgregado = mediadorTomo.AgregarTomo(nuevoTomo);

                            if (TomoAgregado.Codigo > 0)
                            { // si el tomo se agregó exitosamente
                                BoletaPrestamo nuevaBoleta = NuevaBoletaExterna(FuncionarioConvertido, UsuarioConvertido, boleta, expedienteRecuperado);
                                respuesta = mediadorBoleta.GuardarBoletaPrestamo(nuevaBoleta);

                                DetalleBoleta nuevosDatos = NuevosDatosBoleta(boleta, Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido));
                                var datosGuardados = mediadorBoleta.GuardarDatosBoleta(nuevosDatos);

                                if (datosGuardados.Codigo > 0)
                                { //Si guardo la boleta exitosamente.


                                    if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                                    { // si hubo algún error al agregar 
                                        respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                                        throw new Exception();
                                    }
                                    else
                                    { // si todo salió bien.
                                        return respuesta;
                                    }
                                }
                                else
                                { //sino se guardó la boleta exitosamente
                                    respuesta = (CErrorDTO)((CRespuestaDTO)datosGuardados).Contenido;
                                    throw new Exception();
                                }

                            }
                            else
                            { // si el tomo no se agregó exitosamente
                                respuesta = (CErrorDTO)((CRespuestaDTO)TomoAgregado).Contenido;
                                throw new Exception();
                            }
                        }
                        else
                        { // si el expediente  tiene almenos 1 Tomo, agregue la boleta.
                            BoletaPrestamo nuevaBoleta = NuevaBoletaExterna(FuncionarioConvertido, UsuarioConvertido, boleta, expedienteRecuperado);
                            respuesta = mediadorBoleta.GuardarBoletaPrestamo(nuevaBoleta);

                            DetalleBoleta nuevosDatos = NuevosDatosBoleta(boleta, Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido));
                            var datosGuardados = mediadorBoleta.GuardarDatosBoleta(nuevosDatos);

                            if (datosGuardados.Codigo > 0)
                            { //Si guardo la boleta exitosamente.


                                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                                { // si hubo algún error al agregar 
                                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                                    throw new Exception();
                                }
                                else
                                { // si todo salió bien.
                                    return respuesta;
                                }
                            }
                            else
                            { //sino se guardó la boleta exitosamente
                                respuesta = (CErrorDTO)((CRespuestaDTO)datosGuardados).Contenido;
                                throw new Exception();
                            }
                        }
                    }
                    else
                    { // Si no existe expediente.
                        respuesta = new CErrorDTO { MensajeError = "El usuario buscado no posee un expediente." };
                        throw new Exception();
                    }
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };

                return respuesta;
            }
        }

        public CBaseDTO VerificarExistenciaBoleta(string cedula)
        {

            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CBoletaPrestamoD mediadorBoleta = new CBoletaPrestamoD(contexto);
                var boleta = mediadorBoleta.VerificarExistenciaBoleta(cedula);
                if (boleta.Codigo > 0)
                {
                    respuesta.Mensaje = "Existe";
                }
                else
                {
                    respuesta.Mensaje = "NoExiste";
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }
            return respuesta;
        }




        public List<CBaseDTO> ObtenerBoleta(int codigo)
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                //CUsuarioD mediadorUsuario = new CUsuarioD(contexto);
                CBoletaPrestamoD mediadorBoleta = new CBoletaPrestamoD(contexto);

                var boleta = (BoletaPrestamo)mediadorBoleta.ObtenerBoletaPrestamo(codigo).Contenido;
                DetalleBoleta detalle = (DetalleBoleta)mediadorBoleta.ObtenerDetalleBoleta(boleta).Contenido;
                Usuario usuario = boleta.Usuario;
                ExpedienteFuncionario expediente = (ExpedienteFuncionario)mediadorBoleta.ObtenerExpedienteFuncionario(boleta).Contenido;
                // var temp = mediadorBoleta.ObtenerFuncionario(expediente);
                Funcionario funcionario = expediente.Funcionario;


                respuesta.Add(ConvertirBoletaADTO(boleta, detalle, funcionario));
                respuesta.Add(ConvertirUsuarioADTO(usuario));
                respuesta.Add(ConvertirFuncionarioADTO(funcionario));
                respuesta.Add(ConvertirExpedienteFuncionoarioADTO(expediente));

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

        public List<CBaseDTO> BusquedaBoletaSegunParametros(CExpedienteFuncionarioDTO expediente)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            var datoParam = "";
            var fechaInicioParam = "";
            var fechaFinParam = "";

            try
            {
                CBoletaPrestamoD mediadorBoleta = new CBoletaPrestamoD(contexto);
                CFuncionarioD intermedio = new CFuncionarioD(contexto);

                // TODA ESTA BUSQUEDA SE HARÁ BUSCANDO LAS BOLETAS DE PRÉSTAMO, YA SU OBJETIVO ES "TRACKEAR" LOS EXPEDIENTES PRESTADOS.
                if (expediente.FiltroBusqueda != "Fecha Préstamo" && expediente.FiltroBusqueda != "Fecha Devolución")
                {
                    datoParam = expediente.DatoABuscar;
                }
                else
                {
                    fechaInicioParam = expediente.FechaInicio;
                    fechaFinParam= expediente.FechaFin;
                }
                var boletaConsultada = mediadorBoleta.VerificarExistenciaBoletaSegunParametros(expediente.FiltroBusqueda, datoParam, fechaInicioParam, fechaFinParam).Contenido;
                if (boletaConsultada.GetType() != typeof(CErrorDTO))
                {
                    List<BoletaPrestamo> listaBoletasRecuperadas = (List<BoletaPrestamo>)boletaConsultada;
                    foreach (BoletaPrestamo item in listaBoletasRecuperadas)
                    {
                        var detalleNombramientoFuncionario = intermedio.BuscarFuncionarioDetallePuesto(item.ExpedienteFuncionario.Funcionario.IdCedulaFuncionario);

                        Funcionario dato = (Funcionario)detalleNombramientoFuncionario.Contenido;
                        Nombramiento datoNombramiento;
                        if (dato.Nombramiento != null && dato.Nombramiento.Count > 0)
                        {
                            datoNombramiento = dato.Nombramiento.Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now).OrderByDescending(x => x.FecRige).FirstOrDefault();

                            if (datoNombramiento == null)
                            {
                                datoNombramiento = dato.Nombramiento.OrderByDescending(N => N.FecVence ?? DateTime.MaxValue).ThenByDescending(x => x.FecRige).FirstOrDefault();
                                if (datoNombramiento == null)
                                {
                                    throw new Exception("El funcionario no tiene un Nombramiento");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("El funcionario no tiene un Nombramiento");
                        }

                        CPuestoDTO puesto = new CPuestoDTO();
                        puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);
                        respuesta.Add(ConvertirBoletaADTO(item, puesto));
                    }
                }
                else
                {
                    CErrorDTO error = ((CErrorDTO)boletaConsultada);
                    respuesta.Add(error);
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

        public List<List<CBaseDTO>> RealizarFoleo(string cedula)
        {

            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            try
            {
                CExpedienteD mediadorExpediente = new CExpedienteD(contexto);
                CTomoD mediadorTomo = new CTomoD(contexto);

                //Verificamos si el ussuario posee un expediente.
                var expediente = mediadorExpediente.BuscarExpedienteUsuarioPorCedula(cedula);
                if (expediente.Codigo > 0)
                {
                    //Recuperamos el expediente.
                    ExpedienteFuncionario expedienteRecuperado = (ExpedienteFuncionario)expediente.Contenido;
                    
                    // Obtenemos la los tomos relacionados a el expediente recuperado.
                    var tomos = mediadorTomo.ObtenerListaTomosPorExpediente(expedienteRecuperado);

                    if (tomos.FirstOrDefault().Contenido.GetType() != typeof(CErrorDTO))
                    { // si posee al menos un tomo.

                        List<CBaseDTO> objExpediente = new List<CBaseDTO>();
                        List<CBaseDTO> objTomo = new List<CBaseDTO>();
                        List<CBaseDTO> objFolio = new List<CBaseDTO>();

                        objExpediente.Add(ConvertirExpedienteFuncionoarioADTO(expedienteRecuperado));
                        respuesta.Add(objExpediente);

                        foreach (CRespuestaDTO item in tomos)
                        { // recorremos la lista de tomos obtenida según el expediente recuperado.

                            Tomo tomoAdquirido = (Tomo)item.Contenido;

                            objTomo.Add(ConvertirTomoADTO(tomoAdquirido)); // convertimos el Tomo actual a DTO, inicializando la lista de Folios internamente.

                            foreach (Folio itemF in tomoAdquirido.Folio)
                            { // recorremos los folios relacionados a un tomo. 

                                CFolioDTO folioDTO = ConvertirFolioADTO(tomoAdquirido, itemF); // convertimos cada folio a CFolioDTO.
                                objFolio.Add(folioDTO);
                            }
                        }

                        respuesta.Add(objTomo);
                        respuesta.Add(objFolio);
                    }
                    else
                    {
                        respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = "El funcionario solicitado no posee ninún tomo registrado de momento." } });
                    }
                }
                else
                {
                    respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = "La cédula suministrada no coincide con ningún funcionario o el funcionario no posee un expediente aún." } });
                }
            }
            catch (Exception error)
            {
                // respuesta.Add(new CErrorDTO { MensajeError = "Ocurrio un error a la hora de realizar el foleo." });
            }
            return respuesta;
        }

        // Método específico para el Daemon
        public List<CBaseDTO> VerificarFechaVencimientoPrestamo(DateTime fecha)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CBoletaPrestamoD mediadorBoleta = new CBoletaPrestamoD(contexto);

                var boletasPorVencer = mediadorBoleta.VerificarFechaVencimientoPrestamo(fecha);
                if (boletasPorVencer.Codigo > 0)
                {
                    foreach (BoletaPrestamo item in (List<BoletaPrestamo>)boletasPorVencer.Contenido)
                    {

                        var expediente = mediadorBoleta.ObtenerExpedienteFuncionario(item);
                        if (expediente.Codigo > 0)
                        {
                            ExpedienteFuncionario expedienteRecuperado = (ExpedienteFuncionario)expediente.Contenido;
                            respuesta.Add(ConvertirBoletaADTO(item, expedienteRecuperado.Funcionario));
                        }
                    }
                }
                else
                {

                    respuesta.Add(new CErrorDTO { MensajeError = "Sin Boletas en la BD." });
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }
            return respuesta;
        }






        internal static Usuario ConvertirDTOAUsuario(CUsuarioDTO item)
        {
            return new Usuario
            {
                PK_Usuario = item.IdEntidad,
                NomUsuario = item.NombreUsuario,
                TelOficial = item.TelefonoOficial,
                EmlOficial = item.EmailOficial,
                IndEstadoUsuario = item.IndEstUsuario
            };
        }

        internal static Departamento ConvertirDTOADepartamento(CDepartamentoDTO item)
        {
            return new Departamento
            {
                PK_Departamento = item.IdEntidad,
                NomDepartamento = item.NomDepartamento,
                IndEstadoDepartamento = item.IndEstDepartamento
            };
        }

        internal static BoletaPrestamo ConvertirDTOABoleta(CBoletaPrestamoDTO boleta)
        {
            return new BoletaPrestamo
            {
                PK_IdBoletaPrestamo = boleta.IdEntidad,
                IndTipoUsuario = Convert.ToInt32(boleta.TipoUsuario),
                NumBoleta = boleta.NumeroBoleta,
                IdCedulaSolicitante = Convert.ToInt32(boleta.TipoUsuario) == 1 ? boleta.ExpedienteFuncionario.Funcionario.Cedula : boleta.CedulaSolicitante,
                LugarProcedencia = boleta.LugarDeProcedencia
            };
        }

        internal static Funcionario ConvertirDTOAFuncionario(CFuncionarioDTO item)
        {
            return new Funcionario
            {
                PK_Funcionario = item.IdEntidad,
                FK_EstadoFuncionario = Convert.ToInt32(item.EstadoFuncionario.IdEntidad),
                IdCedulaFuncionario = item.Cedula,
                NomFuncionario = item.Nombre,
                NomPrimerApellido = item.PrimerApellido,
                NomSegundoApellido = item.SegundoApellido,
                FecNacimiento = Convert.ToDateTime(item.FechaNacimiento),
                IndSexo = castearGenero(Convert.ToInt32(item.Sexo))
            };
        }

        internal static ExpedienteFuncionario ConvertirDTOAExpedienteFuncionario(CExpedienteFuncionarioDTO item)
        {
            return new ExpedienteFuncionario
            {
                FecCreacion = Convert.ToDateTime(item.FechaCreacion),
                Estado = Convert.ToInt32(item.Estado),
                FecTrasladoArchivoCentral = Convert.ToDateTime(item.FechaTrasladoArchivoCentral),
                numExpediente = item.NumeroExpediente,
                numExpedienteEnArchivo = item.NumeroExpedienteEnArchivo.ToString(),
                numCaja = item.NumeroCaja.ToString()
            };
        }






        internal static CBoletaPrestamoDTO ConvertirBoletaADTO(BoletaPrestamo boleta, DetalleBoleta detalle, Funcionario funcionario)
        {
            return new CBoletaPrestamoDTO
            {
                IdEntidad = boleta.PK_IdBoletaPrestamo,
                TipoUsuario = Convert.ToInt32(boleta.IndTipoUsuario) == 1 ? TipoUsuarioEnum.Interno : TipoUsuarioEnum.Externo,
                CedulaSolicitante = boleta.IdCedulaSolicitante,
                NombreFuncionarioSolicitado = funcionario.NomFuncionario,
                ApellidoFuncionarioSolicitado = funcionario.NomPrimerApellido + "" + funcionario.NomSegundoApellido,
                Telefonolicitante = detalle.Telefono,
                CorreoSolicitante = detalle.Correo,
                FechaPrestamo = Convert.ToDateTime(detalle.FecPrestamo),
                FechaCaducidad = Convert.ToDateTime(detalle.FecCaducidad),
                MotivoPrestamo = detalle.MotivoPrestamo,
                LugarDeProcedencia = boleta.LugarProcedencia,
                NumeroBoleta = boleta.NumBoleta
            };
        }


        //----------------------------- MÉTODO ESPECÍFICO PARA EL DAEMON DE BOLETA --------------------------------------------------
        internal static CBoletaPrestamoDTO ConvertirBoletaADTO(BoletaPrestamo boleta, Funcionario funcionario)
        {
            return new CBoletaPrestamoDTO
            {
                IdEntidad = boleta.PK_IdBoletaPrestamo,
                TipoUsuario = Convert.ToInt32(boleta.IndTipoUsuario) == 1 ? TipoUsuarioEnum.Interno : TipoUsuarioEnum.Externo,
                CedulaSolicitante = boleta.IdCedulaSolicitante,
                NombreFuncionarioSolicitado = funcionario.NomFuncionario,
                ApellidoFuncionarioSolicitado = funcionario.NomPrimerApellido + "" + funcionario.NomSegundoApellido,
                Telefonolicitante = boleta.DetalleBoleta.FirstOrDefault().Telefono,
                CorreoSolicitante = boleta.DetalleBoleta.FirstOrDefault().Correo,
                FechaPrestamo = Convert.ToDateTime(boleta.DetalleBoleta.FirstOrDefault().FecPrestamo),
                FechaCaducidad = Convert.ToDateTime(boleta.DetalleBoleta.FirstOrDefault().FecCaducidad),
                MotivoPrestamo = boleta.DetalleBoleta.FirstOrDefault().MotivoPrestamo,
                NumeroBoleta = boleta.NumBoleta
            };
        }
        //---------------------------------------------------------------------------------------------------------------------------

        internal static CBoletaPrestamoDTO ConvertirBoletaADTO(BoletaPrestamo boleta, CPuestoDTO puesto)
        {
            return new CBoletaPrestamoDTO
            {
                IdEntidad = boleta.PK_IdBoletaPrestamo,
                CedulaFuncionario = boleta.ExpedienteFuncionario.Funcionario.IdCedulaFuncionario,
                CedulaSolicitante = boleta.IdCedulaSolicitante,
                TipoUsuario = (boleta.IndTipoUsuario == 1) ? TipoUsuarioEnum.Interno : TipoUsuarioEnum.Externo,
                Telefonolicitante = boleta.DetalleBoleta.FirstOrDefault().Telefono,
                CorreoSolicitante = boleta.DetalleBoleta.FirstOrDefault().Correo,
                FechaPrestamo = Convert.ToDateTime(boleta.DetalleBoleta.FirstOrDefault().FecPrestamo),
                FechaCaducidad = Convert.ToDateTime(boleta.DetalleBoleta.FirstOrDefault().FecCaducidad),
                NombreFuncionarioSolicitado = boleta.ExpedienteFuncionario.Funcionario.NomFuncionario,
                ApellidoFuncionarioSolicitado = boleta.ExpedienteFuncionario.Funcionario.NomPrimerApellido + " " + boleta.ExpedienteFuncionario.Funcionario.NomSegundoApellido,
                DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                MotivoPrestamo = boleta.DetalleBoleta.FirstOrDefault().MotivoPrestamo,
                LugarDeProcedencia = boleta.LugarProcedencia,
                NumeroExpediente = boleta.ExpedienteFuncionario.numExpediente.ToString(),
                NumeroBoleta = boleta.NumBoleta
            };
        }

        internal static CUsuarioDTO ConvertirUsuarioADTO(Usuario item)
        {
            return new CUsuarioDTO
            {
                IdEntidad = item.PK_Usuario,
                NombreUsuario = item.NomUsuario,
                TelefonoOficial = item.TelOficial,
                EmailOficial = item.EmlOficial,
                IndEstUsuario = Convert.ToInt32(item.IndEstadoUsuario)

            };
        }

        internal static CExpedienteFuncionarioDTO ConvertirExpedienteFuncionoarioADTO(ExpedienteFuncionario expediente)
        {
            return new CExpedienteFuncionarioDTO
            {
                // El tomo no se va a especificar debido a que puede ser una colección de los mismmos.
                FechaCreacion = Convert.ToDateTime(expediente.FecCreacion),
                Estado = castearEstadoExpediente(Convert.ToInt32(expediente.Estado)),
                FechaTrasladoArchivoCentral = Convert.ToDateTime(expediente.FecTrasladoArchivoCentral),
                NumeroExpediente = Convert.ToInt32(expediente.numExpediente),
                listaTomos = new List<CTomoDTO>()
            };
        }

        internal static CFuncionarioDTO ConvertirFuncionarioADTO(Funcionario item)
        {

            return new CFuncionarioDTO
            {
                Cedula = item.IdCedulaFuncionario,
                Nombre = item.NomFuncionario,
                PrimerApellido = item.NomPrimerApellido,
                SegundoApellido = item.NomSegundoApellido,
                Sexo = (item.IndSexo == "1" || item.IndSexo == "2") ? (GeneroEnum)Convert.ToInt32(item.IndSexo) : GeneroEnum.Masculino,
                FechaNacimiento = Convert.ToDateTime(item.FecNacimiento),
                EstadoFuncionario = new CEstadoFuncionarioDTO
                {
                    IdEntidad = item.EstadoFuncionario.PK_EstadoFuncionario,
                    DesEstadoFuncionario = item.EstadoFuncionario.DesEstadoFuncionario
                }
            };
        }

        internal static CDepartamentoDTO ConvertirDepartamentoADTO(Departamento item)
        {
            return new CDepartamentoDTO
            {
                IdEntidad = item.PK_Departamento,
                NomDepartamento = item.NomDepartamento,
                IndEstDepartamento = Convert.ToInt32(item.IndEstadoDepartamento)
            };
        }

        internal static CTomoDTO ConvertirTomoADTO(Tomo item)
        {
            return new CTomoDTO
            {
                IdEntidad = item.PK_IdTomo,
                Descripcion = item.Descripcion,
                FechaCreacion = Convert.ToDateTime(item.FecCreacion),
                ListaFolios = new List<CFolioDTO>(),
                NumeroTomo = item.NumTomo
            };
        }

        internal static CFolioDTO ConvertirFolioADTO(Tomo tomo, Folio item)
        {
            return new CFolioDTO
            {
                IdEntidad = item.PK_IdFolio,
                IdTomo = tomo.PK_IdTomo,
                FechaCreacion = Convert.ToDateTime(item.FecCreacion),
                Descripcion = item.Descripcion,
                NumeroFolio = item.NumFolio
            };
        }








        internal static Tomo CrearTomo(DateTime today, ExpedienteFuncionario expediente, int numeroTomo)
        {
            return new Tomo
            {
                FK_ExpedienteFuncionario = expediente.PK_IdExpedienteFuncionario,
                Descripcion = "Tomo 1",
                FecCreacion = Convert.ToDateTime(today),
                NumTomo = numeroTomo
            };

        }

        internal static Tomo CrearTomoDeExpedienteNuevo(DateTime today, ExpedienteFuncionario expedienteNuevo, int id_expediente, int numeroTomo)
        {
            return new Tomo
            {
                ExpedienteFuncionario = expedienteNuevo,
                FK_ExpedienteFuncionario = id_expediente,
                Descripcion = "",
                FecCreacion = Convert.ToDateTime(today),
                NumTomo = numeroTomo
            };
        }

        internal static ExpedienteFuncionario NuevoExpediente(Funcionario funcionario, CBoletaPrestamoDTO boleta, DateTime today)
        {
            return new ExpedienteFuncionario
            {
                FK_Funcionario = funcionario.PK_Funcionario,
                FecCreacion = Convert.ToDateTime(today),
                Estado = 0,
                FecTrasladoArchivoCentral = null,
                numExpediente = Convert.ToInt32(boleta.NumeroExpediente),
                numExpedienteEnArchivo = null,
                numCaja = null
            };
        }

        internal static DetalleBoleta NuevosDatosBoleta(CBoletaPrestamoDTO boleta, int id_boleta)
        {
            return new DetalleBoleta
            {
                FK_IdBoletaPrestamo = id_boleta,
                FecPrestamo = Convert.ToDateTime(boleta.FechaPrestamo),
                FecCaducidad = Convert.ToDateTime(boleta.FechaCaducidad),
                MotivoPrestamo = boleta.MotivoPrestamo,
                Correo = boleta.CorreoSolicitante,
                Telefono = boleta.Telefonolicitante
            };
        }

        internal static BoletaPrestamo NuevaBoletaInterna(Funcionario funcionario, Usuario usuario, CBoletaPrestamoDTO boleta, ExpedienteFuncionario exp)
        {
            return new BoletaPrestamo
            {
                FK_Usuario = usuario.PK_Usuario,
                FK_ExpedienteFuncionario = exp.PK_IdExpedienteFuncionario,
                IndTipoUsuario = 1,
                IdCedulaSolicitante = boleta.CedulaSolicitante,
                LugarProcedencia = boleta.LugarDeProcedencia,
                NumBoleta = boleta.NumeroBoleta
            };
        }

        internal static BoletaPrestamo NuevaBoletaExterna(Funcionario funcionario, Usuario usuario, CBoletaPrestamoDTO boleta, ExpedienteFuncionario exp
     )
        {
            return new BoletaPrestamo
            {
                FK_Usuario = usuario.PK_Usuario,
                FK_ExpedienteFuncionario = exp.PK_IdExpedienteFuncionario,
                IndTipoUsuario = 2,
                LugarProcedencia = boleta.LugarDeProcedencia,
                IdCedulaSolicitante = boleta.CedulaSolicitante
            };
        }

        internal static string castearGenero(int genero)
        {
            string res;
            switch (genero)
            {
                case 1:
                    res = "Masculino";
                    break;
                case 2:
                    res = "Femenino";
                    break;
                case 3:
                    res = "Indefinido";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        internal static EstadoEnum castearEstadoExpediente(int estado)
        {
            switch (estado)
            {
                case 2:
                    return EstadoEnum.TrasladadoArchivoCentral;
                case 1:
                    return EstadoEnum.Prestado;
                case 0:
                    return EstadoEnum.NoPrestado;

                default:
                    return EstadoEnum.NoDefinido;
            }
        }
        #endregion

    }
}

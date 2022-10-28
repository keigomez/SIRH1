using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCartaPresentacionD
    {
        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCartaPresentacionD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        private List<CartasPresentacion> FiltrarCartaPresentacion(object[] parametros)
        {
            var datos = entidadesBase.CartasPresentacion.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto").AsQueryable();
            string elemento;
            string valor;
           
            for (int i = 0; i < parametros.Length && datos.Count() != 0; i = i + 2)
            {
                elemento = parametros[i].ToString();
                switch (elemento)
                {
                    case "NumFuncionario":
                        valor = parametros[i + 1].ToString();
                        datos = datos.Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario == valor);
                        break;
                    case "NumCarta":
                        valor = parametros[i + 1].ToString();
                        datos = datos.Where(C=> C.NumCarta == valor);
                        break;
                    case "FechaRige":
                        var fechaRigeI = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaRigeF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(C => C.FecRige >= fechaRigeI && C.FecRige <= fechaRigeF);
                        break;
                    case "FechaFinal":
                        var fechaFinalI = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaFinalF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(C => C.FecVence >= fechaFinalI && C.FecVence <= fechaFinalF);
                        break;
                    case "FechaCarta":
                        var fechaCartaI = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                        var fechaCartaF = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(C => C.FecCarta >= fechaCartaI && C.FecCarta <= fechaCartaF);
                        break;
                }
            }
            return datos.ToList();
        }

        public CRespuestaDTO AgregarCartaPresentacion(Nombramiento nombramiento, CartasPresentacion carta)
        {
            CRespuestaDTO respuesta;
            try
            {
                nombramiento = entidadesBase.Nombramiento.FirstOrDefault(N => N.PK_Nombramiento == nombramiento.PK_Nombramiento);
                if (nombramiento != null)
                {
                    var datosCartas = nombramiento.CartasPresentacion.Count(C => C.NumCarta == carta.NumCarta);
                    //si no hay cartas con el mismo numero
                    if (datosCartas == 0)
                    {
                        //Servidor Activo 
                        if (nombramiento.Funcionario.EstadoFuncionario.PK_EstadoFuncionario == 1)
                        {
                            var estadoNombramiento = nombramiento.EstadoNombramiento.PK_EstadoNombramiento;
                            //El estado del nombramiento es: Propiedad,Nombramiento interino,Ascenso interino
                            if (estadoNombramiento == 1 || estadoNombramiento == 2 || estadoNombramiento == 9 || estadoNombramiento == 5)
                            {
                                nombramiento.CartasPresentacion.Add(carta);
                                entidadesBase.SaveChanges();
                                respuesta = new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = carta.PK_CartasPresentacion
                                };
                                return respuesta;
                            }
                            else
                            {
                                throw new Exception("El nombramiento no es valido.");
                            }
                        }
                        else
                        {
                            throw new Exception("El funcionario no tiene el estado valido.");
                        }
                    }
                    else
                    {
                        throw new Exception("Ya existe una carta con el número ingresado");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el nombramiento");
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

        public CRespuestaDTO BuscarCartaPresentacionNum(CartasPresentacion carta)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.CartasPresentacion.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                .Include("Nombramiento.Puesto").FirstOrDefault(C => C.NumCarta == carta.NumCarta);
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún estado de desarraigo");
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

        public CRespuestaDTO BuscarCartaPresentacionCedula(Funcionario funcionario) {
            CRespuestaDTO respuesta;
            try{
                var datosEntidad = entidadesBase.CartasPresentacion.Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario 
                                                                               == funcionario.IdCedulaFuncionario).FirstOrDefault();
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else {
                    throw new Exception("No se encontró cartas de presentación para este funcionario");
                }
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO BuscarCartaPresentacion(object[] parametros)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = FiltrarCartaPresentacion(parametros);
                if (datos.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
                    };
                    return respuesta;
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

        #endregion

    }
}

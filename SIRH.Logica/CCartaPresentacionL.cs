using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCartaPresentacionL
    {

        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCartaPresentacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCartaPresentacionDTO ConstruirCartaPresentacion(CartasPresentacion item)
        {
            return new CCartaPresentacionDTO
            {
                Nombramiento = CNombramientoL.NombramientoGeneral(item.Nombramiento),
                FechaCarta = Convert.ToDateTime(item.FecCarta),
                FechaInicio= Convert.ToDateTime(item.FecRige),
                FechaVencimiento= Convert.ToDateTime(item.FecVence),
                NumeroCarta = item.NumCarta
            };
        }

        internal static List<CCartaPresentacionDTO> ConstruirCartaPresentacion(IEnumerable<CartasPresentacion> items)
        {
            var respuesta = new List<CCartaPresentacionDTO>();
            foreach (var c in items)
            {
                respuesta.Add(ConstruirCartaPresentacion(c));
            }
            return respuesta; 
        }

        public CBaseDTO AgregarCartaPresentacion(CNombramientoDTO nombramiento, CCartaPresentacionDTO carta)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCartaPresentacionD intermedioCarta = new CCartaPresentacionD(contexto);
                
                var nombramientoDB = new Nombramiento { PK_Nombramiento = nombramiento.IdEntidad };
                var cartaDB = new CartasPresentacion
                {
                    FecCarta = carta.FechaCarta,
                    FecRige = carta.FechaInicio,
                    FecVence = carta.FechaVencimiento,
                    NumCarta = carta.NumeroCarta
                };

                var agregadoDesarraigo = intermedioCarta.AgregarCartaPresentacion(nombramientoDB, cartaDB);
                if (agregadoDesarraigo.Codigo < 0){
                    respuesta = (CErrorDTO)((CRespuestaDTO)agregadoDesarraigo).Contenido;
                }
            }
            catch
            {
                return respuesta;
            }
            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarCartaPresentacion(CFuncionarioDTO funcionario, CCartaPresentacionDTO carta,
                                     List<DateTime> fechasRige, List<DateTime> fechasVencimiento, List<DateTime> fechasCarta)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CCartaPresentacionD intermedio = new CCartaPresentacionD(contexto);
            List<CartasPresentacion> datosCartaPresentacion = new List<CartasPresentacion>();
            List<object> parametros = new List<object>();

            if (funcionario.Cedula != null)
            {
                parametros.Add("NumFuncionario");
                parametros.Add(funcionario.Cedula);
            }
            if (carta.NumeroCarta != null)
            {
                parametros.Add("NumCarta");
                parametros.Add(funcionario.Cedula);
            }
            if (fechasRige.Count == 2)
            {
                parametros.Add("FechaRige");
                parametros.Add(fechasRige);
            }
            if (fechasVencimiento.Count == 2)
            {
                parametros.Add("FechaFinal");
                parametros.Add(fechasVencimiento);
            }
            if (fechasCarta.Count == 2)
            {
                parametros.Add("FechaCarta");
                parametros.Add(fechasCarta);
            }
            datosCartaPresentacion = (List<CartasPresentacion>)intermedio.BuscarCartaPresentacion(parametros.ToArray()).Contenido;

            if (datosCartaPresentacion.Count > 0)
            {
                foreach (var item in datosCartaPresentacion)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(ConstruirCartaPresentacion(item));
                    temp.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Nombramiento.Funcionario));
                    temp.Add(CPuestoL.ConstruirPuesto(item.Nombramiento.Puesto, new CPuestoDTO()));
                    // lugar de contratació sale de ua carta de presentacion
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

        public List<CBaseDTO> ObtenerdatosCartaPresentacion(string numero)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CCartaPresentacionD intermedio = new CCartaPresentacionD(contexto);

                var carta = intermedio.BuscarCartaPresentacionNum(new CartasPresentacion { NumCarta = numero});

                if (carta.Codigo > 0)
                {
                    var datoCarta = ConstruirCartaPresentacion((CartasPresentacion)carta.Contenido);
                    respuesta.Add(datoCarta);

                    var funcionario = ((Desarraigo)carta.Contenido).Nombramiento.Funcionario;
                    var datoFuncionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(funcionario);

                    respuesta.Add(datoFuncionario);

                    var nombramiento = ((Desarraigo)carta.Contenido).Nombramiento;
                    var datosNombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(nombramiento);

                    respuesta.Add(datosNombramiento);

                    var puesto = ((Desarraigo)carta.Contenido).Nombramiento.Puesto;
                    var datosPuesto = CPuestoL.ConstruirPuesto(puesto, new CPuestoDTO());

                    respuesta.Add(datosPuesto);
                }
                else
                {
                    respuesta.Add((CErrorDTO)carta.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
            }
            return respuesta;
        }

        public CBaseDTO BuscarCartaPresentacionCedula(CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CCartaPresentacionD intermedio = new CCartaPresentacionD(contexto);
                var funcionarioBD = new Funcionario
                {
                    PK_Funcionario = funcionario.IdEntidad,
                    IdCedulaFuncionario = funcionario.Cedula
                };
                var carta = intermedio.BuscarCartaPresentacionCedula(funcionarioBD);

                if (carta.Codigo > 0)
                {
                    var datoCarta = ConstruirCartaPresentacion((CartasPresentacion)carta.Contenido);
                    respuesta = datoCarta;
                }
                else
                {
                    respuesta =(CErrorDTO)carta.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        #endregion
    }
}

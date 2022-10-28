using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEntidadEducativaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEntidadEducativaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos   
       //Insertado en CFormacionAcademicaService y ICFormacionAcademicaService 27/01/2017
        public CBaseDTO BuscarEntidadEducativa(int codigo)
        {
            CBaseDTO respuesta;
            try
            {
                //entidad educativa de datos será intermedio, la cual es un nuevo CEntidadEducativaD en la BD
                CEntidadEducativaD intermedio = new CEntidadEducativaD(contexto);
                //datos es igual a intermedio que va al método buscarEntidadEducativa por codigo
                var datos = intermedio.BuscarEntidadEducativa(codigo);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta = ConvertirEntidadEducativaADTO((EntidadEducativa)datos.Contenido);
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ha ocurrido en error inesperado");
                }
            }
            catch (Exception Error)
            {
                respuesta = new CErrorDTO { MensajeError = Error.Message };
                return respuesta;
            }
        }
        //Insertado en CFormacionAcademicaService y ICFormacionAcademicaService (Deivert)



        public List<CBaseDTO> BuscarEntidadEducativa(string nombre, int tipo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                //entidad educativa de datos será intermedio, la cual es un nuevo CEntidadEducativaD en la BD
                CEntidadEducativaD intermedio = new CEntidadEducativaD(contexto);
                //datos es igual a intermedio que va al método buscarEntidadEducativa por codigo
                var datos = intermedio.BuscarEntidadEducativa(nombre, tipo);

                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        foreach (EntidadEducativa item in (List<EntidadEducativa>)datos.Contenido)
                            respuesta.Add(ConvertirEntidadEducativaADTO(item));
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ha ocurrido en error inesperado");
                }
            }
            catch (Exception Error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = Error.Message });
                return respuesta;
            }
        }


        public List<CBaseDTO> ListarEntidadEducativa()
        {
            //la lista de CBaseDTO la respuesta estará en una nueva lista de CBaseDTO
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try 
            {
                //de CEntidadEducativaD del intermedio, va a ser una nueva CEntidadEduc en la BD.
                CEntidadEducativaD intermedio = new CEntidadEducativaD(contexto);
                // los Datos estan en el intermedio y de ahí que me liste las entidades educativas.
                var datos = intermedio.ListarEntidadEducativa();
                if(datos != null)
                {
                    if(datos.Codigo != -1)
                    {
                        foreach (var item in (List<EntidadEducativa>)datos.Contenido)
                        {
                            respuesta.Add(ConvertirEntidadEducativaADTO(item));
                        }

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception (((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
	            }
            }
	        catch (Exception error)
            {
                respuesta.Add (new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO GuardarEntidadEducativa(string nombre, int tipo)
        {
            CBaseDTO respuesta;
            try
            {
                //entidad educativa de datos será intermedio, la cual es un nuevo CEntidadEducativaD en la BD
                CEntidadEducativaD intermedio = new CEntidadEducativaD(contexto);
                //datos es igual a intermedio que va al método buscarEntidadEducativa por codigo
                var resultado = intermedio.GuardarEntidadEducativa(new EntidadEducativa { NomEntidad = nombre, TipEntidad = tipo, Estado = 1 });

                if (resultado.Codigo > 0)
                {
                    respuesta = new CEntidadEducativaDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception Error)
            {
                respuesta = new CErrorDTO { MensajeError = Error.Message };
                return respuesta;
            }
        }

        public CBaseDTO AnularEntidadEducativa(int id)
        {
            CBaseDTO respuesta;
            try
            {
                //entidad educativa de datos será intermedio, la cual es un nuevo CEntidadEducativaD en la BD
                CEntidadEducativaD intermedio = new CEntidadEducativaD(contexto);
                //datos es igual a intermedio que va al método buscarEntidadEducativa por codigo
                var resultado = intermedio.AnularEntidadEducativa(id);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CEntidadEducativaDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception Error)
            {
                respuesta = new CErrorDTO { MensajeError = Error.Message };
                return respuesta;
            }
        }

        internal static CEntidadEducativaDTO ConvertirEntidadEducativaADTO(EntidadEducativa entidadEducativa)
        {
            string nomTipo;
            switch (entidadEducativa.TipEntidad)
            {
                case 1: nomTipo = "Entidad Gubernamental / ONG"; break;
                case 2: nomTipo = "Entidad universitaria"; break;
                case 3: nomTipo = "Entidad técnica"; break;
                case 4: nomTipo = "Entidad básica (educación básica, primer y segundo ciclo)"; break;
                case 5: nomTipo = "Otro"; break;
                default: nomTipo = "No indica"; break;
            }
            return new CEntidadEducativaDTO
            {
                IdEntidad = entidadEducativa.PK_EntidadEducativa,
                DescripcionEntidad = entidadEducativa.NomEntidad,
                TipoEntidad = Convert.ToInt32(entidadEducativa.TipEntidad),
                Estado = Convert.ToInt32(entidadEducativa.Estado),
                NombreTipo = nomTipo
            };
        }
        #endregion
    }
}
 
        


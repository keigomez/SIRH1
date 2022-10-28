using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using System.Data.SqlClient;
using System.Data;

namespace SIRH.Datos
{
    public class CBitacoraUsuarioD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CBitacoraUsuarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO GuardarBitacora(BitacoraUsuario bitacora, string entidad)
        {
            try
            {
                bitacora.CodEntidad = ObtenerIDEntidad(entidad);
                entidadBase.BitacoraUsuario.Add(bitacora);

                entidadBase.SaveChanges();
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = bitacora.PK_BitacoraUsuario
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public CRespuestaDTO ListarBitacora(CBitacoraUsuarioDTO bitacora, List<DateTime> fechas)
        {
            CRespuestaDTO respuesta;

            try
            {
                DateTime paramFechaInicio = new DateTime();
                DateTime paramFechaFinal = new DateTime();

                var resultado = new List<BitacoraUsuario>();
                bool condicionFecha = false;

                if (fechas.Count > 0)
                {
                    paramFechaInicio = fechas.ElementAt(0);
                    paramFechaFinal = fechas.ElementAt(1).AddDays(1);
                    condicionFecha = true;
                }

                if (bitacora.CodigoModulo > 0)
                    resultado = entidadBase.BitacoraUsuario
                                           .Include("Usuario")
                                           .Include("Usuario.DetalleAcceso")
                                           .Include("Usuario.DetalleAcceso.Funcionario")
                                           .Where(q => q.CodModulo == bitacora.CodigoModulo)
                                           .ToList();
                else 
                    resultado = entidadBase.BitacoraUsuario
                                            .Include("Usuario")
                                            .Include("Usuario.DetalleAcceso")
                                            .Include("Usuario.DetalleAcceso.Funcionario")
                                            .ToList();

                if (bitacora.Usuario != null && bitacora.Usuario.IdEntidad > 0)
                    resultado = resultado.Where(q => q.Usuario.PK_Usuario == bitacora.Usuario.IdEntidad).ToList();

                if (bitacora.CodigoAccion > 0)
                    resultado = resultado.Where(q => q.CodAccion == bitacora.CodigoAccion).ToList();

                //if (bitacora.CodigoModulo > 0)
                //    resultado = resultado.Where(q => q.CodModulo == bitacora.CodigoModulo).ToList();

                if (condicionFecha)
                    resultado = resultado.Where(q => q.FecEjecucion >= paramFechaInicio && q.FecEjecucion < paramFechaFinal).ToList();

                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Bitácora"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        private int ObtenerIDEntidad(string entidad)
        {
            int resultado = 0;

            try
            {
                var dato = entidadBase.SeleccionarObjectID(entidad).ToList();
                resultado = Convert.ToInt32(dato[0]);
            }
            catch (Exception error)
            {
                resultado = -1;
            }   

            //DatosConexion str = new DatosConexion();
            //int resultado = 0;
            //SqlConnection conn = new SqlConnection(str.ConSIRH);
            ////SqlConnection conn = new SqlConnection(str.ConADABAS);
            //conn.Open();
            //SqlCommand commandSP = new SqlCommand("dbo.SeleccionarObjectID", conn);
            //commandSP.CommandType = CommandType.StoredProcedure;
            //commandSP.Parameters.Add("@entidad", SqlDbType.VarChar).Value = entidad;

            //var a = commandSP.ExecuteReader();
            //if (a.Read())
            //{
            //    resultado = Convert.ToInt32(a.GetValue(0));
            //}
            //conn.Close();
            return resultado;
        }


        #endregion
    }
}

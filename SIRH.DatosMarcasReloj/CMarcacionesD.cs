using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.DatosMarcasReloj
{

    public class CMarcacionesD
    {

        #region Variables

            private MasterTASEntities entidadBase = new MasterTASEntities();

        #endregion

        #region Constructor

        public CMarcacionesD(MasterTASEntities entidadGlobal) {
            entidadBase = entidadGlobal;        
        }

        #endregion

        #region Metodos

        private List<Marcaciones> FiltrarMarcaciones(object[] parametros)
        {
            List<Marcaciones> respuesta = new List<Marcaciones>();
            var datos = entidadBase.Marcaciones.AsQueryable();

            string elemento;
            for (int i = 0; i < parametros.Length && datos.Count() != 0; i = i + 2)
            {
                elemento = parametros[i].ToString();
                switch(elemento){
                    case "CodigoEmpleado":
                        var codigo = parametros[i + 1].ToString(); //Codigo de Acceso
                        datos = datos.Where(M => M.CodigoEmpleado == codigo);
                        break;
                    case "Fecha":
                        var fechaInicio = ((List<DateTime>) parametros[i + 1]).ElementAt(0);
                        var fechaFin = ((List<DateTime>) parametros[i + 1]).ElementAt(1);
                        datos = datos.Where(M => M.Fecha >= fechaInicio && M.Fecha <= fechaFin);
                        break;
                    case "Dispositivo":
                        var disposiitivo = ((short)parametros[i + 1]);
                        datos = datos.Where(M => M.IdDispositivo == disposiitivo);
                        break;
                    case "Procesado": //Para saber si el sistema viejo ya proceso o no la marcacion
                        var procesado = ((short)parametros[i + 1]);
                        datos = datos.Where(M => M.Procesado == procesado);
                        break;
                    case "IdMarcacion":
                        var id = ((decimal)parametros[i + 1]);
                        datos = datos.Where(M => M.IdMarcacion == id);
                        break;
                    default: 
                        throw new Exception("Busqueda no definida");
                }
            }
            return respuesta;    
        }

        public CRespuestaDTO BuscarMarcaciones(object [] parametros) {
            CRespuestaDTO respuesta;
            try{
                var datos = this.FiltrarMarcaciones(parametros);
                if (datos.Count > 0)
                {
                    respuesta = new CRespuestaDTO { 
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else 
                    throw new Exception("No se encontraron resultados para los parámetros de búsqueda establecidos");
            }catch(Exception error){
                respuesta = new CRespuestaDTO { 
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message } 
                };
            }
            return respuesta;
        }


        public CRespuestaDTO ReporteMarcacionesPorDia(DateTime fechaI, DateTime fechaF, Empleado empleado){
            CRespuestaDTO respuesta = new CRespuestaDTO();
            using (var contexto = new EmpresasDataDB1Entities()){
                try
                {
                    var empl = contexto.Empleado.FirstOrDefault(E => E.CodigoEmpleado == empleado.CodigoEmpleado);
                    empleado.CodigoAcceso = empl!=null?empl.CodigoAcceso:"xxxx";
                    if (empl != null){
                        var datos = entidadBase.Marcaciones
                                               .Where(M => M.CodigoEmpleado == empl.CodigoAcceso && 
                                                           M.Fecha >= fechaI && M.Fecha <= fechaF)
                                               .OrderBy(M => M.Fecha)
                                               .ThenBy(M => M.Hora)
                                               .ToList();
                        if (datos.Count > 0){
                            respuesta = new CRespuestaDTO{
                                Codigo = 1,
                                Contenido = datos
                            };
                        }
                        else
                            throw new Exception("No se encontró resultados para esta busqueda");
                    }
                    else
                        throw new Exception("No se encontró el empleado en Marcaciones");
                }
                catch (Exception error){
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = "No se encontró el empleado en Marcaciones" == error.Message ? -2 : -1,
                        Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                    };
                }
            }
            return respuesta;
        }

        public void ObtenerCodigoAcceso(Empleado empleado)
        {
            using (var contexto = new EmpresasDataDB1Entities())
            {
                try
                {
                    var empl = contexto.Empleado.FirstOrDefault(E => E.CodigoEmpleado == empleado.CodigoEmpleado);
                    empleado.CodigoAcceso = empl != null ? empl.CodigoAcceso : "xxxx";
                }
                catch
                {
                    empleado.CodigoAcceso = "xxxx";
                }
            }
        }

        #endregion
        
    }

}

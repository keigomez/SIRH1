using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CListaNombramientoActivoL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CListaNombramientoActivoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

      
        public CBaseDTO ActualizarLista()
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CListaNombramientoActivoD intermedio = new CListaNombramientoActivoD(contexto);

                var dato = intermedio.ActualizarLista();

                if (dato.Codigo > 0)
                    respuesta = dato;
                else
                    respuesta = ((CErrorDTO)dato.Contenido);
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

       
        public List<CBaseDTO> BuscarNombramientos(CListaNombramientosActivosDTO dato)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CListaNombramientoActivoD intermedio = new CListaNombramientoActivoD(contexto);
            CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
            CPuestoL intermedioPuesto = new CPuestoL();
            CFuncionarioL intermedioFuncionario = new CFuncionarioL();
            CFuncionarioD intermedioFD = new CFuncionarioD(contexto);
            CDetallePuestoD intermedioDP = new CDetallePuestoD(contexto);
            CSeccionL intermedioSeccion = new CSeccionL();
            CDepartamentoL intermedioDep = new CDepartamentoL();
            CDireccionGeneralL intermedioDir = new CDireccionGeneralL();
            CDivisionL intermedioDiv = new CDivisionL();

            List<ListaNombramientosActivos> datosNombramiento = new List<ListaNombramientosActivos>();

            bool busquedaAnterior = false;
            
            if (dato.Funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarNombramiento(datosNombramiento, dato.Funcionario.Cedula, "Cedula", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosNombramiento = (List<ListaNombramientosActivos>)resultado.Contenido;

                busquedaAnterior = true;
            }

            
                     
            if (datosNombramiento.Count > 0)
            {
                foreach (var item in datosNombramiento)
                {
                    var listado = new CListaNombramientosActivosDTO();

                    // Funcionario
                    var datoFuncionario = intermedioFuncionario.BuscarFuncionarioBase(item.Funcionario.IdCedulaFuncionario);
                    listado.Funcionario = (CFuncionarioDTO)datoFuncionario;

                    // Nombramiento
                    listado.Nombramiento = new CNombramientoDTO { IdEntidad = item.Nombramiento.PK_Nombramiento };
                    Nombramiento datoNombramiento = intermedioNombramiento.CargarNombramiento(item.Nombramiento.PK_Nombramiento);
                    CPuestoDTO puesto = new CPuestoDTO();
                    //Puesto
                    listado.Puesto = CPuestoL.ConstruirPuesto(datoNombramiento.Puesto, puesto);

                    // DetallePuesto
                    listado.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(intermedioDP.CargarDetallePuesto(item.DetallePuesto.PK_DetallePuesto));

                    // Seccion
                    if (item.PK_Seccion> 0)
                    {
                        var datoSeccion = intermedioSeccion.DescargarSeccions(item.PK_Seccion.Value, "");
                        listado.Seccion= datoSeccion.FirstOrDefault();
                    }
                    else
                    {
                        listado.Seccion = new CSeccionDTO { IdEntidad = 0, NomSeccion = "" };
                    }


                    // Departamento
                    if (item.PK_Departamento > 0)
                    {
                        var datoDep = intermedioDep.DescargarDepartamentos(item.PK_Departamento.Value, "");
                        listado.Departamento = datoDep.FirstOrDefault();
                    }
                    else
                    {
                        listado.Departamento = new CDepartamentoDTO { IdEntidad = 0, NomDepartamento = "" };
                    }

                    // DireccionGeneral
                    if (item.PK_Direccion > 0)
                    {
                        var datoDir = intermedioDir.DescargarDireccionGenerals(item.PK_Direccion.Value, "");
                        listado.DireccionGeneral = datoDir.FirstOrDefault();
                    }
                    else
                    {
                        listado.DireccionGeneral = new CDireccionGeneralDTO { IdEntidad = 0, NomDireccion = "" };
                    }

                    // División
                    var datoDiv = intermedioDiv.DescargarDivisions(item.PK_Division.Value, "");
                    listado.Division= datoDiv.FirstOrDefault();

                    listado.IndPropiedad = item.IndPropiedad;

                    respuesta.Add(listado);
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        #endregion
    }
}
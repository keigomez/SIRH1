using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDepartamentoL
    {
        //Cambio para subir a TFS
        #region Variables

        SIRHEntities contexto;
        CDepartamentoD departamentoDescarga;

        #endregion

        #region Constructor

        public CDepartamentoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CDepartamentoDTO ConstruirDepartamentoDTO(Departamento item)
        {
            return new CDepartamentoDTO
            {
                IdEntidad = item.PK_Departamento,
                IndEstDepartamento = item.IndEstadoDepartamento != null ? Convert.ToInt32(item.IndEstadoDepartamento) : 0,
                NomDepartamento = item.NomDepartamento
            };
        }

        public List<CDepartamentoDTO> DescargarDepartamentos(int codigo, string nombre)
        {
            List<CDepartamentoDTO> resultado = new List<CDepartamentoDTO>();
            departamentoDescarga = new CDepartamentoD(contexto);
            var departamento = departamentoDescarga.CargarDepartamentoesParam(codigo,nombre);
            {
                foreach (var item in departamento)
                {
                    CDepartamentoDTO temp = new CDepartamentoDTO();
                    temp.IdEntidad = item.PK_Departamento;
                    temp.NomDepartamento = item.NomDepartamento;
                    temp.IndEstDepartamento = Convert.ToInt32(item.IndEstadoDepartamento);
                    resultado.Add(temp);
                }
            }
            return resultado;
        }

        internal static CDepartamentoDTO ConvertirDepartamentoADTO(Departamento departamento)
        {
            throw new NotImplementedException();
        }

        public List<CBaseDTO> ListarDepartamentos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CDepartamentoD intermedio = new CDepartamentoD(contexto);
            var departamento = intermedio.CargarDepartamentos();
            if (departamento != null)
            {
                foreach (var item in departamento)
                {
                    respuesta.Add(new CDepartamentoDTO
                    {
                        NomDepartamento = item.NomDepartamento,
                        IndEstDepartamento = Convert.ToInt16(item.IndEstadoDepartamento),
                        IdEntidad = item.PK_Departamento
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron departamentos" });
            }
            return respuesta;
        }

        internal static Departamento ConvertirDepartamentoDTOaDatos(CDepartamentoDTO item)
        {
            return new Departamento
            {
                PK_Departamento = item.IdEntidad,
                NomDepartamento = item.NomDepartamento,
                IndEstadoDepartamento = item.IndEstDepartamento,


            };
        }

        #endregion

    }
}


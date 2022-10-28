using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Servicios
{
    // NOTE: If you change the class name "CEstadoBorradorService" here, you must also update the reference to "CEstadoBorradorService" in App.config.
    public class CEstadoBorradorService : ICEstadoBorradorService
    {
        public List<CBaseDTO> RetornarEstados()
        {
            CEstadoBorradorL respuesta = new CEstadoBorradorL();
            return respuesta.RetornarEstadosBorrador();
        }

        public List<CBaseDTO> ObtenerEstado(int codigo)
        {
            CEstadoBorradorL respuesta = new CEstadoBorradorL();
            return respuesta.ObtenerEstadoBorrador(codigo);
        }
    }
}
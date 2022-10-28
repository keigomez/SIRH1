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
    // NOTE: If you change the class name "CTipoAccionPersonalService" here, you must also update the reference to "CTipoAccionPersonalService" in App.config.
    public class CTipoAccionPersonalService : ICTipoAccionPersonalService
    {
        public List<CBaseDTO> RetornarTipos()
        {
            CTipoAccionPersonalL respuesta = new CTipoAccionPersonalL();
            return respuesta.RetornarTipos();
        }

        public List<CBaseDTO> ObtenerTipo(int codigo)
        {
            CTipoAccionPersonalL respuesta = new CTipoAccionPersonalL();
            return respuesta.ObtenerTipo(codigo);
        }

        public CBaseDTO AgregarTipo(CTipoAccionPersonalDTO tipo)
        {
            CTipoAccionPersonalL respuesta = new CTipoAccionPersonalL();
            return respuesta.AgregarTipo(tipo);
        }

        public CBaseDTO EditarTipo(CTipoAccionPersonalDTO tipo)
        {
            CTipoAccionPersonalL respuesta = new CTipoAccionPersonalL();
            return respuesta.EditarTipo(tipo);
        }
    }
}

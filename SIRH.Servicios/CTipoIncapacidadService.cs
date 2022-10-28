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
    // NOTE: If you change the class name "CTipoIncapacidadService" here, you must also update the reference to "CTipoIncapacidadService" in App.config.
    public class CTipoIncapacidadService : ICTipoIncapacidadService
    {
        public List<CBaseDTO> RetornarTiposIncapacidad()
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.RetornarTiposIncapacidad();
        }

        public List<CBaseDTO> ObtenerTipoIncapacidad(int codigo)
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.ObtenerTipoIncapacidad(codigo);
        }

        public CBaseDTO AgregarTipoIncapacidad(CTipoIncapacidadDTO tipo)
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.AgregarTipoIncapacidad(tipo);
        }

        public CBaseDTO EditarTipoIncapacidad(CTipoIncapacidadDTO tipo)
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.EditarTipoIncapacidad(tipo);
        }
    }
}
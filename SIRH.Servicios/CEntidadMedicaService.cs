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
    // NOTE: If you change the class name "CEntidadMedicaService" here, you must also update the reference to "CEntidadMedicaService" in App.config.
    public class CEntidadMedicaService : ICEntidadMedicaService
    {
        public List<CBaseDTO> RetornarEntidadesMedicas()
        {
            CEntidadMedicaL respuesta = new CEntidadMedicaL();
            return respuesta.RetornarEntidadesMedicas();
        }


        public List<CBaseDTO> ObtenerEntidadMedica(int codigo)
        {
            CEntidadMedicaL respuesta = new CEntidadMedicaL();
            return respuesta.ObtenerEntidadMedica(codigo);
        }

        public CBaseDTO GuardarEntidadMedica(CEntidadMedicaDTO entidad)
        {
            CEntidadMedicaL respuesta = new CEntidadMedicaL();
            return respuesta.AgregarEntidadMedica(entidad);
        }

        public CBaseDTO EditarEntidadMedica(CEntidadMedicaDTO entidad)
        {
            CEntidadMedicaL respuesta = new CEntidadMedicaL();
            return respuesta.EditarEntidadMedica(entidad);
        }
    }
}

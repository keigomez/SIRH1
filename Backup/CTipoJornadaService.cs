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
    // NOTE: If you change the class name "CTipoJornadaService" here, you must also update the reference to "CTipoJornadaService" in App.config.
    public class CTipoJornadaService : ICTipoJornadaService
    {
        public List<CBaseDTO> RegistrarJornadaFuncionario(CFuncionarioDTO funcionario,
                                                            CNombramientoDTO nombramiento,
                                                            CTipoJornadaDTO jornada)
        {
            CTipoJornadaL respuesta = new CTipoJornadaL();
            return respuesta.RegistrarJornadaFuncionario(funcionario, nombramiento, jornada);
        }

        public List<CBaseDTO> EditarJornadaFuncionario(CTipoJornadaDTO jornada)
        {
            CTipoJornadaL respuesta = new CTipoJornadaL();
            return respuesta.EditarJornadaFuncionario(jornada);
        }
    }
}

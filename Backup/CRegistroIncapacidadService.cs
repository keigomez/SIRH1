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
    // NOTE: If you change the class name "CRegistroIncapacidadService" here, you must also update the reference to "CRegistroIncapacidadService" in App.config.
    public class CRegistroIncapacidadService : ICRegistroIncapacidadService
    {
        public CBaseDTO GuardarRegistroIncapacidad(CFuncionarioDTO funcionario, CEntidadMedicaDTO entidad,
                                            CTipoIncapacidadDTO tipo, CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.AgregarRegistroIncapacidad(funcionario, entidad, tipo, registro);
        }


        public CBaseDTO EditarRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.EditarRegistroIncapacidad(registro);
        }


        public CBaseDTO AnularRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.AnularIncapacidad(registro);
        }


        public List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ObtenerRegistroIncapacidad(codigo);
        }


        public List<List<CBaseDTO>> BuscarRegistroIncapacidades(CFuncionarioDTO funcionario,
                                                                CRegistroIncapacidadDTO registro,
                                                                List<DateTime> fechasRige,
                                                                List<DateTime> fechasVence)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.BuscarRegistroIncapacidades(funcionario, registro, fechasRige, fechasVence);
        }

        public List<CBaseDTO> ListarTipoIncapacidad()
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();

            return respuesta.RetornarTiposIncapacidad();
        }

        public List<CBaseDTO> ListarEntidadMedica()
        {
            CEntidadMedicaL respuesta = new CEntidadMedicaL();

            return respuesta.RetornarEntidadesMedicas();
        }


        public List<List<CBaseDTO>> FuncionariosConIncapacidades()
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.FuncionariosConIncapacidades();
        }
    }
}
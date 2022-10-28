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
        public CBaseDTO GuardarRegistroIncapacidad(CFuncionarioDTO funcionario, CTipoIncapacidadDTO tipo, CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.AgregarRegistroIncapacidad(funcionario, tipo, registro);
        }

        public CBaseDTO VerificarFechasIncapacidad(CFuncionarioDTO funcionario, CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.VerificarFechasIncapacidad(funcionario, registro);
        }

        public CBaseDTO EditarRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.EditarRegistroIncapacidad(registro);
        }

        public CBaseDTO AprobarRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ModificarEstadoIncapacidad(registro, 2);
        }

        public CBaseDTO AnularRegistroIncapacidad(CRegistroIncapacidadDTO registro)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ModificarEstadoIncapacidad(registro, 3);
        }

        public List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ObtenerRegistroIncapacidad(codigo);
        }

        public List<CBaseDTO> ObtenerRegistroIncapacidadCodigo(string numIncapacidad)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ObtenerRegistroIncapacidadCodigo(numIncapacidad);
        }

        public List<CBaseDTO> ObtenerRegistroIncapacidadProrroga(string numCedula, int idEntidad, int idTipo, DateTime fecha)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ObtenerRegistroIncapacidadProrroga(numCedula, idEntidad, idTipo, fecha);
        }

        public List<List<CBaseDTO>> BuscarRegistroIncapacidades(CFuncionarioDTO funcionario,
                                                                CRegistroIncapacidadDTO registro,
                                                                CEntidadMedicaDTO entidadMedica,
                                                                CBitacoraUsuarioDTO bitacora,
                                                                List<DateTime> fechasRige,
                                                                List<DateTime> fechasVence,
                                                                List<DateTime> fechasBitacora)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.BuscarRegistroIncapacidades(funcionario, registro, entidadMedica, bitacora, fechasRige, fechasVence, fechasBitacora);
        }

        public List<CBaseDTO> ListarTipoIncapacidad()
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.RetornarTiposIncapacidad();
        }

        public List<CBaseDTO> ObtenerTipoIncapacidad(int codigo)
        {
            CTipoIncapacidadL respuesta = new CTipoIncapacidadL();
            return respuesta.ObtenerTipoIncapacidad(codigo);
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


        public CBaseDTO ObtenerSalarioBruto(string cedula)
        {
            CRegistroIncapacidadL respuesta = new CRegistroIncapacidadL();
            return respuesta.ObtenerSalarioBruto(cedula);
        }
    }
}
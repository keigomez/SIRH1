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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CManualCargosService" in both code and config file together.
    public class CManualCargosService : ICManualCargosService
    {
        public CBaseDTO RegistrarCargo(CCargoDTO cargo)
        {
            return new CCargoL().RegistrarCargo(cargo);
        }

        public CBaseDTO ObtenerCargo(int codigo)
        {
            return new CCargoL().ObtenerCargoPorCodigo(codigo);
        }

        public CBaseDTO RegistrarResultadosCargo(List<CResultadoCargoDTO> resultados) => new CCargoL().RegistroResultadosActividades(resultados);

        public CBaseDTO ResgistrarFactorCargo(CFactorClasificacionCargoDTO factor) => new CCargoL().RegistrarFactorCargo(factor);

        public CBaseDTO RegistrarRequerimientosEspecificos(CRequerimientoEspecificoCargoDTO requerimiento) => new CCargoL().RegistrarRequerimientosEspecificos(requerimiento);

        public CBaseDTO RegistrarCompetenciasTransversales(List<CCompetenciaTransversalCargoDTO> competencia) => new CCargoL().RegistrarCompetenciasTransversales(competencia);

        public CBaseDTO RegistrarCompetenciasGrupo(List<CCompetenciaGrupoOcupacionalDTO> competencia) => new CCargoL().RegistrarCompetenciasGrupo(competencia);

        public List<CBaseDTO> ObtenerResultadoCargo(int codigo) => new CCargoL().ObtenerResultadosCargo(codigo);

        public CBaseDTO EliminarResultado(int codigo) => new CCargoL().EliminarResultado(codigo);

        public CBaseDTO EliminarActividad(int codigo) => new CCargoL().EliminarActividad(codigo);

        public CBaseDTO ObtenerFactoresCargo(int codigo) => new CCargoL().ObtenerFactoresCargo(codigo);

        public CBaseDTO ObtenerRequerimientosEspecificos(int codigo) => new CCargoL().ObtenerRequerimientosEspecificos(codigo);

        public CBaseDTO ActualizarRequerimientoEspecifico(CRequerimientoEspecificoCargoDTO requerimiento) => new CCargoL().RegistrarRequerimientosEspecificos(requerimiento);

        public List<CBaseDTO> ObtenerCompetenciaTransversal(int codigo) => new CCargoL().ObtenerCompetenciasTransversales(codigo);

        public CBaseDTO EliminarCompetenciasTransversales(int codigo) => new CCargoL().EliminarCompetenciasTransversales(codigo);

        public List<CBaseDTO> ObtenerCompetenciaGrupo(int codigo) => new CCargoL().ObtenerCompetenciaGrupo(codigo);

        public CBaseDTO EliminarCompetenciaGrupo(int codigo) => new CCargoL().EliminarCompetenciaGrupo(codigo);

        public CBaseDTO EliminarComportamientoGrupo(int codigo) => new CCargoL().EliminarComportamientoGrupo(codigo);

        public CBaseDTO EliminarEvidenciaGrupo(int codigo) => new CCargoL().EliminarEvidenciaGrupo(codigo);
    }
}

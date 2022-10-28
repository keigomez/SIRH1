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
    // NOTE: If you change the class name "CNombramientoService" here, you must also update the reference to "CNombramientoService" in App.config.
    public class CNombramientoService : ICNombramientoService
    {
        public List<CBaseDTO> DescargarCalificacionesCedula(string cedula)
        {
            CCalificacionNombramientoL logica = new CCalificacionNombramientoL();
            return logica.DescargarCalificacionesCedula(cedula);
        }

        public CNombramientoDTO DescargarNombramiento(string cedula)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.DescargarNombramiento(cedula);
        }

        public CBaseDTO GuardarNombramiento(CNombramientoDTO nombramiento)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.GuardarNombramiento(nombramiento);
        }

        public CBaseDTO CrearNombramientoInicial(CNombramientoDTO entidad)
        {
            CBaseDTO respuesta = new CBaseDTO();

            CNombramientoL nombramiento = new CNombramientoL();

            respuesta = nombramiento.CrearNombramientoInicial(entidad);

            return respuesta;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using SIRH.Web.Helpers;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class DetallePuestoVM
    {
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionTrabajo { get; set; }
        public EAccionesPuestoHelper Accion { get; set; }

        //Esta propiedad es de uso opcional, solo para puestos que estén vacantes
        public CPedimentoPuestoDTO PedimentoPuesto { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }
        public CUbicacionAdministrativaDTO UbicacionPuesto { get; set; }

        //Campos necesarios para el manejo de SetPuestoCaucion

        public string Justificacion { get; set; }
        public string OficioNotificacion { get; set; }

        public string mensaje { get; set; }

        //Campos necesarios para editar el puesto

        public int NivelSeleccionado { get; set; }

        public SelectList ListaNivelOcupacional { get; set; }

        //Controles para ubicaciones de contrato y trabajo

        public SelectList DistritosContrato { get; set; }
        public string DistritoContratoSeleccionado { get; set; }

        public SelectList CantonesContrato { get; set; }
        public string CantonContratoSeleccionado { get; set; }

        public SelectList ProvinciasContrato { get; set; }
        public string ProvinciaContratoSeleccionada { get; set; }

        public SelectList DistritosTrabajo { get; set; }
        public string DistritoTrabajoSeleccionado { get; set; }

        public SelectList CantonesTrabajo { get; set; }
        public string CantonTrabajoSeleccionado { get; set; }

        public SelectList ProvinciasTrabajo { get; set; }
        public string ProvinciaTrabajoSeleccionada { get; set; }
    }
}

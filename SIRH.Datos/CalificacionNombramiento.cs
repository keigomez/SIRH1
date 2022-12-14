
//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SIRH.Datos
{

using System;
    using System.Collections.Generic;
    
public partial class CalificacionNombramiento
{

    public CalificacionNombramiento()
    {

        this.DetalleCalificacionModificada = new HashSet<DetalleCalificacionModificada>();

        this.DetalleCalificacion = new HashSet<DetalleCalificacion>();

    }


    public int PK_CalificacionNombramiento { get; set; }

    public int FK_Calificacion { get; set; }

    public int FK_Nombramiento { get; set; }

    public int FK_PeriodoCalificacion { get; set; }

    public string UsrEvaluador { get; set; }

    public System.DateTime FecCreacion { get; set; }

    public int IndEstado { get; set; }

    public string ObsGeneral { get; set; }

    public string ObsCapacitacion { get; set; }

    public string ObsJustificacionCapacitacion { get; set; }

    public int IdJefeInmediato { get; set; }

    public int IdJefeSuperior { get; set; }

    public int IndFormulario { get; set; }

    public int IndRatificado { get; set; }

    public int IndEntregado { get; set; }

    public int IndConformidad { get; set; }

    public Nullable<System.DateTime> FecRatificacion { get; set; }



    public virtual Calificacion Calificacion { get; set; }

    public virtual PeriodoCalificacion PeriodoCalificacion { get; set; }

    public virtual Nombramiento Nombramiento { get; set; }

    public virtual ICollection<DetalleCalificacionModificada> DetalleCalificacionModificada { get; set; }

    public virtual ICollection<DetalleCalificacion> DetalleCalificacion { get; set; }

}

}

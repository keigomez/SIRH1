
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
    
public partial class DeduccionTemporal
{

    public int PK_DeduccionTemporal { get; set; }

    public Nullable<decimal> IndDias { get; set; }

    public Nullable<decimal> IndHoras { get; set; }

    public string NumDocumento { get; set; }

    public string IndExplicacion { get; set; }

    public Nullable<System.DateTime> FecRige { get; set; }

    public Nullable<System.DateTime> FecVence { get; set; }

    public Nullable<decimal> MtoDeduccion { get; set; }

    public Nullable<int> IndMesProceso { get; set; }

    public string IndPeriodo { get; set; }

    public Nullable<System.DateTime> FecActualizacion { get; set; }

    public int FK_TipoDeduccionTemporal { get; set; }

    public int FK_Nombramiento { get; set; }

    public int IndEstado { get; set; }



    public virtual Nombramiento Nombramiento { get; set; }

    public virtual TipoDeduccionTemporal TipoDeduccionTemporal { get; set; }

}

}

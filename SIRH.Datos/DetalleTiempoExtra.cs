
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
    
public partial class DetalleTiempoExtra
{

    public int PK_DetalleTiempoExtra { get; set; }

    public Nullable<System.DateTime> FecRegistro { get; set; }

    public Nullable<System.DateTime> FecInicio { get; set; }

    public Nullable<System.DateTime> FecFin { get; set; }

    public string IndHoraInicio { get; set; }

    public string IndHoraFin { get; set; }

    public Nullable<int> FK_TipoExtra { get; set; }

    public Nullable<int> FK_RegistroTiempoExtra { get; set; }

    public Nullable<int> IndEstado { get; set; }

    public string MtoH0 { get; set; }

    public string MtoH1 { get; set; }

    public string MtoH2 { get; set; }



    public virtual RegistroTiempoExtra RegistroTiempoExtra { get; set; }

    public virtual TipoExtra TipoExtra { get; set; }

}

}

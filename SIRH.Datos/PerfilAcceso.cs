
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
    
public partial class PerfilAcceso
{

    public int PK_PerfilAcceso { get; set; }

    public int FK_DetalleAcceso { get; set; }

    public Nullable<System.DateTime> FecAsignacion { get; set; }

    public Nullable<int> FK_CatPermiso { get; set; }



    public virtual CatPermiso CatPermiso { get; set; }

    public virtual DetalleAcceso DetalleAcceso { get; set; }

}

}
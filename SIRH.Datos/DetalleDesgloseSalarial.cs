
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
    
public partial class DetalleDesgloseSalarial
{

    public int PK_DetalleDesgloceSalarial { get; set; }

    public int FK_DesgloseSalarial { get; set; }

    public Nullable<int> FK_ComponenteSalarial { get; set; }

    public Nullable<decimal> MtoComponenteSalarial { get; set; }

    public Nullable<decimal> MtoPagocomponenteSalarial { get; set; }



    public virtual ComponenteSalarial ComponenteSalarial { get; set; }

    public virtual DesgloseSalarial DesgloseSalarial { get; set; }

}

}

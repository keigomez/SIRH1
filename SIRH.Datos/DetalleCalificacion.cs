
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
    
public partial class DetalleCalificacion
{

    public int PK_DetalleCalificacion { get; set; }

    public int FK_CatalogoPregunta { get; set; }

    public int FK_CalificacionNombramiento { get; set; }

    public string NumNotasPregunta { get; set; }



    public virtual CalificacionNombramiento CalificacionNombramiento { get; set; }

    public virtual CatalogoPregunta CatalogoPregunta { get; set; }

}

}

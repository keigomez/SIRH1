
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
    
public partial class TipoDetallePagoViatico
{

    public TipoDetallePagoViatico()
    {

        this.DetallePagoViaticoCorrido = new HashSet<DetallePagoViaticoCorrido>();

        this.DetallePagoViaticoRetroactivo = new HashSet<DetallePagoViaticoRetroactivo>();

    }


    public int PK_TipoDetallePagoViatico { get; set; }

    public string DesTipoDetalle { get; set; }



    public virtual ICollection<DetallePagoViaticoCorrido> DetallePagoViaticoCorrido { get; set; }

    public virtual ICollection<DetallePagoViaticoRetroactivo> DetallePagoViaticoRetroactivo { get; set; }

}

}

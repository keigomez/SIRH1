
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
    
public partial class TipoDetalleGastoTransporte
{

    public TipoDetalleGastoTransporte()
    {

        this.DetallePagoGastoTransporte = new HashSet<DetallePagoGastoTransporte>();

    }


    public int PK_TipoDetallePagoGasto { get; set; }

    public string DesTipoDetalle { get; set; }



    public virtual ICollection<DetallePagoGastoTransporte> DetallePagoGastoTransporte { get; set; }

}

}

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
    
public partial class GastoTransporteRutas
{

    public GastoTransporteRutas()
    {

        this.DetalleAsignacionGastoTransporte = new HashSet<DetalleAsignacionGastoTransporte>();

    }


    public int PK_Ruta { get; set; }

    public int FK_GastoTransporte { get; set; }

    public int FK_Estado { get; set; }

    public System.DateTime FecRige { get; set; }

    public System.DateTime FecVence { get; set; }

    public decimal MonGasto { get; set; }



    public virtual ICollection<DetalleAsignacionGastoTransporte> DetalleAsignacionGastoTransporte { get; set; }

    public virtual EstadoGastoTransporte EstadoGastoTransporte { get; set; }

    public virtual GastoTransporte GastoTransporte { get; set; }

}

}

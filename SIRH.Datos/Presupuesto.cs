
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
    
public partial class Presupuesto
{

    public Presupuesto()
    {

        this.Desarraigo = new HashSet<Desarraigo>();

        this.GastoTransporte = new HashSet<GastoTransporte>();

        this.RegistroTiempoExtra = new HashSet<RegistroTiempoExtra>();

        this.UbicacionAdministrativa = new HashSet<UbicacionAdministrativa>();

        this.ViaticoCorrido = new HashSet<ViaticoCorrido>();

    }


    public int PK_Presupuesto { get; set; }

    public Nullable<int> FK_Programa { get; set; }

    public string IdPresupuesto { get; set; }

    public string CodArea { get; set; }

    public string CodActividad { get; set; }

    public string IdUnidadPresupuestaria { get; set; }

    public string IdDireccionPresupuestaria { get; set; }



    public virtual ICollection<Desarraigo> Desarraigo { get; set; }

    public virtual ICollection<GastoTransporte> GastoTransporte { get; set; }

    public virtual Programa Programa { get; set; }

    public virtual ICollection<RegistroTiempoExtra> RegistroTiempoExtra { get; set; }

    public virtual ICollection<UbicacionAdministrativa> UbicacionAdministrativa { get; set; }

    public virtual ICollection<ViaticoCorrido> ViaticoCorrido { get; set; }

}

}
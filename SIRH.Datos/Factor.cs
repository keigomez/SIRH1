
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
    
public partial class Factor
{

    public Factor()
    {

        this.CaracteristicasPuesto = new HashSet<CaracteristicasPuesto>();

    }


    public int PK_Factor { get; set; }

    public Nullable<int> FK_TituloFactor { get; set; }

    public string DesFactor { get; set; }



    public virtual ICollection<CaracteristicasPuesto> CaracteristicasPuesto { get; set; }

    public virtual TituloFactor TituloFactor { get; set; }

}

}

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
    
public partial class CaracteristicasPuesto
{

    public int PK_CaracteristicasPuesto { get; set; }

    public Nullable<int> IndEscala { get; set; }

    public string DesCaracteristica { get; set; }

    public Nullable<int> FK_Puesto { get; set; }

    public Nullable<int> FK_Factor { get; set; }



    public virtual Factor Factor { get; set; }

    public virtual Puesto Puesto { get; set; }

}

}

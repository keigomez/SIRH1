
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
    
public partial class TareasPuesto
{

    public int PK_TareasPuesto { get; set; }

    public string DesQueHace { get; set; }

    public string DesComoLoHace { get; set; }

    public Nullable<int> IndFrecuencia { get; set; }

    public Nullable<int> FK_Puesto { get; set; }



    public virtual Puesto Puesto { get; set; }

}

}

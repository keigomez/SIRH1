using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{

    [DataContract(IsReference = true)]
    public class CExpedienteFuncionarioDTO : CBaseDTO
    {

        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }

        public List<CTomoDTO> listaTomos { get; set; }


        [DataMember]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Estado")]
        public EstadoEnum Estado{ get; set; }

        [DataMember]
        [DisplayName("Fecha de Traslado a Archivo Central")]
        public DateTime FechaTrasladoArchivoCentral { get; set; }

        [DataMember]
        [DisplayName("Número de Expediente")]
        public int NumeroExpediente { get; set; }

        [DataMember]
        [DisplayName("Número de Expediente en Archivo Central")]
        public string NumeroExpedienteEnArchivo { get; set; }

        [DataMember]
        [DisplayName("Número de Caja")]
        public string NumeroCaja { get; set; }






        /*Campos específicos para la búsqueda de Expediente*/
        [DataMember]
        [DisplayName("Filtro de búsqueda")]
        public string FiltroBusqueda { get; set; }

        [DataMember]
        [Required]
        [DisplayName("Dato de búsqueda")]
        public string DatoABuscar { get; set; }

        [DataMember]
        [DisplayName("Fecha Inicio")]
        public string FechaInicio { get; set; }

        [DataMember]
        [DisplayName("Fecha Fin")]
        public string FechaFin { get; set; }
       /*--------------------------------------------------*/
    }

}

[DataContract]
public enum EstadoEnum
{
    [EnumMember]
    TrasladadoArchivoCentral = 2,
    [EnumMember]
    Prestado = 1,
    [EnumMember]
    NoPrestado = 0,
    [EnumMember]
    NoDefinido = -1
}
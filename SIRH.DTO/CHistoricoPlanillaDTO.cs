using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CHistoricoPlanillaDTO : CBaseDTO
    {
        /*********** DATOS GENERALES ********/
        [DataMember]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [DataMember]
        [DisplayName("Fecha inicio")]
        public DateTime FechaInicio { get; set; }

        [DataMember]
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }

        [DataMember]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DataMember]
        [DisplayName("Puesto")]
        public string NumeroPuesto { get; set; }

        [DataMember]
        [DisplayName("Código Presupuestario")]
        public string CodigoPresupuestario { get; set; }

        [DataMember]
        [DisplayName("Categoria")]
        public string Categoria { get; set; }

        [DataMember]
        [DisplayName("Clase")]
        public string ClasePuesto { get; set; }

        [DataMember]
        [DisplayName("Fecha periodo")]
        public string FechaPeriodo { get; set; }

        [DataMember]
        [DisplayName("Fecha corrida")]
        public string FechaCorrida { get; set; }

        /*********** DATOS DE MONTOS ********/

        [DataMember]
        [DisplayName("Salario base")]
        public string SalarioBase { get; set; }

        [DataMember]
        [DisplayName("Salario quincenal")]
        public string SalarioQuincenal { get; set; }

        [DataMember]
        [DisplayName("Aumentos")]
        public string Aumentos { get; set; }

        [DataMember]
        [DisplayName("Carrera profesional")]
        public string Grupo { get; set; }

        [DataMember]
        [DisplayName("Sobresueldos")]
        public string Sobresueldos { get; set; }

        [DataMember]
        [DisplayName("Prohibicion")]
        public string Prohibicion { get; set; }

        [DataMember]
        [DisplayName("Dedicación exclusiva")]
        public string DedicacionExclusiva { get; set; }

        [DataMember]
        [DisplayName("Peligrosidad")]
        public string Peligrosidad { get; set; }

        [DataMember]
        [DisplayName("Quinquenio")]
        public string Quinquenio { get; set; }

        [DataMember]
        [DisplayName("Disponibilidad")]
        public string Disponibilidad { get; set; }

        [DataMember]
        [DisplayName("Grado académico")]
        public string GradoAcademico { get; set; }

        [DataMember]
        [DisplayName("Riesgo policial")]
        public string RiesgoPolicial { get; set; }

        [DataMember]
        [DisplayName("Curso básico")]
        public string CursoBasico { get; set; }

        [DataMember]
        [DisplayName("Instrucción policial")]
        public string InstruccionPolicial { get; set; }

        [DataMember]
        [DisplayName("Bonificación")]
        public string Bonificacion { get; set; }

        [DataMember]
        [DisplayName("Consulta externa")]
        public string ConsultaExterna { get; set; }

        [DataMember]
        [DisplayName("Desarraigo")]
        public string PagoDesarraigo { get; set; }

        [DataMember]
        [DisplayName("Otros incentivos")]
        public string OtrosIncentivos { get; set; }

        [DataMember]
        [DisplayName("Salario mensual")]
        public string SalarioMensual { get; set; }

        /*********** PORCENTAJES Y PUNTOS ********/

        [DataMember]
        [DisplayName("Puntos")]
        public string Puntos { get; set; }

        [DataMember]
        [DisplayName("Porcentaje dedicación exclusiva")]
        public string PorDedicacionExclusiva { get; set; }

        [DataMember]
        [DisplayName("Porcentaje peligrosidad")]
        public string PorPeligrosidad { get; set; }

        [DataMember]
        [DisplayName("Porcentaje prohibición")]
        public string PorProhibicion { get; set; }

        [DataMember]
        [DisplayName("Porcentaje quinquenio")]
        public string PorQuinquenio { get; set; }

        [DataMember]
        [DisplayName("Porcentaje disponibilidad")]
        public string PorDisponibilidad { get; set; }

        [DataMember]
        [DisplayName("Porcentaje grado académico")]
        public string PorGradoAcademico { get; set; }

        [DataMember]
        [DisplayName("Porcentaje riesgo policial")]
        public string PorRiesgoPolicial { get; set; }

        [DataMember]
        [DisplayName("Porcentaje curso básico")]
        public string PorCursoBasico { get; set; }

        [DataMember]
        [DisplayName("Porcentaje instrucción policial")]
        public string PorInstruccionPolicial { get; set; }

        [DataMember]
        [DisplayName("Porcentaje bonificación")]
        public string PorBonificacion { get; set; }

        [DataMember]
        [DisplayName("Porcentaje consulta externa")]
        public string PorConsultaExterna { get; set; }
    }
}

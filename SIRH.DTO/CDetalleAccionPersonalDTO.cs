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
    public class CDetalleAccionPersonalDTO : CBaseDTO
    {
        [DataMember]
        public CAccionPersonalDTO Accion { get; set; }
        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }
        [DataMember]
        public CDetallePuestoDTO DetallePuestoAnterior { get; set; }

        [DataMember]
        [DisplayName("Código Nombramiento")]
        public int CodNombramiento { get; set; }
        [DataMember]
        [DisplayName("Código Programa")]
        public int CodPrograma { get; set; }
        [DataMember]
        [DisplayName("Código Seccion")]
        public int CodSeccion { get; set; }
        [DataMember]
        [DisplayName("Código Especialidad")]
        public int CodEspecialidad{ get; set; }
        [DataMember]
        [DisplayName("Código Subespecialidad")]
        public int CodSubespecialidad { get; set; }
        [DataMember]
        [DisplayName("Código Detalle Puesto")]
        public int CodDetallePuesto { get; set; }
        [DataMember]
        [DisplayName("Horas")]
        public int NumHoras { get; set; }

        [DataMember]
        [DisplayName("Código Clase")]
        public int CodClase { get; set; }

        [DataMember]
        [DisplayName("Núm. Puesto")]
        public string CodPuesto { get; set; }

        //[DataMember]
        //[DisplayName("Número de Horas")]
        //public int NumHoras { get; set; }

        [DataMember]
        [DisplayName("Anualidades")]
        public int NumAnualidad { get; set; }

        [DataMember]
        [DisplayName("Mes Aumento")]
        public int MesAumento { get; set; }

        [DataMember]
        [DisplayName("Categoría")]
        public int IndCategoria { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Salario Base")]
        public decimal MtoSalarioBase { get; set; }

        [DataMember]
        [DisplayName("Monto Anualidad")]
        public decimal MtoAnual { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public decimal MtoAumentosAnuales { get; set; }

        [DataMember]
        [DisplayName("Aumentos Acumulados")]
        public decimal MtoAumentosAnualesAcum { get; set; }
        
        [DataMember]
        [DisplayName("Riesgo")]
        public decimal PorRiesgo { get; set; }

        [DataMember]
        [DisplayName("Disponibilidad")]
        public decimal PorDisponibilidad { get; set; }

        [DataMember]
        [DisplayName("Peligrosidad")]
        public decimal PorPeligrosidad { get; set; }

        [DataMember]
        [DisplayName("Consulta Externa")]
        public decimal PorConsulta { get; set; }

        [DataMember]
        [DisplayName("Bonificación Adicional")]
        public decimal PorBonificacion { get; set; }

        [DataMember]
        [DisplayName("Curso Básico")]
        public decimal PorCurso { get; set; }

        [DataMember]
        [DisplayName("Carrera Policial")]
        public decimal PorCarreraPolicial { get; set; }

        [DataMember]
        [DisplayName("R.E.F.J.")]
        public decimal PorRiesgoJudicial { get; set; }

        [DataMember]
        [DisplayName("R.E.F.J.")]
        public decimal MtoRiesgoJudicial { get; set; }

        [DataMember]
        [DisplayName("I.C.S.")]
        public decimal PorIndCompetitividad { get; set; }

        [DataMember]
        [DisplayName("I.C.S.")]
        public decimal MtoIndCompetitividad { get; set; }

        [DataMember]
        [DisplayName("Grado Policial")]
        public decimal PorGradoPolicial { get; set; }

        [DataMember]
        [DisplayName("Grado Policial")]
        public decimal MtoGradoPolicial { get; set; }

        [DataMember]
        [DisplayName("Instrucción Policial")]
        public decimal PorInstruccionPolicial { get; set; }

        [DataMember]
        [DisplayName("Instrucción Policial")]
        public decimal MtoInstruccionPolicial { get; set; }

        [DataMember]
        [DisplayName("Laudo")]
        public decimal MtoLaudo { get; set; }     

        [DataMember]
        [DisplayName("Recargo de Funciones")]
        public decimal MtoRecargo { get; set; }

        [DataMember]
        [DisplayName("Valor Punto")]
        public decimal MtoPunto { get; set; }

        [DataMember]
        [DisplayName("Carrera Profesional")]
        public decimal NumGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Monto Carrera")]
        public decimal MtoGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Prohibición")]
        public decimal PorProhOriginal { get; set; }

        [DataMember]
        [DisplayName("Prohibición")]
        public decimal PorProhibicion { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public decimal MtoProhibicion { get; set; }

        [DataMember]
        [DisplayName("Quinquenio")]
        public decimal PorQuinquenio { get; set; }

        [DataMember]
        [DisplayName("Desarraigo")]
        public decimal PorDesarraigo { get; set; }

        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public decimal MtoOtros { get; set; }

        [DataMember]
        [DisplayName("Total")]
        public decimal MtoTotal { get; set; }

        [DataMember]
        [DisplayName("Total")]
        public decimal MtoTotalNuevo { get; set; }

        [DataMember]
        [DisplayName("Total")]
        public decimal MtoTotalAnterior { get; set; }

        [DataMember]
        [DisplayName("Anualidades")]
        public int NumAnualidadAnterior { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public decimal MtoAumentosAnualesAnterior { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public decimal MtoProhibicionAnterior { get; set; }

        [DataMember]
        [DisplayName("Riesgo")]
        public decimal PorRiesgoAnterior { get; set; }

        [DataMember]
        [DisplayName("Disponibilidad")]
        public decimal PorDisponibilidadAnterior { get; set; }

        [DataMember]
        [DisplayName("Peligrosidad")]
        public decimal PorPeligrosidadAnterior { get; set; }

        [DataMember]
        [DisplayName("Consulta Externa")]
        public decimal PorConsultaAnterior { get; set; }

        [DataMember]
        [DisplayName("Bonificación Adicional")]
        public decimal PorBonificacionAnterior { get; set; }

        [DataMember]
        [DisplayName("Carrera Policial")]
        public decimal PorCarreraPolicialAnterior { get; set; }

        [DataMember]
        [DisplayName("Curso Básico")]
        public decimal PorCursoAnterior { get; set; }

        [DataMember]
        [DisplayName("Quinquenio")]
        public decimal PorQuinquenioAnterior { get; set; }

        [DataMember]
        [DisplayName("Grado Policial")]
        public decimal PorGradoPolicialAnterior { get; set; }

        [DataMember]
        [DisplayName("Grado Policial")]
        public decimal MtoGradoPolicialAnterior { get; set; }

        [DataMember]
        [DisplayName("Instrucción Policial")]
        public decimal PorInstruccionPolicialAnterior { get; set; }

        [DataMember]
        [DisplayName("Instrucción Policial")]
        public decimal MtoInstruccionPolicialAnterior { get; set; }

        [DataMember]
        [DisplayName("R.E.F.J.")]
        public decimal PorRiesgoJudicialAnterior { get; set; }

        [DataMember]
        [DisplayName("R.E.F.J.")]
        public decimal MtoRiesgoJudicialAnterior { get; set; }

        [DataMember]
        [DisplayName("I.C.S.")]
        public decimal PorIndCompetitividadAnterior { get; set; }

        [DataMember]
        [DisplayName("I.C.S.")]
        public decimal MtoIndCompetitividadAnterior { get; set; }


        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public decimal MtoOtrosAnterior{ get; set; }

        [DataMember]
        [DisplayName("Desarraigo")]
        public decimal PorDesarraigoAnterior { get; set; }
    }
}
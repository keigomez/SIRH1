using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class ManualCargosVM
    {
        // 1ERA PARTE
        //MODIFICAR CON CCARGODTO
        public SelectList Estratos { get; set; }

        public CCargoDTO Cargo { get; set; }

        public CErrorDTO Error { get; set; }

        public List<CResultadoCargoDTO> Resultados { get; set; }

        public CFactorClasificacionCargoDTO Factor { get; set; }

        public CRequerimientoEspecificoCargoDTO RequerimientoEspecifico { get; set; }

        public List<CCompetenciaTransversalCargoDTO> CompetenciasTransversales { get; set; }

        public List<CCompetenciaGrupoOcupacionalDTO> CompetenciasGrupo { get; set; }

        //SEGUNDA PARTE

        [DisplayName("Actividad 1")]
        public string Actividad1Resultado1 { get; set; }

        [DisplayName("Actividad 1")]
        public string Actividad1Resultado2 { get; set; }

        [DisplayName("Actividad 1")]
        public string Actividad1Resultado3 { get; set; }

        //TERCERA PARTE

        [DisplayName("Independencia")]
        public string Independencia { get; set; }

        [DisplayName("Supervisión ejercida")]
        public string Supervision { get; set; }

        [DisplayName("Lugares")]
        public string Lugares { get; set; }

        [DisplayName("Ambiente")]
        public string Ambiente { get; set; }

        [DisplayName("Condiciones")]
        public string Condiciones { get; set; }

        [DisplayName("Modalidad de trabajo")]
        public string Modalidad { get; set; }

        [DisplayName("Impacto de la gestión")]
        public string Impacto { get; set; }

        [DisplayName("Relaciones de trabajo")]
        public string Relaciones { get; set; }

        [DisplayName("Activos, equipos, insumos")]
        public string Activos { get; set; }

        //CUARTA PARTE
        [DisplayName("Requisitos específicos")]
        public string RequisitosEspecificos { get; set; }

        [DisplayName("Conocimientos deseables, ideales o necesarios para el cargo")]
        public string Conocimientos { get; set; }

        //QUINTA PARTE

        [DisplayName("Nombre de la competencia")]
        public string NomCompetencia { get; set; }

        [DisplayName("Tipo de Competencia")]
        public SelectList TipoCompetencia { get; set; }

        [DisplayName("Nivel de dominio")]
        public SelectList NivelDominio { get; set; }

        //SEXTA PARTE

        [DisplayName("Comportamiento")]
        public SelectList Comportamiento { get; set; }

        [DisplayName("Descripcion")]
        public string Descripcion { get; set; }
    }
}
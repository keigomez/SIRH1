using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CPuntosDTO : CBaseDTO
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public decimal TotalPuntos { get; set; }
        [DataMember]
        public int ValorPunto { get; set; }
        [DataMember]
        public int AnnoExpPR { get; set; }
        [DataMember]
        public int Aprovechamiento { get; set; }
        [DataMember]
        public int Participacion { get; set; }
        [DataMember]
        public int ParticipacionInstruccion { get; set; }
        [DataMember]
        public string CalificacionServicio { get; set; }
        [DataMember]
        public decimal HorasAprovechamiento { get; set; }
        [DataMember]
        public decimal HorasParticipacion { get; set; }
        [DataMember]
        public decimal HorasInstruccion { get; set; }
        [DataMember]
        public decimal PtosGrado { get; set; }
        [DataMember]
        public decimal PtosAprovechamiento { get; set; }
        [DataMember]
        public decimal PtosExpPR { get; set; }
        [DataMember]
        public decimal PtosParticipacion { get; set; }
        [DataMember]
        public decimal PtosInstruccion { get; set; }
        [DataMember]
        public decimal PtosOtros { get; set; }
        [DataMember]
        public string ExplOtros { get; set; }
        [DataMember]
        public string Observacion { get; set; }
        [DataMember]
        public int MesCarrera { get; set; }
        [DataMember]
        public int AnnoCarrera { get; set; }
        [DataMember]
        public DateTime FecRigePago { get; set; }
        [DataMember]
        public decimal HsExcPar { get; set; }
        [DataMember]
        public decimal HsExcApr { get; set; }
        [DataMember]
        public string NumResolucion { get; set; }
        [DataMember]
        public DateTime FecResolucion { get; set; }
        [DataMember]
        public string Periodo { get; set; }
        [DataMember]
        public string Grado { get; set; }
        [DataMember]
        public string Nivel { get; set; }
        [DataMember]
        public decimal AnnoExpEst { get; set; }
        [DataMember]
        public decimal AprovechamientoEst { get; set; }
        [DataMember]
        public decimal ParticipacionEst { get; set; }
        [DataMember]
        public decimal HoraAprEst { get; set; }
        [DataMember]
        public decimal HoraParEst { get; set; }
        [DataMember]
        public decimal PtosGrEst { get; set; }
        [DataMember]
        public decimal PtosAprEst { get; set; }
        [DataMember]
        public decimal PtosExpEst { get; set; }
        [DataMember]
        public decimal PtosParEst { get; set; }
        [DataMember]
        public decimal PtosOtrosEst { get; set; }
        [DataMember]
        public decimal ExcParEst { get; set; }
        [DataMember]
        public decimal ExcAprEst { get; set; }
        [DataMember]
        public decimal CAprovechamiento { get; set; }
        [DataMember]
        public decimal CParticipacion { get; set; }
        [DataMember]
        public decimal CInstruccion { get; set; }
        [DataMember]
        public decimal ExpDoc { get; set; }
        [DataMember]
        public string Movimiento { get; set; }
        [DataMember]
        public string Observacion1 { get; set; }
        [DataMember]
        public string Observacion2 { get; set; }
        [DataMember]
        public string Observacion3 { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public decimal PtosPublicacion { get; set; }
        [DataMember]
        public decimal PtosExpDoc { get; set; }
        [DataMember]
        public decimal PtosOrgInt { get; set; }
        [DataMember]
        public DateTime RigeEst { get; set; }
        [DataMember]
        public string CalificacionEst { get; set; }
    }
}

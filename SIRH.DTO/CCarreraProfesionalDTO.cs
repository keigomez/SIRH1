using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCarreraProfesionalDTO : CBaseDTO
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int Puesto { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public int Clase { get; set; }
        [DataMember]
        public int Codigo { get; set; }
        [DataMember]
        public int Division { get; set; }
        [DataMember]
        public int Direccion { get; set; }
        [DataMember]
        public string Ubicacion { get; set; }
        [DataMember]
        public decimal TotalPuntos { get; set; }
        [DataMember]
        public int Departamento { get; set; }
        [DataMember]
        public decimal ValorPunto { get; set; }
        [DataMember]
        public int AnnoExperienciaPR { get; set; }
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
        public decimal PtosExperienciaPR { get; set; }
        [DataMember]
        public decimal PtosParticipacion { get; set; }
        [DataMember]
        public decimal PtosInstruccion { get; set; }
        [DataMember]
        public decimal PtosOtros { get; set; }
        [DataMember]
        public string ExplicacionOtros { get; set; }
        [DataMember]
        public string Observacion { get; set; }
        [DataMember]
        public DateTime PSS1Feri { get; set; }
        [DataMember]
        public DateTime PSS1Feve { get; set; }
        [DataMember]
        public DateTime PSS2Feri { get; set; }
        [DataMember]
        public DateTime PSS2Feve { get; set; }
        [DataMember]
        public DateTime PSS3Feri { get; set; }
        [DataMember]
        public DateTime PSS3Feve { get; set; }
        [DataMember]
        public int MesCarrera { get; set; }
        [DataMember]
        public int AnnoCarrera { get; set; }
        [DataMember]
        public DateTime FecRigePago { get; set; }
        [DataMember]
        public decimal HorasExcParticipacion { get; set; }
        [DataMember]
        public decimal HorasExcAprovechamiento { get; set; }
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
        public int AnnoExperienciaEst { get; set; }
        [DataMember]
        public int AprovechamientoEst { get; set; }
        [DataMember]
        public int ParticipacionEst { get; set; }
        [DataMember]
        public int ParticipacionInstruccionEst { get; set; }
        [DataMember]
        public decimal HorasAprovechamientoEst { get; set; }
        [DataMember]
        public decimal HorasParticipacionEst { get; set; }
        [DataMember]
        public decimal HorasInstruccionEst { get; set; }
        [DataMember]
        public decimal PtosGradoEst { get; set; }
        [DataMember]
        public decimal PtosAprovechamientoEst { get; set; }
        [DataMember]
        public decimal PtosExperienciaEst { get; set; }
        [DataMember]
        public decimal PtosParticipacionEst { get; set; }
        [DataMember]
        public decimal PtosInstruccionEst { get; set; }
        [DataMember]
        public decimal PtosOtroEst { get; set; }
        [DataMember]
        public decimal ExcParticipacionEst { get; set; }
        [DataMember]
        public decimal ExcAprovechamientoEst { get; set; }
        [DataMember]
        public string Marca { get; set; }
        [DataMember]
        public string Curso1 { get; set; }
        [DataMember]
        public string Curso2 { get; set; }
        [DataMember]
        public string Curso3 { get; set; }
        [DataMember]
        public string Curso4 { get; set; }
        [DataMember]
        public string Curso5 { get; set; }
        [DataMember]
        public string Curso6 { get; set; }
        [DataMember]
        public string Movimiento { get; set; }
        [DataMember]
        public string Curso7 { get; set; }
        [DataMember]
        public string Curso8 { get; set; }
        [DataMember]
        public string Observacion1 { get; set; }
        [DataMember]
        public string Observacion2 { get; set; }
        [DataMember]
        public string Observacion3 { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }

    }
}

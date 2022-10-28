using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CDetallePuntosDTO : CBaseDTO
    {
        [DataMember]
        public decimal TotalPuntos { get; set; }
        [DataMember]
        public decimal ValorPunto { get; set; }
        [DataMember]
        public decimal PuntosAdicionales { get; set; }

        //Experiencia Profesional
        [DataMember]
        public int AnnosExpProfesional { get; set; }
        [DataMember]
        public decimal PuntosExpProfesional { get; set; }
        
        //Aprovechamiento
        [DataMember]
        public decimal HorasAprovechamiento { get; set; }
        [DataMember]
        public decimal HorasExcAprovechamiento { get; set; }
        [DataMember]
        public decimal PuntosAprovechamiento { get; set; }
        [DataMember]
        public int CursosAprovechamiento { get; set; }

        //Participacion
        [DataMember]
        public decimal HorasParticipacion { get; set; }
        [DataMember]
        public decimal HorasExcParticipacion { get; set; }
        [DataMember]
        public decimal PuntosParticipacion { get; set; }
        [DataMember]
        public int CursosParticipacion { get; set; }

        //Instruccion
        [DataMember]
        public decimal HorasInstruccion { get; set; }
        [DataMember]
        public decimal PuntosInstruccion { get; set; }
        [DataMember]
        public int CursosInstruccion { get; set; }

        //Aprovechamiento Ley 9635
        [DataMember]
        public decimal HorasAprovechamientoLey { get; set; }
        [DataMember]
        public decimal HorasExcAprovechamientoLey { get; set; }
        [DataMember]
        public decimal PuntosAprovechamientoLey { get; set; }
        [DataMember]
        public int CursosAprovechamientoLey { get; set; }

        //Participacion Ley 9635
        [DataMember]
        public decimal HorasParticipacionLey { get; set; }
        [DataMember]
        public decimal HorasExcParticipacionLey { get; set; }
        [DataMember]
        public decimal PuntosParticipacionLey { get; set; }
        [DataMember]
        public int CursosParticipacionLey { get; set; }

        //Puntos Grado
        [DataMember]
        public string Grado { get; set; }
        [DataMember]
        public decimal PuntosGrado { get; set; }



    }
}

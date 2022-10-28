using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class NotificacionesCorreoVM
    {
        [DisplayName("Remitente")]
        public string CorreoUsuario { get; set; }
        [DisplayName("Correo Destinatario")]
        public string CorreoFuncionario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreFuncionario { get; set; }
        public string CodigoDesarraigo { get; set; }

        [DisplayName("Asunto del Correo")]
        public string Asunto { get; set; }
        [DisplayName("Mensaje")]
        public string MensajePrevio { get; set; }
        [DisplayName("Anotaciones")]
        [Required]
        public string Observaciones { get; set; }
        [DisplayName("Final del Correo")]
        public string PiePagina { get; set; }

        public string PiePagina2 { get; set; }

        public string MensajeCompleto { get { return "<br>"+this.MensajePrevio + "<br>" + this.Observaciones + "<br>" + this.PiePagina+ "<br>" + this.PiePagina2; } }
    }
}
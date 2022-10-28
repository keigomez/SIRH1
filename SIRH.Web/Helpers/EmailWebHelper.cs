using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace SIRH.Web.Helpers
{
    public class EmailWebHelper
    {
        public string EmailBody { get; set; }
        public string Asunto { get; set; }
        public string Destinos { get; set; }

        internal bool EnviarCorreo()
        {
            bool respuesta = false;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MailMessage mensaje = new MailMessage("notificaciones@mopt.go.cr", Destinos, Asunto, FormatearContenido(EmailBody));
                mensaje.IsBodyHtml = true;
                SmtpClient cliente = new SmtpClient("smtp.office365.com", 587);
                // SmtpClient cliente = new SmtpClient("smtp.sendgrid.net", 587);
                cliente.Credentials = new System.Net.NetworkCredential("notificaciones@mopt.go.cr", "Mopt2020**");
                //cliente.Credentials = new System.Net.NetworkCredential("apikey", "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");
                cliente.EnableSsl = true;
                cliente.Send(mensaje);
                cliente.Dispose();
                respuesta = true;
                return respuesta;
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        internal bool EnviarCorreoSendGrid(string to, string nombre, string cedula, string fecha, string hora, string primero, string segundo, string tipo)
        {
            bool respuesta = false;
            try
            {
                switch (tipo)
                {
                    case "1":
                        this.ContenidoSendGrid(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "NUEVA CONVOCATORIA A PRUEBA FÍSICA, CONCURSO EXTERNO N° 001-2019-RP POLICÍA DE TRÁNSITO 1";
                        break;
                    case "2":
                        this.ContenidoSendGridNoAptos(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "Resultado de Evaluaciones Físicas del Concurso Policía Tránsito 1, (001-2019-RP).";
                        break;
                    case "3":
                        this.ContenidoSendGridIncumplimientoRequisitos(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "Oferta no procesada Concurso Policía Tránsito 1, (001-2019-RP); Incumplimiento de Requisitos.";
                        break;
                    case "4":
                        this.ContenidoSendGridLicencia(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "Oferta no procesada Concurso Policía Tránsito 1, (001-2019-RP); Incumplimiento de Requisitos.";
                        break;
                    case "5":
                        this.ContenidoSendGridCosevi(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "Oferta no procesada Concurso Policía Tránsito 1, (001-2019-RP); Incumplimiento de Requisitos.";
                        break;
                    case "6":
                        this.ContenidoSendGridPruebaPsicologica(nombre, cedula, fecha, hora, primero, segundo);
                        this.Asunto = "CONVOCATORIA A PRUEBAS ESCRITAS DE IDONEIDAD PSICOLOGICA, CONCURSO EXTERNO N° 001-2019-RP, POLICÍA DE TRÁNSITO.";
                        break;
                    default:
                        break;
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MailMessage mensaje = new MailMessage(/*"reclutamiento.policia@mopt.go.cr"*/"reclutamiento.policia@mopt.go.cr", to, this.Asunto,
                    FormatearContenido(EmailBody));
                mensaje.IsBodyHtml = true;
                //SmtpClient cliente = new SmtpClient("smtp.office365.com", 587);
                SmtpClient cliente = new SmtpClient("smtp.office365.com", 587);
                //cliente.Credentials = new System.Net.NetworkCredential("notificaciones@mopt.go.cr", "Mopt2020**");
                cliente.Credentials = new System.Net.NetworkCredential("reclutamiento.policia@mopt.go.cr", "Mopt20*Pol21*");
                cliente.EnableSsl = true;
                cliente.Send(mensaje);
                cliente.Dispose();
                respuesta = true;
                return respuesta;
            }
            catch (Exception e)
            {
                return respuesta;
            }
        }

        private string FormatearContenido(string contenido)
        {
            contenido = contenido
                               .Replace(Environment.NewLine, "<br />")
                               .Replace("\n", "<br />")
                               .Replace("\r", "<br />");
            return contenido;
        }

        private void ContenidoSendGrid(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>En virtud de su participación al Concurso Externo N°. 001-2019-RP, para conformar Registro de Elegibles para la Clase: Policía de Tránsito 1, se le convoca a realizar pruebas físicas:</p>";
            this.EmailBody += "<p>Fecha de la cita: <b>" + fecha + "</b></p>";
            this.EmailBody += "<p>Hora: <b>" + hora + "</b></p>";
            this.EmailBody += "Lugar de cita: <b>Escuela de Capacitación de la Policía de Tránsito</b>, 500 metros sur del Centro Comercial Expresso, frente a la entrada del barrio Las Lomas, San Rafael Arriba de Desamparados, Desamparados, San José, Costa Rica.";
            this.EmailBody += "<p>Se le solicita, presentarse al menos con 15 minutos de anticipación.</p>";
            this.EmailBody += "<ul>";
            this.EmailBody += "<li>	Es obligatorio presentar los siguientes documentos:</li>";
            this.EmailBody += "<li>	Cédula de identidad (vigente y en buen estado)</li>";
            this.EmailBody += "<li>	Licencia de Conducir (vigente, con la que participaron).</li>";
            this.EmailBody += "<li>	<b>DE PREFERENCIA: Certificado Médico y/o Epicrisis (historial médico) emitido por la CCSS</b>, <u>ya sea EBAIS, Clínicas u Hospitales; el mismo con una fecha no mayor de 6 meses de emitido</u>, (NO se debe confundir con el Dictamen Digital para obtener una Licencia de Conducir, ya que este documento NO será recibido), ó bien presentar Certificado Médico privado emitido por el Colegio de Médicos bajo el sistema digital SEDIMEC; <u>debe de traerlo impreso </u> sin excepción; los mismos con una fecha no mayor de 3 meses de emitido.</li>";
            this.EmailBody += "</ul>";
            this.EmailBody += "<p>En caso de no ser posible obtener el certificado, deberá firmar un <b>Consentimiento Informado</b> en el cual usted asume bajo su responsabilidad, la aplicación de la prueba física.</p>";
            this.EmailBody += "<p>Como es de conocimiento, el país se vio afectado por la pandemia del COVID-19, la cual llego a suspender la convocatoria inicial; es por ello que, a fin de retomar el Concurso, se tomaron las medidas necesarias para su continuación, de acuerdo a Protocolo avalado por el Ministerio de Salud, según Informe Técnico de Inspección MS-DRRSCS-ARS-D-ERS-IT-0625-2020. </p>";
            this.EmailBody += "<b>Por ello, tenga presente que:</b>";
            this.EmailBody += "<ul>";
            this.EmailBody += "<li>	Es obligatorio portar mascarilla. Además, es importante complementar con otros medios de protección personal durante su estancia en la Escuela (alcohol en gel, careta, jabón líquido)</li>";
            this.EmailBody += "<li>	Es necesario llevar calzado y ropa deportiva (preferiblemente camiseta blanca manga corta o larga, pantaloneta o licra y medias); <b>debe venir listo con la vestimenta completa.</b> </li>";
            this.EmailBody += "<li>	Llevar dos paños pequeños para uso personal (uno para el sudor y otro para limpiar), una bolsa negra y un candado pequeño con llave, para guardar cualquier artículo personal 	en el locker que	 se le va a asignar (en caso que necesite guardar las pertenencias).</li>";
            this.EmailBody += "<li>	Se le recomienda portar agua o algún tipo de bebida hidratante o suero <b>de uso solo personal</b>; asimismo, desayunado con el tiempo y forma, previo a realizar un deporte de intensidad. </li>";
            this.EmailBody += "<li>	Se procederá a tomar la temperatura corporal al ingreso de la institución. Cualquier persona que presente temperatura superior a 37.8 grados, y/o sintomatología sugestiva de un cuadro respiratorio, sea esto fiebre, dolor de garganta, dolor de cuerpo, diarrea, mocos, tos, problemas de olfato y gusto; no se le permitirá permanecer en las instalaciones, y se le instará a que se traslade a un centro médico a un chequeo de salud.</li>";
            this.EmailBody += "<li>	Sin excepción, todas las personas, al ingresar a las instalaciones, deberán lavar sus manos con agua y jabón en los lavatorios destinados para ello. </li>";
            this.EmailBody += "<li>	En todo momento se debe mantener una distancia de al menos 2 metros con las demás personas. No se puede saludar de beso, mano o cualquier otra forma que implique acercamiento menor a 2 metros.</li>";
            this.EmailBody += "<li>	Se debe seguir los protocolos de la forma de controlar al toser y/o estornudar indicados por el Ministerio de Salud. También evite resoplar o utilizar cualquier técnica de expiración forzada que provoque que la saliva pueda pasar al medio ambiente; se le indicará la técnica correcta de respiración mediante inhalación por la nariz y exhalación por la boca.</li>";
            this.EmailBody += "</ul>";
            this.EmailBody += "<u>Evaluación Física</u>";
            this.EmailBody += "<p>La prueba estará compuesta por 5 ejercicios a realizar, según tarjetas instructivas, en un tiempo específico que se detalla a continuación:</p>";
            this.EmailBody += "<ol>";
            this.EmailBody += "<li>	Test de Course Navette (Ir y venir) 1 minuto.</li>";
            this.EmailBody += "<li>	Push up (pechadas) durante 1 minuto.</li>";
            this.EmailBody += "<li>	Jumping Jacks 1 minuto.</li>";
            this.EmailBody += "<li>	Abdominales 1 minuto</li>";
            this.EmailBody += "<li>	Dominadas en barra 1 minuto.</li>";
            this.EmailBody += "</ol>";
            this.EmailBody += "Se recomienda hacer un estiramiento muscular antes y después de la prueba (5 minutos aproximadamente) ";
            this.EmailBody += "<p>Consecuentemente, se le informa que debe ser consciente, que la prueba es de alto impacto y usted se someterá a ello bajo su responsabilidad. </p>";
            this.EmailBody += "<p><b> POR PROTOCOLO NO SE LE PERMITIRÁ REALIZAR PRUEBAS A QUIEN LLEGUE A DESTIEMPO (TARDE) O INCUMPLA LAS OBLIGACIONES INDICADAS</b> (documentos e implementos deportivos y sanitarios).</p>";
            this.EmailBody += "<p><b> NO SE PERMITIRÁ EL INGRESO DE ACOMPAÑANTES, NI SE CUENTA CON AREA DE PARQUEO PARA AUTOMOVILES U MOTOS.  </b> </p>";
            this.EmailBody += "<p><b> NO SE LE PERMITIRA EL INGRESO A QUIEN SE PRESENTE BAJO LOS EFECTOS DEL ALCHOOL U OTRAS DROGAS.</b> </p>";
            this.EmailBody += "<p><b> TAMPOCO SE REPROGRAMARÁN CITAS PARA OTRA FECHA A LA INDICADA, POR LO CUAL SE LE PIDE PUNTUALIDAD</b>  (salvo justificaciones debidamente documentadas).</p>";
            this.EmailBody += "<p><b> DE NO REALIZAR LAS PRUEBAS EN LA FECHA SEÑALADA, QUEDARÁ EXCLUIDO (A) DEL CONCURSO EN MENCIÓN.</b>  </p>";
            //this.EmailBody += "<p><b> Nota:</b>  En el siguiente link podrá conocer todos los Protocolos del Ministerio de Salud, para tomar en cuenta.</p>";
            //this.EmailBody += "<p></p>";
            //this.EmailBody += "<a href=\"https://www.ministeriodesalud.go.cr/index.php/centro-de-informacion/material-comunicacion/protocolos-ms\"> https://www.ministeriodesalud.go.cr/index.php/centro-de-informacion/material-comunicacion/protocolos-ms </a>";
        }

        private void ContenidoSendGridNoAptos(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>Se le comunica que su persona <b>no aprobó</b> satisfactoriamente las Evaluaciones Físicas del Concurso 001-2019-RP, para la clase de puesto Policía de Tránsito 1</p>";
            this.EmailBody += "<p>Por lo anterior, de acuerdo con el Art. 37 del Reglamento de Organización y Servicio de la Policía de Tránsito, su persona no continua en el proceso.</p>";
            this.EmailBody += "<p>Le agradecemos su interés.</p>";
        }

        private void ContenidoSendGridIncumplimientoRequisitos(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>Se le comunica que su oferta correspondiente al Concurso 001-2019-RP, para la clase de puesto Policía de Tránsito 1, no ha sido tramitada, en razón de que su persona no completó los datos solicitados satisfactoriamente.</p>";
            this.EmailBody += "<p>Lo anterior, según el instructivo de la oferta digital y el afiche del Concurso.</p>";
            this.EmailBody += "<p>Le agradecemos su interés.</p>";
        }

        private void ContenidoSendGridCosevi(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>Se le comunica que su oferta correspondiente al Concurso 001-2019-RP, para la clase de puesto Policía de Tránsito 1, fue rechazada, por cuanto los datos consignados en el documento, tales como: número de cédula de identidad y licencia de conducir, no cuenta con registro en el sistema de licencias de conducir del Consejo de Seguridad Vial (COSEVI), por lo que se hace imposible, realizar un proceso de reclutamiento.</p>";
            this.EmailBody += "<p>Le agradecemos su interés.</p>";
        }

        private void ContenidoSendGridLicencia(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>Se le comunica que su oferta correspondiente al Concurso 001-2019-RP, para la clase de puesto Policía de Tránsito 1, fue rechazada.</p>";
            this.EmailBody += "<p>Lo anterior, por cuanto en el Sistema de licencias de conducir, del Consejo de Seguridad Vial (COSEVI), no registra a su nombre licencias de conducir conforme al requisito exigido o las mismas se encuentran en condición de vencida, <b><u>al momento de la inscripción</u></b>.</p>";
            this.EmailBody += "<p>Se le recuerda que, tal y como se indicó en el instructivo de la oferta: </p>";
            this.EmailBody += "<p><i>“…Los datos que consigne deberán ser veraces, ya que el registrar datos incorrectos, falsos u omitir información en el proceso de inscripción, automáticamente anula la Oferta de Servicio que de dicho registro se genere y en consecuencia no será considerado durante el proceso”.</i></p>";
            this.EmailBody += "<p>Le agradecemos su interés.</p>";
        }

        private void ContenidoSendGridPruebaPsicologica(string nombre, string cedula, string fecha, string hora, string primer, string segundo)
        {
            this.EmailBody = "<img src=\"https://i.imgur.com/JrBzKfO.png\">";
            this.EmailBody += "<p>Sr.(a) <b>" + nombre + " " + primer + " " + segundo + "</b></p>";
            this.EmailBody += "<p>Cédula: <b>" + cedula + "</b></p>";
            this.EmailBody += "<p>Estimado Señor (a)</p>";
            this.EmailBody += "<p>En virtud de su participación al Concurso Externo N°. 001-2019-RP, para conformar Registro de Elegibles para la Clase: Policía de Tránsito 1, <b>y por haber aprobado satisfactoriamente las evaluaciones físicas;</b> se le convoca a realizar pruebas escritas correspondientes a la comprobación de idoneidad psicológica, que responde al perfil del puesto:</p>";
            this.EmailBody += "<p>Fecha de la cita: <b>" + fecha + "</b></p>";
            this.EmailBody += "<p>Hora: <b>" + hora + "</b></p>";
            this.EmailBody += "<p>Lugar de cita:  Auditorio de la Dirección General de la Policía de Tránsito, ubicado 200 metros oeste del Plantel Central del MOPT, diagonal a la terminal de buses de TRACOPA. Se le solicita, presentarse al menos con 15 minutos de anticipación.</p>";
            this.EmailBody += "<p><b>Es obligatorio portar:</b></p>";
            this.EmailBody += "<ul>";
            this.EmailBody += "<li>Cédula de identidad (vigente y en buen estado)</li>";
            this.EmailBody += "<li>Lapiceros, lápiz, borrador y lentes (en caso de necesitarlos).</li>";
            this.EmailBody += "</ul>";
            this.EmailBody += "<p><b>Tenga presente que:</b></p>";
            this.EmailBody += "<ul>";
            this.EmailBody += "<li>De acuerdo a decreto emitido por el Gobierno de la Republica el pasado 11 de mayo, el uso de la mascarilla será voluntario. Sin embargo, lo invitamos durante su estancia en la Institución, a seguir utilizando el cubre bocas para seguir cuidándose y adoptando las medidas sanitarias necesarias para proteger su salud y la de sus seres queridos.</li>";
            this.EmailBody += "<li>Se procederá a tomar la temperatura corporal al ingreso de la institución. Cualquier persona que presente temperatura superior a 37.8 grados, y/o sintomatología sugestiva de un cuadro respiratorio, sea esto fiebre, dolor de garganta, dolor de cuerpo, diarrea, mocos, tos, problemas de olfato y gusto; no se le permitirá permanecer en las instalaciones. <b>En este tipo de casos, se le reprogramará la prueba.</b></li>";
            this.EmailBody += "<li>Las personas, al ingresar a las instalaciones, deberán lavar sus manos con agua y jabón en los lavatorios destinados para ello.</li>";
            this.EmailBody += "<li>Mantener la distancia con las demás personas.</li>";
            this.EmailBody += "<li>Se debe seguir los protocolos de la forma de controlar al toser y/o estornudar indicados por el Ministerio de Salud.</li>";
            this.EmailBody += "</ul>";
            this.EmailBody += "<p><b>Además:</b></p>";
            this.EmailBody += "<ul>";
            this.EmailBody += "<li>Se le solicita llegar en buen estado de salud, descansado y alimentado.</li>";
            this.EmailBody += "<li>Dentro del auditorio no se aceptarán teléfonos celulares encendidos.</li>";
            this.EmailBody += "<li>Por protocolo no se le permitirá realizar pruebas a quien llegue a destiempo (tarde) o incumpla las obligaciones indicadas. <b>Iniciada la instrucción referente a las pruebas no se va a permitir el ingreso a más oferentes.</b></li>";
            this.EmailBody += "<li>No se permitirá el ingreso de acompañantes</li>";
            this.EmailBody += "<li>No se cuenta con área de parqueo para automóviles u motos.</li>";
            this.EmailBody += "<li>No se le permitirá el ingreso a quien se presente bajo los efectos del alcohol u otras drogas.</li>";
            this.EmailBody += "<li>Tampoco se reprogramarán citas para otra fecha a la indicada, por lo cual se le pide puntualidad (salvo justificaciones debidamente documentadas).</li>";
            this.EmailBody += "<li>De no realizar las pruebas en la fecha señalada, quedará excluido (a) del concurso en mención.</li>";
            this.EmailBody += "</ul>";
            this.EmailBody += "<p><b><u>Evaluacion</u></b></p>";
            this.EmailBody += "<p>La evaluación de Idoneidad Psicológica contemplada en el reclutamiento del Concurso Externo 01-2019- RP, incluye 3 tipo de instrumentos a utilizar (dos pruebas psicométricas y una entrevista). <b>En esta convocatoria <u>solo se le aplicaran las dos pruebas escritas.</u></b></p>";
            this.EmailBody += "<p>Esta evaluación para el reclutamiento de Policía de Tránsito 1, <b>no es evaluación clínica</b> es de idoneidad, la cual busca verificar que la persona reúna las condiciones psicológicas lo más cercanas al perfil, a fin de que realice un desempeño exitoso del cargo. </p>";
            this.EmailBody += "<p>El Artículo 21, del El Reglamento de Organización y Servicio de la Policía de Tránsito señala que, el perfil responde a: <i>“…el objetivo de verificar que la persona reúna las condiciones físicas, morales, psicológicas y cualquier otra condición para el desempeño exitoso del cargo”</i>.</p>";
            this.EmailBody += "<p>Es importante mencionar que, si en su oferta de servicios indicó que requiere o tiene <b>adecuación curricular</b>. Por favor, enviar el detalle de la referencia con certificación, para adjuntarla a su oferta de servicios lo antes posible.</p>";
            this.EmailBody += "<p>Cordialmente</p>";
        }

        internal string EnviarMensajeCorreo()
        {
            try
            {
                MailMessage mensaje = new MailMessage("SIRH@mopt.go.cr", Destinos, Asunto, EmailBody);
                mensaje.IsBodyHtml = true;
                SmtpClient cliente = new SmtpClient("correo.mopt.go.cr", 25);
                cliente.Send(mensaje);
                return "Enviado";
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
        }
    }
}

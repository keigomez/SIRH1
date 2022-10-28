using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;
using SIRH.DeamonServicios.CaucionProduccion;
using SIRH.DTO;
//using SIRH.DeamonServicios.DesarraigoService;

namespace SIRH.DeamonServicios
{
    public partial class ServiceSIRH : ServiceBase
    {
        private Timer Schedular;
        

        public ServiceSIRH()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("SIRHSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("SIRHSource", "SIRHLog");
            }
            eventLogSIRH.Source = "SIRHSource";
            eventLogSIRH.Log = "SIRHLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLogSIRH.WriteEntry("Servicio iniciado");
            this.ServicioSIRH();
        }

        protected override void OnStop()
        {
            eventLogSIRH.WriteEntry("Servicio detenido");
            Schedular.Dispose();
        }


        public void ServicioSIRH()
        {
            CCaucionWinService caucionWS = new CCaucionWinService();
            CCaucionesServiceClient caucion = new CCaucionesServiceClient();
            this.EscribirEntrada("El estado del servicio es: " + caucion.State.ToString());
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                this.EscribirEntrada("Inicia el timer");

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;
                this.EscribirEntrada("Determina el tiempo por defecto");

                //Get the Scheduled Time from AppSettings.
                scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                this.EscribirEntrada("Obtiene la hora de ejecución");

                if (DateTime.Now > scheduledTime)
                {
                    this.EscribirEntrada("La hora actual es la hora de ejecución");
                    //If Scheduled Time is passed set Schedule for the next day.
                    scheduledTime = scheduledTime.AddDays(1);
                    this.EscribirEntrada("Determina la ejecución del servicio para el día siguiente");
                }
                this.EscribirEntrada("No entró al IF");
                //ejecuta los servicios SIRH
                caucionWS.EjecutarServicioVencimientoPolizas(caucion, this);
                this.EscribirEntrada("Ejecuta el primer servicio");
                caucionWS.EjecutarServicioPolizasPorVencer(caucion, this);
                this.EscribirEntrada("Ejecuta el segundo servicio");
                //Desarraigo
                Desarraigo();

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                this.EscribirEntrada("Obtiene la fecha de la próxima ejecución");
                string schedule = string.Format("{0} día(s) {1} hora(s) {2} minuto(s) {3} segundo(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.EscribirEntrada("El servicio se volverá a ejecutar en: " + schedule);

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                this.EscribirEntrada("Determina el tiempo de espera");

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
                this.EscribirEntrada("Almacena el tiempo de espera");
            }
            catch (Exception ex)
            {
                EscribirEntrada("Error al ejecutar el servicio: " + ex.Message + ex.StackTrace + ex);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("ServiceSIRH"))
                {
                    serviceController.Stop();
                }
            }
        }

        private void SchedularCallback(object e)
        {
            eventLogSIRH.WriteEntry("El servicio se ha almacenado para volver a ejecutarse.");
            this.ServicioSIRH();
        }

        internal void EscribirEntrada(string text)
        {
            eventLogSIRH.WriteEntry(text);
        }
        private void Desarraigo()
        {
            //CDesarraigoWinService desarraigoWS = new CDesarraigoWinService();
            //CDesarraigoServiceClient desarraigo = new CDesarraigoServiceClient();
            //this.EscribirEntrada("El estado del servicio (desarraigo) es: " + desarraigo.State.ToString());
            //try
            //{

            //    desarraigoWS.EjecutarServicioDesarraigosPorVencer(desarraigo, this);
            //    this.EscribirEntrada("Ejecuta el primer servicio(desarraigo)");
            //    desarraigoWS.EjecutarServicioVencimientoDesarraigo(desarraigo, this);
            //    this.EscribirEntrada("Ejecuta el segundo servicio(desarraigo)");
            //}
            //catch (Exception ex)
            //{
            //    EscribirEntrada("Error al ejecutar el servicio desarraigo: " + ex.Message + ex.StackTrace + ex);

            //    //Stop the Windows Service.
            //    using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("ServiceSIRH"))
            //    {
            //        serviceController.Stop();
            //    }
            //}
        }
    }
}

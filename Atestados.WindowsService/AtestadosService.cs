using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Atestados.Utilitarios.Mensajes;

namespace Atestados.WindowsService
{
    partial class AtestadosService : ServiceBase
    {
        private System.Timers.Timer timer1 = null;
        public AtestadosService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            timer1.Enabled = true;
            this.timer1.Interval = double.Parse(ConfigurationManager.AppSettings["TiempoEsperaWinService"]);
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
        }

        protected override void OnStop()
        {

            timer1 = new System.Timers.Timer();
            timer1.Enabled = true;
            this.timer1.Interval = double.Parse(ConfigurationManager.AppSettings["TiempoEsperaWinService"]);
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);

        }

        #region Métodos


        //Al generar cada tick de tiempo
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
               //escribre en log
                WriteToFile("Service is started at " + DateTime.Now);
              
                //llama al método que desea invocar
                this.Metodo();
               

            }
            catch (Exception ex)
            {
                WriteToFile("Service intend to start at " + DateTime.Now + ". Exception= " + ex.Message);
            }
        }

        ////Este se utiliza para probar el servicio de forma local
        public void timer1_Tick_prueba()
        {
            try
            {
                //escribre en log
                WriteToFile("Service is started at " + DateTime.Now);

                //llama al método que desea invocar
                this.Metodo();

            }
            catch (Exception e)
            {
                WriteToFile("Service intend to start at " + DateTime.Now + ". Exception= "+ e.Message);
            }
        }



        private void Metodo()
        {
           
            try
            {

                //funciones
                //bitácora
                WriteToFile("Service success - (" + DateTime.Now + ")");

            }
            catch(Exception e)
            {
               //bitácora
                WriteToFile("Service error - (" + DateTime.Now + "). Exception= " + e.Message);
            }

            
        }

        //función para escribir en el log del servidor y verificar si el servicio está ejecutandose correctamente
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }



        }

        #endregion

      
    }
}

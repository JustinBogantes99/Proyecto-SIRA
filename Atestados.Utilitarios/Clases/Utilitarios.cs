using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Atestados.Utilitarios.Clases
{
    public static class Utilitarios
    {
        /// <summary>
        /// Método que obtiene el nombre único del servidor donde se está ejecutando la aplicación
        /// </summary>
        /// <returns>Dirección IP del servidor de la aplicación</returns>
        public static string ObtenerDireccionIP()
        {
            // Almacena el nombre del equipo
            string nombreEquipo = Dns.GetHostName();

            // Almacena la información del equipo en la red
            var ipEquipo = Dns.GetHostEntry(nombreEquipo);

            // Recorre la información del equipo en busca de la dirección IP
            foreach (var direccionIP in ipEquipo.AddressList.Where(direccionIP => direccionIP.AddressFamily.ToString() == "InterNetwork"))
                return direccionIP.ToString();

            // Retorna un valor por defecto
            return "000.000.000.000";
        }


        public static string GetIpAddress()
        {
            string ipAddressString = HttpContext.Current.Request.UserHostAddress;

            if (ipAddressString == null)
                return null;

            IPAddress ipAddress;
            IPAddress.TryParse(ipAddressString, out ipAddress);

            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                ipAddress = System.Net.Dns.GetHostEntry(ipAddress).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            return ipAddress.ToString();
        }

        /// <summary>
        /// Método que transforma un objeto a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aSerializar"></param>
        /// <returns>objeto serializado en un string</returns>
        public static string SerializarObjeto<T>(this T aSerializar)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(aSerializar.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, aSerializar);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Valida si la contraseña solo posee los caracteres permitidos
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static bool ValidarCaracteresEspeciales(string texto)
        {
            //Regex regex = new Regex(@"|");
            //return regex.IsMatch(texto);
            return false;
        }

        /// <summary>
        /// Valida el formato del correo electronico 
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                return new EmailAddressAttribute().IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        public static string ExtensionCorreoElectronico(string correo)
        {
            string email = null;
            try
            {
                MailAddress eMail = new MailAddress(correo);
                email = eMail.Host;
            }
            catch
            {

            }

            return email;

        }


        public static string ObtenerUsuarioCorreo(string correo)
        {
            string email = null;
            try
            {
                MailAddress eMail = new MailAddress(correo);
                email = eMail.User;
            }
            catch
            {
                throw;
            }

            return email;

        }

        public static int AmPmTo24(int hour, string ampm)
        {
            int horaFix = 0;

            if (ampm.Equals("Pm"))
            {
                switch (hour)
                {
                    case 1:
                        horaFix = 13; break;
                    case 2:
                        horaFix = 14; break;
                    case 3:
                        horaFix = 15; break;
                    case 4:
                        horaFix = 16; break;
                    case 5:
                        horaFix = 17; break;
                    case 6:
                        horaFix = 18; break;
                    case 7:
                        horaFix = 19; break;
                    case 8:
                        horaFix = 20; break;
                    case 9:
                        horaFix = 21; break;
                    case 10:
                        horaFix = 22; break;
                    case 11:
                        horaFix = 23; break;
                    case 12:
                        horaFix = 12; break;
                }
            }
            else
            {
                if (hour == 12)
                {
                    horaFix = 00;
                }
                else {
                    horaFix = hour;
                }         
            }

            return horaFix;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Atestados.Utilitarios.Variables
{
    public static class Variables
    {
        public static string BaseUrl { get; set; }

        static Variables()
        {
            if (HttpContext.Current.Request.Url.Host.Equals("localhost"))
            {
                BaseUrl = "/Atestados";
            }
        }


    }
}

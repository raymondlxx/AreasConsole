using AreasConsole.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Services
{
    public abstract class AbstractTranslate : ITranslate
    {
        public virtual string Url { get; set; }

        public abstract string translate(string words);

        public string getResponseString(HttpWebResponse response)
        {
            try
            {
                string json = null;
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    json = reader.ReadToEnd();
                }
                return json;
            }
            catch (Exception ex)
            {
                ConsoleLogger.ErrorPrint("FailedToInvoke ciba api" + ex.Message);
                return string.Empty;
            }
        }

        public virtual string GetURL(string keyword)
        {
            return "http://fy.iciba.com/ajax.php?a=fy&f=en&t=zh&w=" + keyword;
        }
    }
}

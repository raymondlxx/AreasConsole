using AreasConsole.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Services
{
    public class CiBaTranslate : ITranslate
    {
        public string translate(string words)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetURL(words));
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();

                var responseString = getResponseString(response);
                if (responseString.IndexOf("out") > 0)
                {
                    var translation = JsonConvert.DeserializeObject<TranslateResponse>();
                    
                    return JsonConvert.SerializeObject(translation?.content?.word_mean);
                }
              
         
            }
            catch (WebException xe)
            {
                response = (HttpWebResponse)xe.Response;
                throw xe;
            }
        }

        private string getResponseString(HttpWebResponse response)
        {
            string json = null;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }

        public string GetURL(string keyword)
        {
            return "http://fy.iciba.com/ajax.php?a=fy&f=en&t=zh&w="+keyword;
        }
    }
}

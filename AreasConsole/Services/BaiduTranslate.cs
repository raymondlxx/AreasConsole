using AreasConsole.Extensions;
using AreasConsole.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Services
{
    public class BaiduTranslate : AbstractTranslate, ITranslate
    {
        public override string translate(string words)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetURL(words));
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                response = (HttpWebResponse)request.GetResponse();

                var responseString = getResponseString(response);

                if (string.IsNullOrEmpty(responseString))
                {
                    return string.Empty;
                }

                var translation = JsonConvert.DeserializeObject<BaiduTranslateResponse>(responseString);

                return translation.trans_result.data.FirstOrDefault().dst;
            }
            catch (WebException xe)
            {
                response = (HttpWebResponse)xe.Response;
                ConsoleLogger.ErrorPrint(xe.Message);
                return string.Empty;
            }
        }

        public override string Url
        {
            get
            {
                return "http://fanyi.baidu.com/v2transapi?from=en&to=zh&transtype=translang&simple_means_flag=3&query=";
            }
        }
        public override string GetURL(string keyword)
        {
            return this.Url + keyword;
        }
    }
}

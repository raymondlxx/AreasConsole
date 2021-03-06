﻿using AreasConsole.Extensions;
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
    public class CiBaTranslate : AbstractTranslate, ITranslate
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

                var translation = JsonConvert.DeserializeObject<TranslateResponse>(responseString);

                if (!String.IsNullOrEmpty(translation.content.s_out))
                {
                    return JsonConvert.SerializeObject(translation?.content?.s_out);
                }

                return JsonConvert.SerializeObject(translation?.content?.word_mean);

            }
            catch (WebException xe)
            {
                response = (HttpWebResponse)xe.Response;
                ConsoleLogger.ErrorPrint(xe.Message);
                return string.Empty;
            }
        }


        public override string GetURL(string keyword)
        {
            return "http://fy.iciba.com/ajax.php?a=fy&f=en&t=zh&w=" + keyword;
        }
    }
}

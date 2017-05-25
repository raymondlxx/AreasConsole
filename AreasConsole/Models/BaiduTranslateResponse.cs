using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Models
{
    public class BaiduTranslateResponse
    {
        public BaiduTransResult trans_result{ get; set; }
    }
    public class BaiduTransResult
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("domain")]
        public string domain { get; set; }

        [JsonProperty("data")]
        public List<BaiduTransResultData> data { get; set; }
    }

    public class BaiduTransResultData
    {
        [JsonProperty("dst")]
        public string dst { get; set; }

    }
}

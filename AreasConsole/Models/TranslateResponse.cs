using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreasConsole.Models
{
    [Serializable]
    public class TranslateResponse
    {
        public TranslateContent content { get; set; }
        public int status { get; set; }

    }

    [Serializable]
    public class TranslateContent
    {
        [JsonProperty("ph_am")]
        public string ph_am { get; set; }

        [JsonProperty("ph_am_mp3")]
        public string ph_am_mp3 { get; set; }

        [JsonProperty("ph_en")]
        public string ph_en { get; set; }

        [JsonProperty("ph_en_mp3")]
        public string ph_en_mp3 { get; set; }

        [JsonProperty("ph_tts_mp3")]
        public string ph_tts_mp3 { get; set; }

        [JsonProperty("word_mean")]
        public List<string> word_mean { get; set; }

        [JsonProperty("out")]
        public string s_out { get; set; }
    }
}
